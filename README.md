# downr3

Dirt-simple markdown blog system built using *ASP.NET Core* and *Razor Pages*.

## Blogging with downr

Blogging with downr is deliberately very simple: you basically just write Markdown. If you want to customize the style or HTML layout, you have 3 files to edit. Obviously, you can customize it all you want, but if you're simply into blogging with Markdown you never need to look at the source code.

Here are the basic conventions of blogging with downr:

* The Markdown and media assets for each post are stored in individual folders named according to the posts' slugs
* Each post must have a YAML header containing post metadata
* Each post's content is authored in an individual Markdown file named `index.md`
* Images and other media are stored in the `media` subfolder for each post's folder
* All posts' content are stored in the top-most `_posts` folder in the repository

## Metadata

The top section of each Markdown file must contain a YAML header with some simple metadata elements. All of these elements are required. The YAML below demonstrates this convention.

```yaml
---
title: Introducing downr3
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

`docker run -d -p 80:80 <your username>/downr` to run Docker image localy.