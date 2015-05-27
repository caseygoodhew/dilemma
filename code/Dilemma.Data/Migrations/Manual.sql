USE dilemma
GO

CREATE UNIQUE INDEX IX_PointType ON PointConfiguration (PointType);
GO

CREATE UNIQUE INDEX IX_ServerName ON ServerConfiguration (Name);
GO

CREATE UNIQUE INDEX IX_UniqueVote ON Vote (User_UserId, Question_QuestionId);
GO

CREATE UNIQUE INDEX IX_LogLevel ON EmailLogLevel (LogLevel);
GO







SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF OBJECT_ID ( 'TimeSourceNow', 'FN' ) IS NOT NULL 
    DROP FUNCTION TimeSourceNow;
GO

CREATE FUNCTION TimeSourceNow
	(@DateTimeNow datetime2 = null)
RETURNS datetime2
WITH EXECUTE AS CALLER
AS
BEGIN
	IF @DateTimeNow IS NULL OR NOT EXISTS (SELECT * FROM dbo.SystemConfiguration WHERE SystemEnvironment = 1) -- check for testing environment
	BEGIN
		SET @DateTimeNow = GETUTCDATE()
	END

	RETURN (@DateTimeNow)
END
GO







IF OBJECT_ID ( 'FlushRetirementTables', 'P' ) IS NOT NULL 
    DROP PROCEDURE FlushRetirementTables;
GO

CREATE PROCEDURE FlushRetirementTables
	@ForceFlush bit = null 
AS
BEGIN
	-- check for development or testing environment
	IF @ForceFlush = 1 OR NOT EXISTS (SELECT * FROM dbo.SystemConfiguration WHERE SystemEnvironment in (0, 1)) 
	BEGIN
		DELETE FROM ModerationRetirement
		DELETE FROM UserPointRetirement
		DELETE FROM QuestionRetirement
		DELETE FROM VoteCountRetirement
	END
END
GO







SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF OBJECT_ID ( 'ExpireAnswerSlots', 'P' ) IS NOT NULL 
    DROP PROCEDURE ExpireAnswerSlots;
GO

CREATE PROCEDURE ExpireAnswerSlots
	@DateTimeNow datetime2 = null
AS
	SET @DateTimeNow = dbo.TimeSourceNow(@DateTimeNow)
	
	UPDATE Answer
	   SET AnswerState = 4
	 WHERE AnswerId IN (	
		SELECT AnswerId
		  FROM Answer a, SystemConfiguration sc
		 WHERE a.AnswerState = 0 
		   AND DATEADD("mi", sc.ExpireAnswerSlotsAfterMinutes, a.LastTouchedDateTime) < @DateTimeNow)

	UPDATE LastRunLog SET ExpireAnswerSlots = @DateTimeNow;
GO








SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF OBJECT_ID ( 'CloseQuestions', 'P' ) IS NOT NULL 
    DROP PROCEDURE CloseQuestions;
GO

CREATE PROCEDURE CloseQuestions
	@DateTimeNow datetime2 = null
AS
	SET @DateTimeNow = dbo.TimeSourceNow(@DateTimeNow)

    UPDATE Question
       SET ClosedDateTime = @DateTimeNow
     WHERE QuestionId IN (
        SELECT questionId
		  FROM Question
		 WHERE QuestionState = 1 -- Approved Question
		   AND ClosedDateTime IS NULL
		   AND ClosesDateTime < @DateTimeNow
		 UNION 
		SELECT q.QuestionId
		  FROM Question q, (
			SELECT Question_QuestionId QuestionId, COUNT(*) AnswerCount
			  FROM Answer
			 WHERE AnswerState = 2 -- Approved Answer
			 GROUP BY Question_QuestionId
			) a
		 WHERE q.QuestionId = a.QuestionId
		   AND q.QuestionState = 1 -- Approved Question
		   AND q.ClosedDateTime IS NULL
		   AND (q.MaxAnswers = a.AnswerCount)
	)

	UPDATE LastRunLog SET CloseQuestions = @DateTimeNow;
GO






SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF OBJECT_ID ( 'UserStatistics', 'P' ) IS NOT NULL 
    DROP PROCEDURE UserStatistics;
GO

CREATE PROCEDURE UserStatistics
	@userId int
AS
	SELECT u.UserId, 
		   u.HistoricQuestions + ISNULL(q.QuestionCount, 0) TotalQuestions, 
		   u.HistoricAnswers + ISNULL(a.AnswerCount, 0) TotalAnswers,
		   u.HistoricPoints + ISNULL(p.PointsAwarded, 0) TotalPoints,
		   u.HistoricStarVotes + ISNULL(s.StarVoteCount, 0) TotalStarVotes,
		   u.HistoricPopularVotes TotalPopularVotes
	  FROM [User] u
	  LEFT JOIN (
			SELECT User_UserId UserId, COUNT(*) QuestionCount
			  FROM Question
			 WHERE QuestionState = 1 -- approved
			   AND User_UserId = @userId
			 GROUP BY User_UserId) q
				ON u.UserId = q.UserId
	  LEFT JOIN (
			SELECT User_UserId UserId, COUNT(*) AnswerCount
			  FROM Answer
			 WHERE AnswerState = 2 -- approved
			   AND User_UserId = @userId
			 GROUP BY User_UserId) a
				ON u.UserId = a.UserId
	  LEFT JOIN (
			SELECT ForUser_UserId UserId, SUM(PointsAwarded) PointsAwarded
			  FROM UserPoint
			 WHERE ForUser_UserId = @userId
			 GROUP BY ForUser_UserId) p
				ON u.UserId = p.UserId
	  LEFT JOIN (
			SELECT a.User_UserId UserId, COUNT(*) StarVoteCount
			  FROM Question q, Answer a, Vote v
			 WHERE q.QuestionId = a.Question_QuestionId
			   AND a.Question_QuestionId = v.Question_QuestionId
			   AND a.AnswerId = v.Answer_AnswerId
			   AND q.User_UserId = v.User_UserId
			   AND a.AnswerState = 2 -- approved
			   AND a.User_UserId = @userId
			 GROUP BY a.User_UserId) s
				ON u.UserId = s.UserId
	 WHERE u.UserId = @userId
GO







SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF OBJECT_ID ( 'RetireOldQuestions', 'P' ) IS NOT NULL 
    DROP PROCEDURE RetireOldQuestions;
GO

CREATE PROCEDURE RetireOldQuestions
	@DateTimeNow datetime2 = null
AS
    SET @DateTimeNow = dbo.TimeSourceNow(@DateTimeNow)
	
	SET NOCOUNT ON;

	-- CLEAN UP TEMPORARY DATA
    EXEC FlushRetirementTables
	
	/********************************************************/
	/* BUILD CACHE DATA										*/
	/********************************************************/
    
	-- get the questions to retire
	INSERT INTO QuestionRetirement
    (QuestionId)
    SELECT q.QuestionId
      FROM Question q, SystemConfiguration sc
     WHERE q.ClosedDateTime IS NOT NULL
	   AND DATEADD(dd, sc.RetireQuestionAfterDays, q.ClosedDateTime) < @DateTimeNow
	
	-- grab all of the votes
	INSERT INTO VoteCountRetirement
	(QuestionId, AnswerId, Votes)
	SELECT r.QuestionId, r.AnswerId, r.VoteCount
	  FROM (
		SELECT qr.QuestionId, a.AnswerId, COUNT(*) VoteCount
		  FROM QuestionRetirement qr, Answer a, Vote v
		 WHERE qr.QuestionId = a.Question_QuestionId
		   AND a.Question_QuestionId = v.Question_QuestionId
		   AND a.AnswerId = v.Answer_AnswerId
		   AND a.AnswerState = 2 -- approved
		 GROUP BY qr.QuestionId, a.AnswerId
		 ) r
	 WHERE r.VoteCount >= 10 -- you need at least 10 votes to be popular
	
	-- remove any votes that aren't significant to us
	DELETE FROM VoteCountRetirement
	 WHERE Id NOT IN (
			SELECT a.Id
			  FROM VoteCountRetirement a, (
					SELECT QuestionId, MAX(Votes) MaxVotes
					  FROM VoteCountRetirement
					 GROUP BY QuestionId) b
			 WHERE a.QuestionId = b.QuestionId
			   AND a.Votes = b.MaxVotes)

	-- collect user points for questions being retired
	INSERT INTO UserPointRetirement
    (UserId, TotalPoints)
    SELECT up.ForUser_UserId, SUM(up.PointsAwarded)
      FROM UserPoint up, QuestionRetirement qr
     WHERE up.RelatedQuestion_QuestionId = qr.QuestionId
     GROUP BY up.ForUser_UserId

	-- collect moderation items that are being retired
    INSERT INTO ModerationRetirement
    (ModerationId)
    SELECT m.ModerationId
      FROM Moderation m, QuestionRetirement qr
     WHERE m.Question_QuestionId = qr.QuestionId
     UNION 
    SELECT m.ModerationId
      FROM Moderation m, QuestionRetirement qr, Answer a
     WHERE a.Question_QuestionId = qr.QuestionId
       AND m.Answer_AnswerId = a.AnswerId

	/********************************************************/
	/* UPDATE USER RECORDS									*/
	/********************************************************/
    UPDATE u
	   SET u.HistoricQuestions = u.HistoricQuestions + r.QuestionCount
	  FROM [User] u
	 INNER JOIN (
			SELECT q.User_UserId UserId, COUNT(*) QuestionCount
			  FROM QuestionRetirement qr, Question q
			 WHERE qr.QuestionId = q.QuestionId
			   AND q.QuestionState = 1 -- approved
			 GROUP BY q.User_UserId) r
			 ON u.UserId = r.UserId
	
	UPDATE u
	   SET u.HistoricAnswers = u.HistoricAnswers + r.AnswerCount
	  FROM [User] u
	 INNER JOIN (
			SELECT a.User_UserId UserId, COUNT(*) AnswerCount
			  FROM QuestionRetirement qr, Answer a
			 WHERE qr.QuestionId = a.Question_QuestionId
			   AND a.AnswerState = 2 -- approved
			 GROUP BY a.User_UserId) r
			 ON u.UserId = r.UserId

	UPDATE u
	   SET u.HistoricStarVotes = u.HistoricStarVotes + r.StarVoteCount
	  FROM [User] u
	 INNER JOIN (
			SELECT a.User_UserId UserId, COUNT(*) StarVoteCount
			  FROM QuestionRetirement qr, Question q, Answer a, Vote v
			 WHERE qr.QuestionId = q.QuestionId
			   AND q.QuestionId = a.Question_QuestionId
			   AND a.Question_QuestionId = v.Question_QuestionId
			   AND a.AnswerId = v.Answer_AnswerId
			   AND q.User_UserId = v.User_UserId
			   AND a.AnswerState = 2 -- approved
			 GROUP BY a.User_UserId) r
	         ON u.UserId = r.UserId

	UPDATE u
	   SET u.HistoricPopularVotes = u.HistoricPopularVotes + r.PopularVoteCount
	  FROM [User] u
	  INNER JOIN (
			SELECT a.User_UserId UserId, COUNT(*) PopularVoteCount
			  FROM VoteCountRetirement vc, Answer a
			 WHERE vc.QuestionId = a.Question_QuestionId
			   AND vc.AnswerId = a.AnswerId
			   AND a.AnswerState = 2 -- approved
			 GROUP BY a.User_UserId) r
	         ON u.UserId = r.UserId
	
    UPDATE u
       SET u.HistoricPoints = u.HistoricPoints + upr.TotalPoints
      FROM [User] u
     INNER JOIN UserPointRetirement upr
        ON u.UserId = upr.UserId
    
    /********************************************************/
	/* RETIRE DATA											*/
	/********************************************************/

	DELETE FROM ReportedPost
	 WHERE Answer_AnswerId IN (
            SELECT a.AnswerId 
              FROM Answer a, QuestionRetirement qr
             WHERE a.Question_QuestionId = qr.QuestionId)

	DELETE FROM ReportedPost
	 WHERE Question_QuestionId IN (SELECT QuestionId FROM QuestionRetirement) 
	
	DELETE FROM UserPoint
     WHERE RelatedQuestion_QuestionId IN (SELECT QuestionId FROM QuestionRetirement) 

    -- CLEAN UP RELATIONSHIPS
    DELETE FROM Notification
     WHERE Question_QuestionId IN (SELECT QuestionId FROM QuestionRetirement)
        OR Answer_AnswerId IN (
            SELECT a.AnswerId 
              FROM Answer a, QuestionRetirement qr
             WHERE a.Question_QuestionId = qr.QuestionId)
    
    DELETE FROM ModerationEntry
     WHERE Moderation_ModerationId IN (SELECT ModerationId FROM ModerationRetirement)

    DELETE FROM Moderation
     WHERE ModerationId IN (SELECT ModerationId FROM ModerationRetirement)

    DELETE FROM Vote
     WHERE Question_QuestionId IN (SELECT QuestionId FROM QuestionRetirement)
        OR Answer_AnswerId IN (
            SELECT a.AnswerId 
              FROM Answer a, QuestionRetirement qr
             WHERE a.Question_QuestionId = qr.QuestionId)
         
    DELETE FROM Answer
     WHERE Question_QuestionId IN (SELECT QuestionId FROM QuestionRetirement)
 
    DELETE FROM Bookmark
	 WHERE Question_QuestionId IN (SELECT QuestionId FROM QuestionRetirement)
	
	DELETE From Question
     WHERE QuestionId IN (SELECT QuestionId FROM QuestionRetirement)
 
    -- CLEAN UP TEMPORARY DATA
    EXEC FlushRetirementTables TRUE

	UPDATE LastRunLog SET RetireOldQuestions = @DateTimeNow;
GO



/****************************************************************************************************/
/*  DB MAIL                                                                                         */
/* http://www.snapdba.com/2013/04/enabling-and-configuring-database-mail-in-sql-server-using-t-sql/ */
/****************************************************************************************************/

USE master
GO
sp_configure 'show advanced options',1
GO
RECONFIGURE WITH OVERRIDE
GO
sp_configure 'Database Mail XPs',1
GO
RECONFIGURE 
GO


USE msdb
GO
EXECUTE msdb.dbo.sysmail_add_profile_sp
@profile_name = 'mailer',
@description = 'Profile for sending Automated DBA Notifications'
GO


EXECUTE msdb.dbo.sysmail_add_account_sp
@account_name = 'SQLAlerts',
@description = 'Account for Automated DBA Notifications',
@email_address = 'systemlog@ourdilemmas.com',
@display_name = 'SQL Alerts',
@mailserver_name = 'smtp.ourdilemmas.com',
@port = 25
GO

EXECUTE msdb.dbo.sysmail_add_profileaccount_sp
@profile_name = 'mailer',
@account_name = 'SQLAlerts',
@sequence_number = 1
GO

USE msdb
GO
EXEC master.dbo.xp_instance_regwrite
N'HKEY_LOCAL_MACHINE',
N'SOFTWARE\Microsoft\MSSQLServer\SQLServerAgent',
N'UseDatabaseMail',
N'REG_DWORD', 1
EXEC master.dbo.xp_instance_regwrite
N'HKEY_LOCAL_MACHINE',
N'SOFTWARE\Microsoft\MSSQLServer\SQLServerAgent',
N'DatabaseMailProfile',
N'REG_SZ',
N'mailer'






/*********************************************************************************/
/*  LOGGING                                                                      */
/* http://justinpdavis.blogspot.co.uk/2010/04/logging-to-database-with-nlog.html */
/*********************************************************************************/

SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO

SET ANSI_PADDING ON 
GO

CREATE TABLE [dbo].[SystemLogging]( 
    [SystemLoggingGuid] [UNIQUEIDENTIFIER] ROWGUIDCOL  NOT NULL, 
    [EnteredDate] [DATETIME] NULL, 
    [LogApplication] [VARCHAR](200) NULL, 
    [LogDate] [VARCHAR](100) NULL, 
    [LogLevel] [VARCHAR](100) NULL, 
    [LogLogger] [VARCHAR](8000) NULL, 
    [LogMessage] [VARCHAR](8000) NULL, 
    [LogMachineName] [VARCHAR](8000) NULL, 
    [LogUserName] [VARCHAR](8000) NULL, 
    [LogCallSite] [VARCHAR](8000) NULL, 
    [LogThread] [VARCHAR](100) NULL, 
    [LogException] [VARCHAR](8000) NULL, 
    [LogStacktrace] [VARCHAR](8000) NULL, 
CONSTRAINT [PKSystemLogging] PRIMARY KEY CLUSTERED 
( 
    [SYSTEMLOGGINGGUID] ASC 
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY] 
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF 
GO

ALTER TABLE [DBO].[SYSTEMLOGGING] ADD  CONSTRAINT [DFSystemLoggingSystemLoggingGuid]  DEFAULT (NEWID()) FOR [SYSTEMLOGGINGGUID] 
GO

ALTER TABLE [DBO].[SYSTEMLOGGING] ADD  CONSTRAINT [DFSystemLoggingEnteredDate]  DEFAULT (GETDATE()) FOR [ENTEREDDATE] 
GO



use dilemma
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF OBJECT_ID ( 'LogEmail', 'TR' ) IS NOT NULL 
    DROP TRIGGER LogEmail;
GO

-- ============================================= 
-- Author:        Justin Davis 
-- Create date: 4/1/2008 
-- Description:    Send email of log message 
-- ============================================= 
CREATE TRIGGER [dbo].[LogEmail] 
   ON  [dbo].[systemlogging] 
   AFTER insert 
AS 
BEGIN 
    -- SET NOCOUNT ON added to prevent extra result sets from 
    -- interfering with SELECT statements. 
    SET NOCOUNT ON; 
    
	IF NOT EXISTS (SELECT * FROM SystemConfiguration WHERE EmailErrors = 1 AND EmailErrorsTo is not null)
	Begin
		Return
	End

	Declare @ToEmail varchar(100) 
    Declare @Title varchar(100) 
    Declare @logmessage varchar(8000) 
    Declare @loglevel as varchar(100)    
    set @ToEmail = (SELECT EmailErrorsTo FROM SystemConfiguration)
    set @loglevel = (select loglevel from inserted) 
    set @Title = 'OurDilemmas ' + @loglevel 
	set @logmessage = (select 
		'User Date:' + char(9) + char(9) + logdate + char(13) + char(10) + 
		'Computer:'+ char(9) + logmachinename + char(13) + char(10) +  
		'User:' + char(9) + char(9) + logusername + char(13) + char(10) +  
		'Level:' + char(9)+ loglevel + char(13) + char(10) +  
		'Logger:' + char(9)+ loglogger + char(13) + char(10) + 
		'Thread:'+ char(9) + logthread + char(13) + char(10) +    
		'CallSite:'+ char(9) + logcallsite + char(13) + char(10) + 
		'Message:' + char(9) + logmessage + char(13) + char(10) +  
		'Exception:'+ char(9) + logexception + char(13) + char(10) +  
		'StackTrace:'+ char(9) + logstacktrace as 'emailmessage'
    from inserted) 
    
	If EXISTS (SELECT * FROM EmailLogLevel WHERE LOWER(LogLevel) = LOWER(@loglevel) AND EnableEmails = 1)
	Begin
		EXEC msdb.dbo.sp_send_dbmail @recipients=@ToEmail, @body= @logmessage,  @subject = @Title, @profile_name = 'admin'
	End
END
GO

INSERT INTO [EmailLogLevel] ([LogLevel], [EnableEmails]) VALUES ('Debug', 0)
INSERT INTO [EmailLogLevel] ([LogLevel], [EnableEmails]) VALUES ('Error', 1)
INSERT INTO [EmailLogLevel] ([LogLevel], [EnableEmails]) VALUES ('Fatal', 1)
INSERT INTO [EmailLogLevel] ([LogLevel], [EnableEmails]) VALUES ('Info', 1)
INSERT INTO [EmailLogLevel] ([LogLevel], [EnableEmails]) VALUES ('Trace', 0)
INSERT INTO [EmailLogLevel] ([LogLevel], [EnableEmails]) VALUES ('Warn', 1)
GO

INSERT INTO [SystemLogging] ([LogApplication], [LogDate], [LogLevel], [LogLogger], [LogMessage], [LogMachineName], [LogUserName], [LogCallSite], [LogThread], [LogException], [LogStacktrace])
SELECT 'Logging Mail Test', 
		GETDATE(), 
		'Error', 
		'DB Insert Statement', 
		'This is a test log message', 
		CONVERT(sysname, SERVERPROPERTY('MachineName')), 
		CURRENT_USER, 
		CONVERT(sysname, SERVERPROPERTY('servername')), 
		42, 
		'No Exception', 
		'No Stacktrace'
GO

select * from [msdb].dbo.sysmail_allitems
GO