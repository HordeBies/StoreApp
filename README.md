# BiesStore

## Overview

BiesStore is an e-commerce book store project hosted on Github. The main goal of this project is to provide an online platform where users can buy and sell books. The project is built using `ASP.NET Core 8 MVC`, `Entity Framework (EF) Core`, `Microsoft SQL`, and `ASP.NET Core Identity`.


## Tech Stack

The project uses a variety of technologies to enhance its functionalities:

- `ASP.NET Core 8 MVC`: ASP.NET Core is a cross-platform, open-source web framework for building web applications. It provides a lightweight, high-performance, and modular approach to building web applications. ASP.NET Core MVC is a pattern for building web applications that separates the application into three main components: models, views, and controllers.

- `Entity Framework (EF) Core`: EF Core is a lightweight, cross-platform, and open-source version of Entity Framework. It is an object-relational mapping (ORM) framework that allows developers to work with databases using strongly-typed domain-specific objects.

- `Microsoft SQL`: Microsoft SQL Server is a relational database management system (RDBMS) developed by Microsoft. It is a powerful and reliable database engine that provides high-performance, scalability, and security for enterprise-level applications.

- `ASP.NET Core Identity`: ASP.NET Core Identity is a membership system that provides authentication and authorization functionalities to ASP.NET Core web applications. It allows developers to manage user authentication and authorization using a set of default UI templates and APIs.

Also project uses several external technologies to enhance its functionality and user experience. Here are some brief explanations of each:

- `Stripe`: This is a payment processing platform that allows for secure and easy online payments. It is used in the project to enable customers to purchase products via credit card.

- `Bootstrap`: This is a popular front-end framework that simplifies the process of designing responsive and mobile-friendly web pages. It is used in the project to create a consistent and visually appealing user interface.

- `DataTables`: This is a jQuery plugin that allows for the creation of dynamic tables with advanced features such as sorting, filtering, and pagination. It is used in the project to enable AJAX-based table views with CRUD (create, read, update, delete) operations.

- `TinyMCE`: This is a WYSIWYG (what you see is what you get) rich text editor that allows for the creation of formatted text, images, and other media content. It is used in the project to enable company users to create and edit product descriptions.

- `Toastr`: This is a JavaScript library that provides simple and elegant notification messages. It is used in the project to display notifications to users after certain actions have been performed, such as successful login, purchase, or error messages.

- `Sweet Alert 2`: This is a JavaScript library that creates beautiful, customizable modal windows that can be used for various purposes, such as confirmation dialogs or informational messages. It is used in the project to display modal windows for various user interactions.

These external technologies help the BiesStore project to provide a robust, efficient, and user-friendly e-commerce platform with advanced features and a modern design.

## Architecture

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

### Customer Role

`Customers` are users who visit the online bookstore to purchase books. They can browse through the various books available on the website and add books to their shopping cart. They can view and update their shopping cart, and buy the items in their cart via Stripe payment processing platform. When the purchase is complete, the shopping cart is converted into an order. Customers can also view their order details, track their orders, and confirm when they have received their orders. Additionally, they can cancel orders and request refunds.

### Company Role

`Companies` are users who are selling books through the BiesStore website. In addition to all the functionalities that customers have, company users can create product listings with a price, and edit or delete their product listings as needed.


### Admin Role

The `Admin` role has control over the entire operation of the online bookstore. This includes managing categories, products, and orders. The admin can add, edit, or delete categories and products. They can also view and manage all orders placed on the website, and take necessary actions on them, such as processing refunds or canceling orders.


By having these three distinct user roles, the BiesStore project ensures that the right level of access and permissions are granted to the appropriate user. This also helps in maintaining the overall security and integrity of the website.

## Upcoming Features

The project is currently being updated with new functionalities. Some of the upcoming features are:

- Google and GitHub external register/login options
- Deploying the project on Azure cloud
- Real email hooks to currently functional mock mailing system
- Session management

## Contributing

This project is open for contributions. If you would like to contribute to this project, please fork this repository and submit a pull request.

## License

This project is licensed under the [MIT License](https://opensource.org/licenses/MIT). See the LICENSE file for more details.