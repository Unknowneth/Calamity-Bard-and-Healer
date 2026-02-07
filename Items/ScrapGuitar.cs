using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.Audio;
using ThoriumMod;
using ThoriumMod.Empowerments;
using ThoriumMod.Items;
using ThoriumMod.Items.BardItems;
using CalamityMod.CustomRecipes;
using CalamityMod.Items;

namespace CalamityBardHealer.Items
{
	public class ScrapGuitar : BardItem
	{
		public override BardInstrumentType InstrumentType => BardInstrumentType.String;
		public override void SetStaticDefaults() {
			base.Item.ResearchUnlockCount = 1;
			this.Empowerments.AddInfo<Defense>(2, 0);
			this.Empowerments.AddInfo<InvincibilityFrames>(1, 0);
		}
		public override void SetBardDefaults() {
			base.Item.width = 62;
			base.Item.height = 56;
			base.Item.holdStyle = 5;
			base.Item.useStyle = 12;
			base.Item.useTime = 18;
			base.Item.useAnimation = 18;
			base.Item.damage = 27;
			if(ModContent.GetInstance<BalanceConfig>().bard != 1f) base.Item.damage = (int)MathHelper.Max(ModContent.GetInstance<BalanceConfig>().bard * (float)base.Item.damage, 1);
			base.Item.autoReuse = true;
			base.Item.knockBack = 0.25f;
			base.Item.value = Item.sellPrice(silver: 80);
			base.Item.rare = 3;
			base.Item.noMelee = true;
			base.Item.shootSpeed = 12f;
			base.Item.shoot = ModContent.ProjectileType<Projectiles.ScrapGuitar>();
			base.Item.DamageType = BardDamage.Instance;
			base.Item.UseSound = SoundID.Item47;
			base.InspirationCost = 1;
			base.Item.GetGlobalItem<CalamityGlobalItem>().UsesCharge = true;
			base.Item.GetGlobalItem<CalamityGlobalItem>().MaxCharge = 100f;
			base.Item.GetGlobalItem<CalamityGlobalItem>().ChargePerUse = 0.05f;
		}
		public override bool BardShoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
			if(Main.myPlayer == player.whoAmI) {
				int p = Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI, velocity.ToRotation());
				NetMessage.SendData(27, -1, -1, null, p);
			}
			return false;
		}
		public override void AddRecipes() {
			System.Func<bool> condition;
			if(ModLoader.TryGetMod("CalamityMod", out Mod calamity)) CreateRecipe().AddIngredient(calamity.Find<ModItem>("MysteriousCircuitry").Type, 5).AddIngredient(calamity.Find<ModItem>("DubiousPlating").Type, 7).AddIngredient(calamity.Find<ModItem>("AerialiteBar").Type, 9).AddIngredient(calamity.Find<ModItem>("SeaPrism").Type, 7).AddCondition(ArsenalTierGatedRecipe.ConstructRecipeCondition(1, out condition), condition).AddTile(16).Register();
		}
		public override void HoldItemFrame(Player player) => player.itemLocation += new Vector2(-10, 8) * player.Directions;
		public override void UseItemFrame(Player player) => this.HoldItemFrame(player);
		public override void BardModifyTooltips(System.Collections.Generic.List<TooltipLine> list) => CalamityGlobalItem.InsertKnowledgeTooltip(list, 1, false);
	}
}