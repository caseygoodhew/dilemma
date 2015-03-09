USE dilemma
GO

CREATE UNIQUE INDEX IX_PointType ON PointConfiguration (PointType);
GO

CREATE UNIQUE INDEX IX_ServerName ON ServerConfiguration (Name);
GO

CREATE UNIQUE INDEX IX_UniqueVote ON Vote (User_UserId, Question_QuestionId);
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

IF OBJECT_ID ( 'RetireOldQuestions', 'P' ) IS NOT NULL 
    DROP PROCEDURE RetireOldQuestions;
GO

CREATE PROCEDURE RetireOldQuestions
	@DateTimeNow datetime2 = null
AS
    SET @DateTimeNow = dbo.TimeSourceNow(@DateTimeNow)
	
	SET NOCOUNT ON;

    INSERT INTO QuestionRetirement
    (QuestionId)
    SELECT q.QuestionId
      FROM Question q, SystemConfiguration sc
     WHERE q.ClosedDateTime IS NOT NULL
	   AND DATEADD(dd, sc.RetireQuestionAfterDays, q.ClosedDateTime) < @DateTimeNow

    INSERT INTO UserPointRetirement
    (UserId, TotalPoints)
    SELECT up.ForUser_UserId, SUM(up.PointsAwarded)
      FROM UserPoint up, QuestionRetirement qr
     WHERE up.RelatedQuestion_QuestionId = qr.QuestionId
     GROUP BY up.ForUser_UserId

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
   
    -- UPDATE USR POINTS
    UPDATE u
       SET u.HistoricPoints = u.HistoricPoints + upr.TotalPoints
      FROM [User] u
     INNER JOIN UserPointRetirement upr
        ON u.UserId = upr.UserId
    
    DELETE FROM UserPoint
     WHERE RelatedQuestion_QuestionId IN (SELECT QuestionId FROM QuestionRetirement) 

    -- CLEAN UP RELATIONSHIPS
    DELETE FROM Notification
     WHERE Moderation_ModerationId IN (SELECT ModerationId FROM ModerationRetirement)
        OR Question_QuestionId IN (SELECT QuestionId FROM QuestionRetirement)
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
 
    DELETE From Question
     WHERE QuestionId IN (SELECT QuestionId FROM QuestionRetirement)
 
    -- CLEAN UP TEMPORARY DATA
    DELETE FROM ModerationRetirement
    DELETE FROM UserPointRetirement
    DELETE FROM QuestionRetirement

	UPDATE LastRunLog SET RetireOldQuestions = @DateTimeNow;
GO




