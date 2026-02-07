using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod;
using ThoriumMod.Items;
using ThoriumMod.Items.HealerItems;

namespace CalamityBardHealer.Items
{
	public class StarBirth : ThoriumItem
	{
		public override bool IsLoadingEnabled(Mod mod) => ModLoader.HasMod("CatalystMod");
		public override void SetStaticDefaults() => Item.staff[base.Item.type] = true;
		public override void SetDefaults() {
			this.healType = HealType.AllyAndPlayer;
			this.healAmount = 13;
			if(ModContent.GetInstance<BalanceConfig>().healing != 1) this.healAmount = (int)MathHelper.Max(ModContent.GetInstance<BalanceConfig>().healing * (float)this.healAmount, 1);
			this.healDisplay = true;
			this.isHealer = true;
			base.Item.DamageType = HealerTool.Instance;
			base.Item.mana = 25;
			base.Item.width = 52;
			base.Item.height = 52;
			base.Item.useTime = 42;
			base.Item.useAnimation = 42;
			base.Item.useStyle = 5;
			base.Item.noMelee = true;
			base.Item.knockBack = 1f;
			base.Item.value = Item.sellPrice(gold: 30);
			if(ModLoader.TryGetMod("CatalystMod", out Mod catalyst)) {
				base.Item.rare = catalyst.Find<ModRarity>("SuperbossRarity").Type;
				base.Item.shoot = ModContent.ProjectileType<Projectiles.StarBirth>();
			}
			else base.Item.rare = 10;
			base.Item.UseSound = SoundID.Item20;
			base.Item.autoReuse = false;
			base.Item.shootSpeed = 10f;
		}
		public override void AddRecipes() {
			if(ModLoader.TryGetMod("CatalystMod", out Mod catalyst)) CreateRecipe().AddIngredient(ModContent.ItemType<Items.GelatinTherapy>()).AddIngredient(ModContent.ItemType<Items.LostOasis>()).AddIngredient(catalyst.Find<ModItem>("MetanovaBar").Type, 5).AddTile(412).Register();
		}
		public override Color? GetAlpha(Color lightColor) => Color.White;
	}
}