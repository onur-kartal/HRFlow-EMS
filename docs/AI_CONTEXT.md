# HRFlow EMS - AI Context

> Bu dosya, HRFlow EMS projesine katkı sağlayacak geliştiriciler ve yapay zeka araçları (ChatGPT, Codex, GitHub Copilot, Cursor vb.) için hazırlanmıştır.
>
> Bu projede kod üretmeden önce bu dosya okunmalıdır.

---

# Proje Bilgisi

Proje Adı

HRFlow EMS

Açılımı

Human Resources Flow - Employee Management System

Amaç

Kurumsal seviyede geliştirilen modern İnsan Kaynakları Yönetim Sistemi.

Bu proje yalnızca çalışan bilgilerini tutan basit bir CRUD uygulaması değildir.

Hedef;

- Kurumsal Mimari
- Clean Architecture prensipleri
- SOLID
- Repository Pattern
- DTO
- AutoMapper
- Identity
- Role Based Authorization
- Maintainable Code
- Performans

üzerine kurulmuş gerçek bir HR yazılımıdır.

---

# Teknolojiler

Backend

- ASP.NET Core 9 MVC

ORM

- Entity Framework Core

Authentication

- ASP.NET Core Identity

Database

- SQL Server

Frontend

- Bootstrap 5
- AdminLTE 4
- jQuery
- DataTables
- SweetAlert2

---

# Solution Yapısı

HRFlow.sln

Projects

HRFlow.Web

MVC katmanı

Sadece

- Controller
- View
- ViewComponent
- ViewModel

bulunur.

Business katmanı burada yazılmaz.

---

HRFlow.Business

İş kuralları.

İçerik

- Services
- DTO
- AutoMapper
- Interfaces

Repository erişimi burada yapılır.

---

HRFlow.Data

Database işlemleri.

İçerik

- DbContext
- Repository
- Configurations
- Seed

---

HRFlow.Entities

Entity modelleri.

Entity dışında hiçbir iş kuralı bulunmaz.

---

HRFlow.Common

Genel yardımcı sınıflar.

Örneğin

CurrentUserService

---

# Mimari Kurallar

Layered Architecture kullanılmaktadır.

Bağımlılık sırası

Web

↓

Business

↓

Data

↓

Entities

Hiçbir katman yukarı katmana bağımlı olmamalıdır.

---

# Repository Kuralları

Repository

SADECE

database erişiminden sorumludur.

Repository

iş kuralı içermez.

Örnek

DOĞRU

GetEmployeeById()

YANLIŞ

DeleteEmployeeIfManager()

Bu Business katmanında olmalıdır.

---

# Business Kuralları

Business

iş kurallarını içerir.

Örnek

Personel silinebilir mi

Rol değişebilir mi

Kullanıcı oluşturulabilir mi

Business karar verir.

---

# Controller Kuralları

Controller

ince tutulmalıdır.

Controller

iş kuralı yazmaz.

Controller

yalnızca

Service çağırır.

---

# DTO Kuralları

Entity

View'a gönderilmez.

Her zaman DTO kullanılır.

Entity

↓

DTO

↓

View

---

# AutoMapper

Entity

DTO

dönüşümleri AutoMapper ile yapılır.

Elle mapleme yazılmaz.

---

# Identity Yapısı

Identity kullanılmaktadır.

Identity User

SystemUser

adlı sınıftır.

Identity tabloları

Employee tablolarından ayrıdır.

---

# Employee ve SystemUser

Bu projedeki en önemli mimari karar budur.

Employee

Personelin gerçek bilgileridir.

SystemUser

Sadece sisteme giriş yapar.

Bu iki yapı birbirinden ayrıdır.

İlişki

Employee

1

↓

0..1

SystemUser

Yani

Her personelin kullanıcı hesabı olmak zorunda değildir.

---

# Kullanıcı Oluşturma

Yeni personel eklenir.

↓

Personel listesinde görünür.

↓

İstenirse

"Kullanıcı Oluştur"

butonuna basılır.

↓

Identity hesabı oluşturulur.

Bu süreç bilinçli olarak seçilmiştir.

Personel kaydı ile kullanıcı kaydı birbirine bağımlı değildir.

---

# Roller

Projede kullanılan roller

Admin

HR

Manager

Employee

---

# Rol Açıklamaları

Admin

Sistemin tamamını yönetebilir.

HR

Personel yönetebilir.

Departman yönetebilir.

Pozisyon yönetebilir.

Kullanıcı oluşturabilir.

Rol değiştirebilir.

Manager

Kendi ekibini yönetir.

İzin onaylar.

Employee

Sadece kendi bilgilerini görebilir.

---

# Authorization

Controller seviyesinde

Authorize

kullanılır.

View tarafında

User.IsInRole()

ile görünürlük yönetilir.

---

# CurrentUserService

Business katmanında

HttpContext

kullanılmaz.

Bunun yerine

CurrentUserService

kullanılır.

Bu servis

EmployeeId

Email

UserName

bilgilerini sağlar.

---

# Dashboard

Dashboard

tek bir View'dur.

Dashboard için ayrı ViewComponent kullanılmayacaktır.

Layout

↓

Navbar ViewComponent

↓

Sidebar ViewComponent

↓

RenderBody()

↓

Dashboard View

Dashboard içerisinde

role göre

Partial View

render edilir.

Örnek

Admin Dashboard

HR Dashboard

Manager Dashboard

Employee Dashboard

---

# Navbar

Navbar

ViewComponent kullanmaktadır.

Navbar

giriş yapan kullanıcı bilgilerini göstermelidir.

---

# Sidebar

Sidebar

ViewComponent kullanmaktadır.

Sidebar

role göre değişmelidir.

Employee

Admin menülerini görmemelidir.

---

# Kod Standartları

Method isimleri

İngilizce.

Class isimleri

İngilizce.

DTO isimleri

...

CreateDto

UpdateDto

ListDto

DetailDto

şeklinde olmalıdır.

---

# Entity Kuralları

Entity

View tarafından kullanılmaz.

Entity

Business dışına çıkmaz.

---

# Service Kuralları

Service

Repository ile konuşur.

Repository

Service çağırmaz.

---

# View Kuralları

View

iş kuralı içermez.

Sadece

görsel işlemler yapılır.

---

# JavaScript Kuralları

Mümkün olduğunca

View içine gömülmez.

Tekrar eden kodlar

wwwroot/js

altında tutulmalıdır.

---

# DataTables

Bütün listeler

DataTables

üzerinden çalışacaktır.

Tek tip görünüm kullanılacaktır.

---

# SweetAlert

Silme

Onay

Bilgilendirme

işlemleri SweetAlert2 ile yapılacaktır.

---

# Tasarım Kararları

Personel

ve

Identity

ayrıdır.

Dashboard

Partial View kullanacaktır.

Sidebar

role göre oluşacaktır.

Navbar

gerçek kullanıcı bilgisini gösterecektir.

Entity

View'a gitmeyecektir.

DTO kullanılacaktır.

Repository

iş kuralı yazmayacaktır.

Business

iş kurallarını yönetecektir.

---

# Tamamlanan Modüller

Authentication

Authorization

Employee CRUD

Department CRUD

Position CRUD

Identity

Role Management

User Creation

CurrentUserService

Role Based Visibility

---

# Geliştirilecek Modüller

Dashboard

Leave Management

Shift Management

Asset Management

Announcement Management

Report Management

Notification System

Audit Log

Settings

Email Service

File Upload

Holiday Management

Payroll Integration

---

# Yapay Zeka Kuralları

Bu projeye katkı sağlayacak AI aşağıdaki kurallara uymalıdır.

Yeni mimari oluşturma.

Mevcut mimari korunacaktır.

Repository içerisine iş kuralı yazılmayacaktır.

Entity doğrudan View'a gönderilmeyecektir.

CurrentUserService kullanılacaktır.

Employee ile SystemUser birleştirilmeyecektir.

Kod mümkün olduğunca mevcut yapıya uygun yazılacaktır.

Kod tekrarından kaçınılacaktır.

SOLID prensiplerine uyulacaktır.

Kod okunabilirliği performanstan önce gelir.

Var olan isimlendirme standartları korunacaktır.

Yeni modül geliştirirken mevcut klasör yapısı değiştirilmeyecektir.

Kod yazmadan önce mevcut servis ve repository incelenmelidir.

Bu dosyada belirtilen kurallar, yeni geliştirilecek tüm modüller için referans kabul edilir.