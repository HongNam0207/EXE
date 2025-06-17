using System;
using System.Collections.Generic;

namespace SmartCampusExplorer.Models.Entities;

public partial class User
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? PasswordHash { get; set; }

    public string? Role { get; set; }

    public string? AvatarUrl { get; set; }

    public string? LoginMethod { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<AdminLog> AdminLogs { get; set; } = new List<AdminLog>();

    public virtual ICollection<Analytic> Analytics { get; set; } = new List<Analytic>();

    public virtual ICollection<AuthProvider> AuthProviders { get; set; } = new List<AuthProvider>();

    public virtual ICollection<ChatLog> ChatLogs { get; set; } = new List<ChatLog>();
}
