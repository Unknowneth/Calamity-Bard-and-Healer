using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.Audio;
using Terraria.ModLoader;
using System;
using System.IO;
using ThoriumMod.Projectiles.Scythe;

namespace CalamityBardHealer.Projectiles
{
	public class TimesOldRoman : ScythePro
	{
		public override bool IsLoadingEnabled(Mod mod) => ModLoader.HasMod("CalamityHunt");
		private float scaleMultiplier = 0f;
		private float startPoint = 0f;
		public override string Texture => "CalamityBardHealer/Items/TimesOldRoman";
		public override void SafeSetStaticDefaults() {
			ProjectileID.Sets.TrailCacheLength[Type] = 16;
			ProjectileID.Sets.TrailingMode[Type] = 2;
			ProjectileID.Sets.HeldProjDoesNotUsePlayerGfxOffY[Type] = true;
			if(!ModLoader.TryGetMod("Redemption", out Mod mor)) return;
			mor.Call("addElementProj", 1, base.Projectile.type);
			mor.Call("addElementProj", 8, base.Projectile.type);
			mor.Call("addElementProj", 14, base.Projectile.type);
		}
		public override void SafeSetDefaults() {
			Projectile.width = 94;
			Projectile.height = 94;
			Projectile.aiStyle = -1;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
			Projectile.ownerHitCheck = true;
			Projectile.DamageType = ThoriumMod.HealerDamage.Instance;
			Projectile.timeLeft = 2;
			Projectile.ArmorPenetration = 20;
			Projectile.hide = true;
			Projectile.extraUpdates = 1;
		}
		public override bool PreAI() {
			Player player = Main.player[Projectile.owner];
			if(startPoint == 0f) {
				if(Main.myPlayer == player.whoAmI) {
					startPoint = Projectile.ai[1];
					int p = Projectile.NewProjectile(player.GetSource_ItemUse(player.HeldItem), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<Projectiles.TimesOldTrail>(), Projectile.damage, Projectile.knockBack, player.whoAmI);
					NetMessage.SendData(27, -1, -1, null, p);
					Projectile.ai[2] = p;
					NetMessage.SendData(27, -1, -1, null, Projectile.whoAmI);
				}
				SoundEngine.PlaySound(SoundID.DD2_MonkStaffSwing, Projectile.position);
			}
			float swingTime = 0f;
			scaleMultiplier = Projectile.scale * player.GetAdjustedItemScale(player.HeldItem);
			if(Projectile.ai[1] <= 0 || player.dead) Projectile.Kill();
			else {
				Projectile.timeLeft = (int)Projectile.ai[1];
				player.itemTime = (int)Projectile.ai[1];
				player.itemAnimation = (int)Projectile.ai[1];
				swingTime = Projectile.ai[1] / startPoint;
				if(Projectile.ai[0] != 3f) for(int e = 0; e < (Projectile.ai[0] == 2f ? 6 : Projectile.ai[0] == 1f ? 3 : 4); e++) swingTime = MathHelper.SmoothStep(0f, 1f, swingTime);
				if(player.whoAmI == Main.myPlayer) {
					Projectile.velocity += Vector2.Normalize(Main.MouseWorld - player.MountedCenter) * 0.54f;
					Projectile.velocity *= 0.96f;
					NetMessage.SendData(27, -1, -1, null, Projectile.whoAmI);
				}
				if(Projectile.ai[0] != 2f) player.heldProj = Projectile.whoAmI;
				player.compositeFrontArm.enabled = true;
				if(Projectile.velocity.X != 0) player.ChangeDir(Projectile.velocity.X > 0 ? 1 : -1);
			}
			switch(Projectile.ai[0]) {
				case 0:
					float swing = MathHelper.Lerp(MathHelper.ToRadians(135) * player.direction, MathHelper.ToRadians(135) * -player.direction, swingTime) + Projectile.velocity.ToRotation();
					float armSwing = MathHelper.Lerp(MathHelper.PiOver2 * player.direction, MathHelper.PiOver2 * -player.direction, swingTime) + Projectile.velocity.ToRotation();
					Projectile.Center = swing.ToRotationVector2() * 54f * scaleMultiplier;
					Projectile.rotation = swing;
					player.compositeFrontArm.rotation = armSwing - MathHelper.PiOver2 - (player.gravDir - 1) * MathHelper.PiOver2;
				break;
				case 1:
					swing = MathHelper.Lerp(MathHelper.ToRadians(135) * -player.direction, MathHelper.ToRadians(135) * player.direction, swingTime) + Projectile.velocity.ToRotation();
					armSwing = MathHelper.Lerp(MathHelper.PiOver2 * -player.direction, MathHelper.PiOver2 * player.direction, swingTime) + Projectile.velocity.ToRotation();
					Projectile.Center = swing.ToRotationVector2() * 54f * scaleMultiplier;
					Projectile.rotation = swing;
					player.compositeFrontArm.rotation = armSwing - MathHelper.PiOver2 - (player.gravDir - 1) * MathHelper.PiOver2;
				break;
			}
			player.compositeFrontArm.stretch = Player.CompositeArmStretchAmount.Full;
			Projectile.spriteDirection = player.direction;
			Projectile.Center = player.GetFrontHandPosition(player.compositeFrontArm.stretch, player.compositeFrontArm.rotation) + Projectile.Center;
			if(Main.projectile[(int)Projectile.ai[2]].ModProjectile is Projectiles.TimesOldTrail modProjectile) modProjectile.AddPoint(Projectile.Center + Projectile.rotation.ToRotationVector2() * (Projectile.Size.Length() - 9f) * 0.5f - Projectile.velocity);
			if(Projectile.ai[1] > 0f) if(--Projectile.ai[1] == 0f) if(player.HeldItem.ModItem is Items.TimesOldRoman modItem) modItem.oldVelocity = Projectile.velocity;
			return false;
		}
		public override void SafeOnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) => CanGiveScytheCharge = true;
		public override void SendExtraAI(BinaryWriter writer) {
			writer.Write(startPoint);
			writer.Write(scaleMultiplier);
			writer.Write(Projectile.direction);
		}
		public override void ReceiveExtraAI(BinaryReader reader) {
			startPoint = reader.ReadSingle();
			scaleMultiplier = reader.ReadSingle();
			Projectile.direction = reader.ReadInt32();
		}
		public override void SafeModifyDamageHitbox(ref Rectangle hitbox) {
			if(Projectile.ai[0] != 4f) hitbox = new Rectangle((int)(Projectile.Center.X + Vector2.Normalize(Projectile.velocity).X * 4f - Projectile.width / 2 * scaleMultiplier), (int)(Projectile.Center.Y + Vector2.Normalize(Projectile.velocity).Y * 4f - Projectile.height / 2 * scaleMultiplier), (int)(Projectile.width * scaleMultiplier), (int)(Projectile.height * scaleMultiplier));
		}
		public override bool PreDraw(ref Color lightColor) {
			lightColor = Lighting.GetColor((int)(Projectile.Center.X / 16f), (int)(Projectile.Center.Y / 16f));
			float rotation = MathHelper.PiOver4 * Projectile.spriteDirection;
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
			for(int i = Projectile.oldPos.Length - 1; i >= 0; i--) Main.EntitySpriteDraw(texture, Projectile.oldPos[i] + Projectile.Size * 0.5f + (Projectile.ai[0] != 4f ? Vector2.UnitY * Projectile.gfxOffY + (Main.player[Projectile.owner].position - Main.player[Projectile.owner].oldPosition) * 0.5f * i : Vector2.Zero) - Main.screenPosition, null, (i > 0 ? Main.DiscoColor with { A = 0 } * MathHelper.Lerp(0.9f, 0f, (float)i / (float)Projectile.oldPos.Length) : lightColor), Projectile.oldRot[i] + rotation, new Vector2(texture.Width, texture.Height / Main.projFrames[Type]) / 2, scaleMultiplier, Projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipVertically, 0);
			if(Projectile.ai[0] != 2f) return false;
			rotation = Projectile.rotation + rotation;
			float swingTime = Projectile.ai[1] / startPoint;
			return false;
		}
		public override bool ShouldUpdatePosition() => false;
	}
}