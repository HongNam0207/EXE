using System;
using System.Collections.Generic;

namespace SmartCampusExplorer.Models.Entities;

public partial class Location
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Type { get; set; }

    public string? Description { get; set; }

    public string? Coordinates { get; set; }

    public string? ImageUrl { get; set; }

    public string? QrCodeUrl { get; set; }

    public string? GlbModelUrl { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<Analytic> Analytics { get; set; } = new List<Analytic>();

    public virtual ICollection<Map> Maps { get; set; } = new List<Map>();

    public virtual ICollection<Topic> Topics { get; set; } = new List<Topic>();
}
