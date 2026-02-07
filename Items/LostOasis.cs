using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod;
using ThoriumMod.Items;
using ThoriumMod.Items.HealerItems;

namespace CalamityBardHealer.Items
{
	public class LostOasis : ThoriumItem
	{
		public override void SetStaticDefaults() => Item.staff[base.Item.type] = true;
		public override void SetDefaults() {
			this.healType = HealType.AllyAndPlayer;
			this.healAmount = 11;
			if(ModContent.GetInstance<BalanceConfig>().healing != 1) this.healAmount = (int)Microsoft.Xna.Framework.MathHelper.Max(ModContent.GetInstance<BalanceConfig>().healing * (float)this.healAmount, 1);
			this.healDisplay = true;
			this.isHealer = true;
			base.Item.DamageType = ThoriumDamageBase<HealerTool>.Instance;
			base.Item.mana = 25;
			base.Item.width = 52;
			base.Item.height = 52;
			base.Item.useTime = 42;
			base.Item.useAnimation = 42;
			base.Item.useStyle = 5;
			base.Item.noMelee = true;
			base.Item.knockBack = 1f;
			base.Item.value = Item.sellPrice(silver: 80);
			base.Item.rare = 3;
			base.Item.UseSound = SoundID.Item20;
			base.Item.autoReuse = false;
			base.Item.shoot = ModContent.ProjectileType<Projectiles.LostOasis>();
			base.Item.shootSpeed = 10f;
		}
		public override void AddRecipes() {
			if(ModLoader.TryGetMod("CalamityMod", out Mod calamity)) CreateRecipe().AddIngredient(calamity.Find<ModItem>("PearlShard").Type, 5).AddIngredient(calamity.Find<ModItem>("SeaPrism").Type, 5).AddIngredient(calamity.Find<ModItem>("Navystone").Type, 10).AddTile(16).Register();
		}
	}
}