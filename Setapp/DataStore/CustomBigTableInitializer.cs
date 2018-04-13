using EPiServer.Data;
using EPiServer.Framework;
using EPiServer.Data.Dynamic;
using EPiServer.Framework.Initialization;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using EPiServer.ServiceLocation;

namespace Setapp.DataStore
{
    [InitializableModule]
    [ModuleDependency(typeof(DataInitialization))]
    public class CustomBigTableInitializer : IInitializableModule
    {
        private const string StoreName = "PageViewsDataStore";
        private const string TableName = "tblPageViewsDataStoreBigTable";
        private static readonly Type ObjectType = typeof(CustomTablePageViewsData);

        private static readonly string PageViewsDataSqlCreateColumns =
            @"[PageId] int null,  
             [ViewsAmount] int null,";

        private static readonly IEnumerable<string> PageViewsDataSqlCreateIndexes = new[] { "PageId" };

        public void Initialize(InitializationEngine initializationEngine)
        {
            CreatePageViewTable();
            AssignTableToStore();
            ObjectType.GetOrCreateStore();
        }


        public void Uninitialize(InitializationEngine initializationEngine)
        {
        }

        private void CreatePageViewTable()
        {
            var databaseHandler = ServiceLocator.Current.GetInstance<IDatabaseHandler>();

            var tableUpdater = new DynamicDataStoreSqlProvider();
            string sqlCreateTable = tableUpdater.GetCreateTableSql(
                TableName,
                PageViewsDataSqlCreateColumns,
                StoreName,
                PageViewsDataSqlCreateIndexes);

            using (var connection = new SqlConnection(databaseHandler.ConnectionSettings.ConnectionString))
            {
                connection.Open();

                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = sqlCreateTable;
                    command.ExecuteNonQuery();
                }
            }
        }

        private void AssignTableToStore()
        {
            if (GlobalTypeToStoreMap.Instance.ContainsKey(ObjectType))
            {
                GlobalTypeToStoreMap.Instance.Remove(ObjectType);
            }

            GlobalTypeToStoreMap.Instance.Add(ObjectType, StoreName);

            var parameters = new StoreDefinitionParameters
            {
                TableName = TableName,
            };

            GlobalStoreDefinitionParametersMap.Instance.Add(StoreName, parameters);
        }
    }
}