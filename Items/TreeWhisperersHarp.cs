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
	public class TreeWhisperersHarp : BardItem
	{
		public override BardInstrumentType InstrumentType => BardInstrumentType.String;
		public override void SetStaticDefaults() {
			base.Item.ResearchUnlockCount = 1;
			this.Empowerments.AddInfo<DamageReduction>(3, 0);
			this.Empowerments.AddInfo<Defense>(3, 0);
			this.Empowerments.AddInfo<LifeRegeneration>(3, 0);
		}
		public override void SetBardDefaults() {
			base.Item.width = 50;
			base.Item.height = 78;
			base.Item.useStyle = 5;
			if(!ModLoader.HasMod("Look")) base.Item.holdStyle = 3;
			base.Item.useTime = 42;
			base.Item.useAnimation = 42;
			base.Item.damage = 189;
			if(ModContent.GetInstance<BalanceConfig>().bard != 1f) base.Item.damage = (int)MathHelper.Max(ModContent.GetInstance<BalanceConfig>().bard * (float)base.Item.damage, 1);
			base.Item.autoReuse = true;
			base.Item.knockBack = 0.5f;
			base.Item.value = Item.sellPrice(gold: 24);
			if(ModLoader.TryGetMod("CalamityMod", out Mod calamity)) base.Item.rare = calamity.Find<ModRarity>("Turquoise").Type;
			else base.Item.rare = 10;
			base.Item.noMelee = true;
			base.Item.shootSpeed = 10f;
			base.Item.shoot = ModContent.ProjectileType<Projectiles.WhisperingTrunk>();
			base.Item.DamageType = BardDamage.Instance;
			base.Item.UseSound = SoundID.Item26;
			base.InspirationCost = 2;
		}
		public override bool BardShoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
			if(Main.myPlayer == player.whoAmI) {
				int p = Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI, 0f, 64f);
				NetMessage.SendData(27, -1, -1, null, p);
			}
			return false;
		}
		public override void AddRecipes() {
			if(ModLoader.TryGetMod("CalamityMod", out Mod calamity) && ModLoader.TryGetMod("ThoriumMod", out Mod thorium)) CreateRecipe().AddIngredient(thorium.Find<ModItem>("Nocturne").Type).AddIngredient(ModContent.ItemType<Items.AnahitasArpeggio>()).AddIngredient(calamity.Find<ModItem>("UelibloomBar").Type, 7).AddTile(412).Register();
		}
		public override void UseItemFrame(Player player) {
			if(ModLoader.HasMod("Look") && player.compositeFrontArm.enabled && player.itemAnimation > 0) {
				float attackTime = (float)player.itemAnimation / (float)player.itemAnimationMax;
				player.compositeFrontArm.stretch = (attackTime > 0.66f || attackTime < 0.33f) ? Player.CompositeArmStretchAmount.ThreeQuarters : Player.CompositeArmStretchAmount.Quarter;
				player.compositeFrontArm.rotation -= attackTime * MathHelper.PiOver4 * 0.1f * player.direction * player.gravDir;
			}
			this.HoldItemFrame(player);
		}
		public override void HoldItemFrame(Player player) => player.itemLocation += (new Vector2(22, 4) * -player.Directions).RotatedBy(player.itemRotation);
	}
}