using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityBardHealer.Projectiles
{
	public class HydrogenSulfideFlames : ModProjectile
	{
		public override string Texture => "Terraria/Images/Extra_98";
		public override void SetStaticDefaults() {
			ProjectileID.Sets.TrailCacheLength[Type] = 20;
			ProjectileID.Sets.TrailingMode[Type] = 2;
			if(!ModLoader.TryGetMod("Redemption", out Mod mor)) return;
			mor.Call("addElementProj", 2, base.Projectile.type);
			mor.Call("addElementProj", 5, base.Projectile.type);
		}
		public override void SetDefaults() {
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.aiStyle = -1;
			Projectile.hostile = false;
			Projectile.friendly = true;
			Projectile.DamageType = ModLoader.GetMod("ThoriumMod").Find<DamageClass>("HealerDamage");
			Projectile.ArmorPenetration = 50;
			Projectile.penetrate = -1;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = -1;
			Projectile.extraUpdates = 1;
			Projectile.tileCollide = false;
			Projectile.timeLeft = 180;
		}
		public override void AI() {
			if(Collision.SolidCollision(Projectile.position, Projectile.width, Projectile.height) || Projectile.ai[0] < 0f) {
				if(Projectile.timeLeft > 15) Projectile.timeLeft = 15;
				Projectile.ai[0] = 0f;
				Projectile.ai[1] = 0f;
			}
			else if(Projectile.ai[1] == 0f && Main.myPlayer == Projectile.owner) {
				float maxRange = 640f;
				for(int i = 0; i < Main.maxNPCs; i++) if(Main.npc[i].CanBeChasedBy(this, false) && Collision.CanHitLine(Main.MouseWorld, 0, 0, Main.npc[i].Center, 0, 0) && Main.npc[i].Distance(Main.MouseWorld) < maxRange) {
					maxRange = Main.npc[i].Distance(Main.MouseWorld);
					Projectile.ai[1] = i + 1;
				}
				if(Projectile.ai[1] > 0f) NetMessage.SendData(27, -1, -1, null, Projectile.whoAmI);
			}
			else if(Projectile.ai[1] > 0f) if(!Main.npc[(int)Projectile.ai[1] - 1].CanBeChasedBy(this, false)) Projectile.ai[0] = Projectile.ai[1] = 0f;
			else Projectile.velocity = Vector2.Normalize(Vector2.Lerp(Vector2.Normalize(Projectile.velocity), Vector2.Normalize(Main.npc[(int)Projectile.ai[1] - 1].Center - Projectile.Center), MathHelper.Min(++Projectile.ai[0] / 120f, 1f) * 0.25f)) * Projectile.velocity.Length();
			Projectile.rotation = Projectile.velocity.ToRotation();
		}
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
			if(Main.myPlayer == Projectile.owner && Projectile.ai[0] > 0f) {
				Projectile.ai[0] = -1f;
				NetMessage.SendData(27, -1, -1, null, Projectile.whoAmI);
			}
			if(ModLoader.TryGetMod("CalamityMod", out Mod calamity)) target.AddBuff(calamity.Find<ModBuff>("CrushDepth").Type, 240);
			target.AddBuff(BuffID.OnFire3, 240);
		}
		public override bool PreDraw(ref Color lightColor) {
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
			for(int i = 0; i < Projectile.oldPos.Length; i++) {
				float colorLerp = (float)i / (float)Projectile.oldPos.Length;
				colorLerp *= colorLerp;
				lightColor = Color.Lerp(Color.OrangeRed, Color.Blue, colorLerp) * MathHelper.Lerp(0.8f * (float)MathHelper.Min(15, Projectile.timeLeft) / 15f, 0f, colorLerp);
				lightColor.A = 0;
				Main.EntitySpriteDraw(texture, Projectile.oldPos[i] + Projectile.Size * 0.5f - Main.screenPosition, null, lightColor, Projectile.oldRot[i] + MathHelper.PiOver2, texture.Size() / 2, Projectile.scale, SpriteEffects.None, 0);
			}
			return false;
		}
	}
}