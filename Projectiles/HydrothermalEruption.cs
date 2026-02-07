using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityBardHealer.Projectiles
{
	public class HydrothermalEruption : ModProjectile
	{
		public override string Texture => "Terraria/Images/Projectile_85";
		public override void SetStaticDefaults() {
			Main.projFrames[Type] = 7;
			ProjectileID.Sets.TrailCacheLength[Type] = 30;
			ProjectileID.Sets.TrailingMode[Type] = 0;
			if(!ModLoader.TryGetMod("Redemption", out Mod mor)) return;
			mor.Call("addElementProj", 2, base.Projectile.type);
			mor.Call("addElementProj", 5, base.Projectile.type);
		}
		public override void SetDefaults() {
			Projectile.width = 90;
			Projectile.height = 90;
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.DamageType = ModLoader.GetMod("ThoriumMod").Find<DamageClass>("BardDamage");
			Projectile.aiStyle = -1;
			Projectile.extraUpdates = 7;
			Projectile.timeLeft = 90;
			Projectile.tileCollide = true;
			Projectile.penetrate = -1;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = -1;
		}
		public override bool PreAI() {
			if(Projectile.ai[0] < 0f) return true;
			Projectile.extraUpdates = 0;
			if(Projectile.localAI[0] == 0) {
				for(int i = 0; i < Main.maxProjectiles; i++) if(Main.projectile[i].active && Main.projectile[i].type == Type && Main.projectile[i].timeLeft < Projectile.timeLeft && Main.projectile[i].ai[0] > -1f) {
					Projectile.Kill();
					return false;
				}
				Projectile.localAI[0]++;
			}
			Projectile.Center = Main.npc[(int)Projectile.ai[0]].Center;
			if(Projectile.timeLeft % 2 == 0 && Main.myPlayer == Projectile.owner) {
				int p = Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Projectile.velocity * 3f, Type, Projectile.damage, 0f, Projectile.owner, -1f);
				NetMessage.SendData(27, -1, -1, null, p);
			}
			return false;
		}
		public override void AI() {
			if(Projectile.localAI[0] == 0) {
				Projectile.localAI[0]++;
				Projectile.rotation = Main.rand.NextFloat(MathHelper.TwoPi);
			}
			else {
				Color color = Color.White;
				if(Projectile.timeLeft < 30) color = Color.Lerp(Color.Black, Color.DarkOrange, (float)Projectile.timeLeft / 30f);
				else  color = Color.Lerp(Color.DarkOrange, Color.DarkBlue, (float)System.Math.Sqrt((float)(Projectile.timeLeft - 30) / 60f));
				Lighting.AddLight(Projectile.Center, color.ToVector3());
			}
			if(Projectile.ai[1] < Main.projFrames[Projectile.type]) if(++Projectile.frameCounter >= (Projectile.ai[1] > 3 ? 10 : 30)) {
				Projectile.ai[1]++;
				Projectile.frameCounter = 0;
			}
			Projectile.frame = (int)Projectile.ai[1];
		}
		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac) {
			fallThrough = true;
			float hitboxSize = 15;
			switch(Projectile.ai[1]) {
				case 1:
					hitboxSize += 9;
				break;
				case 2:
					hitboxSize += 14;
				break;
				case 3:
					hitboxSize += 23;
				break;
				case 4:
					hitboxSize += 26;
				break;
				case 5:
					hitboxSize += 28;
				break;
				case 6:
					hitboxSize += 30;
				break;
			}
			hitboxSize *= Projectile.scale;
			return Collision.SolidCollision(Projectile.Center - new Vector2(hitboxSize), (int)(hitboxSize * 2f), (int)(hitboxSize * 2f));
		}
		public override bool CanHitPlayer(Player target) {
			float hitboxSize = 15;
			switch(Projectile.ai[1]) {
				case 1:
					hitboxSize += 9;
				break;
				case 2:
					hitboxSize += 14;
				break;
				case 3:
					hitboxSize += 23;
				break;
				case 4:
					hitboxSize += 26;
				break;
				case 5:
					hitboxSize += 28;
				break;
				case 6:
					hitboxSize += 30;
				break;
			}
			hitboxSize *= Projectile.scale;
			return Projectile.Distance(target.Center) < hitboxSize;
		}
		public override bool PreDraw(ref Color lightColor) {
			if(Projectile.ai[0] >= 0f) return false;
			if(Projectile.timeLeft < 30) lightColor = Color.Lerp(Color.Black, Color.DarkOrange, (float)Projectile.timeLeft / 30f);
			else lightColor = Color.Lerp(Color.DarkOrange, Color.DarkBlue, (float)System.Math.Sqrt((float)(Projectile.timeLeft - 30) / 60f));
			lightColor.A = 25;
			lightColor *= 0.4f;
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
			for(int k = 0; k < Projectile.oldPos.Length; k++) Main.EntitySpriteDraw(texture, Projectile.oldPos[k] + Projectile.Size * 0.5f - Main.screenPosition, new Rectangle(0, texture.Height / Main.projFrames[Projectile.type] * (int)MathHelper.Max(Projectile.frame - k / Main.projFrames[Projectile.type] / 2, 0), texture.Width, texture.Height / Main.projFrames[Projectile.type]), lightColor, -MathHelper.TwoPi * Main.GlobalTimeWrappedHourly + Projectile.rotation + k * MathHelper.PiOver4 / 3f, new Vector2(texture.Width, texture.Height / Main.projFrames[Projectile.type]) / 2, Projectile.scale, SpriteEffects.None, 0);
			return false;
		}
		public override bool OnTileCollide(Vector2 oldVelocity) => false;
		public override bool? CanDamage() => Projectile.ai[0] < 0f ? null : false;
		public override bool ShouldUpdatePosition() => Projectile.ai[0] < 0f;
	}
}