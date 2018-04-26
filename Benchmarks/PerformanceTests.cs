using BenchmarkDotNet.Attributes;
using Benchmarks.DataStore;
using EPiServer.Data.Dynamic;
using System;
using System.ComponentModel;
using System.Linq;

namespace Benchmarks
{
    public class PerformanceTests
    {
        [Params(50000)]
        public int RowsInTableCount;

        [Benchmark]
        public void AddRemoveTestEF()
        {
            ClearAllEf();
            FillEntityFrameworkPageViewStore();
        }

        private void ClearAllEf()
        {
            var dbContext = new ApplicationDbContext();
            var entityFrameworkEntries = from pageViewData in dbContext.PageViewsData select pageViewData;
            dbContext.PageViewsData.RemoveRange(entityFrameworkEntries);
            dbContext.SaveChanges();
        }

        [Benchmark]
        public void AddRemoveTestDDS()
        {
            var dds = typeof(CustomTablePageViewsData);

            ClearAllDDS();
            FillCustomTablePageViewStoreForType(dds);
        }

        private void ClearAllDDS()
        {
            var dds = typeof(CustomTablePageViewsData);
            dds.GetOrCreateStore().DeleteAll();
        }

        [Benchmark]
        public void TestRetrievingDataByIndexedValueEF()
        {
            //todo idk if that materializes
            var items = new ApplicationDbContext().PageViewsData
                    .FirstOrDefault(item => item.PageId == 25000);
        }

        [Benchmark]
        public void TestRetrievingDataByIndexedValueDDS()
        {
            var dds = typeof(CustomTablePageViewsData);

            var item = dds.GetOrCreateStore()
                    .Items<CustomTablePageViewsData>()
                    .FirstOrDefault(x => x.PageId == 25000);
        }

        [Benchmark]
        public void TestRetrievingDataByUnindexedValueEF()
        {
            //todo idk if that materializes
            var item = new ApplicationDbContext().PageViewsData
                .FirstOrDefault(x => x.ViewsAmount == 25000);
        }

        [Benchmark]
        public void TestRetrievingDataByUnindexedValueDDS()
        {
            var dds = typeof(CustomTablePageViewsData);

            var items = dds.GetOrCreateStore()
                    .Items<CustomTablePageViewsData>()
                    .FirstOrDefault(item => item.ViewsAmount == 25000);
        }

        [Benchmark]
        public void TestRetrievingMultipleObjectsByUnindexedValueEF()
        {
            var items = new ApplicationDbContext().PageViewsData
                    .Where(item => item.ViewsAmount > 25000)
                    .ToList();
        }

        [Benchmark]
        public void TestRetrievingMultipleObjectsByUnindexedValueDDS()
        {
            var dds = typeof(CustomTablePageViewsData);

            var items = dds.GetOrCreateStore()
                    .Items<CustomTablePageViewsData>()
                    .Where(item => item.ViewsAmount > 25000)
                    .ToList();
        }

        [Benchmark]
        public void TestRetrievingCountByUnindexedValueEF()
        {
            var count = new ApplicationDbContext().PageViewsData
                    .Count(item => item.ViewsAmount > 25000);
        }

        [Benchmark]
        public void TestRetrievingCountByUnindexedValueDDS()
        {
            var dds = typeof(CustomTablePageViewsData);

            dds.GetOrCreateStore()
                    .Items<CustomTablePageViewsData>()
                    .Count(item => item.ViewsAmount > 25000);
        }

        private void FillEntityFrameworkPageViewStore()
        {
            var dbcontext = new ApplicationDbContext();
            var entries = new BindingList<EntityPageViewsData>();

            for (int i = 0; i < RowsInTableCount; i++)
            {
                entries.Add(new EntityPageViewsData
                {
                    PageId = i,
                    ViewsAmount = i
                });
            }

            dbcontext.PageViewsData.AddRange(entries);
            dbcontext.SaveChanges();
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