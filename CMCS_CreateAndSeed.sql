-- CMCS SQL Server schema + sample data (T-SQL)
-- Compatible with Microsoft SQL Server

IF DB_ID('CMCS') IS NULL
BEGIN
    CREATE DATABASE CMCS;
END
GO

USE CMCS;
GO

IF OBJECT_ID('dbo.Approvals', 'U') IS NOT NULL DROP TABLE dbo.Approvals;
IF OBJECT_ID('dbo.Attachments', 'U') IS NOT NULL DROP TABLE dbo.Attachments;
IF OBJECT_ID('dbo.ClaimEntries', 'U') IS NOT NULL DROP TABLE dbo.ClaimEntries;
IF OBJECT_ID('dbo.Claims', 'U') IS NOT NULL DROP TABLE dbo.Claims;
GO

CREATE TABLE dbo.Claims
(
    Id UNIQUEIDENTIFIER NOT NULL CONSTRAINT PK_Claims PRIMARY KEY,
    LecturerName NVARCHAR(200) NOT NULL,
    Month NVARCHAR(10) NOT NULL, -- stored as dd/MM/yyyy text
    HourlyRate DECIMAL(18,2) NOT NULL,
    TotalHours FLOAT NOT NULL,
    Status INT NOT NULL
);
GO

CREATE TABLE dbo.ClaimEntries
(
    Id UNIQUEIDENTIFIER NOT NULL CONSTRAINT PK_ClaimEntries PRIMARY KEY,
    ClaimId UNIQUEIDENTIFIER NOT NULL,
    WorkDate DATE NOT NULL,
    Module NVARCHAR(50) NOT NULL,
    TaskType NVARCHAR(100) NOT NULL,
    Hours FLOAT NOT NULL,
    Notes NVARCHAR(4000) NULL,
    CONSTRAINT FK_ClaimEntries_Claims FOREIGN KEY (ClaimId)
        REFERENCES dbo.Claims(Id) ON DELETE CASCADE
);
GO

CREATE TABLE dbo.Attachments
(
    Id UNIQUEIDENTIFIER NOT NULL CONSTRAINT PK_Attachments PRIMARY KEY,
    ClaimId UNIQUEIDENTIFIER NOT NULL,
    FileName NVARCHAR(260) NOT NULL,
    ContentType NVARCHAR(100) NOT NULL,
    FileSize BIGINT NOT NULL,
    StoragePath NVARCHAR(400) NOT NULL,
    UploadedAt DATETIME2 NOT NULL,
    CONSTRAINT FK_Attachments_Claims FOREIGN KEY (ClaimId)
        REFERENCES dbo.Claims(Id) ON DELETE CASCADE
);
GO

CREATE TABLE dbo.Approvals
(
    Id UNIQUEIDENTIFIER NOT NULL CONSTRAINT PK_Approvals PRIMARY KEY,
    ClaimId UNIQUEIDENTIFIER NOT NULL,
    ApproverRole NVARCHAR(50) NOT NULL,
    Decision NVARCHAR(20) NOT NULL,
    Comment NVARCHAR(1000) NULL,
    DecidedAt DATETIME2 NOT NULL,
    CONSTRAINT FK_Approvals_Claims FOREIGN KEY (ClaimId)
        REFERENCES dbo.Claims(Id) ON DELETE CASCADE
);
GO

-- Sample data
DECLARE @ClaimId UNIQUEIDENTIFIER = NEWID();

INSERT INTO dbo.Claims (Id, LecturerName, Month, HourlyRate, TotalHours, Status)
VALUES (@ClaimId, N'Demo Lecturer', CONVERT(NVARCHAR(10), GETDATE(), 103), 450.00, 4, 1);

INSERT INTO dbo.ClaimEntries (Id, ClaimId, WorkDate, Module, TaskType, Hours, Notes)
VALUES (NEWID(), @ClaimId, CAST(GETDATE() AS DATE), N'INF101', N'Teaching', 2, N'Demo row'),
       (NEWID(), @ClaimId, CAST(DATEADD(DAY, -1, GETDATE()) AS DATE), N'INF101', N'Marking', 2, N'Demo row');

INSERT INTO dbo.Approvals (Id, ClaimId, ApproverRole, Decision, Comment, DecidedAt)
VALUES (NEWID(), @ClaimId, N'PC/AM', N'Approved', N'Demo approval for script', SYSDATETIME());
GO
