using TodoApp.Models.Entities.Commons;

namespace TodoApp.Models.Entities.Concretes;

public class TodoItem : BaseEntity
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public bool IsCompleted { get; set; } = false;
    
    // Foreign Key
    public string? UserId { get; set; }
    // Navigation Properies
    public User User { get; set; }
}
