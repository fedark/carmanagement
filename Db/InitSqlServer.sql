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

insert into Roles Values ('beaaa4aa-da9c-4885-a2b8-ad1d84344910', 'owner');
insert into Roles Values ('32c8735f-65c4-46c0-83ee-7d0f32d1fab3', 'admin');
insert into Users Values ('03422484-4f6c-4ef7-b4f4-a6ee8c3a71a9', 'owner', 'AQAAAAIAAYagAAAAEMSLsAhjpCkQQFLgYzEayGuftskIWEFosvCtFjNkeTqM1ViBeMI8t36f+2qRPR6Z0A==', 1);
go

insert into UserRoles Values ('03422484-4f6c-4ef7-b4f4-a6ee8c3a71a9', 'beaaa4aa-da9c-4885-a2b8-ad1d84344910');
