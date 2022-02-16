using System;
using System.Threading.Tasks;
using Basket.Domain.Interfaces;
using Basket.Infrastructure.Interfaces;
using MassTransit;

namespace Basket.Infrastructure.SendEndpointProvider
{
    //Added for subscriber maybe we have an another Subscriber who listen this queue.
    public class SendEndPointProviderHelper<TEntity> : ISendEndpointProviderHelper<TEntity> 
        where TEntity:class, IEntity
    {
        private readonly ISendEndpointProvider _sendEndpointProvider;
        private readonly ILogHelper<SendEndPointProviderHelper<TEntity>, TEntity> _logHelper;
        public SendEndPointProviderHelper(ISendEndpointProvider sendEndpointProvider, ILogHelper<SendEndPointProviderHelper<TEntity>, TEntity> logHelper)
        {
            _sendEndpointProvider = sendEndpointProvider;
            _logHelper = logHelper;
        }
        public async Task SendToQueue(TEntity entity)
        {
            try
            {
                var sendingEndPoint = await _sendEndpointProvider.GetSendEndpoint(new Uri($"queue:{typeof(TEntity)}-service"));
                await sendingEndPoint.Send<TEntity>(entity);
            }
            catch (Exception e)
            {
                _logHelper.CreatedButCouldNotSendToQueue(entity, e.Message);
            }
        }
    }
}