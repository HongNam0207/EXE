using System;
using System.Collections.Generic;

namespace SmartCampusExplorer.Models.Entities;

public partial class ChatLog
{
    public Guid Id { get; set; }

    public Guid? UserId { get; set; }

    public string? Question { get; set; }

    public string? Response { get; set; }

    public DateTime? Timestamp { get; set; }

    public virtual User? User { get; set; }
}
