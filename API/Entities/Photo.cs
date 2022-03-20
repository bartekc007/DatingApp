namespace API.Entities;

public class Photo: EntityBase
{
    public string Url { get; set; }
    public bool IsMain { get; set; }
    public string PublicId { get; set; }
    public AppUser AppUser { get; set; }
    public int AppUserId { get; set; }
}