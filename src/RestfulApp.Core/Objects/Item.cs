using RestfulApp.Core.ValueObjects;

namespace RestfulApp.Core.Objects;

public class Item
{
    public ItemId Id { get; set; }
    public string Name { get; set; }
    public string UserId { get; set; }
}
