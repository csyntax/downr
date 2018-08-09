# downr

Dirt-simple markdown blog system built using *ASP.NET Core* and *Razor Pages*.

## Metadata

```yaml
---
title: Introducing downr2
date: 09-08-2018
categories: downr
---
```

## Project Details

* Initial project structure created using `dotnet new downr`.
* The NuGet dependencies:
    * _Markdig_ is used for Markdown-to-HTML conversion.
    * _YamlDotNet_ is used to parse the YAML headers in post Markdown files.
    * The _HTML Agility Pack for .NET Core_ is used for proccessing HTML files.