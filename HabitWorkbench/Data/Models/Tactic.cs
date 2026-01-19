using SQLite;

namespace HabitWorkbench.Data.Models;

public enum TacticIntent
{
    Improve = 1,
    Reinforce = 2,
    Reduce = 3
}

[Table("tactics")]
public class Tactic
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    [Indexed, NotNull]
    public int HabitId { get; set; }

    [MaxLength(240), NotNull]
    public string Text { get; set; } = "";

    public TacticIntent Intent { get; set; }

    public decimal Order { get; set; }

    public bool IsActive { get; set; } = true;
}
