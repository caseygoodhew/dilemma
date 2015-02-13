CREATE UNIQUE INDEX IX_PointType ON PointConfiguration (PointType);
CREATE UNIQUE INDEX IX_ServerName ON ServerConfiguration (Name);
CREATE UNIQUE INDEX IX_UniqueVote ON Vote (User_UserId, Question_QuestionId);