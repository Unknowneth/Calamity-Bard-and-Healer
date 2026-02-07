using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ModLoader;
using ThoriumMod;
using ThoriumMod.Empowerments;
using ThoriumMod.Items;
using ThoriumMod.Items.BardItems;

namespace CalamityBardHealer.Items
{
	public class CrystalHydraulophone : BardItem
	{
		public override BardInstrumentType InstrumentType => BardInstrumentType.String;
		public override void SetStaticDefaults() {
			base.Item.ResearchUnlockCount = 1;
			this.Empowerments.AddInfo<Defense>(2, 0);
			this.Empowerments.AddInfo<InvincibilityFrames>(1, 0);
		}
		public override void SetBardDefaults() {
			base.Item.width = 48;
			base.Item.height = 24;
			base.Item.holdStyle = 5;
			base.Item.useStyle = 12;
			base.Item.useTime = 28;
			base.Item.useAnimation = 28;
			base.Item.damage = 17;
			if(ModContent.GetInstance<BalanceConfig>().bard != 1f) base.Item.damage = (int)MathHelper.Max(ModContent.GetInstance<BalanceConfig>().bard * (float)base.Item.damage, 1);
			base.Item.autoReuse = true;
			base.Item.knockBack = 0.5f;
			base.Item.value = Item.sellPrice(silver: 80);
			base.Item.rare = 2;
			base.Item.noMelee = true;
			base.Item.shootSpeed = 5f;
			base.Item.shoot = ModContent.ProjectileType<Projectiles.CrystalHydraulophone>();
			base.Item.DamageType = BardDamage.Instance;
			base.Item.UseSound = new SoundStyle("CalamityBardHealer/Sounds/CrystalHydraulophone") { SoundLimitBehavior = SoundLimitBehavior.ReplaceOldest, MaxInstances = 5 };
			base.Item.scale = 0.9f;
			base.InspirationCost = 1;
		}
		public override bool BardShoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
			int j = Main.rand.Next(3);
			if(Main.myPlayer == player.whoAmI) for(int i = -1; i <= 1; i++) if(i != 0) {
				j += i;
				if(j > 2) j = 0;
				else if(j < 0) j = 2;
				if(j > 1) position = new Vector2(12);
				else if(j < 1) position = new Vector2(24, 10);
				else position = new Vector2(36, 8);
				position *= base.Item.scale;
				int p = Projectile.NewProjectile(source, player.MountedCenter + (position * player.Directions).RotatedBy(player.fullRotation), -Vector2.UnitY.RotatedBy(MathHelper.ToRadians(i * 5f) + player.fullRotation) * player.gravDir * velocity.Length(), type, damage, knockback, player.whoAmI, 0f, 30f);
				NetMessage.SendData(27, -1, -1, null, p);
			}
			return false;
		}
		public override void AddRecipes() {
			if(ModLoader.TryGetMod("CalamityMod", out Mod calamity)) CreateRecipe().AddIngredient(calamity.Find<ModItem>("PearlShard").Type, 4).AddIngredient(calamity.Find<ModItem>("SeaPrism").Type, 8).AddIngredient(calamity.Find<ModItem>("Navystone").Type, 4).AddTile(16).Register();
		}
		public override void UseItemFrame(Player player) {
			float attackTime = (float)player.itemAnimation / (float)player.itemAnimationMax;
			if(attackTime < 0.3f && attackTime > 0.1f) player.compositeBackArm.stretch = Player.CompositeArmStretchAmount.Quarter;
			else if(attackTime < 0.8f && attackTime > 0.5f) player.compositeBackArm.stretch = Player.CompositeArmStretchAmount.Full;
			else player.compositeBackArm.stretch = Player.CompositeArmStretchAmount.ThreeQuarters;
			player.compositeBackArm.rotation -= Vector2.UnitY.RotatedBy(attackTime * MathHelper.Pi).X * MathHelper.PiOver4 / 3f * player.direction;
			this.HoldItemFrame(player);
		}
		public override void HoldItemFrame(Player player) => player.itemLocation += new Vector2(-4f, 4f) * player.Directions;
	}
}