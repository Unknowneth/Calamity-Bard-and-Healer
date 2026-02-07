using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityBardHealer.Projectiles
{
	public class WilloftheRagnarok : ModProjectile
	{
		public override string Texture => "Terraria/Images/Extra_98";
		public override string GlowTexture => "CalamityBardHealer/Projectiles/Sparkle";
		public override void SetStaticDefaults() {
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 60;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
			if(!ModLoader.TryGetMod("Redemption", out Mod mor)) return;
			mor.Call("addElementProj", 9, base.Projectile.type);
			mor.Call("addElementProj", 14, base.Projectile.type);
		}
		public override void SetDefaults() {
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.aiStyle = -1;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
			Projectile.DamageType = ModLoader.GetMod("ThoriumMod").Find<DamageClass>("HealerDamage");
			Projectile.extraUpdates = 2;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 30;
			Projectile.timeLeft = 60;
		}
		public override void AI() {
			Player player = Main.player[Projectile.owner];
			switch(Projectile.ai[2]) {
				case 0:
					if(Projectile.timeLeft < 15) Projectile.velocity = (Projectile.ai[0] - MathHelper.PiOver2 * Projectile.ai[1]).ToRotationVector2() * Projectile.velocity.Length();
					else if(Projectile.timeLeft < 25) Projectile.velocity = Projectile.ai[0].ToRotationVector2() * Projectile.velocity.Length();
					else {
						Projectile.velocity = Projectile.velocity.RotatedBy(MathHelper.ToRadians(Projectile.ai[1]) * 4f);
						if(Main.myPlayer == player.whoAmI && (Projectile.timeLeft - 3) % 6 == 0) {
							int p = Projectile.NewProjectile(player.GetSource_ItemUse(player.HeldItem), Projectile.Center, Projectile.velocity.RotatedBy(MathHelper.PiOver2 * -Projectile.ai[1]), Type, Projectile.damage, Projectile.knockBack, player.whoAmI, 0f, Projectile.ai[1], 1f);
							NetMessage.SendData(27, -1, -1, null, p);
						}
					}
				break;
				case 1:
					if(Projectile.timeLeft == 10 && Main.myPlayer == player.whoAmI) {
						int who = 0;
						float maxRange = 3200f;
						for(int i = 0; i < Main.maxNPCs; i++) if(Main.npc[i].CanBeChasedBy(this, false) && Collision.CanHitLine(Main.MouseWorld, 0, 0, Main.npc[i].Center, 0, 0) && Main.npc[i].Distance(Main.MouseWorld) < maxRange) {
							maxRange = Main.npc[i].Distance(Main.MouseWorld);
							who = i + 1;
						}
						Vector2 spawnPos = Vector2.Lerp(Projectile.oldPos[0], Projectile.oldPos[10], 0.5f) + Projectile.Size * 0.5f;
						int p = Projectile.NewProjectile(player.GetSource_ItemUse(player.HeldItem), spawnPos, Vector2.Normalize(Main.MouseWorld - spawnPos) * Projectile.velocity.Length(), Type, Projectile.damage, Projectile.knockBack, player.whoAmI, 0f, who, 2f);
						NetMessage.SendData(27, -1, -1, null, p);
					}
					if(Projectile.timeLeft < 30) Projectile.velocity = Projectile.velocity.RotatedBy(MathHelper.ToRadians(Projectile.ai[1]) * 18f);
					else Projectile.velocity = Projectile.velocity.RotatedBy(MathHelper.ToRadians(-Projectile.ai[1]));
				break;
				case 2:
					if(Projectile.localAI[0] == 0f && Projectile.localAI[1] == 0f) {
						Projectile.localAI[0] = Projectile.Center.X;
						Projectile.localAI[1] = Projectile.Center.Y;
						Projectile.localAI[2] = 30f;
						Projectile.timeLeft = 180;
					}
					if(Projectile.localAI[2] > 0f) Projectile.localAI[2]--;
					if(Projectile.ai[1] > 0f) Projectile.velocity = Vector2.Normalize(Vector2.Lerp(Vector2.Normalize(Projectile.velocity), Vector2.Normalize(Main.npc[(int)Projectile.ai[1] - 1].Center - Projectile.Center), MathHelper.Min(++Projectile.ai[0] / 60f, 1f) * 0.5f)) * Projectile.velocity.Length();
				break;
			}
			Projectile.rotation = Projectile.velocity.ToRotation();
		}
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
			if(ModLoader.TryGetMod("CalamityMod", out Mod calamity)) target.AddBuff(calamity.Find<ModBuff>("GodSlayerInferno").Type, 600);
		}
		public override bool PreDraw(ref Color lightColor) {
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
			float fade = MathHelper.Lerp(1f, 0f, (int)Projectile.alpha / 255f) * (MathHelper.Min(Projectile.timeLeft, 30) / 30);
			if(Projectile.velocity != Vector2.Zero) for(int i = 0; i < Projectile.oldPos.Length; i++) Main.EntitySpriteDraw(texture, Projectile.oldPos[i] + new Vector2(Projectile.width, Projectile.height) / 2 + Main.rand.NextVector2Circular(i, i) / (float)Projectile.oldPos.Length- Main.screenPosition, null, new Color(189, 48, 255, 0) * fade, Projectile.oldRot[i] + MathHelper.PiOver2, texture.Size() * 0.5f, new Vector2(MathHelper.Lerp(Projectile.scale * 0.65f, 0.05f, (float)i / (float)Projectile.oldPos.Length)), SpriteEffects.None, 0);
			texture = (Texture2D)ModContent.Request<Texture2D>(GlowTexture);
			Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, new Color(189, 48, 255, 0) * fade, Projectile.rotation, texture.Size() * 0.5f, Projectile.scale * fade, SpriteEffects.None, 0);
			Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, new Color(189, 48, 255, 0) * fade, Projectile.rotation + MathHelper.PiOver4, texture.Size() * 0.5f, Projectile.scale * fade * 0.5f, SpriteEffects.None, 0);
			if(Projectile.localAI[0] == 0f || Projectile.localAI[1] == 0f || Projectile.localAI[2] <= 0f) return false;
			fade = MathHelper.Lerp(1f, 0f, (int)Projectile.alpha / 255f) * (Projectile.localAI[2] / 30f);
			Main.EntitySpriteDraw(texture, new Vector2(Projectile.localAI[0], Projectile.localAI[1]) - Main.screenPosition, null, new Color(189, 48, 255, 0) * fade, Projectile.rotation, texture.Size() * 0.5f, Projectile.scale * fade, SpriteEffects.None, 0);
			Main.EntitySpriteDraw(texture, new Vector2(Projectile.localAI[0], Projectile.localAI[1]) - Main.screenPosition, null, new Color(189, 48, 255, 0) * fade, Projectile.rotation + MathHelper.PiOver4, texture.Size() * 0.5f, Projectile.scale * fade * 0.5f, SpriteEffects.None, 0);
			texture = (Texture2D)ModContent.Request<Texture2D>("Terraria/Images/Projectile_657");
			fade = Vector2.UnitX.RotatedBy(fade * MathHelper.Pi).Y;
			Main.EntitySpriteDraw(texture, new Vector2(Projectile.localAI[0], Projectile.localAI[1]) - Main.screenPosition, null, new Color(189, 48, 255, 0) * fade, Main.GlobalTimeWrappedHourly * MathHelper.Pi, texture.Size() * 0.5f, Projectile.scale * fade, SpriteEffects.None, 0);
			Main.EntitySpriteDraw(texture, new Vector2(Projectile.localAI[0], Projectile.localAI[1]) - Main.screenPosition, null, new Color(189, 48, 255, 0) * fade, Main.GlobalTimeWrappedHourly * -MathHelper.Pi, texture.Size() * 0.5f, Projectile.scale * fade * 0.5f, SpriteEffects.None, 0);
			return false;
		}
	}
}