using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.Drawing;
using Terraria.ModLoader;
using System;
using System.Collections.Generic;

namespace CalamityBardHealer.Projectiles
{
	public class HarmonyoftheOldGod : ModProjectile
	{
		public override bool IsLoadingEnabled(Mod mod) => ModLoader.HasMod("CalamityHunt");
		private static List<int> noteVariant = new();
		private static List<Vector2> notePosClient = new();
		private static Vector2 textureSize = new(0f, 0f);
		public override string GlowTexture => "CalamityBardHealer/Items/HarmonyoftheOldGod";
		public override void SetStaticDefaults() => Terraria.ID.ProjectileID.Sets.HeldProjDoesNotUsePlayerGfxOffY[Type] = true;
		public override void SetDefaults() {
			Projectile.width = 1;
			Projectile.height = 1;
			Projectile.aiStyle = -1;
			Projectile.tileCollide = false;
			Projectile.timeLeft = 2;
			Projectile.hide = true;
		}
		public override void AI() {
			Player player = Main.player[Projectile.owner];
			if(player.dead || player.HeldItem.ModItem is not Items.HarmonyoftheOldGod) {
				Projectile.Kill();
				return;
			}
			if(player.itemTime > 0) Projectile.ai[0]++;
			else if(Projectile.ai[0] > 0f) Projectile.ai[0]--;
			Projectile.timeLeft = 2;
			Projectile.Center = player.Center;
			player.heldProj = Projectile.whoAmI;
			player.compositeFrontArm.enabled = true;
			player.compositeBackArm.enabled = true;
			float sine = (float)Math.Sin(player.itemAnimation / (float)player.itemAnimationMax * MathHelper.TwoPi);
			player.compositeFrontArm.rotation = (-MathHelper.PiOver4 - sine * MathHelper.PiOver4 * 0.5f) * player.direction + player.itemRotation * 0.15f;
			Projectile.rotation = (float)Math.Sin(player.itemAnimation / (float)player.itemAnimationMax * MathHelper.Pi) * MathHelper.PiOver4 * 0.2f * player.direction + player.itemRotation * 0.25f;
			player.compositeBackArm.rotation = player.itemRotation * 0.2f - MathHelper.PiOver2 * player.direction;
			player.compositeFrontArm.stretch = sine < -0.5f ? Player.CompositeArmStretchAmount.Quarter : sine < 0f ? Player.CompositeArmStretchAmount.ThreeQuarters : Player.CompositeArmStretchAmount.Full;
			player.compositeBackArm.stretch = Player.CompositeArmStretchAmount.Full;
			if(player.whoAmI != Main.myPlayer || notePosClient is null || notePosClient.Count == 0) return;
			List<Vector2> notePosClientClone = notePosClient;
			for(int h = 0; h < notePosClientClone.Count; h++) if(Main.rand.NextBool(50) || notePosClientClone[h].X - 1 <= 0) {
				Vector2 staffOffset = notePosClientClone[h];
				sine = (float)Math.Sin(Main.GlobalTimeWrappedHourly * MathHelper.TwoPi + MathHelper.PiOver4 * 0.2f * staffOffset.X);
				float colorHsl = sine + MathHelper.ToRadians(staffOffset.X + Main.screenWidth / textureSize.Y * 2 + staffOffset.Y);
				staffOffset.Y *= textureSize.X / 4;
				staffOffset.Y += sine * 6f;
				staffOffset.X *= textureSize.Y / 4;
				staffOffset.X += Main.screenWidth / 2;
				staffOffset += Main.screenPosition;
				int p = Projectile.NewProjectile(player.GetSource_ItemUse(player.HeldItem), staffOffset, Main.rand.NextVector2Circular(9f, 1f) - Vector2.UnitY, ModContent.ProjectileType<Projectiles.GodNote>(), player.GetWeaponDamage(player.HeldItem), player.GetWeaponKnockback(player.HeldItem), player.whoAmI, colorHsl / MathHelper.Pi, noteVariant[h]);
				NetMessage.SendData(27, -1, -1, null, p);
				for(int g = 0; g < 10; g++) ParticleOrchestrator.RequestParticleSpawn(clientOnly: false, ParticleOrchestraType.ChlorophyteLeafCrystalShot, new ParticleOrchestraSettings { PositionInWorld = staffOffset, MovementVector = Main.rand.NextVector2Circular(g, g), UniqueInfoPiece = (byte)(Main.rgbToHsl(Main.hslToRgb(colorHsl / MathHelper.Pi % 1f, 1f, 0.5f)).X * 255f)});
				notePosClient.RemoveAt(h);
				noteVariant.RemoveAt(h);
			}
			else notePosClient[h] = notePosClient[h] - Vector2.UnitX;
		}
		public override bool PreDraw(ref Color lightColor) {
			Player player = Main.player[Projectile.owner];
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(GlowTexture);
			SpriteEffects spriteEffects = player.direction < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
			if(player.gravDir < 0f) spriteEffects |= SpriteEffects.FlipVertically;
			Main.EntitySpriteDraw(texture, player.GetBackHandPosition(player.compositeBackArm.stretch, player.compositeBackArm.rotation) - Main.screenPosition, null, lightColor, player.itemRotation * 0.1f, new Vector2(player.direction < 0f ? texture.Width - 12f : 12f, player.gravDir < 0f ? texture.Height - 66f : 66f), Projectile.scale, spriteEffects, 0);
			texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
			Main.EntitySpriteDraw(texture, player.GetFrontHandPosition(player.compositeFrontArm.stretch, player.compositeFrontArm.rotation) - Main.screenPosition, null, lightColor, Projectile.rotation, new Vector2(player.direction < 0f ? texture.Width - 10f : 10f, player.gravDir < 0f ? texture.Height - 11f : 11f), Projectile.scale, spriteEffects, 0);
			if(Main.myPlayer != player.whoAmI) return false;
			texture = (Texture2D)ModContent.Request<Texture2D>("Terraria/Images/Extra_98");
			textureSize = texture.Size();
			Vector2 staffPos = new Vector2(Main.screenWidth / 2f, 16f);
			int width = Main.screenWidth / (int)textureSize.Y * 2;
			Texture2D glowTexture = (Texture2D)ModContent.Request<Texture2D>("CalamityBardHealer/Projectiles/G_ClefLarge");
			string[] notes = new string[] {
				"WholeNote",
				"HalfNote",
				"QuarterNote",
				"EighthNote"
			};
			if(player.HeldItem.ModItem is Items.HarmonyoftheOldGod modItem && modItem.shootClient) {
				modItem.shootClient = false;
				notePosClient.Add(new Vector2(width, Main.rand.Next(5)));
				noteVariant.Add(Main.rand.Next(notes.Length));
			}
			for(int i = -width; i <= width; i++) for(int j = 0; j < 5; j++) {
				float sine = Main.GlobalTimeWrappedHourly * MathHelper.TwoPi + MathHelper.PiOver4 * 0.2f * i;
				lightColor = Main.hslToRgb(((sine + MathHelper.ToRadians(i + width + j)) / MathHelper.Pi) % 1f, 1f, 0.5f) with { A = 0 };
				Vector2 staffOffset = new Vector2(i * textureSize.Y / 4, (float)Math.Sin(sine) * 6 + j * textureSize.X / 4);
				Main.EntitySpriteDraw(texture, staffPos + staffOffset, null, lightColor, MathHelper.PiOver2 + (float)Math.Cos(sine) * 0.06f, textureSize * 0.5f, new Vector2(0.3f, 1.8f), SpriteEffects.None, 0);
				if(i == -width + 2 && j == 2) for(int k = 0; k < 7; k++) Main.EntitySpriteDraw(glowTexture, staffPos + staffOffset + Vector2.UnitY.RotatedBy(MathHelper.TwoPi / 3f * (k - 1) + (k > 3 ? MathHelper.TwoPi / 6f : 0f) + sine) * MathHelper.Max(k, 1), null, lightColor * (k > 3 ? 0.15f : 0.45f), 0f, glowTexture.Size() * 0.5f, 1f, SpriteEffects.None, 0);
				if(notePosClient?.Count > 0) foreach(Vector2 v in notePosClient) if(i == (int)v.X && j == (int)v.Y) {
					Texture2D noteTexture = (Texture2D)ModContent.Request<Texture2D>("CalamityBardHealer/Projectiles/" + notes[noteVariant[notePosClient.IndexOf(v)]]);
					for(int l = 0; l < 7; l++) Main.EntitySpriteDraw(noteTexture, staffPos + staffOffset + Vector2.UnitY.RotatedBy(MathHelper.TwoPi / 3f * (l - 1) + (l > 3 ? MathHelper.TwoPi / 6f : 0f) + sine) * MathHelper.Max(l, 1), null, lightColor * (l > 3 ? 0.15f : 0.45f), 0f, noteTexture.Size() * 0.5f, 1f, SpriteEffects.None, 0);
				}
			}
			return false;
		}
		public override bool? CanDamage() => false;
		public override bool ShouldUpdatePosition() => false;
	}
}
			/*reminder to self that you accidentally made a false water thingy with this lol, keep this for future use :)
			--Variant 1-- longer but skinnier
			texture = (Texture2D)ModContent.Request<Texture2D>("Terraria/Images/Extra_98");
			Vector2 staffPos = new Vector2(Main.screenWidth / 2f, 16f);
			int width = Main.screenWidth / texture.Height * 3;
			for(int i = -width; i <= width; i++) for(int j = 0; j < 5; j++) {
				float sine = (float)Math.Sin(Main.GlobalTimeWrappedHourly * MathHelper.Pi + MathHelper.PiOver4 * 0.2f * i);
				Main.EntitySpriteDraw(texture, staffPos + new Vector2(i * texture.Height / 6, sine * 8 + j * texture.Width / 4), null, Color.Gold with { A = 0 }, MathHelper.PiOver2 + sine * 0.1f, texture.Size() * 0.5f, new Vector2(0.4f, 1.1f), SpriteEffects.None, 0);
			}
			--Variant 2-- thicker but shorter
			texture = (Texture2D)ModContent.Request<Texture2D>("Terraria/Images/Extra_98");
			Vector2 staffPos = new Vector2(Main.screenWidth / 2f, 16f);
			int width = Main.screenWidth / texture.Height * 4;
			for(int i = -width; i <= width; i++) for(int j = 0; j < 5; j++) {
				float sine = (float)Math.Sin(sineMain.GlobalTimeWrappedHourly * MathHelper.Pi + MathHelper.PiOver4 * 0.2f * i);
				Main.EntitySpriteDraw(texture, staffPos + new Vector2(i * texture.Height / 8, sine * 8 + j * texture.Width / 4), null, Color.Gold with { A = 0 }, MathHelper.PiOver2 + sine * 0.1f, texture.Size() * 0.5f, 1f, SpriteEffects.None, 0);
			}*/