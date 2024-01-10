namespace PhoneStore.Models;

public class Comment
{
    public int Id { get; set; }
    public int PhoneId { get; set; }
    public Phone Phone { get; set; }
    public string UserId { get; set; }
    public User User { get; set; }
    public string CommentText { get; set; }
    public DateTime CreatedAt { get; set; }
}