﻿using TradeVault.Models;

namespace TradeVault.Interfaces;

public interface ICandlesRepository
{
    Task AddCandle(Candle entity);
}