﻿using System.ComponentModel.DataAnnotations;

namespace NestWebApp.DAL.Models;

public class ProductCategory
{
    [Key]
    public int Id { get; set; }
    public string? Title { get; set; }
    public bool IsDeleted { get; set; }
    public List<Product>? Product { get; set; }
}
