using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SQLite;
using WordbookApp.Core.Interfaces;
using WordbookApp.Core.Models;

namespace WordbookApp.Core.Repositories;

public class SqliteWordRepository : IWordRepository
{
    private readonly SQLiteAsyncConnection _database;
    private bool _initialized;

    public SqliteWordRepository(string databasePath)
    {
        if (string.IsNullOrWhiteSpace(databasePath))
        {
            throw new ArgumentException("Database path is required.", nameof(databasePath));
        }

        _database = new SQLiteAsyncConnection(databasePath);
    }

    private async Task EnsureInitializedAsync()
    {
        if (_initialized)
        {
            return;
        }

        await _database.CreateTableAsync<Word>();
        _initialized = true;
    }

    public async Task<IReadOnlyList<Word>> GetAllAsync()
    {
        await EnsureInitializedAsync();
        return await _database.Table<Word>().OrderBy(word => word.Id).ToListAsync();
    }

    public async Task<Word?> GetByIdAsync(int id)
    {
        await EnsureInitializedAsync();
        return await _database.FindAsync<Word>(id);
    }

    public async Task AddAsync(Word word)
    {
        if (word is null)
        {
            throw new ArgumentNullException(nameof(word));
        }

        await EnsureInitializedAsync();
        await _database.InsertAsync(word);
    }

    public async Task UpdateAsync(Word word)
    {
        if (word is null)
        {
            throw new ArgumentNullException(nameof(word));
        }

        await EnsureInitializedAsync();
        await _database.UpdateAsync(word);
    }

    public async Task DeleteAsync(int id)
    {
        await EnsureInitializedAsync();
        await _database.DeleteAsync<Word>(id);
    }
}
