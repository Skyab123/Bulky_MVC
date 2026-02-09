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

## Aanmaken van een nieuw tabblad

### Controller aanmaken voor dat tabblad

In ons geval gaan we de klasse **Category** gebruiken.
Het doel is dus om een lijst van categoriëen weer te geven.

=> D.w.z.: de tabel in de DB opvullen, uitlezen en weergeven op het scherm.

- Eerst moeten we de navigatie naar dat tabblad mogelijk maken:

1. Aanmaken van een Controller:

    ```csharp
    using BulkyWeb.Data;
    using BulkyWeb.Models;
    using Microsoft.AspNetCore.Mvc;

    namespace BulkyWeb.Controllers
    {   
        public class CategoryController : Controller
        {
            private readonly ApplicationDbContext _db;

            public CategoryController(ApplicationDbContext db) 
            {
                _db = db;
            }

            public IActionResult Index()
            {
                List<Category> objCategoryList = _db.Categories.ToList();
                // Leest de data uit de tabel vd DB
                return View(objCategoryList);
            }
        }
    }
    ```

    -> We gaan de DBcontext injecteren in de constructor zodat de die kunnen gebruiken om data uit de DB te fetchen of erin te steken.

    We gebruiken een **ActionResult** om de view aan te maken:

    - Het idee: ```localhost:7000/[controller]/[action]```

        -> Dus **Category** zal hier de ```controller``` zijn & de ActionResult **Index** is de ```action```. Met die URL kom je op dat tabblad.

        -> Bij de eerste keer surfen zal je normaliter een foutmelding krijgen, omdat er nog geen index is om te tonen (HTML).

    - In de views moet je een folder aanmaken van de controller & daarin zet je de views die je wilt weergeven:

        - In die view kan je **HTML** schrijven. Voor we dit doen gaan we eerst het tabblad in de header toevoegen, zodat je kan surfen zonder in de URL te moeten typen.

        - In de **Shared** folder zit onze layout-HTML voor alle pagina's. Daar zullen we de header aanpassen zodat er een link (anchor) bijkomt.

        Voorbeeldcode **_Layout.cshtml:**

        ```csharp
        <div class="navbar-collapse collapse d-sm-inline-flex justify-content-between">
            <ul class="navbar-nav flex-grow-1">
                <li class="nav-item">
                    <a class="nav-link text-dark" asp-controller="Home" asp-action="Index">Home</a>
                </li>
                <li class="nav-item">
                    <a class="nav-link text-dark" asp-controller="Category" asp-action="Index">Category</a> // Hier hebben we de controller toegevoegd.
                </li>
                <li class="nav-item">
                    <a class="nav-link text-dark" asp-controller="Home" asp-action="Privacy">Privacy</a>
                </li>
            </ul>
        </div>
        ```

        - Bij het klikken op die link zal je op de url komen die we hierboven gedefinieerd hebbben.

2. De database opvullen met data zodat we die erna kunnen fetchen

    We moeten de methode van **DbContext** overriden zodat we de **Categories** tabel kunnen opvullen.

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

            protected override void OnModelCreating(ModelBuilder modelBuilder) // Tabel opvullen met data (Bouwen van uw Model)
            {
                modelBuilder.Entity<Category>()
                    .HasData(
                    new Category { Id = 1, Name="Action", DisplayOrder = 1},
                    new Category { Id = 2, Name = "SciFi", DisplayOrder = 2 },
                    new Category { Id = 3, Name = "History", DisplayOrder = 3 }
                    );
            }
        }
    }
    ```

    => **Belangrijk:** Doe een nieuwe migration zodat de update in de DB terecht komt. Controleer in de DB of het toegevoegd is.

    ```csharp
    PM> Add-Migration SeedCategoryTable
    Build started...
    Build succeeded.
    To undo this action, use Remove-Migration.
    PM> Updata-Database
    ```

3. Het weergeven van de data op de webapplicatie

    -> Hier zullen we gebruik maken van Csharp code gemengd met HTML code.

    -> Er wordt gebruik gemaakt van bootstrapklassen & icons.

    Voorbeeld:

     ```csharp
     @model List<Category> // We willen de lijst van Categoriëen weergeven

    <div class="container">
        <div class="row pt-4 pb-3">
            <div class="col-6">
                <h2 class="text-primary">Category List</h2>
            </div>
            <div class="col-6 text-end">
                <a asp-controller="Category" asp-action="Create" class="btn btn-primary">
                    <i class="bi bi-plus-circle"></i> Create New Category</a>
            </div>
        </div>
    
    <table class="table table-bordered table-striped">
        <thead>
            <tr>
                <th>
                    Category Name
                </th>
                <th>
                    Display Order
                </th>
            </tr>
        </thead>
        <tbody>
            // Iteren over alle categoriën (LINQ voor volgorde)
            @foreach (var cat in Model.OrderBy(u => u.DisplayOrder))
            {
                <tr>
                    <td>@cat.Name</td>
                    <td>@cat.DisplayOrder</td>
                </tr>
            }
        </tbody>
    </table>

    </div>
     ```

    - We hebben dus ook een nieuwe action geïntroduceerd om een Categorie te creëren.
    - In de **Category** Controller gaan we dus een nieuwe View aanmaken om daar dan een form weer te geven.

    ```csharp
    public IActionResult Create()
    {
        return View();
    }
    ```

    - Vergeet niet de view ook met de juiste naam aan te maken

    ```csharp
    @model Category // Automatische instantiatie

    <form method="post">
        <div class="border p-3 mt-4">
            <div class="row-pb-2">
                <h2 class="text-primary">Create Category</h2>
                <hr/>
            </div>
            <div class="mb-3 row p-1">
                <label asp-for="Name" class="p-0"></label>
                <input asp-for="Name" class="form-control"/>
            </div>
            <div class="mb-3 row p-1">
                <label asp-for="DisplayOrder" class="p-0"></label>
                <input asp-for="DisplayOrder" class="form-control" />
            </div>
        </div>

        <div class="row">
            <div class="col-6 col-md-3">
                <button type="submit" class="btn btn-primary form form-control">Create</button>
            </div>
            <div class="col-6 col-md-3">
                <a asp-controller="Category" asp-action="Index" class="btn btn-secondary border-2 border-black  form-control">Back to list</a>
            </div>
        </div>
    </form>
    ```

    -> In de ActionResult moet je dus geen object van Category instantiëren, dit zal .NET core zelf doen via de **@model Category**

    -> We gebruiken in de labels & inputs **asp-for** om de properties van het object weer te geven & te binden.

    -> In de view zal de label weergegeven worden zoals de property zelf, indien je een ander naam wilt of spaties wilt toevoegen gaan we terug **annotatie** toevoegen.

    Toevoegen annotaties:

    ```csharp
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [DisplayName("Category Name")] // Layout voor view
        public string Name { get; set; }
        [DisplayName("Display Order")] // Layout voor view
        public int DisplayOrder { get; set; }
    }
    ```

## CRUD-functionaliteit (Database)

Nu willen we dat de **Create** button wel degelijk een Category aanmaakt en toevoegt in de database.

We gaan een nieuwe endpoint introduceren voor de **HTTP-POST** request.

- Wat zijn endpoints?

    Een endpoint= een URL + HTTP-methode die uitkomt op één van de methodes (actions.)

    Volgende zaken zijn eigenlijk allemaal endpoints:

    ```csharp
    public IActionResult Index()
    {
        List<Category> objCategoryList = _db.Categories.ToList();
        return View(objCategoryList);
    }

    public IActionResult Create()
    {
        return View();
    }
    [HttpPost]
    public IActionResult Create(Category obj)   
    {
        _db.Categories.Add(obj);
        _db.SaveChanges(); // werkelijke uitvoering
        return RedirectToAction("Index", "Category"); // Indien in dezelfde controller is ActionResult genoeg
    }
    ```

    1. ```csharp
       public IActionResult Index()
       ```

        Endpoint:

        ```GET /Category/Index```

        => Wat geberut er als dit endpoint 'geraakt' wordt?

        - Database wordt gelezen
        - Lijst met categorieën wordt opgehaald
        - View teruggestuurd met die lijst

            -> Een MVC endpoint die HTML terugstuurt.

    2. ```csharp
       public IActionResult Create()
       ```

        Endpoint:

        ```GET /Category/Create```

        => Dit wordt geraakt als iemand naar de "Create Page" gaat.

        - Geen database actie
        - Gewoon een formulier tonen

            -> Endpoint dat een formulierpagina toont.

    3. ```csharp
       [HttpPost]
       public IActionResult Create(Category obj)
       ```

        Endpoint:

        ```POST /Category/Create```

        => Dit wordt geraakt wanneer iemand op de submit button klikt in het formulier.

        Wat gebeurt er?

        1. Browser stuurt form data -> POST request
        2. Naar URL  ```/Category/Create```
        3. Dat is het endpoint
        4. Deze methode draait:

            ```csharp
            _db.Categories.Add(obj);
            _db.SaveChanges();
            ```

            -> Hier gebeurt **de echte actie** (data opslaan)

            Daarna:

            ```csharp
            return RedirectAction("Index", "Category");
            ```

            -> Browser wordt doorgestuurd naar GET /Category/Index (andere endpoint)
