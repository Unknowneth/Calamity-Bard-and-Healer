using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.Audio;
using ThoriumMod;
using ThoriumMod.Sounds;
using ThoriumMod.Empowerments;
using ThoriumMod.Items;
using ThoriumMod.Items.BardItems;
using CalamityMod.CustomRecipes;
using CalamityMod.Items;

namespace CalamityBardHealer.Items
{
	public class ScuffedKeytar : BardItem
	{
		public override BardInstrumentType InstrumentType => BardInstrumentType.Electronic;
		public override void SetStaticDefaults() {
			base.Item.ResearchUnlockCount = 1;
			this.Empowerments.AddInfo<EmpowermentProlongation>(2, 0);
			this.Empowerments.AddInfo<FlatDamage>(3, 0);
		}
		public override void SetBardDefaults() {
			base.Item.width = 62;
			base.Item.height = 56;
			base.Item.holdStyle = 5;
			base.Item.useStyle = 12;
			base.Item.useTime = 26;
			base.Item.useAnimation = 26;
			base.Item.damage = 47;
			if(ModContent.GetInstance<BalanceConfig>().bard != 1f) base.Item.damage = (int)MathHelper.Max(ModContent.GetInstance<BalanceConfig>().bard * (float)base.Item.damage, 1);
			base.Item.autoReuse = true;
			base.Item.knockBack = 0.75f;
			base.Item.value = Item.sellPrice(gold: 4, silver: 80);
			base.Item.rare = 5;
			base.Item.noMelee = true;
			base.Item.shootSpeed = 12f;
			base.Item.shoot = ModContent.ProjectileType<Projectiles.ScuffedKeytar>();
			base.Item.DamageType = BardDamage.Instance;
			base.Item.UseSound = new SoundStyle?(ThoriumSounds.Piano_Sound);
			base.InspirationCost = 1;
			base.Item.GetGlobalItem<CalamityGlobalItem>().UsesCharge = true;
			base.Item.GetGlobalItem<CalamityGlobalItem>().MaxCharge = 200f;
			base.Item.GetGlobalItem<CalamityGlobalItem>().ChargePerUse = 0.04f;
		}
		public override bool BardShoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
			if(Main.myPlayer == player.whoAmI) {
				float rotation = velocity.ToRotation();
				if(rotation > MathHelper.PiOver2) rotation -= MathHelper.Pi;
				else if(rotation < -MathHelper.PiOver2) rotation += MathHelper.Pi;
				int p = Projectile.NewProjectile(source, position, velocity.RotatedBy(MathHelper.PiOver4 * player.direction * 0.4f), type, damage, knockback, player.whoAmI, -player.direction, 0f, rotation);
				NetMessage.SendData(27, -1, -1, null, p);
			}
			return false;
		}
		public override void AddRecipes() {
			System.Func<bool> condition;
			if(ModLoader.TryGetMod("CalamityMod", out Mod calamity)) CreateRecipe().AddIngredient(calamity.Find<ModItem>("MysteriousCircuitry").Type, 15).AddIngredient(calamity.Find<ModItem>("DubiousPlating").Type, 5).AddRecipeGroup("AnyMythrilBar", 10).AddIngredient(548, 20).AddCondition(ArsenalTierGatedRecipe.ConstructRecipeCondition(2, out condition), condition).AddTile(134).Register();
		}
		public override void HoldItemFrame(Player player) => player.itemLocation += new Vector2(-14, 8) * player.Directions;
		public override void UseItemFrame(Player player) => this.HoldItemFrame(player);
		public override void BardModifyTooltips(System.Collections.Generic.List<TooltipLine> list) => CalamityGlobalItem.InsertKnowledgeTooltip(list, 2, false);
	}
}