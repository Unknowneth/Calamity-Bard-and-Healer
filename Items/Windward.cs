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
	public class Windward : BardItem
	{
		public override BardInstrumentType InstrumentType => BardInstrumentType.Wind;
		public override void SetStaticDefaults() {
			base.Item.ResearchUnlockCount = 1;
			this.Empowerments.AddInfo<ResourceRegen>(2, 0);
		}
		public override void SetBardDefaults() {
			base.Item.width = 66;
			base.Item.height = 16;
			base.Item.useStyle = 5;
			if(!ModLoader.HasMod("Look")) base.Item.holdStyle = 3;
			base.Item.useTime = 24;
			base.Item.useAnimation = 24;
			base.Item.damage = 35;
			if(ModContent.GetInstance<BalanceConfig>().bard != 1f) base.Item.damage = (int)MathHelper.Max(ModContent.GetInstance<BalanceConfig>().bard * (float)base.Item.damage, 1);
			base.Item.autoReuse = true;
			base.Item.knockBack = 0.5f;
			base.Item.value = Item.sellPrice(silver: 80);
			base.Item.rare = 3;
			base.Item.noMelee = true;
			base.Item.shootSpeed = 13f;
			base.Item.shoot = ModContent.ProjectileType<Projectiles.Windward>();
			base.Item.DamageType = BardDamage.Instance;
			base.Item.UseSound = new SoundStyle?(ThoriumSounds.Flute_Sound);
			base.InspirationCost = 1;
		}
		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
			Vector2 muzzleOffset = Vector2.Normalize(velocity) * 70f;
			if(Collision.CanHitLine(position, 0, 0, position + muzzleOffset, 0, 0)) position += muzzleOffset;
		}
		public override bool BardShoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
			if(Main.myPlayer == player.whoAmI) for(int i = -1; i <= 1; i++) if(i != 0) {
				int p = Projectile.NewProjectile(source, position, velocity.RotatedBy(MathHelper.PiOver4 * 0.25f * i), type, damage, knockback, player.whoAmI);
				NetMessage.SendData(27, -1, -1, null, p);
			}
			return false;
		}
		public override void AddRecipes() {
			if(ModLoader.TryGetMod("CalamityMod", out Mod calamity)) CreateRecipe().AddIngredient(calamity.Find<ModItem>("AerialiteBar").Type, 7).AddIngredient(824, 3).AddTile(305).Register();
		}
	}
}