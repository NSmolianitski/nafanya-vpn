﻿using NafanyaVPN.Entities.Users;

namespace NafanyaVPN.Entities.Outline;

public class OutlineKey
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public int OutlineId { get; set; }
    public bool Enabled { get; set; }
    public string AccessUrl { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }
}