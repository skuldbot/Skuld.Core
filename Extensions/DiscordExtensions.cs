using Discord;
using NodaTime;
using Skuld.Core.Extensions.Formatting;
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
        public static string FullName(this IUser usr)
            => $"{usr.Username}#{usr.Discriminator}";

        public static string FullNameWithNickname(this IGuildUser usr)
        {
            if (usr.Nickname == null)
                return usr.FullName();
            else
                return $"{usr.Username} ({usr.Nickname})#{usr.Discriminator}";
        }
        
        public static Color FromHex(this string hex)
        {
            var col = System.Drawing.ColorTranslator.FromHtml(hex);
            return new Color(col.R, col.G, col.B);
        }

        public static string ToHex(this Color color)
            => System.Drawing.ColorTranslator.ToHtml(System.Drawing.Color.FromArgb(255, color.R, color.G, color.B));

        public static async Task<IList<IGuildUser>> GetUsersWithRoleAsync(this IGuild guild, IRole role)
        {
            List<IGuildUser> usersWithRole = new List<IGuildUser>();

            await guild.DownloadUsersAsync().ConfigureAwait(false);
            var users = await guild.GetUsersAsync().ConfigureAwait(false);

            foreach (var user in users)
            {
                if(user.RoleIds.Contains(role.Id))
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

        public static string ToEmoji(this ClientType client)
         => client switch
         {
             ClientType.Desktop => "🖥️",
             ClientType.Mobile => "📱",
             ClientType.Web => "🔗",

             _ => "🖥️",
         };

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
    }
}