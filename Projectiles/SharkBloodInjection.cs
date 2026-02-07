using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace CalamityBardHealer.Projectiles
{
	public class SharkBloodInjection : ModProjectile
	{
		public override bool IsLoadingEnabled(Mod mod) => ModLoader.HasMod("InfernumMode");
		public override void SetStaticDefaults() {
			if(ModLoader.TryGetMod("Redemption", out Mod mor)) {
				mor.Call("addElementProj", 3, Projectile.type);
				mor.Call("addElementProj", 5, Projectile.type);
			}
			Main.projFrames[Type] = 6;
		}
		public override void SetDefaults() {
			Projectile.width = 38;
			Projectile.height = 38;
			Projectile.aiStyle = -1;
			Projectile.friendly = true;
			Projectile.tileCollide = true;
			Projectile.DamageType = ThoriumMod.HealerDamage.Instance;
			Projectile.timeLeft = 300;
			Projectile.penetrate = -1;
			Projectile.extraUpdates = 1;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 10;
			Projectile.ArmorPenetration = 30;
		}
		public override void AI() {
			if(Projectile.ai[1] >= 30f) {
				if(Main.myPlayer == Projectile.owner && Projectile.ai[2] != 0f) {
					Projectile.ai[2] = 0f;
					Projectile.velocity = Vector2.Normalize(Projectile.velocity) * 12f;
					NetMessage.SendData(27, -1, -1, null, Projectile.whoAmI);
				}
				Projectile.frame = 5;
				Projectile.rotation += Projectile.velocity.X / 15f;
			}
			else Projectile.rotation = Projectile.velocity.ToRotation();
			if(Projectile.ai[2] == 0f) {
				if(++Projectile.ai[0] > 30f) {
					for(int i = 0; i < Main.maxPlayers; i++) if(Projectile.Hitbox.Intersects(Main.player[i].Hitbox) && Main.player[i].active && !Main.player[i].dead && Main.player[i].statLife < Main.player[i].statLifeMax2 && Main.player[i].team == Main.player[Projectile.owner].team && HealerHelper.HealPlayer(Main.player[Projectile.owner], Main.player[i], 16, 420) && Main.myPlayer == Projectile.owner) {
						Projectile.ai[2] = -i - 1;
						Projectile.Center = Main.player[i].Center;
						Projectile.velocity = Projectile.Center - Main.player[i].Center;
						NetMessage.SendData(27, -1, -1, null, Projectile.whoAmI);
						return;
					}
					Projectile.velocity.Y += 0.22f;
					Projectile.velocity *= 0.98f;
				}
				return;
			}
			Projectile.extraUpdates = 0;
			Vector2 attachTo = Projectile.ai[2] > 0f ? Main.npc[(int)Projectile.ai[2] - 1].Center : Main.player[(int)-(Projectile.ai[2] - 1)].Center;
			Projectile.rotation = (attachTo - Projectile.Center).ToRotation();
			Projectile.Center = attachTo;
			Projectile.timeLeft = 300;
			if(Projectile.ai[1] == 0f) SoundEngine.PlaySound(SoundID.Item17, Projectile.Center);
			Projectile.frame = (int)(++Projectile.ai[1] / 5f);
		}
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
			if(ModLoader.TryGetMod("CalamityMod", out Mod calamity)) target.AddBuff(calamity.Find<ModBuff>(Main.rand.NextBool() ? "CrushDepth" : "ArmorCrunch").Type, 300);
			if(Main.myPlayer != Projectile.owner || Projectile.ai[2] != 0f || Projectile.ai[1] >= 30f) return;
			Projectile.ai[2] = target.whoAmI + 1;
			Projectile.velocity = Projectile.Center - target.Center;
			if(Projectile.velocity.HasNaNs()) Projectile.velocity = Projectile.oldVelocity;
			Projectile.Center = target.Center;
			NetMessage.SendData(27, -1, -1, null, Projectile.whoAmI);
		}
		public override void OnKill(int timeLeft) {
			for(int i = 0; i < Main.rand.Next(7, 11); i++) {
				Dust dust = Dust.NewDustPerfect(Projectile.Center, 278, Main.rand.NextVector2Circular(8f, 8f), 0, Color.Aquamarine, Projectile.scale);
				dust.position += Vector2.Normalize(dust.velocity) * Main.rand.NextVector2Circular(Projectile.width, Projectile.height) * 0.5f; 
				dust.noGravity = true;
				dust.scale = Main.rand.NextFloat(0.6f, 0.9f) * Projectile.scale;
			}
			SoundEngine.PlaySound(SoundID.Shatter, Projectile.Center);
		}
		public override bool PreDraw(ref Color lightColor) {
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
			Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, new Rectangle(0, texture.Height / Main.projFrames[Type] * Projectile.frame, texture.Width, texture.Height / Main.projFrames[Type]), lightColor, Projectile.rotation, new Vector2(texture.Width, texture.Height / Main.projFrames[Type]) * 0.5f, Projectile.scale, Projectile.spriteDirection < 0 ? SpriteEffects.FlipVertically : SpriteEffects.None, 0);
			if(Projectile.frame >= 5) return false;
			texture = (Texture2D)ModContent.Request<Texture2D>(GlowTexture);
			Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, new Rectangle(0, texture.Height / Main.projFrames[Type] * Projectile.frame, texture.Width, texture.Height / Main.projFrames[Type]), new Color(250, 250, 250, 0), Projectile.rotation, new Vector2(texture.Width, texture.Height / Main.projFrames[Type]) * 0.5f, Projectile.scale, Projectile.spriteDirection < 0 ? SpriteEffects.FlipVertically : SpriteEffects.None, 0);
			return false;
		}
		public override bool? CanDamage() => Projectile.ai[1] >= 30f ? false : null;
	}
}