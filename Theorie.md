# Theorie - testProject - ASP.NET MVC Core

## SQL concept in klassen (Models)

### Primary keys

Automatisch zal het .NET framework in een klasse (bv. Category) de property **Id** nemen als PK.

- Ook als de naam in dat geval: **CategoryId** is (Naam van klasse + Id) zal het ook automatisch PK worden.

Indien het een andere naam heeft of als je het expliciet wilt verduidelijken gaan we gebruik maken van een **data annotatie**.

```csharp
public class Category
    {
        [Key]
        public int Id { get; set; } // PK van tabel (adhv data annotatie)

        public string Name { get; set; }

        public int DisplayOrder { get; set; }
    }
```

### Not null (verplicht in te geven)

Als je wilt dat een bepaalde kolom verplicht moet ingegeven worden, gaan we terug gebruik maken van **data annotatie**.

```csharp
public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required] // Zegt tegen SQL dat dit NN is
        public string Name { get; set; }

        public int DisplayOrder { get; set; }
    }
```

## Connection string (DB_connectie maken) - Entity FrameWork & SQL Server

Dit zorgt voor een connectie tussen de applicatie & de database. (Hoe en waar hij die kan vinden)

**Voorbeeld van connection string:**

**Voorbeeld in json file:**

```json
"Server:localhost;Database=WebshopDB;Trusted_Connection=True;TrustServerCertificate=True"    
```

- Localhost = server-naam (Waar staat de DB?)
- WebshopDB = DB zelf (Welke DB?)
- Trusted_Connection=True = Gebruik windows authenticatie (geen ww)

=> De string zelf plaats je in de **appsettings.json** file.

**Voorbeeld in json file:**

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "ConnectionStrings": {
    "DefaultConnection": "Server:localhost;DataBase=Bulky;Trusted_Connection=True;TrustServerCertificate=True"
  }
}
    
```

### Aanmaken van een DB (Entity FrameWork)

Dit is best practice om te doen via code -> NIET in de MSSM zelf.
Vroeger zat het framework in de basis-package, maar in .NET 10 moet je het framework toevoegen via **Nuget Package Manager**.

Stappen:

1. Nuget package toevoegen via Packet manager **(Rechtsklik op project)**
2. De nodige packages installeren:

    ```csharp
        Microsoft.EntityFrameWorkCore // Entity FrameWork
        Microsoft.EntityFrameWorkCore.SqlServer // SQL server 
        Microsoft.EntityFrameWorkCore.Tools // Migration
    ```

    -> Zorg dat elke package van dezelfde versie/release is.

3. Een databasecontext klasse maken en instantiëren in **Program.cs**:

    ```csharp
    using BulkyWeb.Models;
    using Microsoft.EntityFrameworkCore;

    namespace BulkyWeb.Data
    {
        public class ApplicationDbContext : DbContext
        {
            public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
            {

            }

            public DbSet<Category> Categories { get; set; } // Tabel voor in DB
        }
    }
    ```

    **Program.cs**

     ```csharp
    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.
    builder.Services.AddControllersWithViews();
    builder.Services.AddDbContext<ApplicationDbContext>(options => 
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))); // DB context toevoegen + optie (SQL server gebruiken)

    var app = builder.Build();
    ```

    =>  ConnectionString moet perfect kloppen met string in **appsettings.json** - anders zal de DB niet goed aangemaakt worden.

4. Nu gaan we via **Package Manager Console** de DB aanmaken:

    (Tools > Nuget Package Manager > Package Manager Console)

    - Commando: update-database
    - Controleer in SSMS of de DB erbij is gekomen.

5. Tabel aanmaken:

    - Een tabel maak je aan door in Models een klasse te definiëren waarvan je een tabel wil. (bv. Category in ons geval)
    - Belangrijke zaken voor de aanmaken tabel:
        1. Zorg dat het een PK heeft (data annotatie)
        2. Zorg dat de DBContext een **DbSet** bevat voor die Model (zie code hierboven)

        => Dit is de kracht van 'Entity FrameWork', die zal automatisch door migratie die tabel aanmaken zonder één lijn SQL te moeten schrijven.

    - Aanmaken van een **migratie:**
        1. Via Manager Console:

            PM> Add-Migration 'beschrijvende naam voor migratie'

             ```csharp
             PM> Add-Migration AddCategoryTableToDB
             ```

        2. Nu moet je enkel de DB nog eens updaten en zal de tabel zichtbaar zijn. De migraties worden ook opgeslaan in de code & DB.
