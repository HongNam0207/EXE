using System;
using System.Collections.Generic;

namespace SmartEXE.Models;

public partial class Tour
{
    public Guid Id { get; set; }

    public string Title { get; set; } = null!;

    public string? Type { get; set; }

    public string? LocationIds { get; set; }
}
