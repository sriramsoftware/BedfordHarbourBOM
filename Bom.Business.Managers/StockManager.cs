﻿using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Bom.Business.Contracts;
using Bom.Business.Entities;
using Bom.Data.Contracts;
using Core.Common.Contracts;

namespace Bom.Business.Managers
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall,
               ConcurrencyMode = ConcurrencyMode.Multiple,
               ReleaseServiceInstanceOnTransactionComplete = false)]
    public class StockManager : ManagerBase, IStockService
    {
        public StockManager()
        {
        }

        public StockManager(IDataRepositoryFactory dataRepositoryFactory)
        {
            _dataRepositoryFactory = dataRepositoryFactory;
        }

        [Import]
        private IDataRepositoryFactory _dataRepositoryFactory;

        public Stock[] GetAllStocks()
        {
            return ExecuteFaultHandledOperation(() =>
            {
                IStockRepository repo = _dataRepositoryFactory.GetDataRepository<IStockRepository>();
                var stocks = repo.Get();
                return stocks.ToArray();
            });
        }

        [OperationBehavior(TransactionScopeRequired = true)]
        public Stock UpdateStock(Stock stockItem)
        {
            return ExecuteFaultHandledOperation(() =>
            {
                IStockRepository stockRepository = _dataRepositoryFactory.GetDataRepository<IStockRepository>();

                Stock updatedEntity = null;

                if (stockItem.Id == 0)
                    updatedEntity = stockRepository.Add(stockItem);
                else
                    updatedEntity = stockRepository.Update(stockItem);

                return updatedEntity;
            });
        }

        public void DeleteStock(int stockItemId)
        {
            throw new NotImplementedException();
        }

        public StockItemData[] GetAllStockItems()
        {
            return ExecuteFaultHandledOperation(() =>
            {
                IStockRepository stockRepository = _dataRepositoryFactory.GetDataRepository<IStockRepository>();

                List<StockItemData> stockItems = new List<StockItemData>();

                IEnumerable<StockItemsInfo> stockInfoSet = stockRepository.GetAllStockItemsInfo();
                foreach (StockItemsInfo stockItemsInfo in stockInfoSet)
                {
                    stockItems.Add(new StockItemData()
                    {
                        StockId = stockItemsInfo.Stock.Id,
                        PartId = stockItemsInfo.Part.Id,
                        PartDescription = stockItemsInfo.Part.Description,
                        Count = stockItemsInfo.Stock.Count,
                        CountDate = stockItemsInfo.Stock.CountDate,
                        Cost = stockItemsInfo.Stock.Count * (stockItemsInfo.Part.ComponentsCost + stockItemsInfo.Part.OwnCost),
                        Notes = stockItemsInfo.Stock.Notes
                    });
                }

                return stockItems.ToArray();
            });
        }
    }
}
