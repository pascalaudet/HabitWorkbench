using SQLite;

namespace HabitWorkbench.Data.Models;

[Table("habits")]
public class Habit
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    [Indexed, NotNull]
    public int RoutineId { get; set; }

    [MaxLength(80), NotNull]
    public string Name { get; set; } = "";

    public decimal OrderInRoutine { get; set; }

    public bool Good { get; set; }

    [MaxLength(240)]
    public string? Trigger { get; set; }

    [MaxLength(400)]
    public string? Environment { get; set; }

    [MaxLength(240)]
    public string? Description { get; set; }

    [MaxLength(800)]
    public string? Notes { get; set; }
}
