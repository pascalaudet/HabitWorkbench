using SQLite;

namespace HabitWorkbench.Data.Models;

[Table("continual_improvements")]
public class ContinualImprovement
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    [Indexed, NotNull]
    public int HabitId { get; set; }

    [MaxLength(800), NotNull]
    public string Text { get; set; } = "";

    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

    public bool IsArchived { get; set; } = false;
}
