using System;

using NUnit.Framework;

namespace CryptoCurrency.Core.Tests
{
    [TestFixture]
    public class EpochTests
    {
        [Test]
        public void BasicResolve()
        {
            var dateTime = new DateTime(2000, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            var epoch = new Epoch(dateTime);

            Assert.AreEqual(epoch.TimestampMilliseconds, 946684800000);
        }

        private void ResolveLocalDateTime()
        {
            var dateTime = new DateTime(2000, 1, 1);

            new Epoch(dateTime);
        }

        [Test]
        public void OnlyAllowUtc()
        {
            Assert.Throws<Exception>(new TestDelegate(ResolveLocalDateTime));
        }
    }
}
