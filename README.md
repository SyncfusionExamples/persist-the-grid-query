# Persist the Grid Query

## Repository Description

This repository demonstrates how to persist and restore the query state of a Syncfusion DataGrid so that sorting, filtering, paging, and other grid interactions remain intact across page reloads or navigation.

## Overview

The project shows a practical approach to capturing the grid’s query parameters and reapplying them when the grid is initialized again. This is useful in real-world applications where users expect the grid state to remain consistent when they refresh the page or return from another view. The example focuses on maintaining user context without requiring additional server-side storage.

## Key Features

- Persist grid query information such as sorting and filtering
- Restore grid state on reload or reinitialization
- Uses Syncfusion Grid component behavior
- Lightweight and easy-to-understand example implementation

## Prerequisites

- Visual Studio with ASP.NET Core support  
- .NET Framework compatible with ASP.NET MVC  
- Syncfusion EJ2 ASP.NET Core components  

## Installation

1. Clone the repository from GitHub.
2. Open the solution file in Visual Studio.
3. Restore NuGet packages if required.
4. Ensure the external service endpoint is accessible.
5. Build and run the application.un serve

## Resources

- https://ej2.syncfusion.com/aspnetcore/documentation/grid/state-management