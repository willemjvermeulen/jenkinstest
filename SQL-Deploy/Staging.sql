IF not EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES 
           WHERE TABLE_NAME = N'Table_1')
BEGIN
CREATE TABLE [dbo].[Table_1] (
   [id] [int] NULL,
   [name] [nchar](10) NULL
)
END
else
BEGIN
print 'table exists Table_1'
end



INSERT INTO [Staging].[dbo].[Table_1]
           ([id]
           ,[name])
     VALUES
           (1
           ,'test')
GO

INSERT INTO [Staging].[dbo].[Table_1]
           ([id]
           ,[name])
     VALUES
           (2
           ,'test')
           GO
           INSERT INTO [Staging].[dbo].[Table_1]
           ([id]
           ,[name])
     VALUES
           (3
           ,'test')
           go


GO


'sqlcmd -S DESKTOP-NS5M003 -U sa -P Annette4me -d Staging -i E:\REPO\jenkins-test\jenkinstest\SQL-Deploy\Staging.sql