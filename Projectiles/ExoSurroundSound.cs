using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using System.Collections.Generic;

namespace CalamityBardHealer.Projectiles
{
	public class ExoSurroundSound : ModProjectile
	{
		public override void SetDefaults() {
			Projectile.width = 52;
			Projectile.height = 82;
			Projectile.aiStyle = -1;
			Projectile.tileCollide = false;
			Projectile.timeLeft = 2;
			Projectile.hide = true;
		}
		public override void AI() {
			Player player = Main.player[Projectile.owner];
			if(player.HeldItem.type == ModContent.ItemType<Items.SymphonicFabrications>() && !player.dead) Projectile.timeLeft = 2;
			else {
				Projectile.Kill();
				return;
			}
			bool attack = player.itemTime > 0 || player.channel;
			Projectile.spriteDirection = player.direction;
			Vector2 hoverPos = player.MountedCenter + new Vector2(48f - 96f * Projectile.ai[0], 16f) * player.Directions;
			if(Projectile.Center != hoverPos && Vector2.Distance(Projectile.Center, hoverPos) < 1f) Projectile.Center = hoverPos;
			else if(Vector2.Distance(Projectile.Center, hoverPos) > 1f) Projectile.Center = Vector2.Lerp(Projectile.Center, hoverPos, 0.8f);
			if(Main.myPlayer == player.whoAmI) {
				Projectile.velocity = Vector2.Normalize(Main.MouseWorld - Projectile.Center);
				NetMessage.SendData(27, -1, -1, null, Projectile.whoAmI);
			}
			if(attack) {
				if(Projectile.ai[1] < 20f) Projectile.ai[1]++;
				else if(Projectile.ai[2] == 45f * Projectile.ai[0] + 45f) for(int i = -1; i <= 1; i++) if(i != 0) {
					int p = Projectile.NewProjectile(player.GetSource_ItemUse(player.HeldItem), Projectile.Center + new Vector2(8, 19 * i).RotatedBy(Projectile.velocity.ToRotation()), Projectile.velocity.RotatedBy(MathHelper.PiOver4 * i / 9f) * 6f, ModContent.ProjectileType<Projectiles.ExoSound>(), (int)(player.GetWeaponDamage(player.HeldItem) * 1.5f), player.GetWeaponKnockback(player.HeldItem), player.whoAmI);
					NetMessage.SendData(27, -1, -1, null, p);
				}
			}
			else if(Projectile.ai[1] > 0f) Projectile.ai[1]--;
			Projectile.rotation = MathHelper.SmoothStep(0f, (Projectile.velocity * player.direction).ToRotation(), Projectile.ai[1] / 20f);
			float rot = ++Projectile.ai[2] / MathHelper.TwoPi / 60f;
			Projectile.position.Y += Vector2.UnitY.RotatedBy(rot).X * 8f;
			if(Projectile.ai[2] > 90f) Projectile.ai[2] = 0f;
		}
		public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI) {
			if(Projectile.ai[0] > 0f) overPlayers.Add(index);
			else behindNPCs.Add(index);
		}
		public override bool PreDraw(ref Color lightColor) {
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
			Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, Color.White, Projectile.rotation, new Vector2(38, 58), base.Projectile.scale, Projectile.spriteDirection < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
			return false;
		}
		public override bool? CanDamage() => false;
		public override bool ShouldUpdatePosition() => false;
	}
}