namespace Clean.Domain.Entities;

public class GeneralRequest : Request
{
    private GeneralRequest() { }

    private GeneralRequest(
        int requestBy,
        int requestTo,
        DateTime fromDate,
        DateTime toDate,
        string description
    )
        : base(requestBy, requestTo, fromDate, toDate, description) { }

    public static GeneralRequest Create(
        int requestBy,
        int requestTo,
        DateTime fromDate,
        DateTime toDate,
        string description
    )
    {
        return new GeneralRequest(requestBy, requestTo, fromDate, toDate, description);
    }
}
