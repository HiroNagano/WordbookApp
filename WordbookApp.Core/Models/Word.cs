using System.ComponentModel.DataAnnotations;
using SQLite;

namespace WordbookApp.Core.Models;

/// <summary>
/// Represents a word entry in the wordbook.
/// </summary>
[Table("Words")]
public class Word
{
	public const int MaxTextLength = 200;

	/// <summary>
	/// Gets or sets the unique identifier for the word entry.
	/// </summary>
	[PrimaryKey, AutoIncrement]
	public int Id { get; set; }

	/// <summary>
	/// Gets or sets the Japanese text.
	/// </summary>
	[Required(ErrorMessage = "Japanese is required.")]
	[System.ComponentModel.DataAnnotations.MaxLength(MaxTextLength, ErrorMessage = "Japanese must be {1} characters or fewer.")]
	public string Japanese { get; set; } = string.Empty;

	/// <summary>
	/// Gets or sets the English text.
	/// </summary>
	[Required(ErrorMessage = "English is required.")]
	[System.ComponentModel.DataAnnotations.MaxLength(MaxTextLength, ErrorMessage = "English must be {1} characters or fewer.")]
	public string English { get; set; } = string.Empty;
}
