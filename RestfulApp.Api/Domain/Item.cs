namespace RestfulApp.Api.Domain;

[StronglyTypedId(jsonConverter: StronglyTypedIdJsonConverter.SystemTextJson)]
public partial struct ItemId { }

public class Item
{
    public ItemId Id { get; set; }
    public string Name { get; set; }
    public string UserId { get; set; }
}