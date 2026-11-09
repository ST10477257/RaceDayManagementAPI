use raceday_test;
go


/*
RaceDay Event Management System
Database Creation Script

Purpose:
Creates the RaceDay relational database structure,
including entities, primary keys, foreign keys,
unique constraints, check constraints and defaults.
*/


/* ============================================================
   SECTION 1: TABLE CREATION
   ============================================================ */

-- 1. User
CREATE TABLE [User]
(
user_id INT IDENTITY(1,1) NOT NULL,
first_name VARCHAR(50) NOT NULL,
last_name VARCHAR(50) NOT NULL,
email VARCHAR(100) NOT NULL,
password_hash VARCHAR(255) NOT NULL,
role VARCHAR(20) NOT NULL,
phone_number VARCHAR(20) NULL,
created_at DATETIME NOT NULL
CONSTRAINT DF_User_created_at DEFAULT GETDATE(),

CONSTRAINT PK_User
PRIMARY KEY (user_id),

CONSTRAINT UQ_User_Email
UNIQUE (email),

CONSTRAINT CK_User_Role
CHECK (role IN ('Participant', 'Organiser'))
);
GO


-- 2. Category
CREATE TABLE Category
(
category_id INT IDENTITY(1,1) NOT NULL,
category_name VARCHAR(50) NOT NULL,
description VARCHAR(255) NULL,

CONSTRAINT PK_Category
PRIMARY KEY (category_id),

CONSTRAINT UQ_Category_Name
UNIQUE (category_name)
);
GO


-- 3. Event
CREATE TABLE Event
(
event_id INT IDENTITY(1,1) NOT NULL,
organiser_id INT NOT NULL,
event_name VARCHAR(100) NOT NULL,
description VARCHAR(500) NOT NULL,
event_date DATE NOT NULL,
start_time TIME NOT NULL,
location VARCHAR(150) NOT NULL,
status VARCHAR(20) NOT NULL,

CONSTRAINT PK_Event
PRIMARY KEY (event_id),

CONSTRAINT FK_Event_Organiser
FOREIGN KEY (organiser_id)
REFERENCES [User](user_id),

CONSTRAINT CK_Event_Status
CHECK (status IN ('Upcoming', 'Cancelled', 'Completed', 'Active'))
);
GO


-- 4. EventCategory
CREATE TABLE EventCategory
(
event_category_id INT IDENTITY(1,1) NOT NULL,
event_id INT NOT NULL,
category_id INT NOT NULL,
entry_fee DECIMAL(10,2) NOT NULL,
participant_limit INT NOT NULL,

CONSTRAINT PK_EventCategory
PRIMARY KEY (event_category_id),

CONSTRAINT FK_EventCategory_Event
FOREIGN KEY (event_id)
REFERENCES Event(event_id),

CONSTRAINT FK_EventCategory_Category
FOREIGN KEY (category_id)
REFERENCES Category(category_id),

CONSTRAINT UQ_EventCategory
UNIQUE (event_id, category_id),

CONSTRAINT CK_EventCategory_EntryFee
CHECK (entry_fee >= 0),

CONSTRAINT CK_EventCategory_Limit
CHECK (participant_limit > 0)
);
GO


-- 5. Enrolment
CREATE TABLE Enrolment
(
enrolment_id INT IDENTITY(1,1) NOT NULL,
participant_id INT NOT NULL,
event_category_id INT NOT NULL,
enrolment_date DATETIME NOT NULL
CONSTRAINT DF_Enrolment_enrolment_date DEFAULT GETDATE(),
enrolment_status VARCHAR(20) NOT NULL,

CONSTRAINT PK_Enrolment
PRIMARY KEY (enrolment_id),

CONSTRAINT FK_Enrolment_Participant
FOREIGN KEY (participant_id)
REFERENCES [User](user_id),

CONSTRAINT FK_Enrolment_EventCategory
FOREIGN KEY (event_category_id)
REFERENCES EventCategory(event_category_id),

CONSTRAINT UQ_Enrolment
UNIQUE (event_category_id, participant_id),

CONSTRAINT CK_Enrolment_Status
CHECK (enrolment_status IN ('Pending', 'Confirmed', 'Registered'))
);
GO


-- 6. Result
CREATE TABLE Result
(
result_id INT IDENTITY(1,1) NOT NULL,
enrolment_id INT NOT NULL,
finish_time TIME NULL,
finishing_position INT NULL,
result_status VARCHAR(20) NOT NULL,
recorded_at DATETIME NOT NULL
CONSTRAINT DF_Result_recorded_at DEFAULT GETDATE(),

CONSTRAINT PK_Result
PRIMARY KEY (result_id),

CONSTRAINT FK_Result_Enrolment
FOREIGN KEY (enrolment_id)
REFERENCES Enrolment(enrolment_id),

CONSTRAINT UQ_Result
UNIQUE (enrolment_id),

CONSTRAINT CK_Result_Position
CHECK (finishing_position IS NULL OR finishing_position > 0),
CONSTRAINT CK_Result_Status
CHECK (result_status IN ('Did Not Finish', 'Finished', 'Pending'))
);
GO



-- 7. Route
CREATE TABLE Route
(
route_id INT IDENTITY(1,1) NOT NULL,
event_id INT NOT NULL,
route_name VARCHAR(100) NOT NULL,
distance_km DECIMAL(6,2) NOT NULL,
route_description VARCHAR(500) NULL,
route_map_url VARCHAR(500) NULL,

CONSTRAINT PK_Route
PRIMARY KEY (route_id),

CONSTRAINT FK_Route_Event
FOREIGN KEY (event_id)
REFERENCES Event(event_id),

CONSTRAINT UQ_Route_Event
UNIQUE (event_id),

CONSTRAINT CK_Route_Distance
CHECK (distance_km > 0)
);
GO

insert into [user](first_name,last_name,email,password_hash,role,phone_number) values
('john','mokoena','john.mokoena@email.com','hashedpassword1','Participant','0712345678'),
('sarah','dlamini','sarah.dlamini@email.com','hashedpassword2','Participant','0723456789'),
('thabo','molefe','thabo.molefe@email.com','hashedpassword3','Participant','0734567890'),
('lerato','nkosi','lerato.nkosi@email.com','hashedpassword4','Participant','0745678901'),
('peter','naidoo','peter.naidoo@email.com','hashedpassword5','Organiser','0756789012');

insert into category(category_name,description) values
('5 km fun run','a short recreational race for beginners and casual runners'),
('10 km road race','a competitive road race for intermediate runners'),
('21 km half marathon','a long-distance race for experienced runners'),
('cycling','a cycling event for recreational and competitive cyclists');

insert into category(category_name,description) values
('5 km fun run','a short recreational race for beginners and casual runners'),
('10 km road race','a competitive road race for intermediate runners'),
('21 km half marathon','a long-distance race for experienced runners'),
('cycling','a cycling event for recreational and competitive cyclists');

insert into event(organiser_id,event_name,description,event_date,start_time,location,status) values
(5,'polokwane fun run','a community fun run open to participants of all experience levels','2026-10-10','07:00','polokwane sports complex','Upcoming'),
(5,'limpopo road race','a competitive 10 km road race for intermediate runners','2026-11-14','06:30','polokwane city centre','Upcoming'),
(5,'summer half marathon','a 21 km half marathon for experienced runners','2026-12-05','06:00','polokwane stadium','Upcoming');

insert into eventcategory(event_id,category_id,entry_fee,participant_limit) values
(1,1,50.00,200),
(1,2,80.00,150),
(2,2,100.00,150),
(3,3,150.00,100);

insert into enrolment(participant_id,event_category_id,enrolment_date,enrolment_status) values
(1,1,getdate(),'Registered'),
(2,1,getdate(),'Registered'),
(3,2,getdate(),'Confirmed'),
(4,3,getdate(),'Registered');

insert into result(enrolment_id,finish_time,finishing_position,result_status) values
(1,'08:15:30',12,'Finished'),
(2,'08:22:10',18,'Finished');

insert into route(event_id,route_name,distance_km,route_description,route_map_url) values
(1,'fun run route',5.00,'a 5 km route through the polokwane community area','https://raceday.co.za/routes/fun-run'),
(2,'road race route',10.00,'a 10 km road route through polokwane city centre','https://raceday.co.za/routes/road-race'),
(3,'half marathon route',21.10,'a 21.1 km route starting and finishing at polokwane stadium','https://raceday.co.za/routes/half-marathon');