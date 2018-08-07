﻿namespace downr.Models
{
    using System;

    public class Document
    {
        public string Slug { get; set; }

        public string Title { get; set; }

        public DateTime Date { get; set; }

        public string[] Categories { get; set; }

        public string Content { get; set; }
    }
}