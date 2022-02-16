using System.Collections.Generic;
using Basket.Domain.Interfaces;

namespace Basket.Domain.Result
{
    //Added for return type.
    public class ResultWithEntity<TEntity> where TEntity: class, IEntity
    {
        public ResultWithEntity()
        {
            Result = true;
        }
        public TEntity Entity { get; set; }
        public bool Result { get; set; }
        public List<string> Message { get; set; } = new List<string>();
    }
}