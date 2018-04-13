using EPiServer.Data;
using EPiServer.Data.Dynamic;

namespace Setapp.DataStore
{
    [EPiServerDataStore(AutomaticallyCreateStore = true, AutomaticallyRemapStore = true)]
    public class CustomTablePageViewsData
    {
        public Identity Id { get; set; }

        [EPiServerDataIndex]
        [EPiServerDataColumn(ColumnName = "PageId")]
        public int PageId { get; set; }

        [EPiServerDataColumn(ColumnName = "ViewsAmount")]
        public int ViewsAmount { get; set; }
    }
}