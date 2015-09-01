
WITH 
UsersWithRole as 
( 
	
-- Wheeee a comment!!
select u.* from Users u
inner join UserRoles ur on u.UserId = ur.UserId
inner join Roles r on ur.RoleId = r.RoleId
where r.Role = @role and u.IsActive = 1

),
PublicProjects as
(
	
select * from Projects p
where p.IsSuperSecret = @isSuperSecret and p.IsActive = 1

)
select p.* from UsersWithRole u
inner join PublicProjects p on p.OwnerUserId = u.UserId
