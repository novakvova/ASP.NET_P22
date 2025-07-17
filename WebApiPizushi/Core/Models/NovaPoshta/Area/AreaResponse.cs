namespace Core.Models.NovaPoshta.Area;

public class AreaResponse
{
    public bool Success { get; set; }
    public List<AreaItemResponse>? Data { get; set; }
    public List<object>? Errors { get; set; }
    public List<object>? Warnings { get; set; }
    public List<object>? Info { get; set; }
    public List<object>? MessageCodes { get; set; }
    public List<object>? ErrorCodes { get; set; }
    public List<object>? WarningCodes { get; set; }
    public List<object>? InfoCodes { get; set; }
}
