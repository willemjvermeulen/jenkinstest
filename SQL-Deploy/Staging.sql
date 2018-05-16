ALTER TABLE [dbo].[Table_1] ADD [test] [nchar](10) NULL
ALTER TABLE [dbo].[Table_1] ADD [another] [nchar](10) NULL
'sqlcmd -S DESKTOP-NS5M003 -U sa -P Annette4me -d Staging -i E:\REPO\jenkins-test\jenkinstest\SQL-Deploy\Staging.sql