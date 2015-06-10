CREATE UNIQUE INDEX IX_PointType ON PointConfiguration (PointType);
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
	   SET AnswerState = 500 -- Expired
	 WHERE AnswerId IN (	
		SELECT AnswerId
		  FROM Answer a, SystemConfiguration sc
		 WHERE a.AnswerState = 100 -- ReservedSlot
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
		 WHERE QuestionState = 200 -- Approved Question
		   AND ClosedDateTime IS NULL
		   AND ClosesDateTime < @DateTimeNow
		 UNION 
		SELECT q.QuestionId
		  FROM Question q, (
			SELECT Question_QuestionId QuestionId, COUNT(*) AnswerCount
			  FROM Answer
			 WHERE AnswerState = 300 -- Approved Answer
			 GROUP BY Question_QuestionId
			) a
		 WHERE q.QuestionId = a.QuestionId
		   AND q.QuestionState = 200 -- Approved Question
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
			 WHERE QuestionState = 200 -- approved
			   AND User_UserId = @userId
			 GROUP BY User_UserId) q
				ON u.UserId = q.UserId
	  LEFT JOIN (
			SELECT User_UserId UserId, COUNT(*) AnswerCount
			  FROM Answer
			 WHERE AnswerState = 300 -- approved
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
			   AND a.AnswerState = 300 -- approved
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
		   AND a.AnswerState = 300 -- approved
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
			   AND q.QuestionState = 200 -- approved
			 GROUP BY q.User_UserId) r
			 ON u.UserId = r.UserId
	
	UPDATE u
	   SET u.HistoricAnswers = u.HistoricAnswers + r.AnswerCount
	  FROM [User] u
	 INNER JOIN (
			SELECT a.User_UserId UserId, COUNT(*) AnswerCount
			  FROM QuestionRetirement qr, Answer a
			 WHERE qr.QuestionId = a.Question_QuestionId
			   AND a.AnswerState = 300 -- approved
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
			   AND a.AnswerState = 300 -- approved
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
			   AND a.AnswerState = 300 -- approved
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




-- sql generated by https://github.com/timabell/ef-enum-to-lookup

set nocount on;
set xact_abort on; -- rollback on error
begin tran;
IF OBJECT_ID('Enum_AnswerState', 'U') IS NULL
begin
	CREATE TABLE [Enum_AnswerState] (Id int PRIMARY KEY, Name nvarchar(255));
	exec sys.sp_addextendedproperty @name=N'MS_Description', @level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE',
		@level1name=N'Enum_AnswerState', @value=N'Automatically generated. Contents will be overwritten on app startup. Table & contents generated by https://github.com/timabell/ef-enum-to-lookup';
end
IF OBJECT_ID('Enum_QuestionState', 'U') IS NULL
begin
	CREATE TABLE [Enum_QuestionState] (Id int PRIMARY KEY, Name nvarchar(255));
	exec sys.sp_addextendedproperty @name=N'MS_Description', @level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE',
		@level1name=N'Enum_QuestionState', @value=N'Automatically generated. Contents will be overwritten on app startup. Table & contents generated by https://github.com/timabell/ef-enum-to-lookup';
end
IF OBJECT_ID('Enum_FollowupState', 'U') IS NULL
begin
	CREATE TABLE [Enum_FollowupState] (Id int PRIMARY KEY, Name nvarchar(255));
	exec sys.sp_addextendedproperty @name=N'MS_Description', @level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE',
		@level1name=N'Enum_FollowupState', @value=N'Automatically generated. Contents will be overwritten on app startup. Table & contents generated by https://github.com/timabell/ef-enum-to-lookup';
end
IF OBJECT_ID('Enum_UserType', 'U') IS NULL
begin
	CREATE TABLE [Enum_UserType] (Id int PRIMARY KEY, Name nvarchar(255));
	exec sys.sp_addextendedproperty @name=N'MS_Description', @level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE',
		@level1name=N'Enum_UserType', @value=N'Automatically generated. Contents will be overwritten on app startup. Table & contents generated by https://github.com/timabell/ef-enum-to-lookup';
end
IF OBJECT_ID('Enum_ModerationState', 'U') IS NULL
begin
	CREATE TABLE [Enum_ModerationState] (Id int PRIMARY KEY, Name nvarchar(255));
	exec sys.sp_addextendedproperty @name=N'MS_Description', @level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE',
		@level1name=N'Enum_ModerationState', @value=N'Automatically generated. Contents will be overwritten on app startup. Table & contents generated by https://github.com/timabell/ef-enum-to-lookup';
end
IF OBJECT_ID('Enum_ModerationFor', 'U') IS NULL
begin
	CREATE TABLE [Enum_ModerationFor] (Id int PRIMARY KEY, Name nvarchar(255));
	exec sys.sp_addextendedproperty @name=N'MS_Description', @level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE',
		@level1name=N'Enum_ModerationFor', @value=N'Automatically generated. Contents will be overwritten on app startup. Table & contents generated by https://github.com/timabell/ef-enum-to-lookup';
end
IF OBJECT_ID('Enum_NotificationType', 'U') IS NULL
begin
	CREATE TABLE [Enum_NotificationType] (Id int PRIMARY KEY, Name nvarchar(255));
	exec sys.sp_addextendedproperty @name=N'MS_Description', @level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE',
		@level1name=N'Enum_NotificationType', @value=N'Automatically generated. Contents will be overwritten on app startup. Table & contents generated by https://github.com/timabell/ef-enum-to-lookup';
end
IF OBJECT_ID('Enum_NotificationTarget', 'U') IS NULL
begin
	CREATE TABLE [Enum_NotificationTarget] (Id int PRIMARY KEY, Name nvarchar(255));
	exec sys.sp_addextendedproperty @name=N'MS_Description', @level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE',
		@level1name=N'Enum_NotificationTarget', @value=N'Automatically generated. Contents will be overwritten on app startup. Table & contents generated by https://github.com/timabell/ef-enum-to-lookup';
end
IF OBJECT_ID('Enum_PointType', 'U') IS NULL
begin
	CREATE TABLE [Enum_PointType] (Id int PRIMARY KEY, Name nvarchar(255));
	exec sys.sp_addextendedproperty @name=N'MS_Description', @level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE',
		@level1name=N'Enum_PointType', @value=N'Automatically generated. Contents will be overwritten on app startup. Table & contents generated by https://github.com/timabell/ef-enum-to-lookup';
end
IF OBJECT_ID('Enum_ReportReason', 'U') IS NULL
begin
	CREATE TABLE [Enum_ReportReason] (Id int PRIMARY KEY, Name nvarchar(255));
	exec sys.sp_addextendedproperty @name=N'MS_Description', @level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE',
		@level1name=N'Enum_ReportReason', @value=N'Automatically generated. Contents will be overwritten on app startup. Table & contents generated by https://github.com/timabell/ef-enum-to-lookup';
end
IF OBJECT_ID('Enum_ServerRole', 'U') IS NULL
begin
	CREATE TABLE [Enum_ServerRole] (Id int PRIMARY KEY, Name nvarchar(255));
	exec sys.sp_addextendedproperty @name=N'MS_Description', @level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE',
		@level1name=N'Enum_ServerRole', @value=N'Automatically generated. Contents will be overwritten on app startup. Table & contents generated by https://github.com/timabell/ef-enum-to-lookup';
end
IF OBJECT_ID('Enum_QuestionLifetime', 'U') IS NULL
begin
	CREATE TABLE [Enum_QuestionLifetime] (Id int PRIMARY KEY, Name nvarchar(255));
	exec sys.sp_addextendedproperty @name=N'MS_Description', @level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE',
		@level1name=N'Enum_QuestionLifetime', @value=N'Automatically generated. Contents will be overwritten on app startup. Table & contents generated by https://github.com/timabell/ef-enum-to-lookup';
end
IF OBJECT_ID('Enum_SystemEnvironment', 'U') IS NULL
begin
	CREATE TABLE [Enum_SystemEnvironment] (Id int PRIMARY KEY, Name nvarchar(255));
	exec sys.sp_addextendedproperty @name=N'MS_Description', @level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE',
		@level1name=N'Enum_SystemEnvironment', @value=N'Automatically generated. Contents will be overwritten on app startup. Table & contents generated by https://github.com/timabell/ef-enum-to-lookup';
end

CREATE TABLE #lookups (Id int, Name nvarchar(255) COLLATE database_default);
INSERT INTO #lookups (Id, Name) VALUES (100, N'Reserved Slot');
INSERT INTO #lookups (Id, Name) VALUES (200, N'Ready For Moderation');
INSERT INTO #lookups (Id, Name) VALUES (300, N'Approved');
INSERT INTO #lookups (Id, Name) VALUES (400, N'Rejected');
INSERT INTO #lookups (Id, Name) VALUES (500, N'Expired');

MERGE INTO [Enum_AnswerState] dst
	USING #lookups src ON src.Id = dst.Id
	WHEN MATCHED AND src.Name <> dst.Name THEN
		UPDATE SET Name = src.Name
	WHEN NOT MATCHED THEN
		INSERT (Id, Name)
		VALUES (src.Id, src.Name)
	WHEN NOT MATCHED BY SOURCE THEN
		DELETE
;
TRUNCATE TABLE #lookups;

INSERT INTO #lookups (Id, Name) VALUES (100, N'Ready For Moderation');
INSERT INTO #lookups (Id, Name) VALUES (200, N'Approved');
INSERT INTO #lookups (Id, Name) VALUES (300, N'Rejected');

MERGE INTO [Enum_QuestionState] dst
	USING #lookups src ON src.Id = dst.Id
	WHEN MATCHED AND src.Name <> dst.Name THEN
		UPDATE SET Name = src.Name
	WHEN NOT MATCHED THEN
		INSERT (Id, Name)
		VALUES (src.Id, src.Name)
	WHEN NOT MATCHED BY SOURCE THEN
		DELETE
;
TRUNCATE TABLE #lookups;

INSERT INTO #lookups (Id, Name) VALUES (100, N'Ready For Moderation');
INSERT INTO #lookups (Id, Name) VALUES (200, N'Approved');
INSERT INTO #lookups (Id, Name) VALUES (300, N'Rejected');

MERGE INTO [Enum_FollowupState] dst
	USING #lookups src ON src.Id = dst.Id
	WHEN MATCHED AND src.Name <> dst.Name THEN
		UPDATE SET Name = src.Name
	WHEN NOT MATCHED THEN
		INSERT (Id, Name)
		VALUES (src.Id, src.Name)
	WHEN NOT MATCHED BY SOURCE THEN
		DELETE
;
TRUNCATE TABLE #lookups;

INSERT INTO #lookups (Id, Name) VALUES (100, N'Anonymous');
INSERT INTO #lookups (Id, Name) VALUES (200, N'Registered');

MERGE INTO [Enum_UserType] dst
	USING #lookups src ON src.Id = dst.Id
	WHEN MATCHED AND src.Name <> dst.Name THEN
		UPDATE SET Name = src.Name
	WHEN NOT MATCHED THEN
		INSERT (Id, Name)
		VALUES (src.Id, src.Name)
	WHEN NOT MATCHED BY SOURCE THEN
		DELETE
;
TRUNCATE TABLE #lookups;

INSERT INTO #lookups (Id, Name) VALUES (100, N'Queued');
INSERT INTO #lookups (Id, Name) VALUES (200, N'Approved');
INSERT INTO #lookups (Id, Name) VALUES (300, N'Rejected');
INSERT INTO #lookups (Id, Name) VALUES (400, N'Reported');
INSERT INTO #lookups (Id, Name) VALUES (500, N'Re Queued');

MERGE INTO [Enum_ModerationState] dst
	USING #lookups src ON src.Id = dst.Id
	WHEN MATCHED AND src.Name <> dst.Name THEN
		UPDATE SET Name = src.Name
	WHEN NOT MATCHED THEN
		INSERT (Id, Name)
		VALUES (src.Id, src.Name)
	WHEN NOT MATCHED BY SOURCE THEN
		DELETE
;
TRUNCATE TABLE #lookups;

INSERT INTO #lookups (Id, Name) VALUES (0, N'Question');
INSERT INTO #lookups (Id, Name) VALUES (1, N'Answer');
INSERT INTO #lookups (Id, Name) VALUES (2, N'Followup');

MERGE INTO [Enum_ModerationFor] dst
	USING #lookups src ON src.Id = dst.Id
	WHEN MATCHED AND src.Name <> dst.Name THEN
		UPDATE SET Name = src.Name
	WHEN NOT MATCHED THEN
		INSERT (Id, Name)
		VALUES (src.Id, src.Name)
	WHEN NOT MATCHED BY SOURCE THEN
		DELETE
;
TRUNCATE TABLE #lookups;

INSERT INTO #lookups (Id, Name) VALUES (110, N'Question Approved');
INSERT INTO #lookups (Id, Name) VALUES (120, N'Answer Approved');
INSERT INTO #lookups (Id, Name) VALUES (130, N'Followup Approved');
INSERT INTO #lookups (Id, Name) VALUES (140, N'Question Rejected');
INSERT INTO #lookups (Id, Name) VALUES (150, N'Answer Rejected');
INSERT INTO #lookups (Id, Name) VALUES (160, N'Followup Rejected');
INSERT INTO #lookups (Id, Name) VALUES (170, N'Flagged Question Approved');
INSERT INTO #lookups (Id, Name) VALUES (180, N'Flagged Answer Approved');
INSERT INTO #lookups (Id, Name) VALUES (190, N'Flagged Followup Approved');
INSERT INTO #lookups (Id, Name) VALUES (200, N'Flagged Question Rejected');
INSERT INTO #lookups (Id, Name) VALUES (210, N'Flagged Answer Rejected');
INSERT INTO #lookups (Id, Name) VALUES (220, N'Flagged Followup Rejected');
INSERT INTO #lookups (Id, Name) VALUES (230, N'Open For Voting');
INSERT INTO #lookups (Id, Name) VALUES (240, N'Vote On Answer');
INSERT INTO #lookups (Id, Name) VALUES (250, N'Best Answer Awarded');

MERGE INTO [Enum_NotificationType] dst
	USING #lookups src ON src.Id = dst.Id
	WHEN MATCHED AND src.Name <> dst.Name THEN
		UPDATE SET Name = src.Name
	WHEN NOT MATCHED THEN
		INSERT (Id, Name)
		VALUES (src.Id, src.Name)
	WHEN NOT MATCHED BY SOURCE THEN
		DELETE
;
TRUNCATE TABLE #lookups;

INSERT INTO #lookups (Id, Name) VALUES (0, N'Questioner');
INSERT INTO #lookups (Id, Name) VALUES (1, N'Answerer');
INSERT INTO #lookups (Id, Name) VALUES (2, N'Bookmarker');
INSERT INTO #lookups (Id, Name) VALUES (3, N'Flagger');

MERGE INTO [Enum_NotificationTarget] dst
	USING #lookups src ON src.Id = dst.Id
	WHEN MATCHED AND src.Name <> dst.Name THEN
		UPDATE SET Name = src.Name
	WHEN NOT MATCHED THEN
		INSERT (Id, Name)
		VALUES (src.Id, src.Name)
	WHEN NOT MATCHED BY SOURCE THEN
		DELETE
;
TRUNCATE TABLE #lookups;

INSERT INTO #lookups (Id, Name) VALUES (1, N'Question Asked');
INSERT INTO #lookups (Id, Name) VALUES (2, N'Question Answered');
INSERT INTO #lookups (Id, Name) VALUES (3, N'Star Vote Received');
INSERT INTO #lookups (Id, Name) VALUES (4, N'Popular Vote Received');

MERGE INTO [Enum_PointType] dst
	USING #lookups src ON src.Id = dst.Id
	WHEN MATCHED AND src.Name <> dst.Name THEN
		UPDATE SET Name = src.Name
	WHEN NOT MATCHED THEN
		INSERT (Id, Name)
		VALUES (src.Id, src.Name)
	WHEN NOT MATCHED BY SOURCE THEN
		DELETE
;
TRUNCATE TABLE #lookups;

INSERT INTO #lookups (Id, Name) VALUES (100, N'Offensive Content');
INSERT INTO #lookups (Id, Name) VALUES (200, N'Spam');
INSERT INTO #lookups (Id, Name) VALUES (300, N'Unrelated To Subject');

MERGE INTO [Enum_ReportReason] dst
	USING #lookups src ON src.Id = dst.Id
	WHEN MATCHED AND src.Name <> dst.Name THEN
		UPDATE SET Name = src.Name
	WHEN NOT MATCHED THEN
		INSERT (Id, Name)
		VALUES (src.Id, src.Name)
	WHEN NOT MATCHED BY SOURCE THEN
		DELETE
;
TRUNCATE TABLE #lookups;

INSERT INTO #lookups (Id, Name) VALUES (100, N'Offline');
INSERT INTO #lookups (Id, Name) VALUES (200, N'Moderation');
INSERT INTO #lookups (Id, Name) VALUES (300, N'Public');
INSERT INTO #lookups (Id, Name) VALUES (400, N'Question Seeder');

MERGE INTO [Enum_ServerRole] dst
	USING #lookups src ON src.Id = dst.Id
	WHEN MATCHED AND src.Name <> dst.Name THEN
		UPDATE SET Name = src.Name
	WHEN NOT MATCHED THEN
		INSERT (Id, Name)
		VALUES (src.Id, src.Name)
	WHEN NOT MATCHED BY SOURCE THEN
		DELETE
;
TRUNCATE TABLE #lookups;

INSERT INTO #lookups (Id, Name) VALUES (0, N'One Minute');
INSERT INTO #lookups (Id, Name) VALUES (1, N'Five Minutes');
INSERT INTO #lookups (Id, Name) VALUES (2, N'One Day');
INSERT INTO #lookups (Id, Name) VALUES (3, N'One Year');

MERGE INTO [Enum_QuestionLifetime] dst
	USING #lookups src ON src.Id = dst.Id
	WHEN MATCHED AND src.Name <> dst.Name THEN
		UPDATE SET Name = src.Name
	WHEN NOT MATCHED THEN
		INSERT (Id, Name)
		VALUES (src.Id, src.Name)
	WHEN NOT MATCHED BY SOURCE THEN
		DELETE
;
TRUNCATE TABLE #lookups;

INSERT INTO #lookups (Id, Name) VALUES (100, N'Development');
INSERT INTO #lookups (Id, Name) VALUES (200, N'Testing');
INSERT INTO #lookups (Id, Name) VALUES (300, N'Production');

MERGE INTO [Enum_SystemEnvironment] dst
	USING #lookups src ON src.Id = dst.Id
	WHEN MATCHED AND src.Name <> dst.Name THEN
		UPDATE SET Name = src.Name
	WHEN NOT MATCHED THEN
		INSERT (Id, Name)
		VALUES (src.Id, src.Name)
	WHEN NOT MATCHED BY SOURCE THEN
		DELETE
;
TRUNCATE TABLE #lookups;

DROP TABLE #lookups;

 IF OBJECT_ID('FK_Answer_AnswerState', 'F') IS NULL ALTER TABLE [Answer] ADD CONSTRAINT FK_Answer_AnswerState FOREIGN KEY ([AnswerState]) REFERENCES [Enum_AnswerState] (Id);
 IF OBJECT_ID('FK_Question_QuestionState', 'F') IS NULL ALTER TABLE [Question] ADD CONSTRAINT FK_Question_QuestionState FOREIGN KEY ([QuestionState]) REFERENCES [Enum_QuestionState] (Id);
 IF OBJECT_ID('FK_Followup_FollowupState', 'F') IS NULL ALTER TABLE [Followup] ADD CONSTRAINT FK_Followup_FollowupState FOREIGN KEY ([FollowupState]) REFERENCES [Enum_FollowupState] (Id);
 IF OBJECT_ID('FK_User_UserType', 'F') IS NULL ALTER TABLE [User] ADD CONSTRAINT FK_User_UserType FOREIGN KEY ([UserType]) REFERENCES [Enum_UserType] (Id);
 IF OBJECT_ID('FK_ModerationEntry_State', 'F') IS NULL ALTER TABLE [ModerationEntry] ADD CONSTRAINT FK_ModerationEntry_State FOREIGN KEY ([State]) REFERENCES [Enum_ModerationState] (Id);
 IF OBJECT_ID('FK_Moderation_ModerationFor', 'F') IS NULL ALTER TABLE [Moderation] ADD CONSTRAINT FK_Moderation_ModerationFor FOREIGN KEY ([ModerationFor]) REFERENCES [Enum_ModerationFor] (Id);
 IF OBJECT_ID('FK_Notification_NotificationType', 'F') IS NULL ALTER TABLE [Notification] ADD CONSTRAINT FK_Notification_NotificationType FOREIGN KEY ([NotificationType]) REFERENCES [Enum_NotificationType] (Id);
 IF OBJECT_ID('FK_Notification_NotificationTarget', 'F') IS NULL ALTER TABLE [Notification] ADD CONSTRAINT FK_Notification_NotificationTarget FOREIGN KEY ([NotificationTarget]) REFERENCES [Enum_NotificationTarget] (Id);
 IF OBJECT_ID('FK_PointConfiguration_PointType', 'F') IS NULL ALTER TABLE [PointConfiguration] ADD CONSTRAINT FK_PointConfiguration_PointType FOREIGN KEY ([PointType]) REFERENCES [Enum_PointType] (Id);
 IF OBJECT_ID('FK_ReportedPost_Reason', 'F') IS NULL ALTER TABLE [ReportedPost] ADD CONSTRAINT FK_ReportedPost_Reason FOREIGN KEY ([Reason]) REFERENCES [Enum_ReportReason] (Id);
 IF OBJECT_ID('FK_SystemConfiguration_QuestionLifetime', 'F') IS NULL ALTER TABLE [SystemConfiguration] ADD CONSTRAINT FK_SystemConfiguration_QuestionLifetime FOREIGN KEY ([QuestionLifetime]) REFERENCES [Enum_QuestionLifetime] (Id);
 IF OBJECT_ID('FK_SystemConfiguration_SystemEnvironment', 'F') IS NULL ALTER TABLE [SystemConfiguration] ADD CONSTRAINT FK_SystemConfiguration_SystemEnvironment FOREIGN KEY ([SystemEnvironment]) REFERENCES [Enum_SystemEnvironment] (Id);
 IF OBJECT_ID('FK_UserPoint_PointType', 'F') IS NULL ALTER TABLE [UserPoint] ADD CONSTRAINT FK_UserPoint_PointType FOREIGN KEY ([PointType]) REFERENCES [Enum_PointType] (Id);

commit;





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
    [LogLogger] [VARCHAR](MAX) NULL, 
    [LogMessage] [VARCHAR](MAX) NULL, 
    [LogMachineName] [VARCHAR](MAX) NULL, 
    [LogUserName] [VARCHAR](MAX) NULL, 
    [LogCallSite] [VARCHAR](MAX) NULL, 
    [LogThread] [VARCHAR](100) NULL, 
    [LogException] [VARCHAR](MAX) NULL, 
    [LogStacktrace] [VARCHAR](MAX) NULL, 
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





