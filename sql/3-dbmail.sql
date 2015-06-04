
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

