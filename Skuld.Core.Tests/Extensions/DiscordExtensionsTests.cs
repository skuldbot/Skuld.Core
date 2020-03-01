using Discord;
using Skuld.Core.Extensions;
using Skuld.Core.Utilities;
using System;
using Xunit;

namespace Skuld.Core.Tests
{
    public class DiscordExtensionsTests
    {
        [Fact]
        public void Conversions()
        {
            Assert.Equal(new Color(0, 0, 0), "#000000".FromHex());
            Assert.Equal("#000000", new Color(0, 0, 0).ToHex());

            Assert.Equal("🖥️", ClientType.Desktop.ToEmoji());
        }

        [Fact]
        public void Information()
        {
            Assert.Equal("#43b581", UserStatus.Online.HexFromStatus());
            Assert.Equal(DiscordUtilities.Online_Emote.ToString(), UserStatus.Online.StatusToEmote());
        }

        [Fact]
        public void Formatting()
        {
            Assert.Equal("", "<>".TrimEmbedHiders());
            Assert.Equal("", "<@0>".PruneMention(0));
            Assert.Equal("", "<@!0>".PruneMention(0));
            Assert.Equal("<@1>", "<@1>".PruneMention(0));
            Assert.Equal("<@!1>", "<@!1>".PruneMention(0));
        }
    }
}
