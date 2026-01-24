USE [master]
GO
/****** Object:  Database [QuanLyVatTu]    Script Date: 21/01/2026 10:33:00 SA ******/
CREATE DATABASE [QuanLyVatTu]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'QuanLyVatTu', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER\MSSQL\DATA\QuanLyVatTu.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'QuanLyVatTu_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER\MSSQL\DATA\QuanLyVatTu_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [QuanLyVatTu] SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [QuanLyVatTu].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [QuanLyVatTu] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [QuanLyVatTu] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [QuanLyVatTu] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [QuanLyVatTu] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [QuanLyVatTu] SET ARITHABORT OFF 
GO
ALTER DATABASE [QuanLyVatTu] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [QuanLyVatTu] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [QuanLyVatTu] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [QuanLyVatTu] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [QuanLyVatTu] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [QuanLyVatTu] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [QuanLyVatTu] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [QuanLyVatTu] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [QuanLyVatTu] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [QuanLyVatTu] SET  ENABLE_BROKER 
GO
ALTER DATABASE [QuanLyVatTu] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [QuanLyVatTu] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [QuanLyVatTu] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [QuanLyVatTu] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [QuanLyVatTu] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [QuanLyVatTu] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [QuanLyVatTu] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [QuanLyVatTu] SET RECOVERY FULL 
GO
ALTER DATABASE [QuanLyVatTu] SET  MULTI_USER 
GO
ALTER DATABASE [QuanLyVatTu] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [QuanLyVatTu] SET DB_CHAINING OFF 
GO
ALTER DATABASE [QuanLyVatTu] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [QuanLyVatTu] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [QuanLyVatTu] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [QuanLyVatTu] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'QuanLyVatTu', N'ON'
GO
ALTER DATABASE [QuanLyVatTu] SET QUERY_STORE = ON
GO
ALTER DATABASE [QuanLyVatTu] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [QuanLyVatTu]
GO
/****** Object:  Table [dbo].[BAOCAOTON]    Script Date: 21/01/2026 10:33:00 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BAOCAOTON](
	[MaVatTu] [int] NOT NULL,
	[Thang] [int] NOT NULL,
	[Nam] [int] NOT NULL,
	[TonDau] [int] NULL,
	[PhatSinhNhap] [int] NULL,
	[PhatSinhXuat] [int] NULL,
	[TonCuoi] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[MaVatTu] ASC,
	[Thang] ASC,
	[Nam] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CT_HD]    Script Date: 21/01/2026 10:33:00 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CT_HD](
	[MaHoaDon] [int] NOT NULL,
	[MaVatTu] [int] NOT NULL,
	[MaDonViTinh] [int] NULL,
	[SoLuongBan] [int] NOT NULL,
	[DonGiaBan] [decimal](18, 0) NULL,
	[ThanhTien] [decimal](18, 0) NULL,
PRIMARY KEY CLUSTERED 
(
	[MaHoaDon] ASC,
	[MaVatTu] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CT_PNVT]    Script Date: 21/01/2026 10:33:00 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CT_PNVT](
	[MaPhieuNhap] [int] NOT NULL,
	[MaVatTu] [int] NOT NULL,
	[MaNhanVien] [int] NULL,
	[MaDonViTinh] [int] NULL,
	[SoLuong] [int] NOT NULL,
	[DonGiaNhap] [decimal](18, 0) NULL,
	[ThanhTien] [decimal](18, 0) NULL,
	[DonGiaBan] [decimal](18, 0) NULL,
PRIMARY KEY CLUSTERED 
(
	[MaPhieuNhap] ASC,
	[MaVatTu] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DONVITINH]    Script Date: 21/01/2026 10:33:00 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DONVITINH](
	[MaDonViTinh] [int] IDENTITY(1,1) NOT NULL,
	[TenDonViTinh] [nvarchar](50) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[MaDonViTinh] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[HOADON]    Script Date: 21/01/2026 10:33:00 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[HOADON](
	[MaHoaDon] [int] IDENTITY(1,1) NOT NULL,
	[MaKhachHang] [int] NULL,
	[MaNhanVien] [int] NULL,
	[NgayLapHoaDon] [datetime] NULL,
	[TongTien] [decimal](18, 0) NULL,
PRIMARY KEY CLUSTERED 
(
	[MaHoaDon] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[KHACHHANG]    Script Date: 21/01/2026 10:33:00 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[KHACHHANG](
	[MaKhachHang] [int] IDENTITY(1,1) NOT NULL,
	[HoVaTen] [nvarchar](100) NOT NULL,
	[SoDienThoai] [varchar](20) NULL,
	[DiaChi] [nvarchar](200) NULL,
	[GioiTinh] [nvarchar](10) NULL,
	[NgaySinh] [date] NULL,
PRIMARY KEY CLUSTERED 
(
	[MaKhachHang] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LOAIVATTU]    Script Date: 21/01/2026 10:33:00 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LOAIVATTU](
	[MaLoai] [int] IDENTITY(1,1) NOT NULL,
	[TenLoai] [nvarchar](100) NOT NULL,
	[MoTa] [nvarchar](255) NULL,
PRIMARY KEY CLUSTERED 
(
	[MaLoai] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[NHANVIEN]    Script Date: 21/01/2026 10:33:00 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NHANVIEN](
	[MaNhanVien] [int] IDENTITY(1,1) NOT NULL,
	[HoTen] [nvarchar](100) NOT NULL,
	[NgaySinh] [date] NULL,
	[SoDienThoai] [varchar](20) NULL,
	[ChucVu] [nvarchar](50) NULL,
	[Username] [varchar](50) NULL,
	[Password] [varchar](100) NULL,
	[NgayNhanViec] [date] NULL,
	[RoleID] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[MaNhanVien] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PHIEUNHAPVATTU]    Script Date: 21/01/2026 10:33:00 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PHIEUNHAPVATTU](
	[MaPhieuNhap] [int] IDENTITY(1,1) NOT NULL,
	[NgayNhapPhieu] [datetime] NULL,
	[TongTien] [decimal](18, 0) NULL,
PRIMARY KEY CLUSTERED 
(
	[MaPhieuNhap] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PHIEUTHUTIEN]    Script Date: 21/01/2026 10:33:00 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PHIEUTHUTIEN](
	[MaPhieuThu] [int] IDENTITY(1,1) NOT NULL,
	[MaKhachHang] [int] NULL,
	[MaNhanVien] [int] NULL,
	[SoTienThu] [decimal](18, 0) NULL,
	[NgayThuTien] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[MaPhieuThu] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ROLE]    Script Date: 21/01/2026 10:33:00 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ROLE](
	[RoleID] [int] IDENTITY(1,1) NOT NULL,
	[RoleName] [nvarchar](50) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[RoleID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[THAMSO]    Script Date: 21/01/2026 10:33:00 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[THAMSO](
	[TenThamSo] [nvarchar](50) NOT NULL,
	[GiaTri] [float] NULL,
PRIMARY KEY CLUSTERED 
(
	[TenThamSo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[VATTU]    Script Date: 21/01/2026 10:33:00 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[VATTU](
	[MaVatTu] [int] IDENTITY(1,1) NOT NULL,
	[TenVatTu] [nvarchar](100) NOT NULL,
	[MaLoai] [int] NULL,
	[NhaCungCap] [nvarchar](100) NULL,
	[MoTa] [nvarchar](255) NULL,
	[SoLuongTon] [int] NULL,
	[MaDonViTinh] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[MaVatTu] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[BAOCAOTON] ([MaVatTu], [Thang], [Nam], [TonDau], [PhatSinhNhap], [PhatSinhXuat], [TonCuoi]) VALUES (6, 1, 2026, 0, 2, 0, 2)
GO
INSERT [dbo].[BAOCAOTON] ([MaVatTu], [Thang], [Nam], [TonDau], [PhatSinhNhap], [PhatSinhXuat], [TonCuoi]) VALUES (6, 2, 2026, 2, 0, 0, 2)
GO
INSERT [dbo].[BAOCAOTON] ([MaVatTu], [Thang], [Nam], [TonDau], [PhatSinhNhap], [PhatSinhXuat], [TonCuoi]) VALUES (6, 3, 2026, 2, 0, 0, 2)
GO
INSERT [dbo].[BAOCAOTON] ([MaVatTu], [Thang], [Nam], [TonDau], [PhatSinhNhap], [PhatSinhXuat], [TonCuoi]) VALUES (6, 4, 2026, 0, 0, 0, 0)
GO
INSERT [dbo].[BAOCAOTON] ([MaVatTu], [Thang], [Nam], [TonDau], [PhatSinhNhap], [PhatSinhXuat], [TonCuoi]) VALUES (6, 7, 2026, 0, 0, 0, 0)
GO
INSERT [dbo].[BAOCAOTON] ([MaVatTu], [Thang], [Nam], [TonDau], [PhatSinhNhap], [PhatSinhXuat], [TonCuoi]) VALUES (6, 12, 2025, 0, 0, 0, 0)
GO
INSERT [dbo].[BAOCAOTON] ([MaVatTu], [Thang], [Nam], [TonDau], [PhatSinhNhap], [PhatSinhXuat], [TonCuoi]) VALUES (6, 12, 2026, 0, 0, 0, 0)
GO
INSERT [dbo].[BAOCAOTON] ([MaVatTu], [Thang], [Nam], [TonDau], [PhatSinhNhap], [PhatSinhXuat], [TonCuoi]) VALUES (12, 1, 2026, 0, 7, 6, 1)
GO
INSERT [dbo].[BAOCAOTON] ([MaVatTu], [Thang], [Nam], [TonDau], [PhatSinhNhap], [PhatSinhXuat], [TonCuoi]) VALUES (12, 2, 2026, 1, 0, 0, 1)
GO
INSERT [dbo].[BAOCAOTON] ([MaVatTu], [Thang], [Nam], [TonDau], [PhatSinhNhap], [PhatSinhXuat], [TonCuoi]) VALUES (12, 3, 2026, 1, 0, 0, 1)
GO
INSERT [dbo].[BAOCAOTON] ([MaVatTu], [Thang], [Nam], [TonDau], [PhatSinhNhap], [PhatSinhXuat], [TonCuoi]) VALUES (12, 4, 2026, 0, 0, 0, 0)
GO
INSERT [dbo].[BAOCAOTON] ([MaVatTu], [Thang], [Nam], [TonDau], [PhatSinhNhap], [PhatSinhXuat], [TonCuoi]) VALUES (12, 7, 2026, 0, 0, 0, 0)
GO
INSERT [dbo].[BAOCAOTON] ([MaVatTu], [Thang], [Nam], [TonDau], [PhatSinhNhap], [PhatSinhXuat], [TonCuoi]) VALUES (12, 12, 2025, 0, 0, 0, 0)
GO
INSERT [dbo].[BAOCAOTON] ([MaVatTu], [Thang], [Nam], [TonDau], [PhatSinhNhap], [PhatSinhXuat], [TonCuoi]) VALUES (12, 12, 2026, 0, 0, 0, 0)
GO
INSERT [dbo].[BAOCAOTON] ([MaVatTu], [Thang], [Nam], [TonDau], [PhatSinhNhap], [PhatSinhXuat], [TonCuoi]) VALUES (13, 1, 2026, 20, 0, 0, 20)
GO
INSERT [dbo].[BAOCAOTON] ([MaVatTu], [Thang], [Nam], [TonDau], [PhatSinhNhap], [PhatSinhXuat], [TonCuoi]) VALUES (13, 2, 2026, 20, 0, 0, 20)
GO
INSERT [dbo].[BAOCAOTON] ([MaVatTu], [Thang], [Nam], [TonDau], [PhatSinhNhap], [PhatSinhXuat], [TonCuoi]) VALUES (13, 3, 2026, 20, 0, 0, 20)
GO
INSERT [dbo].[BAOCAOTON] ([MaVatTu], [Thang], [Nam], [TonDau], [PhatSinhNhap], [PhatSinhXuat], [TonCuoi]) VALUES (13, 4, 2026, 0, 0, 0, 0)
GO
INSERT [dbo].[BAOCAOTON] ([MaVatTu], [Thang], [Nam], [TonDau], [PhatSinhNhap], [PhatSinhXuat], [TonCuoi]) VALUES (13, 7, 2026, 0, 0, 0, 0)
GO
INSERT [dbo].[BAOCAOTON] ([MaVatTu], [Thang], [Nam], [TonDau], [PhatSinhNhap], [PhatSinhXuat], [TonCuoi]) VALUES (13, 12, 2025, 0, 20, 0, 20)
GO
INSERT [dbo].[BAOCAOTON] ([MaVatTu], [Thang], [Nam], [TonDau], [PhatSinhNhap], [PhatSinhXuat], [TonCuoi]) VALUES (13, 12, 2026, 0, 0, 0, 0)
GO
INSERT [dbo].[BAOCAOTON] ([MaVatTu], [Thang], [Nam], [TonDau], [PhatSinhNhap], [PhatSinhXuat], [TonCuoi]) VALUES (14, 1, 2026, 0, 1, 0, 1)
GO
INSERT [dbo].[BAOCAOTON] ([MaVatTu], [Thang], [Nam], [TonDau], [PhatSinhNhap], [PhatSinhXuat], [TonCuoi]) VALUES (14, 2, 2026, 1, 0, 0, 1)
GO
INSERT [dbo].[BAOCAOTON] ([MaVatTu], [Thang], [Nam], [TonDau], [PhatSinhNhap], [PhatSinhXuat], [TonCuoi]) VALUES (14, 3, 2026, 1, 0, 0, 1)
GO
INSERT [dbo].[BAOCAOTON] ([MaVatTu], [Thang], [Nam], [TonDau], [PhatSinhNhap], [PhatSinhXuat], [TonCuoi]) VALUES (14, 4, 2026, 0, 0, 0, 0)
GO
INSERT [dbo].[BAOCAOTON] ([MaVatTu], [Thang], [Nam], [TonDau], [PhatSinhNhap], [PhatSinhXuat], [TonCuoi]) VALUES (14, 7, 2026, 0, 0, 0, 0)
GO
INSERT [dbo].[BAOCAOTON] ([MaVatTu], [Thang], [Nam], [TonDau], [PhatSinhNhap], [PhatSinhXuat], [TonCuoi]) VALUES (15, 1, 2026, 0, 0, 0, 0)
GO
INSERT [dbo].[BAOCAOTON] ([MaVatTu], [Thang], [Nam], [TonDau], [PhatSinhNhap], [PhatSinhXuat], [TonCuoi]) VALUES (15, 2, 2026, 0, 0, 0, 0)
GO
INSERT [dbo].[BAOCAOTON] ([MaVatTu], [Thang], [Nam], [TonDau], [PhatSinhNhap], [PhatSinhXuat], [TonCuoi]) VALUES (15, 3, 2026, 0, 0, 0, 0)
GO
INSERT [dbo].[BAOCAOTON] ([MaVatTu], [Thang], [Nam], [TonDau], [PhatSinhNhap], [PhatSinhXuat], [TonCuoi]) VALUES (15, 4, 2026, 0, 0, 0, 0)
GO
INSERT [dbo].[BAOCAOTON] ([MaVatTu], [Thang], [Nam], [TonDau], [PhatSinhNhap], [PhatSinhXuat], [TonCuoi]) VALUES (15, 7, 2026, 0, 0, 0, 0)
GO
INSERT [dbo].[BAOCAOTON] ([MaVatTu], [Thang], [Nam], [TonDau], [PhatSinhNhap], [PhatSinhXuat], [TonCuoi]) VALUES (16, 1, 2026, 0, 0, 0, 0)
GO
INSERT [dbo].[CT_HD] ([MaHoaDon], [MaVatTu], [MaDonViTinh], [SoLuongBan], [DonGiaBan], [ThanhTien]) VALUES (1, 12, 1, 5, CAST(11000 AS Decimal(18, 0)), CAST(55000 AS Decimal(18, 0)))
GO
INSERT [dbo].[CT_HD] ([MaHoaDon], [MaVatTu], [MaDonViTinh], [SoLuongBan], [DonGiaBan], [ThanhTien]) VALUES (2, 12, 2, 1, CAST(11000 AS Decimal(18, 0)), CAST(11000 AS Decimal(18, 0)))
GO
INSERT [dbo].[CT_PNVT] ([MaPhieuNhap], [MaVatTu], [MaNhanVien], [MaDonViTinh], [SoLuong], [DonGiaNhap], [ThanhTien], [DonGiaBan]) VALUES (10, 13, 15, 1, 20, CAST(5000 AS Decimal(18, 0)), CAST(100000 AS Decimal(18, 0)), CAST(6000 AS Decimal(18, 0)))
GO
INSERT [dbo].[CT_PNVT] ([MaPhieuNhap], [MaVatTu], [MaNhanVien], [MaDonViTinh], [SoLuong], [DonGiaNhap], [ThanhTien], [DonGiaBan]) VALUES (1011, 12, 13, 2, 5, CAST(10000 AS Decimal(18, 0)), CAST(50000 AS Decimal(18, 0)), CAST(15000 AS Decimal(18, 0)))
GO
INSERT [dbo].[CT_PNVT] ([MaPhieuNhap], [MaVatTu], [MaNhanVien], [MaDonViTinh], [SoLuong], [DonGiaNhap], [ThanhTien], [DonGiaBan]) VALUES (1012, 6, 13, 3, 2, CAST(10000 AS Decimal(18, 0)), CAST(20000 AS Decimal(18, 0)), CAST(11000 AS Decimal(18, 0)))
GO
INSERT [dbo].[CT_PNVT] ([MaPhieuNhap], [MaVatTu], [MaNhanVien], [MaDonViTinh], [SoLuong], [DonGiaNhap], [ThanhTien], [DonGiaBan]) VALUES (1012, 12, 13, 2, 2, CAST(10000 AS Decimal(18, 0)), CAST(20000 AS Decimal(18, 0)), CAST(11000 AS Decimal(18, 0)))
GO
INSERT [dbo].[CT_PNVT] ([MaPhieuNhap], [MaVatTu], [MaNhanVien], [MaDonViTinh], [SoLuong], [DonGiaNhap], [ThanhTien], [DonGiaBan]) VALUES (1013, 14, 13, 2, 1, CAST(1111 AS Decimal(18, 0)), CAST(1111 AS Decimal(18, 0)), CAST(11111 AS Decimal(18, 0)))
GO
SET IDENTITY_INSERT [dbo].[DONVITINH] ON 
GO
INSERT [dbo].[DONVITINH] ([MaDonViTinh], [TenDonViTinh]) VALUES (1, N'Cái')
GO
INSERT [dbo].[DONVITINH] ([MaDonViTinh], [TenDonViTinh]) VALUES (2, N'Chiếc')
GO
INSERT [dbo].[DONVITINH] ([MaDonViTinh], [TenDonViTinh]) VALUES (3, N'Bộ')
GO
SET IDENTITY_INSERT [dbo].[DONVITINH] OFF
GO
SET IDENTITY_INSERT [dbo].[HOADON] ON 
GO
INSERT [dbo].[HOADON] ([MaHoaDon], [MaKhachHang], [MaNhanVien], [NgayLapHoaDon], [TongTien]) VALUES (1, 2023, 13, CAST(N'2026-01-04T13:02:11.277' AS DateTime), CAST(55000 AS Decimal(18, 0)))
GO
INSERT [dbo].[HOADON] ([MaHoaDon], [MaKhachHang], [MaNhanVien], [NgayLapHoaDon], [TongTien]) VALUES (2, 2023, 13, CAST(N'2026-01-16T14:52:48.870' AS DateTime), CAST(11000 AS Decimal(18, 0)))
GO
SET IDENTITY_INSERT [dbo].[HOADON] OFF
GO
SET IDENTITY_INSERT [dbo].[KHACHHANG] ON 
GO
INSERT [dbo].[KHACHHANG] ([MaKhachHang], [HoVaTen], [SoDienThoai], [DiaChi], [GioiTinh], [NgaySinh]) VALUES (2023, N'Hữu Tính', N'0344686177', N'ppppph', N'Nữ', CAST(N'2025-12-01' AS Date))
GO
INSERT [dbo].[KHACHHANG] ([MaKhachHang], [HoVaTen], [SoDienThoai], [DiaChi], [GioiTinh], [NgaySinh]) VALUES (2024, N'htinh', N'0344686155', N'pc', N'Nữ', CAST(N'2025-12-07' AS Date))
GO
INSERT [dbo].[KHACHHANG] ([MaKhachHang], [HoVaTen], [SoDienThoai], [DiaChi], [GioiTinh], [NgaySinh]) VALUES (2025, N'htinh', N'0344686156', N'ddd', N'Nam', CAST(N'2026-01-18' AS Date))
GO
SET IDENTITY_INSERT [dbo].[KHACHHANG] OFF
GO
SET IDENTITY_INSERT [dbo].[LOAIVATTU] ON 
GO
INSERT [dbo].[LOAIVATTU] ([MaLoai], [TenLoai], [MoTa]) VALUES (1, N'Vật liệu xây dựng thô', N'Bao gồm cát, đá, xi măng, gạch xây, sắt thép định hình')
GO
INSERT [dbo].[LOAIVATTU] ([MaLoai], [TenLoai], [MoTa]) VALUES (15, N'Vật liệu hoàn thiện', N'Các loại gạch ốp lát, sơn tường, thạch cao và giấy dán tường')
GO
INSERT [dbo].[LOAIVATTU] ([MaLoai], [TenLoai], [MoTa]) VALUES (16, N'Thiết bị điện', N'Dây dẫn điện, công tắc, ổ cắm, tủ điện và các thiết bị bảo vệ')
GO
INSERT [dbo].[LOAIVATTU] ([MaLoai], [TenLoai], [MoTa]) VALUES (17, N'Thiết bị chiếu sáng', N'Các loại bóng đèn LED, đèn trang trí, đèn chùm và đèn ngoài trời')
GO
INSERT [dbo].[LOAIVATTU] ([MaLoai], [TenLoai], [MoTa]) VALUES (18, N'Thiết bị vệ sinh', N'Bồn cầu, lavabo, vòi sen, bồn tắm và các phụ kiện nhà tắm')
GO
INSERT [dbo].[LOAIVATTU] ([MaLoai], [TenLoai], [MoTa]) VALUES (19, N'Hệ thống cấp thoát nước', N'Ống nhựa PVC, HDPE, phụ kiện nối ống và các loại van nước')
GO
INSERT [dbo].[LOAIVATTU] ([MaLoai], [TenLoai], [MoTa]) VALUES (20, N'Kim khí - Dụng cụ', N'Các loại bu lông, ốc vít, bản lề, khóa cửa và dụng cụ cầm tay')
GO
INSERT [dbo].[LOAIVATTU] ([MaLoai], [TenLoai], [MoTa]) VALUES (21, N'Hóa chất xây dựng', N'Keo dán gạch, phụ gia chống thấm, hóa chất tẩy rửa công nghiệp')
GO
INSERT [dbo].[LOAIVATTU] ([MaLoai], [TenLoai], [MoTa]) VALUES (22, N'Thiết bị nội thất', N'Bàn ghế, tủ kệ trưng bày và các vật dụng trang trí nội thất khác')
GO
SET IDENTITY_INSERT [dbo].[LOAIVATTU] OFF
GO
SET IDENTITY_INSERT [dbo].[NHANVIEN] ON 
GO
INSERT [dbo].[NHANVIEN] ([MaNhanVien], [HoTen], [NgaySinh], [SoDienThoai], [ChucVu], [Username], [Password], [NgayNhanViec], [RoleID]) VALUES (13, N'Nguyễn Văn A', CAST(N'2005-07-08' AS Date), N'0344677777', N'Thủ Kho', N'VanA', N'huutinh', CAST(N'2026-01-03' AS Date), 2)
GO
INSERT [dbo].[NHANVIEN] ([MaNhanVien], [HoTen], [NgaySinh], [SoDienThoai], [ChucVu], [Username], [Password], [NgayNhanViec], [RoleID]) VALUES (14, N'Hà Thị T', CAST(N'2006-07-22' AS Date), N'0344686155', N'NV Bán hàng', N'HT', N'hhhh', CAST(N'2026-01-04' AS Date), 3)
GO
INSERT [dbo].[NHANVIEN] ([MaNhanVien], [HoTen], [NgaySinh], [SoDienThoai], [ChucVu], [Username], [Password], [NgayNhanViec], [RoleID]) VALUES (15, N'Hữu Tính', CAST(N'2006-01-04' AS Date), N'0234484742', N'NV Bán hàng', N'Huu', N'123', CAST(N'2026-01-03' AS Date), 3)
GO
INSERT [dbo].[NHANVIEN] ([MaNhanVien], [HoTen], [NgaySinh], [SoDienThoai], [ChucVu], [Username], [Password], [NgayNhanViec], [RoleID]) VALUES (16, N'Trần Thị Bình', CAST(N'1995-08-20' AS Date), N'0912345678', N'Thủ Kho', N'binhtt', N'binh95@', CAST(N'2026-01-10' AS Date), 2)
GO
INSERT [dbo].[NHANVIEN] ([MaNhanVien], [HoTen], [NgaySinh], [SoDienThoai], [ChucVu], [Username], [Password], [NgayNhanViec], [RoleID]) VALUES (17, N'Lê Hoàng Cường', CAST(N'1992-10-08' AS Date), N'0987654321', N'Thủ Kho', N'cuonglh', N'secret456', CAST(N'2026-01-09' AS Date), 2)
GO
INSERT [dbo].[NHANVIEN] ([MaNhanVien], [HoTen], [NgaySinh], [SoDienThoai], [ChucVu], [Username], [Password], [NgayNhanViec], [RoleID]) VALUES (18, N'Phạm Minh Đức', CAST(N'2000-02-02' AS Date), N'0933445566', N'NV Bán hàng', N'ducmh', N'duc2022', CAST(N'2026-01-11' AS Date), 3)
GO
INSERT [dbo].[NHANVIEN] ([MaNhanVien], [HoTen], [NgaySinh], [SoDienThoai], [ChucVu], [Username], [Password], [NgayNhanViec], [RoleID]) VALUES (19, N'Vũ Thu Thảo', CAST(N'1999-01-08' AS Date), N'0944556677', N'Quản Lý', N'thaovt', N'thao_vip', CAST(N'2026-01-02' AS Date), 1)
GO
INSERT [dbo].[NHANVIEN] ([MaNhanVien], [HoTen], [NgaySinh], [SoDienThoai], [ChucVu], [Username], [Password], [NgayNhanViec], [RoleID]) VALUES (20, N'Văn Cười', CAST(N'2000-02-08' AS Date), N'0556633448', N'Thủ Kho', N'cuoi', N'cuoi', CAST(N'2026-01-20' AS Date), 2)
GO
SET IDENTITY_INSERT [dbo].[NHANVIEN] OFF
GO
SET IDENTITY_INSERT [dbo].[PHIEUNHAPVATTU] ON 
GO
INSERT [dbo].[PHIEUNHAPVATTU] ([MaPhieuNhap], [NgayNhapPhieu], [TongTien]) VALUES (10, CAST(N'2025-12-13T16:22:03.187' AS DateTime), CAST(100000 AS Decimal(18, 0)))
GO
INSERT [dbo].[PHIEUNHAPVATTU] ([MaPhieuNhap], [NgayNhapPhieu], [TongTien]) VALUES (1011, CAST(N'2026-01-04T12:54:08.417' AS DateTime), CAST(50000 AS Decimal(18, 0)))
GO
INSERT [dbo].[PHIEUNHAPVATTU] ([MaPhieuNhap], [NgayNhapPhieu], [TongTien]) VALUES (1012, CAST(N'2026-01-04T12:54:48.170' AS DateTime), CAST(40000 AS Decimal(18, 0)))
GO
INSERT [dbo].[PHIEUNHAPVATTU] ([MaPhieuNhap], [NgayNhapPhieu], [TongTien]) VALUES (1013, CAST(N'2026-01-16T11:03:20.247' AS DateTime), CAST(1111 AS Decimal(18, 0)))
GO
SET IDENTITY_INSERT [dbo].[PHIEUNHAPVATTU] OFF
GO
SET IDENTITY_INSERT [dbo].[ROLE] ON 
GO
INSERT [dbo].[ROLE] ([RoleID], [RoleName]) VALUES (1, N'QuanLy')
GO
INSERT [dbo].[ROLE] ([RoleID], [RoleName]) VALUES (2, N'ThuKho')
GO
INSERT [dbo].[ROLE] ([RoleID], [RoleName]) VALUES (3, N'NVBanHang')
GO
SET IDENTITY_INSERT [dbo].[ROLE] OFF
GO
INSERT [dbo].[THAMSO] ([TenThamSo], [GiaTri]) VALUES (N'TiLeTinhDonGia', 1.05)
GO
INSERT [dbo].[THAMSO] ([TenThamSo], [GiaTri]) VALUES (N'TuoiToiThieu', 18)
GO
SET IDENTITY_INSERT [dbo].[VATTU] ON 
GO
INSERT [dbo].[VATTU] ([MaVatTu], [TenVatTu], [MaLoai], [NhaCungCap], [MoTa], [SoLuongTon], [MaDonViTinh]) VALUES (6, N'Xi măng', 15, NULL, N'svdsag', 2, 1)
GO
INSERT [dbo].[VATTU] ([MaVatTu], [TenVatTu], [MaLoai], [NhaCungCap], [MoTa], [SoLuongTon], [MaDonViTinh]) VALUES (12, N'Bút mực', 15, NULL, N'cvcxcx', 1, 1)
GO
INSERT [dbo].[VATTU] ([MaVatTu], [TenVatTu], [MaLoai], [NhaCungCap], [MoTa], [SoLuongTon], [MaDonViTinh]) VALUES (13, N'Bút bi', 16, NULL, N'đssdsá', 20, 1)
GO
INSERT [dbo].[VATTU] ([MaVatTu], [TenVatTu], [MaLoai], [NhaCungCap], [MoTa], [SoLuongTon], [MaDonViTinh]) VALUES (14, N'Xi mănggg', 1, NULL, N'dùng để xây dựng', 1, 3)
GO
INSERT [dbo].[VATTU] ([MaVatTu], [TenVatTu], [MaLoai], [NhaCungCap], [MoTa], [SoLuongTon], [MaDonViTinh]) VALUES (15, N'Sách', 18, NULL, N'hihi', 0, 2)
GO
INSERT [dbo].[VATTU] ([MaVatTu], [TenVatTu], [MaLoai], [NhaCungCap], [MoTa], [SoLuongTon], [MaDonViTinh]) VALUES (16, N'Guong', 18, NULL, N'khong', 0, 1)
GO
SET IDENTITY_INSERT [dbo].[VATTU] OFF
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__NHANVIEN__536C85E41BD3B503]    Script Date: 21/01/2026 10:33:00 SA ******/
ALTER TABLE [dbo].[NHANVIEN] ADD UNIQUE NONCLUSTERED 
(
	[Username] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__NHANVIEN__536C85E4EB198302]    Script Date: 21/01/2026 10:33:00 SA ******/
ALTER TABLE [dbo].[NHANVIEN] ADD UNIQUE NONCLUSTERED 
(
	[Username] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[HOADON] ADD  DEFAULT (getdate()) FOR [NgayLapHoaDon]
GO
ALTER TABLE [dbo].[HOADON] ADD  DEFAULT ((0)) FOR [TongTien]
GO
ALTER TABLE [dbo].[PHIEUNHAPVATTU] ADD  DEFAULT (getdate()) FOR [NgayNhapPhieu]
GO
ALTER TABLE [dbo].[PHIEUNHAPVATTU] ADD  DEFAULT ((0)) FOR [TongTien]
GO
ALTER TABLE [dbo].[PHIEUTHUTIEN] ADD  DEFAULT (getdate()) FOR [NgayThuTien]
GO
ALTER TABLE [dbo].[VATTU] ADD  DEFAULT ((0)) FOR [SoLuongTon]
GO
ALTER TABLE [dbo].[BAOCAOTON]  WITH CHECK ADD FOREIGN KEY([MaVatTu])
REFERENCES [dbo].[VATTU] ([MaVatTu])
GO
ALTER TABLE [dbo].[BAOCAOTON]  WITH CHECK ADD FOREIGN KEY([MaVatTu])
REFERENCES [dbo].[VATTU] ([MaVatTu])
GO
ALTER TABLE [dbo].[CT_HD]  WITH CHECK ADD FOREIGN KEY([MaDonViTinh])
REFERENCES [dbo].[DONVITINH] ([MaDonViTinh])
GO
ALTER TABLE [dbo].[CT_HD]  WITH CHECK ADD FOREIGN KEY([MaDonViTinh])
REFERENCES [dbo].[DONVITINH] ([MaDonViTinh])
GO
ALTER TABLE [dbo].[CT_HD]  WITH CHECK ADD FOREIGN KEY([MaHoaDon])
REFERENCES [dbo].[HOADON] ([MaHoaDon])
GO
ALTER TABLE [dbo].[CT_HD]  WITH CHECK ADD FOREIGN KEY([MaHoaDon])
REFERENCES [dbo].[HOADON] ([MaHoaDon])
GO
ALTER TABLE [dbo].[CT_HD]  WITH CHECK ADD FOREIGN KEY([MaVatTu])
REFERENCES [dbo].[VATTU] ([MaVatTu])
GO
ALTER TABLE [dbo].[CT_HD]  WITH CHECK ADD FOREIGN KEY([MaVatTu])
REFERENCES [dbo].[VATTU] ([MaVatTu])
GO
ALTER TABLE [dbo].[CT_PNVT]  WITH CHECK ADD FOREIGN KEY([MaDonViTinh])
REFERENCES [dbo].[DONVITINH] ([MaDonViTinh])
GO
ALTER TABLE [dbo].[CT_PNVT]  WITH CHECK ADD FOREIGN KEY([MaDonViTinh])
REFERENCES [dbo].[DONVITINH] ([MaDonViTinh])
GO
ALTER TABLE [dbo].[CT_PNVT]  WITH CHECK ADD FOREIGN KEY([MaNhanVien])
REFERENCES [dbo].[NHANVIEN] ([MaNhanVien])
GO
ALTER TABLE [dbo].[CT_PNVT]  WITH CHECK ADD FOREIGN KEY([MaNhanVien])
REFERENCES [dbo].[NHANVIEN] ([MaNhanVien])
GO
ALTER TABLE [dbo].[CT_PNVT]  WITH CHECK ADD FOREIGN KEY([MaPhieuNhap])
REFERENCES [dbo].[PHIEUNHAPVATTU] ([MaPhieuNhap])
GO
ALTER TABLE [dbo].[CT_PNVT]  WITH CHECK ADD FOREIGN KEY([MaPhieuNhap])
REFERENCES [dbo].[PHIEUNHAPVATTU] ([MaPhieuNhap])
GO
ALTER TABLE [dbo].[CT_PNVT]  WITH CHECK ADD FOREIGN KEY([MaVatTu])
REFERENCES [dbo].[VATTU] ([MaVatTu])
GO
ALTER TABLE [dbo].[CT_PNVT]  WITH CHECK ADD FOREIGN KEY([MaVatTu])
REFERENCES [dbo].[VATTU] ([MaVatTu])
GO
ALTER TABLE [dbo].[HOADON]  WITH CHECK ADD FOREIGN KEY([MaKhachHang])
REFERENCES [dbo].[KHACHHANG] ([MaKhachHang])
GO
ALTER TABLE [dbo].[HOADON]  WITH CHECK ADD FOREIGN KEY([MaKhachHang])
REFERENCES [dbo].[KHACHHANG] ([MaKhachHang])
GO
ALTER TABLE [dbo].[HOADON]  WITH CHECK ADD FOREIGN KEY([MaNhanVien])
REFERENCES [dbo].[NHANVIEN] ([MaNhanVien])
GO
ALTER TABLE [dbo].[HOADON]  WITH CHECK ADD FOREIGN KEY([MaNhanVien])
REFERENCES [dbo].[NHANVIEN] ([MaNhanVien])
GO
ALTER TABLE [dbo].[NHANVIEN]  WITH CHECK ADD  CONSTRAINT [FK_NV_ROLE] FOREIGN KEY([RoleID])
REFERENCES [dbo].[ROLE] ([RoleID])
GO
ALTER TABLE [dbo].[NHANVIEN] CHECK CONSTRAINT [FK_NV_ROLE]
GO
ALTER TABLE [dbo].[PHIEUTHUTIEN]  WITH CHECK ADD FOREIGN KEY([MaKhachHang])
REFERENCES [dbo].[KHACHHANG] ([MaKhachHang])
GO
ALTER TABLE [dbo].[PHIEUTHUTIEN]  WITH CHECK ADD FOREIGN KEY([MaKhachHang])
REFERENCES [dbo].[KHACHHANG] ([MaKhachHang])
GO
ALTER TABLE [dbo].[PHIEUTHUTIEN]  WITH CHECK ADD FOREIGN KEY([MaNhanVien])
REFERENCES [dbo].[NHANVIEN] ([MaNhanVien])
GO
ALTER TABLE [dbo].[PHIEUTHUTIEN]  WITH CHECK ADD FOREIGN KEY([MaNhanVien])
REFERENCES [dbo].[NHANVIEN] ([MaNhanVien])
GO
ALTER TABLE [dbo].[VATTU]  WITH CHECK ADD FOREIGN KEY([MaLoai])
REFERENCES [dbo].[LOAIVATTU] ([MaLoai])
GO
ALTER TABLE [dbo].[VATTU]  WITH CHECK ADD FOREIGN KEY([MaLoai])
REFERENCES [dbo].[LOAIVATTU] ([MaLoai])
GO
ALTER TABLE [dbo].[VATTU]  WITH CHECK ADD  CONSTRAINT [FK_VatTu_DonViTinh] FOREIGN KEY([MaDonViTinh])
REFERENCES [dbo].[DONVITINH] ([MaDonViTinh])
GO
ALTER TABLE [dbo].[VATTU] CHECK CONSTRAINT [FK_VatTu_DonViTinh]
GO
USE [master]
GO
ALTER DATABASE [QuanLyVatTu] SET  READ_WRITE 
GO
