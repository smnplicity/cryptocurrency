using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;

using CryptoCurrency.Core;
using CryptoCurrency.Core.Interval;
using CryptoCurrency.Repository.Edm.Historian;

namespace CryptoCurrency.Repository
{
    public class IntervalRepository : IIntervalRepository
    {
        private IIntervalFactory IntervalFactory { get; set; }

        private IDesignTimeDbContextFactory<HistorianDbContext> ContextFactory { get; set; }

        public IntervalRepository(IIntervalFactory intervalFactory, IDesignTimeDbContextFactory<HistorianDbContext> contextFactory)
        {
            IntervalFactory = intervalFactory;

            ContextFactory = contextFactory;
        }

        public async Task Add(IntervalKey intervalKey)
        {
            using (var context = ContextFactory.CreateDbContext(null))
            {
                if (await context.IntervalKey.FindAsync(intervalKey.Key) == null)
                {
                    await context.IntervalKey.AddAsync(new IntervalKeyEntity
                    {
                        IntervalKey = intervalKey.Key,
                        IntervalGroupId = (int)intervalKey.IntervalGroup,
                        Label = intervalKey.Label
                    });

                    await context.SaveChangesAsync();

                    var from = new DateTime(2008, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                    var to = new DateTime(DateTime.Now.Year + 2, 1, 1, 0, 0, 0, DateTimeKind.Utc);

                    var cursor = from;

                    while (cursor < to)
                    {
                        var intervals = IntervalFactory.GenerateIntervals(intervalKey, new Epoch(cursor), new Epoch(cursor.AddYears(1)));

                        await AddInterval(intervals);

                        cursor = cursor.AddYears(1);
                    }
                }
            }
        }

        public async Task AddInterval(ICollection<Interval> interval)
        {
            using (var context = ContextFactory.CreateDbContext(null))
            {
                // Process 100k at a time to manage memory
                var cursor = 0;
                var chunkCount = 100000;

                using (var cmd = context.Database.GetDbConnection().CreateCommand())
                {
                    await cmd.Connection.OpenAsync();

                    while (cursor <= interval.Count)
                    {
                        var chunk = interval.Skip(cursor).Take(chunkCount);

                        var sql = $"insert ignore into `interval` (`interval_key`, `from_timestamp`, `to_timestamp`) values {string.Join(",", chunk.Select(i => $"('{i.IntervalKey.Key}',{i.From.TimestampMilliseconds},{i.To.TimestampMilliseconds})"))}";

                        cmd.CommandText = sql;

                        await cmd.ExecuteNonQueryAsync();

                        cursor += chunkCount;
                    }
                }
            }
        }
    }
}
