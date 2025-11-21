CMCS – Contract Monthly Claim System (Final, SQL Server + Automation)

- ASP.NET Core MVC (net8.0)
- Entity Framework Core with **Microsoft SQL Server** provider (no InMemory)
- Connection string in appsettings.json under `ConnectionStrings:DefaultConnection`
- On startup the app calls `Database.EnsureCreated()` so the schema is created.

Automation implemented:
- Lecturer view:
  - Auto-calculation of total claim amount on the Create Claim screen via JavaScript (hours × hourly rate).
  - Server-side validation of entries and uploads.
- PC/AM view:
  - Separate queue showing only Submitted claims with Approve / Reject workflow and audit table.
- HR view:
  - Summary of Approved claims and overall total amount to be paid.

SQL script:
- `SqlScripts/CMCS_CreateAndSeed.sql` – T‑SQL script for Microsoft SQL Server that:
  - Creates CMCS database and all tables (Claims, ClaimEntries, Attachments, Approvals).
  - Adds sample data so markers can test quickly.

How to run:
1. Open `CMCS.Web/CMCS.Web.csproj` in Visual Studio 2022 or later.
2. Ensure SQL Server is running locally; update the connection string in `appsettings.json` if needed.
3. Set CMCS.Web as the startup project.
4. Press F5 to run.

5. GitHub Link - https://github.com/dikomakamogelo/programmingfinal.git
