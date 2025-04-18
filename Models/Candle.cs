﻿using System.ComponentModel.DataAnnotations.Schema;

namespace TradeVault.Models;

public class Candle
{
    public int Id { get; set; }
    public string Symbol { get; set; }
    [NotMapped]
    public List<decimal> PriceValues { get; set; }
    public decimal StartPrice { get; set; }
    public decimal EndPrice { get; set; }
    public decimal MinPrice { get; set; }
    public decimal MaxPrice { get; set; }
    public decimal AveragePrice { get; set; }
    public DateTime Time { get; set; }
    public int TimeSpan { get; set; }
}