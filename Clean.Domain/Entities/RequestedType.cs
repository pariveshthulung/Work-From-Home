namespace Clean.Domain.Entities;

public class RequestedType
{
    public int Id { get; private set; }
    public string Type { get; private set; }

    private RequestedType(int id, string type)
    {
        Id = id;
        Type = type;
    }

    public static RequestedType Create(int id, string type)
    {
        return new RequestedType(id, type);
    }

    public void Update(string type)
    {
        Type = type;
    }
}
