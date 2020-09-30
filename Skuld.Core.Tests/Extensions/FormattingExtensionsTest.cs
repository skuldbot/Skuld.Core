using Skuld.Core.Extensions.Formatting;
using System;
using Xunit;

namespace Skuld.Core.Tests
{
	public class FormattingExtensionsTest
	{
		[Fact]
		public void TestAll()
		{
			Assert.Equal("This is an example", "this is an example".CapitaliseFirstLetter());

			Assert.Equal("this is an example", "This is an example".LowercaseFirstLetter());

			Assert.Equal("1 day ago", DateTimeOffset.Now.AddDays(-1).GetStringFromOffset(DateTime.Now));
			Assert.Equal("2 days ago", DateTimeOffset.Now.AddDays(-2).GetStringFromOffset(DateTime.Now));
			Assert.Equal("today", DateTimeOffset.Now.GetStringFromOffset(DateTime.Now));

			Assert.Equal("hello", "<html><body><p>hello</p></body></html>".StripHtml());
		}
	}
}
