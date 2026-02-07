using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod.Items.HealerItems;

namespace CalamityBardHealer.Items
{
	public class HyphaeBaton : ScytheItem
	{
		public override void SetStaticDefaults() => base.SetStaticDefaultsToScythe();
		public override void SetDefaults() {
			base.SetDefaultsToScythe();
			base.Item.damage = 15;
			base.Item.DamageType = ThoriumMod.HealerDamage.Instance;
			this.isHealer = true;
			this.scytheSoulCharge = 1;
			base.Item.width = 36;
			base.Item.height = 36;
			base.Item.value = Item.sellPrice(0, 0, 0, 40);
			base.Item.rare = 2;
			base.Item.shoot = ModContent.ProjectileType<Projectiles.HyphaeBaton>();
			if(ModContent.GetInstance<BalanceConfig>().radiant != 1f) base.Item.damage = (int)MathHelper.Max(ModContent.GetInstance<BalanceConfig>().radiant * (float)base.Item.damage, 1);
			if(ModContent.GetInstance<BalanceConfig>().soulEssence != 1) this.scytheSoulCharge = (int)MathHelper.Clamp(ModContent.GetInstance<BalanceConfig>().soulEssence * this.scytheSoulCharge, 0, 5);
		}
	}
}