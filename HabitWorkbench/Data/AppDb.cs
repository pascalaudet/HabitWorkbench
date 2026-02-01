using HabitWorkbench.Data.Models;
using SQLite;

namespace HabitWorkbench.Data;

public sealed class AppDb
{
    private SQLiteAsyncConnection? _db;
    private readonly SemaphoreSlim _initLock = new(1, 1);

    public async Task InitAsync()
    {
        if (_db is not null) return;

        await _initLock.WaitAsync();
        try
        {
            if (_db is not null) return;

            var dbPath = Path.Combine(FileSystem.AppDataDirectory, "habitworkbench.db3");
            Directory.CreateDirectory(Path.GetDirectoryName(dbPath)!);

            _db = new SQLiteAsyncConnection(dbPath);

            await _db.CreateTableAsync<Routine>();
            await _db.CreateTableAsync<Habit>();
            await _db.CreateTableAsync<Tactic>();
            await _db.CreateTableAsync<ContinualImprovement>();
        }
        finally
        {
            _initLock.Release();
        }
    }

    public SQLiteAsyncConnection Conn
        => _db ?? throw new InvalidOperationException("Database not initialized. Call InitAsync() first.");
}
