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

namespace CalamityBardHealer.Items
{
	public class HarpY : BardItem
	{
		public override BardInstrumentType InstrumentType => BardInstrumentType.String;
		public override void SetStaticDefaults() {
			base.Item.ResearchUnlockCount = 1;
			this.Empowerments.AddInfo<LifeRegeneration>(2, 0);
		}
		public override void SetBardDefaults() {
			base.Item.width = 54;
			base.Item.height = 44;
			base.Item.useStyle = 5;
			if(!ModLoader.HasMod("Look")) base.Item.holdStyle = 3;
			base.Item.useTime = 12;
			base.Item.useAnimation = 12;
			base.Item.damage = 28;
			if(ModContent.GetInstance<BalanceConfig>().bard != 1f) base.Item.damage = (int)MathHelper.Max(ModContent.GetInstance<BalanceConfig>().bard * (float)base.Item.damage, 1);
			base.Item.autoReuse = true;
			base.Item.knockBack = 0.5f;
			base.Item.value = Item.sellPrice(silver: 80);
			base.Item.rare = 3;
			base.Item.noMelee = true;
			base.Item.shootSpeed = 10f;
			base.Item.shoot = ModContent.ProjectileType<Projectiles.HarpY>();
			base.Item.DamageType = BardDamage.Instance;
			base.Item.UseSound = SoundID.Item26;
			base.InspirationCost = 2;
		}
		public override bool BardShoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
			if(Main.myPlayer == player.whoAmI) for(int i = -1; i <= 1; i++) {
				int p = Projectile.NewProjectile(source, position, velocity.RotatedBy(MathHelper.PiOver4 * 0.25f * i), type, damage, knockback, player.whoAmI);
				NetMessage.SendData(27, -1, -1, null, p);
			}
			return false;
		}
		public override void AddRecipes() {
			if(ModLoader.TryGetMod("CalamityMod", out Mod calamity)) CreateRecipe().AddIngredient(calamity.Find<ModItem>("AerialiteBar").Type, 9).AddIngredient(824, 3).AddTile(305).Register();
		}
		public override void UseItemFrame(Player player) {
			if(ModLoader.HasMod("Look") && player.compositeFrontArm.enabled && player.itemAnimation > 0) {
				float attackTime = (float)player.itemAnimation / (float)player.itemAnimationMax;
				player.compositeFrontArm.stretch = (attackTime > 0.66f || attackTime < 0.33f) ? Player.CompositeArmStretchAmount.ThreeQuarters : Player.CompositeArmStretchAmount.Quarter;
				player.compositeFrontArm.rotation -= attackTime * MathHelper.PiOver4 * 0.1f * player.direction * player.gravDir;
			}
			this.HoldItemFrame(player);
		}
		public override void HoldItemFrame(Player player) => player.itemLocation += (new Vector2(-16, 4) * player.Directions).RotatedBy(player.itemRotation);
	}
}