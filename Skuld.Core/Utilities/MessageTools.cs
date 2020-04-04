using Discord;
using Discord.WebSocket;
using System;
using System.Threading.Tasks;

namespace Skuld.Core.Utilities
{
    public static class MessageTools
    {
        public static string ModAdminBypass = "-!{MA_commands}";
        public static string NoOneCommands = "-!commands";

        public static async Task<bool> CheckEmbedPermission(DiscordShardedClient client, IChannel channel)
        {
            if (channel is IDMChannel)
            {
                return true;
            }

            if(channel is ITextChannel chan)
            {
                var gusr = await chan.GetUserAsync(client.CurrentUser.Id).ConfigureAwait(false);
                
                return gusr.GetPermissions(chan).EmbedLinks;
            }

            return true;
        }

        public static string GetPrefixFromCommand(string command, params string[] prefixes)
        {
            string result = null;

            foreach (var prefix in prefixes)
            {
                if (command.StartsWith(prefix))
                {
                    result = prefix;
                    break;
                }
            }

            return result;
        }

        public static string GetCommandName(string prefix, SocketMessage message)
        {
            string cmdname = message.Content;

            if (cmdname.StartsWith(prefix))
            {
                cmdname = cmdname.Remove(0, prefix.Length);
            }

            var content = cmdname.Split(' ');

            return content[0];
        }

        public static bool IsEnabledChannel(IGuildUser user, ITextChannel channel)
        {
            if (channel == null) return true;
            if (channel.Topic == null) return true;
            if (channel.Topic.ToUpperInvariant().Contains(ModAdminBypass.ToUpperInvariant(), StringComparison.InvariantCulture))
            {
                if (user.GuildPermissions.Administrator) return true;
                else if (user.GuildPermissions.RawValue == DiscordUtilities.ModeratorPermissions.RawValue) return true;
                else return false;
            }
            if (channel.Topic.ToUpperInvariant().Contains(NoOneCommands.ToUpperInvariant(), StringComparison.InvariantCulture)) return false;
            return true;
        }

        public static bool HasPrefix(IUserMessage message, params string[] prefixes)
            => !string.IsNullOrEmpty(GetPrefixFromCommand(message.Content, prefixes));
    }
}