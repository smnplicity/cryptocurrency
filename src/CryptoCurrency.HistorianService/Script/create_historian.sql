CREATE DATABASE  IF NOT EXISTS `cryptocurrency_historian` /*!40100 DEFAULT CHARACTER SET utf8 COLLATE=utf8_bin  */;
USE `cryptocurrency_historian`;

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */; 
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `currency`
--

DROP TABLE IF EXISTS `currency`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `currency` (
  `currency_id` int(11) NOT NULL,
  `code` varchar(8) NOT NULL,
  `symbol` varchar(8) NOT NULL,
  `label` varchar(128) NULL,
  PRIMARY KEY (`currency_id`),
  UNIQUE KEY `currency_uix` (`code`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `exchange`
--

DROP TABLE IF EXISTS `exchange`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `exchange` (
  `exchange_id` int(11) NOT NULL,
  `name` varchar(32) NOT NULL,
  PRIMARY KEY (`exchange_id`),
  UNIQUE KEY `exchange_uix` (`exchange_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `exchange_symbol`
--

DROP TABLE IF EXISTS `exchange_symbol`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `exchange_symbol` (
  `exchange_id` int(11) NOT NULL,
  `symbol_id` int(11) NOT NULL,
  PRIMARY KEY (`exchange_id`,`symbol_id`),
  KEY `exchange_symbol_fk2_ix` (`symbol_id`),
  CONSTRAINT `exchange_symbol_fk1` FOREIGN KEY (`exchange_id`) REFERENCES `exchange` (`exchange_id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `exchange_symbol_fk2` FOREIGN KEY (`symbol_id`) REFERENCES `symbol` (`symbol_id`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `exchange_trade`
--

DROP TABLE IF EXISTS `exchange_trade`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `exchange_trade` (
  `exchange_id` int(11) NOT NULL,
  `symbol_id` int(11) NOT NULL,
  `timestamp` bigint(20) NOT NULL,
  `trade_id` bigint(20) NOT NULL AUTO_INCREMENT,
  `order_side_id` int(11) DEFAULT NULL,
  `price` decimal(32,16) NOT NULL,
  `volume` decimal(32,16) NOT NULL,
  `source_trade_id` varchar(64) DEFAULT NULL,
  PRIMARY KEY (`trade_id`,`exchange_id`,`symbol_id`,`timestamp`),
  UNIQUE KEY `exchange_trade_uix` (`exchange_id`,`symbol_id`,`source_trade_id`),
  KEY `exchange_trade_ix_1` (`exchange_id`,`symbol_id`,`timestamp`),
  KEY `exchange_trade_ix_2` (`timestamp`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8 COLLATE=utf8_bin;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `exchange_trade_aggregate`
--

DROP TABLE IF EXISTS `exchange_trade_aggregate`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `exchange_trade_aggregate` (
  `exchange_id` int(11) NOT NULL,
  `symbol_id` int(11) NOT NULL,
  `interval_key` varchar(8) NOT NULL,
  `timestamp` bigint(20) NOT NULL,
  `open` decimal(32,16) DEFAULT NULL,
  `open_timestamp` bigint(20) DEFAULT NULL,
  `high` decimal(32,16) DEFAULT NULL,
  `low` decimal(32,16) DEFAULT NULL,
  `close` decimal(32,16) DEFAULT NULL,
  `close_timestamp` bigint(20) DEFAULT NULL,
  `buy_volume` decimal(32,16) DEFAULT NULL,
  `sell_volume` decimal(32,16) DEFAULT NULL,
  `total_volume` decimal(32,16) DEFAULT NULL,
  `buy_count` int(11) DEFAULT NULL,
  `sell_count` int(11) DEFAULT NULL,
  `total_count` int(11) DEFAULT NULL,
  PRIMARY KEY (`exchange_id`,`symbol_id`,`timestamp`,`interval_key`),
  KEY `exchange_trade_aggregate_ix1` (`symbol_id`,`interval_key`,`timestamp`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `exchange_trade_stat`
--

DROP TABLE IF EXISTS `exchange_trade_stat`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `exchange_trade_stat` (
  `exchange_id` int(11) NOT NULL,
  `symbol_id` int(11) NOT NULL,
  `stat_key_id` int(11) NOT NULL,
  `timestamp` bigint(20) NOT NULL,
  `trade_stat_id` bigint(20) NOT NULL AUTO_INCREMENT,
  `value` decimal(32,16) NOT NULL,
  PRIMARY KEY (`trade_stat_id`,`exchange_id`,`symbol_id`,`stat_key_id`,`timestamp`),
  UNIQUE KEY `exchange_trade_stats_uix` (`exchange_id`,`symbol_id`,`stat_key_id`,`timestamp`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8 COLLATE=utf8_bin;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `historian_exchange_symbol`
--

DROP TABLE IF EXISTS `historian_exchange_symbol`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `historian_exchange_symbol` (
  `exchange_id` int(11) NOT NULL,
  `symbol_id` int(11) NOT NULL,
  `trade_filter` varchar(256) DEFAULT NULL,
  `last_trade_id` bigint(20) DEFAULT '0',
  `last_trade_stat_id` bigint(20) DEFAULT '0',
  PRIMARY KEY (`exchange_id`,`symbol_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `historian_log`
--

DROP TABLE IF EXISTS `historian_log`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `historian_log` (
  `log_id` int(11) NOT NULL AUTO_INCREMENT,
  `timestamp` bigint(20) NOT NULL,
  `log_level_id` int(11) NOT NULL,
  `category` varchar(128) NOT NULL,
  `message` varchar(8000) NOT NULL,
  `exception` longtext,
  `exchange_id` int(11) DEFAULT NULL,
  `symbol_id` int(11) DEFAULT NULL,
  `protocol` varchar(32) DEFAULT NULL,
  PRIMARY KEY (`log_id`),
  KEY `historian_log_ix_2` (`category`,`exchange_id`,`symbol_id`),
  KEY `historian_log_ix_3` (`category`,`timestamp`),
  KEY `historian_log_ix_1` (`exchange_id`,`category`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8 COLLATE=utf8_bin;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `historian_trade_catchup`
--

DROP TABLE IF EXISTS `historian_trade_catchup`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `historian_trade_catchup` (
  `exchange_id` int(11) NOT NULL,
  `symbol_id` int(11) NOT NULL,
  `trade_filter` varchar(256) NOT NULL,
  `timestamp_to` bigint(20) NOT NULL,
  `current_trade_filter` varchar(256) NOT NULL,
  `priority` int(11) NOT NULL,
  PRIMARY KEY (`exchange_id`,`symbol_id`,`trade_filter`(255)),
  CONSTRAINT `historian_trade_catchup_fk1` FOREIGN KEY (`exchange_id`, `symbol_id`) REFERENCES `historian_exchange_symbol` (`exchange_id`, `symbol_id`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `interval`
--

DROP TABLE IF EXISTS `interval`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `interval` (
  `interval_key` varchar(8) CHARACTER SET utf8 COLLATE utf8_bin NOT NULL,
  `from_timestamp` bigint(20) NOT NULL,
  `to_timestamp` bigint(20) NOT NULL,
  PRIMARY KEY (`interval_key`,`from_timestamp`,`to_timestamp`),
  KEY `IX_interval_1` (`from_timestamp`,`to_timestamp`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `interval_key`
--

DROP TABLE IF EXISTS `interval_key`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `interval_key` (
  `interval_key` varchar(8) NOT NULL,
  `interval_group_id` int(11) NOT NULL,
  `label` varchar(64) NOT NULL,
  PRIMARY KEY (`interval_key`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `order_side`
--

DROP TABLE IF EXISTS `order_side`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `order_side` (
  `order_side_id` int(11) NOT NULL,
  `label` varchar(16) NOT NULL,
  PRIMARY KEY (`order_side_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `symbol`
--

DROP TABLE IF EXISTS `symbol`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `symbol` (
  `symbol_id` int(11) NOT NULL,
  `code` varchar(16) NOT NULL,
  `base_currency_id` int(11) NOT NULL,
  `quote_currency_id` int(11) NOT NULL,
  `tradable` tinyint(1) NOT NULL,
  PRIMARY KEY (`symbol_id`),
  UNIQUE KEY `symbol_uix` (`code`),
  KEY `symbol_fk1_ix` (`base_currency_id`),
  KEY `symbol_fk2_ix` (`quote_currency_id`),
  CONSTRAINT `symbol_fk1` FOREIGN KEY (`base_currency_id`) REFERENCES `currency` (`currency_id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `symbol_fk2` FOREIGN KEY (`quote_currency_id`) REFERENCES `currency` (`currency_id`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin;
/*!40101 SET character_set_client = @saved_cs_client */;

/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;
