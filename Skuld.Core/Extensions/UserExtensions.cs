using Discord;
using System.Linq;

namespace Skuld.Core.Extensions
{
	public static class UserExtensions
	{
		public static IRole GetHighestRole(this IGuildUser user)
		{
			var roles = user.RoleIds.Select(x => user.Guild.GetRole(x));

			return roles.OrderByDescending(x => x.Position).FirstOrDefault();
		}
	}
}