DROP SCHEMA IF EXISTS tms;
CREATE SCHEMA tms;
USE tms;


CREATE TABLE Organization (
	OrganizationID SMALLINT NOT NULL AUTO_INCREMENT,
	OrganizationName VARCHAR(50) NOT NULL,
	address VARCHAR(50) NOT NULL,
    PRIMARY KEY(OrganizationID)
    );
    
INSERT INTO Organization VALUES 
(1001, 'Head Company', '123 Road');
    
CREATE TABLE Customer (
	CustomerID SMALLINT NOT NULL AUTO_INCREMENT,
	CustomerName varchar(50) not null,
	SoldToParty varchar(100),
    SoldToAddress varchar(100),
    ShipToParty varchar(100),
    ShipToAddress varchar(100),
    PRIMARY KEY(CustomerID)
    );

INSERT INTO Customer VALUES 
(2001, 'Atlantis Railway', 'Atlantis Railway', '1300 Highway', 'Atlantis Railway', '1300 Highway'),
(2002, 'MacDongles', 'MacDongles', '2500 Bleans Road', 'MacDongles', '2500 Bleans Road'),
(2003, 'Farm Supply Co', 'Farm Supply Co', '1 Glen Street', 'Farm Supply Co', '1 Glen Street');
    
CREATE TABLE users (
	userID SMALLINT NOT NULL AUTO_INCREMENT,
	name VARCHAR(50) NOT NULL,
	password VARCHAR(50) NOT NULL,
    OrganizationID SMALLINT NOT NULL,
	userType VARCHAR(50),
    PRIMARY KEY(userID),
    FOREIGN KEY (OrganizationID) REFERENCES Organization (OrganizationID)
    );
	
INSERT INTO users VALUES (1, 'admin1', 'Conestoga1', 1001, 'admin'),
(2, 'buyer1', 'Conestoga1', 1001, 'buyer'),
(3, 'planner1', 'Conestoga1', 1001, 'planner');



CREATE TABLE Carrier (
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
    
INSERT INTO Carrier VALUES (1, 'Planet Express', 'Windsor', 1, 50, 640, 5.21, 0.3621, 0.08),
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

CREATE TABLE Rate (
	RateID SMALLINT NOT NULL,
	CarrierID SMALLINT NOT NULL,
    RateDescription VARCHAR(50) NOT NULL,
	ChargeType VARCHAR(50) NOT NULL,
	Currency VARCHAR(50) NOT NULL,
	CalculationBase SMALLINT NOT NULL,
	Unit VARCHAR(50) NOT NULL,
    PRIMARY KEY(RateID)
    );

CREATE TABLE Orders (
	OrderID SMALLINT NOT NULL AUTO_INCREMENT,
	OrderDate date,
	CustomerID smallint not null,
    CustomerName varchar(50) not null,
    CreatedBy smallint not null,
    Qty smallint not null,
    Origin varchar(50),
    Destination varchar(50),
    LTLFTL varchar(25),
    OrderStatus bool,
    PRIMARY KEY (OrderID),
    FOREIGN KEY (CustomerID) REFERENCES Customer (CustomerID),
	FOREIGN KEY (CreatedBy) REFERENCES users (userID)
    );
    
INSERT INTO Orders VALUES 
(101, 20221104, 2001, 'Atlantis Railway', 2, 18, 'Windsor', 'Ottawa', 'LTL', false),
(102, 20221201, 2002, 'MacDongles', 2, 26, 'Windsor', 'Toronto', 'LTL', false),
(103, 20221201, 2003, 'Farm Supply Co', 2, 28, 'Kingston', 'London', 'LTL', false);

CREATE TABLE Hub (
	HubID SMALLINT NOT NULL,
    HubName VARCHAR(50) NOT NULL,
	HubAddress VARCHAR(50) NOT NULL,
	Capacity VARCHAR(50) NOT NULL,
	TransTimeToNextHub SMALLINT NOT NULL,
    DistencToNextHub SMALLINT NOT NULL,
	Unit VARCHAR(50) NOT NULL,
    PRIMARY KEY (HubID)
    );


CREATE TABLE Trip (
	TripID SMALLINT NOT NULL,
    OrderID SMALLINT NOT NULL,
	HubFrom SMALLINT NOT NULL,
    HubTo SMALLINT NOT NULL,
    TripDestination VARCHAR(100),
	DistenceKM SMALLINT,
    TransitTime TIME,
    RateID SMALLINT NOT NULL,
    PRIMARY KEY (TripID),
    FOREIGN KEY (OrderID) REFERENCES Orders (OrderID),
    FOREIGN KEY (HubFrom) REFERENCES Hub (HubID),
    FOREIGN KEY (RateID) REFERENCES Rate (RateID)
    );
    
CREATE TABLE Shipment (
	ShipmentID SMALLINT NOT NULL,
    OrderID SMALLINT NOT NULL,
	CustomerID SMALLINT NOT NULL,
	CarrierID SMALLINT NOT NULL,
    RateID SMALLINT NOT NULL,
    ShipmentType VARCHAR(10),
	LoadTime VARCHAR(5),
    UnloadTime VARCHAR(5),
    PRIMARY KEY(ShipmentID),
	FOREIGN KEY (OrderID) REFERENCES Orders (OrderID),
    FOREIGN KEY (CarrierID) REFERENCES Carrier (cID),
	FOREIGN KEY (CustomerID) REFERENCES Customer (CustomerID)
    );

CREATE TABLE Invoice (
	InvoiceID SMALLINT NOT NULL AUTO_INCREMENT,
	InvoiceDate date NOT NULL,
	OrderID SMALLINT NOT NULL,
    ShipmentID SMALLINT NOT NULL,
	CustomerID SMALLINT NOT NULL,
	TripID SMALLINT NOT NULL,
	TripName VARCHAR(50) NOT NULL,
    Qty SMALLINT,
    Unit varchar(25),
    Currency VARCHAR(50) NOT NULL,
    Amount SMALLINT NOT NULL,
    PRIMARY KEY(InvoiceID),
    FOREIGN KEY (CustomerID) REFERENCES Customer (CustomerID),
	FOREIGN KEY (OrderID) REFERENCES Orders (OrderID),
	FOREIGN KEY (TripID) REFERENCES Trip (TripID),
	FOREIGN KEY (ShipmentID) REFERENCES Shipment (ShipmentID)
    );
    


