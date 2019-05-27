# Schematics

## Features

### Core featues

* Define CRUD functionality for Asp.Net Core app with consistent generic API
* Entities Metadata
* Entities Versioning
* Storage agnostic (plugins for specific storage technologies)
* Define entitities at startup with generic or custom read \ write logic
* Feature-based "plugin" system for storages (system defines capabilities which may or may not be implemented by specific storage plugin)

### Advanced features

* Metadata localization
* Create entities at runtime
* GraphQL

## Samples

```csharp
public class Startup
{
	public void Configure(IServiceCollection services)
	{
		services
			.AddSchematics(x => { /* General configuration */ })
			.WithEntity(x => SchematicsEntity
				.Create("MyEntity", "1")
				.WithProperty<int>("Id", PropertyRole.PrimaryKey)
				.WithProperty<string>("Name")
				.WithFeatures(Schematics.SqlServer.Features.All());
			)
			.WithGraphQl(x => { /* GraphQl configuration */ })

	}

	[Entity("ClassEntity", "1")]
	private class ClassEntity
	{
		[Property("Id")]
		[PrimaryKey]
		public int Id { get; set; }

		public string Name { get; set; }
	}
}
```