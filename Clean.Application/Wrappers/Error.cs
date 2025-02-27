namespace Clean.Application.Wrappers;

public class Error
{
    public int ErrorCode { get; set; }
    public string? FieldName { get; set; } = null;
    public string? Descriptions { get; set; } = null;

    public Error(int errorCode, string? fieldName, string? description)
    {
        ErrorCode = errorCode;
        FieldName = fieldName;
        Descriptions = description;
    }

    public override bool Equals(object? obj)
    {
        if (obj is not Error other)
            return false;
        return ErrorCode == other.ErrorCode
            && Descriptions == other.Descriptions
            && FieldName == other.FieldName;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Descriptions, ErrorCode, FieldName);
    }
}
