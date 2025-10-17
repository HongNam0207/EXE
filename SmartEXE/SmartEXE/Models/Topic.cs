using System;
using System.Collections.Generic;

namespace SmartEXE.Models;

public partial class Topic
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<Location> Locations { get; set; } = new List<Location>();
}
