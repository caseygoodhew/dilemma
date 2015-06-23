

/*********************************************************************************/
/*  LOGGING                                                                      */
/* http://justinpdavis.blogspot.co.uk/2010/04/logging-to-database-with-nlog.html */
/*********************************************************************************/

use dilemma
go

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
