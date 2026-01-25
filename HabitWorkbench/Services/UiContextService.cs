namespace HabitWorkbench.Services;

public class UiContextService
{
    public event Action? OnChange;

    public string Left { get; private set; } = "HabitWorkbench";
    public string? Right { get; private set; } = null;

    public void Set(string left, string? right = null)
    {
        Left = left;
        Right = right;
        OnChange?.Invoke();
    }

    public void Clear(string left = "HabitWorkbench")
    {
        Left = left;
        Right = null;
        OnChange?.Invoke();
    }
}
