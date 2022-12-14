USE [DynamicReportLoader]
GO
/****** Object:  Table [dbo].[Country_s]    Script Date: 27/09/2022 9:15:36 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Country_s](
	[Id] [int] NULL,
	[Name] [varchar](50) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SystemReport_s]    Script Date: 27/09/2022 9:15:36 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SystemReport_s](
	[Id] [int] NOT NULL,
	[ReportTitle] [varchar](50) NULL,
	[IsFunction] [bit] NULL,
	[StoredProcedureName] [varchar](50) NULL,
	[InputStructure] [xml] NULL,
	[OutputStructure] [xml] NULL,
	[IsActive] [bit] NULL,
 CONSTRAINT [PK_SystemReport_s] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  StoredProcedure [dbo].[GetRep]    Script Date: 27/09/2022 9:15:36 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[GetRep]
(
@ID BIGINT,
@Name VARCHAR(50),
@Country INT,
@BirthDate DATETIME,
@Attended BIT,
@Enum INT,
@From DATETIME,
@To DATETIME,
@ClientId INT
 --{rest of params}
)
AS
BEGIN
 SELECT  '1' AS ID, 'NAME 1' AS [Name], 'Country Name' AS Country, GETDATE() AS BirthDate, 1 AS Attended, 'Enum' AS Enum
END
GO
