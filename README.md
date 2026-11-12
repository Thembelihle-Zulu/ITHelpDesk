# IT Help Desk API

A backend for an internal IT help desk. Staff log support tickets, agents are alerted by email and work the tickets, and admins assign tickets and manage who is an agent. Built with C#, ASP.NET Core 8, Entity Framework Core and SQL Server, with JWT authentication and role-based access control.

> Status: the backend APIs are working. A React front end is planned.

## Features

- **Tickets:** create, view, assign, update status (Open, In Progress, Closed) and delete tickets, each with a category (Software, Hardware) and priority (Low, Medium, High).
- **Comments (coming soon):** comments on tickets are in progress.
- **Authentication:** a separate identity service handles registration, login, email confirmation and issues signed JWTs.
- **Role-based access:** four roles (SuperAdmin, Admin, Agent, User) enforced on the API endpoints.
- **Email notifications:** every agent is emailed when a new ticket is logged.
- **Admin seeding:** the roles and a SuperAdmin account are created on first start-up.
- **API docs:** Swagger UI with Bearer-token support in development.

## Architecture

```
ITHelpDeskDemo.sln
├── IdentityServer/   (AuthProject)  Registration, login, email confirmation; issues JWTs
├── ITHelpDeskDemo/                  Help desk REST API: tickets, comments, users, agents; EF Core migrations
└── SharedLibrary/                   Shared by both services: EF Core DbContext, models, DTOs, mappers, email service
```

The two services share one SQL Server database through `SharedLibrary`. The identity service issues a JWT on login, and the help desk API validates it (issuer, audience, lifetime and signing key) before applying role checks.

## Tech stack

C# · .NET 8 · ASP.NET Core Web API · ASP.NET Core Identity · JWT Bearer authentication · Entity Framework Core · SQL Server · Serilog · Swagger / Swashbuckle · SMTP email

## Getting started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- SQL Server (LocalDB, Express or Developer edition)
- The EF Core CLI: `dotnet tool install --global dotnet-ef`

### 1. Configure

Configuration files are not committed. Create an `appsettings.json` in both `ITHelpDeskDemo/` and `IdentityServer/`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=ITHelpDesk;Trusted_Connection=True;TrustServerCertificate=True"
  },
  "Jwt": {
    "Key": "<a random secret of at least 32 characters>",
    "Issuer": "ITHelpDesk.Identity",
    "Audience": "ITHelpDesk.Api"
  },
  "EMAIL_CONFIGURATION": {
    "EMAIL": "<sender email address>",
    "PASSWORD": "<SMTP or app password>",
    "HOST": "smtp.gmail.com",
    "PORT": 587
  }
}
```

`Jwt` values must match in both files. The identity service also reads these optional settings for the seeded SuperAdmin:

| Key | Purpose |
| --- | --- |
| `admin` | SuperAdmin username |
| `AdminEmail` | SuperAdmin email |
| `superpassword` | SuperAdmin password |

If they're missing, development defaults are used, so set your own for anything beyond local testing. The identity service can also take its connection string from a `HelpDeskConnectionString` environment variable or `.env` file.

### 2. Create the database

```bash
dotnet ef database update --project ITHelpDeskDemo
```

### 3. Run both services

```bash
dotnet run --project IdentityServer --launch-profile https   # https://localhost:7111
dotnet run --project ITHelpDeskDemo --launch-profile https   # https://localhost:7249
```

Open `/swagger` on either service, log in through the identity service, then click **Authorize** in the API's Swagger UI and paste the token.

## API overview

### Identity service — `/api/Login`

| Method | Endpoint | Access | Description |
| --- | --- | --- | --- |
| POST | `/api/Login/register` | Public | Register a user (assigned the User role) |
| POST | `/api/Login/Login` | Public | Log in and receive a JWT |
| GET | `/api/Login?userId=&Token=` | Public | Confirm an email address |

### Help desk API

| Method | Endpoint | Access | Description |
| --- | --- | --- | --- |
| POST | `/api/Tickets/createTicket` | Public | Log a ticket and email all agents |
| GET | `/api/Tickets/AllTickets` | Agent | List all tickets |
| GET | `/api/Tickets/{id}` | Agent | Get a ticket |
| PUT | `/api/Tickets/assignTicket/{id}` | Admin | Assign a ticket to an agent |
| PUT | `/api/Tickets/updateStatus/{id}` | Signed in | Update a ticket's status |
| DELETE | `/api/Tickets/Delete/{id}` | Signed in | Delete a ticket |
| GET / POST / PUT / DELETE | `/api/Comments/...` | Signed in | Ticket comments (in progress) |
| GET | `/api/AppUser/Users`, `/api/AppUser/Agents` | SuperAdmin, Admin, Agent | List users or agents |
| PUT | `/api/AppUser/AllocateRole/{id}` | SuperAdmin, Admin, Agent | Give a user the Agent role |
| GET / POST / PUT / DELETE | `/api/Users/...` | Agent | Manage users |

## Roadmap

- Finish ticket comments
- React front end for users, agents and admins
- Take the ticket's user from the JWT claims instead of the request body, and require sign-in to log a ticket
- Restrict status updates and deletes by role or ticket ownership
- Handle email-sending failures without failing ticket creation
- Restrict CORS to the front end's origin (currently open for local development)
- Unit and integration tests
