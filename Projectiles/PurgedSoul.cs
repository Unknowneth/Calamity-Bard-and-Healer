using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod;
using ThoriumMod.Utilities;

namespace CalamityBardHealer.Projectiles
{
	public class PurgedSoul : ModProjectile
	{
		public override string Texture => "CalamityMod/Projectiles/Rogue/LostSoulFriendly";
		public override void SetStaticDefaults() {
			Main.projFrames[Type] = 4;
			ProjectileID.Sets.TrailCacheLength[Type] = 8;
			ProjectileID.Sets.TrailingMode[Type] = 2;
			if(!ModLoader.TryGetMod("Redemption", out Mod mor)) return;
			mor.Call("addElementProj", 9, base.Projectile.type);
			mor.Call("addElementProj", 12, base.Projectile.type);
		}
		public override void SetDefaults() {
			Projectile.width = 12;
			Projectile.height = 12;
			Projectile.aiStyle = -1;
			Projectile.hostile = false;
			Projectile.friendly = true;
			Projectile.DamageType = HealerDamage.Instance;
			Projectile.ArmorPenetration = 150;
			Projectile.tileCollide = false;
			Projectile.extraUpdates = 2;
			Projectile.timeLeft = 300;
		}
		public override void AI() {
			if(++Projectile.frameCounter >= 12) Projectile.frameCounter = 0;
			Projectile.frame = (int)(Projectile.frameCounter / 3f);
			if(Projectile.ai[1] < 0f) {
				Projectile.Kill();
				return;
			}
			else if(Projectile.ai[2] > 0f) {
				if(Projectile.timeLeft > 295 && Projectile.ai[1] == 0f && Main.myPlayer == Projectile.owner) {
					float maxRange = 3200f;
					for(int i = 0; i < Main.maxPlayers; i++) if(i != Projectile.owner && !Main.player[i].dead && Main.player[i].active && Collision.CanHitLine(Main.MouseWorld, 0, 0, Main.player[i].Center, 0, 0) && Main.player[i].Distance(Main.MouseWorld) < maxRange && Main.player[i].statLife < Main.player[i].statLifeMax2 && Main.player[i].team == Main.player[Projectile.owner].team) {
						maxRange = Main.player[i].Distance(Main.MouseWorld);
						Projectile.ai[1] = i + 1;
					}
					if(Projectile.ai[1] > 0f) NetMessage.SendData(27, -1, -1, null, Projectile.whoAmI);
				}
				else if(Projectile.ai[1] > 0f) if(Main.player[(int)Projectile.ai[1] - 1].dead || !Main.player[(int)Projectile.ai[1] - 1].active) Projectile.ai[0] = Projectile.ai[1] = 0f;
				else if(Projectile.Hitbox.Intersects(Main.player[(int)Projectile.ai[1] - 1].Hitbox)) {
					if(HealerHelper.HealPlayer(Main.player[Projectile.owner], Main.player[(int)Projectile.ai[1] - 1], 9, 120) && Main.myPlayer == Projectile.owner) {
						Projectile.ai[1] = -1f;
						Projectile.Kill();
						Projectile.netUpdate = true;
					}
				}
				else Projectile.velocity = Vector2.Normalize(Vector2.Lerp(Vector2.Normalize(Projectile.velocity), Vector2.Normalize(Main.player[(int)Projectile.ai[1] - 1].Center - Projectile.Center), MathHelper.Min(++Projectile.ai[0] / 60f, 1f) * 0.75f)) * Projectile.velocity.Length();
			}
			else {
				if(Projectile.timeLeft > 295 && Projectile.ai[1] == 0f && Main.myPlayer == Projectile.owner) {
					float maxRange = 3200f;
					for(int i = 0; i < Main.maxNPCs; i++) if(Main.npc[i].CanBeChasedBy(this, false) && Collision.CanHitLine(Main.MouseWorld, 0, 0, Main.npc[i].Center, 0, 0) && Main.npc[i].Distance(Main.MouseWorld) < maxRange) {
						maxRange = Main.npc[i].Distance(Main.MouseWorld);
						Projectile.ai[1] = i + 1;
					}
					if(Projectile.ai[1] > 0f) NetMessage.SendData(27, -1, -1, null, Projectile.whoAmI);
				}
				else if(Projectile.ai[1] > 0f) if(!Main.npc[(int)Projectile.ai[1] - 1].CanBeChasedBy(this, false)) Projectile.ai[0] = Projectile.ai[1] = 0f;
				else Projectile.velocity = Vector2.Normalize(Vector2.Lerp(Vector2.Normalize(Projectile.velocity), Vector2.Normalize(Main.npc[(int)Projectile.ai[1] - 1].Center - Projectile.Center), MathHelper.Min(++Projectile.ai[0] / 60f, 1f) * 0.75f)) * Projectile.velocity.Length();
			}
			if(Projectile.ai[0] == 0f) {
				if(Projectile.localAI[0] == 0f) {
					int assignedRng = Projectile.whoAmI;
					while(assignedRng > 18) assignedRng -= 18;
					assignedRng -= 9;
					if(assignedRng > 0) Projectile.localAI[0] = -1f;
					else if(assignedRng < 0) Projectile.localAI[0] = 1f;
					else Projectile.localAI[0] = Main.player[Projectile.owner].direction;
					Projectile.velocity = Projectile.velocity.RotatedBy(MathHelper.PiOver4 * 0.5f * -Projectile.localAI[0]);
				}
				else Projectile.localAI[1] += Projectile.localAI[0];
				if(Projectile.localAI[1] < -9f) {
					Projectile.localAI[0] = -Projectile.localAI[0];
					Projectile.localAI[1] = -9f;
				}
				else if(base.Projectile.localAI[1] > 9f) {
					Projectile.localAI[0] = -Projectile.localAI[0];
					Projectile.localAI[1] = 9f;
				}
				Projectile.velocity = Projectile.velocity.RotatedBy(MathHelper.ToRadians(Projectile.localAI[1]) * 0.5f);
			}
			Projectile.rotation = Projectile.velocity.ToRotation();
		}
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
			if(Projectile.ai[2] > 0f) {
				int healAmount = (int)(ModContent.GetInstance<BalanceConfig>().healing * 3f);
				Main.player[Projectile.owner].HealLife(healAmount, Main.player[Projectile.owner]);
				Main.player[Projectile.owner].GetThoriumPlayer().mostRecentHeal = healAmount;
				Main.player[Projectile.owner].GetThoriumPlayer().mostRecentHealer = Projectile.owner;
			}
			if(ModLoader.TryGetMod("CalamityMod", out Mod calamity)) target.AddBuff(calamity.Find<ModBuff>("WhisperingDeath").Type, 180);
		}
		public override bool PreDraw(ref Color lightColor) {
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
			float fade = (float)MathHelper.Min(Projectile.timeLeft, 15) / 15f;
			lightColor = Projectile.ai[2] == 0f ? new Color(255, 55, 55, 255) : new Color(200, 200, 255, 255);
			Vector2 origin = new Vector2(texture.Width, texture.Height / Main.projFrames[Type]) * 0.5f;
			for(int k = 1; k < Projectile.oldPos.Length; k++) {
				int frames = Projectile.frame - k;
				while(frames < 0) frames += Main.projFrames[Projectile.type] + 1;
				Main.EntitySpriteDraw(texture, Projectile.oldPos[k] + new Vector2(Projectile.width, Projectile.height) * 0.5f - Main.screenPosition, new Rectangle(0, texture.Height / Main.projFrames[Projectile.type] * frames, texture.Width, texture.Height / Main.projFrames[Projectile.type]), Color.Lerp(lightColor, new Color(55, 55, 55, 0), MathHelper.Lerp(1f, 0f, (float)k / (float)Projectile.oldPos.Length) * fade) * MathHelper.Lerp(1f, 0f, (float)k / (float)Projectile.oldPos.Length) * fade, Projectile.oldRot[k], origin, Projectile.scale * MathHelper.Lerp(1.25f, 0.75f, (float)k / (float)Projectile.oldPos.Length) * fade, Projectile.oldRot[k] >= -MathHelper.PiOver2 && Projectile.oldRot[k] < MathHelper.PiOver2 ? SpriteEffects.FlipVertically : SpriteEffects.None, 0);
			}
			SpriteEffects spriteEffects = Projectile.rotation >= -MathHelper.PiOver2 && Projectile.rotation < MathHelper.PiOver2 ? SpriteEffects.FlipVertically : SpriteEffects.None;
			Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, new Rectangle(0, texture.Height / Main.projFrames[Projectile.type] * Projectile.frame, texture.Width, texture.Height / Main.projFrames[Projectile.type]), lightColor * fade, Projectile.rotation, origin, Projectile.scale * fade, spriteEffects, 0);
			Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, new Rectangle(0, texture.Height / Main.projFrames[Projectile.type] * Projectile.frame, texture.Width, texture.Height / Main.projFrames[Projectile.type]), (Projectile.ai[2] == 0f ? new Color(100, 0, 0, 0) : new Color(100, 100, 100, 0)) * fade, Projectile.rotation, origin, Projectile.scale * fade + Vector2.UnitX.RotatedBy(Main.GlobalTimeWrappedHourly * MathHelper.Pi).Y * 0.1f, spriteEffects, 0);
			return false;
		}
	}
}