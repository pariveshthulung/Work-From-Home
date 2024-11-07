namespace Clean.Application.Wrappers;

public class Error
{
    public int ErrorCode { get; set; }
    public string? FiledName { get; set; } = null;
    public string? Descriptions { get; set; } = null;

    public Error(int errorCode, string? filedName, string? description)
    {
        ErrorCode = errorCode;
        FiledName = filedName;
        Descriptions = description;
    }

    public override bool Equals(object? obj)
    {
        if (obj is not Error other)
            return false;
        return ErrorCode == other.ErrorCode
            && Descriptions == other.Descriptions
            && FiledName == other.FiledName;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Descriptions, ErrorCode, FiledName);
    }
}
