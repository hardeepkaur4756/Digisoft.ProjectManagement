USE [ProjectManagement-test]
GO

/****** Object:  Table [dbo].[UserIncrement]    Script Date: 20-05-2022 12:15:53 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[UserIncrement](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [nvarchar](128) NOT NULL,
	[DateOfIncrement] [date] NULL,
	[Salary] [decimal](18, 2) NULL,
 CONSTRAINT [PK_UserIncrement] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[UserIncrement]  WITH CHECK ADD  CONSTRAINT [FK_UserIncrement_AspNetUsers] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO

ALTER TABLE [dbo].[UserIncrement] CHECK CONSTRAINT [FK_UserIncrement_AspNetUsers]
GO

