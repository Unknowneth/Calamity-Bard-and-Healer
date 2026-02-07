using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using ThoriumMod.Projectiles.Scythe;

namespace CalamityBardHealer.Projectiles
{
	public class HyphaeBaton : ScythePro
	{
		public override void SafeSetDefaults() {
			base.Projectile.Size = new Vector2(102f);
			this.dustType = ModContent.DustType<ThoriumMod.Dusts.MushroomDust>();
			this.dustOffset = new Vector2(-8f, 8f);
		}
		public override void SafeOnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
			if(Main.myPlayer == base.Projectile.owner && Main.player[base.Projectile.owner].GetModPlayer<ThoriumMod.ThoriumPlayer>().soulEssence > 4) for(int i = 0; i < 16; i++) {
				Vector2 spawnPos = Main.rand.NextVector2Circular(102f, 102f);
				int p = Projectile.NewProjectile(Projectile.GetSource_OnHit(target), Projectile.Center + spawnPos, spawnPos * 0.1f, ModContent.ProjectileType<Projectiles.HyphaeBatonSporeCloud>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
				NetMessage.SendData(27, -1, -1, null, p);
			}
		}
		public override void ModifyDust(Dust dust, Vector2 position, int scytheIndex) => dust.rotation = (Projectile.Center - dust.position).ToRotation() - MathHelper.PiOver2;
	}
}