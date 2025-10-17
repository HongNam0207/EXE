using System;
using System.Collections.Generic;

namespace SmartEXE.Models;

public partial class Analytic
{
    public Guid Id { get; set; }

    public Guid? UserId { get; set; }

    public Guid? LocationId { get; set; }

    public string? Action { get; set; }

    public DateTime? Timestamp { get; set; }

    public virtual Location? Location { get; set; }

    public virtual User? User { get; set; }
}
