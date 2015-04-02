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

	INSERT INTO VoteCount
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
		 
	 DELETE FROM VoteCount
	  WHERE Id NOT IN (
			SELECT a.Id
			  FROM VoteCount a, (
					SELECT QuestionId, MAX(Votes) MaxVotes
					  FROM VoteCount
					 GROUP BY QuestionId) b
			 WHERE a.QuestionId = b.QuestionId
			   AND a.Votes = b.MaxVotes)
	
	UPDATE u
	   SET u.HistoricPopularVotes = u.HistoricPopularVotes + r.PopularVoteCount
	  FROM [User] u
	  INNER JOIN (
			SELECT a.User_UserId UserId, COUNT(*) PopularVoteCount
			  FROM VoteCount vc, Answer a
			 WHERE vc.QuestionId = a.Question_QuestionId
			   AND vc.AnswerId = a.AnswerId
			 GROUP BY a.User_UserId) r
	         ON u.UserId = r.UserId
	
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
   
    -- UPDATE USER POINTS
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
	DELETE FROM VoteCount;

	UPDATE LastRunLog SET RetireOldQuestions = @DateTimeNow;
GO




