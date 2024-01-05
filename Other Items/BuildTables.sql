DROP TABLE IF EXISTS OrderHistory
DROP TABLE IF EXISTS Offer
DROP TABLE IF EXISTS Customer
DROP TABLE IF EXISTS Menu
DROP TABLE IF EXISTS Restaurant

/* Create table for restaurant information */
CREATE TABLE Restaurant (
	RestaurantID int NOT NULL,
	RestaurantName nvarchar(25),
	RestaurantStreetName nvarchar(25),
	RestaurantParish nvarchar(25),
	RestaurantCity nvarchar(25),
	RestaurantCounty nvarchar(25),
	RestaurantPostCode nvarchar(8),
	RestaurantPhone int,
	RestaurantEmail nvarchar(50),

	/* Primary key */
	CONSTRAINT PKRestaurantID PRIMARY KEY (RestaurantID),
)

/* Create table for menu information */
CREATE TABLE Menu (
	ItemID int NOT NULL,
	ItemName nvarchar(25),
	ItemDescription nvarchar(100),
	NumOfItemsAvailable int,
	ItemPrice money,
	ItemImage varBinary(MAX),

	/* Primary key */
	CONSTRAINT PKItemID PRIMARY KEY (ItemID)
)

/* Create table for customer information */
CREATE TABLE Customer (
	CustomerID int NOT NULL,
	CustomerName nvarchar(25),
	CustomerStreetName nvarchar(25),
	CustomerParish nvarchar(25),
	CustomerCity nvarchar(25),
	CustomerCounty nvarchar(25),
	CustomerPostCode nvarchar(8),
	CustomerPhone int,
	CustomerEmail nvarchar(50),

	/* Primary key */
	CONSTRAINT PKCustomerID PRIMARY KEY (CustomerID),
)

/* Create table for offer information */
CREATE TABLE Offer (
	OfferID int NOT NULL,
	OfferName nvarchar(25),
	OfferDescription nvarchar(100),
	OfferStart nvarchar(10),
	OfferExpiry nvarchar(10),
	ReductionPercentage int,

	/* Primary key */
	CONSTRAINT PKOfferID PRIMARY KEY (OfferID)
)

/* Create table for ofer history */
CREATE TABLE OrderHistory (
	OrderID int NOT NULL,
	CustomerID int NOT NULL,
	ItemID int NOT NULL,
	Quantity int,
	FinalPrice money,
	RestaurantID int NOT NULL,
	OfferID int NOT NULL,

	/* Primary key */
	CONSTRAINT PKOrderID PRIMARY KEY (OrderID),

	/* Foreign keys */
	CONSTRAINT FKCustomerID FOREIGN KEY (CustomerID) REFERENCES Customer (CustomerID),
	CONSTRAINT FKItemID FOREIGN KEY (ItemID) REFERENCES Menu (ItemID),
	CONSTRAINT FKRestaurantID FOREIGN KEY (RestaurantID) REFERENCES Restaurant (RestaurantID),
	CONSTRAINT FKOfferID FOREIGN KEY (OfferID) REFERENCES Offer (OfferID)
)

/* Insert data which should appear when retrieved */
INSERT INTO Restaurant(RestaurantID, RestaurantName, RestaurantStreetName, RestaurantParish, RestaurantCity, RestaurantCounty, RestaurantPostCode, RestaurantPhone, RestaurantEmail)
VALUES (1, 'Parkgate House', '10 Wilko Street', 'Molloy', 'Chester', 'West Cheshire', 'CH1 2D3', 01244123456, 'parkgate@stonehouse.co.uk')
INSERT INTO Restaurant(RestaurantID, RestaurantName, RestaurantStreetName, RestaurantParish, RestaurantCity, RestaurantCounty, RestaurantPostCode, RestaurantPhone, RestaurantEmail)
VALUES (2, 'The Seventy Seven', '37 Blue Lane', 'Seaborne', 'Chester', 'West Cheshire', 'CH6 1Q1', 01244123982, 'seven@stonehouse.co.uk')

INSERT INTO Menu(ItemID, ItemName, ItemDescription, NumOfItemsAvailable, ItemPrice)
VALUES (1, 'MenuItemExample', 'An example item which should appear', 1, 1.99)

INSERT INTO Customer(CustomerID, CustomerName, CustomerStreetName, CustomerParish, CustomerCity, CustomerCounty, CustomerPostCode, CustomerPhone, CustomerEmail)
VALUES (1, 'Example Customer', '14 Wilko Street', 'Molloy', 'Chester', 'West Cheshire', 'CH1 2D4', 01244123411, 'example.customer@gmail.com')

INSERT INTO Offer(OfferID, OfferName, OfferDescription, OfferStart, OfferExpiry, ReductionPercentage)
/* Though it is possible to use the date format as suggested by W3Schools (n.d), it is easier to use nvarchar, which works with strings in C#. The date format can then match the British format (day, month, year)*/
VALUES (1, 'Halloween Special', '50% off everything', '25-10-2021', '01-11-2021', 50)

INSERT INTO OrderHistory(OrderID, CustomerID, ItemID, Quantity, FinalPrice, RestaurantID, OfferID)
VALUES (1, 1, 1, 1, 1.99, 1, 1)

/* References:
W3Schools. (n.d). SQL Working With Dates. Retrieved from: https://www.w3schools.com/sql/sql_dates.asp
*/