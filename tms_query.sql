DROP SCHEMA IF EXISTS tms;
CREATE SCHEMA tms;
USE tms;

CREATE TABLE users (
	userID SMALLINT NOT NULL AUTO_INCREMENT,
	name VARCHAR(50) NOT NULL,
	password VARCHAR(50) NOT NULL,
	userType VARCHAR(50),
    PRIMARY KEY(userID)
    );
	
INSERT INTO users VALUES (1, 'admin1', 'Conestoga1', 'admin'),
(2, 'buyer1', 'Conestoga1', 'buyer'),
(3, 'planner1', 'Conestoga1', 'planner');

CREATE TABLE Customer (
	CustomerID SMALLINT NOT NULL AUTO_INCREMENT,
	CustomerName VARCHAR(50) NOT NULL,
	Address varchar(100),
    PRIMARY KEY(CustomerID)
    );
    
INSERT INTO Customer VALUES 
(1001, 'Atlantis Railway', '1300 Highway'),
(1002, 'MacDongles', '2500 Bleans Road'),
(1003, 'Farm Supply Co', '1 Glen Street');

CREATE TABLE Carriers (
	cID SMALLINT NOT NULL AUTO_INCREMENT,
	cName VARCHAR(50) NOT NULL,
	dCity VARCHAR(50) NOT NULL,
    dCityNo int,
	FTLA SMALLINT NOT NULL,
	LTLA SMALLINT NOT NULL,
	FTLRate DECIMAL(5,2) NOT NULL,
	LTLRate DECIMAL(5,4) NOT NULL,
	reefCharge DECIMAL(5,3) NOT NULL,
    PRIMARY KEY(cID)
    );
	
	
INSERT INTO Carriers VALUES (1, 'Planet Express', 'Windsor', 1, 50, 640, 5.21, 0.3621, 0.08),
(2, 'Planet Express', 'Hamilton', 3, 50, 640, 5.21, 0.3621, 0.08),
(3, 'Planet Express', 'Oshawa', 5, 50, 640, 5.21, 0.3621, 0.08),
(4, 'Planet Express', 'Belleville', 6, 50, 640, 5.21, 0.3621, 0.08),
(5, 'Planet Express', 'Ottawa', 8, 50, 640, 5.21, 0.3621, 0.08),
(6, 'Schooners', 'London', 2, 18, 98, 5.05, 0.3434, 0.07),
(7, 'Schooners', 'Toronto', 4, 18, 98, 5.05, 0.3434, 0.07),
(8, 'Schooners', 'Kingston', 7, 18, 98, 5.05, 0.3434, 0.07),
(9, 'Tillman Transport', 'Windsor', 1, 24, 35, 5.11, 0.3012, 0.09),
(10, 'Tillman Transport', 'London', 2, 18, 45, 5.11, 0.3012, 0.09),
(11, 'Tillman Transport', 'Hamilton', 3, 18, 45, 5.11, 0.3012, 0.09),
(12, 'We Haul', 'Ottawa', 8, 11, 0, 5.2, 0, 0.065),
(13, 'We Haul', 'Toronto', 4, 11, 0, 5.2, 0, 0.065);    
        
CREATE TABLE Orders (
	OrderID SMALLINT NOT NULL AUTO_INCREMENT,
	OrderDate date,
	CustomerID SMALLINT NOT NULL,
    CustomerName varchar(50) NOT NULL,
    Qty SMALLINT NOT NULL,
    Origin varchar(50),
    Destination varchar(50),
    LTLFTL varchar(25),
    OrderStatus bool,
    PRIMARY KEY(OrderID),
    FOREIGN KEY (CustomerID) REFERENCES Customer (CustomerID)
    );
INSERT INTO Orders VALUES 
(101, 20221104, 1001, 'Atlantis Railway', 18, 'Windsor', 'Ottawa', 'LTL', false),
(102, 20221201, 1002, 'MacDongles', 26, 'Windsor', 'Toronto', 'LTL', false),
(103, 20221201, 1003, 'Farm Supply Co', 28, 'Kingston', 'London', 'LTL', false);

CREATE TABLE Trip (
	OrderID smallint,
    Origin varchar(50),
	Destination varchar(50),
    qty int,
    LTLFTL varchar(35),
	TripID varchar(35),
    cID smallint,
    cName varchar(50),
    TripDest varchar(35),
    unit DECIMAL(5,2),
    rate DECIMAL(5,2),
    status bool,
    FOREIGN KEY (OrderID) REFERENCES orders (OrderID),
    FOREIGN KEY (cID) REFERENCES Carriers (cID)
    );
    
INSERT INTO Trip VALUES 
(null, null, null, null, null, null, null, null, null, null, null, false)
;    
        
    CREATE TABLE Rate (
	CarrierID SMALLINT NOT NULL,
    OrderID SMALLINT NOT NULL,
    RateDescription VARCHAR(50) NOT NULL,
	ChargeType VARCHAR(50) NOT NULL,
	Currency VARCHAR(50) NOT NULL,
	CalculationBase SMALLINT NOT NULL,
	Unit VARCHAR(50) NOT NULL
    );

    
CREATE TABLE Invoice (
	InvoiceID SMALLINT NOT NULL AUTO_INCREMENT,
	InvoiceDate date NOT NULL,
	OrderID SMALLINT NOT NULL,
	CustomerID SMALLINT NOT NULL,
	TripID VARCHAR(50),
	TripName VARCHAR(50) NOT NULL,
    Qty SMALLINT,
    Unit varchar(25),
    Currency VARCHAR(50) NOT NULL,
    Amount SMALLINT NOT NULL,
    PRIMARY KEY(InvoiceID)
    );
    
INSERT INTO Invoice VALUES 
(1, '20221101', 101, 1001, 'T1', '1 Windsor', 18, 'trip', 'CAD', '6.1812'); 
