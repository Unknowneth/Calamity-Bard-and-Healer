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
	public class ReturntoSludge : BardItem
	{
		private int sludge = -1;
		public override BardInstrumentType InstrumentType => BardInstrumentType.Brass;
		public override void SetStaticDefaults() {
			base.Item.ResearchUnlockCount = 1;
			this.Empowerments.AddInfo<Damage>(2, 0);
		}
		public override void SetBardDefaults() {
			base.Item.width = 50;
			base.Item.height = 26;
			base.Item.useStyle = 5;
			if(!ModLoader.HasMod("Look")) base.Item.holdStyle = 3;
			base.Item.useTime = 12;
			base.Item.useAnimation = 12;
			base.Item.damage = 23;
			if(ModContent.GetInstance<BalanceConfig>().bard != 1f) base.Item.damage = (int)MathHelper.Max(ModContent.GetInstance<BalanceConfig>().bard * (float)base.Item.damage, 1);
			base.Item.autoReuse = true;
			base.Item.knockBack = 0.5f;
			base.Item.value = Item.sellPrice(gold: 2, silver: 40);
			base.Item.rare = 4;
			base.Item.noMelee = true;
			base.Item.shootSpeed = 6f;
			base.Item.shoot = ModContent.ProjectileType<Projectiles.SludgeBomb>();
			base.Item.DamageType = BardDamage.Instance;
			base.Item.UseSound = new SoundStyle?(ThoriumSounds.Bard_Horn);
			base.Item.channel = true;
			base.InspirationCost = 1;
		}
		public override Color? GetAlpha(Color lightColor) {
			lightColor.A = 200;
			return lightColor;
		}
		public override void BardHoldItem(Player player) {
			if(!player.channel || (sludge >= 0 && !Main.projectile[sludge].active)) sludge = -1;
		}
		public override bool BardShoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
			if(Main.myPlayer == player.whoAmI) if(sludge < 0) {
				sludge = Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
				NetMessage.SendData(27, -1, -1, null, sludge);
			}
			else if(Main.projectile[sludge].ai[0] < 10f) {
				Main.projectile[sludge].ai[0]++;
				Main.projectile[sludge].damage += damage;
				NetMessage.SendData(27, -1, -1, null, sludge);
				for(int i = 1; i <= 5; i++) {
					int abyssal = Dust.NewDust(Main.projectile[sludge].position, Main.projectile[sludge].width, Main.projectile[sludge].height, 173, 0f, 0f, 100, default(Color), 0.7f + Main.projectile[sludge].ai[0] * 0.1f);
					Main.dust[abyssal].noGravity = true;
					Main.dust[abyssal].velocity *= i;
				}
			}
			else {
				sludge = -1;
				player.channel = false;
			}
			return false;
		}
		public override void AddRecipes() {
			if(ModLoader.TryGetMod("CalamityMod", out Mod calamity)) CreateRecipe().AddIngredient(calamity.Find<ModItem>("PurifiedGel").Type, 15).AddIngredient(calamity.Find<ModItem>("BlightedGel").Type, 15).AddTile(calamity.Find<ModTile>("StaticRefiner").Type).Register();
		}
	}
}