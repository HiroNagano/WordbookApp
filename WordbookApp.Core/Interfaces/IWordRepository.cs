using System.Collections.Generic;
using System.Threading.Tasks;
using WordbookApp.Core.Models;

namespace WordbookApp.Core.Interfaces;

/// <summary>
/// Defines data access operations for word entries.
/// </summary>
public interface IWordRepository
{
    /// <summary>
    /// Gets all word entries.
    /// </summary>
    Task<IReadOnlyList<Word>> GetAllAsync();

    /// <summary>
    /// Gets a word entry by its identifier.
    /// </summary>
    Task<Word?> GetByIdAsync(int id);

    /// <summary>
    /// Adds a new word entry.
    /// </summary>
    Task AddAsync(Word word);

    /// <summary>
    /// Updates an existing word entry.
    /// </summary>
    Task UpdateAsync(Word word);

    /// <summary>
    /// Deletes a word entry by its identifier.
    /// </summary>
    Task DeleteAsync(int id);
}
