# **NZWalks 🏞️**  
A **full-stack web application** built with **ASP.NET Core Web API (.NET 8)** and **ASP.NET MVC**, designed to help users explore and manage walking trails in New Zealand.  

## **📌 Project Overview**  
NZWalks allows users to **browse, search, and manage walking trails**, providing key trail details, interactive maps, and secure authentication. The project follows **RESTful API principles**, **best practices**, and **clean coding techniques** to ensure scalability and maintainability.  

## **✨ Features**  
✅ **Built using .NET 8** – Latest **ASP.NET Core Web API** for high-performance RESTful APIs.  
✅ **ASP.NET MVC Frontend** – Consumes the API, providing an interactive user interface.  
✅ **CRUD Operations with EF Core** – Uses **Entity Framework Core (Code-First approach)** for database interactions.  
✅ **Repository Pattern & Domain-Driven Design (DDD)** – Enhances **code structure and maintainability**.  
✅ **Data Validation** – Implements **FluentValidation** to ensure input integrity.  
✅ **Authentication & Authorization** – Uses **JWT-based authentication** and **Role-Based Authorization** with **ASP.NET Core Identity**.  
✅ **Filtering, Sorting, and Pagination** – Improves API usability and performance.  
✅ **Automapper Integration** – Simplifies **object-to-object mapping** in API responses.  
✅ **Swagger & Postman Testing** – API documentation and testing using **Swagger UI & Postman**.  
✅ **Secure Deployment** – Deployed using **Azure**.  
✅ **Logging** – **Serilog** for error tracking and debugging.  

## **🛠 Tech Stack**  
🔹 **Backend:** ASP.NET Core Web API (.NET 8), Entity Framework Core  
🔹 **Frontend:** ASP.NET MVC  
🔹 **Database:** PostgreSQL / SQL Server  
🔹 **Architecture:** Repository Pattern, Domain-Driven Design (DDD)  
🔹 **Authentication:** JWT, ASP.NET Core Identity, Role-Based Authorization  
🔹 **Validation:** FluentValidation  
🔹 **Utilities:** Automapper, Swagger, Postman  
🔹 **Deployment & DevOps:** Azure
🔹 **Monitoring & Logging:** Serilog

## **🚀 Getting Started**  

### **1️⃣ Clone the Repository**  
```bash
git clone https://github.com/AkashWarkhad/NZWalks.git
cd NZWalks
```

### **2️⃣ Setup the Backend (ASP.NET Core API)**  
- Configure the **PostgreSQL connection** in `appsettings.json`.  
- Run database migrations:  
```bash
dotnet ef database update
```  
- Start the API:  
```bash
dotnet run
```

## **🤝 Contributing**  
Contributions are welcome! Feel free to open issues or submit PRs.  

## **🔗 Links**  
📂 **GitHub Repository:** [NZWalks](https://github.com/AkashWarkhad/NZWalks)  
