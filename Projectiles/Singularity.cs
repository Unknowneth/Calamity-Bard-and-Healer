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
	public class Singularity : ScythePro
	{
		private bool shouldSpin = false;
		public override bool IsLoadingEnabled(Mod mod) => ModLoader.HasMod("CatalystMod");
		public override string Texture => "CalamityBardHealer/Items/Singularity";
		public override void SafeSetStaticDefaults() {
			if(!ModLoader.TryGetMod("Redemption", out Mod mor)) return;
			mor.Call("addElementProj", 14, base.Projectile.type);
		}
		public override void SafeSetDefaults() {
			base.Projectile.width = 86;
			base.Projectile.height = 126;
			base.Projectile.idStaticNPCHitCooldown = 4;
			base.Projectile.ArmorPenetration = 100;
			base.Projectile.alpha = 255;
			base.Projectile.manualDirectionChange = true;
		}
		public override bool PreAI() {
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
						if(player.HeldItem.ModItem is Items.Singularity s) shouldSpin = s.spin >= 3;
						base.Projectile.velocity = Vector2.Normalize(Main.MouseWorld - player.MountedCenter);
						NetMessage.SendData(27, -1, -1, null, base.Projectile.whoAmI);
					}
					swingTime = (base.Projectile.ai[1] / base.Projectile.ai[0] - 0.75f) * 4f;
					swing = base.Projectile.velocity.ToRotation() + MathHelper.ToRadians(MathHelper.SmoothStep(135f, 80f, swingTime) * player.direction);
					armSwing = base.Projectile.velocity.ToRotation() + MathHelper.Lerp(MathHelper.PiOver2, MathHelper.PiOver4, swingTime) * player.direction;
				}
				else if(shouldSpin) {
					swingTime = base.Projectile.ai[1] / (base.Projectile.ai[0] * 0.75f);
					swingTime = MathHelper.SmoothStep(0f, 1f, swingTime);
					swing = MathHelper.Lerp(MathHelper.ToRadians(100) * -player.direction, MathHelper.ToRadians(495) * player.direction, swingTime) + base.Projectile.velocity.ToRotation();
					armSwing = MathHelper.Lerp(MathHelper.PiOver2 * -player.direction, MathHelper.PiOver2 * 5f * player.direction, swingTime) + base.Projectile.velocity.ToRotation();
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
						if(player.HeldItem.ModItem is Items.Singularity s) shouldSpin = s.spin >= 3;
						base.Projectile.velocity = Vector2.Normalize(Main.MouseWorld - player.MountedCenter);
						NetMessage.SendData(27, -1, -1, null, base.Projectile.whoAmI);
					}
					swingTime = (base.Projectile.ai[1] / base.Projectile.ai[0] - 0.75f) * 4f;
					swing = base.Projectile.velocity.ToRotation() - MathHelper.ToRadians(MathHelper.SmoothStep(135f, 80f, swingTime) * player.direction);
					armSwing = base.Projectile.velocity.ToRotation() - MathHelper.Lerp(MathHelper.PiOver2, MathHelper.PiOver4, swingTime) * player.direction;
				}
				else if(shouldSpin) {
					swingTime = base.Projectile.ai[1] / (base.Projectile.ai[0] * 0.75f);
					swingTime = MathHelper.SmoothStep(0f, 1f, swingTime);
					swing = MathHelper.Lerp(MathHelper.ToRadians(100) * player.direction, MathHelper.ToRadians(495) * -player.direction, swingTime) + base.Projectile.velocity.ToRotation();
					armSwing = MathHelper.Lerp(MathHelper.PiOver2 * player.direction, MathHelper.PiOver2 * 5f * -player.direction, swingTime) + base.Projectile.velocity.ToRotation();
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
				bool oneStar = false;
				if(player.HeldItem.ModItem is Items.Singularity s) oneStar = s.spin < 2 && !shouldSpin;
				if(Main.myPlayer == player.whoAmI) if(shouldSpin) for(int i = 0; i < 6; i++) {
					int p = Projectile.NewProjectile(player.GetSource_ItemUse(player.HeldItem), base.Projectile.Center + Vector2.Normalize(base.Projectile.velocity).RotatedBy(MathHelper.ToRadians(60f) * i) * base.Projectile.height, Vector2.Normalize(base.Projectile.velocity).RotatedBy(MathHelper.ToRadians(60f) * i) * 16f, ModContent.ProjectileType<Projectiles.SingularityStar>(), base.Projectile.damage / 3, base.Projectile.knockBack, player.whoAmI);
					NetMessage.SendData(27, -1, -1, null, p);
				}
				else if(oneStar) {
					int p = Projectile.NewProjectile(player.GetSource_ItemUse(player.HeldItem), base.Projectile.Center + Vector2.Normalize(base.Projectile.velocity) * base.Projectile.height, Vector2.Normalize(base.Projectile.velocity) * 16f, ModContent.ProjectileType<Projectiles.SingularityStar>(), base.Projectile.damage, base.Projectile.knockBack, player.whoAmI);
					NetMessage.SendData(27, -1, -1, null, p);
				}
				else for(int i = -1; i <= 1; i++) {
					int p = Projectile.NewProjectile(player.GetSource_ItemUse(player.HeldItem), base.Projectile.Center + Vector2.Normalize(base.Projectile.velocity).RotatedBy(MathHelper.PiOver4 * 0.5f * i) * base.Projectile.height, Vector2.Normalize(base.Projectile.velocity).RotatedBy(MathHelper.PiOver4 * 0.25f * i) * 16f, ModContent.ProjectileType<Projectiles.SingularityStar>(), base.Projectile.damage, base.Projectile.knockBack, player.whoAmI);
					NetMessage.SendData(27, -1, -1, null, p);
				}
				SoundEngine.PlaySound(SoundID.DD2_MonkStaffSwing, player.position);
				if(shouldSpin) SoundEngine.PlaySound(SoundID.Item105, player.position);
				else SoundEngine.PlaySound(SoundID.Item9, player.position);
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
			if(ModLoader.TryGetMod("CatalystMod", out Mod catalyst)) target.AddBuff(catalyst.Find<ModBuff>("AstralBlight").Type, 240);
		}
		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) {
			float point = 0f;
			return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), base.Projectile.Center, base.Projectile.Center + base.Projectile.rotation.ToRotationVector2() * base.Projectile.height * base.Projectile.scale, base.Projectile.width * base.Projectile.scale * 0.5f, ref point);
		}
		public override bool PreDraw(ref Color lightColor) {
			lightColor = Color.White;
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
			Main.EntitySpriteDraw(texture, base.Projectile.Center - Main.screenPosition, null, lightColor, base.Projectile.rotation + MathHelper.PiOver2 - MathHelper.ToRadians(base.Projectile.spriteDirection * 15f), new Vector2(texture.Width / 2 - base.Projectile.spriteDirection * 16, texture.Height - 12), base.Projectile.scale, base.Projectile.spriteDirection < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
			if(base.Projectile.ai[1] / base.Projectile.ai[0] < 0.75f) {
				Player player = Main.player[base.Projectile.owner];
				float swingTime = base.Projectile.ai[1] / (base.Projectile.ai[0] * 0.75f);
				if(shouldSpin) swingTime = MathHelper.SmoothStep(0f, 1f, swingTime);
				else for(int e = 0; e < 4; e++) swingTime = MathHelper.SmoothStep(0f, 1f, swingTime);
				swingTime = Vector2.UnitX.RotatedBy(swingTime * MathHelper.Pi).Y - 0.3f;
				if(swingTime < 0f) swingTime = 0f;
				SpriteEffects spriteEffects = base.Projectile.spriteDirection > 0 ? SpriteEffects.FlipVertically : SpriteEffects.None;
				spriteEffects |= SpriteEffects.FlipHorizontally;
				lightColor = Color.Lerp(Color.Purple, Color.Magenta, swingTime) * 0.8f;
				lightColor.A = 0;
				texture = (Texture2D)ModContent.Request<Texture2D>("CalamityMod/Particles/TrientCircularSmear");
				Main.EntitySpriteDraw(texture, player.MountedCenter - new Vector2(4, 2) * player.Directions - Main.screenPosition, null, lightColor * swingTime, base.Projectile.rotation, texture.Size() / 2, base.Projectile.scale * 1.6f, spriteEffects, 0);
				texture = (Texture2D)ModContent.Request<Texture2D>("CalamityMod/Particles/SlashSmear");
				Main.EntitySpriteDraw(texture, player.MountedCenter - new Vector2(4, 2) * player.Directions - Main.screenPosition, null, lightColor * swingTime * 0.7f, base.Projectile.rotation, texture.Size() / 2, base.Projectile.scale, spriteEffects, 0);

			}
			return false;
		}
		public override void SendExtraAI(BinaryWriter writer) {
			writer.Write(shouldSpin);
			writer.Write(Projectile.direction);
		}
		public override void ReceiveExtraAI(BinaryReader reader) {
			shouldSpin = reader.ReadBoolean();
			Projectile.direction = reader.ReadInt32();
		}
		public override bool? CanDamage() => base.Projectile.ai[1] < base.Projectile.ai[0] * 0.6f && base.Projectile.ai[1] > base.Projectile.ai[0] * 0.1f ? null : false;
	}
}