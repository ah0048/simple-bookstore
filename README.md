# Digital Library System

A simple ASP.NET Core MVC application for managing book collections and borrowers.

## Features

- 📚 Book management
- 👥 Borrower management
- 📖 Book borrowing system
- 🖼️ Photo upload support (Cloudinary integration)
- 📱 Responsive Bootstrap UI

## Tech Stack

- **Backend**: ASP.NET Core MVC
- **Database**: SQL Server with Entity Framework Core
- **Frontend**: Bootstrap, jQuery
- **Cloud Storage**: Cloudinary (for book cover images)
- **Architecture**: 4-layer architecture with Service & Repository pattern with Unit of Work, in addition to unifying responses using ServiceResult pattern

## Project Structure

```
bookstore/
├── Controllers/          # MVC Controllers
├── Models/              # Data models
├── ViewModels/          # View models
├── Services/            # Business logic services
├── Repositories/        # Data access layer
├── Views/               # Razor views
├── wwwroot/             # Static files
└── Migrations/          # Database migrations
```

## Getting Started

1. **Clone the repository**

   ```bash
   git clone https://github.com/yourusername/simple-bookstore.git
   cd simple-bookstore/bookstore
   ```

2. **Update connection string**

   - Modify `appsettings.json` with your SQL Server connection string

3. **Configure Cloudinary (optional)**

   - Add your Cloudinary settings to `appsettings.json`

4. **Run the application**

   ```bash
   dotnet run
   ```

5. **Access the app**
   - Open your browser and navigate to `https://localhost:7216`

## License

This project is for educational/hobby purposes.# simple-bookstore
