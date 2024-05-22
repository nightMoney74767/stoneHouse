<h1>Introduction</h1>

<img src=https://github.com/nightMoney74767/stoneHouse/blob/main/J85452%20-%20CO5227%20Restaurant%20Project/wwwroot/img/LogoPNG.png width=200px/>

<strong>Logo of StoneHouse Restaurant (made with Draw.io)</strong>

This GitHub project contains the files of an ASP.Net Core website that has been created as part of an assignment at University of Chester. The files include the CSHTML pages, CSS and other resources where appropriate.
The wbesite has been built upon a simple HTML website I have created before the beginning of the second academic year, which you can find in the Original HTML Site folder.

This website, provides an online ordering system for a fictional restaurant. Menu items, order history and other data are stored in an SQL Server database (described further below). Optimisations have been made to ensure the best possible experience for mobile users. Users can optionally install the website as a Progressive Web App (PWA) using a supported web browser (e.g. Google Chrome and Microsoft Edge). PWAs offer an app-like experience (with no browser toolbars) with easy access from a mobile device's home screen or via a desktop operating system's app launcher.

<h1>Installation</h1>
If you'd like to run this project on your own device or webserver, please follow these steps:
<ol>
        <li>setup an SQL instance. For local databases, install <a href="https://learn.microsoft.com/en-us/sql/database-engine/configure-windows/sql-server-express-localdb?view=sql-server-ver16">SQL Express</a>. Run through the setup utility to install a local server with a username and password of your choice</li>
        <li>download and install <a href="https://learn.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms?view=sql-server-ver16">SQL Server Management Studio (SSMS)</a></li>
        <li>open SSMS and login to your SQL instance</li>
        <li>create a new query and run it with the <a href="https://github.com/nightMoney74767/stoneHouse/blob/main/Other%20Items/BuildTables.sql">BuildTables</a> script. This will setup your SQL instance with the required tables</li>
        <li>download and run the installer for <a href="https://visualstudio.microsoft.com/downloads/">Visual Studio</a>. The community edition is available for free. When asked, select the ASP.Net workload and click install</li>
        <li>download the project code and open the .sln file in Visual Studio</li>
        <li>open the appsettings.json file and modify it to reflect your SQL database</li>
        <li>go to the existing migration (in solution explorer) and copy the code for creating ASP.NET identity tables</li>
        <li>delete the existing migration then open the Nuget package manager (tools > Nuget packet manager > package manager console)</li>
        <li>run add-migration initialCreate and paste the copied code to the Up method</li>
        <li>run update-database and check that the tables were added to your database in SSMS</li>
        <li>to enable the admin functionality, run the project and go to the registration page. Create an account using the fictional address admin@stonehouse.co.uk and provide your own password. Close the browser window and open the startup.cs file. Uncomment the code that calls the CreateRoles method. Run the project again, log-in using the admin email and password. You should now be able to access the admin pages. You can comment out the code again</li>
      </ol>
      
<h1>Screenshots</h1>
Note: the website has two themes which are automatically set based on the settings of the user's web browser and/or operating system (i.e. the dark theme is used when the browser's theme is also dark). Screenshots have been taken within Windows 11 and Android 10, using the original styling which you can find <a href="https://github.com/nightMoney74767/stoneHouse/blob/main/J85452%20-%20CO5227%20Restaurant%20Project/wwwroot/css/StyleSheetOG.css">here</a>. Following a revisit of the project in 2023, the CSS was modified to provide a better mobile experience.

<h2>Light Theme</h2>
<img src=https://github.com/nightMoney74767/stoneHouse/blob/main/Other%20Items/ChromeLight2.png alt="Screenshots of the website in light theme"/>

<h2>Dark Theme</h2>
<img src=https://github.com/nightMoney74767/stoneHouse/blob/main/Other%20Items/ChromeDark.png alt="Screenshots of the website in dark theme" />

<h2>Progressive Web App (PWA)</h2>
<img src=https://github.com/nightMoney74767/stoneHouse/blob/main/Other%20Items/PWAWindows.png alt="Screenshots of the website when opened as a PWA" />

<h1>Pages</h1>
The following table outlines each CSHTML page, with descriptions on their functionality:

| Public-facing Pages | Description |
| ------ | ------ |
| Index | The homepage of the website |
| Checkout | Allows customers to view each item to be purchased and to proceed with said purchase |
| Contact | Displays contact details and information about the company. An interactive map is available (will be accompanied with a static version if JavaScript is disabled). A form is included to allow users to send messages to the company |
| Find | Allows users to find and display a list of restaurants that are open |
| Login | Allows users to sign into their accounts |
| Logout | Allows users to sign out of their accounts |
| Register | Allows new users to register accounts |
| AccessDenied | This page appears when any user other than an admin attempts to access an admin page |
| Menu | Displays a menu - sourced from a database |
| Offline | This page appears when a user access the website, installed as a PWA, without an internet connection |
| PurchaseConfirmed | This page appears after a user makes a purchase |
| Error | This page was generated upon creation of this ASP.NET Core project. Modifications have been made to match the design of other pages |

| Admin-only Pages | Description |
| ------ | ------ |
| Admin | The landing page that leads to other admin pages (with the word admin in the name). Allows admins to add new database data |
| AdminCustomer | Allows admins to view and delete a list of customers |
| AdminMenu | Allows admins to view and delete items in the menu |
| AdminOffer | Allows admins to view and delete offers |
| AdminOrderHistory | Allows admins to view and delete items in the order history |
| AdminOrderItems | Allows admins to view and delete items in the order items table |
| AdminRestaurant | Allows admins to view and delete restaurants |
| EditCustomer | Allows admins to edit customers |
| EditMenu | Allows admins to edit menu items |
| EditOffer | Allows admins to edit offers |
| EditOrderHistory | Allows admins to edit order history items |
| EditRestaurant | Allows admins to edit restaurants |


<h1>Database</h1>
The website uses an SQL database which includes the following tables:

- a menu
- an order history
- a list of restaurants
- a list of customers
- a list of offers

An entity relationship diagram (ERD) and an SQL script (called BuildTables.sql) have been designed in order to set up the database. These can be found in the Other Items folder. The ERD is also displayed below:
<img src="https://github.com/nightMoney74767/stoneHouse/blob/main/Other%20Items/CO5227ERD.png" alt="An image of the ERD"/>

This database has been expanded with additional tables, which store users' baskets and user information (e.g. usernames and hashed passwords). The migrations table was made as a result of using Visual Studio's database migration tool for use with ASP.NET Core's Identity service. The tables that make up the database are included below:
<img src="https://github.com/nightMoney74767/stoneHouse/blob/main/Other%20Items/SQL_Relationships__Main_.png" alt="An image of the ERD with new tables" />
<img src="https://github.com/nightMoney74767/stoneHouse/blob/main/Other%20Items/SQL_Relationships__AspNetUsers_.png" alt="An image featuring the tables used in the login system" />
