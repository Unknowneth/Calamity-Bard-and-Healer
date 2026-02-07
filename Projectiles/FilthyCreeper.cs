using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using System.Collections.Generic;

namespace CalamityBardHealer.Projectiles
{
	public class FilthyCreeper : ModProjectile
	{
		public override void SetDefaults() {
			Projectile.width = 26;
			Projectile.height = 26;
			Projectile.aiStyle = -1;
			Projectile.tileCollide = false;
			Projectile.timeLeft = 60;
		}
		public override void AI() {
			Player player = Main.player[Projectile.owner];
			float attackTime = (float)Projectile.timeLeft / 60f;
			Projectile.Center = player.MountedCenter + (Vector2.UnitX.RotatedBy(attackTime * MathHelper.TwoPi) * new Vector2(Projectile.velocity.Length(), Projectile.velocity.Length() * 2f)).RotatedBy(Projectile.velocity.ToRotation()) * 2f - Projectile.velocity;
			if(Main.myPlayer == player.whoAmI && Projectile.timeLeft == 15) {
				int index = -1;
				float maxDistSQ = 1000000f;
				foreach(NPC npc in Main.ActiveNPCs) if(npc.CanBeChasedBy(null, false)) {
					float distSQ = player.DistanceSQ(npc.Center);
					if(distSQ < maxDistSQ && Collision.CanHit(player.Center, 1, 1, npc.Center, 1, 1)) {
						maxDistSQ = distSQ;
						index = npc.whoAmI;
					}
				}
				int p = Projectile.NewProjectile(player.GetSource_ItemUse(player.HeldItem), Projectile.Center, Vector2.Normalize((index > -1 ? Main.npc[index].Center : Main.MouseWorld) - Projectile.Center) * Projectile.ai[0], ModContent.ProjectileType<Projectiles.FilthyFlute>(), Projectile.damage, Projectile.knockBack, player.whoAmI, Projectile.ai[1]);
				NetMessage.SendData(27, -1, -1, null, p);
			}
			Projectile.hide = (attackTime * MathHelper.TwoPi).ToRotationVector2().X < 0f;
		}
		public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI) {
			if(Projectile.hide) overPlayers.Add(index);
			else behindProjectiles.Add(index);
		}
		public override bool? CanDamage() => false;
		public override bool ShouldUpdatePosition() => false;
	}
}