using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using WordbookApp.Core.Interfaces;
using WordbookApp.Core.Models;

namespace WordbookApp.Core.Services;

/// <summary>
/// Provides application-level operations for word entries.
/// </summary>
public class WordService
{
    private readonly IWordRepository _repository;

    /// <summary>
    /// Initializes a new instance of the <see cref="WordService"/> class.
    /// </summary>
    public WordService(IWordRepository repository)
    {
        _repository = repository;
    }

    /// <summary>
    /// Gets all word entries.
    /// </summary>
    public Task<IReadOnlyList<Word>> GetAllAsync()
    {
        return _repository.GetAllAsync();
    }

    /// <summary>
    /// Gets a word entry by its identifier.
    /// </summary>
    public Task<Word?> GetByIdAsync(int id)
    {
        return _repository.GetByIdAsync(id);
    }

    /// <summary>
    /// Adds a new word entry.
    /// </summary>
    public Task AddAsync(Word word)
    {
        ValidateWord(word);
        return _repository.AddAsync(word);
    }

    /// <summary>
    /// Updates an existing word entry.
    /// </summary>
    public Task UpdateAsync(Word word)
    {
        ValidateWord(word);
        return _repository.UpdateAsync(word);
    }

    /// <summary>
    /// Deletes a word entry by its identifier.
    /// </summary>
    public Task DeleteAsync(int id)
    {
        return _repository.DeleteAsync(id);
    }

    private static void ValidateWord(Word word)
    {
        if (word is null)
        {
            throw new ValidationException("Word cannot be null.");
        }

        var context = new ValidationContext(word);
        Validator.ValidateObject(word, context, validateAllProperties: true);
    }
}
