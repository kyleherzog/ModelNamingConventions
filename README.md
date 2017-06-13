# Model Naming Conventions

[![Build status](https://ci.appveyor.com/api/projects/status/1qg4m7wfh5ihndyu?svg=true)](https://ci.appveyor.com/project/kyleherzog/modelnamingconventions)

This library is available from [NuGet.org](https://www.nuget.org/packages/ModelNamingConventions/)
or download from the [CI build feed](https://ci.appveyor.com/nuget/ModelNamingConventions).

--------------------------

A class library that allows the developer to define Entity Framework naming conventions that transform entity names into alternate database object names. 

See the [changelog](CHANGELOG.md) for changes and roadmap.

## Initialization
To apply the model naming convention to an entity framework context, the convention must be added in the OnModelCreating method.
```C#
protected override void OnModelCreating(DbModelBuilder modelBuilder)
{
    modelBuilder.Conventions.Add<ModelNamingConvention>();
}
```


## Features

- Case Transformation
- Table Prefixing
- Selective Column Prefixing

### Case Transformation
The casing of entities can be modified to any of the code casing styles made available in the [Code Casing Library](https://github.com/kyleherzog/CodeCasing). For instance, by specifying a casing style of *ScreamingSnakeCase*, an entity object named "LongDescription" becomes a database object called "LONG_DESCTIPTION".

#### Casing Assembly Attributes
The following assembly attributes control how entity names are cased at the assembly level. These need to be defined in the assembly of the entity context.
```C#
[assembly: TableCasing(CasingStyle.ScreamingSnake)]
[assembly: ColumnCasing(CasingStyle.Snake)]
```  
  

#### Casing Config Settings
Casing transformation styles can also be configured in the app/web.config file.  The following shows an example of the configuration parameters.

```xml
  <modelConventions     
    tableCasing="Snake"
    columnCasing="Snake"
    tableCasingDefault="ScreamingSnake" 
    columnCasingDefault="ScreamingSnake">
  </modelConventions>
```

#### Applying Casing Settings
The casing settings are applied using the first match found when searching in the following order.

- tableCasing/columnCasing setting in the app/web.config file
- TableCasing/ColumnCasing assembly attribute
- tableCasingDefault/columnCasingDefault setting in the app/web.config file

### Table Prefixing
Every table can have a standard prefix added to it by specifying a table prefix setting.  Table prefixing is only applied at the context assembly level using the `TablePrefix` attribute.
```C#
[assembly: TablePrefix("Tbl")]
```

NOTE: Table prefixing is applyed prior to casing transformations.  Therefore, the prefix should be entered as Pascal case.

### Selective Column Prefixing
At times, it may be desirable to prefix certain columns with the name of the table.  This can be done by specifying the names of the columns that should be automatically prefixed.

#### Column Prefixing Assembly Attributes
To specify which columns to prefix for a specific context, add the `ColumnAutoPrefixing` assembly attribute similar to the following.
```c#
[assembly: ColumnAutoPrefixing(new string[]{"Id", "Key"})]
```

#### Column Prefixing Config Settings
To specify which columns to prefix at an application-wide level, apply the `autoPrefixColumns` settings to the app/web.config file.

```xml
  <modelConventions>
    <autoPrefixColumns>
      <add name="Id"></add>      
    </autoPrefixColumns>
  </modelConventions>
```

By default, any columns added via the config file will be added to any columns specified in the assembly attributes.  However, by adding `isReplacingAutoPrefixColumns="true"` to the modelConventions element, only the column names in the config file will be utilized.


## License
[Apache 2.0](LICENSE)