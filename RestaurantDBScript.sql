USE [master]
GO
/****** Object:  Database [RestaurantDB]    Script Date: 15.01.2022 04:26:07 ******/
CREATE DATABASE [RestaurantDB]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'RestaurantDB', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\RestaurantDB.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'RestaurantDB_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\RestaurantDB_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [RestaurantDB] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [RestaurantDB].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [RestaurantDB] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [RestaurantDB] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [RestaurantDB] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [RestaurantDB] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [RestaurantDB] SET ARITHABORT OFF 
GO
ALTER DATABASE [RestaurantDB] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [RestaurantDB] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [RestaurantDB] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [RestaurantDB] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [RestaurantDB] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [RestaurantDB] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [RestaurantDB] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [RestaurantDB] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [RestaurantDB] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [RestaurantDB] SET  DISABLE_BROKER 
GO
ALTER DATABASE [RestaurantDB] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [RestaurantDB] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [RestaurantDB] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [RestaurantDB] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [RestaurantDB] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [RestaurantDB] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [RestaurantDB] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [RestaurantDB] SET RECOVERY FULL 
GO
ALTER DATABASE [RestaurantDB] SET  MULTI_USER 
GO
ALTER DATABASE [RestaurantDB] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [RestaurantDB] SET DB_CHAINING OFF 
GO
ALTER DATABASE [RestaurantDB] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [RestaurantDB] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [RestaurantDB] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [RestaurantDB] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'RestaurantDB', N'ON'
GO
ALTER DATABASE [RestaurantDB] SET QUERY_STORE = OFF
GO
USE [RestaurantDB]
GO
/****** Object:  Table [dbo].[Cities]    Script Date: 15.01.2022 04:26:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Cities](
	[ID] [smallint] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
 CONSTRAINT [PK_Cities] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Ingredients]    Script Date: 15.01.2022 04:26:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Ingredients](
	[ID] [tinyint] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[Category] [tinyint] NOT NULL,
 CONSTRAINT [PK_Ingredients] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[IngredientsCategories]    Script Date: 15.01.2022 04:26:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[IngredientsCategories](
	[ID] [tinyint] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
 CONSTRAINT [PK_IngredientsCategories] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Meals]    Script Date: 15.01.2022 04:26:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Meals](
	[ID] [smallint] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[Price] [float] NOT NULL,
	[Description] [varchar](max) NOT NULL,
	[PicutreName] [varchar](50) NOT NULL,
	[Category] [tinyint] NOT NULL,
 CONSTRAINT [PK_Meals] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MealsCategories]    Script Date: 15.01.2022 04:26:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MealsCategories](
	[ID] [tinyint] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
 CONSTRAINT [PK_Categories] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Orders]    Script Date: 15.01.2022 04:26:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Orders](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[MealID] [smallint] NOT NULL,
	[Amount] [tinyint] NOT NULL,
 CONSTRAINT [PK_Orders] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OrdersDetails]    Script Date: 15.01.2022 04:26:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrdersDetails](
	[OrderID] [bigint] NOT NULL,
	[UserID] [bigint] NOT NULL,
	[CityID] [smallint] NOT NULL,
	[Address] [varchar](64) NOT NULL,
	[Phone] [varchar](16) NOT NULL,
	[DiscountCode] [varchar](32) NOT NULL,
	[Status] [tinyint] NOT NULL,
	[TotalPrice] [float] NOT NULL,
	[OrderDate] [smalldatetime] NOT NULL,
 CONSTRAINT [PK_OrdersDetails] PRIMARY KEY CLUSTERED 
(
	[OrderID] ASC,
	[UserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Recipes]    Script Date: 15.01.2022 04:26:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Recipes](
	[MealID] [smallint] NOT NULL,
	[IngredientID] [tinyint] NOT NULL,
 CONSTRAINT [PK_Recipes] PRIMARY KEY CLUSTERED 
(
	[MealID] ASC,
	[IngredientID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Roles]    Script Date: 15.01.2022 04:26:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Roles](
	[ID] [tinyint] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
 CONSTRAINT [PK_Roles] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 15.01.2022 04:26:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[Email] [varchar](50) NOT NULL,
	[Password] [varchar](255) NOT NULL,
	[IsActive] [bit] NOT NULL,
	[Role] [tinyint] NOT NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UsersDetails]    Script Date: 15.01.2022 04:26:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UsersDetails](
	[ID] [bigint] NOT NULL,
	[Name] [varchar](50) NULL,
	[Surname] [varchar](50) NULL,
	[Phone] [varchar](16) NULL,
	[CityID] [smallint] NULL,
	[Address] [varchar](64) NULL,
	[CreateDate] [smalldatetime] NOT NULL,
	[UpdateDate] [smalldatetime] NOT NULL,
 CONSTRAINT [PK_UsersDetails] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Ingredients]  WITH CHECK ADD  CONSTRAINT [FK_Ingredients_IngredientsCategories1] FOREIGN KEY([ID])
REFERENCES [dbo].[IngredientsCategories] ([ID])
GO
ALTER TABLE [dbo].[Ingredients] CHECK CONSTRAINT [FK_Ingredients_IngredientsCategories1]
GO
ALTER TABLE [dbo].[Meals]  WITH CHECK ADD  CONSTRAINT [FK_Meals_Categories] FOREIGN KEY([Category])
REFERENCES [dbo].[MealsCategories] ([ID])
GO
ALTER TABLE [dbo].[Meals] CHECK CONSTRAINT [FK_Meals_Categories]
GO
ALTER TABLE [dbo].[Orders]  WITH CHECK ADD  CONSTRAINT [FK_Orders_Meals] FOREIGN KEY([MealID])
REFERENCES [dbo].[Meals] ([ID])
GO
ALTER TABLE [dbo].[Orders] CHECK CONSTRAINT [FK_Orders_Meals]
GO
ALTER TABLE [dbo].[OrdersDetails]  WITH CHECK ADD  CONSTRAINT [FK_OrdersDetails_Cities] FOREIGN KEY([CityID])
REFERENCES [dbo].[Cities] ([ID])
GO
ALTER TABLE [dbo].[OrdersDetails] CHECK CONSTRAINT [FK_OrdersDetails_Cities]
GO
ALTER TABLE [dbo].[OrdersDetails]  WITH CHECK ADD  CONSTRAINT [FK_OrdersDetails_Orders] FOREIGN KEY([OrderID])
REFERENCES [dbo].[Orders] ([ID])
GO
ALTER TABLE [dbo].[OrdersDetails] CHECK CONSTRAINT [FK_OrdersDetails_Orders]
GO
ALTER TABLE [dbo].[OrdersDetails]  WITH CHECK ADD  CONSTRAINT [FK_OrdersDetails_Users] FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([ID])
GO
ALTER TABLE [dbo].[OrdersDetails] CHECK CONSTRAINT [FK_OrdersDetails_Users]
GO
ALTER TABLE [dbo].[Recipes]  WITH CHECK ADD  CONSTRAINT [FK_Recipes_Ingredients] FOREIGN KEY([IngredientID])
REFERENCES [dbo].[Ingredients] ([ID])
GO
ALTER TABLE [dbo].[Recipes] CHECK CONSTRAINT [FK_Recipes_Ingredients]
GO
ALTER TABLE [dbo].[Recipes]  WITH CHECK ADD  CONSTRAINT [FK_Recipes_Meals] FOREIGN KEY([MealID])
REFERENCES [dbo].[Meals] ([ID])
GO
ALTER TABLE [dbo].[Recipes] CHECK CONSTRAINT [FK_Recipes_Meals]
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD  CONSTRAINT [FK_Users_Roles] FOREIGN KEY([Role])
REFERENCES [dbo].[Roles] ([ID])
GO
ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [FK_Users_Roles]
GO
ALTER TABLE [dbo].[UsersDetails]  WITH CHECK ADD  CONSTRAINT [FK_UsersDetails_Cities] FOREIGN KEY([CityID])
REFERENCES [dbo].[Cities] ([ID])
GO
ALTER TABLE [dbo].[UsersDetails] CHECK CONSTRAINT [FK_UsersDetails_Cities]
GO
ALTER TABLE [dbo].[UsersDetails]  WITH CHECK ADD  CONSTRAINT [FK_UsersDetails_Users] FOREIGN KEY([ID])
REFERENCES [dbo].[Users] ([ID])
GO
ALTER TABLE [dbo].[UsersDetails] CHECK CONSTRAINT [FK_UsersDetails_Users]
GO
/****** Object:  Trigger [dbo].[TR_Users_AfterInsert_Date]    Script Date: 15.01.2022 04:26:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Patryk Zub
-- Create date: 03.01.2022
-- Description:	Insterts create and update date
-- =============================================
CREATE TRIGGER [dbo].[TR_Users_AfterInsert_Date] 
   ON  [dbo].[Users] 
   AFTER INSERT
AS 
	DECLARE @ID INT
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	-- Gets id of inserted user
	SELECT @ID = ins.ID FROM INSERTED ins;

    -- Insert statements for trigger here

INSERT INTO [dbo].[UsersDetails]
           ([ID]
           ,[CreateDate]
           ,[UpdateDate])
     VALUES
           (@ID
           ,GETDATE()
           ,GETDATE())

END
GO
ALTER TABLE [dbo].[Users] ENABLE TRIGGER [TR_Users_AfterInsert_Date]
GO
USE [master]
GO
ALTER DATABASE [RestaurantDB] SET  READ_WRITE 
GO
