using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using ThoriumMod;
using ThoriumMod.Utilities;
using ThoriumMod.Buffs.Healer;
using ThoriumMod.Projectiles.Healer;
using ThoriumMod.Items;
using System;

namespace CalamityBardHealer
{
	public class HealerHelper
	{
		public static bool HealPlayer(Player healer, Player target, int healAmount = 1, int recoveryTime = 0, bool healEffects = true, Action<Player> extraEffects = null, Func<Player, bool> canHealTarget = null) {
			if(canHealTarget != null && !canHealTarget(target)) return false;
			int type = ModContent.ProjectileType<HealNoEffects>();
			if(healEffects) type = ModContent.ProjectileType<Heal>();
			if(healer.whoAmI == Main.myPlayer) {
				int p = Projectile.NewProjectile(healer.GetSource_OnHit(target), target.Center, Vector2.Zero, type, 0, 0f, healer.whoAmI, ModContent.GetInstance<BalanceConfig>().healing * healAmount, target.whoAmI);
				NetMessage.SendData(27, -1, -1, null, p);
			}
			if(recoveryTime > 0) {
				target.GetThoriumPlayer().SetLifeRecoveryEffect(LifeRecoveryEffectType.Generic, (short)recoveryTime, true);
				target.AddBuff(ModContent.BuffType<QuickRecovery>(), recoveryTime, true, false);
			}
			if(extraEffects != null) extraEffects(target);
			return true;
		}
	}
}