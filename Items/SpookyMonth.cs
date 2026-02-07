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

namespace CalamityBardHealer.Items
{
	public class SpookyMonth : BardItem
	{
		public override BardInstrumentType InstrumentType => BardInstrumentType.Brass;
		public override void SetStaticDefaults() {
			base.Item.ResearchUnlockCount = 1;
			this.Empowerments.AddInfo<CriticalStrikeChance>(3, 0);
			this.Empowerments.AddInfo<Damage>(4, 0);
			this.Empowerments.AddInfo<FlatDamage>(4, 0);
		}
		public override void SetBardDefaults() {
			base.Item.width = 68;
			base.Item.height = 34;
			base.Item.useStyle = 5;
			if(!ModLoader.HasMod("Look")) base.Item.holdStyle = 3;
			base.Item.useTime = 20;
			base.Item.useAnimation = 20;
			base.Item.damage = 185;
			if(ModContent.GetInstance<BalanceConfig>().bard != 1f) base.Item.damage = (int)MathHelper.Max(ModContent.GetInstance<BalanceConfig>().bard * (float)base.Item.damage, 1);
			base.Item.autoReuse = true;
			base.Item.knockBack = 2.5f;
			if(ModLoader.TryGetMod("CalamityMod", out Mod calamity)) base.Item.rare = calamity.Find<ModRarity>("CosmicPurple").Type;
			else base.Item.rare = 11;
			base.Item.value = Item.sellPrice(gold: 28);
			base.Item.noMelee = true;
			base.Item.shootSpeed = 13f;
			base.Item.shoot = ModContent.ProjectileType<Projectiles.SpookyMonth>();
			base.Item.DamageType = BardDamage.Instance;
			base.Item.UseSound = new SoundStyle?(ThoriumSounds.Bard_Horn);
			base.InspirationCost = 1;
		}
		public override bool BardShoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
			if(Main.myPlayer == player.whoAmI) for(int i = 0; i < 6; i++) {
				float rotOff = Main.rand.Next(-99, 100) * 0.015f;
				int p = Projectile.NewProjectile(source, position + Vector2.Normalize(velocity) * 72f, velocity.RotatedBy(rotOff * -0.5f), type, damage, knockback, player.whoAmI, rotOff, -Main.rand.Next(20) - 1f);
				NetMessage.SendData(27, -1, -1, null, p);
			}
			return false;
		}
		public override void AddRecipes() {
			if(ModLoader.TryGetMod("CalamityMod", out Mod calamity) && ModLoader.TryGetMod("ThoriumMod", out Mod thorium)) CreateRecipe().AddIngredient(thorium.Find<ModItem>("BoneTrumpet").Type).AddIngredient(thorium.Find<ModItem>("HotHorn").Type).AddIngredient(calamity.Find<ModItem>("CosmiliteBar").Type, 8).AddIngredient(calamity.Find<ModItem>("NightmareFuel").Type, 20).AddTile(calamity.Find<ModTile>("CosmicAnvil").Type).Register();
		}
	}
}