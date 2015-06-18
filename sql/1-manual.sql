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
     UNION 
    SELECT m.ModerationId
      FROM Moderation m, QuestionRetirement qr, Followup f
     WHERE f.Question_QuestionId = qr.QuestionId
       AND m.Followup_FollowupId = f.FollowupId

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

/*	DELETE FROM ReportedPost
	 WHERE Followup_FollowupId IN (
            SELECT f.FollowupId 
              FROM Followup_Followup f, QuestionRetirement qr
             WHERE f.Question_QuestionId = qr.QuestionId)*/

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
        OR Followup_FollowupId IN (
            SELECT f.FollowupId 
              FROM Followup f, QuestionRetirement qr
             WHERE f.Question_QuestionId = qr.QuestionId)
    
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
 
    UPDATE Question
	   SET Followup_FollowupId = NULL
	 WHERE QuestionId IN (SELECT QuestionId FROM QuestionRetirement)
	
	UPDATE Question
	   SET Followup_FollowupId = NULL
	 WHERE QuestionId IN (SELECT QuestionId FROM QuestionRetirement)
	
	DELETE FROM Followup
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
INSERT INTO #lookups (Id, Name) VALUES (5, N'Vote Cast');
INSERT INTO #lookups (Id, Name) VALUES (6, N'Star Vote Awarded');
INSERT INTO #lookups (Id, Name) VALUES (7, N'What Happened Next');

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
 IF OBJECT_ID('FK_SystemConfiguration_SystemEnvironment', 'F') IS NULL ALTER TABLE [SystemConfiguration] ADD CONSTRAINT FK_SystemConfiguration_SystemEnvironment FOREIGN KEY ([SystemEnvironment]) REFERENCES [Enum_SystemEnvironment] (Id);
 IF OBJECT_ID('FK_UserPoint_PointType', 'F') IS NULL ALTER TABLE [UserPoint] ADD CONSTRAINT FK_UserPoint_PointType FOREIGN KEY ([PointType]) REFERENCES [Enum_PointType] (Id);


commit;


SET IDENTITY_INSERT [dbo].[PointConfiguration] ON 

GO
INSERT [dbo].[PointConfiguration] ([Id], [PointType], [Name], [Description], [Points]) VALUES (5, 5, N'Vote Cast', N'Points are awarded for casting a vote for someone else''s answer. Points are awarded immediately.', 100)
GO
INSERT [dbo].[PointConfiguration] ([Id], [PointType], [Name], [Description], [Points]) VALUES (6, 6, N'Star Vote Awarded', N'Points are awarded for awarding the star vote on your dilemma. Points are awarded immediately.', 100)
GO
INSERT [dbo].[PointConfiguration] ([Id], [PointType], [Name], [Description], [Points]) VALUES (7, 7, N'What Happened Next', N'Points are awarded for telling the people that answered, voted, and bookmarked what course of action you took. Points are awarded immediately.', 100)
GO
SET IDENTITY_INSERT [dbo].[PointConfiguration] OFF
GO
