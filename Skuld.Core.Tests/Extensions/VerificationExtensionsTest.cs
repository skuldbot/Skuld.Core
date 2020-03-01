using Skuld.Core.Extensions.Verification;
using Xunit;

namespace Skuld.Core.Tests
{
    public class VerificationExtensionsTest
    {
        [Fact]
        public void ImageExtension()
        {
            Assert.True(".jpg".IsImageExtension());
            Assert.False(".webm".IsImageExtension());
        }
        [Fact]
        public void VideoFile()
        {
            Assert.True(".webm".IsVideoFile());
            Assert.False(".jpg".IsVideoFile());
        }
        [Fact]
        public void Website()
        {
            Assert.True("https://example.com".IsWebsite());
            Assert.False("example".IsWebsite());
        }
        [Fact]
        public void IsRecurring()
        {
            Assert.True(1111UL.IsRecurring(4));
            Assert.False(1234UL.IsRecurring(4));
            Assert.False(1111UL.IsRecurring(5));
        }
        [Fact]
        public void IsBitSet()
        {
            Assert.False(0UL.IsBitSet(1 << 0));
            Assert.True(1UL.IsBitSet(1 << 0));
        }
        [Fact]
        public void SameUppered()
        {
            Assert.True("example".IsSameUpperedInvariant("example"));
            Assert.True("example".IsSameUpperedInvariant("eXaMpLe"));
            Assert.False("example".IsSameUpperedInvariant("example2"));
            Assert.False("example".IsSameUpperedInvariant("1234"));
        }
        [Fact]
        public void ContainsUppered()
        {
            Assert.True("example".ContainsUpperedInvariant("example"));
            Assert.False("example".ContainsUpperedInvariant("this is an example"));
            Assert.True("this is an example".ContainsUpperedInvariant("example"));
            Assert.True("example".ContainsUpperedInvariant("eXaMpLe"));
            Assert.True("example2".ContainsUpperedInvariant("example"));
            Assert.True("eXaMpLe2".ContainsUpperedInvariant("example"));
            Assert.False("example".ContainsUpperedInvariant("example2"));
            Assert.False("example".ContainsUpperedInvariant("eXaMpLe2"));
            Assert.False("example".ContainsUpperedInvariant("1234"));
        }
    }
}
