namespace Clean.Domain.Entities;

public class GeneralRequest : Request
{
    private GeneralRequest() { }

    private GeneralRequest(
        int requestBy,
        int requestTo,
        int requestedTypeId,
        DateTime fromDate,
        DateTime toDate
    )
        : base(requestBy, requestTo, requestedTypeId, fromDate, toDate) { }

    public static GeneralRequest Create(
        int requestBy,
        int requestTo,
        int requestedTypeId,
        DateTime fromDate,
        DateTime toDate
    )
    {
        return new GeneralRequest(requestBy, requestTo, requestedTypeId, fromDate, toDate);
    }
}
