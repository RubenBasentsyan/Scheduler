﻿/*
Deployment script for C:\USERS\ADMIN\SOURCE\REPOS\SCHEDULER\WEBAPPLICATION1\APP_DATA\SCHEDULE.MDF

This code was generated by a tool.
Changes to this file may cause incorrect behavior and will be lost if
the code is regenerated.
*/

GO
SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, CONCAT_NULL_YIELDS_NULL, QUOTED_IDENTIFIER ON;

SET NUMERIC_ROUNDABORT OFF;


GO
:setvar DatabaseName "C:\USERS\ADMIN\SOURCE\REPOS\SCHEDULER\WEBAPPLICATION1\APP_DATA\SCHEDULE.MDF"
:setvar DefaultFilePrefix "C_\USERS\ADMIN\SOURCE\REPOS\SCHEDULER\WEBAPPLICATION1\APP_DATA\SCHEDULE.MDF_"
:setvar DefaultDataPath "C:\Users\Admin\AppData\Local\Microsoft\Microsoft SQL Server Local DB\Instances\MSSQLLocalDB\"
:setvar DefaultLogPath "C:\Users\Admin\AppData\Local\Microsoft\Microsoft SQL Server Local DB\Instances\MSSQLLocalDB\"

GO
:on error exit
GO
/*
Detect SQLCMD mode and disable script execution if SQLCMD mode is not supported.
To re-enable the script after enabling SQLCMD mode, execute the following:
SET NOEXEC OFF; 
*/
:setvar __IsSqlCmdEnabled "True"
GO
IF N'$(__IsSqlCmdEnabled)' NOT LIKE N'True'
    BEGIN
        PRINT N'SQLCMD mode must be enabled to successfully execute this script.';
        SET NOEXEC ON;
    END


GO
USE [$(DatabaseName)];


GO
PRINT N'Creating [dbo].[Colors]...';


GO
CREATE TABLE [dbo].[Colors] (
    [Course_Fk] INT NOT NULL,
    [Day]       INT NULL,
    [TimeSlot]  INT NULL,
    CONSTRAINT [PK_Colors] PRIMARY KEY CLUSTERED ([Course_Fk] ASC)
);


GO
PRINT N'Creating [dbo].[Courses]...';


GO
CREATE TABLE [dbo].[Courses] (
    [Id]   INT          IDENTITY (1, 1) NOT NULL,
    [Name] VARCHAR (64) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'Creating [dbo].[Courses].[IX_Courses_Id]...';


GO
CREATE NONCLUSTERED INDEX [IX_Courses_Id]
    ON [dbo].[Courses]([Id] ASC);


GO
PRINT N'Creating [dbo].[Entrollments]...';


GO
CREATE TABLE [dbo].[Entrollments] (
    [Person_Fk]    INT NOT NULL,
    [Course_Fk]    INT NOT NULL,
    [EnrollmentId] INT IDENTITY (1, 1) NOT NULL,
    PRIMARY KEY CLUSTERED ([EnrollmentId] ASC)
);


GO
PRINT N'Creating [dbo].[Persons]...';


GO
CREATE TABLE [dbo].[Persons] (
    [Id]       INT          IDENTITY (1, 1) NOT NULL,
    [Name]     VARCHAR (64) NOT NULL,
    [Username] VARCHAR (64) NOT NULL,
    [Password] VARCHAR (64) NOT NULL,
    [IsAdmin]  BIT          NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    UNIQUE NONCLUSTERED ([Username] ASC)
);


GO
PRINT N'Creating unnamed constraint on [dbo].[Persons]...';


GO
ALTER TABLE [dbo].[Persons]
    ADD DEFAULT ((0)) FOR [IsAdmin];


GO
PRINT N'Creating [dbo].[FK_Colors_ToCourses]...';


GO
ALTER TABLE [dbo].[Colors] WITH NOCHECK
    ADD CONSTRAINT [FK_Colors_ToCourses] FOREIGN KEY ([Course_Fk]) REFERENCES [dbo].[Courses] ([Id]) ON DELETE CASCADE;


GO
PRINT N'Creating [dbo].[FK_Entrollments_ToCourses]...';


GO
ALTER TABLE [dbo].[Entrollments] WITH NOCHECK
    ADD CONSTRAINT [FK_Entrollments_ToCourses] FOREIGN KEY ([Course_Fk]) REFERENCES [dbo].[Courses] ([Id]) ON DELETE CASCADE;


GO
PRINT N'Creating [dbo].[FK_Entrollments_ToPersons]...';


GO
ALTER TABLE [dbo].[Entrollments] WITH NOCHECK
    ADD CONSTRAINT [FK_Entrollments_ToPersons] FOREIGN KEY ([Person_Fk]) REFERENCES [dbo].[Persons] ([Id]) ON DELETE CASCADE;


GO
PRINT N'Checking existing data against newly created constraints';


GO
USE [$(DatabaseName)];


GO
ALTER TABLE [dbo].[Colors] WITH CHECK CHECK CONSTRAINT [FK_Colors_ToCourses];

ALTER TABLE [dbo].[Entrollments] WITH CHECK CHECK CONSTRAINT [FK_Entrollments_ToCourses];

ALTER TABLE [dbo].[Entrollments] WITH CHECK CHECK CONSTRAINT [FK_Entrollments_ToPersons];


GO
PRINT N'Update complete.';


GO
