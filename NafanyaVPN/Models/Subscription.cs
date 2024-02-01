﻿namespace NafanyaVPN.Models;

public class Subscription
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal DailyCostInRoubles { get; set; }
    public DateTime NextUpdateTime { get; set; }
}