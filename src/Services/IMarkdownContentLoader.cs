﻿namespace downr.Services
{
    public interface IMarkdownContentLoader
    {
        string GetContentToRender(string path, string slug);
    }
}