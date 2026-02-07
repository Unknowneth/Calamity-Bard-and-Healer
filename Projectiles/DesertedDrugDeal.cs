using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.Audio;
using Terraria.GameContent.Drawing;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ModLoader;
using System;

namespace CalamityBardHealer.Projectiles
{
	public class DesertedDrugDeal : ModProjectile
	{
		public override string Texture => "CalamityBardHealer/Items/DesertedDrugDeal";
		public override string GlowTexture => "CalamityBardHealer/Projectiles/DesertedDrugDeal";
		public override void SetStaticDefaults() {
			ProjectileID.Sets.HeldProjDoesNotUsePlayerGfxOffY[Type] = true;
			if(!ModLoader.TryGetMod("Redemption", out Mod mor)) return;
			mor.Call("addElementProj", 3, base.Projectile.type);
			mor.Call("addElementProj", 5, base.Projectile.type);
		}
		public override void SetDefaults() {
			Projectile.width = 24;
			Projectile.height = 76;
			Projectile.aiStyle = -1;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
			Projectile.ownerHitCheck = true;
			Projectile.timeLeft = 2;
			Projectile.hide = true;
			Projectile.netImportant = true;
			Projectile.DamageType = ThoriumMod.HealerDamage.Instance;
		}
		public override void AI() {
			Player player = Main.player[Projectile.owner];
			if(player.dead || player.HeldItem.ModItem is not Items.DesertedDrugDeal) {
				Projectile.Kill();
				return;
			}
			else Projectile.timeLeft = 2;
			if(player.whoAmI == Main.myPlayer) {
				Projectile.velocity = Vector2.Normalize(Main.MouseWorld - player.MountedCenter);
				NetMessage.SendData(27, -1, -1, null, Projectile.whoAmI);
			}
			if(Projectile.ai[1] < 10f && Projectile.ai[1] > 0f) {
				Projectile.ai[1]--;
				player.itemTime = (int)Projectile.ai[1];
				player.itemAnimation = (int)Projectile.ai[1];
			}
			else if(player.channel || (Projectile.ai[1] >= 10f && Projectile.ai[0] < 10f)) {
				if(Projectile.ai[1] < 10f) {
					Projectile.ai[0] = 0f;
					Projectile.ai[1] = 10f;
				}
				else Projectile.ai[0] += player.GetWeaponAttackSpeed(player.HeldItem);
			}
			else if(!player.channel) {
				if(Projectile.ai[1] > 0f) Projectile.ai[1]--;
				else if(Projectile.ai[1] <= 0f) Projectile.ai[0] -= player.GetWeaponAttackSpeed(player.HeldItem) * 6f;
			}
			Projectile.ai[0] = (float)MathHelper.Clamp(Projectile.ai[0], 0f, 50f);
			float pullTime = (float)Math.Sqrt(MathHelper.Min(Projectile.ai[0] / 20f, 1f));
			if(Projectile.velocity.X != 0) player.direction = Projectile.velocity.X > 0 ? 1 : -1;
			Projectile.spriteDirection = Projectile.direction = player.direction;
			player.itemRotation = Projectile.rotation = (Projectile.velocity * Projectile.direction).ToRotation();
			player.heldProj = Projectile.whoAmI;
			player.SetCompositeArmFront(enabled: true, pullTime > 0.9f ? Player.CompositeArmStretchAmount.Quarter : pullTime > 0.4f ? Player.CompositeArmStretchAmount.ThreeQuarters : Player.CompositeArmStretchAmount.Full, Projectile.rotation * player.gravDir - Projectile.spriteDirection * MathHelper.PiOver2);
			player.SetCompositeArmBack(enabled: true, Player.CompositeArmStretchAmount.Full, Projectile.rotation * 1.15f * player.gravDir - Projectile.spriteDirection * MathHelper.PiOver2);
			Projectile.position = (new Vector2(player.Center.X - Projectile.width / 2 - (float)(Projectile.spriteDirection * 2), player.MountedCenter.Y - Projectile.height / 2)) - (new Vector2(2, ((player.bodyFrame.Y >= player.bodyFrame.Height * 7 && player.bodyFrame.Y <= player.bodyFrame.Height * 9) || (player.bodyFrame.Y >= player.bodyFrame.Height * 14 && player.bodyFrame.Y <= player.bodyFrame.Height * 16) ? 4 : 2)) * player.Directions) + new Vector2(Projectile.spriteDirection * 24, player.gravDir * 2).RotatedBy(Projectile.rotation);
			if(player.PickAmmo(player.HeldItem, out int type, out float speed, out int Damage, out float KnockBack, out int usedAmmoItemId, Projectile.ai[1] != 9f) && Projectile.ai[1] == 9f) {
				float actualDamage = player.GetWeaponDamage(player.HeldItem) + (float)player.GetWeaponDamage(player.HeldItem) * Projectile.ai[0] / 40f;
				bool maxCharge = Projectile.ai[0] >= 40f;
				if(Main.myPlayer == Projectile.owner) if(!maxCharge) {
					int p = Projectile.NewProjectile(player.GetSource_ItemUse(player.HeldItem), Projectile.Center - Projectile.velocity * 16f, Projectile.velocity * 16f, ModContent.ProjectileType<Projectiles.SharkBloodInjection>(), (int)actualDamage, player.HeldItem.knockBack, Projectile.owner);
					NetMessage.SendData(27, -1, -1, null, p);
				}
				else for(int i = -1; i <= 1; i++) {
					int p = Projectile.NewProjectile(player.GetSource_ItemUse(player.HeldItem), Projectile.Center - Projectile.velocity * 16f, Projectile.velocity.RotatedBy(MathHelper.PiOver4 / 3f * i) * 16f, ModContent.ProjectileType<Projectiles.SharkBloodInjection>(), (int)actualDamage, player.HeldItem.knockBack, Projectile.owner);
					NetMessage.SendData(27, -1, -1, null, p);
				}
				for(int j = 0; j < 9; j++) ParticleOrchestrator.RequestParticleSpawn(clientOnly: false, ParticleOrchestraType.ChlorophyteLeafCrystalShot, new ParticleOrchestraSettings { PositionInWorld = Projectile.Center, MovementVector = Vector2.Normalize(Projectile.velocity) * 16f + Main.rand.NextVector2Circular(10f, 10f), UniqueInfoPiece = (byte)(Main.rgbToHsl(Color.Cyan).X * 255f)});
				SoundEngine.PlaySound(SoundID.Item68, Projectile.Center);
			}
		}
		public override bool PreDraw(ref Color lightColor) {
			Player player = Main.player[Projectile.owner];
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
			SpriteEffects spriteEffects = Projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
			if(player.gravDir == -1) spriteEffects |= SpriteEffects.FlipVertically;
			float pullTime = (float)Math.Sqrt(MathHelper.Min(Projectile.ai[0] / 20f, 1f));
			if(Projectile.ai[1] < 10f) {
				pullTime *= (float)(Projectile.ai[1] - 5f) / 5f;
				pullTime *= Math.Abs(pullTime);
				if(Projectile.ai[1] < 5f) pullTime *= 0.25f;
			}
			Main.EntitySpriteDraw(texture, Projectile.Center - new Vector2(8f * Projectile.spriteDirection, 28f).RotatedBy(Projectile.rotation) - Main.screenPosition, new Rectangle(2, 10, 2, 28), Color.White, Projectile.rotation + pullTime * MathHelper.ToRadians(19.65389f) * Projectile.spriteDirection, new Vector2(Projectile.spriteDirection + 1, 0), Projectile.scale * new Vector2(1f, 1f + Math.Abs(pullTime) * 0.08948f), spriteEffects, 0);
			Main.EntitySpriteDraw(texture, Projectile.Center - new Vector2(8f * Projectile.spriteDirection, -28f).RotatedBy(Projectile.rotation) - Main.screenPosition, new Rectangle(2, 38, 2, 28), Color.White, Projectile.rotation - pullTime * MathHelper.ToRadians(19.65389f) * Projectile.spriteDirection, new Vector2(Projectile.spriteDirection + 1, 28), Projectile.scale * new Vector2(1f, 1f + Math.Abs(pullTime) * 0.08948f), spriteEffects, 0);
			Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, new Rectangle(0, 0, 24, 10), lightColor, Projectile.rotation, texture.Size() * 0.5f - (player.gravDir == 1 ? Vector2.Zero : Vector2.UnitY * 66f), Projectile.scale, spriteEffects, 0);
			Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, new Rectangle(4, 0, 20, 76), lightColor, Projectile.rotation, texture.Size() * 0.5f - (Projectile.spriteDirection == 1 ? Vector2.UnitX * 4f : Vector2.Zero), Projectile.scale, spriteEffects, 0);
			Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, new Rectangle(0, 66, 24, 10), lightColor, Projectile.rotation, texture.Size() * 0.5f - (player.gravDir == 1 ? Vector2.UnitY * 66f : Vector2.Zero), Projectile.scale, spriteEffects, 0);
			texture = (Texture2D)ModContent.Request<Texture2D>(GlowTexture);
			Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, new Color(250, 250, 250, 0), Projectile.rotation, texture.Size() * 0.5f, Projectile.scale, spriteEffects, 0);
			if(Projectile.ai[1] >= 10f) for(int i = 0; i < 2; i++) {
				Vector2 arrowPos = Projectile.Center + Vector2.UnitX.RotatedBy(Projectile.rotation) * MathHelper.Lerp(10f, -2f, pullTime) * Projectile.spriteDirection;
				Color glowColor = new Color(250, 250, 250, 0);
				texture = (Texture2D)ModContent.Request<Texture2D>("CalamityBardHealer/Projectiles/SharkBloodInjection");
				Texture2D glowTexture = (Texture2D)ModContent.Request<Texture2D>("CalamityBardHealer/Projectiles/SharkBloodInjection_Glow");
				Rectangle frame = new Rectangle(0, 0, texture.Width, texture.Height / 6);
				Rectangle glowFrame = new Rectangle(0, 0, glowTexture.Width, glowTexture.Height / 6);
				Vector2 origin = new Vector2(frame.Width, frame.Height) / 2f;
				Vector2 glowOrigin = new Vector2(glowFrame.Width, glowFrame.Height) / 2f;
				if(Projectile.ai[0] >= 30f) {
					float tilt = MathHelper.PiOver4 * (Projectile.ai[0] - 30f) / 60f;
					Vector2 arrowOffset = -Vector2.UnitX.RotatedBy(Projectile.rotation) * 12f * Projectile.spriteDirection + Vector2.UnitX.RotatedBy(Projectile.rotation + tilt) * 12f * Projectile.spriteDirection;
					Main.EntitySpriteDraw(texture, arrowPos + arrowOffset - Main.screenPosition, frame, lightColor * ((Projectile.ai[0] - 30f) / 20f), Projectile.rotation + tilt, origin, Projectile.scale, spriteEffects, 0);
					Main.EntitySpriteDraw(glowTexture, arrowPos + arrowOffset - Main.screenPosition, glowFrame, glowColor * ((Projectile.ai[0] - 30f) / 20f), Projectile.rotation + tilt, glowOrigin, Projectile.scale, spriteEffects, 0);
					arrowOffset = -Vector2.UnitX.RotatedBy(Projectile.rotation) * 12f * Projectile.spriteDirection + Vector2.UnitX.RotatedBy(Projectile.rotation - tilt) * 12f * Projectile.spriteDirection;
					Main.EntitySpriteDraw(texture, arrowPos + arrowOffset - Main.screenPosition, frame, lightColor * ((Projectile.ai[0] - 30f) / 20f), Projectile.rotation - tilt, origin, Projectile.scale, spriteEffects, 0);
					Main.EntitySpriteDraw(glowTexture, arrowPos + arrowOffset - Main.screenPosition, glowFrame, glowColor * ((Projectile.ai[0] - 30f) / 20f), Projectile.rotation - tilt, glowOrigin, Projectile.scale, spriteEffects, 0);
				}
				Main.EntitySpriteDraw(texture, arrowPos - Main.screenPosition, frame, lightColor, Projectile.rotation, origin, Projectile.scale, spriteEffects, 0);
				Main.EntitySpriteDraw(glowTexture, arrowPos - Main.screenPosition, glowFrame, glowColor, Projectile.rotation, glowOrigin, Projectile.scale, spriteEffects, 0);
			}
			texture = (Texture2D)ModContent.Request<Texture2D>("Terraria/Images/Extra_174");
			if(Projectile.ai[1] < 10f && Projectile.ai[1] > 0f) for(int i = 0; i < 2; i++) {
				Main.EntitySpriteDraw(texture, Projectile.Center + Projectile.rotation.ToRotationVector2() * MathHelper.Lerp(24f, 10f, Projectile.ai[1] / 10f) * Projectile.spriteDirection - Main.screenPosition, null, Color.Aquamarine * (Projectile.ai[1] / 10f), Projectile.rotation, texture.Size() * 0.5f, Projectile.scale * MathHelper.Lerp(0.5f, 0f, Projectile.ai[1] / 10f) * new Vector2(0.75f, 1.5f), spriteEffects, 0);
				if(Projectile.ai[0] >= 20f) Main.EntitySpriteDraw(texture, Projectile.Center + Projectile.rotation.ToRotationVector2() * MathHelper.Lerp(36f, 12f, Projectile.ai[1] / 10f) * Projectile.spriteDirection - Main.screenPosition, null, Color.Cyan * (Projectile.ai[1] / 10f), Projectile.rotation, texture.Size() * 0.5f, Projectile.scale * MathHelper.Lerp(0.3f, 0f, Projectile.ai[1] / 10f) * new Vector2(0.75f, 1.5f), spriteEffects, 0);
				if(Projectile.ai[0] >= 40f) Main.EntitySpriteDraw(texture, Projectile.Center + Projectile.rotation.ToRotationVector2() * MathHelper.Lerp(48f, 14f, Projectile.ai[1] / 10f) * Projectile.spriteDirection - Main.screenPosition, null, Color.Aquamarine * (Projectile.ai[1] / 10f), Projectile.rotation, texture.Size() * 0.5f, Projectile.scale * MathHelper.Lerp(0.2f, 0f, Projectile.ai[1] / 10f) * new Vector2(0.75f, 1.5f), spriteEffects, 0);
			}
			return false;
		}
		public override bool? CanDamage() => false;
		public override bool ShouldUpdatePosition() => false;
	}
}