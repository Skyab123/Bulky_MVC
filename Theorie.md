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

### Aanmaken van een tabel (Entity FrameWork)

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
