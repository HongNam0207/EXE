using System;
using System.Collections.Generic;

namespace SmartEXE.Models;

public partial class Partner
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Type { get; set; }

    public string? ContactEmail { get; set; }

    public string? Phone { get; set; }

    public string? Website { get; set; }

    public string? Description { get; set; }

    public DateTime? CreatedAt { get; set; }
}
