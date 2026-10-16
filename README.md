# DVLD - Driving & Vehicle License Department

A desktop application for managing driving licenses, built with **C# (Windows Forms)** using a **3-layer architecture**.

## Features

- **Authentication**: user login and password change
- **People management**: add, edit, search and view personal information
- **User management**: create, edit, activate / deactivate users
- **Application types and test types**: view and update fees
- **Local license applications**: create, track and cancel
- **Tests**: schedule and take tests (vision, written, street)
- **Licenses**:
  - first-time license issuance
  - renewal
  - replacement for lost or damaged licenses
  - international licenses
  - license history for a person
- **License detention**: detain and release licenses
- **Drivers**: list and view drivers

## Architecture

```
DVLD-Project/
├── DVLD/            # Presentation layer (Windows Forms)
├── Business Layer/  # Business layer (logic and business rules)
└── DataAccessLayer/ # Data access layer (ADO.NET)
```

| Layer | Role |
|-------|------|
| Presentation | Forms and user controls |
| Business | Business rules and validation |
| Data Access | Communication with the database |

## Database Schema

The SQL Server database is made of 13 tables: `People`, `Users`, `Countries`, `Applications`, `ApplicationTypes`, `LocalDrivingLicenseApplications`, `LicenseClasses`, `TestTypes`, `TestAppointments`, `Tests`, `Drivers`, `Licenses` and `DetainedLicenses`.

<img width="4855" height="3521" alt="DVLD Database" src="https://github.com/user-attachments/assets/7cf02717-bf42-446d-837e-55a952dc4463" />

## Screenshots

### Login
<img width="1135" height="662" alt="Capture d&#39;écran 2026-10-16 195228" src="https://github.com/user-attachments/assets/3f08b76b-8014-4f9d-b778-903b08b5883a" />

### Main Menu
<img width="1920" height="1020" alt="Main" src="https://github.com/user-attachments/assets/9b67d135-4464-4eef-a065-052afa37c296" />

### Manage People
<img width="1902" height="965" alt="People" src="https://github.com/user-attachments/assets/3c261b83-cc7b-43b6-8f83-1baa4438c197" />

### Local Driving License Applications
<img width="1791" height="976" alt="LDLApp" src="https://github.com/user-attachments/assets/2df7ee8d-a0be-429e-aff8-160429a71248" />

### License Details
<img width="976" height="677" alt="DriverLicenseInfo" src="https://github.com/user-attachments/assets/c32206bf-bb31-4d3d-bb44-8c4f64a613ba" />
## Technologies

- C# / Windows Forms
- ADO.NET
- SQL Server
- Visual Studio

## Getting Started

1. Clone the repository:
   ```bash
   git clone https://github.com/YOUR_USERNAME/DVLD.git
   ```
2. Create the DVLD database on SQL Server.
3. In the `DataAccessLayer` project, update the connection string to match your server (server name, database, credentials).
4. Open `DVLD/DVLD.sln` in Visual Studio.
5. Restore packages, build (`Ctrl + Shift + B`), then run (`F5`).

## Author

**Soufiane** - software development student at Eurelec.

## Acknowledgments

This project is based on the DVLD project from the [Programming Advices](https://programmingadvices.com) course by Mohammed Abu-Hadhoud. The database schema image comes from the course material.

## Status

Learning project, still being improved.
