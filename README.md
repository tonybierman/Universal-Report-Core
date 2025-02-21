# Universal Report Core

A fast and versatile framework for presenting tabular data reports in ASP.NET Core.

## Features

- **Paging**: Efficiently handle large datasets with built-in pagination.
- **Column Sorting**: Sort data by columns dynamically.
- **Column Aggregation**: Supports sum, average, count, min, and max operations.
- **Cohort Aggregation**: Aggregate data within user-defined subsets.

## Screen shot

![Screenshot](screenshot.png)

## Installation

To install via NuGet:

```sh
dotnet add package BiermanTech.UniversalReportCore
```

Or add it to your `csproj` file:

```xml
<PackageReference Include="BiermanTech.UniversalReportCore" />
```

## Usage

### 1. Register report services in `Program.cs`

https://github.com/tonybierman/Universal-Report-Core/blob/b9da70afed1f5bb7d0ab7aa4ceeababf5b33a814/UniversalReportDemo/Program.cs#L52-L59

### 2. Create an entity that represents a row your report:

https://github.com/tonybierman/Universal-Report-Core/blob/b9da70afed1f5bb7d0ab7aa4ceeababf5b33a814/UniversalReportDemo/Data/CityPopulation.cs#L10-L27

### 3. Define the report's columns

https://github.com/tonybierman/Universal-Report-Core/blob/b9da70afed1f5bb7d0ab7aa4ceeababf5b33a814/UniversalReportDemo/Reports/CityPop/CityPopulationDemoReportColumnProvider.cs#L5-L53

### 4. Define the base query for the report

https://github.com/tonybierman/Universal-Report-Core/blob/b9da70afed1f5bb7d0ab7aa4ceeababf5b33a814/UniversalReportDemo/Reports/CityPop/CityPopulationDemoPageHelper.cs#L23-L41

## Contributing

Contributions are welcome! To contribute:

1. Fork the repository.
2. Create a new branch (`feature/new-feature`).
3. Submit a pull request.

## License

This project is licensed under the MIT License.

## Support

For questions or issues, open an [issue on GitHub](https://github.com/tonybierman/Universal-Report-Core/issues).
