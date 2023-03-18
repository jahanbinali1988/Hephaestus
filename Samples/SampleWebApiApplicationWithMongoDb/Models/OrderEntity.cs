using Hephaestus.Repository.Abstraction.Base;
using System;

namespace SampleWebApiApplicationWithMongoDb.Models
{
    public class OrderEntity : Entity
    {
        public OrderEntity(Guid Id)
        {
            this.Id = Id;
        }

        public static OrderEntity Create(Guid orderId)
        {
            var order = new OrderEntity(orderId);

            return order;
        }
    }
}
