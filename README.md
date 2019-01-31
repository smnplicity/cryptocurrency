# cryptocurrency platform

## Features
* Collects cryptocurrency trades from exchanges and runs ohlc (and a bit more) aggregation on it
* The trades and trade aggregations are stored in a MySQL database in a standard structure
* Provides various market indicators such as RSI and MACD
* Supports private exchange API functionality e.g. placing orders

## Supported Exchanges

Exchange | Https | Web Socket
-------- | :-----: | :-----------:
Bitfinex | :heavy_check_mark: | :heavy_check_mark:
Binance | :heavy_check_mark:
CoinbasePro | :heavy_check_mark:
Kraken | :heavy_check_mark:

## Supported Symbols
Note: The symbols supported by exchange will differ.

Symbol | Description
-------- | -----
BTCAUD | Bitcoin / Australian Dollar
BTCUSD | Bitcoin / U.S Dollar
BTCUSDLONGS | Open long positions in BTCUSD on Bitfinex
BTCUSDSHORTS | Open short positions in BTCUSD on Bitfinex 
BTCUSDT | Bitcoin / Tether USD
ETCAUD | Ethereum Classic / Australian Dollar
ETHAUD | Ethereum / Australian Dollar
ETHBTC | Ethereum / Bitcoin
ETHBTCLONGS | Open long positions in ETHBTC on Bitfinex 
ETHBTCSHORTS | Open short positions in ETHBTC on Bitfinex
ETHUSD | Ethereum / U.S. Dollar
ETHUSDLONGS | Open long positions in ETHUSD on Bitfinex
ETHUSDSHORTS | Open short positions in BTCUSD on Bitfinex 
ETHUSDT | Ethereum / Tether USD
LTCAUD | Litecoin / Australian Dollar
LTCBTC | Litecoin / Bitcoin
LTCBTCLONGS | Open long positions in LTCBTC on Bitfinex 
LTCBTCSHORTS | Open short positions in LTCBTC on Bitfinex 
LTCUSD | Litecoin / U.S. Dollar
LTCUSDLONGS | Open long positions in LTCUSD on Bitfinex 
LTCUSDSHORTS | Open short positions in LTCUSD on Bitfinex 
LTCUSDT | Litecoin / Tether USD
XLMBTC | Stellar Lumens / Bitcoin
XLMETH | Stellar Lumens / Ethereum

## Supported OHLC intervals
Interval Key | Label
-------------|----------
1m | 1 minute
3m | 3 minutes
5m | 5 minutes
15m | 15 minutes
30m | 30 minutes
1h | 1 hour
2h | 2 hours
3h | 3 hours
4h | 4 hours
6h | 6 hours
12h | 12 hours
1D | 1 day
1W | 1 week
1M | 1 month
	
## Historian Service
### Installation
1. Run the database script found in src\CryptoCurrency.HistorianService\create_historian.sql on a MySQL instance
2. Create a user in MySQL with the following permissions on the schema: CREATE TEMPORARY TABLES, DELETE, EXECUTE, GRANT OPTION, INSERT, LOCK TABLES, SELECT, SHOW VIEW, UPDATE
3. In src\CryptoCurrency.HistorianService\appsettings.json:
    * Modify the Historian connection string to point to the above schema you just created
    * Modify the list of exchanges to get trades from (optional)
         * Note: The required hardware resources increases with every exchange worker that is running
4. Compile the HistorianService project
5. Run dotnet CryptoCurrency.HistorianService.dll

## Code examples

### C#
Get daily trade aggregate data for Kraken/BTCUSD for January 2019.

``` C#
var builder = new ConfigurationBuilder().AddJsonFile($"appsettings.json", true, true);

var appConfig = builder.Build();

var historianConnectionString = appConfig.GetConnectionString("Historian");

var serviceProvider = new ServiceCollection()
	.AddFactories()
	.AddExchangeFactory()
	.AddRepositories(historianConnectionString, historianConnectionString);
	
var intervalFactory = serviceProvider.GetService<IIntervalFactory>();
var marketRepository = serviceProvider.GetService<IMarketRepository>();

var intervalKey = intervalFactory.GetIntervalKey("1D");

var from = new Epoch(new DateTime(2019, 1, 1, 0, 0, 0, DateTimeKind.Utc));

var tradeAggregates = await marketRepository.GetTradeAggregates(ExchangeEnum.Kraken, SymbolCodeEnum.BTCUSD, intervalKey, from, 31);
```

### MySQL
Get daily trade aggregate data for Kraken/BTCUSD for January 2019.

Note: Timestamp values are milliseconds from unix epoch.

``` SQL
select * from `exchange_trade_aggregate` where `exchange_id` = 3 and `symbol_id` = 3 and `interval_key` = '1D' and `timestamp` between 1546300800000 and 1548892800000
```
