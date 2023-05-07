# [Bies Store](https://bies-bookstore.azurewebsites.net/)
BiesStore is an e-commerce bookstore built with ASP.NET Core MVC, using Entity Framework Core and Microsoft SQL Server as the backend technologies. The project uses ASP.NET Core Identity for authentication and authorization, and follows the repository and unit of work pattern for data access. The project is structured using n-tier architecture, with separate projects for the web application, models, data access, and utilities.

# Project Description
BiesStore allows customers to browse and purchase books, while companies can sell their products through the platform. The project consists of three roles: Customer, Company, and Admin. Customers can add products to the shopping cart, view and update the shopping cart, and complete orders via Stripe payment processing. Companies can add product listings, edit or delete existing listings, and manage their orders. Admins have control over the entire operation, including categories, products, and orders.

# External Technologies
The project uses DataTables for AJAX tables with CRUD operations, TinyMCE for rich text editing, Bootstrap for UI building and theming, Toastr for toast notifications, Sweet Alert 2 for modal popup windows, Stripe for payment processing, and now has email verification and order notification capabilities through MimeKit and MailKit.

# Deployment
BiesStore is now deployed on Azure, including both the application and database. Users can access the platform at https://bies-bookstore.azurewebsites.net/

**Disclaimer: Deployed project still uses test stripe therefore can be bypassed with 4111111111111111 as a card number or with any other valid test credit card number**

# Google and Github External Login Options
BiesStore now has the added functionality of Google and GitHub external login options, allowing users to register and sign in using their Google or GitHub accounts.

# Session Management
BiesStore now uses session management to keep track of the number of items in the shopping cart displayed in the navbar. The project uses the ASP.NET Core built-in session middleware to create and manage user sessions.

# Contact
Please feel free to contact me with any questions or feedback.

# Contributing

This project is open for contributions. If you would like to contribute to this project, please fork this repository and submit a pull request.


## Detailed Tech Stack

The project uses a variety of technologies to enhance its functionalities:

- `ASP.NET Core 8 MVC`
- `Entity Framework (EF) Core`
- `Microsoft SQL`
- `ASP.NET Core Identity`

Also project uses several external technologies to enhance its functionality and user experience:
- `Stripe`
- `Bootstrap`
- `DataTables`
- `TinyMCE`
- `Toastr`
- `Sweet Alert 2`

These external technologies help the BiesStore project to provide a robust, efficient, and user-friendly e-commerce platform with advanced features and a modern design.

## Detailed Architecture

BiesStore project uses an n-tier architecture with separate projects for different layers. Here is a brief explanation of each layer and its purpose:

- `Web Layer`: This is the top layer of the architecture and is responsible for handling user requests and displaying responses. The Web Layer in the BiesStore project is implemented using ASP.NET Core MVC, which provides a framework for building web applications.

- `Models Layer`: This layer contains the business entities or models used in the application. These models are responsible for representing the data used by the application, such as user data, product data, or order data. The Models Layer in the BiesStore project is implemented as a separate project and is used to store all the POCOs (plain old C# objects) that represent the entities.

- `DataAccess Layer`: This layer is responsible for interacting with the database and performing CRUD (create, read, update, delete) operations on the data. The DataAccess Layer in the BiesStore project is implemented as a separate project and uses ASP.NET Core EF Core for ORM (object-relational mapping) and MSSQL for database management.

- `Utility Layer`: This layer contains utility classes and functions that are used across the application. The Utility Layer in the BiesStore project is implemented as a separate project and contains classes such as email service, logging service, and Stripe payment service.

By dividing the application into these separate layers, the n-tier architecture provides several benefits, such as separation of concerns, scalability, maintainability, and testability. It also allows for easier deployment of the application components and makes it easier to switch out components if necessary.
The repository and unit of work patterns are commonly used in n-tier architecture to manage data access.

In the BiesStore project, the `repository pattern` is used to encapsulate the logic for interacting with the data store. This pattern separates the logic for retrieving data from the actual implementation of the data store. In other words, the repository pattern allows the application to interact with the data store through an abstraction layer, without having to know the underlying implementation details.

The `unit of work` pattern is used to manage the transactional behavior of the application. This pattern groups together all the database operations into a single transaction, so that either all the operations are completed successfully or none of them are. This helps to ensure data consistency and integrity in the database.

By using these patterns, the BiesStore project can achieve better separation of concerns, maintainability, and testability of the data access layer. The repository pattern allows for easy substitution of the actual data store implementation, while the unit of work pattern ensures that data consistency and integrity is maintained during transactions.

## Roles

The BiesStore project consists of three user roles: `Customer`, `Company`, and `Admin`. 

By having these three distinct user roles, the BiesStore project ensures that the right level of access and permissions are granted to the appropriate user. This also helps in maintaining the overall security and integrity of the website.

## External Login Options

In addition to the standard local authentication provided by ASP.NET Core Identity, BiesStore now supports external logins through GitHub and Google. This allows users to sign in with their existing GitHub or Google account, making the registration and login process faster and more convenient. To implement this feature, the project utilizes the ASP.NET Core Identity framework's built-in support for external authentication providers. The implementation involves configuring the project to use the OAuth-based authentication flow with the desired providers, creating client IDs and secrets for each provider, and modifying the user interface to display the external login options. With this feature, BiesStore offers a more seamless and flexible authentication experience for its users.

## Email Providers

BiesStore now supports email verification and order confirmation using MimeKit and MailKit. With this feature, users can receive email notifications for account verification, order details, and other important updates. MimeKit is an open-source .NET library that provides a high-level API for creating and sending email messages. MailKit is a .NET email library that supports both POP3 and IMAP protocols. To implement this feature, the project uses the MimeKit and MailKit libraries to create and send email messages, along with ASP.NET Core Identity's built-in email confirmation and password recovery features. This integration allows BiesStore to provide a reliable and secure email service for its users.

## Session Management

BiesStore now uses session management to keep track of the number of items in the shopping cart displayed in the navbar. Session management is an essential feature of web applications that allows you to store and retrieve user-specific data across multiple requests. With this feature, users can see the number of items in their cart at a glance, without having to navigate to the shopping cart page. To implement this feature, the project uses the ASP.NET Core built-in session middleware to create and manage user sessions. Additionally, the project modifies the navbar view to display the cart item count, which is stored in the session data. With this feature, BiesStore provides a more user-friendly and convenient shopping experience for its customers.

## Deployment

BiesStore is now deployed to Azure, including both the application and database. Azure is a cloud computing platform that provides a wide range of services for hosting, managing, and scaling web applications. With this deployment, users can access BiesStore from anywhere with an internet connection, and the project benefits from the scalability, reliability, and security of the Azure platform. To deploy the project, we used the Azure portal to create an App Service plan, a Web App, and a SQL Database. Then, we configured the Web App to connect to the database using the connection string provided by Azure. Finally, we published the project to the Web App using Visual Studio, which automatically deployed the necessary files and dependencies to Azure. With this deployment, BiesStore is now a fully functional and accessible e-commerce bookstore.

## License

This project is licensed under the [MIT License](https://opensource.org/licenses/MIT). See the LICENSE file for more details.
