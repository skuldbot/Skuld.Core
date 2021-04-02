using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Skuld.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skuld.Core.Extensions
{
	public static class DiscordExtensions
	{
		#region Conversions

		public static Color FromHex(this string hex)
		{
			var col = System.Drawing.ColorTranslator.FromHtml(hex);
			return new Color(col.R, col.G, col.B);
		}

		public static string ToHex(this Color color)
			=> System.Drawing.ColorTranslator.ToHtml(System.Drawing.Color.FromArgb(255, color.R, color.G, color.B));

		public static string ToEmoji(this ClientType client)
		 => client switch
		 {
			 ClientType.Desktop => "🖥️",
			 ClientType.Mobile => "📱",
			 ClientType.Web => "🔗",

			 _ => "🖥️",
		 };

		#endregion Conversions

		#region Information

		public static string FullName([NotNull] this IUser user)
			=> $"{user.Username}#{user.Discriminator}";

		public static string FullNameWithNickname([NotNull] this IGuildUser user)
		{
			if (string.IsNullOrEmpty(user.Nickname))
				return user.FullName();
			else
				return $"{user.FullName()} ({user.Nickname})";
		}

		public static async Task<int> RobotMembersAsync([NotNull] this IGuild guild)
		{
			await guild.DownloadUsersAsync().ConfigureAwait(false);

			return (await guild.GetUsersAsync(CacheMode.AllowDownload).ConfigureAwait(false)).Count(x => x.IsBot);
		}

		public static async Task<int> HumanMembersAsync([NotNull] this IGuild guild)
		{
			await guild.DownloadUsersAsync().ConfigureAwait(false);

			return (await guild.GetUsersAsync(CacheMode.AllowDownload).ConfigureAwait(false)).Count(x => !x.IsBot);
		}

		public static async Task<decimal> GetBotUserRatioAsync([NotNull] this IGuild guild)
		{
			await guild.DownloadUsersAsync().ConfigureAwait(false);
			var botusers = await guild.RobotMembersAsync().ConfigureAwait(false);
			var userslist = await guild.GetUsersAsync().ConfigureAwait(false);
			var users = userslist.Count;
			return Math.Round((decimal)botusers / users * 100m, 2);
		}

		public static async Task<IList<IGuildUser>> GetUsersWithRoleAsync([NotNull] this IGuild guild, [NotNull] IRole role)
		{
			List<IGuildUser> usersWithRole = new();

			await guild.DownloadUsersAsync().ConfigureAwait(false);
			var users = await guild.GetUsersAsync().ConfigureAwait(false);

			foreach (var user in users)
			{
				if (user.RoleIds.Contains(role.Id))
				{
					usersWithRole.Add(user);
				}
			}

			return usersWithRole;
		}

		public static string HexFromStatus([NotNull] this UserStatus status)
			=> status switch
			{
				UserStatus.Online => "#43b581",
				UserStatus.AFK or UserStatus.Idle => "#faa61a",
				UserStatus.DoNotDisturb => "#f04747",
				UserStatus.Invisible or UserStatus.Offline => "#747f8d",
				_ => "#fff",
			};

		public static string ActivityToString([NotNull] this IActivity activity)
		{
			StringBuilder builder = new();

			switch (activity.Type)
			{
				case ActivityType.Listening:
					builder = builder.Append("Listening to");
					break;

				case ActivityType.Playing:
					builder = builder.Append("Playing");
					break;

				case ActivityType.Streaming:
					builder = builder.Append("Streaming");
					break;

				case ActivityType.Watching:
					builder = builder.Append("Watching");
					break;
			}

			builder = builder.Append(' ');
			builder = builder.Append(activity.Name);

			return builder.ToString();
		}

		public static string StatusToEmote([NotNull] this UserStatus status) => status switch
		{
			UserStatus.Online => DiscordUtilities.Online_Emote.ToString(),
			UserStatus.AFK => DiscordUtilities.Idle_Emote.ToString(),
			UserStatus.Idle => DiscordUtilities.Idle_Emote.ToString(),
			UserStatus.DoNotDisturb => DiscordUtilities.DoNotDisturb_Emote.ToString(),
			UserStatus.Invisible => DiscordUtilities.Offline_Emote.ToString(),
			UserStatus.Offline => DiscordUtilities.Offline_Emote.ToString(),

			_ => DiscordUtilities.Offline_Emote.ToString(),
		};

		public static string BoostMonthToEmote(this DateTimeOffset boostMonths)
		{
			var months = DateTime.UtcNow.MonthsBetween(boostMonths.Date);

			if (months <= 1)
			{
				return DiscordUtilities.NitroBoostRank1Emote.ToString();
			}
			if (months == 2)
			{
				return DiscordUtilities.NitroBoostRank2Emote.ToString();
			}
			if (months == 3)
			{
				return DiscordUtilities.NitroBoostRank3Emote.ToString();
			}
			if (months >= 4)
			{
				return DiscordUtilities.NitroBoostRank4Emote.ToString();
			}

			return null;
		}

		public static Color GetHighestRoleColor(this IGuildUser user, IGuild guild)
		{
			IRole highestRole = null;

			foreach (var roleid in user.RoleIds.OrderByDescending(x => x))
			{
				var role = guild.GetRole(roleid);

				if (role.Color == Color.Default)
				{
					continue;
				}

				highestRole = role;
				break;
			}

			return (highestRole is not null ? highestRole.Color : Color.Default);
		}

		#endregion Information

		#region Formatting

		public static string TrimEmbedHiders(this string message)
		{
			if (string.IsNullOrEmpty(message)) return null;

			if (message.StartsWith("<"))
			{
				message = message[1..];
			}
			if (message.EndsWith(">"))
			{
				message = message[0..^1];
			}

			return message;
		}

		public static string ReplaceGuildEventMessage(this string message, IUser user, SocketGuild guild)
		{
			if (string.IsNullOrEmpty(message)) return message;

			return message
				.ReplaceFirst("-m", "**" + user.Mention + "**")
				.ReplaceFirst("-s", "**" + guild.Name + "**")
				.ReplaceFirst("-uc", Convert.ToString(guild.MemberCount, CultureInfo.InvariantCulture))
				.ReplaceFirst("-ud", "**" + user.FullName() + "**")
				.ReplaceFirst("-u", "**" + user.Username + "**");
		}

		public static string ReplaceSocialEventMessage(this string message, string name, Uri url)
		{
			if (string.IsNullOrEmpty(message)) return message;

			return message
				.ReplaceFirst("-name", "**" + name + "**")
				.ReplaceFirst("-url", url.OriginalString);
		}

		public static string PruneMention(this string message, ulong id)
		{
			if (string.IsNullOrEmpty(message)) return message;

			return message
				.Replace($"<@{id}> ", "", StringComparison.InvariantCultureIgnoreCase)
				.Replace($"<@{id}>", "", StringComparison.InvariantCultureIgnoreCase)
				.Replace($"<@!{id}> ", "", StringComparison.InvariantCultureIgnoreCase)
				.Replace($"<@!{id}>", "", StringComparison.InvariantCultureIgnoreCase);
		}

		public static string ToMessage(this Embed embed)
		{
			if (embed is null) return null;

			string message = "";

			if (embed.Author.HasValue)
			{
				message += $"**__{embed.Author.Value.Name}__**\n";
			}
			if (!string.IsNullOrEmpty(embed.Title))
			{
				message += $"**{embed.Title}**\n";
			}
			if (!string.IsNullOrEmpty(embed.Description))
			{
				message += embed.Description + "\n";
			}

			foreach (var field in embed.Fields)
			{
				message += $"__{field.Name}__\n{field.Value}\n\n";
			}

			if (embed.Video.HasValue)
			{
				message += embed.Video.Value.Url + "\n";
			}
			if (embed.Thumbnail.HasValue)
			{
				message += embed.Thumbnail.Value.Url + "\n";
			}
			if (embed.Image.HasValue)
			{
				message += embed.Image.Value.Url + "\n";
			}
			if (embed.Footer.HasValue)
			{
				message += $"`{embed.Footer.Value.Text}`";
			}
			if (embed.Timestamp.HasValue)
			{
				message += " | " + embed.Timestamp.Value.ToString("dd'/'MM'/'yyyy hh:mm:ss tt", CultureInfo.InvariantCulture);
			}

			return message;
		}

		#endregion Formatting

		public static async Task DeleteAfterSecondsAsync(this IUserMessage message, int timeout)
		{
			await Task.Delay(timeout * 1000).ConfigureAwait(false);
			await message.DeleteAsync().ConfigureAwait(false);
		}

		public static async Task<bool> CanEmbedAsync([NotNull] this IMessageChannel channel, IGuild guild = null)
		{
			if (guild is null) return true;
			else
			{
				var curr = await guild.GetCurrentUserAsync().ConfigureAwait(false);
				var chan = await guild.GetChannelAsync(channel.Id).ConfigureAwait(false);
				var perms = curr.GetPermissions(chan);
				return perms.EmbedLinks;
			}
		}

		public static string JumpLink([NotNull] this IGuildChannel channel)
			=> $"https://discord.com/channels/{channel.GuildId}/{channel.Id}";

		public static string GetModulePath([NotNull] this ModuleInfo module)
		{
			if (!module.IsSubmodule)
			{
				return module.Name;
			}

			StringBuilder path = new();

			ModuleInfo previousModule = module;

			while (previousModule is not null)
			{
				path.Append(previousModule.Name ?? previousModule.Group);

				if (previousModule.Parent is not null)
				{
					path.Append('/');
				}

				previousModule = previousModule.Parent;
			}

			var split = path.ToString().Split("/");

			return string.Join("/", split.Reverse());
		}

		public static ModuleInfo GetTopLevelParent([NotNull] this ModuleInfo childModule)
		{
			if (!childModule.IsSubmodule)
			{
				return childModule;
			}

			ModuleInfo previousModule = childModule.Parent;

			while (previousModule.Parent is not null)
			{
				previousModule = previousModule.Parent;
			}

			return previousModule;
		}

		public static MessageReference GetReference(this IMessage message)
		{
			MessageReference reference = new(message.Id, message.Channel.Id);

			if (message.Channel is IGuildChannel guildChan)
			{
				reference = new(message.Id, message.Channel.Id, guildChan.Guild.Id);
			}

			return reference;
		}
	}
}