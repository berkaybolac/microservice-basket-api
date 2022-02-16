using System.Text;
using Basket.Domain.Constants;
using Basket.Domain.Interfaces;
using Basket.Infrastructure.Interfaces;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;

namespace Basket.Infrastructure.Logger
{
    //Added for log mechanism, we able to access everywhere.
    public class LogHelper<TService, TEntity>:ILogHelper<TService, TEntity> where TService: class
    where TEntity:class, IEntity
    {
        public readonly ILogger<TService> _logger;
        public LogHelper(ILogger<TService> logger)
        {
            _logger = logger;
        }
        
        //If customer insert something in him/her basket, automatic logging to Elasticsearch even if anonymous. 
        public void AnonymousCreatedCartLog(TEntity shoppingCardItem)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine(MessagesConstants.CreatedItem);
            stringBuilder.AppendLine(MessagesConstants.Anonymous);
            stringBuilder.AppendLine($"{shoppingCardItem.ToJson()}");
            _logger.Log(LogLevel.Information, stringBuilder.ToString());
        }
        
        //If customer insert something in him/her basket, automatic logging to Elasticsearch.
        public void CreatedCartLog(TEntity shoppingCardItem)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine(MessagesConstants.CreatedItem);
            stringBuilder.AppendLine($"{shoppingCardItem.ToJson()}");
            _logger.Log(LogLevel.Information, stringBuilder.ToString());
        }
        
        //If customer insert went wrong something in him/her basket, automatic logging to Elasticsearch.
        public void CreationFailedLog(TEntity shoppingCardItem, string exception)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine(MessagesConstants.CreationFailed);
            stringBuilder.AppendLine($"Exception {exception}");
            stringBuilder.AppendLine($"{shoppingCardItem.ToJson()}");
            _logger.Log(LogLevel.Information, stringBuilder.ToString());
        }
        
        //If customer insert something in him/her basket but something went wrong the mass-transmit, automatic logging to Elasticsearch.
        public void CreatedButCouldNotSendToQueue(TEntity shoppingCardItem,string exception)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine(MessagesConstants.CouldNotSentToQueue);
            stringBuilder.AppendLine($"Exception {exception}");
            stringBuilder.AppendLine($"{shoppingCardItem.ToJson()}");
            _logger.Log(LogLevel.Information, stringBuilder.ToString());
        }
    }
}