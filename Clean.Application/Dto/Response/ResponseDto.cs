namespace Clean.Application.Dto.Response;

public class ResponseDto
{
    public string Message { get; set; } = default!;
    public bool IsSuccess { get; set; }
    public List<string> Errors { get; set; } = default!;
}
