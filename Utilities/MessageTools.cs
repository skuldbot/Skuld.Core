using Discord.WebSocket;
using System;
using System.Threading.Tasks;
using DiscordNet = Discord;

namespace Skuld.Core.Utilities
{
    public static class MessageTools
    {
        private static readonly string Key = "MsgTools";
        public static string ModAdminBypass = "-!{MA_commands}";
        public static string NoOneCommands = "-!commands";

        public static async Task<bool> CheckEmbedPermission(DiscordShardedClient client, DiscordNet.IChannel channel)
        {
            if (channel is DiscordNet.IDMChannel)
            {
                return true;
            }

            var gchan = channel as DiscordNet.ITextChannel;
            var gusr = await gchan.GetUserAsync(client.CurrentUser.Id).ConfigureAwait(false);
            return gusr.GetPermissions(gchan).EmbedLinks;
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

        public static bool IsEnabledChannel(DiscordNet.IGuildUser user, DiscordNet.ITextChannel channel)
        {
            if (channel == null) return true;
            if (channel.Topic == null) return true;
            if (channel.Topic.Contains(ModAdminBypass))
            {
                if (user.GuildPermissions.Administrator) return true;
                else if (user.GuildPermissions.RawValue == DiscordUtilities.ModeratorPermissions.RawValue) return true;
                else return false;
            }
            if (channel.Topic.Contains(NoOneCommands)) return false;
            return true;
        }

        public static bool HasPrefix(DiscordNet.IUserMessage message, params string[] prefixes)
            => !string.IsNullOrEmpty(GetPrefixFromCommand(message.Content, prefixes));

        public static async Task<DiscordNet.IUserMessage> SendChannelAsync(this DiscordShardedClient client, DiscordNet.IChannel channel, string message)
        {
            try
            {
                var textChan = (DiscordNet.ITextChannel)channel;
                var mesgChan = (DiscordNet.IMessageChannel)channel;
                if (channel == null || textChan == null || mesgChan == null) { return null; }
                await mesgChan.TriggerTypingAsync();
                Log.Info(Key, $"Dispatched message to {(channel as DiscordNet.IGuildChannel).Guild} in {(channel as DiscordNet.IGuildChannel).Name}");
                return await mesgChan.SendMessageAsync(message);
            }
            catch (Exception ex)
            {
                Log.Error(Key, $"Error dispatching Message - {ex.Message}", ex);
                return null;
            }
        }
    }
}