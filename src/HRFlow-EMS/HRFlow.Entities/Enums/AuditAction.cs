using System.ComponentModel.DataAnnotations;

namespace HRFlow.Entities.Enums
{
    public enum AuditAction
    {
        [Display(Name = "Oluşturuldu")]
        Created,
        [Display(Name = "Güncellendi")]
        Updated,
        [Display(Name = "Silindi")]
        Deleted,
        [Display(Name = "Onaylandı")]
        Approved,
        [Display(Name = "Reddedildi")]
        Rejected,
        [Display(Name = "İptal Edildi")]
        Cancelled,
        [Display(Name = "Başarılı Giriş")]
        LoginSuccess,
        [Display(Name = "Başarısız Giriş")]
        LoginFailed,
        [Display(Name = "Çıkış Yapıldı")]
        Logout,
        [Display(Name = "Şifre Değiştirildi")]
        PasswordChanged,
        [Display(Name = "Şifre Sıfırlandı")]
        PasswordReset,
        [Display(Name = "Rol Değiştirildi")]
        RoleChanged,
        [Display(Name = "Durum Değiştirildi")]
        StatusChanged
    }
}
