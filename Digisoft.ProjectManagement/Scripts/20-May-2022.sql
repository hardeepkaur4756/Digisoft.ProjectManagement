Create Table UserAttendance
(
Id int Identity(1,1) NOT NULL,
[UserId] [nvarchar](128) NOT NULL,
Date DateTime NULL,
InTime Time NULL,
OutTime Time NULL,
OnLeave bit NULL,
LeaveApprovedBy NVARCHAR(128) NULL,
Comment NVARCHAR(MAX) NULL,
CreatedBy Nvarchar(128) NULL,
CreatedDate DateTime NULL,
 CONSTRAINT [PK_UserAttendance] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].UserAttendance  WITH CHECK ADD  CONSTRAINT [FK_UserAttendance_AspNetUsers] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO

ALTER TABLE [dbo].[UserAttendance] CHECK CONSTRAINT [FK_UserAttendance_AspNetUsers]
GO

