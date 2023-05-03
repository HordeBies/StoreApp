-- Join Users with Roles and Companies
select u.Name,u.UserName,r.Name, c.Name
from AspNetUserRoles ur
join AspNetUsers u on u.Id = ur.UserId
join AspNetRoles r on r.Id = ur.RoleId
left join Companies c on c.Id = u.CompanyId