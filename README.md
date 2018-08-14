# downr2

Dirt-simple markdown blog system built using *ASP.NET Core* and *Razor Pages*.

## Metadata

The top section of each Markdown file must contain a YAML header with some simple metadata elements. All of these elements are required. The YAML below demonstrates this convention.

```yaml
---
title: Introducing downr2
date: 09-08-2018
categories: downr
---
```

## Image Path Fix-ups

At run-time, the src attributes for each image in your posts will be fixed automatically. 
This enables you to edit and preview your content in *Visual Studio Code* 
in exactly the same way it'll be rendered once you publish your blog.

From `![Sample post](media/header.png)` to
`<img src="/posts/sample-post/media/header.png" alt="Sample post">`

## Project Details

* Initial project structure created using `dotnet new downr`.
* The NuGet dependencies:
    * _Markdig_ is used for Markdown-to-HTML conversion.
    * _YamlDotNet_ is used to parse the YAML headers in post Markdown files.
    * The _HTML Agility Pack for .NET Core_ is used for proccessing HTML files.
* Bower is used to install the client-side JavaScript resources, 
specifically, Sematic UI, as this is used for the client-side experience construction.
* Obviously, NPM is being used to install the build resources

## Build Docker image

Downr supported Docker and you can built your project image.

* `docker build -t <your username>/downr .`
* `docker push <your username>/downr`

Run downr docker image localy:

`docker run -d -p 80:8080 <your username>/downr` if you want to run on port 8080.