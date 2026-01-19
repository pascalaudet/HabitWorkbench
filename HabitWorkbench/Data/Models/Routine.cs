using SQLite;

namespace HabitWorkbench.Data.Models;

[Table("routines")]
public class Routine
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    [MaxLength(80), NotNull]
    public string Name { get; set; } = "";

    public decimal Order { get; set; }
}
