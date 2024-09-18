﻿using System.ComponentModel.DataAnnotations;

namespace NestWebApp.Models
{
    public class Brand
    {
        [Key]
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Image { get; set; }
    }
}
