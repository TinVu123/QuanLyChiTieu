USE [master]
GO
/****** Object:  Database [QuanLyChiTieu]    Script Date: 05/12/2024 8:13:47 PM ******/
CREATE DATABASE [QuanLyChiTieu]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'QuanLyChiTieu', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER\MSSQL\DATA\QuanLyChiTieu.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'QuanLyChiTieu_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER\MSSQL\DATA\QuanLyChiTieu_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [QuanLyChiTieu] SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [QuanLyChiTieu].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [QuanLyChiTieu] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [QuanLyChiTieu] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [QuanLyChiTieu] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [QuanLyChiTieu] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [QuanLyChiTieu] SET ARITHABORT OFF 
GO
ALTER DATABASE [QuanLyChiTieu] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [QuanLyChiTieu] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [QuanLyChiTieu] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [QuanLyChiTieu] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [QuanLyChiTieu] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [QuanLyChiTieu] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [QuanLyChiTieu] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [QuanLyChiTieu] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [QuanLyChiTieu] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [QuanLyChiTieu] SET  ENABLE_BROKER 
GO
ALTER DATABASE [QuanLyChiTieu] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [QuanLyChiTieu] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [QuanLyChiTieu] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [QuanLyChiTieu] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [QuanLyChiTieu] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [QuanLyChiTieu] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [QuanLyChiTieu] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [QuanLyChiTieu] SET RECOVERY FULL 
GO
ALTER DATABASE [QuanLyChiTieu] SET  MULTI_USER 
GO
ALTER DATABASE [QuanLyChiTieu] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [QuanLyChiTieu] SET DB_CHAINING OFF 
GO
ALTER DATABASE [QuanLyChiTieu] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [QuanLyChiTieu] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [QuanLyChiTieu] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [QuanLyChiTieu] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'QuanLyChiTieu', N'ON'
GO
ALTER DATABASE [QuanLyChiTieu] SET QUERY_STORE = ON
GO
ALTER DATABASE [QuanLyChiTieu] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [QuanLyChiTieu]
GO
/****** Object:  Table [dbo].[ChiTieu]    Script Date: 05/12/2024 8:13:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ChiTieu](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[DanhMuc] [nvarchar](100) NULL,
	[SoTien] [decimal](18, 0) NOT NULL,
	[ThoiGian] [date] NULL,
	[ChiTiet] [nvarchar](200) NULL,
	[UserID] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DanhMucChi]    Script Date: 05/12/2024 8:13:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DanhMucChi](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[TenMucChi] [nvarchar](100) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DanhMucThu]    Script Date: 05/12/2024 8:13:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DanhMucThu](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[TenMucThu] [nvarchar](100) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ThuNhap]    Script Date: 05/12/2024 8:13:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ThuNhap](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[DanhMuc] [nvarchar](100) NULL,
	[SoTien] [decimal](18, 0) NOT NULL,
	[ThoiGian] [date] NULL,
	[ChiTiet] [nvarchar](200) NULL,
	[UserID] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 05/12/2024 8:13:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[EmailorPhone] [nvarchar](100) NULL,
	[Password] [nvarchar](100) NULL,
	[Username] [nvarchar](100) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[ChiTieu] ON 

INSERT [dbo].[ChiTieu] ([ID], [DanhMuc], [SoTien], [ThoiGian], [ChiTiet], [UserID]) VALUES (2, N'Hoá đơn', CAST(900 AS Decimal(18, 0)), CAST(N'2024-10-24' AS Date), N'Tiền điện', 1)
INSERT [dbo].[ChiTieu] ([ID], [DanhMuc], [SoTien], [ThoiGian], [ChiTiet], [UserID]) VALUES (3, N'Mua sắm', CAST(1900000 AS Decimal(18, 0)), CAST(N'2024-10-22' AS Date), N'Mua nồi cơm điện', 1)
INSERT [dbo].[ChiTieu] ([ID], [DanhMuc], [SoTien], [ThoiGian], [ChiTiet], [UserID]) VALUES (4, N'Giải trí', CAST(200000 AS Decimal(18, 0)), CAST(N'2024-10-20' AS Date), N'Xem phim', 1)
INSERT [dbo].[ChiTieu] ([ID], [DanhMuc], [SoTien], [ThoiGian], [ChiTiet], [UserID]) VALUES (5, N'Du lịch', CAST(9900000 AS Decimal(18, 0)), CAST(N'2024-09-24' AS Date), N'Đi biển', 1)
INSERT [dbo].[ChiTieu] ([ID], [DanhMuc], [SoTien], [ThoiGian], [ChiTiet], [UserID]) VALUES (6, N'Ăn uống', CAST(900000 AS Decimal(18, 0)), CAST(N'2024-08-24' AS Date), N'Gà cháy đen', 1)
INSERT [dbo].[ChiTieu] ([ID], [DanhMuc], [SoTien], [ThoiGian], [ChiTiet], [UserID]) VALUES (7, N'Đầu tư', CAST(9900000 AS Decimal(18, 0)), CAST(N'2024-07-24' AS Date), N'Mua Vàng', 1)
INSERT [dbo].[ChiTieu] ([ID], [DanhMuc], [SoTien], [ThoiGian], [ChiTiet], [UserID]) VALUES (8, N'Sức khoẻ', CAST(3900000 AS Decimal(18, 0)), CAST(N'2024-10-24' AS Date), N'Khám định kỳ', 1)
INSERT [dbo].[ChiTieu] ([ID], [DanhMuc], [SoTien], [ThoiGian], [ChiTiet], [UserID]) VALUES (9, N'Học phí', CAST(2900000 AS Decimal(18, 0)), CAST(N'2024-08-24' AS Date), N'Học tiếng anh', 1)
INSERT [dbo].[ChiTieu] ([ID], [DanhMuc], [SoTien], [ThoiGian], [ChiTiet], [UserID]) VALUES (10, N'Đầu tư', CAST(23000030 AS Decimal(18, 0)), CAST(N'2024-06-24' AS Date), N'Mua cổ vịt', 1)
INSERT [dbo].[ChiTieu] ([ID], [DanhMuc], [SoTien], [ThoiGian], [ChiTiet], [UserID]) VALUES (11, N'Hoá đơn', CAST(400000 AS Decimal(18, 0)), CAST(N'2024-05-24' AS Date), N'Tiền nước', 1)
INSERT [dbo].[ChiTieu] ([ID], [DanhMuc], [SoTien], [ThoiGian], [ChiTiet], [UserID]) VALUES (12, N'Ăn uống', CAST(900000 AS Decimal(18, 0)), CAST(N'2024-05-24' AS Date), N'Thịt cầy', 1)
INSERT [dbo].[ChiTieu] ([ID], [DanhMuc], [SoTien], [ThoiGian], [ChiTiet], [UserID]) VALUES (13, N'Học phí', CAST(2900000 AS Decimal(18, 0)), CAST(N'2024-08-24' AS Date), N'Học tiếng anh', 1)
INSERT [dbo].[ChiTieu] ([ID], [DanhMuc], [SoTien], [ThoiGian], [ChiTiet], [UserID]) VALUES (14, N'Đầu tư', CAST(2300000 AS Decimal(18, 0)), CAST(N'2024-04-24' AS Date), N'Mua cổ phiếu', 1)
INSERT [dbo].[ChiTieu] ([ID], [DanhMuc], [SoTien], [ThoiGian], [ChiTiet], [UserID]) VALUES (15, N'Hoá đơn', CAST(400000 AS Decimal(18, 0)), CAST(N'2024-04-24' AS Date), N'Tiền nước', 1)
INSERT [dbo].[ChiTieu] ([ID], [DanhMuc], [SoTien], [ThoiGian], [ChiTiet], [UserID]) VALUES (17, N'Học phí', CAST(2900000 AS Decimal(18, 0)), CAST(N'2024-02-24' AS Date), N'Học tiếng anh', 1)
INSERT [dbo].[ChiTieu] ([ID], [DanhMuc], [SoTien], [ThoiGian], [ChiTiet], [UserID]) VALUES (18, N'Đầu tư', CAST(2300000 AS Decimal(18, 0)), CAST(N'2024-02-24' AS Date), N'Mua cổ phiếu', 1)
INSERT [dbo].[ChiTieu] ([ID], [DanhMuc], [SoTien], [ThoiGian], [ChiTiet], [UserID]) VALUES (19, N'Hoá đơn', CAST(400000 AS Decimal(18, 0)), CAST(N'2024-01-24' AS Date), N'Tiền nước', 1)
INSERT [dbo].[ChiTieu] ([ID], [DanhMuc], [SoTien], [ThoiGian], [ChiTiet], [UserID]) VALUES (20, N'Ăn uống', CAST(2 AS Decimal(18, 0)), CAST(N'2024-01-24' AS Date), N'Thịt cầy', 1)
INSERT [dbo].[ChiTieu] ([ID], [DanhMuc], [SoTien], [ThoiGian], [ChiTiet], [UserID]) VALUES (40, N'Hoá đơn', CAST(1000000 AS Decimal(18, 0)), CAST(N'2024-11-17' AS Date), N'Tiền điện', 1)
SET IDENTITY_INSERT [dbo].[ChiTieu] OFF
GO
SET IDENTITY_INSERT [dbo].[DanhMucChi] ON 

INSERT [dbo].[DanhMucChi] ([ID], [TenMucChi]) VALUES (1, N'Hoá đơn')
INSERT [dbo].[DanhMucChi] ([ID], [TenMucChi]) VALUES (2, N'Đầu tư')
INSERT [dbo].[DanhMucChi] ([ID], [TenMucChi]) VALUES (3, N'Sức khoẻ')
INSERT [dbo].[DanhMucChi] ([ID], [TenMucChi]) VALUES (4, N'Giải trí')
INSERT [dbo].[DanhMucChi] ([ID], [TenMucChi]) VALUES (5, N'Mua sắm')
INSERT [dbo].[DanhMucChi] ([ID], [TenMucChi]) VALUES (6, N'Du lịch')
INSERT [dbo].[DanhMucChi] ([ID], [TenMucChi]) VALUES (7, N'Ăn uống')
INSERT [dbo].[DanhMucChi] ([ID], [TenMucChi]) VALUES (8, N'Học phí')
INSERT [dbo].[DanhMucChi] ([ID], [TenMucChi]) VALUES (9, N'Khác')
INSERT [dbo].[DanhMucChi] ([ID], [TenMucChi]) VALUES (10, N'Null')
SET IDENTITY_INSERT [dbo].[DanhMucChi] OFF
GO
SET IDENTITY_INSERT [dbo].[DanhMucThu] ON 

INSERT [dbo].[DanhMucThu] ([ID], [TenMucThu]) VALUES (1, N'Tiền lương')
INSERT [dbo].[DanhMucThu] ([ID], [TenMucThu]) VALUES (2, N'Tiền thưởng')
INSERT [dbo].[DanhMucThu] ([ID], [TenMucThu]) VALUES (3, N'Tiền đầu tư')
INSERT [dbo].[DanhMucThu] ([ID], [TenMucThu]) VALUES (4, N'Tiền bán sản phẩm')
INSERT [dbo].[DanhMucThu] ([ID], [TenMucThu]) VALUES (5, N'Khác')
SET IDENTITY_INSERT [dbo].[DanhMucThu] OFF
GO
SET IDENTITY_INSERT [dbo].[ThuNhap] ON 

INSERT [dbo].[ThuNhap] ([ID], [DanhMuc], [SoTien], [ThoiGian], [ChiTiet], [UserID]) VALUES (1, N'Tiền lương', CAST(11900000 AS Decimal(18, 0)), CAST(N'2024-09-30' AS Date), N'', 1)
INSERT [dbo].[ThuNhap] ([ID], [DanhMuc], [SoTien], [ThoiGian], [ChiTiet], [UserID]) VALUES (2, N'Tiền lương', CAST(11500000 AS Decimal(18, 0)), CAST(N'2024-08-30' AS Date), N'Đi làm muộn 2 hôm đi làm muộn 2 hôm đi làm muộn 2 hôm', 1)
INSERT [dbo].[ThuNhap] ([ID], [DanhMuc], [SoTien], [ThoiGian], [ChiTiet], [UserID]) VALUES (4, N'Tiền lương', CAST(11900000 AS Decimal(18, 0)), CAST(N'2024-06-30' AS Date), N'test1', 1)
INSERT [dbo].[ThuNhap] ([ID], [DanhMuc], [SoTien], [ThoiGian], [ChiTiet], [UserID]) VALUES (5, N'Tiền lương', CAST(11900000 AS Decimal(18, 0)), CAST(N'2024-05-30' AS Date), N'', 1)
INSERT [dbo].[ThuNhap] ([ID], [DanhMuc], [SoTien], [ThoiGian], [ChiTiet], [UserID]) VALUES (6, N'Tiền lương', CAST(11900000 AS Decimal(18, 0)), CAST(N'2024-04-30' AS Date), N'', 1)
INSERT [dbo].[ThuNhap] ([ID], [DanhMuc], [SoTien], [ThoiGian], [ChiTiet], [UserID]) VALUES (7, N'Tiền lương', CAST(9900000 AS Decimal(18, 0)), CAST(N'2024-03-30' AS Date), N'', 1)
INSERT [dbo].[ThuNhap] ([ID], [DanhMuc], [SoTien], [ThoiGian], [ChiTiet], [UserID]) VALUES (8, N'Tiền lương', CAST(9900000 AS Decimal(18, 0)), CAST(N'2024-02-28' AS Date), N'Tăng lương', 1)
INSERT [dbo].[ThuNhap] ([ID], [DanhMuc], [SoTien], [ThoiGian], [ChiTiet], [UserID]) VALUES (9, N'Tiền lương', CAST(8900000 AS Decimal(18, 0)), CAST(N'2024-01-30' AS Date), N'Mua nồi cơm điện', 1)
INSERT [dbo].[ThuNhap] ([ID], [DanhMuc], [SoTien], [ThoiGian], [ChiTiet], [UserID]) VALUES (11, N'Tiền Lãi', CAST(11500000 AS Decimal(18, 0)), CAST(N'2024-08-30' AS Date), N'', 1)
INSERT [dbo].[ThuNhap] ([ID], [DanhMuc], [SoTien], [ThoiGian], [ChiTiet], [UserID]) VALUES (16, N'Tiền lương', CAST(20000000 AS Decimal(18, 0)), CAST(N'2024-10-16' AS Date), N'tiền lương 1', 1)
INSERT [dbo].[ThuNhap] ([ID], [DanhMuc], [SoTien], [ThoiGian], [ChiTiet], [UserID]) VALUES (34, N'Tiền bán sản phẩm', CAST(1000000 AS Decimal(18, 0)), CAST(N'2024-11-17' AS Date), N'g', 1)
INSERT [dbo].[ThuNhap] ([ID], [DanhMuc], [SoTien], [ThoiGian], [ChiTiet], [UserID]) VALUES (35, N'Tiền đầu tư', CAST(1000000 AS Decimal(18, 0)), CAST(N'2024-11-17' AS Date), N'g', 1)
INSERT [dbo].[ThuNhap] ([ID], [DanhMuc], [SoTien], [ThoiGian], [ChiTiet], [UserID]) VALUES (36, N'Khác', CAST(6000000 AS Decimal(18, 0)), CAST(N'2024-07-16' AS Date), N'', 1)
SET IDENTITY_INSERT [dbo].[ThuNhap] OFF
GO
SET IDENTITY_INSERT [dbo].[Users] ON 

INSERT [dbo].[Users] ([ID], [EmailorPhone], [Password], [Username]) VALUES (1, N'admin@gmail.com', N'admin', N'admin')
INSERT [dbo].[Users] ([ID], [EmailorPhone], [Password], [Username]) VALUES (9, N'tin@gmail.com', N'111', N'vvtin')
SET IDENTITY_INSERT [dbo].[Users] OFF
GO
ALTER TABLE [dbo].[ChiTieu]  WITH CHECK ADD FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([ID])
GO
ALTER TABLE [dbo].[ThuNhap]  WITH CHECK ADD FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([ID])
GO
/****** Object:  StoredProcedure [dbo].[LayChiTieu]    Script Date: 05/12/2024 8:13:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[LayChiTieu] @userid int, @month int, @year int
AS
SELECT * FROM [dbo].[ChiTieu]
WHERE MONTH([ThoiGian]) = @month AND YEAR([ThoiGian]) = @year AND [UserID] = @userid
GO
/****** Object:  StoredProcedure [dbo].[LayThuNhap]    Script Date: 05/12/2024 8:13:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[LayThuNhap] @userid int, @month int, @year int
AS
SELECT * FROM [dbo].[ThuNhap]
WHERE MONTH([ThoiGian]) = @month AND YEAR([ThoiGian]) = @year AND [UserID] = @userid
GO
/****** Object:  StoredProcedure [dbo].[LocTienChi]    Script Date: 05/12/2024 8:13:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[LocTienChi] (@startdate Date, @enddate Date, @userid INT)
AS 
SELECT * FROM [dbo].[ChiTieu]
					WHERE [ThoiGian] BETWEEN @startdate AND  @enddate   
					AND [UserID] = userid
GO
/****** Object:  StoredProcedure [dbo].[LocTienThu]    Script Date: 05/12/2024 8:13:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[LocTienThu] (@startdate Date, @enddate Date, @userid INT)
AS 
SELECT * FROM [dbo].[ThuNhap]
					WHERE [ThoiGian] BETWEEN @startdate AND  @enddate   
					AND [UserID] = userid
GO
/****** Object:  StoredProcedure [dbo].[TinhTongTienChi]    Script Date: 05/12/2024 8:13:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[TinhTongTienChi] (@month INT, @year INT, @userid INT)
AS
BEGIN
  DECLARE @Tong DECIMAL
  SELECT @Tong =  SUM(SoTien) FROM [dbo].[ChiTieu]
          WHERE MONTH([ThoiGian]) = @month AND Year([ThoiGian]) = @year AND [UserID] = @userid
          SELECT @Tong AS Tong
END;
GO
/****** Object:  StoredProcedure [dbo].[TinhTongTienThu]    Script Date: 05/12/2024 8:13:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[TinhTongTienThu] (@month INT, @year INT, @userid INT)
AS
BEGIN
  DECLARE @Tong DECIMAL
  SELECT @Tong =  SUM(SoTien) FROM [dbo].[ThuNhap]
          WHERE MONTH([ThoiGian]) = @month AND Year([ThoiGian]) = @year AND [UserID] = @userid
          SELECT @Tong AS Tong
END;
GO
USE [master]
GO
ALTER DATABASE [QuanLyChiTieu] SET  READ_WRITE 
GO
