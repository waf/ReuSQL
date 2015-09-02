# ReuSQL
Tiny amount of code for prototyping code reuse in SQL, using [Dapper](https://github.com/StackExchange/dapper-dot-net) and Visual Studio [T4 text templates](https://msdn.microsoft.com/en-us/library/dd820620(v=vs.120).aspx). 
SQL Server doesn't really have any facilities for code reuse that perform well, so this provides reusability via templating.

## Implementation Notes

- All the templating happens at development-time, not runtime. 
- SQL files are deployed with the application and read by DapperFileExtensions.QueryFromFile method.
- Dapper handles all SQL variable substitution and serialization.

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

We can run these queries like this, assuming the POCOs `Animal` and/or `DeadlyMachine` exist:
```csharp
using (var connection = new SqlConnection(ConnectionString))
{
    connection.Open();

    IEnumerable<Animal> a = connection.QueryFromFile<Animal>("AnimalsInPeril", new { Location = "NorthAmerica" });
    //or
    IEnumerable<Animal> cuteAnimals = connection.QueryFromFile<Animal>("CuteAnimalsByLocation", new { Location = "NorthAmerica" });
    //or
    IEnumerable<DeadlyMachine> deadlyMachines = connection.QueryFromFile<Animal>("DeadlyMachinesByLocation", new { Location = "NorthAmerica" });
}
```

## Caveats

- Make sure that the generated SQL files are included during deploy (e.g. "Copy If Newer" in Visual Studio).
- During development, if you make changes to an included file, you'll need to resave the dependent file to pick up the changes.

## TODO
Right now, QueryFromFile will read the filesystem every time it's called.
