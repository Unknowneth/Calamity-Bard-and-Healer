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
	public class SongoftheElements : BardItem
	{
		public override BardInstrumentType InstrumentType => BardInstrumentType.Wind;
		public override void SetStaticDefaults() {
			base.Item.ResearchUnlockCount = 1;
			this.Empowerments.AddInfo<ResourceMaximum>(3, 0);
			this.Empowerments.AddInfo<ResourceRegen>(3, 0);
			this.Empowerments.AddInfo<CriticalStrikeChance>(3, 0);
			this.Empowerments.AddInfo<FlightTime>(3, 0);
		}
		public override void SetBardDefaults() {
			base.Item.width = 70;
			base.Item.height = 26;
			base.Item.useStyle = 5;
			if(!ModLoader.HasMod("Look")) base.Item.holdStyle = 3;
			base.Item.useTime = 28;
			base.Item.useAnimation = 28;
			base.Item.damage = 111;
			if(ModContent.GetInstance<BalanceConfig>().bard != 1f) base.Item.damage = (int)MathHelper.Max(ModContent.GetInstance<BalanceConfig>().bard * (float)base.Item.damage, 1);
			base.Item.crit = 16;
			base.Item.autoReuse = true;
			base.Item.knockBack = 1.5f;
			base.Item.value = Item.sellPrice(gold: 22);
			base.Item.rare = 11;
			base.Item.noMelee = true;
			base.Item.shootSpeed = 8f;
			base.Item.shoot = ModContent.ProjectileType<Projectiles.ElementalSong>();
			base.Item.DamageType = BardDamage.Instance;
			base.Item.UseSound = new SoundStyle?(ThoriumSounds.Flute_Sound);
			base.InspirationCost = 1;
		}
		public override bool BardShoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
			int random = Main.rand.Next(91);
			if(Main.myPlayer == player.whoAmI) for(int i = 0; i < 3; i++) {
				int p = Projectile.NewProjectile(source, position + Vector2.Normalize(velocity) * 70f, velocity, type, damage, knockback, player.whoAmI, velocity.Length(), i * 30f + random, player.direction * player.gravDir);
				if(Main.projectile[p].ai[1] > 90f) Main.projectile[p].ai[1] -= 90f;
				NetMessage.SendData(27, -1, -1, null, p);
			}
			return false;
		}
		public override void AddRecipes() {
			if(ModLoader.TryGetMod("CalamityMod", out Mod calamity)) CreateRecipe().AddIngredient(ModContent.ItemType<Items.SongoftheAncients>()).AddIngredient(3467, 5).AddIngredient(calamity.Find<ModItem>("LifeAlloy").Type, 5).AddIngredient(calamity.Find<ModItem>("GalacticaSingularity").Type, 5).AddTile(412).Register();
		}
		public override Color? GetAlpha(Color lightColor) => Color.White;
		public override void UseItemFrame(Player player) => this.HoldItemFrame(player);
		public override void HoldItemFrame(Player player) => player.itemLocation -= (new Vector2(ModLoader.HasMod("Look") ? 4 : 6) * player.Directions).RotatedBy(player.itemRotation);
	}
}