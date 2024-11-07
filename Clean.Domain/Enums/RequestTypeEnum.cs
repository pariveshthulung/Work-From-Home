namespace Clean.Domain.Enums;

public class RequestTypeEnum : Enumeration
{
    public RequestTypeEnum(int id, string name)
        : base(id, name) { }

    public static readonly RequestTypeEnum WorkFromHome = new RequestTypeEnum(1, "WorkFromHome");
    public static readonly RequestTypeEnum WorkFromOffice = new RequestTypeEnum(
        2,
        "WorkFromOffice"
    );
    public static readonly RequestTypeEnum Leave = new RequestTypeEnum(3, "Leave");

    private static readonly List<RequestTypeEnum> _requestTypes = new List<RequestTypeEnum>
    {
        WorkFromHome,
        WorkFromOffice,
        Leave
    };
    public static IEnumerable<RequestTypeEnum> RequestTypes => _requestTypes;

    public static RequestTypeEnum FromName(string name)
    {
        return _requestTypes.FirstOrDefault(x => x.Name == name)
            ?? throw new ArgumentException($"No RequestType with {name} is found");
    }

    public static RequestTypeEnum FromId(int id)
    {
        return _requestTypes.FirstOrDefault(x => x.Id == id)
            ?? throw new ArgumentException($"No RequestType with id {id} is found");
    }
}
