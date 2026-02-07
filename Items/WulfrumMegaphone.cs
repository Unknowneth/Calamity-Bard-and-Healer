using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using ThoriumMod;
using ThoriumMod.Empowerments;
using ThoriumMod.Items;
using ThoriumMod.Items.BardItems;

namespace CalamityBardHealer.Items
{
	public class WulfrumMegaphone : BardItem
	{
		public override BardInstrumentType InstrumentType => BardInstrumentType.Electronic;
		public override void SetStaticDefaults() {
			base.Item.ResearchUnlockCount = 1;
			this.Empowerments.AddInfo<EmpowermentProlongation>(1, 0);
		}
		public override void SetBardDefaults() {
			base.Item.width = 42;
			base.Item.height = 34;
			base.Item.useStyle = 5;
			if(!ModLoader.HasMod("Look")) base.Item.holdStyle = 3;
			base.Item.useTime = 20;
			base.Item.useAnimation = 20;
			base.Item.damage = 16;
			if(ModContent.GetInstance<BalanceConfig>().bard != 1f) base.Item.damage = (int)MathHelper.Max(ModContent.GetInstance<BalanceConfig>().bard * (float)base.Item.damage, 1);
			base.Item.autoReuse = true;
			base.Item.knockBack = 0.5f;
			base.Item.value = Item.sellPrice(silver: 20);
			base.Item.rare = 1;
			base.Item.noMelee = true;
			base.Item.shootSpeed = 8f;
			base.Item.shoot = ModContent.ProjectileType<Projectiles.WulfrumSoundWave>();
			base.Item.DamageType = BardDamage.Instance;
			base.Item.UseSound = Terraria.ID.SoundID.Item96;
			base.InspirationCost = 2;
		}
		public override void AddRecipes() {
			if(ModLoader.TryGetMod("CalamityMod", out Mod calamity)) CreateRecipe().AddIngredient(calamity.Find<ModItem>("WulfrumMetalScrap").Type, 7).AddTile(16).Register();
		}
		public override void UseItemFrame(Player player) => this.HoldItemFrame(player);
		public override void HoldItemFrame(Player player) => player.itemLocation -= (new Vector2(ModLoader.HasMod("Look") ? 4 : 6) * player.Directions).RotatedBy(player.itemRotation) - (new Vector2(0f, 2f) * player.Directions).RotatedBy(player.fullRotation);
	}
}