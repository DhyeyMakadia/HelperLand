Create Database Helperland2;
Go

USE Helperland2;
GO

Create table [Admin] (admin_id int NOT NULL Primary Key Identity(1,1), first_name varchar(50) not null, last_name varchar(50) not null, email varchar(50) not null constraint UQ_Admin_email Unique, [password] varchar(50) not null, insert_date_time datetime not null, update_date_time timestamp not null);

Go

Create table Customer (cust_id int NOT NULL Primary Key Identity(1,1), first_name varchar(50) not null, last_name varchar(50) not null, email varchar(50) not null constraint UQ_Customer_email Unique, [password] varchar(50) not null, mobile_number int, dob date, perffered_language varchar(50), insert_date_time datetime not null, update_date_time timestamp not null);

Go

Create table CustomerAddresses (cust_address_id int NOT NULL Primary Key Identity(1,1), cust_id int not null constraint FK_CustomerAddresses_Customer_cust_id FOREIGN KEY REFERENCES Customer(cust_id), street_name varchar(50) not null, house_name varchar(50) not null, postal_code int not null, city varchar(50) not null, insert_date_time datetime not null, update_date_time timestamp not null);

Go

Create table ServiceProvider (sp_id int NOT NULL Primary Key Identity(1,1), first_name varchar(50) not null, last_name varchar(50) not null, email varchar(50) not null constraint UQ_ServiceProvider_email Unique, [password] varchar(50) not null, [profile] image not null, mobile_number int, dob date, gender varchar(50) not null, nationality varchar(50), street_name varchar(50) not null, house_name varchar(50) not null, postal_code int not null, city varchar(50) not null, is_active bit not null Default 0, insert_date_time datetime not null, update_date_time timestamp not null);

Go

Create table ServiceRequests (service_id int not null Primary key Identity(1,1), cust_id int not null constraint FK_ServiceRequests_Customer_cust_id Foreign Key References Customer(cust_id), sp_id int not null constraint FK_ServiceRequests_ServiceProvider_sp_id Foreign Key References ServiceProvider(sp_id), service_date date not null, service_time time not null, duration_of_service int not null, comments varchar(100), have_pets bit default 0, [status] varchar(50) not null, payment_status varchar(50) not null, [priority_level] int, cancelled_reason varchar(50), edit_sr_by_admin varchar(50), cc_emp_notes_by_admin varchar(50), insert_date_time datetime not null, update_date_time timestamp not null);

Go

Create table ExtraServices (extra_service_id int not null Primary Key Identity(1,1), service_id int not null constraint FK_ExtraServices_ServiceRequests_service_id foreign key references ServiceRequests(service_id), cabinets bit default 0, fridge bit default 0, oven bit default 0, laundry bit default 0, [windows] bit default 0);

Go

Create table BlockCustomer (block_customer_id int not null Primary key Identity(1,1), cust_id int not null constraint FK_BlockCustomer_Customer_cust_id foreign key references Customer(cust_id), sp_id int not null constraint FK_BlockCustomer_ServiceProvider_sp_id foreign key references ServiceProvider(sp_id), is_blocked bit default 0, update_date_time timestamp not null);

Go

Create table Ratings (rate_id int not null Primary Key Identity(1,1), service_id int not null constraint FK_Ratings_ServiceRequests_service_id Foreign Key References ServiceRequests(service_id), cust_id int not null constraint FK_Ratings_Customer_cust_id Foreign Key References Customer(cust_id), sp_id int not null constraint FK_Ratings_ServiceProvider_sp_id Foreign Key References ServiceProvider(sp_id), on_time_arrival float not null, friendly float not null, quality_of_service float not null, average float not null, feedback varchar(50));

Go

Create table BlockSP (block_sp_id int not null Primary key Identity(1,1), cust_id int not null constraint FK_BlockSP_Customer_cust_id foreign key references Customer(cust_id), sp_id int not null constraint FK_BlockSP_ServiceProvider_sp_id foreign key references ServiceProvider(sp_id), is_blocked bit default 0, update_date_time timestamp not null);

GO

Create table Payment (payment_id int not null Primary Key Identity(1,1), service_id int not null constraint FK_Payment_ServiceRequests_service_id Foreign Key References ServiceRequests(service_id), promocode varchar(50), net_amount int not null, gross_amount int not null);

Go

Create table FavouritePros (fav_pros_id int not null Primary Key Identity(1,1), cust_id int not null constraint FK_FavouritePros_Customer_cust_id foreign key references Customer(cust_id), sp_id int not null constraint FK_FavouritePros_ServiceProvider_sp_id foreign key references ServiceProvider(sp_id), is_favourite bit default 0);

Go

Create table ContactUs (contact_us_id int not null Primary key Identity(1,1), first_name varchar(50) not null, last_name varchar(50) not null, email varchar(50) not null, phone_number int not null, [subject] varchar(50) not null, [message] text, insert_date_time datetime not null);

Go

Create table GetNewsLetter (get_newsletter_id int not null Primary key Identity(1,1), email varchar(50) not null, insert_date_time datetime not null);