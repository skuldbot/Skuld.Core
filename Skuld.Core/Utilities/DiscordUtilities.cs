using Discord;
using Skuld.Core.Extensions;
using System.Text.RegularExpressions;

namespace Skuld.Core.Utilities
{
    public static class DiscordUtilities
    {
        public const double PHI = 1.618;
        public const double LevelModifier = 2.5;
        public static GuildPermissions ModeratorPermissions 
            = new GuildPermissions(268443648);
        public static ChannelPermissions RequiredForStarboard 
            = new ChannelPermissions(19520);

        #region Response Emojis

        public static readonly string Ok_Emoji = "👌";
        public static readonly string Question_Emoji = "❓";
        public static readonly string Confused_Emoji = "⁉";
        public static readonly string Prohibit_Emoji = "🚫";
        public static readonly string Warning_Emoji = "⚠";
        public static readonly string Remind_Emoji = "🔔";
        public static readonly string NotNSFW_Emoji = "🔞";
        public static readonly string ATM_Emoji = "🏧";
        public static readonly string NoBotsString = "Bots are not supported.";

        #endregion Response Emojis

        #region Status Emotes

        public static readonly Emote Streaming_Emote = Emote.Parse("<:streaming:614849478926794752>");
        public static readonly Emote Online_Emote = Emote.Parse("<:online:614849479161544751>");
        public static readonly Emote Idle_Emote = Emote.Parse("<:away:614849478847102986>");
        public static readonly Emote DoNotDisturb_Emote = Emote.Parse("<:dnd:614849478482198528>");
        public static readonly Emote Offline_Emote = Emote.Parse("<:offline:614849478758760479>");

        #endregion Status Emotes

        #region Embed Colours

        public static readonly Color Ok_Color = "#339966".FromHex();
        public static readonly Color Warning_Color = "#FFFF00".FromHex();
        public static readonly Color Failed_Color = "#FF0000".FromHex();

        #endregion Embed Colours

        #region Twitch Emotes

        public static readonly Emote TwitchAdmins = Emote.Parse("<:TwitchAdmins:552666767609036825>");
        public static readonly Emote TwitchAffiliate = Emote.Parse("<:TwitchAffiliate:552666767630008354>");
        public static readonly Emote TwitchBroadcaster = Emote.Parse("<:TwitchBroadcaster:552666767647047680>");
        public static readonly Emote TwitchChatMod = Emote.Parse("<:TwitchChatMod:552666768406216714>");
        public static readonly Emote TwitchPrime = Emote.Parse("<:TwitchPrime:552666767122759722>");
        public static readonly Emote TwitchStaff = Emote.Parse("<:TwitchStaff:552666767412035584>");
        public static readonly Emote TwitchTurbo = Emote.Parse("<:TwitchTurbo:552666767609167873>");
        public static readonly Emote TwitchVerified = Emote.Parse("<:TwitchVerified:552666767625814086>");
        public static readonly Emote TwitchVIP = Emote.Parse("<:TwitchVIP:552666767416360971>");
        public static readonly Emote TwitchGlobalMod = Emote.Parse("<:TwitchGlobalMod:552668468877590538>");

        #endregion Twitch Emotes

        #region Nitro Emotes

        public static readonly Emote NitroBoostEmote = Emote.Parse("<:boost:614875223417684126>");
        public static readonly Emote NitroBoostRank1Emote = Emote.Parse("<:boostrank1:614875835123499212>");
        public static readonly Emote NitroBoostRank2Emote = Emote.Parse("<:boostrank2:614875835131887795>");
        public static readonly Emote NitroBoostRank3Emote = Emote.Parse("<:boostrank3:614875835102658560>");
        public static readonly Emote NitroBoostRank4Emote = Emote.Parse("<:boostrank4:614875835249197076>");

        #endregion Nitro Emotes

        #region Boost Icons

        public static string Level1ServerBoost = "server/level1.svg";
        public static string Level2ServerBoost = "server/level2.svg";
        public static string Level3ServerBoost = "server/level3.svg";

        public static string Level1UserBoost = "profile/level1.svg";
        public static string Level2UserBoost = "profile/level2.svg";
        public static string Level3UserBoost = "profile/level3.svg";

        #endregion Boost Icons

        #region User Flags

        public const ulong BotCreator = 1 << 0;
        public const ulong BotAdmin = 1 << 1;
        public const ulong BotDonator = 1 << 2;
        public const ulong BotTester = 1 << 3;
        public const ulong Banned = 1 << 62;
        public const ulong NormalUser = 0;

        #endregion User Flags

        #region Emotes

        public static readonly Emote Tick_Emote = Emote.Parse("<:tick:667153179175157760>");
        public static readonly Emote Cross_Emote = Emote.Parse("<:cross:667153179154186278>");
        public static Emote Empty_Emote = Emote.Parse("<:empty:663083920992239638>");

        #endregion Emotes

        #region Regex

        public static Regex UserMentionRegex = new Regex("<@.?[0-9]*?>");
        public static Regex RoleMentionRegex = new Regex("<&[0-9]*?>");
        public static Regex ChannelMentionRegex = new Regex("<#[0-9]*?>");

        #endregion Regex
    }
}