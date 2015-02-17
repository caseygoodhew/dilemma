CREATE UNIQUE INDEX IX_PointType ON PointConfiguration (PointType);
CREATE UNIQUE INDEX IX_ServerName ON ServerConfiguration (Name);
CREATE UNIQUE INDEX IX_UniqueVote ON Vote (User_UserId, Question_QuestionId);







INSERT INTO QuestionRetirement
(QuestionId)
SELECT QuestionId
  FROM Question
 WHERE DATEADD(dd, 14, ClosesDateTime) < GETDATE()

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
UPDATE User
   SET User.HistoricPoints = User.HistoricPoints + UserPointRetirement.TotalPoints
  FROM User
 INNER JOIN UserPointRetirement
    ON User.UserId = UserPointRetirement.UserId
    
DELETE FROM UserPointRetirement
 WHERE Question_QuestionId IN (SELECT QuestionId FROM QuestionRetirement) 

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
 WHERE Moderation_ModerationId IN (SELECT ModerationId FROM ModerationRetirement)

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




