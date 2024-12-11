using System.Collections.Concurrent;

namespace Eshop.Basket.Domain
{
    public class Basket
    {
        private ConcurrentDictionary<Guid, Item> _items = new ConcurrentDictionary<Guid, Item>();

        public Guid Id { get; }

        public IReadOnlyCollection<Item> Items => _items.Select(s => s.Value).ToList();

        public Guid UserId { get; private set; }
        public Address Address { get; private set; }

        public Basket(Guid id, Guid userId, IReadOnlyCollection<Item> items)
        {
            Id = id;
            UserId = userId;
            _items = new ConcurrentDictionary<Guid, Item>(items.ToDictionary(i => i.ProductId));
        }

        public void AddDeliveryAddress(Address address)
        {
            Address = address;
        }

        public void AddProduct(Guid productId)
        {
            if (_items.TryGetValue(productId, out var item))
            {
                item.Quantity += 1;
            }

            else
            {
                _items.TryAdd(productId, new Item(productId, 1));
            }
        }

        public void RemoveProduct(Guid productId)
        {
            if (_items.TryGetValue(productId, out var item))
            {
                if(item.Quantity >= 2)
                {
                    item.Quantity -= 1;
                }
                else if(item.Quantity == 1)
                {
                    _items.TryRemove(productId, out _);
                }
                
            }
            else
            {
                throw new Exception("Product not found in basket"); // Better handling than throwing an exception
            }

        }
    }
}
