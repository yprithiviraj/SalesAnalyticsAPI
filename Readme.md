# 📊 Sales Revenue API

An ASP.NET Core Web API to calculate sales revenue reports by **Product**, **Category**, and **Region** from CSV order data.

---

Prerequisites
-----------------

| Tech                           | Version / Info            |
|:-------------------------------|:--------------------------|
| **.NET SDK**                   | 8.0+                      |
| **Entity Framework Core**      | 8.0+                      |
| **SQL Server**                 | Any local / cloud instance|
| **Postman** (for testing APIs) | Optional                  |



How to Run
------------

1. Open Git Bash (or any terminal) and run:
   ```git clone https://github.com/yprithiviraj/SalesAnalyticsAPI.git```
   cd sales-revenue-api
1. Open the ```ApplicationDbContextFactory.cs``` file and modify the ```optionsBuilder.UseSqlServer()``` line to point to your local or desired SQL Server connection string:
1. Example : ```optionsBuilder.UseSqlServer("Server=localhost;Database=SalesRevenueDb;Trusted_Connection=True;TrustServerCertificate=True;");```


2. Restore dependencies using ```dotnet restore```
3. Apply migration using ```dotnet ef database update```
4. Run the application using ```dotnet run```

Files
-------

For demonstration purposes, a sample CSV file is included in the repository under the Files directory. 

The API will use this file to import data whenever the import service is triggered — either via an API call or a scheduled job. 

This implementation is intentionally kept simple for now, 
but it can be easily extended in the future to allow users to upload their own CSV files or fetch files from external sources like Amazon S3 or other storage services.

Expected CSV Columns : OrderId, ProductId, ProductName, Category, Region, DateOfSale, QuantitySold, UnitPrice
Sample CSV Rows : 1, 101, "Laptop", "Electronics", "West", "2024-06-12", 2, 500.00


Data Refresh Mechanism
-----------------------
New data can be imported anytime via ```/api/sales/import```

Existing data can be updated

Revenue reports automatically reflect latest imported data

Notes
----------------
Records from CSV will be fetched async to reduce memory load and used batch mechanism while saving to the database to improve performance

Indexes applied on Orders.DateOfSale, Orders.Region, and Orders.ProductId for query optimization

Uses AsNoTracking() for read-only queries to improve performance

Includes error handling for invalid file uploads and missing parameters

Graceful API responses with appropriate HTTP status codes

API Details
------------------

| **API Name**                  | **Route**                          | **Method** | **Body (if any)**         | **Sample Response**                                                       | **Description**                                                             |
|:------------------------------|:-----------------------------------|:-----------|:--------------------------|:--------------------------------------------------------------------------|:------------------------------------------------------------------------------|
| Import CSV                    | `/api/sales/import`                  | `POST`     | `filePath` (optional query param or JSON body if extended) | `"CSV import initiated."`                                                 | Triggers CSV import process. Currently reads from a static file location or scheduled service. |
| Get Total Revenue             | `/api/analysis/total-revenue`        | `GET`      | `startDate`, `endDate` (query params)                      | `120000.00`                                                               | Fetches total revenue between the provided date range.                     |
| Get Revenue by Product        | `/api/analysis/revenue-by-product`   | `GET`      | `startDate`, `endDate` (query params)                      | `[{ "productId": 1, "productName": "Item A", "totalRevenue": 50000.00 }]` | Fetches total revenue grouped by product within the specified date range. |
| Get Revenue by Category       | `/api/analysis/revenue-by-category`  | `GET`      | `startDate`, `endDate` (query params)                      | `[{ "category": "Electronics", "totalRevenue": 80000.00 }]`               | Fetches total revenue grouped by category within the date range.           |
| Get Revenue by Region         | `/api/analysis/revenue-by-region`    | `GET`      | `startDate`, `endDate` (query params)                      | `[{ "region": "North", "totalRevenue": 70000.00 }]`                       | Fetches total revenue grouped by region within the date range.             |

Sample Calls
--------------

https://localhost:7076/api/analysis/total-revenue?startDate=2022-01-01&endDate=2025-12-31
https://localhost:7076/api/analysis/revenue-by-category?startDate=2022-01-01&endDate=2025-12-31
https://localhost:7076/api/analysis/revenue-by-product?startDate=2022-01-01&endDate=2025-12-31
https://localhost:7076/api/analysis/revenue-by-region?startDate=2022-01-01&endDate=2025-12-31

Schema
----------

The schema diagram can be found under Data folder as Schema.png

Assumptions
-----------------

```order.QuantitySold * order.UnitPrice * (1 - order.Discount)```
Total revenue is calculated as the product of quantity sold and unit price, adjusted for any discounts (discount as a percentage value).
