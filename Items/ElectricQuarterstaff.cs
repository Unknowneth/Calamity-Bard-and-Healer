using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using ThoriumMod;
using ThoriumMod.Items.HealerItems;
using CalamityMod.CustomRecipes;
using CalamityMod.Items;

namespace CalamityBardHealer.Items
{
	public class ElectricQuarterstaff: ScytheItem
	{
		public override void SetStaticDefaults() => base.SetStaticDefaultsToScythe();
		public override void SetDefaults() {
			base.SetDefaultsToScythe();
			base.Item.crit = 12;
			base.Item.damage = 46;
			this.scytheSoulCharge = 2;
			base.Item.width = 94;
			base.Item.height = 94;
			base.Item.value = Item.sellPrice(gold: 4, silver: 80);
			base.Item.rare = 5;
			base.Item.shoot = ModContent.ProjectileType<Projectiles.ElectricQuarterstaff>();
			base.Item.GetGlobalItem<CalamityGlobalItem>().UsesCharge = true;
			base.Item.GetGlobalItem<CalamityGlobalItem>().MaxCharge = 500f;
			base.Item.GetGlobalItem<CalamityGlobalItem>().ChargePerUse = 0.1f;
			if(ModContent.GetInstance<BalanceConfig>().radiant != 1f) base.Item.damage = (int)MathHelper.Max(ModContent.GetInstance<BalanceConfig>().radiant * (float)base.Item.damage, 1);
			if(ModContent.GetInstance<BalanceConfig>().soulEssence != 1) this.scytheSoulCharge = (int)MathHelper.Clamp(ModContent.GetInstance<BalanceConfig>().soulEssence * this.scytheSoulCharge, 0, 5);
		}
		public override void AddRecipes() {
			System.Func<bool> condition;
			if(ModLoader.TryGetMod("CalamityMod", out Mod calamity)) CreateRecipe().AddIngredient(calamity.Find<ModItem>("MysteriousCircuitry").Type, 15).AddIngredient(calamity.Find<ModItem>("DubiousPlating").Type, 5).AddRecipeGroup("AnyMythrilBar", 10).AddIngredient(549, 20).AddCondition(ArsenalTierGatedRecipe.ConstructRecipeCondition(2, out condition), condition).AddTile(134).Register();
		}
		public override void ModifyTooltips(System.Collections.Generic.List<TooltipLine> tooltips) {
			base.ModifyTooltips(tooltips);
			CalamityGlobalItem.InsertKnowledgeTooltip(tooltips, 2, false);
		}
	}
}