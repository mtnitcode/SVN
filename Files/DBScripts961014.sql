USE [SVN]
GO
/****** Object:  Table [dbo].[Branch]    Script Date: 01/09/2018 14:02:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Branch](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](500) NULL,
	[DeveloperID] [int] NULL,
	[CreateDate] [date] NULL,
	[ProjectVersionID] [int] NULL,
	[LocalPath] [nvarchar](1000) NULL,
 CONSTRAINT [PK_Branch] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[BranchFile]    Script Date: 01/09/2018 14:02:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[BranchFile](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[DeveloperID] [int] NOT NULL,
	[FileContent] [varbinary](max) NOT NULL,
	[EditionDate] [datetime] NOT NULL,
	[ProjectContentID] [bigint] NOT NULL,
	[BranchID] [int] NOT NULL,
	[ApplyToProjectVersion] [bit] NOT NULL,
 CONSTRAINT [PK_BranchVersion] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Developer]    Script Date: 01/09/2018 14:02:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Developer](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NULL,
	[Password] [varchar](50) NULL,
 CONSTRAINT [PK_Account] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Project]    Script Date: 01/09/2018 14:02:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Project](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](500) NULL,
	[SolutionID] [bigint] NULL,
 CONSTRAINT [PK_Project] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ProjectAccessright]    Script Date: 01/09/2018 14:02:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProjectAccessright](
	[DeveloperID] [int] NULL,
	[ProjectVersionID] [int] NULL,
	[AccessrightType] [int] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ProjectContent]    Script Date: 01/09/2018 14:02:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ProjectContent](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[ContentPath] [varchar](500) NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[CreatorDeveloperID] [int] NOT NULL,
	[ProjectVersionID] [int] NOT NULL,
	[Deleted] [bit] NULL,
 CONSTRAINT [PK_ProjectContent] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ProjectVersion]    Script Date: 01/09/2018 14:02:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProjectVersion](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[ProjectID] [int] NULL,
	[VersionNumber] [int] NULL,
	[VersionDate] [date] NULL,
 CONSTRAINT [PK_ProjectVersion] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ProjectVersionFile]    Script Date: 01/09/2018 14:02:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ProjectVersionFile](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[FileContent] [varbinary](max) NOT NULL,
	[ProjectContentID] [bigint] NOT NULL,
	[ProjectVersionID] [int] NOT NULL,
	[editiondate] [datetime] NOT NULL,
 CONSTRAINT [PK_ProjectVersionFile] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Solution]    Script Date: 01/09/2018 14:02:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Solution](
	[ID] [int] NOT NULL,
	[Name] [varchar](500) NOT NULL,
 CONSTRAINT [PK_Solution] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  StoredProcedure [mtnitcom].[FreeTransactionLog]    Script Date: 01/09/2018 14:02:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [mtnitcom].[FreeTransactionLog]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;


-- Truncate the log by changing the database recovery model to SIMPLE.
ALTER DATABASE SVN
SET RECOVERY SIMPLE;

-- Shrink the truncated log file to 1 MB.
DBCC SHRINKFILE (SVN_log, 1);

-- Reset the database recovery model.
ALTER DATABASE SVN
SET RECOVERY FULL;

END

GO
