using Setapp.Business;

namespace Setapp.Models
{
    public class PerformanceTestsResults
    {
        public readonly TimeComparision FillingData;
        public readonly TimeComparision RetrievingDataByIndexColumn;
        public readonly TimeComparision RetrievingDataByUnindexColumn;
        public readonly TimeComparision RetrievingMultipleObjectsByUnindexColumn;
        public readonly TimeComparision RetrievingCountByUnindexedValue;

        public PerformanceTestsResults(
            TimeComparision fillingData,
            TimeComparision retrievingDataByIndexColumn,
            TimeComparision retrievingDataByUnindexColumn,
            TimeComparision retrievingMultipleObjectsByUnindexColumn,
            TimeComparision retrievingCountByUnindexedValue)
        {
            FillingData = fillingData;
            RetrievingDataByIndexColumn = retrievingDataByIndexColumn;
            RetrievingDataByUnindexColumn = retrievingDataByUnindexColumn;
            RetrievingMultipleObjectsByUnindexColumn = retrievingMultipleObjectsByUnindexColumn;
            RetrievingCountByUnindexedValue = retrievingCountByUnindexedValue;
        }
    }
}