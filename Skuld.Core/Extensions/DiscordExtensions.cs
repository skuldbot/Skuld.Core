using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Skuld.Core.Utilities;
using System;
using System.Collections.Generic;
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

        public static string FullName(this IUser user)
            => $"{user.Username}#{user.Discriminator}";

        public static string FullNameWithNickname(this IGuildUser user)
        {
            if (string.IsNullOrEmpty(user.Nickname))
                return user.FullName();
            else
                return $"{user.FullName()} ({user.Nickname})";
        }

        public static async Task<int> RobotMembersAsync(this IGuild guild)
        {
            await guild.DownloadUsersAsync().ConfigureAwait(false);

            return (await guild.GetUsersAsync(CacheMode.AllowDownload)).Count(x => x.IsBot);
        }

        public static async Task<int> HumanMembersAsync(this IGuild guild)
        {
            await guild.DownloadUsersAsync().ConfigureAwait(false);

            return (await guild.GetUsersAsync(CacheMode.AllowDownload)).Count(x => !x.IsBot);
        }

        public static async Task<decimal> GetBotUserRatioAsync(this IGuild guild)
        {
            await guild.DownloadUsersAsync().ConfigureAwait(false);
            var botusers = await guild.RobotMembersAsync().ConfigureAwait(false);
            var userslist = await guild.GetUsersAsync().ConfigureAwait(false);
            var users = userslist.Count;
            return Math.Round((((decimal)botusers / users) * 100m), 2);
        }

        public static async Task<IList<IGuildUser>> GetUsersWithRoleAsync(this IGuild guild, IRole role)
        {
            List<IGuildUser> usersWithRole = new List<IGuildUser>();

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

        public static string HexFromStatus(this UserStatus status)
        {
            switch (status)
            {
                case UserStatus.Online:
                    return "#43b581";

                case UserStatus.AFK:
                case UserStatus.Idle:
                    return "#faa61a";

                case UserStatus.DoNotDisturb:
                    return "#f04747";

                case UserStatus.Invisible:
                case UserStatus.Offline:
                    return "#747f8d";

                default:
                    return "#fff";
            }
        }

        public static string ActivityToString(this IActivity activity)
        {
            StringBuilder builder = new StringBuilder();

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

            builder = builder.Append(" ");
            builder = builder.Append(activity.Name);

            return builder.ToString();
        }

        public static string StatusToEmote(this UserStatus status) => status switch
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

            return (highestRole != null ? highestRole.Color : Color.Default);
        }

        #endregion Information

        #region Formatting

        public static string TrimEmbedHiders(this string message)
        {
            if (string.IsNullOrEmpty(message)) return null;

            if (message.StartsWith("<"))
            {
                message = message.Substring(1);
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
                .ReplaceFirst("-uc", Convert.ToString(guild.MemberCount))
                .ReplaceFirst("-ud", "**" + user.FullName() + "**")
                .ReplaceFirst("-u", "**" + user.Username + "**");
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
            if (embed == null) return null;

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
                message += " | " + embed.Timestamp.Value.ToString("dd'/'MM'/'yyyy hh:mm:ss tt");
            }

            return message;
        }

        #endregion Formatting

        public static async Task DeleteAfterSecondsAsync(this IUserMessage message, int timeout)
        {
            await Task.Delay(timeout * 1000).ConfigureAwait(false);
            await message.DeleteAsync().ConfigureAwait(false);
        }

        public static async Task<bool> CanEmbedAsync(this IMessageChannel channel, IGuild guild = null)
        {
            if (guild == null) return true;
            else
            {
                var curr = await guild.GetCurrentUserAsync();
                var chan = await guild.GetChannelAsync(channel.Id);
                var perms = curr.GetPermissions(chan);
                return perms.EmbedLinks;
            }
        }

        public static string JumpLink(this IGuildChannel channel)
            => $"https://discordapp.com/channels/{channel.GuildId}/{channel.Id}";

        public static string GetModulePath(this ModuleInfo module)
        {
            if (!module.IsSubmodule)
            {
                return module.Name;
            }

            StringBuilder path = new StringBuilder();

            ModuleInfo previousModule = module;

            while (previousModule != null)
            {
                path.Append(previousModule.Name??previousModule.Group);

                if(previousModule.Parent != null)
                {
                    path.Append("/");
                }

                previousModule = previousModule.Parent;
            }

            var split = path.ToString().Split("/");

            return string.Join("/", split.Reverse());
        }

        public static ModuleInfo GetTopLevelParent(this ModuleInfo childModule)
        {
            if (!childModule.IsSubmodule)
            {
                return childModule;
            }

            ModuleInfo previousModule = childModule.Parent;

            while(previousModule.Parent != null)
            {
                previousModule = previousModule.Parent;
            }

            return previousModule;
        }
    }
}