namespace Clean.Domain.Enums;

public abstract class Enumeration
{
    public int Id { get; protected set; }
    public string Name { get; protected set; }

    protected Enumeration(int id, string name)
    {
        Id = id;
        Name = name;
    }
}
