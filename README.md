# ReuSQL
Tiny amount of code for prototyping code reuse in SQL, using Dapper and Visual Studio T4 templates. 
SQL Server doesn't really have any facilities for code reuse that perform well, so this provides reusability via templating.

All the templating happens at development-time, not runtime.

## Example
Given a query like this, in the template file `CuteAnimalsByLocation.tt`:

```sql
<#@ output extension=".sql" #>

select * from animals a
where a.IsCute = 1 and a.IsFuzzy = 1 and a.Location = @location
```

And this file, `DeadlyMachinesByLocation.tt`:

```sql
<#@ output extension=".sql" #>

select * from DeadlyMachines m
where m.IsLethal = 1 and m.HasExplosives = 1 and m.Location = @location
```

we can find animals in need of help (and gain SQL code reuse!) by writing the following, `AnimalsInPeril.tt`:

```sql
<#@ output extension=".sql" #>

WITH 
CuteAnimalsInLocation as 
( 
	<#@ include file="CuteAnimalsByLocation.tt" #>
),
DeadlyMachinesInLocation as
(
	<#@ include file="DeadlyMachinesByLocation.tt" #>
)
select a.* from CuteAnimalsInLocation a
inner join DeadlyMachinesInLocation m on a.Location = m.Location
```

We can run these queries like this:
```csharp
using (var connection = new SqlConnection(ConnectionString))
{
    connection.Open();

    connection.QueryFromFile<Project>("AnimalsInPeril", new { Location = "NorthAmerica" });
    //or
    connection.QueryFromFile<Project>("CuteAnimalsByLocation", new { Location = "NorthAmerica" });
    //or
    connection.QueryFromFile<Project>("DeadlyMachinesByLocation", new { Location = "NorthAmerica" });
}
```

## Caveats

- Make sure that the generated SQL files are included during deploy (e.g. "Copy If Newer" in Visual Studio).
- During development, if you make changes to an included file, you'll need to resave the dependent file to pick up the changes.

## TODO
Right now, QueryFromFile will read the filesystem every time it's called.
