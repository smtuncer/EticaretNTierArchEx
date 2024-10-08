﻿using System.ComponentModel.DataAnnotations;

namespace NestWebApp.DAL.Models;

public class Brand
{
    [Key]
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Image { get; set; }
}
