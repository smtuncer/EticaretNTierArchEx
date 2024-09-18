﻿namespace NestWebApp.Models.ViewModels;

public class GetUserVM
{
    public string NameSurname { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public DateTimeOffset? LockoutEnd { get; set; }
    public IList<string> Roles { get; set; } = new List<string>();
}
