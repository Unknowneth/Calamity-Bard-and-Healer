using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod;
using ThoriumMod.Items;
using ThoriumMod.Items.HealerItems;

namespace CalamityBardHealer.Items
{
	public class Transfiguration : ThoriumItem
	{
		public override void SetStaticDefaults() => Item.staff[base.Item.type] = true;
		public override void SetDefaults() {
			this.healType = HealType.Ally;
			this.healAmount = 7;
			if(ModContent.GetInstance<BalanceConfig>().healing != 1) this.healAmount = (int)MathHelper.Max(ModContent.GetInstance<BalanceConfig>().healing * (float)this.healAmount, 1);
			this.healDisplay = true;
			this.isHealer = true;
			base.Item.DamageType = ThoriumDamageBase<HealerTool>.Instance;
			base.Item.mana = 5;
			base.Item.width = 70;
			base.Item.height = 66;
			base.Item.useTime = 7;
			base.Item.useAnimation = 7;
			base.Item.useStyle = 5;
			base.Item.noMelee = true;
			base.Item.knockBack = 1f;
			base.Item.channel = true;
			base.Item.value = Item.sellPrice(gold: 24);
			if(ModLoader.TryGetMod("CalamityMod", out Mod calamity)) base.Item.rare = calamity.Find<ModRarity>("Turquoise").Type;
			else base.Item.rare = 10;
			base.Item.UseSound = SoundID.Item20;
			base.Item.autoReuse = true;
			base.Item.shoot = ModContent.ProjectileType<Projectiles.Transfiguration>();
			base.Item.shootSpeed = 0f;
		}
		public override void AddRecipes() {
			if(ModLoader.TryGetMod("CalamityMod", out Mod calamity) && ModLoader.TryGetMod("ThoriumMod", out Mod thorium)) CreateRecipe().AddIngredient(thorium.Find<ModItem>("StaffofSol").Type).AddIngredient(calamity.Find<ModItem>("DivineGeode").Type, 5).AddIngredient(calamity.Find<ModItem>("UnholyEssence").Type, 15).AddTile(412).Register();
		}
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) => player.ownedProjectileCounts[type] == 0;
	}
}