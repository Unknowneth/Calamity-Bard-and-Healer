using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ModLoader;
using ThoriumMod;
using ThoriumMod.Sounds;
using ThoriumMod.Empowerments;
using ThoriumMod.Items;
using ThoriumMod.Items.BardItems;
using CalamityMod.Items;

namespace CalamityBardHealer.Items
{
	public class Supercluster : BardItem
	{
		public override bool IsLoadingEnabled(Mod mod) => ModLoader.HasMod("CatalystMod");
		public override BardInstrumentType InstrumentType => BardInstrumentType.Brass;
		public override void SetStaticDefaults() {
			base.Item.ResearchUnlockCount = 1;
			this.Empowerments.AddInfo<AttackSpeed>(4, 0);
			this.Empowerments.AddInfo<CriticalStrikeChance>(3, 0);
			this.Empowerments.AddInfo<Damage>(3, 0);
			this.Empowerments.AddInfo<FlatDamage>(3, 0);
		}
		public override void SetBardDefaults() {
			base.Item.width = 68;
			base.Item.height = 34;
			base.Item.useStyle = 5;
			if(!ModLoader.HasMod("Look")) base.Item.holdStyle = 3;
			base.Item.useTime = 20;
			base.Item.useAnimation = 20;
			base.Item.damage = 201;
			if(ModContent.GetInstance<BalanceConfig>().bard != 1f) base.Item.damage = (int)MathHelper.Max(ModContent.GetInstance<BalanceConfig>().bard * (float)base.Item.damage, 1);
			base.Item.autoReuse = true;
			base.Item.knockBack = 2.5f;
			if(ModLoader.TryGetMod("CatalystMod", out Mod catalyst)) {
				base.Item.rare = catalyst.Find<ModRarity>("SuperbossRarity").Type;
				base.Item.shoot = ModContent.ProjectileType<Projectiles.Supercluster>();
			}
			else base.Item.rare = 10;
			base.Item.value = Item.sellPrice(gold: 30);
			base.Item.noMelee = true;
			base.Item.shootSpeed = 13f;
			base.Item.DamageType = BardDamage.Instance;
			base.Item.UseSound = new SoundStyle?(ThoriumSounds.Bard_Horn);
			base.InspirationCost = 1;
			base.Item.GetGlobalItem<CalamityGlobalItem>().CannotBeEnchanted = true;
		}
		public override bool BardShoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
			if(Main.myPlayer == player.whoAmI) {
				int p = Projectile.NewProjectile(source, position + Vector2.Normalize(velocity) * 72f, velocity, type, damage, knockback, player.whoAmI);
				NetMessage.SendData(27, -1, -1, null, p);
			}
			return false;
		}
		public override void AddRecipes() {
			if(ModLoader.TryGetMod("CatalystMod", out Mod catalyst)) CreateRecipe().AddIngredient(ModContent.ItemType<Items.ReturntoSludge>()).AddIngredient(ModContent.ItemType<Items.StarCluster>()).AddIngredient(catalyst.Find<ModItem>("MetanovaBar").Type, 5).AddTile(412).Register();
		}
		public override Color? GetAlpha(Color lightColor) => Color.White;
	}
}