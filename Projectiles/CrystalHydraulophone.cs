using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using ThoriumMod;
using ThoriumMod.Projectiles;
using ThoriumMod.Projectiles.Bard;

namespace CalamityBardHealer.Projectiles
{
	public class CrystalHydraulophone : BardProjectile
	{
		public override BardInstrumentType InstrumentType => BardInstrumentType.String;
		public override void SetStaticDefaults() {
			if(!ModLoader.TryGetMod("Redemption", out Mod mor)) return;
			mor.Call("addElementProj", 1, base.Projectile.type);
			mor.Call("addElementProj", 3, base.Projectile.type);
		}
		public override void SetBardDefaults() {
			base.Projectile.width = 14;
			base.Projectile.height = 14;
			base.Projectile.aiStyle = -1;
			base.Projectile.hostile = false;
			base.Projectile.friendly = true;
			base.Projectile.DamageType = BardDamage.Instance;
			base.Projectile.tileCollide = true;
			base.Projectile.timeLeft = 300;
			base.Projectile.extraUpdates = 1;
			base.Projectile.alpha = 255;
		}
		public override void AI() {
			Main.dust[Dust.NewDust(base.Projectile.position, base.Projectile.width, base.Projectile.height, 187, 0f, 0f, 0, Color.White, 1f)].noGravity = true;
			base.Projectile.rotation = base.Projectile.velocity.ToRotation();
			if(base.Projectile.ai[1] > 0f) if(Main.myPlayer == base.Projectile.owner && base.Projectile.ai[0] == 0f) {
				float maxRange = 64f;
				for(int i = 0; i < Main.maxNPCs; i++) if(Main.npc[i].CanBeChasedBy(this, false) && Collision.CanHitLine(Main.MouseWorld, 0, 0, Main.npc[i].Center, 0, 0) && Main.npc[i].Distance(Main.MouseWorld) < maxRange) {
					maxRange = Main.npc[i].Distance(Main.MouseWorld);
					base.Projectile.ai[0] = i + 1;
				}
				if(base.Projectile.ai[0] > 0f && Main.npc[(int)base.Projectile.ai[0] - 1].active) return;
				else if(base.Projectile.Distance(Main.MouseWorld) < 16f) {
					base.Projectile.ai[0] = 0f;
					base.Projectile.ai[1] = 0f;
					return;
				}
				else base.Projectile.velocity = Vector2.Normalize(Vector2.Lerp(Vector2.Normalize(Main.MouseWorld - base.Projectile.Center), Vector2.Normalize(base.Projectile.velocity), --base.Projectile.ai[1] / 120f + 0.75f)) * base.Projectile.velocity.Length();
				NetMessage.SendData(27, -1, -1, null, base.Projectile.whoAmI);
			}
			else if(base.Projectile.ai[0] > 0f && Main.npc[(int)base.Projectile.ai[0] - 1].active) base.Projectile.velocity = Vector2.Normalize(Vector2.Lerp(Vector2.Normalize(Main.npc[(int)base.Projectile.ai[0] - 1].Center - base.Projectile.Center), Vector2.Normalize(base.Projectile.velocity), --base.Projectile.ai[1] / 120f + 0.75f)) * base.Projectile.velocity.Length();
			if(base.Projectile.ai[1] <= 0f) base.Projectile.ai[0] = 0f;
		}
		public override void OnKill(int timeLeft) {
			for(int j = 0; j < 15; j++) {
				int d = Dust.NewDust(base.Projectile.position, base.Projectile.width, base.Projectile.height, 187, base.Projectile.oldVelocity.X, base.Projectile.oldVelocity.Y, 0, Color.White, 1.7f);
				Main.dust[d].noGravity = true;
				Main.dust[d].velocity *= 5f;
				d = Dust.NewDust(base.Projectile.position, base.Projectile.width, base.Projectile.height, 187, base.Projectile.oldVelocity.X, base.Projectile.oldVelocity.Y, 0, Color.White, 1f);
				Main.dust[d].velocity *= 2f;
			}
			Terraria.Audio.SoundEngine.PlaySound(Terraria.ID.SoundID.Item20, base.Projectile.Center, null);
		}
		public override bool OnTileCollide(Vector2 oldVelocity) {
			if(++base.Projectile.ai[2] < 1 + Main.player[base.Projectile.owner].GetModPlayer<ThoriumPlayer>().bardBounceBonus) {
				if(Main.myPlayer == Projectile.owner) {
					base.Projectile.ai[1] = 30f;
					NetMessage.SendData(27, -1, -1, null, base.Projectile.whoAmI);
				}
				if(base.Projectile.velocity.X != oldVelocity.X) base.Projectile.velocity.X = -oldVelocity.X;
				if(base.Projectile.velocity.Y != oldVelocity.Y) base.Projectile.velocity.Y = -oldVelocity.Y;
				return false;
			}
			return true;
		}
		public override bool PreDraw(ref Color lightColor) {
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
			Main.EntitySpriteDraw(texture, base.Projectile.Center - Main.screenPosition, null, lightColor, base.Projectile.rotation, texture.Size() / 2, base.Projectile.scale, SpriteEffects.None, 0);
			return false;
		}
	}
}