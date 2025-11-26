using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace PageNavigation.Model;

public partial class QuanLyVatTuContext : DbContext
{
    public QuanLyVatTuContext()
    {
    }

    public QuanLyVatTuContext(DbContextOptions<QuanLyVatTuContext> options)
        : base(options)
    {
    }

    public virtual DbSet<BaoCaoM> Baocaotons { get; set; }

    public virtual DbSet<CT_HoaDonM> CtHds { get; set; }

    public virtual DbSet<CT_PhieuNhapVatTuM> CtPnvts { get; set; }

    public virtual DbSet<DonViTinhM> Donvitinhs { get; set; }

    public virtual DbSet<HoaDonM> Hoadons { get; set; }

    public virtual DbSet<KhachHangM> Khachhangs { get; set; }

    public virtual DbSet<LoaiVatTuM> Loaivattus { get; set; }

    public virtual DbSet<NhanVienM> Nhanviens { get; set; }

    public virtual DbSet<PhieuNhapVatTuM> Phieunhapvattus { get; set; }

    public virtual DbSet<PhieuThuTienM> Phieuthutiens { get; set; }

    public virtual DbSet<Thamso> Thamsos { get; set; }

    public virtual DbSet<VatTuM> Vattus { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)

        => optionsBuilder.UseSqlServer("Data Source=TIENIZDABEZT;Initial Catalog=QuanLyVatTu;Integrated Security=True;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BaoCaoM>(entity =>
        {
            entity.HasKey(e => new { e.MaVatTu, e.Thang, e.Nam }).HasName("PK__BAOCAOTO__7AC8E52F3D525E48");

            entity.ToTable("BAOCAOTON");

            entity.HasOne(d => d.MaVatTuNavigation).WithMany(p => p.Baocaotons)
                .HasForeignKey(d => d.MaVatTu)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__BAOCAOTON__MaVat__619B8048");
        });

        modelBuilder.Entity<CT_HoaDonM>(entity =>
        {
            entity.HasKey(e => new { e.MaHoaDon, e.MaVatTu }).HasName("PK__CT_HD__33E3F68D6A10F7DD");

            entity.ToTable("CT_HD");

            entity.Property(e => e.DonGiaBan).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.ThanhTien).HasColumnType("decimal(18, 0)");

            entity.HasOne(d => d.MaDonViTinhNavigation).WithMany(p => p.CT_HoaDon)
                .HasForeignKey(d => d.MaDonViTinh)
                .HasConstraintName("FK__CT_HD__MaDonViTi__5070F446");

            entity.HasOne(d => d.MaHoaDonNavigation).WithMany(p => p.CT_HoaDon)
                .HasForeignKey(d => d.MaHoaDon)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CT_HD__MaHoaDon__4E88ABD4");

            entity.HasOne(d => d.MaVatTuNavigation).WithMany(p => p.CT_HoaDon)
                .HasForeignKey(d => d.MaVatTu)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CT_HD__MaVatTu__4F7CD00D");
        });

        modelBuilder.Entity<CT_PhieuNhapVatTuM>(entity =>
        {
            entity.HasKey(e => new { e.MaPhieuNhap, e.MaVatTu }).HasName("PK__CT_PNVT__A4CDC88D0CB0CE5F");

            entity.ToTable("CT_PNVT");

            entity.Property(e => e.DonGiaBan).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.DonGiaNhap).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.ThanhTien).HasColumnType("decimal(18, 0)");

            entity.HasOne(d => d.MaDonViTinhNavigation).WithMany(p => p.CT_PhieuNhapVatTu)
                .HasForeignKey(d => d.MaDonViTinh)
                .HasConstraintName("FK__CT_PNVT__MaDonVi__59FA5E80");

            entity.HasOne(d => d.MaNhanVienNavigation).WithMany(p => p.CT_PhieuNhapVatTu)
                .HasForeignKey(d => d.MaNhanVien)
                .HasConstraintName("FK__CT_PNVT__MaNhanV__59063A47");

            entity.HasOne(d => d.MaPhieuNhapNavigation).WithMany(p => p.CT_PhieuNhapVatTu)
                .HasForeignKey(d => d.MaPhieuNhap)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CT_PNVT__MaPhieu__571DF1D5");

            entity.HasOne(d => d.MaVatTuNavigation).WithMany(p => p.CT_PhieuNhapVatTu)
                .HasForeignKey(d => d.MaVatTu)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CT_PNVT__MaVatTu__5812160E");
        });

        modelBuilder.Entity<DonViTinhM>(entity =>
        {
            entity.HasKey(e => e.MaDonViTinh).HasName("PK__DONVITIN__ADD89AA2FAEB3E5D");

            entity.ToTable("DONVITINH");

            entity.Property(e => e.TenDonViTinh).HasMaxLength(50);
        });

        modelBuilder.Entity<HoaDonM>(entity =>
        {
            entity.HasKey(e => e.MaHoaDon).HasName("PK__HOADON__835ED13B8E845A78");

            entity.ToTable("HOADON");

            entity.Property(e => e.NgayLapHoaDon)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.TongTien)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 0)");

            entity.HasOne(d => d.MaKhachHangNavigation).WithMany(p => p.HoaDon)
                .HasForeignKey(d => d.MaKhachHang)
                .HasConstraintName("FK__HOADON__MaKhachH__4AB81AF0");

            entity.HasOne(d => d.MaNhanVienNavigation).WithMany(p => p.HoaDon)
                .HasForeignKey(d => d.MaNhanVien)
                .HasConstraintName("FK__HOADON__MaNhanVi__4BAC3F29");
        });

        modelBuilder.Entity<KhachHangM>(entity =>
        {
            entity.HasKey(e => e.MaKhachHang).HasName("PK__KHACHHAN__88D2F0E56FC51FB8");

            entity.ToTable("KHACHHANG");

            entity.Property(e => e.DiaChi).HasMaxLength(200);
            entity.Property(e => e.GioiTinh).HasMaxLength(10);
            entity.Property(e => e.HoVaTen).HasMaxLength(100);
            entity.Property(e => e.SoDienThoai)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        modelBuilder.Entity<LoaiVatTuM>(entity =>
        {
            entity.HasKey(e => e.MaLoai).HasName("PK__LOAIVATT__730A57597FB9C2DE");

            entity.ToTable("LOAIVATTU");

            entity.Property(e => e.MoTa).HasMaxLength(255);
            entity.Property(e => e.TenLoai).HasMaxLength(100);
        });

        modelBuilder.Entity<NhanVienM>(entity =>
        {
            entity.HasKey(e => e.MaNhanVien).HasName("PK__NHANVIEN__77B2CA4725D79B92");

            entity.ToTable("NHANVIEN");

            entity.HasIndex(e => e.Username, "UQ__NHANVIEN__536C85E49BD05871").IsUnique();

            entity.Property(e => e.ChucVu).HasMaxLength(50);
            entity.Property(e => e.HoTen).HasMaxLength(100);
            entity.Property(e => e.Password)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.SoDienThoai)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<PhieuNhapVatTuM>(entity =>
        {
            entity.HasKey(e => e.MaPhieuNhap).HasName("PK__PHIEUNHA__1470EF3B9599EC9D");

            entity.ToTable("PHIEUNHAPVATTU");

            entity.Property(e => e.NgayNhapPhieu)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.TongTien)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 0)");
        });

        modelBuilder.Entity<PhieuThuTienM>(entity =>
        {
            entity.HasKey(e => e.MaPhieuThu).HasName("PK__PHIEUTHU__1D8B9C69C0568C93");

            entity.ToTable("PHIEUTHUTIEN");

            entity.Property(e => e.NgayThuTien)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.SoTienThu).HasColumnType("decimal(18, 0)");

            entity.HasOne(d => d.MaKhachHangNavigation).WithMany(p => p.PhieuThuTien)
                .HasForeignKey(d => d.MaKhachHang)
                .HasConstraintName("FK__PHIEUTHUT__MaKha__5DCAEF64");

            entity.HasOne(d => d.MaNhanVienNavigation).WithMany(p => p.PhieuThuTien)
                .HasForeignKey(d => d.MaNhanVien)
                .HasConstraintName("FK__PHIEUTHUT__MaNha__5EBF139D");
        });

        modelBuilder.Entity<Thamso>(entity =>
        {
            entity.HasKey(e => e.TenThamSo).HasName("PK__THAMSO__A49B135984051162");

            entity.ToTable("THAMSO");

            entity.Property(e => e.TenThamSo).HasMaxLength(50);
        });

        modelBuilder.Entity<VatTuM>(entity =>
        {
            entity.HasKey(e => e.MaVatTu).HasName("PK__VATTU__0BD27B6A04098E6A");

            entity.ToTable("VATTU");

            entity.Property(e => e.MoTa).HasMaxLength(255);
            entity.Property(e => e.NhaCungCap).HasMaxLength(100);
            entity.Property(e => e.SoLuongTon).HasDefaultValue(0);
            entity.Property(e => e.TenVatTu).HasMaxLength(100);

            entity.HasOne(d => d.MaLoaiNavigation).WithMany(p => p.Vattus)
                .HasForeignKey(d => d.MaLoai)
                .HasConstraintName("FK__VATTU__MaLoai__45F365D3");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
