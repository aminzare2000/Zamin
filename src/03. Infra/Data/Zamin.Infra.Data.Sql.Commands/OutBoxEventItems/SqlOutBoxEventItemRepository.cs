﻿using Zamin.Infra.Events.Outbox;
using System.Collections.Generic;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Linq;
using Zamin.Utilities.Configurations;

namespace Zamin.Infra.Data.Sql.Commands.OutBoxEventItems
{
    public class SqlOutBoxEventItemRepository : IOutBoxEventItemRepository
    {
        private readonly ZaminConfigurations  _configurations;

        public SqlOutBoxEventItemRepository(ZaminConfigurations configurations)
        {
            _configurations = configurations;
        }
        public List<OutBoxEventItem> GetOutBoxEventItemsForPublishe(int maxCount = 100)
        {
            using var connection = new SqlConnection(_configurations.PoolingPublisher.SqlOutBoxEvent.ConnectionString);
            string query = string.Format(_configurations.PoolingPublisher.SqlOutBoxEvent.SelectCommand,maxCount);
            var result = connection.Query<OutBoxEventItem>(query).ToList();
            return result;
        }
        public void MarkAsRead(List<OutBoxEventItem> outBoxEventItems)
        {
            string idForMark = string.Join(',' ,outBoxEventItems.Where(c => c.IsProcessed).Select(c => c.OutBoxEventItemId).ToList());
            if (!string.IsNullOrWhiteSpace(idForMark))
            {
                using var connection = new SqlConnection(_configurations.PoolingPublisher.SqlOutBoxEvent.ConnectionString);
                string query = string.Format(_configurations.PoolingPublisher.SqlOutBoxEvent.UpdateCommand, idForMark);
                connection.Execute(query);
            }
        }
    }
}
