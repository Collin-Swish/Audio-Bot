using DSharpPlus.Entities;
using DSharpPlus.Lavalink;
using DSharpPlus.SlashCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Audio_Bot
{
	// This class represents the slashcommands which will be instantiated on the server
	public class Slashcommand : ApplicationCommandModule
	{
		[SlashCommand("join", "Run this to make the bot join your voice channel")]
		public async Task Join(InteractionContext ctx)
		{
			var lava = ctx.Client.GetLavalink();
			if (!lava.ConnectedNodes.Any())
			{
				await ctx.CreateResponseAsync("The Lavalink connection is not established");
				return;
			}

			var node = lava.ConnectedNodes.Values.First();
			DiscordChannel channel = ctx.Member.VoiceState.Channel;
			if (channel == null)
			{
				await ctx.CreateResponseAsync("The User must be in a voice channel");
				return;
			}
			node.ConnectAsync(channel);
			await ctx.CreateResponseAsync($"Joined {channel.Name}");
		}

		[SlashCommand("play", "Play audio from this url")]
		public async Task Play(InteractionContext ctx, [Option("Url", "The url of the audio to play")] String URL)
		{
			// Performing null checks
			if (ctx.Member.VoiceState == null || ctx.Member.VoiceState.Channel == null)
			{
				await ctx.CreateResponseAsync("An error occurred");
				return;
			}
			var lava = ctx.Client.GetLavalink();
			var node = lava.ConnectedNodes.Values.First();
			var conn = node.GetGuildConnection(ctx.Member.VoiceState.Guild);
			if (conn == null)
			{
				await ctx.CreateResponseAsync("Lavalink is not connected");
				return;
			}
			var Response = await node.Rest.GetTracksAsync(URL);
			if (Response.LoadResultType == LavalinkLoadResultType.LoadFailed || Response.LoadResultType == LavalinkLoadResultType.NoMatches)
			{
				await ctx.CreateResponseAsync($"Track search failed for {URL}.");
				return;
			}
			var track = Response.Tracks.FirstOrDefault();
			await conn.PlayAsync(track);
			await ctx.CreateResponseAsync($"Now playing {track.Title}!");
		}
		[SlashCommand("pause","This command pauses the audio")]
		public async Task Pause(InteractionContext ctx)
		{
			if(ctx.Member.VoiceState == null || ctx.Member.VoiceState.Channel == null)
			{
				await ctx.CreateResponseAsync("You are not in a voice channel");
				return;
			}
			var lava = ctx.Client.GetLavalink();
			var node = lava.ConnectedNodes.Values.First();
			var conn = node.GetGuildConnection(ctx.Member.VoiceState.Guild);
			if(conn == null)
			{
				await ctx.CreateResponseAsync("Lavalink is not connected");
				return;
			}
			if (conn.CurrentState.CurrentTrack == null)
			{
				await ctx.CreateResponseAsync("There are no tracks loaded.");
				return;
			}
			await conn.PauseAsync();
		}
		[SlashCommand("unpause","This command unpauses the audio")]
		public async Task Unpause(InteractionContext ctx)
		{
			if (ctx.Member.VoiceState == null || ctx.Member.VoiceState.Channel == null)
			{
				await ctx.CreateResponseAsync("You are not in a voice channel");
				return;
			}
			var lava = ctx.Client.GetLavalink();
			var node = lava.ConnectedNodes.Values.First();
			var conn = node.GetGuildConnection(ctx.Member.VoiceState.Guild);
			if (conn == null)
			{
				await ctx.CreateResponseAsync("Lavalink is not connected");
				return;
			}
			if (conn.CurrentState.CurrentTrack == null)
			{
				await ctx.CreateResponseAsync("There are no tracks loaded.");
				return;
			}
			await conn.ResumeAsync();
		}
	}
}
