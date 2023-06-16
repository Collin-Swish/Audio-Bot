// See https://aka.ms/new-console-template for more information
using DSharpPlus;

Console.WriteLine("Hello, World!");
namespace Audio_Bot
{
	internal class Program
	{
		static async Task Main(string[] args)
		{
			// If the authentication token is not provided via the commandline argument the bot reads from secret.txt
			// For authentication token details see https://discord.com/developers/docs/topics/oauth2
			string Token;
			if(args.Length == 0)
			{
				Token = args[1];
			}
			else
			{
				Token = File.ReadAllText("secret.txt");
			}
			// Loading config
			DiscordClient discord = new DiscordClient(new DiscordConfiguration()
			{
				Token = Token,
				TokenType = TokenType.Bot,
				Intents = DiscordIntents.MessageContents | DiscordIntents.GuildIntegrations | DiscordIntents.GuildVoiceStates | DiscordIntents.DirectMessages | DiscordIntents.MessageContents | DiscordIntents.Guilds
			});
			await discord.ConnectAsync(); // Connecting to discord API
			await Task.Delay(-1);         // Pausing execution and entering the event loop
		}
	}
}