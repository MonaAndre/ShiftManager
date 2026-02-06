# ShiftManager
ShiftManager is a console application for managing departments, employees, roles, and work shifts.  
The application allows scheduling weekly shifts and viewing summary reports.

## Seed data
The project uses Entity Framework Core migrations to create the database schema and seed initial data.

Before running migrations, make sure the **connection string in `AppDbContext` matches your local PostgreSQL setup** (host, database name, username, and password).

To apply all migrations with seed data:
```bash
dotnet ef database update
```

To run the application:
```bash
dotnet run
```
