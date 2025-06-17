using System;
using System.Collections.Generic;

namespace SmartCampusExplorer.Models.Entities;

public partial class Map
{
    public Guid Id { get; set; }

    public Guid? LocationId { get; set; }

    public string? MapData { get; set; }

    public virtual Location? Location { get; set; }
}
