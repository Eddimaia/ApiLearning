namespace EstudoAPI.Models;

public class Post
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Summary { get; set; }
    public string Body { get; set; }
    public string Slug { get; set; }
    public DateTime CreateDate { get; set; }
    public DateTime LastUpdateDate { get; set; }
    public virtual Category Category { get; set; }
    public virtual User Author { get; set; }

    public List<Tag> Tags { get; set; }
}
