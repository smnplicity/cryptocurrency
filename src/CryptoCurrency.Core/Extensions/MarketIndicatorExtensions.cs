using System;

using CryptoCurrency.Core.MarketIndicator;

using static TicTacTec.TA.Library.Core;

namespace CryptoCurrency.Core.Extensions
{
    public static class MarketIndicatorExtensions
    {
        public static MAType ToTaLib(this MovingAverageTypeEnum maType)
        {
            switch(maType)
            {
                case MovingAverageTypeEnum.Simple:
                    return MAType.Sma;
                case MovingAverageTypeEnum.Exponential:
                    return MAType.Ema;
                default:
                    throw new NotSupportedException();
            }
        }
    }
}
