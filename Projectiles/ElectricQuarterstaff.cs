using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using ThoriumMod.Projectiles.Scythe;

namespace CalamityBardHealer.Projectiles
{
	public class ElectricQuarterstaff : ScythePro
	{
		public override void SafeSetStaticDefaults() {
			if(!ModLoader.TryGetMod("Redemption", out Mod mor)) return;
			mor.Call("addElementProj", 7, base.Projectile.type);
		}
		public override void SafeSetDefaults() {
			base.Projectile.Size = new Vector2(152f);
			this.dustCount = 1;
			this.dustType = 226;
			this.dustOffset = new Vector2(-9f, 9f);
		}
		public override void ModifyDust(Dust dust, Vector2 position, int scytheIndex) {
			dust.scale = Main.rand.NextFloat(0.6f, 0.9f);
			dust.velocity += Main.rand.NextVector2Circular(5f, 5f);
			dust.noGravity = !Main.rand.NextBool(5);
		}
		public override void OnFirstHit(NPC target, NPC.HitInfo hit, int damageDone) {
			if(Main.myPlayer == base.Projectile.owner) {
				float maxRange = 640f;
				Vector2 shootDir = Vector2.Normalize(base.Projectile.velocity);
				for(int i = 0; i < Main.maxNPCs; i++) if(i != target.whoAmI && Main.npc[i].CanBeChasedBy(this, false) && Collision.CanHitLine(base.Projectile.Center, 0, 0, Main.npc[i].Center, 0, 0) && Main.npc[i].Distance(base.Projectile.Center) < maxRange) {
					maxRange = Main.npc[i].Distance(base.Projectile.Center);
					shootDir = Vector2.Normalize(Main.npc[i].Center - target.Center);
				}
				int p = Projectile.NewProjectile(base.Projectile.GetSource_OnHit(target), target.Center, shootDir * 14f, ModContent.ProjectileType<Projectiles.ElectricChain>(), base.Projectile.damage, base.Projectile.knockBack, base.Projectile.owner, shootDir.ToRotation());
				NetMessage.SendData(27, -1, -1, null, p);
			}
		}
	}
}