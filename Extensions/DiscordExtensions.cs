using Discord;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Skuld.Core.Extensions
{
    public static class DiscordExtensions
    {
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
    }
}