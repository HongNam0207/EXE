using System;
using System.Collections.Generic;

namespace SmartEXE.Models;

public partial class Map
{
    public Guid Id { get; set; }

    public Guid? LocationId { get; set; }

    public string? MapData { get; set; }

    public virtual Location? Location { get; set; }
}
