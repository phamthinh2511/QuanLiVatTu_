using System;

namespace PageNavigation.Service
{
    public static class PhieuNhapVatTuService
    {
        // 1. Khai báo sự kiện (Hộp thư)
        public static event EventHandler PhieuNhapVatTuChanged;

        // 2. Hàm kích hoạt (Gọi khi Lưu thành công)
        public static void NotifyChanged()
        {
            // Bắn tín hiệu cho các bên lắng nghe
            PhieuNhapVatTuChanged?.Invoke(null, EventArgs.Empty);
        }
    }
}