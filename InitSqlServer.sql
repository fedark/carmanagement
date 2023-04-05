create table Companies (
	Id nvarchar(50) not null,
	Name nvarchar(50) not null,
	constraint PK_Companies_Id primary key (Id)
);
go

create table Models (
	Id nvarchar(50) not null,
	Name nvarchar(50) not null,
	Year int not null,
	CompanyId nvarchar(50) not null,
	constraint PK_Models_Id primary key (Id),
	constraint FK_Models_CompanyId foreign key (CompanyId) references Companies (Id) on delete cascade
);
go

create table Cars (
	Id nvarchar(50) not null,
	Displacement int null,
	Picture varbinary(max) not null,
	ModelId nvarchar(50) not null,
	constraint PK_Cars_Id primary key (Id),
	constraint FK_Cars_ModelId foreign key (ModelId) references Models (Id) on delete cascade
);
go

create table Users (
	Id nvarchar(50) not null,
	Name nvarchar(50) not null,
	PasswordHash nvarchar(max) not null,
	HasDriverLicense bit null,
	constraint PK_Users_Id primary key (Id),
	constraint UQ_Users_Name unique (Name)
);
go

create table Roles (
	Id nvarchar(50) not null,
	Name nvarchar(50) not null,
	constraint PK_Roles_Id primary key (Id),
	constraint UQ_Roles_Name unique (Name)
);
go

create table UserRoles (
	UserId nvarchar(50) not null,
	RoleId nvarchar(50) not null,
	constraint PK_UserRoles primary key (UserId, RoleId),
	constraint FK_UserRoles_UserId foreign key (UserId) references Users (Id) on delete cascade,
	constraint FK_UserRoles_RoleId foreign key (RoleId) references Roles (Id) on delete cascade
);
go