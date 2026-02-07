using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.Audio;
using Terraria.ModLoader;
using ThoriumMod.Projectiles.Scythe;
using CalamityMod.Particles;
using System.IO;
using System;

namespace CalamityBardHealer.Projectiles
{
	public class Procioplexon : ScythePro
	{
		public override bool IsLoadingEnabled(Mod mod) => ModLoader.HasMod("CalamityEntropy");
		private bool shouldSpin = false;
		public override string Texture => "CalamityBardHealer/Items/Procioplexon";
		public override void SafeSetStaticDefaults() {
			if(!ModLoader.TryGetMod("Redemption", out Mod mor)) return;
			mor.Call("addElementProj", 8, base.Projectile.type);
			mor.Call("addElementProj", 14, base.Projectile.type);
		}
		public override void SafeSetDefaults() {
			base.Projectile.width = 90;
			base.Projectile.height = 140;
			base.Projectile.idStaticNPCHitCooldown = 4;
			base.Projectile.ArmorPenetration = 100;
			base.Projectile.alpha = 255;
			base.Projectile.manualDirectionChange = true;
			base.Projectile.ArmorPenetration = 75;
		}
		public override bool PreAI() {
			float swingTime = 0f;
			float swingDelta = 0f;
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
						if(player.HeldItem.ModItem is Items.Procioplexon p) shouldSpin = p.spin >= 3;
						base.Projectile.velocity = Vector2.Normalize(Main.MouseWorld - player.MountedCenter);
						NetMessage.SendData(27, -1, -1, null, base.Projectile.whoAmI);
					}
					swingTime = (base.Projectile.ai[1] / base.Projectile.ai[0] - 0.75f) * 4f;
					swing = base.Projectile.velocity.ToRotation() + MathHelper.ToRadians(MathHelper.SmoothStep(135f, 80f, swingTime) * player.direction);
					armSwing = base.Projectile.velocity.ToRotation() + MathHelper.Lerp(MathHelper.PiOver2, MathHelper.PiOver4, swingTime) * player.direction;
				}
				else if(shouldSpin) {
					swingTime = base.Projectile.ai[1] / (base.Projectile.ai[0] * 0.75f);
					swingDelta = (base.Projectile.ai[1] + 1f) / (base.Projectile.ai[0] * 0.75f);
					swingTime = MathHelper.SmoothStep(0f, 1f, swingTime);
					swingDelta = MathHelper.SmoothStep(0f, 1f, swingDelta);
					swingDelta = Math.Abs((swingTime + MathHelper.Pi) - (swingDelta + MathHelper.Pi));
					swing = MathHelper.Lerp(MathHelper.ToRadians(100) * -player.direction, MathHelper.ToRadians(495) * player.direction, swingTime) + base.Projectile.velocity.ToRotation();
					armSwing = MathHelper.Lerp(MathHelper.PiOver2 * -player.direction, MathHelper.PiOver2 * 5f * player.direction, swingTime) + base.Projectile.velocity.ToRotation();
				}
				else {
					swingTime = base.Projectile.ai[1] / (base.Projectile.ai[0] * 0.75f);
					swingDelta = (base.Projectile.ai[1] + 1f) / (base.Projectile.ai[0] * 0.75f);
					for(int e = 0; e < 4; e++) {
						swingTime = MathHelper.SmoothStep(0f, 1f, swingTime);
						swingDelta = MathHelper.SmoothStep(0f, 1f, swingDelta);
					}
					swingDelta = Math.Abs((swingTime + MathHelper.Pi) - (swingDelta + MathHelper.Pi));
					swing = MathHelper.Lerp(MathHelper.ToRadians(100) * -player.direction, MathHelper.ToRadians(135) * player.direction, swingTime) + base.Projectile.velocity.ToRotation();
					armSwing = MathHelper.Lerp(MathHelper.PiOver2 * -player.direction, MathHelper.PiOver2 * player.direction, swingTime) + base.Projectile.velocity.ToRotation();
				}
				base.Projectile.spriteDirection = -player.direction;
			}
			else {
				if(base.Projectile.ai[1] / base.Projectile.ai[0] > 0.75f) {
					if(player.whoAmI == Main.myPlayer) {
						if(player.HeldItem.ModItem is Items.Procioplexon p) shouldSpin = p.spin >= 3;
						base.Projectile.velocity = Vector2.Normalize(Main.MouseWorld - player.MountedCenter);
						NetMessage.SendData(27, -1, -1, null, base.Projectile.whoAmI);
					}
					swingTime = (base.Projectile.ai[1] / base.Projectile.ai[0] - 0.75f) * 4f;
					swing = base.Projectile.velocity.ToRotation() - MathHelper.ToRadians(MathHelper.SmoothStep(135f, 80f, swingTime) * player.direction);
					armSwing = base.Projectile.velocity.ToRotation() - MathHelper.Lerp(MathHelper.PiOver2, MathHelper.PiOver4, swingTime) * player.direction;
				}
				else if(shouldSpin) {
					swingTime = base.Projectile.ai[1] / (base.Projectile.ai[0] * 0.75f);
					swingDelta = (base.Projectile.ai[1] + 1f) / (base.Projectile.ai[0] * 0.75f);
					swingTime = MathHelper.SmoothStep(0f, 1f, swingTime);
					swingDelta = MathHelper.SmoothStep(0f, 1f, swingDelta);
					swingDelta = Math.Abs((swingTime + MathHelper.Pi) - (swingDelta + MathHelper.Pi));
					swing = MathHelper.Lerp(MathHelper.ToRadians(100) * player.direction, MathHelper.ToRadians(495) * -player.direction, swingTime) + base.Projectile.velocity.ToRotation();
					armSwing = MathHelper.Lerp(MathHelper.PiOver2 * player.direction, MathHelper.PiOver2 * 5f * -player.direction, swingTime) + base.Projectile.velocity.ToRotation();
				}
				else {
					swingTime = base.Projectile.ai[1] / (base.Projectile.ai[0] * 0.75f);
					swingDelta = (base.Projectile.ai[1] + 1f) / (base.Projectile.ai[0] * 0.75f);
					for(int e = 0; e < 4; e++) {
						swingTime = MathHelper.SmoothStep(0f, 1f, swingTime);
						swingDelta = MathHelper.SmoothStep(0f, 1f, swingDelta);
					}
					swingDelta = Math.Abs((swingTime + MathHelper.Pi) - (swingDelta + MathHelper.Pi));
					swing = MathHelper.Lerp(MathHelper.ToRadians(100) * player.direction, MathHelper.ToRadians(135) * -player.direction, swingTime) + base.Projectile.velocity.ToRotation();
					armSwing = MathHelper.Lerp(MathHelper.PiOver2 * player.direction, MathHelper.PiOver2 * -player.direction, swingTime) + base.Projectile.velocity.ToRotation();
				}
				base.Projectile.spriteDirection = player.direction;
			}
			if(base.Projectile.localAI[2] == 0f && base.Projectile.ai[1] <= base.Projectile.ai[0] * 0.5f) {
				if(Main.myPlayer == player.whoAmI) if(!shouldSpin) {
					Vector2 shootDir = Vector2.Normalize(Main.MouseWorld - player.MountedCenter).RotatedBy(MathHelper.PiOver4 * base.Projectile.direction + Main.rand.Next(-10, 11) * MathHelper.PiOver4 * 0.02f);
					int p = Projectile.NewProjectile(player.GetSource_ItemUse(player.HeldItem), Main.MouseWorld - shootDir * 480f, shootDir * 16f, ModContent.ProjectileType<Projectiles.ProcioplexonSlash>(), base.Projectile.damage, base.Projectile.knockBack, player.whoAmI);
					NetMessage.SendData(27, -1, -1, null, p);
					p = Projectile.NewProjectile(player.GetSource_ItemUse(player.HeldItem), Projectile.Center + Vector2.Normalize(base.Projectile.velocity) * base.Projectile.height * 0.6f, Vector2.Normalize(Projectile.velocity) * 16f, ModContent.ProjectileType<Projectiles.ProcioplexonWave>(), base.Projectile.damage, base.Projectile.knockBack, player.whoAmI);
					NetMessage.SendData(27, -1, -1, null, p);
				}
				else for(int i = 0; i < 3; i++) {
					int p = Projectile.NewProjectile(player.GetSource_ItemUse(player.HeldItem), player.Center, Vector2.Zero, ModContent.ProjectileType<Projectiles.ProcioplexonVortex>(), base.Projectile.damage, base.Projectile.knockBack, player.whoAmI, i * 15f);
					NetMessage.SendData(27, -1, -1, null, p);
				}
				SoundEngine.PlaySound(SoundID.DD2_MonkStaffSwing, player.position);
				base.Projectile.localAI[2]++;
			}
			if(base.Projectile.alpha > 0) base.Projectile.alpha -= 17;
			player.compositeFrontArm.rotation = armSwing - MathHelper.PiOver2 - (player.gravDir - 1) * MathHelper.PiOver2;
			base.Projectile.Center = player.GetFrontHandPosition(player.compositeFrontArm.stretch, player.compositeFrontArm.rotation);
			base.Projectile.rotation = swing;
			if(swingDelta > 0f) for(int i = 0; i < (int)MathHelper.ToDegrees(swingDelta); i++) {
				Dust dust = Dust.NewDustPerfect(base.Projectile.Center + Main.rand.NextVector2Circular(20f, 20f) * base.Projectile.scale + new Vector2(84f, 40f * -base.Projectile.direction).RotatedBy(base.Projectile.rotation - (MathHelper.ToRadians(i + 1) * 2 + (player.direction - 1f) * 0.5f * MathHelper.PiOver4) * base.Projectile.direction) * base.Projectile.scale, 135, Vector2.Zero, 200, Color.SlateBlue with {A = 0}, base.Projectile.scale);
				dust.velocity -= Vector2.UnitY.RotatedBy(base.Projectile.rotation - MathHelper.ToRadians(i) * base.Projectile.direction) * Main.rand.Next(60, 91)  * (i % 2 == 0 ? -0.05f : 0.05f);
				dust.noGravity = true;
				dust.scale = Main.rand.NextFloat(0.6f, 0.9f) * base.Projectile.scale;
			}
			if(base.Projectile.ai[1] > 0f) base.Projectile.ai[1]--;
			return false;
		}
		public override void SafeOnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
			Color dustColor = Color.SlateBlue;
			for(int i = 0; i < 10; i++) {
				float rot = MathHelper.ToRadians(MathHelper.ToRadians(36f) * i);
				Vector2 velocity = Vector2.Normalize(target.Center - base.Projectile.Center).RotatedBy(MathHelper.PiOver4 * base.Projectile.spriteDirection) * base.Projectile.velocity.Length();
				Vector2 offset = velocity.RotatedBy((double)(rot * Main.rand.NextFloat(3.1f, 9.1f))) * new Vector2(Main.rand.NextFloat(1.5f, 5.5f));
				Vector2 velOffset = velocity.RotatedBy((double)(rot * Main.rand.NextFloat(3.1f, 9.1f))) * new Vector2(Main.rand.NextFloat(1.5f, 5.5f));
				GeneralParticleHandler.SpawnParticle(new MediumMistParticle(target.Center + offset, velOffset * Main.rand.NextFloat(1.5f, 3f), dustColor, dustColor, Main.rand.NextFloat(0.9f, 1.2f), 160f, Main.rand.NextFloat(0.03f, -0.03f)));
				Dust dust = Dust.NewDustPerfect(target.Center + offset, 135, new Vector2?(new Vector2(velOffset.X, velOffset.Y)), 0, dustColor, 0.6f);
				dust.noGravity = true;
				dust.velocity = velOffset;
				dust.scale = Main.rand.NextFloat(0.6f, 0.9f);
			}
			if(ModLoader.TryGetMod("CalamityEntropy", out Mod entropy)) target.AddBuff(entropy.Find<ModBuff>("LifeOppress").Type, 600);
		}
		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) {
			float point = 0f;
			return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), base.Projectile.Center, base.Projectile.Center + base.Projectile.rotation.ToRotationVector2() * base.Projectile.height * base.Projectile.scale, base.Projectile.width * base.Projectile.scale * 0.5f, ref point);
		}
		public override bool PreDraw(ref Color lightColor) {
			lightColor = Color.White;
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
			Main.EntitySpriteDraw(texture, base.Projectile.Center - Main.screenPosition, null, lightColor, base.Projectile.rotation + MathHelper.PiOver2 - MathHelper.ToRadians(base.Projectile.spriteDirection * 15f), new Vector2(texture.Width / 2 - base.Projectile.spriteDirection * 18, texture.Height - 42), base.Projectile.scale, base.Projectile.spriteDirection < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
			if(base.Projectile.ai[1] / base.Projectile.ai[0] < 0.75f) {
				Player player = Main.player[base.Projectile.owner];
				float swingTime = base.Projectile.ai[1] / (base.Projectile.ai[0] * 0.75f);
				if(shouldSpin) swingTime = MathHelper.SmoothStep(0f, 1f, swingTime);
				else for(int e = 0; e < 4; e++) swingTime = MathHelper.SmoothStep(0f, 1f, swingTime);
				swingTime = Vector2.UnitX.RotatedBy(swingTime * MathHelper.Pi).Y - 0.3f;
				if(swingTime < 0f) swingTime = 0f;
				float rotation = base.Projectile.rotation + Main.GlobalTimeWrappedHourly * MathHelper.PiOver4;
				while(rotation > MathHelper.Pi) rotation -= MathHelper.TwoPi;
				while(rotation < -MathHelper.Pi) rotation += MathHelper.TwoPi;
				SpriteEffects spriteEffects = base.Projectile.spriteDirection > 0 ? SpriteEffects.FlipVertically : SpriteEffects.None;
				spriteEffects |= SpriteEffects.FlipHorizontally;
				lightColor = Color.SlateBlue;
				lightColor.A = 0;
				texture = (Texture2D)ModContent.Request<Texture2D>("CalamityMod/Particles/TrientCircularSmear");
				Main.EntitySpriteDraw(texture, player.MountedCenter - new Vector2(4, 2) * player.Directions - Main.screenPosition, null, lightColor * swingTime, base.Projectile.rotation, texture.Size() / 2, base.Projectile.scale * 1.6f, spriteEffects, 0);
				texture = (Texture2D)ModContent.Request<Texture2D>("CalamityMod/Particles/SlashSmear");
				Main.EntitySpriteDraw(texture, player.MountedCenter - new Vector2(4, 2) * player.Directions - Main.screenPosition, null, lightColor * swingTime * 0.7f, base.Projectile.rotation, texture.Size() / 2, base.Projectile.scale * 1.2f, spriteEffects, 0);
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