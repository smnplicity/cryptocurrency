using System;

using Newtonsoft.Json;

namespace CryptoCurrency.Core
{
    public class Epoch : IComparable
    {
        public static Epoch Now
        {
            get
            {
                var timestamp = (long)DateTime.UtcNow.Subtract(Min).TotalMilliseconds;

                return Epoch.FromMilliseconds(timestamp);
            }
        }

        public static DateTime Min
        {
            get
            {
                return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            }
        }

        [JsonProperty("timestamp")]
        public long Timestamp { get; private set; }

        [JsonProperty("timestampMilliseconds")]
        public long TimestampMilliseconds { get; private set; }

        [JsonProperty("dateTime")]
        public DateTime DateTime { get; private set; }

        public static Epoch FromMilliseconds(double timestamp)
        {
            return new Epoch(Min.AddMilliseconds(timestamp));
        }

        public static Epoch FromSeconds(double timestamp)
        {
            return new Epoch(Min.AddSeconds(timestamp));
        }

        public Epoch(DateTime dateTime)
        {
            if (dateTime.Kind != DateTimeKind.Utc)
                throw new Exception("Provided date can only be UTC");

            DateTime = dateTime;
            TimestampMilliseconds = (long)(dateTime - Min).TotalMilliseconds;
            Timestamp = (long)(dateTime - Min).TotalSeconds;
        }

        public Epoch RoundUp(TimeSpan ts)
        {
            var modTicks = DateTime.Ticks % ts.Ticks;
            var delta = modTicks != 0 ? ts.Ticks - modTicks : 0;
            var rounded = new DateTime(DateTime.Ticks + delta, DateTime.Kind);

            return new Epoch(rounded);
        }

        public Epoch RoundDown(TimeSpan ts)
        {
            var delta = DateTime.Ticks % ts.Ticks;
            var rounded = new DateTime(DateTime.Ticks - delta, DateTime.Kind);

            return new Epoch(rounded);
        }

        public Epoch AddSeconds(int seconds)
        {
            return new Epoch(DateTime.AddSeconds(seconds));
        }

        public Epoch AddMilliseconds(int milliSeconds)
        {
            return new Epoch(DateTime.AddMilliseconds(milliSeconds));
        }

        public int CompareTo(object obj)
        {
            if (obj == null)
                return 1;

            var otherEpoch = obj as Epoch;

            if (otherEpoch != null)
                return TimestampMilliseconds.CompareTo(otherEpoch.TimestampMilliseconds);

            throw new ArgumentException("Comparing object is not an Epoch");
        }
    }
}
