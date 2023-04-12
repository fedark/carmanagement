if not exists (select * from sys.databases where name = 'CarManage')
create database CarManage;
go

use CarManage
go

if not exists (select * from sysobjects where xtype = 'U' and name = 'Companies')
create table Companies (
	Id nvarchar(50) not null,
	Name nvarchar(50) not null,
	constraint PK_Companies_Id primary key (Id),
	constraint UQ_Companies_Name unique (Name)
);
go

if not exists (select * from sysobjects where xtype = 'U' and name = 'Models')
create table Models (
	Id nvarchar(50) not null,
	Name nvarchar(50) not null,
	Year int not null,
	CompanyId nvarchar(50) not null,
	constraint PK_Models_Id primary key (Id),
	constraint FK_Models_CompanyId foreign key (CompanyId) references Companies (Id) on delete cascade
);
go

if not exists (select * from sysobjects where xtype = 'U' and name = 'Cars')
create table Cars (
	Id nvarchar(50) not null,
	Displacement float null,
	Picture varbinary(max) not null,
	PictureType nvarchar(20) not null,
	ModelId nvarchar(50) not null,
	constraint PK_Cars_Id primary key (Id),
	constraint FK_Cars_ModelId foreign key (ModelId) references Models (Id) on delete cascade
);
go

if not exists (select * from sysobjects where xtype = 'U' and name = 'Users')
create table Users (
	Id nvarchar(50) not null,
	Name nvarchar(50) not null,
	PasswordHash nvarchar(max) not null,
	HasDriverLicense bit null,
	constraint PK_Users_Id primary key (Id),
	constraint UQ_Users_Name unique (Name)
);
go

if not exists (select * from sysobjects where xtype = 'U' and name = 'Roles')
create table Roles (
	Id nvarchar(50) not null,
	Name nvarchar(50) not null,
	constraint PK_Roles_Id primary key (Id),
	constraint UQ_Roles_Name unique (Name)
);
go

if not exists (select * from sysobjects where xtype = 'U' and name = 'UserRoles')
create table UserRoles (
	UserId nvarchar(50) not null,
	RoleId nvarchar(50) not null,
	constraint PK_UserRoles primary key (UserId, RoleId),
	constraint FK_UserRoles_UserId foreign key (UserId) references Users (Id) on delete cascade,
	constraint FK_UserRoles_RoleId foreign key (RoleId) references Roles (Id) on delete cascade
);
go
