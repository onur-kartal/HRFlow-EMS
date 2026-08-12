using System.Text.RegularExpressions;

namespace HRFlow.Web.Helpers
{
    public sealed record PageMetadata(string Title, string? Section, bool IsDashboard = false);

    public static class PageMetadataHelper
    {
        private static readonly Dictionary<string, PageMetadata> PageMappings = new(StringComparer.OrdinalIgnoreCase)
        {
            ["Dashboard:Index"] = new("Dashboard", null, true),
            ["Employee:Index"] = new("Personeller", "Personel Yönetimi"),
            ["Employee:Create"] = new("Yeni Personel", "Personel Yönetimi"),
            ["Employee:Edit"] = new("Personel Düzenle", "Personel Yönetimi"),
            ["Employee:Details"] = new("Personel Detayı", "Personel Yönetimi"),
            ["Employee:ChangeRole"] = new("Personel Rolünü Değiştir", "Personel Yönetimi"),
            ["Employee:Delete"] = new("Personeller", "Personel Yönetimi"),
            ["Department:Index"] = new("Departmanlar", "Organizasyon"),
            ["Department:Create"] = new("Yeni Departman", "Organizasyon"),
            ["Department:Edit"] = new("Departman Düzenle", "Organizasyon"),
            ["Department:Delete"] = new("Departmanlar", "Organizasyon"),
            ["Position:Index"] = new("Pozisyonlar", "Organizasyon"),
            ["Position:Create"] = new("Yeni Pozisyon", "Organizasyon"),
            ["Position:Edit"] = new("Pozisyon Düzenle", "Organizasyon"),
            ["Position:Delete"] = new("Pozisyonlar", "Organizasyon"),
            ["LeaveRequest:Index"] = new("İzin Talepleri", "İzin Yönetimi"),
            ["LeaveRequest:Create"] = new("Yeni İzin Talebi", "İzin Yönetimi"),
            ["LeaveRequest:Edit"] = new("İzin Talebini Düzenle", "İzin Yönetimi"),
            ["LeaveRequest:Approve"] = new("İzin Talebini Onayla", "İzin Yönetimi"),
            ["LeaveRequest:Reject"] = new("İzin Talebini Reddet", "İzin Yönetimi"),
            ["LeaveRequest:Cancel"] = new("İzin Talebini İptal Et", "İzin Yönetimi"),
            ["LeaveType:Index"] = new("İzin Türleri", "İzin Yönetimi"),
            ["LeaveType:Create"] = new("Yeni İzin Türü", "İzin Yönetimi"),
            ["LeaveType:Edit"] = new("İzin Türünü Düzenle", "İzin Yönetimi"),
            ["LeaveType:Delete"] = new("İzin Türleri", "İzin Yönetimi"),
            ["OvertimeRequest:MyRequests"] = new("Fazla Mesailerim", "Fazla Mesai Yönetimi"),
            ["OvertimeRequest:Create"] = new("Yeni Fazla Mesai Talebi", "Fazla Mesai Yönetimi"),
            ["OvertimeRequest:ApprovalList"] = new("Fazla Mesai Onayları", "Fazla Mesai Yönetimi"),
            ["OvertimeRequest:Management"] = new("Fazla Mesai Talepleri", "Fazla Mesai Yönetimi"),
            ["OvertimeRequest:Approve"] = new("Fazla Mesai Talebini Onayla", "Fazla Mesai Yönetimi"),
            ["OvertimeRequest:Reject"] = new("Fazla Mesai Talebini Reddet", "Fazla Mesai Yönetimi"),
            ["OvertimeRequest:Cancel"] = new("Fazla Mesai Talebini İptal Et", "Fazla Mesai Yönetimi"),
            ["OvertimeRequest:ChangeStatus"] = new("Fazla Mesai Durumunu Güncelle", "Fazla Mesai Yönetimi"),
            ["Announcement:Index"] = new("Duyurular", "Duyuru Yönetimi"),
            ["Announcement:Create"] = new("Yeni Duyuru", "Duyuru Yönetimi"),
            ["Announcement:Edit"] = new("Duyuru Düzenle", "Duyuru Yönetimi"),
            ["Announcement:Delete"] = new("Duyurular", "Duyuru Yönetimi"),
            ["Announcement:ChangeStatus"] = new("Duyuru Durumunu Güncelle", "Duyuru Yönetimi"),
            ["PayrollPeriod:Index"] = new("Bordro Dönemleri", "Bordro Yönetimi"),
            ["PayrollPeriod:Create"] = new("Yeni Bordro Dönemi", "Bordro Yönetimi"),
            ["PayrollPeriod:Details"] = new("Bordro Dönemi Detayı", "Bordro Yönetimi"),
            ["PayrollPeriod:GeneratePayrolls"] = new("Bordroları Oluştur", "Bordro Yönetimi"),
            ["PayrollPeriod:Approve"] = new("Bordro Dönemini Onayla", "Bordro Yönetimi"),
            ["PayrollPeriod:RevertApproval"] = new("Bordro Dönemi Onayını Geri Al", "Bordro Yönetimi"),
            ["PayrollPeriod:MarkAsPaid"] = new("Bordro Dönemini Ödendi Yap", "Bordro Yönetimi"),
            ["PayrollPeriod:ChangeStatus"] = new("Bordro Dönemi Durumunu Güncelle", "Bordro Yönetimi"),
            ["EmployeePayroll:Index"] = new("Çalışan Bordroları", "Bordro Yönetimi"),
            ["EmployeePayroll:Details"] = new("Bordro Detayı", "Bordro Yönetimi"),
            ["EmployeePayroll:Edit"] = new("Bordro Düzenle", "Bordro Yönetimi"),
            ["EmployeePayroll:Approve"] = new("Bordroyu Onayla", "Bordro Yönetimi"),
            ["EmployeePayroll:MarkAsPaid"] = new("Bordroyu Ödendi Yap", "Bordro Yönetimi"),
            ["EmployeePayroll:MyPayrolls"] = new("Bordrolarım", "Bordro"),
            ["EmployeePayroll:MyDetails"] = new("Bordro Detayı", "Bordro"),
            ["EmployeePayroll:ViewPdf"] = new("Bordro Detayı", "Bordro"),
            ["EmployeePayroll:DownloadPdf"] = new("Bordro Detayı", "Bordro"),
            ["AuditLog:Index"] = new("Denetim Kayıtları", "Denetim"),
            ["RequestLog:Index"] = new("İstek Kayıtları", "Denetim"),
            ["Account:Profile"] = new("Profilim", "Profil"),
            ["Account:UpdateProfile"] = new("Profilim", "Profil"),
            ["Account:ChangePassword"] = new("Profilim", "Profil"),
            ["Account:Login"] = new("Giriş Yap", "Hesap"),
            ["Account:Logout"] = new("Çıkış Yap", "Hesap"),
            ["Account:AccessDenied"] = new("Erişim Engellendi", "Hesap"),
            ["Notification:Index"] = new("Bildirimlerim", null),
            ["Notification:Open"] = new("Bildirimlerim", null),
            ["Home:Index"] = new("Ana Sayfa", null),
            ["Home:Privacy"] = new("Gizlilik", null),
            ["Home:Error"] = new("Hata", null)
        };

        public static PageMetadata Resolve(string? controller, string? action, string? requestedTitle)
        {
            var normalizedController = string.IsNullOrWhiteSpace(controller) ? "Home" : controller;
            var normalizedAction = string.IsNullOrWhiteSpace(action) ? "Index" : action;

            if (PageMappings.TryGetValue($"{normalizedController}:{normalizedAction}", out var mappedPage))
            {
                return string.IsNullOrWhiteSpace(requestedTitle)
                    ? mappedPage
                    : mappedPage with { Title = requestedTitle };
            }

            var controllerTitle = SplitPascalCase(normalizedController);
            var actionTitle = SplitPascalCase(normalizedAction);
            var title = string.IsNullOrWhiteSpace(requestedTitle)
                ? normalizedAction.Equals("Index", StringComparison.OrdinalIgnoreCase)
                    ? controllerTitle
                    : actionTitle
                : requestedTitle;

            return new PageMetadata(title, controllerTitle);
        }

        private static string SplitPascalCase(string value)
        {
            return Regex.Replace(value, "(?<!^)([A-Z])", " $1");
        }
    }
}
