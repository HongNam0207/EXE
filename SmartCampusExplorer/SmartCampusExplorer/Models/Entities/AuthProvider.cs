using System;
using System.Collections.Generic;

namespace SmartCampusExplorer.Models.Entities;

public partial class AuthProvider
{
    public Guid Id { get; set; }

    public Guid? UserId { get; set; }

    public string? Provider { get; set; }

    public string? ProviderUid { get; set; }

    public string? AccessToken { get; set; }

    public string? RefreshToken { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual User? User { get; set; }
}
