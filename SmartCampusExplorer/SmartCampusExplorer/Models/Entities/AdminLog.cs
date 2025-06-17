using System;
using System.Collections.Generic;

namespace SmartCampusExplorer.Models.Entities;

public partial class AdminLog
{
    public Guid Id { get; set; }

    public Guid? AdminId { get; set; }

    public string? Action { get; set; }

    public DateTime? Timestamp { get; set; }

    public virtual User? Admin { get; set; }
}
