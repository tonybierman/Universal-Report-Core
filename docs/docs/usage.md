---
title: Usage
layout: default
---
# Usage

This guide shows how to use Universal Report Core in a .NET 8 application.

1. **Add the Library to Your Project**:
   - If Universal Report Core is available as a NuGet package, add it to your project:
     ```bash
     dotnet add package UniversalReportCore
     ```
   - If not, reference the project directly by adding it to your solution:
     ```bash
     dotnet sln add path/to/UniversalReportCore.csproj
     ```

2. **Create a Report**:
   - Example code (to be updated based on the actual API):
     ```csharp
     // Placeholder example
     var report = new UniversalReport();
     report.Title = "Sample Report";
     report.Columns = new List<ReportColumn>
     {
         new ReportColumn { Title = "Name", Property = "Name" },
         new ReportColumn { Title = "Value", Property = "Value" }
     };
     report.DataSource = new List<object>
     {
         new { Name = "Item 1", Value = 100 },
         new { Name = "Item 2", Value = 200 }
     };
     report.Generate();
     ```

3. **Render the Report**:
   - Depending on your application, you might render the report to a file, a web view, or another output format. *(Please provide details on how to render the report.)*

*Note*: This section is a placeholder. Please provide an example of how to use Universal Report Core in a .NET application.