using ReuSQL

create table Users (UserId int, Username varchar(500), IsActive bit)
create table Roles (RoleId int, Role varchar(500))
create table UserRoles (UserId int, RoleId int)
create table Projects (ProjectId int, OwnerUserId int, Name varchar(500), IsSuperSecret bit, IsActive bit)

insert into Users(UserId, Username, IsActive)
values (1, 'Will', 1),
       (2, 'Bob', 1),
	   (3, 'Spammy', 0)

insert into Roles(RoleId, Role)
values (0, 'Admin'),
       (1, 'User')

insert into UserRoles(UserId, RoleId)
values (1, 0),
       (2, 1),
	   (3, 1)

insert into Projects(ProjectId, OwnerUserId, Name, IsSuperSecret, IsActive)
values (1, 1, 'ReuSQL', 0, 1),
       (2, 1, 'TakeOverTheWorld', 1, 1),
       (3, 2, 'HobbyProject', 0, 1),
	   (4, 3, 'SpamBot', 1, 0)