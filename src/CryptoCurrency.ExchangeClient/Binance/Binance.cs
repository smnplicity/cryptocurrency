using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using CryptoCurrency.Core;
using CryptoCurrency.Core.Currency;
using CryptoCurrency.Core.Exchange;
using CryptoCurrency.Core.Exchange.Model;
using CryptoCurrency.Core.Extensions;
using CryptoCurrency.Core.OrderSide;
using CryptoCurrency.Core.OrderState;
using CryptoCurrency.Core.OrderType;
using CryptoCurrency.Core.Symbol;

using CryptoCurrency.ExchangeClient.Binance.Model;

namespace CryptoCurrency.ExchangeClient.Binance
{
    public class Binance : IExchange
    {
        private ICurrencyFactory CurrencyFactory { get; set; }
        private ISymbolFactory SymbolFactory { get; set; }

        public Binance(ICurrencyFactory currencyFactory, ISymbolFactory symbolFactory)
        {
            CurrencyFactory = currencyFactory;
            SymbolFactory = symbolFactory;
        }

        public ExchangeEnum Name => ExchangeEnum.Binance;

        public ICollection<ExchangeCurrencyConverter> CurrencyConverter
        {
            get
            {
                return new List<ExchangeCurrencyConverter>()
                {
                    new ExchangeCurrencyConverter { CurrencyCode = CurrencyCodeEnum.YOYOW, AltCurrencyCode = "YOYO" },
                    new ExchangeCurrencyConverter { CurrencyCode = CurrencyCodeEnum.ETHOS, AltCurrencyCode = "BQX"}
                };
            }
        }

        public ICollection<SymbolCodeEnum> Symbol
        {
            get
            {
                return new List<SymbolCodeEnum>
                {
                    SymbolCodeEnum.BTCUSDT,
                    SymbolCodeEnum.ETHUSDT,
                    SymbolCodeEnum.LTCUSDT,
                    SymbolCodeEnum.ETHBTC,
                    SymbolCodeEnum.LTCBTC,
                    SymbolCodeEnum.XLMBTC,
                    SymbolCodeEnum.XLMETH,
                    SymbolCodeEnum.BNBBTC,
                    SymbolCodeEnum.NEOBTC,
                    SymbolCodeEnum.QTUMETH,
                    SymbolCodeEnum.EOSETH,
                    SymbolCodeEnum.SNTETH,
                    SymbolCodeEnum.BNTETH,
                    SymbolCodeEnum.GASBTC,
                    SymbolCodeEnum.BNBETH,
                    SymbolCodeEnum.OAXETH,
                    SymbolCodeEnum.DNTETH,
                    SymbolCodeEnum.MCOETH,
                    SymbolCodeEnum.MCOBTC,
                    SymbolCodeEnum.WTCBTC,
                    SymbolCodeEnum.WTCETH,
                    SymbolCodeEnum.LRCBTC,
                    SymbolCodeEnum.LRCETH,
                    SymbolCodeEnum.QTUMBTC,
                    SymbolCodeEnum.YOYOWBTC,
                    SymbolCodeEnum.OMGBTC,
                    SymbolCodeEnum.OMGETH,
                    SymbolCodeEnum.ZRXBTC,
                    SymbolCodeEnum.ZRXETH,
                    SymbolCodeEnum.STRATBTC,
                    SymbolCodeEnum.STRATETH,
                    SymbolCodeEnum.SNGLSBTC,
                    SymbolCodeEnum.SNGLSETH,
                    SymbolCodeEnum.ETHOSBTC,
                    SymbolCodeEnum.ETHOSETH,
                    SymbolCodeEnum.KNCBTC,
                    SymbolCodeEnum.KNCETH,
                    SymbolCodeEnum.FUNBTC,
                    SymbolCodeEnum.FUNETH,
                    SymbolCodeEnum.SNMBTC,
                    SymbolCodeEnum.SNMETH,
                    SymbolCodeEnum.NEOETH,
                    SymbolCodeEnum.IOTABTC,
                    SymbolCodeEnum.IOTAETH,
                    SymbolCodeEnum.LINKBTC,
                    SymbolCodeEnum.LINKETH,
                    SymbolCodeEnum.XVGBTC,
                    SymbolCodeEnum.XVGETH,
                    SymbolCodeEnum.MDABTC,
                    SymbolCodeEnum.MDAETH,
                    SymbolCodeEnum.MTLBTC,
                    SymbolCodeEnum.MTLETH,
                    SymbolCodeEnum.EOSBTC,
                    SymbolCodeEnum.SNTBTC,
                    SymbolCodeEnum.ETCETH,
                    SymbolCodeEnum.ETCBTC,
                    SymbolCodeEnum.MTHBTC,
                    SymbolCodeEnum.MTHETH,
                    SymbolCodeEnum.ENGBTC,
                    SymbolCodeEnum.ENGETH,
                    SymbolCodeEnum.DNTBTC,
                    SymbolCodeEnum.ZECBTC,
                    SymbolCodeEnum.ZECETH,
                    SymbolCodeEnum.BNTBTC,
                    SymbolCodeEnum.ASTBTC,
                    SymbolCodeEnum.ASTETH,
                    SymbolCodeEnum.DASHBTC,
                    SymbolCodeEnum.DASHETH,
                    SymbolCodeEnum.OAXBTC,
                    SymbolCodeEnum.EVXBTC,
                    SymbolCodeEnum.EVXETH,
                    SymbolCodeEnum.REQBTC,
                    SymbolCodeEnum.REQETH,
                    SymbolCodeEnum.VIBBTC,
                    SymbolCodeEnum.VIBETH,
                    SymbolCodeEnum.TRXBTC,
                    SymbolCodeEnum.TRXETH,
                    SymbolCodeEnum.POWRBTC,
                    SymbolCodeEnum.POWRETH,
                    SymbolCodeEnum.ARKBTC,
                    SymbolCodeEnum.ARKETH,
                    SymbolCodeEnum.YOYOWETH,
                    SymbolCodeEnum.XRPBTC,
                    SymbolCodeEnum.XRPETH,
                    SymbolCodeEnum.ENJBTC,
                    SymbolCodeEnum.ENJETH,
                    SymbolCodeEnum.STORJBTC,
                    SymbolCodeEnum.STORJETH,
                    SymbolCodeEnum.BNBUSDT,
                    SymbolCodeEnum.YOYOWBNB,
                    SymbolCodeEnum.POWRBNB,
                    SymbolCodeEnum.KMDBTC,
                    SymbolCodeEnum.KMDETH,
                    SymbolCodeEnum.NULSBNB,
                    SymbolCodeEnum.RCNBTC,
                    SymbolCodeEnum.RCNETH,
                    SymbolCodeEnum.RCNBNB,
                    SymbolCodeEnum.NULSBTC,
                    SymbolCodeEnum.NULSETH,
                    SymbolCodeEnum.RDNBTC,
                    SymbolCodeEnum.RDNETH,
                    SymbolCodeEnum.RDNBNB,
                    SymbolCodeEnum.XMRBTC,
                    SymbolCodeEnum.XMRETH,
                    SymbolCodeEnum.DLTBNB,
                    SymbolCodeEnum.WTCBNB,
                    SymbolCodeEnum.DLTBTC,
                    SymbolCodeEnum.DLTETH,
                    SymbolCodeEnum.AMBBTC,
                    SymbolCodeEnum.AMBETH,
                    SymbolCodeEnum.AMBBNB,
                    SymbolCodeEnum.BATBTC,
                    SymbolCodeEnum.BATETH,
                    SymbolCodeEnum.BATBNB,
                    SymbolCodeEnum.BCPTBTC,
                    SymbolCodeEnum.BCPTETH,
                    SymbolCodeEnum.BCPTBNB,
                    SymbolCodeEnum.ARNBTC,
                    SymbolCodeEnum.ARNETH,
                    SymbolCodeEnum.GVTBTC,
                    SymbolCodeEnum.GVTETH,
                    SymbolCodeEnum.CDTBTC,
                    SymbolCodeEnum.CDTETH,
                    SymbolCodeEnum.GXSBTC,
                    SymbolCodeEnum.GXSETH,
                    SymbolCodeEnum.NEOUSDT,
                    SymbolCodeEnum.NEOBNB,
                    SymbolCodeEnum.POEBTC,
                    SymbolCodeEnum.POEETH,
                    SymbolCodeEnum.QSPBTC,
                    SymbolCodeEnum.QSPETH,
                    SymbolCodeEnum.QSPBNB,
                    SymbolCodeEnum.BTSBTC,
                    SymbolCodeEnum.BTSETH,
                    SymbolCodeEnum.BTSBNB,
                    SymbolCodeEnum.XZCBTC,
                    SymbolCodeEnum.XZCETH,
                    SymbolCodeEnum.XZCBNB,
                    SymbolCodeEnum.LSKBTC,
                    SymbolCodeEnum.LSKETH,
                    SymbolCodeEnum.LSKBNB,
                    SymbolCodeEnum.TNTBTC,
                    SymbolCodeEnum.TNTETH,
                    SymbolCodeEnum.FUELBTC,
                    SymbolCodeEnum.FUELETH,
                    SymbolCodeEnum.MANABTC,
                    SymbolCodeEnum.MANAETH,
                    SymbolCodeEnum.BCDBTC,
                    SymbolCodeEnum.BCDETH,
                    SymbolCodeEnum.DGDBTC,
                    SymbolCodeEnum.DGDETH,
                    SymbolCodeEnum.IOTABNB,
                    SymbolCodeEnum.ADXBTC,
                    SymbolCodeEnum.ADXETH,
                    SymbolCodeEnum.ADXBNB,
                    SymbolCodeEnum.ADABTC,
                    SymbolCodeEnum.ADAETH,
                    SymbolCodeEnum.PPTBTC,
                    SymbolCodeEnum.PPTETH,
                    SymbolCodeEnum.CMTBTC,
                    SymbolCodeEnum.CMTETH,
                    SymbolCodeEnum.CMTBNB,
                    SymbolCodeEnum.XLMBNB,
                    SymbolCodeEnum.CNDBTC,
                    SymbolCodeEnum.CNDETH,
                    SymbolCodeEnum.CNDBNB,
                    SymbolCodeEnum.LENDBTC,
                    SymbolCodeEnum.LENDETH,
                    SymbolCodeEnum.WABIBTC,
                    SymbolCodeEnum.WABIETH,
                    SymbolCodeEnum.WABIBNB,
                    SymbolCodeEnum.LTCETH,
                    SymbolCodeEnum.LTCBNB,
                    SymbolCodeEnum.TNBBTC,
                    SymbolCodeEnum.TNBETH,
                    SymbolCodeEnum.WAVESBTC,
                    SymbolCodeEnum.WAVESETH,
                    SymbolCodeEnum.WAVESBNB,
                    SymbolCodeEnum.GTOBTC,
                    SymbolCodeEnum.GTOETH,
                    SymbolCodeEnum.GTOBNB,
                    SymbolCodeEnum.ICXBTC,
                    SymbolCodeEnum.ICXETH,
                    SymbolCodeEnum.ICXBNB,
                    SymbolCodeEnum.OSTBTC,
                    SymbolCodeEnum.OSTETH,
                    SymbolCodeEnum.OSTBNB,
                    SymbolCodeEnum.ELFBTC,
                    SymbolCodeEnum.ELFETH,
                    SymbolCodeEnum.AIONBTC,
                    SymbolCodeEnum.AIONETH,
                    SymbolCodeEnum.AIONBNB,
                    SymbolCodeEnum.NEBLBTC,
                    SymbolCodeEnum.NEBLETH,
                    SymbolCodeEnum.NEBLBNB,
                    SymbolCodeEnum.BRDBTC,
                    SymbolCodeEnum.BRDETH,
                    SymbolCodeEnum.BRDBNB,
                    SymbolCodeEnum.MCOBNB,
                    SymbolCodeEnum.EDOBTC,
                    SymbolCodeEnum.EDOETH,
                    SymbolCodeEnum.NAVBTC,
                    SymbolCodeEnum.NAVETH,
                    SymbolCodeEnum.NAVBNB,
                    SymbolCodeEnum.LUNBTC,
                    SymbolCodeEnum.LUNETH,
                    SymbolCodeEnum.APPCBTC,
                    SymbolCodeEnum.APPCETH,
                    SymbolCodeEnum.APPCBNB,
                    SymbolCodeEnum.VIBEBTC,
                    SymbolCodeEnum.VIBEETH,
                    SymbolCodeEnum.RLCBTC,
                    SymbolCodeEnum.RLCETH,
                    SymbolCodeEnum.RLCBNB,
                    SymbolCodeEnum.INSBTC,
                    SymbolCodeEnum.INSETH,
                    SymbolCodeEnum.PIVXBTC,
                    SymbolCodeEnum.PIVXETH,
                    SymbolCodeEnum.PIVXBNB,
                    SymbolCodeEnum.IOSTBTC,
                    SymbolCodeEnum.IOSTETH,
                    SymbolCodeEnum.STEEMBTC,
                    SymbolCodeEnum.STEEMETH,
                    SymbolCodeEnum.STEEMBNB,
                    SymbolCodeEnum.NANOBTC,
                    SymbolCodeEnum.NANOETH,
                    SymbolCodeEnum.NANOBNB,
                    SymbolCodeEnum.VIABTC,
                    SymbolCodeEnum.VIAETH,
                    SymbolCodeEnum.VIABNB,
                    SymbolCodeEnum.BLZBTC,
                    SymbolCodeEnum.BLZETH,
                    SymbolCodeEnum.BLZBNB,
                    SymbolCodeEnum.AEBTC,
                    SymbolCodeEnum.AEETH,
                    SymbolCodeEnum.AEBNB,
                    SymbolCodeEnum.NCASHBTC,
                    SymbolCodeEnum.NCASHETH,
                    SymbolCodeEnum.NCASHBNB,
                    SymbolCodeEnum.POABTC,
                    SymbolCodeEnum.POAETH,
                    SymbolCodeEnum.POABNB,
                    SymbolCodeEnum.ZILBTC,
                    SymbolCodeEnum.ZILETH,
                    SymbolCodeEnum.ZILBNB,
                    SymbolCodeEnum.ONTBTC,
                    SymbolCodeEnum.ONTETH,
                    SymbolCodeEnum.ONTBNB,
                    SymbolCodeEnum.STORMBTC,
                    SymbolCodeEnum.STORMETH,
                    SymbolCodeEnum.STORMBNB,
                    SymbolCodeEnum.QTUMBNB,
                    SymbolCodeEnum.QTUMUSDT,
                    SymbolCodeEnum.XEMBTC,
                    SymbolCodeEnum.XEMETH,
                    SymbolCodeEnum.XEMBNB,
                    SymbolCodeEnum.WANBTC,
                    SymbolCodeEnum.WANETH,
                    SymbolCodeEnum.WANBNB,
                    SymbolCodeEnum.WPRBTC,
                    SymbolCodeEnum.WPRETH,
                    SymbolCodeEnum.QLCBTC,
                    SymbolCodeEnum.QLCETH,
                    SymbolCodeEnum.SYSBTC,
                    SymbolCodeEnum.SYSETH,
                    SymbolCodeEnum.SYSBNB,
                    SymbolCodeEnum.QLCBNB,
                    SymbolCodeEnum.GRSBTC,
                    SymbolCodeEnum.GRSETH,
                    SymbolCodeEnum.ADAUSDT,
                    SymbolCodeEnum.ADABNB,
                    SymbolCodeEnum.GNTBTC,
                    SymbolCodeEnum.GNTETH,
                    SymbolCodeEnum.GNTBNB,
                    SymbolCodeEnum.LOOMBTC,
                    SymbolCodeEnum.LOOMETH,
                    SymbolCodeEnum.LOOMBNB,
                    SymbolCodeEnum.XRPUSDT,
                    SymbolCodeEnum.REPBTC,
                    SymbolCodeEnum.REPETH,
                    SymbolCodeEnum.REPBNB,
                    SymbolCodeEnum.TUSDBTC,
                    SymbolCodeEnum.TUSDETH,
                    SymbolCodeEnum.TUSDBNB,
                    SymbolCodeEnum.ZENBTC,
                    SymbolCodeEnum.ZENETH,
                    SymbolCodeEnum.ZENBNB,
                    SymbolCodeEnum.SKYBTC,
                    SymbolCodeEnum.SKYETH,
                    SymbolCodeEnum.SKYBNB,
                    SymbolCodeEnum.EOSUSDT,
                    SymbolCodeEnum.EOSBNB,
                    SymbolCodeEnum.CVCBTC,
                    SymbolCodeEnum.CVCETH,
                    SymbolCodeEnum.CVCBNB,
                    SymbolCodeEnum.THETABTC,
                    SymbolCodeEnum.THETAETH,
                    SymbolCodeEnum.THETABNB,
                    SymbolCodeEnum.XRPBNB,
                    SymbolCodeEnum.TUSDUSDT,
                    SymbolCodeEnum.IOTAUSDT,
                    SymbolCodeEnum.XLMUSDT,
                    SymbolCodeEnum.IOTXBTC,
                    SymbolCodeEnum.IOTXETH,
                    SymbolCodeEnum.QKCBTC,
                    SymbolCodeEnum.QKCETH,
                    SymbolCodeEnum.AGIBTC,
                    SymbolCodeEnum.AGIETH,
                    SymbolCodeEnum.AGIBNB,
                    SymbolCodeEnum.NXSBTC,
                    SymbolCodeEnum.NXSETH,
                    SymbolCodeEnum.NXSBNB,
                    SymbolCodeEnum.ENJBNB,
                    SymbolCodeEnum.DATABTC,
                    SymbolCodeEnum.DATAETH,
                    SymbolCodeEnum.ONTUSDT,
                    SymbolCodeEnum.TRXBNB,
                    SymbolCodeEnum.TRXUSDT,
                    SymbolCodeEnum.ETCUSDT,
                    SymbolCodeEnum.ETCBNB,
                    SymbolCodeEnum.ICXUSDT,
                    SymbolCodeEnum.SCBTC,
                    SymbolCodeEnum.SCETH,
                    SymbolCodeEnum.SCBNB,
                    SymbolCodeEnum.NPXSBTC,
                    SymbolCodeEnum.NPXSETH,
                    SymbolCodeEnum.KEYBTC,
                    SymbolCodeEnum.KEYETH,
                    SymbolCodeEnum.NASBTC,
                    SymbolCodeEnum.NASETH,
                    SymbolCodeEnum.NASBNB,
                    SymbolCodeEnum.MFTBTC,
                    SymbolCodeEnum.MFTETH,
                    SymbolCodeEnum.MFTBNB,
                    SymbolCodeEnum.DENTBTC,
                    SymbolCodeEnum.DENTETH,
                    SymbolCodeEnum.ARDRBTC,
                    SymbolCodeEnum.ARDRETH,
                    SymbolCodeEnum.ARDRBNB,
                    SymbolCodeEnum.NULSUSDT,
                    SymbolCodeEnum.HOTBTC,
                    SymbolCodeEnum.HOTETH,
                    SymbolCodeEnum.VETBTC,
                    SymbolCodeEnum.VETETH,
                    SymbolCodeEnum.VETUSDT,
                    SymbolCodeEnum.VETBNB,
                    SymbolCodeEnum.DOCKBTC,
                    SymbolCodeEnum.DOCKETH,
                    SymbolCodeEnum.POLYBTC,
                    SymbolCodeEnum.POLYBNB,
                    SymbolCodeEnum.PHXBTC,
                    SymbolCodeEnum.PHXETH,
                    SymbolCodeEnum.PHXBNB,
                    SymbolCodeEnum.HCBTC,
                    SymbolCodeEnum.HCETH,
                    SymbolCodeEnum.GOBTC,
                    SymbolCodeEnum.GOBNB,
                    SymbolCodeEnum.PAXUSDT,
                    SymbolCodeEnum.RVNBTC,
                    SymbolCodeEnum.RVNBNB,
                    SymbolCodeEnum.DCRBTC,
                    SymbolCodeEnum.DCRBNB,
                    SymbolCodeEnum.MITHBTC,
                    SymbolCodeEnum.MITHBNB,
                    SymbolCodeEnum.BNBPAX,
                    SymbolCodeEnum.BTCPAX,
                    SymbolCodeEnum.ETHPAX,
                    SymbolCodeEnum.XRPPAX,
                    SymbolCodeEnum.EOSPAX,
                    SymbolCodeEnum.XLMPAX,
                    SymbolCodeEnum.RENBTC,
                    SymbolCodeEnum.RENBNB,
                    SymbolCodeEnum.XRPTUSD,
                    SymbolCodeEnum.EOSTUSD,
                    SymbolCodeEnum.XLMTUSD,
                    SymbolCodeEnum.BNBUSDC,
                    SymbolCodeEnum.BTCUSDC,
                    SymbolCodeEnum.ETHUSDC,
                    SymbolCodeEnum.XRPUSDC,
                    SymbolCodeEnum.EOSUSDC,
                    SymbolCodeEnum.XLMUSDC,
                    SymbolCodeEnum.USDCUSDT,
                    SymbolCodeEnum.ADATUSD,
                    SymbolCodeEnum.TRXTUSD,
                    SymbolCodeEnum.NEOTUSD,
                    SymbolCodeEnum.TRXXRP,
                    SymbolCodeEnum.XZCXRP,
                    SymbolCodeEnum.PAXTUSD,
                    SymbolCodeEnum.USDCTUSD,
                    SymbolCodeEnum.USDCPAX,
                    SymbolCodeEnum.LINKUSDT,
                    SymbolCodeEnum.LINKTUSD,
                    SymbolCodeEnum.LINKPAX,
                    SymbolCodeEnum.LINKUSDC,
                    SymbolCodeEnum.WAVESUSDT,
                    SymbolCodeEnum.WAVESTUSD,
                    SymbolCodeEnum.WAVESPAX,
                    SymbolCodeEnum.WAVESUSDC,
                    SymbolCodeEnum.LTCTUSD,
                    SymbolCodeEnum.LTCPAX,
                    SymbolCodeEnum.LTCUSDC,
                    SymbolCodeEnum.TRXPAX,
                    SymbolCodeEnum.TRXUSDC,
                    SymbolCodeEnum.BNBUSDS,
                    SymbolCodeEnum.BTCUSDS,
                    SymbolCodeEnum.USDSUSDT,
                    SymbolCodeEnum.USDSPAX,
                    SymbolCodeEnum.USDSTUSD,
                    SymbolCodeEnum.USDSUSDC,
                    SymbolCodeEnum.ONGBNB,
                    SymbolCodeEnum.ONGBTC,
                    SymbolCodeEnum.ONGUSDT,
                    SymbolCodeEnum.HOTBNB,
                    SymbolCodeEnum.HOTUSDT,
                    SymbolCodeEnum.ZILUSDT,
                };
            }
        }
        
        public ICollection<ExchangeStatsKeyEnum> SupportedStatKeys => null;

        public bool SupportsHistoricalLoad => true;

        public IExchangeHttpClient GetHttpClient()
        {
            return new Http.Client(this, CurrencyFactory, SymbolFactory);
        }

        public IExchangeWebSocketClient GetWebSocketClient()
        {
            return new WebSocket.Client(this, CurrencyFactory, SymbolFactory);
        }

        #region Custom functionality
        private BinanceInfo Info { get; set; }

        public async Task<BinanceInfo> GetExchangeInfo()
        {
            if (Info == null)
                Info = await HttpProxy.GetJson<BinanceInfo>(GetHttpClient().GetFullUrl("v1/exchangeInfo"), null);
  
            return Info;
        }

        public OrderSideEnum GetOrderSide(string side)
        {
            switch(side)
            {
                case "SELL":
                    return OrderSideEnum.Sell;
                default:
                    return OrderSideEnum.Buy;
            }
        }

        public OrderTypeEnum GetOrderType(string type)
        {
            switch(type)
            {
                case "MARKET":
                    return OrderTypeEnum.Market;
                case "LIMIT":
                    return OrderTypeEnum.Limit;
            }

            throw new Exception($"Order Type {type} is not supported.");
        }

        public OrderStateEnum GetOrderState(string state)
        {
            switch (state)
            {
                case "NEW":
                    return OrderStateEnum.Pending;
                case "PARTIALLY_FILLED":
                    return OrderStateEnum.Processing;
                case "FILLED":
                    return OrderStateEnum.Complete;
                case "CANCELED":
                case "REJECTED":
                case "EXPIRED":
                    return OrderStateEnum.Cancelled;
            }

            throw new Exception($"Order State {state} is not supported.");
        }
        #endregion
    }
}
