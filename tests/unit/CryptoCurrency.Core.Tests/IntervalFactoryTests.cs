using System;
using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;

using CryptoCurrency.Core.Interval;
using CryptoCurrency.Core.Interval.Group;

namespace CryptoCurrency.Core.Tests
{
    [TestFixture]
    public class IntervalFactoryTests
    {
        private IIntervalFactory IntervalFactory { get; set; }
        
        [SetUp]
        protected void SetUp()
        {
            var groups = new List<IIntervalGroup>();
            groups.Add(new Minute());
            groups.Add(new Hour());
            groups.Add(new Day());
            groups.Add(new Week());
            groups.Add(new Month());

            IntervalFactory = new IntervalFactory(groups);
        }

        [Test]
        public void GeneratesExpectedIntervalCountFor1d()
        {
            var from = new Epoch(new DateTime(2018, 1, 1, 0, 30, 0, DateTimeKind.Utc));
            var to = from.AddSeconds((int)TimeSpan.FromDays(5).TotalSeconds);

            var intervalKey = IntervalFactory.GetIntervalKey("1D");

            var intervals = IntervalFactory.GenerateIntervals(intervalKey, from, to);
            
            Assert.AreEqual(6, intervals.Count);
        }

        [Test]
        public void GeneratesExpectedWeekEpoch()
        {
            var from = new Epoch(new DateTime(2018, 1, 3, 0, 30, 0, DateTimeKind.Utc));
            var to = new Epoch(new DateTime(2018, 1, 3, 0, 30, 0, DateTimeKind.Utc));

            var expected = new Epoch(new DateTime(2018, 1, 1, 0, 0, 0, DateTimeKind.Utc));

            var intervalKey = IntervalFactory.GetIntervalKey("1W");

            var intervals = IntervalFactory.GenerateIntervals(intervalKey, from, to);
            var firstInterval = intervals.First();

            Assert.AreEqual(1, intervals.Count);
            Assert.AreEqual(expected.DateTime, firstInterval.From.DateTime);
        }

        [Test]
        public void GeneratesExpectedMonthEpoch()
        {
            var from = new Epoch(new DateTime(2018, 1, 14, 3, 4, 1, DateTimeKind.Utc));
            var to = new Epoch(new DateTime(2018, 1, 22, 1, 0, 3, DateTimeKind.Utc));

            var expected = new Epoch(new DateTime(2018, 1, 1, 0, 0, 0, DateTimeKind.Utc));

            var intervalKey = IntervalFactory.GetIntervalKey("1M");

            var intervals = IntervalFactory.GenerateIntervals(intervalKey, from, to);
            var firstInterval = intervals.First();

            Assert.AreEqual(1, intervals.Count);
            Assert.AreEqual(expected.DateTime, firstInterval.From.DateTime);
        }

        [Test]
        public void GetsExpectedDayIntervalNegativeOffset()
        {
            var epoch = new Epoch(new DateTime(2018, 1, 3, 12, 0, 0, DateTimeKind.Utc));

            var expected = new Epoch(new DateTime(2018, 1, 1, 0, 0, 0, DateTimeKind.Utc));

            var intervalKey = IntervalFactory.GetIntervalKey("1D");

            var evaluated = IntervalFactory.GetInterval(intervalKey, epoch, -2);

            Assert.AreEqual(expected.DateTime, evaluated.From.DateTime);
        }

        [Test]
        public void GetsExpectedDayIntervalPositiveOffset()
        {
            var epoch = new Epoch(new DateTime(2018, 1, 3, 12, 0, 0, DateTimeKind.Utc));

            var expected = new Epoch(new DateTime(2018, 1, 5, 0, 0, 0, DateTimeKind.Utc));

            var intervalKey = IntervalFactory.GetIntervalKey("1D");

            var evaluated = IntervalFactory.GetInterval(intervalKey, epoch, 2);

            Assert.AreEqual(expected.DateTime, evaluated.From.DateTime);
        }
    }
}
