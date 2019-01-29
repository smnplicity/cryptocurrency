using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Microsoft.Extensions.Logging;

using CryptoCurrency.Core;
using CryptoCurrency.Core.Historian.Model;
using CryptoCurrency.Core.Exchange;
using CryptoCurrency.Core.Symbol;

namespace CryptoCurrency.Repository.Logging
{
    public class HistorianLogger : ILogger
    {
        private HistorianLoggerProvider LoggerProvider { get; set; }

        private string CategoryName { get; set; }

        private Dictionary<string, object> State { get; set; }
        
        public HistorianLogger(HistorianLoggerProvider loggerProvider, string categoryName)
        {
            LoggerProvider = loggerProvider;
            CategoryName = categoryName;
            State = new Dictionary<string, object>();
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            if (!ValidCategory())
                return state as IDisposable;

            var properties =
                from property in state.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
                select new
                {
                    Name = property.Name,
                    Value = property.GetValue(state, null)
                };

            foreach (var property in properties)
            {
                if (!State.ContainsKey(property.Name))
                    State.Add(property.Name, property.Value);
                else
                    State[property.Name] = property.Value;
            }

            return state as IDisposable;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return !(new[] { LogLevel.Debug, LogLevel.Trace }).Contains(logLevel);
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!ValidCategory())
                return;

            var logItem = new HistorianLogItem
            {
                Epoch = Epoch.Now,
                LogLevel = logLevel,
                Category = CategoryName,
                Message = formatter(state, exception),
                Exception = exception,
                Exchange = State.ContainsKey("Exchange") ? (ExchangeEnum)State["Exchange"] : (ExchangeEnum?)null,
                SymbolCode = State.ContainsKey("SymbolCode") ? (SymbolCodeEnum)State["SymbolCode"] : (SymbolCodeEnum?)null,
                Worker = State.ContainsKey("Worker") ? (string)State["Worker"] : null,
                Protocol = State.ContainsKey("Protocol") ? (string)State["Protocol"] : null
            };

            LoggerProvider.Enqueue(logItem);
        }

        private bool ValidCategory()
        {
            if (string.IsNullOrEmpty(CategoryName))
                return false;

            var parts = CategoryName.Split(".");

            return string.Equals(parts[0], "Historian");
        }
    }
}
