# Universal Report Core

A fast and versatile framework for presenting tabular data reports in ASP.NET Core.

## Features

- **Paging**: Efficiently handle large datasets with built-in pagination.
- **Column Sorting**: Sort data by columns dynamically.
- **Column Aggregation**: Supports sum, average, count, min, and max operations.
- **Cohort Aggregation**: Aggregate data within defined subsets.

## Installation

To install via NuGet:

```sh
dotnet add package UniversalReportCore
```

Or add it to your `csproj` file:

```xml
<PackageReference Include="UniversalReportCore" Version="1.0.0" />
```

## Usage

### 1. Register Services in `Startup.cs` (ASP.NET Core 6+)

```csharp
builder.Services.AddScoped<IQueryFactory<CityPopulation>, CityPopulationQueryFactory>();
builder.Services.AddScoped<IPageMetaProvider, CityPopulationDemoPageMetaProvider>();
builder.Services.AddScoped<IReportColumnProvider, CityPopulationDemoReportColumnProvider>();
builder.Services.AddScoped<IPagedQueryProvider<CityPopulation>, CityPopulationDemoQueryProvider>();
builder.Services.AddTransient(typeof(IPageHelper<CityPopulation, CityPopulationViewModel>), typeof(CityPopulationDemoPageHelper));
```

### 2. Define Your Report Model

Create an entity that represents a row your dataset:

```csharp
# Embed Code Example
[View CityPopulation.cs](https://github.com/tonybierman/Universal-Report-Core/blob/master/UniversalReportDemo/Data/CityPopulation.cs)
```

### 3. Configure a Report Query

```csharp
# Embed Code Example
[View CityPopulationDemoReportColumnProvider.cs](UniversalReportDemo/Reports/CityPop/CityPopulationDemoReportColumnProvider.cs)
```

## Extending & Customization

- **Custom Aggregation**: Implement your own aggregation logic using `ComputeAggregates()`.
- **Custom Page Helpers**: Extend `BasePageHelper<TEntity, TViewModel>` for additional functionality.

## Contributing

Contributions are welcome! To contribute:

1. Fork the repository.
2. Create a new branch (`feature/new-feature`).
3. Submit a pull request.

## License

This project is licensed under the MIT License.

## Support

For questions or issues, open an [issue on GitHub](https://github.com/yourrepo/issues).
