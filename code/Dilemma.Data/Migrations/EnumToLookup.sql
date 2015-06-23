﻿use dilemma
go

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