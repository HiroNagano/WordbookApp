using System.IO;
using Microsoft.Extensions.Logging;
using WordbookApp.Core.Interfaces;
using WordbookApp.Core.Repositories;
using WordbookApp.Core.Services;

namespace WordbookApp.Mobile;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

#if DEBUG
		builder.Logging.AddDebug();
#endif

		builder.Services.AddSingleton<IWordRepository>(provider =>
		{
			var databasePath = Path.Combine(FileSystem.AppDataDirectory, "wordbook.db3");
			return new SqliteWordRepository(databasePath);
		});
		builder.Services.AddSingleton<WordService>();

		return builder.Build();
	}
}
