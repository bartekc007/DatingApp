using API.Entities;

namespace API.DTOs;

public class VPhoto : DtoBase
{
    public string Url { get; set; }
    public bool IsMain { get; set; }
    public int AppUserId { get; set; }
}