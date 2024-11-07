namespace Clean.Application.Dto.Base;

public record BaseDto
{
    public Guid GuidId { get; init; }
    public int? UpdatedBy { get; init; }
    public DateTime? UpdatedOn { get; init; }
    public DateTime AddedOn { get; init; }
    public int? AddedBy { get; init; }
}
