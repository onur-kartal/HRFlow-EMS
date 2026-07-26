using System.ComponentModel.DataAnnotations;

namespace HRFlow.Entities.Enums
{
    public enum AuditModule
    {
        [Display(Name = "Personel")]
        Employee,
        [Display(Name = "Departman")]
        Department,
        [Display(Name = "Pozisyon")]
        Position,
        [Display(Name = "İzin Talebi")]
        LeaveRequest,
        [Display(Name = "Fazla Mesai Talebi")]
        OvertimeRequest,
        [Display(Name = "Duyuru")]
        Announcement,
        [Display(Name = "Kimlik Doğrulama")]
        Authentication,
        [Display(Name = "Kullanıcı")]
        User,
        [Display(Name = "Rol")]
        Role,
        [Display(Name = "Sistem")]
        System
    }
}
