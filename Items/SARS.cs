using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using ThoriumMod;
using ThoriumMod.Items.HealerItems;

namespace CalamityBardHealer.Items
{
	public class SARS: ScytheItem
	{
		public override void SetStaticDefaults() => base.SetStaticDefaultsToScythe();
		public override void SetDefaults() {
			base.SetDefaultsToScythe();
			base.Item.crit = 12;
			base.Item.damage = 123;
			this.scytheSoulCharge = 3;
			base.Item.width = 74;
			base.Item.height = 56;
			base.Item.scale = 1.25f;
			base.Item.value = Item.sellPrice(gold: 12);
			base.Item.rare = 8;
			base.Item.shoot = ModContent.ProjectileType<Projectiles.SARS>();
			if(ModContent.GetInstance<BalanceConfig>().radiant != 1f) base.Item.damage = (int)MathHelper.Max(ModContent.GetInstance<BalanceConfig>().radiant * (float)base.Item.damage, 1);
			if(ModContent.GetInstance<BalanceConfig>().soulEssence != 1) this.scytheSoulCharge = (int)MathHelper.Clamp(ModContent.GetInstance<BalanceConfig>().soulEssence * this.scytheSoulCharge, 0, 5);
		}
	}
}