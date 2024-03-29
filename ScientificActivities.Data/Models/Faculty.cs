﻿namespace ScientificActivities.Data.Models;

public class Faculty
{
    public long Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public List<Department> Departments { get; set; } = null!;
}