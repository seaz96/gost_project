﻿namespace GostStorage.Models.Docs;

public class FtsSearchEntity
{
    public int Id { get; set; }
    public string? CodeOks { get; set; }
    public string Designation { get; set; }
    public string FullName { get; set; }
    public double Score { get; set; }
}