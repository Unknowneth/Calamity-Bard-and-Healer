using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.Audio;
using Terraria.ModLoader;
using ThoriumMod.Projectiles.Scythe;
using CalamityMod.Particles;
using System.IO;

namespace CalamityBardHealer.Projectiles
{
	public class FireHazard : ScythePro
	{
		public override string Texture => "CalamityBardHealer/Items/FireHazard";
		public override void SafeSetStaticDefaults() {
			Main.projFrames[base.Projectile.type] = 3;
			if(!ModLoader.TryGetMod("Redemption", out Mod mor)) return;
			mor.Call("addElementProj", 2, base.Projectile.type);
			mor.Call("addElementProj", 12, base.Projectile.type);
		}
		public override void SafeSetDefaults() {
			base.Projectile.width = 92;
			base.Projectile.height = 126;
			base.Projectile.idStaticNPCHitCooldown = 4;
			base.Projectile.ArmorPenetration = 15;
			base.Projectile.alpha = 255;
			base.Projectile.manualDirectionChange = true;
		}
		public override bool PreAI() {
			if(++base.Projectile.frameCounter > 3) {
				if(++base.Projectile.frame >= Main.projFrames[base.Projectile.type]) base.Projectile.frame = 0;
				base.Projectile.frameCounter = 0;
			}
			float swingTime = 0f;
			Player player = Main.player[base.Projectile.owner];
			base.Projectile.scale = player.GetAdjustedItemScale(player.HeldItem);
			if(base.Projectile.ai[1] <= 0 || player.dead) {
				base.Projectile.Kill();
				return false;
			}
			base.Projectile.timeLeft = (int)base.Projectile.ai[1];
			player.itemTime = (int)base.Projectile.ai[1];
			player.itemAnimation = (int)base.Projectile.ai[1];
			player.heldProj = base.Projectile.whoAmI;
			player.compositeFrontArm.enabled = true;
			if(base.Projectile.velocity.X != 0) player.ChangeDir(base.Projectile.velocity.X > 0 ? 1 : -1);
			float swing = 0f;
			float armSwing = 0f;
			if(base.Projectile.direction == -1) {
				if(base.Projectile.ai[1] / base.Projectile.ai[0] > 0.75f) {
					if(player.whoAmI == Main.myPlayer) {
						base.Projectile.velocity = Vector2.Normalize(Main.MouseWorld - player.MountedCenter);
						NetMessage.SendData(27, -1, -1, null, base.Projectile.whoAmI);
					}
					swingTime = (base.Projectile.ai[1] / base.Projectile.ai[0] - 0.75f) * 4f;
					swing = base.Projectile.velocity.ToRotation() + MathHelper.ToRadians(MathHelper.SmoothStep(135f, 80f, swingTime) * player.direction);
					armSwing = base.Projectile.velocity.ToRotation() + MathHelper.Lerp(MathHelper.PiOver2, MathHelper.PiOver4, swingTime) * player.direction;
				}
				else {
					swingTime = base.Projectile.ai[1] / (base.Projectile.ai[0] * 0.75f);
					for(int e = 0; e < 4; e++) swingTime = MathHelper.SmoothStep(0f, 1f, swingTime);
					swing = MathHelper.Lerp(MathHelper.ToRadians(100) * -player.direction, MathHelper.ToRadians(135) * player.direction, swingTime) + base.Projectile.velocity.ToRotation();
					armSwing = MathHelper.Lerp(MathHelper.PiOver2 * -player.direction, MathHelper.PiOver2 * player.direction, swingTime) + base.Projectile.velocity.ToRotation();
				}
				base.Projectile.spriteDirection = -player.direction;
			}
			else {
				if(base.Projectile.ai[1] / base.Projectile.ai[0] > 0.75f) {
					if(player.whoAmI == Main.myPlayer) {
						base.Projectile.velocity = Vector2.Normalize(Main.MouseWorld - player.MountedCenter);
						NetMessage.SendData(27, -1, -1, null, base.Projectile.whoAmI);
					}
					swingTime = (base.Projectile.ai[1] / base.Projectile.ai[0] - 0.75f) * 4f;
					swing = base.Projectile.velocity.ToRotation() - MathHelper.ToRadians(MathHelper.SmoothStep(135f, 80f, swingTime) * player.direction);
					armSwing = base.Projectile.velocity.ToRotation() - MathHelper.Lerp(MathHelper.PiOver2, MathHelper.PiOver4, swingTime) * player.direction;
				}
				else {
					swingTime = base.Projectile.ai[1] / (base.Projectile.ai[0] * 0.75f);
					for(int e = 0; e < 4; e++) swingTime = MathHelper.SmoothStep(0f, 1f, swingTime);
					swing = MathHelper.Lerp(MathHelper.ToRadians(100) * player.direction, MathHelper.ToRadians(135) * -player.direction, swingTime) + base.Projectile.velocity.ToRotation();
					armSwing = MathHelper.Lerp(MathHelper.PiOver2 * player.direction, MathHelper.PiOver2 * -player.direction, swingTime) + base.Projectile.velocity.ToRotation();
				}
				base.Projectile.spriteDirection = player.direction;
			}
			if(base.Projectile.localAI[2] == 0f && base.Projectile.ai[1] <= base.Projectile.ai[0] * 0.5f) {
				if(Main.myPlayer == player.whoAmI) {
					int p = Projectile.NewProjectile(player.GetSource_ItemUse(player.HeldItem), base.Projectile.Center + Vector2.Normalize(base.Projectile.velocity) * base.Projectile.height * 0.6f, Vector2.Normalize(base.Projectile.velocity) * 10f, ModContent.ProjectileType<Projectiles.FireStarter>(), base.Projectile.damage, base.Projectile.knockBack, player.whoAmI);
					NetMessage.SendData(27, -1, -1, null, p);
				}
				SoundEngine.PlaySound(SoundID.DD2_MonkStaffSwing, player.position);
				base.Projectile.localAI[2]++;
			}
			if(base.Projectile.alpha > 0) base.Projectile.alpha -= 17;
			player.compositeFrontArm.rotation = armSwing - MathHelper.PiOver2 - (player.gravDir - 1) * MathHelper.PiOver2;
			base.Projectile.Center = player.GetFrontHandPosition(player.compositeFrontArm.stretch, player.compositeFrontArm.rotation);
			base.Projectile.rotation = swing;
			if(base.Projectile.ai[1] > 0f) base.Projectile.ai[1]--;
			return false;
		}
		public override void SafeOnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
			for(int i = 0; i < 10; i++) {
				float rot = MathHelper.ToRadians(MathHelper.ToRadians(36f) * i);
				Vector2 velocity = Vector2.Normalize(target.Center - base.Projectile.Center).RotatedBy(MathHelper.PiOver4 * base.Projectile.spriteDirection) * base.Projectile.velocity.Length();
				Vector2 offset = velocity.RotatedBy((double)(rot * Main.rand.NextFloat(3.1f, 9.1f))) * new Vector2(Main.rand.NextFloat(1.5f, 5.5f));
				Vector2 velOffset = velocity.RotatedBy((double)(rot * Main.rand.NextFloat(3.1f, 9.1f))) * new Vector2(Main.rand.NextFloat(1.5f, 5.5f));
				GeneralParticleHandler.SpawnParticle(new MediumMistParticle(target.Center + offset, velOffset * Main.rand.NextFloat(1.5f, 3f), Color.Red, Color.DarkRed, Main.rand.NextFloat(0.9f, 1.2f), 160f, Main.rand.NextFloat(0.03f, -0.03f)));
			}
			if(ModLoader.TryGetMod("CalamityMod", out Mod calamity)) target.AddBuff(calamity.Find<ModBuff>("BrimstoneFlames").Type, 240);
		}
		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) {
			float point = 0f;
			return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), base.Projectile.Center, base.Projectile.Center + base.Projectile.rotation.ToRotationVector2() * base.Projectile.height * base.Projectile.scale, base.Projectile.width * base.Projectile.scale * 0.5f, ref point);
		}
		public override bool PreDraw(ref Color lightColor) {
			lightColor = Color.White;
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
			Main.EntitySpriteDraw(texture, base.Projectile.Center - Main.screenPosition, new Rectangle(0, texture.Height / Main.projFrames[base.Projectile.type] * base.Projectile.frame, texture.Width, texture.Height / Main.projFrames[base.Projectile.type]), lightColor, base.Projectile.rotation + MathHelper.PiOver2 - MathHelper.ToRadians(base.Projectile.spriteDirection * 15f), new Vector2(texture.Width / 2 - base.Projectile.spriteDirection * 12, texture.Height / Main.projFrames[base.Projectile.type] - 22), base.Projectile.scale, base.Projectile.spriteDirection < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
			if(base.Projectile.ai[1] / base.Projectile.ai[0] < 0.75f) {
				Player player = Main.player[base.Projectile.owner];
				float swingTime = base.Projectile.ai[1] / (base.Projectile.ai[0] * 0.75f);
				for(int e = 0; e < 4; e++) swingTime = MathHelper.SmoothStep(0f, 1f, swingTime);
				swingTime = Vector2.UnitX.RotatedBy(swingTime * MathHelper.Pi).Y - 0.3f;
				if(swingTime < 0f) swingTime = 0f;
				lightColor = Color.Red;
				lightColor.A = 0;
				texture = (Texture2D)ModContent.Request<Texture2D>("CalamityBardHealer/Projectiles/Slash_3");
				Main.EntitySpriteDraw(texture, player.MountedCenter - new Vector2(4, 2) * player.Directions - Main.screenPosition, null, lightColor * swingTime * 0.9f, base.Projectile.rotation + MathHelper.ToRadians(base.Projectile.spriteDirection * 15f), texture.Size() / 2, base.Projectile.scale * 2.4f, base.Projectile.spriteDirection < 0 ? SpriteEffects.FlipVertically : SpriteEffects.None, 0);
				lightColor = Color.Lerp(Color.Red, Color.DarkOrange, 0.25f);
				lightColor.A = 0;
				texture = (Texture2D)ModContent.Request<Texture2D>("CalamityBardHealer/Projectiles/Slash_2");
				Main.EntitySpriteDraw(texture, player.MountedCenter - new Vector2(4, 2) * player.Directions - Main.screenPosition, null, lightColor * swingTime * 0.9f, base.Projectile.rotation + MathHelper.ToRadians(base.Projectile.spriteDirection * 15f), texture.Size() / 2, base.Projectile.scale * 2.8f, base.Projectile.spriteDirection < 0 ? SpriteEffects.FlipVertically : SpriteEffects.None, 0);
				lightColor = Color.Lerp(Color.Red, Color.DarkOrange, 0.5f);
				lightColor.A = 0;
				texture = (Texture2D)ModContent.Request<Texture2D>("CalamityBardHealer/Projectiles/Slash_1");
				Main.EntitySpriteDraw(texture, player.MountedCenter - new Vector2(4, 2) * player.Directions - Main.screenPosition, null, lightColor * swingTime * 0.9f, base.Projectile.rotation + MathHelper.ToRadians(base.Projectile.spriteDirection * 15f), texture.Size() / 2, base.Projectile.scale * 3f, base.Projectile.spriteDirection < 0 ? SpriteEffects.FlipVertically : SpriteEffects.None, 0);
			}
			return false;
		}
		public override bool? CanDamage() => base.Projectile.ai[1] < base.Projectile.ai[0] * 0.6f && base.Projectile.ai[1] > base.Projectile.ai[0] * 0.1f ? null : false;
		public override void SendExtraAI(BinaryWriter writer) => writer.Write(Projectile.direction);
		public override void ReceiveExtraAI(BinaryReader reader) => Projectile.direction = reader.ReadInt32();
	}
}