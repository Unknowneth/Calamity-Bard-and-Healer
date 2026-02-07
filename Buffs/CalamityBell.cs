using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace CalamityBardHealer.Buffs
{
	public class CalamityBell : ModBuff
	{
		public override void SetStaticDefaults() {
			Main.buffNoTimeDisplay[Type] = true;
			Main.buffNoSave[Type] = true;
		}
		public override void Update(Player player, ref int buffIndex) {
			if(player.ownedProjectileCounts[ModContent.ProjectileType<Projectiles.CalamityBell>()] == 0 && Main.myPlayer == player.whoAmI) {
				int z = Projectile.NewProjectile(player.GetSource_Misc("God Slayer Deathsinger's Cowl"), player.MountedCenter, Vector2.Zero, ModContent.ProjectileType<Projectiles.CalamityBell>(), 0, 0, player.whoAmI);
				NetMessage.SendData(27, -1, -1, null, z);
			}
		}
	}
}