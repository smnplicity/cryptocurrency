# cryptocurrency platform

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
