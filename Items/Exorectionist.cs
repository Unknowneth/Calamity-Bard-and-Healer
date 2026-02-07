using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod;
using ThoriumMod.Items;
using ThoriumMod.Items.HealerItems;
using CalamityMod.CustomRecipes;
using CalamityMod.Items;

namespace CalamityBardHealer.Items
{
	public class Exorectionist : ThoriumItem
	{
		public override void SetDefaults() {
			this.healType = HealType.Ally;
			this.healAmount = 14;
			if(ModContent.GetInstance<BalanceConfig>().healing != 1) this.healAmount = (int)MathHelper.Max(ModContent.GetInstance<BalanceConfig>().healing * (float)this.healAmount, 1);
			this.healDisplay = true;
			this.isHealer = true;
			base.Item.DamageType = ThoriumDamageBase<HealerTool>.Instance;
			base.Item.mana = 40;
			base.Item.width = 72;
			base.Item.height = 30;
			base.Item.useTime = 1;
			base.Item.useAnimation = 1;
			base.Item.useStyle = 5;
			base.Item.noMelee = true;
			base.Item.knockBack = 0f;
			base.Item.value = Item.sellPrice(gold: 24);
			if(ModLoader.TryGetMod("CalamityMod", out Mod calamity)) base.Item.rare = calamity.Find<ModRarity>("CosmicPurple").Type;
			else base.Item.rare = 11;
			base.Item.UseSound = SoundID.Item94;
			base.Item.autoReuse = true;
			base.Item.shoot = ModContent.ProjectileType<Projectiles.Exorectionist>();
			base.Item.shootSpeed = 1f;
			base.Item.GetGlobalItem<CalamityGlobalItem>().UsesCharge = true;
			base.Item.GetGlobalItem<CalamityGlobalItem>().MaxCharge = 250f;
			base.Item.GetGlobalItem<CalamityGlobalItem>().ChargePerUse = 0.025f;
		}
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
			if(base.Item.GetGlobalItem<CalamityGlobalItem>().Charge <= 0f) return false;
			base.Item.GetGlobalItem<CalamityGlobalItem>().Charge -= base.Item.GetGlobalItem<CalamityGlobalItem>().ChargePerUse;
			return player.ownedProjectileCounts[type] == 0 && base.Item.GetGlobalItem<CalamityGlobalItem>().Charge > 0f;
		}
		public override void AddRecipes() {
			System.Func<bool> condition;
			if(ModLoader.TryGetMod("CalamityMod", out Mod calamity)) CreateRecipe().AddIngredient(calamity.Find<ModItem>("MysteriousCircuitry").Type, 15).AddIngredient(calamity.Find<ModItem>("DubiousPlating").Type, 25).AddIngredient(calamity.Find<ModItem>("CosmiliteBar").Type, 8).AddIngredient(calamity.Find<ModItem>("AscendantSpiritEssence").Type, 2).AddCondition(ArsenalTierGatedRecipe.ConstructRecipeCondition(5, out condition), condition).AddTile(calamity.Find<ModTile>("CosmicAnvil").Type).Register();
		}
		public override void ModifyTooltips(System.Collections.Generic.List<TooltipLine> tooltips) {
			base.ModifyTooltips(tooltips);
			CalamityGlobalItem.InsertKnowledgeTooltip(tooltips, 5, false);
		}
		public override void UseItemFrame(Player player) => this.HoldItemFrame(player);
		public override void HoldItemFrame(Player player) => player.itemLocation -= (new Vector2(16, 0) * player.Directions).RotatedBy(player.itemRotation);
	}
}