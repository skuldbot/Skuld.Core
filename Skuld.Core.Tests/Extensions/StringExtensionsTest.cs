using Discord;
using Discord.WebSocket;
using Moq;
using Skuld.Core.Extensions;
using Xunit;

namespace Skuld.Core.Tests
{
    public class StringExtensionsTest
    {
        [Fact]
        public void TestAll()
        {
            Assert.Equal("We're, They're", "They're, They're".ReplaceFirst("They're", "We're"));
            Assert.Equal("They're, We're", "They're, They're".ReplaceLast("They're", "We're"));

            Assert.Equal("1,234", 1234UL.ToFormattedString());
            Assert.Equal("1,234", 1234L.ToFormattedString());
            Assert.Equal("1,234", 1234.ToFormattedString());
            Assert.Equal("1,234", 1234U.ToFormattedString());

            var globalmock = new Mock<IUser>(MockBehavior.Strict);

            globalmock.Setup(x => x.Username)
                .Returns("Clyde");

            globalmock.Setup(x => x.Discriminator)
                .Returns("0001");

            globalmock.Setup(x => x.DiscriminatorValue)
                .Returns(0001);

            Assert.Equal("Clyde#0001", globalmock.Object.FullName());

            var guildmock = new Mock<IGuildUser>(MockBehavior.Strict);

            guildmock.Setup(x => x.Username)
                .Returns("Clyde");

            guildmock.Setup(x => x.Nickname)
                .Returns("Wumpus");

            guildmock.Setup(x => x.Discriminator)
                .Returns("0001");

            guildmock.Setup(x => x.DiscriminatorValue)
                .Returns(0001);

            Assert.Equal("Clyde#0001", guildmock.Object.FullName());
            Assert.Equal("Clyde#0001 (Wumpus)", guildmock.Object.FullNameWithNickname());
        }
    }
}
