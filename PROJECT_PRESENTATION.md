# 🎓 Project Presentation Guide: CanteenReview
> **Campus Canteen Menu & Student Review Web Application**  
> Built with **.NET 10 (C#)**, **ASP.NET Core Razor Pages**, **Entity Framework Core 10**, and **SQL Server**.

---

## 📋 Table of Contents
1. [Project Overview & Architecture](#-project-overview--architecture)
2. [Tech Stack & Tools Used](#-tech-stack--tools-used)
3. [Database Schema & Data Models](#-database-schema--data-models)
4. [.NET Razor Pages Breakdown](#-net-razor-pages-breakdown)
5. [Complete Master Reference: Functions & Methods](#-complete-master-reference-functions--methods)
6. [Key Features & Business Logic](#-key-features--business-logic)
7. [How to Run & Demo](#-how-to-run--demo)

---

## 🏗️ Project Overview & Architecture

**CanteenReview** is a full-stack, enterprise-grade ASP.NET Core Razor Pages web application designed for campus dining halls and canteens. It empowers students to browse food items, check real-time ratings, filter by categories, write detailed reviews with 1–5 star ratings, and upload item images or select food emojis.

### Architectural Pattern
* **Pattern**: PageModel (MVVM variant native to ASP.NET Core Razor Pages)
* **ORM**: Entity Framework Core 10.0.11 (Code-First Approach)
* **Database**: Microsoft SQL Server
* **Dependency Injection**: Built-in .NET IoC container for `DbContext` and `IWebHostEnvironment`

---

## 🛠️ Tech Stack & Tools Used

### Backend Framework & Libraries
* **.NET 10.0 SDK** (`Microsoft.NET.Sdk.Web`) — Latest high-performance .NET runtime.
* **C# 13** — Primary language featuring strongly-typed models, nullability checks, LINQ, and async/await.
* **Entity Framework Core 10.0.11**:
  * `Microsoft.EntityFrameworkCore`
  * `Microsoft.EntityFrameworkCore.SqlServer`
  * `Microsoft.EntityFrameworkCore.Tools`
  * `Microsoft.EntityFrameworkCore.Design`
  * `Microsoft.VisualStudio.Web.CodeGeneration.Design`

### Frontend & UI Design
* **Razor Views (`.cshtml`)** — Server-side HTML rendering engine with tag helpers (`asp-for`, `asp-page`, `asp-route-*`, `asp-validation-summary`).
* **Custom Vanilla CSS (`site.css`)** — Glassmorphism cards, custom orange/amber gradient accents, responsive grid, dynamic star pickers.
* **Bootstrap 5.3** — Responsive layout framework.
* **Bootstrap Icons 1.11** — Vector iconography for star ratings, food badges, search, edit, delete, and metrics.
* **jQuery & jQuery Validation** (`_ValidationScriptsPartial.cshtml`) — Client-side form validation.

---

## 🗄️ Database Schema & Data Models

Location: [`Models/CanteenModels.cs`](file:///e:/CanteenReview/Models/CanteenModels.cs) | [`Data/CanteenContext.cs`](file:///e:/CanteenReview/Data/CanteenContext.cs)

### 1. Entity: `FoodItem`
Represents a canteen food menu item.

| Field | Data Type | Validation / Constraints | Description |
| :--- | :--- | :--- | :--- |
| `Id` | `int` | Primary Key, Identity | Unique identifier for food item. |
| `Name` | `string` | Required, Max 100 chars | Name of the dish (e.g., Samosa, Chai). |
| `Price` | `decimal(18,2)` | Required, Range 0.01 – 1000.00 | Item price in Currency standard (₹). |
| `Category` | `string` | Max 50 chars, Default `"Snacks"` | Category (e.g., Snacks, Meals, Beverages). |
| `Description` | `string` | Max 500 chars | Ingredients and item details. |
| `ImageUrl` | `string` | Optional | Uploaded relative image path (`/uploads/guid.png`) OR Emoji (`🥟`). |
| `Reviews` | `List<Review>` | Navigation Property | Collection of student reviews attached to this item. |
| `AverageRating` | `double` | `[NotMapped]` Property | Dynamically computes average rating rounded to 1 decimal place. |
| `ReviewCount` | `int` | `[NotMapped]` Property | Returns count of attached student reviews. |
| `IsImageFileOrUrl`| `bool` | `[NotMapped]` Property | Boolean helper detecting whether `ImageUrl` is a file path or emoji. |

### 2. Entity: `Review`
Represents student feedback and ratings.

| Field | Data Type | Validation / Constraints | Description |
| :--- | :--- | :--- | :--- |
| `Id` | `int` | Primary Key, Identity | Unique identifier for review. |
| `StudentName` | `string` | Required, Max 100 chars | Name of the student submitting review. |
| `Rating` | `int` | Required, Range 1 – 5 | Star rating (1 to 5 stars). |
| `Comment` | `string` | Required, Max 1000 chars | Textual feedback comment. |
| `CreatedAt` | `DateTime` | Default `DateTime.Now` | Timestamp of submission. |
| `FoodItemId` | `int` | Foreign Key (Required) | Refers to `FoodItem.Id`. |
| `FoodItem` | `FoodItem?` | Navigation Property | Reference to parent `FoodItem`. Cascade Delete enabled. |

---

## 📄 .NET Razor Pages Breakdown

### 1. `Pages/Index.cshtml` & `Pages/Index.cshtml.cs` (Home & Menu Catalog)
* **Route**: `/` or `/Index`
* **Purpose**: Main student dashboard displaying menu items grid, interactive search bar, category dropdown, campus rating statistics, and action links.
* **Key PageModel Properties**:
  * `FoodItems` (`IList<FoodItem>`): Filtered list of food items loaded from database.
  * `SearchTerm` (`string?`): Bound via `[BindProperty(SupportsGet = true)]` for query search.
  * `SelectedCategory` (`string?`): Bound via `[BindProperty(SupportsGet = true)]` for dropdown filter.
  * `Categories` (`List<string>`): Distinct list of available categories.
  * `TotalItems` (`int`), `TotalReviews` (`int`), `OverallAvgRating` (`double`): Overall canteen stats.
* **Page Handlers**:
  * [`OnGetAsync()`](file:///e:/CanteenReview/Pages/Index.cshtml.cs#L36-L67): Queries categories, applies LINQ search filter (`Contains`), filters by category, computes aggregate stats, and renders the catalog.

---

### 2. `Pages/FoodItems/Create.cshtml` & `Pages/FoodItems/Create.cshtml.cs` (Add New Food Item)
* **Route**: `/FoodItems/Create`
* **Purpose**: Form page for canteen admins/staff to list a new food item, supporting file uploads or emoji avatars.
* **Key PageModel Properties**:
  * `FoodItem` (`FoodItem`): Model bound to form input fields.
  * `ImageFile` (`IFormFile?`): File uploaded via HTML `<input type="file" />`.
* **Page Handlers**:
  * [`OnGet()`](file:///e:/CanteenReview/Pages/FoodItems/Create.cshtml.cs#L25-L28): Prepares blank creation form.
  * [`OnPostAsync()`](file:///e:/CanteenReview/Pages/FoodItems/Create.cshtml.cs#L36-L78): Validates inputs, checks allowed image extensions (`.jpg`, `.png`, `.webp`, `.svg`, etc.), creates `wwwroot/uploads` directory if missing, saves uploaded image with GUID filename, sets fallback emoji if empty (`🍲`), adds item to EF Core context, and redirects to `/Index`.

---

### 3. `Pages/FoodItems/Details.cshtml` & `Pages/FoodItems/Details.cshtml.cs` (Item Details & Reviews)
* **Route**: `/FoodItems/Details/{id:int}`
* **Purpose**: Detailed item view showing item info, full student review timeline, rating distribution, and interactive submission form for adding new student reviews or deleting existing ones.
* **Key PageModel Properties**:
  * `FoodItem` (`FoodItem`): Item loaded with EF Core `.Include(f => f.Reviews)`.
  * `NewReview` (`Review`): Bound model for submitting new student reviews.
* **Page Handlers**:
  * [`OnGetAsync(int? id)`](file:///e:/CanteenReview/Pages/FoodItems/Details.cshtml.cs#L26-L45): Loads item by ID with attached reviews. Returns `404 NotFound` if invalid.
  * [`OnPostAddReviewAsync(int id)`](file:///e:/CanteenReview/Pages/FoodItems/Details.cshtml.cs#L47-L75): Validates and inserts a student review into `CanteenContext.Reviews`, assigns timestamp, and redirects back to refresh the page.
  * [`OnPostDeleteReviewAsync(int reviewId, int foodItemId)`](file:///e:/CanteenReview/Pages/FoodItems/Details.cshtml.cs#L77-L87): Deletes a specific review by ID and reloads item details.

---

### 4. `Pages/FoodItems/Edit.cshtml` & `Pages/FoodItems/Edit.cshtml.cs` (Update Food Item)
* **Route**: `/FoodItems/Edit/{id:int}`
* **Purpose**: Allows updating item details (Name, Category, Price, Description, Image/Emoji).
* **Key PageModel Properties**:
  * `FoodItem` (`FoodItem`): Item being edited.
  * `ImageFile` (`IFormFile?`): Optional replacement image file.
* **Page Handlers**:
  * [`OnGetAsync(int? id)`](file:///e:/CanteenReview/Pages/FoodItems/Edit.cshtml.cs#L32-L46): Fetches existing item from database for editing.
  * [`OnPostAsync()`](file:///e:/CanteenReview/Pages/FoodItems/Edit.cshtml.cs#L48-L121): Validates inputs, handles replacement file uploads, deletes old image file from `wwwroot/uploads/` if replaced, updates entity state in EF Core (`EntityState.Modified`), handles concurrency exceptions via `FoodItemExists(id)`, and redirects to `/Index`.
  * [`FoodItemExists(int id)`](file:///e:/CanteenReview/Pages/FoodItems/Edit.cshtml.cs#L123-L126): Helper method checking DB existence.

---

### 5. `Pages/FoodItems/Delete.cshtml` & `Pages/FoodItems/Delete.cshtml.cs` (Delete Food Item)
* **Route**: `/FoodItems/Delete/{id:int}`
* **Purpose**: Confirmation view before deleting a food item and all its associated reviews.
* **Key PageModel Properties**:
  * `FoodItem` (`FoodItem`): Item to be deleted.
* **Page Handlers**:
  * [`OnGetAsync(int? id)`](file:///e:/CanteenReview/Pages/FoodItems/Delete.cshtml.cs#L26-L44): Loads item details for user confirmation.
  * [`OnPostAsync(int? id)`](file:///e:/CanteenReview/Pages/FoodItems/Delete.cshtml.cs#L46-L74): Deletes associated image file from disk, removes `FoodItem` entity (which triggers cascading deletion of reviews in database), saves changes, and redirects to `/Index`.

---

### 6. Supporting System Pages
* [`Pages/Shared/_Layout.cshtml`](file:///e:/CanteenReview/Pages/Shared/_Layout.cshtml): Master HTML template, navigation bar, brand logo, footer, and asset links.
* [`Pages/Privacy.cshtml`](file:///e:/CanteenReview/Pages/Privacy.cshtml): Canteen privacy policy page.
* [`Pages/Error.cshtml`](file:///e:/CanteenReview/Pages/Error.cshtml): System error view with `RequestId` trace logging.

---

## 📊 Complete Master Reference: Functions & Methods

| Class / Component | Method / Handler Name | Parameters | Return Type | Description |
| :--- | :--- | :--- | :--- | :--- |
| **`Program.cs`** | Main Execution Pipeline | `string[] args` | `void` | Configures services, registers `CanteenContext` with SQL Server connection string, runs automatic DB seeding, maps Razor Pages. |
| **`DbInitializer`** | `Initialize(CanteenContext context)` | `CanteenContext` | `void` | Ensures DB is created (`EnsureCreated()`) and seeds initial canteen menu items (Samosa, Dosa, Burger, Chai, etc.) with pre-populated reviews. |
| **`CanteenContext`**| `OnModelCreating(ModelBuilder modelBuilder)` | `ModelBuilder` | `void` | Configures decimal precision `decimal(18,2)` for `Price` and configures One-to-Many Cascade Delete relationship between `FoodItem` and `Review`. |
| **`FoodItem`** | `AverageRating` `[NotMapped]` | None | `double` | Getter property calculating average star rating rounded to 1 decimal place. Returns `0` if no reviews exist. |
| **`FoodItem`** | `ReviewCount` `[NotMapped]` | None | `int` | Getter property returning total review count. |
| **`FoodItem`** | `IsImageFileOrUrl` `[NotMapped]` | None | `bool` | Helper checking if `ImageUrl` starts with `http`, `https`, `/uploads/`, `data:image`, or ends with image file extensions (`.png`, `.jpg`, etc.). |
| **`IndexModel`** | `OnGetAsync()` | None | `Task` | Fetches categories, executes LINQ search and category filter, computes overall stats, and populates food list. |
| **`CreateModel`** | `OnGet()` | None | `IActionResult` | Renders blank food item creation page. |
| **`CreateModel`** | `OnPostAsync()` | None | `Task<IActionResult>` | Validates uploaded file, saves image to `wwwroot/uploads/`, sets default emoji fallback, saves `FoodItem` entity, redirects to Index. |
| **`DetailsModel`** | `OnGetAsync(int? id)` | `int? id` | `Task<IActionResult>` | Loads item details along with attached student reviews sorted by date. |
| **`DetailsModel`** | `OnPostAddReviewAsync(int id)` | `int id` | `Task<IActionResult>` | Handler bound to review submit form. Validates review, attaches timestamp and foreign key, saves to DB. |
| **`DetailsModel`** | `OnPostDeleteReviewAsync(int reviewId, int foodItemId)` | `int reviewId, int foodItemId` | `Task<IActionResult>` | Deletes specific review by ID and reloads details page. |
| **`EditModel`** | `OnGetAsync(int? id)` | `int? id` | `Task<IActionResult>` | Loads food item record into form for editing. |
| **`EditModel`** | `OnPostAsync()` | None | `Task<IActionResult>` | Processes replacement image file, deletes old file from disk, updates entity in EF Core, handles concurrency. |
| **`EditModel`** | `FoodItemExists(int id)` | `int id` | `bool` | Helper method checking whether food item ID exists in database. |
| **`DeleteModel`** | `OnGetAsync(int? id)` | `int? id` | `Task<IActionResult>` | Loads item confirmation page before deletion. |
| **`DeleteModel`** | `OnPostAsync(int? id)` | `int? id` | `Task<IActionResult>` | Deletes local upload file, deletes food item entity (cascades to reviews), redirects to Index. |
| **`ErrorModel`** | `OnGet()` | None | `void` | Captures current trace/activity ID for diagnostic error display. |

---

## 🌟 Key Features & Business Logic

1. **Direct Image File Upload System**:
   * Users upload food dish photos (`.jpg`, `.jpeg`, `.png`, `.webp`, `.gif`, `.svg`) via HTML `<input type="file" />`.
   * Uploaded files are validated for security, saved with GUID filenames in `wwwroot/uploads/`, and evaluated dynamically via `FoodItem.IsImageFileOrUrl`.

2. **Automatic Physical File Cleanup**:
   * When an item image is replaced (Edit page) or when an item is deleted (Delete page), the system automatically identifies uploaded files in `wwwroot/uploads/` and deletes them from the server filesystem using `System.IO.File.Delete`.

3. **Cascading Relational Integrity**:
   * Built using Entity Framework Core Fluent API (`OnDelete(DeleteBehavior.Cascade)`). Deleting a food item automatically cleans up all associated review records in SQL Server.

4. **Live Search & Dynamic Category Filtering**:
   * Uses LINQ `AsQueryable()` to compose SQL queries dynamically based on `SearchTerm` and `SelectedCategory`.

5. **Client & Server-Side Form Validation**:
   * Data Annotations (`[Required]`, `[StringLength]`, `[Range]`, `[DataType]`) prevent bad data entries on both client (jQuery) and server.

---

## 🚀 How to Run & Demo

### Prerequisites
* .NET 10 SDK installed
* SQL Server / LocalDB installed

### Execution Steps
1. Open terminal in project directory:
   ```bash
   cd e:\CanteenReview
   ```
2. Run database initialization and start dev server:
   ```bash
   dotnet run
   ```
3. Open browser at:
   `https://localhost:7198` or `http://localhost:5198` (as displayed in console log).

---
*Created for CanteenReview Project Presentation.*
