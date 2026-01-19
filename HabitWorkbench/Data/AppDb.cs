using HabitWorkbench.Data.Models;
using Microsoft.Maui.Controls;
using SQLite;

namespace HabitWorkbench.Data;

public sealed class AppDb
{
    private SQLiteAsyncConnection? _db;

    public async Task InitAsync()
    {
        if (_db is not null) return;

        var dbPath = Path.Combine(FileSystem.AppDataDirectory, "habitworkbench.db3");
        _db = new SQLiteAsyncConnection(dbPath);

        await _db.CreateTableAsync<Routine>();
        await _db.CreateTableAsync<Habit>();
        await _db.CreateTableAsync<Tactic>();
        await _db.CreateTableAsync<ContinualImprovement>();
    }

    public SQLiteAsyncConnection Conn
        => _db ?? throw new InvalidOperationException("Database not initialized. Call InitAsync() first.");
}
