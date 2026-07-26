# CODEX_RULES.md

> Bu dosya, HRFlow EMS projesinde Codex tarafından yapılacak tüm geliştirmeler için zorunlu çalışma kurallarını içerir.

---

# Genel Kural

Kod yazmadan önce aşağıdaki dosyaları mutlaka oku.

1. AI_CONTEXT.md
2. README.md
3. Bu dosya (CODEX_RULES.md)

Bu dosyalar okunmadan kod üretilmemelidir.

---

# Öncelik

Her zaman mevcut mimari korunmalıdır.

Yeni mimari üretmeye çalışma.

Var olan yapıyı iyileştir.

---

# Çalışma Şekli

Her görev aşağıdaki sırayla ilerlemelidir.

1.

Projeyi analiz et.

2.

İlgili dosyaları belirle.

3.

Yapacağın işlemi maddeler halinde açıkla.

4.

Kullanıcı onayı bekle.

5.

Onay geldikten sonra kod yaz.

---

# Asla Yapma

❌ Büyük refactor yapma.

❌ Kullanıcının istemediği dosyaları değiştirme.

❌ Yeni mimari üretme.

❌ Gereksiz NuGet paketi ekleme.

❌ Aynı işi yapan ikinci servis oluşturma.

❌ Gereksiz helper oluşturma.

❌ Gereksiz extension method yazma.

❌ Kullanıcı istemedikçe namespace değiştirme.

❌ Kullanıcı istemedikçe dosya taşıma.

❌ Dosya isimlerini değiştirme.

❌ Kod stilini değiştirme.

❌ Yorum satırlarını silme.

---

# Kod Yazma Kuralları

Kod

- okunabilir
- sade
- SOLID uyumlu

olmalıdır.

Kısa kod yazmak yerine anlaşılır kod yaz.

---

# Mimari Kuralları

Layered Architecture korunacaktır.

Dependency yönü değiştirilmeyecektir.

Web

↓

Business

↓

Data

↓

Entities

---

# Repository

Repository

SADECE

database işlemi yapar.

İş kuralı yazılmaz.

---

# Business

İş kuralları

Business katmanındadır.

Repository içerisine taşınmaz.

---

# Controller

Controller

ince tutulmalıdır.

Controller içerisinde

iş kuralı yazılmaz.

---

# Entity

Entity

View'a gönderilmez.

Her zaman DTO kullanılır.

---

# DTO

Yeni ekran gerekiyorsa

uygun DTO oluştur.

Entity kullanma.

---

# AutoMapper

Elle mapleme yazma.

Mevcut MappingProfile kullan.

---

# Authentication

Identity

SystemUser

üzerinden çalışır.

Employee ile birleştirme.

---

# Employee

Employee

Personeldir.

Identity değildir.

Bu yapı değiştirilmeyecektir.

---

# CurrentUserService

Giriş yapan kullanıcı bilgileri

CurrentUserService

üzerinden alınacaktır.

HttpContext

Business katmanında kullanılmayacaktır.

---

# Authorization

Role kontrolü

Controller

veya

View

tarafında yapılabilir.

Business içerisinde

User.IsInRole()

kullanılmaz.

---

# ViewComponent

Mevcut ViewComponent yapısı korunacaktır.

Yeni ViewComponent

yalnızca kullanıcı isterse oluşturulacaktır.

---

# Dashboard

Dashboard

Partial View

kullanacaktır.

Dashboard için yeni mimari kurulmayacaktır.

---

# Sidebar

Sidebar

role göre değişecektir.

Employee

Admin menülerini göremez.

---

# Navbar

Navbar

giriş yapan kullanıcı bilgilerini gösterecektir.

---

# JavaScript

Tekrar eden kod

wwwroot/js

altında tutulmalıdır.

View içerisine mümkün olduğunca az JavaScript yaz.

---

# CSS

Var olan tema korunmalıdır.

Yeni CSS

gerekmedikçe eklenmez.

---

# DataTables

Mevcut DataTable helper kullanılmalıdır.

Yeni DataTable altyapısı oluşturulmaz.

---

# SweetAlert

Silme

Onay

Bilgilendirme

işlemleri

SweetAlert2

ile yapılmalıdır.

---

# Performans

Gereksiz sorgu yazma.

N+1 sorgularından kaçın.

Mevcut Include yapısını bozma.

AsNoTracking gerekiyorsa kullan.

---

# Kod Düzeni

Methodlar küçük olmalıdır.

Tek sorumluluk ilkesi uygulanmalıdır.

Magic string kullanılmamalıdır.

Mevcut Constants sınıfları kullanılmalıdır.

---

# Yeni Dosya Oluşturma

Yeni dosya oluşturmadan önce

mevcut dosyalar kontrol edilmelidir.

Aynı işi yapan sınıf varsa

yenisi oluşturulmaz.

---

# Kod Üretmeden Önce

Mutlaka aşağıdaki soruları cevapla.

Bu değişiklik hangi dosyaları etkileyecek?

Yeni dosya gerekiyor mu?

Mevcut yapı kullanılabilir mi?

Mimari bozuluyor mu?

---

# Kod Ürettikten Sonra

Her zaman rapor oluştur.

Örnek

Değiştirilen dosyalar

- EmployeeController
- EmployeeService
- EmployeeList.cshtml

Sebep

Role bazlı görünürlük eklendi.

Mimari

Korundu.

Yeni dosya

Oluşturulmadı.

Repository

Değişmedi.

---

# Kullanıcı İletişim Kuralı

Kullanıcı istemedikçe

uzun açıklama yapma.

İstenen görevi yap.

Eksik bilgi varsa

tek bir soru sor.

Tahmin ederek kod yazma.

---

# HRFlow Özel Kuralı

Bu proje kurumsal seviyede geliştirilmektedir.

Amaç

çalışan kod yazmak değil,

bakımı kolay,

okunabilir,

genişletilebilir,

profesyonel

bir kod tabanı oluşturmaktır.

Kod üretirken her zaman bunu öncelik olarak kabul et.

---

# En Önemli Kural

Kod yazmadan önce düşün.

Mimariyi bozma.

Kullanıcının istemediği değişikliği yapma.

Her zaman mevcut yapıya uyum sağlayacak çözüm üret.