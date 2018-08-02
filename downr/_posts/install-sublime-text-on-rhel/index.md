---
title: Install Sublime Text on Red Hat Enterprise Linux
slug: install-sublime-text-on-rhel
date: 24-04-2018
---

![Install Sublime Text on Red Hat Enterprise Linux](/install-sublime-text-on-rhel/media/header.jpg)

*Sublime Text* is a sophisticated text editor for code, markup and prose.
You’ll love the slick user interface, extraordinary features and amazing performance. 
In this post I will show you how to install **Sublime Text 3** in **Red Hat Enterprise Linux**.

## Download the Sublime Text 3 from official site
You can download the latest version from the **Sublime Text** [website](https://www.sublimetext.com).
But I will use the "wget" command to download the tar-file.

```bash
wget https://download.sublimetext.com/sublime_text_3_build_3143_x64.tar.bz2
tar vxif ./sublime_text_3_build_3143_x64.tar.bz2
```

## Move the directory to the **/opt** folder
Once unpacked, you will see a directory called **Sublime Text 3**. 
Now move this folder to the **/opt** location. 

```bash
sudo mv sublime_text_3 /opt
```

## Create a symbolic link
Now I will create a symbolic link to call **Sublime Text** from the command line as **sublime**.

```bash
sudo ln -s /opt/sublime_text_3/sublime_text /usr/bin/sublime
```

## Start Sublime Text 3
To launch Sublime Text 3 from the command line, just type **sublime**.

```bash
sublime
```