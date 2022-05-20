USE [ProjectManagement-test]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Course](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](120) NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[CreatedBy] [nvarchar](128) NOT NULL,
	[IsActive] [bit] NULL,
 CONSTRAINT [PK_Course] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Course]  WITH CHECK ADD  CONSTRAINT [FK_Course_AspNetUsers] FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO

ALTER TABLE [dbo].[Course] CHECK CONSTRAINT [FK_Course_AspNetUsers]
GO


CREATE TABLE [dbo].[UserEducation](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [nvarchar](128) NOT NULL,
	CourseId int NOT NULL,
	Percentage Decimal(18,2) NULL,
	YearPassed Nvarchar(4) NULL,
	Comment NVARCHAR(MAX) NULL
	
 CONSTRAINT [PK_UserEducation] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[UserEducation]  WITH CHECK ADD  CONSTRAINT [FK_UserEducation_AspNetUsers] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO

ALTER TABLE [dbo].[UserEducation] CHECK CONSTRAINT [FK_UserEducation_AspNetUsers]
GO
ALTER TABLE [dbo].[UserEducation]  WITH CHECK ADD  CONSTRAINT [FK_UserEducation_Course] FOREIGN KEY([CourseId])
REFERENCES [dbo].[Course] ([Id])
GO

ALTER TABLE [dbo].[UserEducation] CHECK CONSTRAINT [FK_UserEducation_Course]
GO

INSERT INTO [dbo].[Course]
           ([Name]
           ,[CreatedOn]
           ,[CreatedBy]
           ,[IsActive])
     VALUES
           ('10th'
           ,GETDATE()
           ,'62ab2752-d0c8-4be7-ba16-59806c303cc2'
           ,0)
GO

INSERT INTO [dbo].[Course]
           ([Name]
           ,[CreatedOn]
           ,[CreatedBy]
           ,[IsActive])
     VALUES
           ('12th'
           ,GETDATE()
           ,'62ab2752-d0c8-4be7-ba16-59806c303cc2'
           ,0)
GO
INSERT INTO [dbo].[Course]
           ([Name]
           ,[CreatedOn]
           ,[CreatedBy]
           ,[IsActive])
     VALUES
           ('Graduate'
           ,GETDATE()
           ,'62ab2752-d0c8-4be7-ba16-59806c303cc2'
           ,0)
GO
INSERT INTO [dbo].[Course]
           ([Name]
           ,[CreatedOn]
           ,[CreatedBy]
           ,[IsActive])
     VALUES
           ('Post Graduate'
           ,GETDATE()
           ,'62ab2752-d0c8-4be7-ba16-59806c303cc2'
           ,0)
GO


