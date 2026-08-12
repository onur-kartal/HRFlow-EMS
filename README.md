# HRFlow EMS

A modern Human Resources Management System built with ASP.NET Core MVC.

HRFlow EMS is a portfolio project designed to demonstrate a modular, layered and role-based Human Resources Management System using the .NET ecosystem.

The application includes employee management, leave and overtime workflows, payroll management, announcements, notifications, audit logging and role-based dashboards.

---

## ✨ Features

### 👨‍💼 Employee Management

* Employee create, update and soft delete
* Department and position management
* Employee profile management
* Personal and address information
* Profile image upload
* Active / passive employee management
* Role-based employee access
* User account creation for employees

### 👤 Profile Management

Employees can view their personal and employment information from their profile page.

Editable personal information includes:

* Phone number
* Personal email
* Address
* City
* District
* Postal code

Employment information such as department, position, hire date and company email is displayed as read-only.

---

## 🎂 Upcoming Birthdays

Role-based dashboards include an Upcoming Birthdays widget.

Features:

* Displays birthdays within the upcoming 5 days
* Profile image support
* Position information
* Today / Tomorrow / Days Left indicators
* Scrollable birthday list
* Leap year support for February 29 birthdays

---

## 🏢 Organization Management

* Department CRUD
* Position CRUD
* Employee–Department relationship
* Employee–Position relationship

---

## 📝 Leave Management

Employees can create and track their own leave requests.

Supported workflow:

* Pending
* Approved
* Rejected
* Cancelled

Authorization rules:

* Employees can cancel their own pending requests
* Managers, HR and Admin can approve or reject pending requests
* Approved or rejected requests cannot be modified by regular users
* Admin has extended management permissions

---

## ⏰ Overtime Management

Employees can create overtime requests.

Features:

* Work date
* Start / end time
* Automatic total-hour calculation
* Approval workflow
* Pending / Approved / Rejected / Cancelled statuses
* Role-based authorization
* Overtime data integration with Payroll

---

## 💰 Payroll Management

Payroll Management provides internal HR payroll tracking.

Features:

* Payroll period management
* Automatic payroll generation for active employees
* Base salary integration from employee records
* Approved overtime integration
* Automatic overtime amount calculation
* Bonus management
* Deduction management
* Automatic net salary calculation
* Payroll approval workflow
* Payment tracking
* Employee self-service payroll view
* PDF payroll export

Access rules:

* Admin and HR can manage all payroll records
* Managers and Employees can only view their own payroll records
* Users cannot access another employee's payroll by modifying URLs

> HRFlow EMS does not currently perform statutory tax, SGK or official e-payroll calculations.

---

## 📢 Announcement Management

Admin and HR users can create and manage internal announcements.

Features:

* Create
* Update
* Delete
* Active / passive management
* Start and end dates
* Dashboard announcement display

---

## 🔔 Notification Center

The application includes a user-specific notification system.

Notifications are generated for events such as:

* Leave request approval / rejection
* Overtime approval / rejection
* Payroll creation and payment
* New announcements

Features:

* Navbar notification dropdown
* Unread notification counter
* User-specific notifications
* Read / unread tracking
* Mark all as read
* Notification history
* Secure notification ownership checks

---

## 📊 Role-Based Dashboard

Dashboard content changes according to the current user's role.

Supported roles:

* Admin
* HR
* Manager
* Employee

Dashboard features include:

* Employee statistics
* Leave summaries
* Overtime summaries
* Charts
* Announcements
* Upcoming birthdays
* Role-specific information

---

## 🔐 Authentication & Authorization

The system uses ASP.NET Core Identity.

Features:

* Login / Logout
* Role-based authorization
* Admin
* HR
* Manager
* Employee roles
* Current user abstraction
* Secure page access
* User-to-employee relationship
* Password change
* Role management

---

## 📋 Audit Logging

Important business actions are automatically recorded.

Examples:

* Employee created / updated / deleted
* Department and position changes
* Leave approval / rejection
* Overtime approval / rejection
* Announcement management
* User creation
* Role changes
* Login / logout
* Password changes
* Payroll operations

Audit Logs are accessible only by Admin users.

---

## 🌐 Request Logging

HTTP requests are logged through custom ASP.NET Core middleware.

Tracked information includes:

* User
* Role
* IP address
* Request path
* HTTP method
* Status code
* Duration
* User Agent

Static files are excluded from unnecessary request logging.

---

## 🧭 Dynamic Page Metadata

The AdminLTE interface includes centralized page metadata management.

Features:

* Dynamic browser page titles
* Dynamic breadcrumbs
* Controller / Action based page metadata
* Centralized page header structure

Example:

`Dashboard > Payroll Management > Payroll Periods`

Browser title:

`Payroll Periods | HRFlow EMS`

---

## 🏗️ Architecture

HRFlow EMS uses a layered architecture.

```text
HRFlow.Entities
    ↓
HRFlow.Data
    ↓
HRFlow.Business
    ↓
HRFlow.Web

HRFlow.Common
    ↳ Shared abstractions and common infrastructure
```

### Project Layers

**HRFlow.Entities**

Contains:

* Entities
* Enums
* Base entities

**HRFlow.Data**

Contains:

* Entity Framework Core DbContext
* Repository implementations
* Repository interfaces
* Entity configurations
* EF Core migrations

**HRFlow.Business**

Contains:

* Business services
* Service interfaces
* DTOs
* AutoMapper configuration
* Business rules

**HRFlow.Common**

Contains shared abstractions and reusable common components.

**HRFlow.Web**

ASP.NET Core MVC presentation layer.

Contains:

* Controllers
* Razor Views
* ViewComponents
* AdminLTE UI
* Authentication / Authorization
* Middleware

---

## 🧩 Design Patterns & Practices

The project uses:

* Layered Architecture
* Repository Pattern
* Generic Repository
* Generic Service
* DTO Pattern
* AutoMapper
* Dependency Injection
* Soft Delete
* Role-Based Authorization
* Business Layer Validation
* Async / Await
* AsNoTracking for read operations
* Middleware-based request logging
* Audit logging
* Centralized UI metadata

---

## 🛠️ Technology Stack

### Backend

* C#
* .NET 9
* ASP.NET Core MVC
* ASP.NET Core Identity
* Entity Framework Core 9
* SQL Server

### Architecture & Data

* Repository Pattern
* Generic Repository / Service
* DTO
* AutoMapper
* Dependency Injection

### Frontend

* Razor Views
* AdminLTE 4
* Bootstrap 5
* Bootstrap Icons
* JavaScript
* jQuery
* DataTables
* SweetAlert2
* Chart.js

### Documents

* QuestPDF

### Development Tools

* Visual Studio 2022
* SQL Server Management Studio
* Git
* GitHub

---

## 🔒 Roles & Permissions

| Feature                          | Admin | HR  | Manager | Employee |
| -------------------------------- | ----- | --- | ------- | -------- |
| Employee Management              | ✅     | ✅   | ❌       | ❌        |
| Department / Position Management | ✅     | ✅   | ❌       | ❌        |
| Leave Request                    | ✅     | ✅   | ✅       | ✅        |
| Leave Approval                   | ✅     | ✅   | ✅       | ❌        |
| Overtime Request                 | ✅     | ✅   | ✅       | ✅        |
| Overtime Approval                | ✅     | ✅   | ✅       | ❌        |
| Payroll Management               | ✅     | ✅   | ❌       | ❌        |
| View Own Payroll                 | ✅     | ✅   | ✅       | ✅        |
| Announcement Management          | ✅     | ✅   | ❌       | ❌        |
| Notifications                    | Own   | Own | Own     | Own      |
| Audit Logs                       | ✅     | ❌   | ❌       | ❌        |
| Request Logs                     | ✅     | ❌   | ❌       | ❌        |

---

## 📁 Project Structure

```text
HRFlow-EMS
│
├── docs
│
└── src
    └── HRFlow-EMS
        ├── HRFlow.Entities
        ├── HRFlow.Data
        ├── HRFlow.Business
        ├── HRFlow.Common
        └── HRFlow.Web
```

---

## 📸 Screenshots

Screenshots will be added here.

Recommended screens:

* Role-Based Dashboard
* Employee Management
* Employee Profile
* Leave Management
* Overtime Management
* Payroll Management
* Notification Center
* Audit Logs

---

## 🚀 Project Status

### Core HR Modules Completed

HRFlow EMS currently includes the main HR management workflows required for the first portfolio-ready version.

Completed:

* Employee Management
* Organization Management
* Authentication & Authorization
* Leave Management
* Overtime Management
* Payroll Management
* Announcements
* Notification Center
* Audit & Request Logging
* Role-Based Dashboard
* Employee Profiles
* Upcoming Birthdays

Future improvements may include:

* Reports & Analytics
* Training Management
* Asset Management
* Performance Reviews
* Holiday Management

---

## 🎯 Purpose

HRFlow EMS was developed as a portfolio project to demonstrate practical experience with:

* ASP.NET Core MVC
* Entity Framework Core
* SQL Server
* Layered application architecture
* Role-based authorization
* Business workflow design
* Repository and service patterns
* Real-world HR processes

---

## 👨‍💻 Author

**Onur Kartal**

.NET Software Developer

GitHub: [github.com/onur-kartal](https://github.com/onur-kartal)
