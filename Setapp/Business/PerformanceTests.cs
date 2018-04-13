using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using EPiServer.Data.Dynamic;
using Setapp.DataStore;
using Setapp.Models;

namespace Setapp.Business
{
    public class PerformanceTests
    {
        private const int RowsInTableCount = 50000;

        private readonly ApplicationDbContext _applicationDbContext = new ApplicationDbContext();
        private readonly Type _customTableStoreType = typeof(CustomTablePageViewsData);

        public PerformanceTestsResults RunTests()
        {
            TimeComparision fillDataTimeComparision = FillTablesWithData();
            TimeComparision retrievingValueByIndexedColumnComparision = TestRetrievingDataByIndexedValue();
            TimeComparision retrievingValueByUnindexedColumnComparision = TestRetrievingDataByUnindexedValue();
            TimeComparision retrievingMultipleObjectsByUnindexedColumnComparision = TestRetrievingMultipleObjectsByUnindexedValue();
            TimeComparision retrievingCountByUnindexedValue = TestRetrievingCountByUnindexedValue();


            return new PerformanceTestsResults(
                fillDataTimeComparision,
                retrievingValueByIndexedColumnComparision,
                retrievingValueByUnindexedColumnComparision,
                retrievingMultipleObjectsByUnindexedColumnComparision,
                retrievingCountByUnindexedValue);
        }

        private TimeComparision FillTablesWithData()
        {
            const int iterationsCount = 10;
            double entityFrameworkTime = 0,
                customTableTime = 0;

            for (var i = 0; i < iterationsCount; i++)
            {
                var entityFrameworkEntries = from pageViewData in _applicationDbContext.PageViewsData select pageViewData;
                _applicationDbContext.PageViewsData.RemoveRange(entityFrameworkEntries);
                _applicationDbContext.SaveChanges();

                _customTableStoreType.GetOrCreateStore().DeleteAll();

                var entityFramworkWatch = Stopwatch.StartNew();
                FillEntityFrameworkPageViewStore();
                entityFramworkWatch.Stop();
                entityFrameworkTime += entityFramworkWatch.Elapsed.TotalMilliseconds;

                var customTableWatch = Stopwatch.StartNew();
                FillCustomTablePageViewStoreForType(_customTableStoreType);
                customTableWatch.Stop();
                customTableTime += customTableWatch.Elapsed.TotalMilliseconds;
            }

            double entityFramworkAvgTime = entityFrameworkTime / iterationsCount;
            double customTableAvg = customTableTime / iterationsCount;

            return new TimeComparision(entityFramworkAvgTime, customTableAvg);
        }

        private TimeComparision TestRetrievingDataByIndexedValue()
        {
            const int iterationsCount = 1000;
            double defaultTableTime = 0,
                customTableTime = 0;

            for (var i = 0; i < iterationsCount; i++)
            {
                var defaultTableWatch = Stopwatch.StartNew();
                _applicationDbContext.PageViewsData
                    .FirstOrDefault(item => item.PageId == 25000);
                defaultTableWatch.Stop();
                defaultTableTime += defaultTableWatch.Elapsed.TotalMilliseconds;

                var customTableWatch = Stopwatch.StartNew();
                _customTableStoreType.GetOrCreateStore()
                    .Items<CustomTablePageViewsData>()
                    .FirstOrDefault(item => item.PageId == 25000);
                customTableWatch.Stop();
                customTableTime += customTableWatch.Elapsed.TotalMilliseconds;
            }

            double defaultTableAvg = defaultTableTime / iterationsCount;
            double customTableAvg = customTableTime / iterationsCount;

            return new TimeComparision(defaultTableAvg, customTableAvg);
        }

        private TimeComparision TestRetrievingDataByUnindexedValue()
        {
            const int iterationsCount = 1000;
            double defaultTableTime = 0,
                customTableTime = 0;

            for (var i = 0; i < iterationsCount; i++)
            {
                var defaultTableWatch = Stopwatch.StartNew();
                _applicationDbContext.PageViewsData
                    .FirstOrDefault(item => item.ViewsAmount == 25000);
                defaultTableWatch.Stop();
                defaultTableTime += defaultTableWatch.Elapsed.TotalMilliseconds;

                var customTableWatch = Stopwatch.StartNew();
                _customTableStoreType.GetOrCreateStore()
                    .Items<CustomTablePageViewsData>()
                    .FirstOrDefault(item => item.ViewsAmount == 25000);
                customTableWatch.Stop();
                customTableTime += customTableWatch.Elapsed.TotalMilliseconds;
            }

            double defaultTableAvg = defaultTableTime / iterationsCount;
            double customTableAvg = customTableTime / iterationsCount;

            return new TimeComparision(defaultTableAvg, customTableAvg);
        }

        private TimeComparision TestRetrievingMultipleObjectsByUnindexedValue()
        {
            const int iterationsCount = 10;
            double defaultTableTime = 0,
                customTableTime = 0;

            for (var i = 0; i < iterationsCount; i++)
            {
                var defaultTableWatch = Stopwatch.StartNew();
                _applicationDbContext.PageViewsData
                    .Where(item => item.ViewsAmount > 25000)
                    .ToList();
                defaultTableWatch.Stop();
                defaultTableTime += defaultTableWatch.Elapsed.TotalMilliseconds;

                var customTableWatch = Stopwatch.StartNew();
                _customTableStoreType.GetOrCreateStore()
                    .Items<CustomTablePageViewsData>()
                    .Where(item => item.ViewsAmount > 25000)
                    .ToList();
                customTableWatch.Stop();
                customTableTime += customTableWatch.Elapsed.TotalMilliseconds;
            }

            double defaultTableAvg = defaultTableTime / iterationsCount;
            double customTableAvg = customTableTime / iterationsCount;

            return new TimeComparision(defaultTableAvg, customTableAvg);
        }

        private TimeComparision TestRetrievingCountByUnindexedValue()
        {
            const int iterationsCount = 500;
            double defaultTableTime = 0,
                customTableTime = 0;

            for (var i = 0; i < iterationsCount; i++)
            {
                var defaultTableWatch = Stopwatch.StartNew();
                _applicationDbContext.PageViewsData
                    .Count(item => item.ViewsAmount > 25000);
                defaultTableWatch.Stop();
                defaultTableTime += defaultTableWatch.Elapsed.TotalMilliseconds;

                var customTableWatch = Stopwatch.StartNew();
                _customTableStoreType.GetOrCreateStore()
                    .Items<CustomTablePageViewsData>()
                    .Count(item => item.ViewsAmount > 25000);
                customTableWatch.Stop();
                customTableTime += customTableWatch.Elapsed.TotalMilliseconds;
            }

            double defaultTableAvg = defaultTableTime / iterationsCount;
            double customTableAvg = customTableTime / iterationsCount;

            return new TimeComparision(defaultTableAvg, customTableAvg);
        }

        private void FillEntityFrameworkPageViewStore()
        {
            var entries = new BindingList<EntityPageViewsData>();

            for (int i = 0; i < RowsInTableCount; i++)
            {
                entries.Add(new EntityPageViewsData
                {
                    PageId = i,
                    ViewsAmount = i
                });
            }

            _applicationDbContext.PageViewsData.AddRange(entries);
            _applicationDbContext.SaveChanges();
        }

        private void FillCustomTablePageViewStoreForType(Type storeType)
        {
            DynamicDataStore store = storeType.GetOrCreateStore();

            for (int i = 0; i < RowsInTableCount; i++)
            {
                store.Save(new CustomTablePageViewsData
                {
                    PageId = i,
                    ViewsAmount = i
                });
            }
        }
    }
}