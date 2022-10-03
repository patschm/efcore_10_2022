﻿namespace ACME.DataLayer.Entities;

public class Review: Entity
{
    public string? Text { get; set; }
    public byte Score { get; set; }
    public ReviewType ReviewType { get; set; }
    public long ProductId { get; set; }
    public DateTime DateBought { get; set; }
    public string? Organization { get; set; }
    public string? ReviewUrl { get; set; }

    public virtual Product? Product { get; set; } = null!;
    public virtual Reviewer? Reviewer { get; set; } = null!;
}
