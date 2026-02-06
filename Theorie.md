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
