namespace Eshop.Basket.Domain;

public class Basket
{
    public Guid Id { get; set ; }

    public IList<Item> Items { get; set; } = new List<Item>();

    public Guid UserId { get; set; }

    public Address Address { get; set; }

    public Basket(Guid id, Guid userId, IList<Item> items)
    {
        Id = id;
        UserId = userId;
        Items = items;
    }

    public Basket(){
    
    }

    public Basket(Guid id, Guid userId) : this(id, userId, new List<Item>()) { }

    public void AddDeliveryAddress(Address address)
    {
        Address = address;
    }

    public void AddProduct(Guid productId)
    {
        Items.Add(new Item(productId, 1));
    }

    
}
