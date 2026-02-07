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
	public class TectonicPlates : BardItem
	{
		public override BardInstrumentType InstrumentType => BardInstrumentType.Percussion;
		public override void SetStaticDefaults() {
			base.Item.ResearchUnlockCount = 1;
			this.Empowerments.AddInfo<MovementSpeed>(3, 0);
			this.Empowerments.AddInfo<FlightTime>(3, 0);
			this.Empowerments.AddInfo<AquaticAbility>(4, 0);
		}
		public override void SetBardDefaults() {
			base.Item.width = 56;
			base.Item.height = 26;
			base.Item.holdStyle = 5;
			base.Item.useStyle = 12;
			base.Item.useTime = 14;
			base.Item.useAnimation = 14;
			base.Item.damage = 92;
			if(ModContent.GetInstance<BalanceConfig>().bard != 1f) base.Item.damage = (int)MathHelper.Max(ModContent.GetInstance<BalanceConfig>().bard * (float)base.Item.damage, 1);
			base.Item.autoReuse = true;
			base.Item.knockBack = 2.75f;
			base.Item.value = Item.sellPrice(gold: 12);
			base.Item.rare = 8;
			base.Item.noMelee = true;
			base.Item.shootSpeed = 1f;
			base.Item.shoot = ModContent.ProjectileType<Projectiles.TectonicPlate>();
			base.Item.DamageType = BardDamage.Instance;
			base.Item.UseSound = new SoundStyle?(ThoriumSounds.Bongo);
			base.InspirationCost = 1;
		}
		public override bool BardShoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
			if(Main.myPlayer == player.whoAmI) for(int i = -1; i <= 1; i++) if(i != 0) {
				int p = Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI, i, Main.MouseWorld.X, Main.MouseWorld.Y);
				NetMessage.SendData(27, -1, -1, null, p);
			}
			return false;
		}
		public override void AddRecipes() {
			if(ModLoader.TryGetMod("CalamityMod", out Mod calamity)) CreateRecipe().AddIngredient(calamity.Find<ModItem>("ScoriaBar").Type, 10).AddTile(134).Register();
		}
		public override void UseItemFrame(Player player) {
			float attackTime = (float)player.itemAnimation / (float)player.itemAnimationMax;
			if(attackTime < 0.3f && attackTime > 0.1f) player.compositeBackArm.stretch = Player.CompositeArmStretchAmount.Quarter;
			else if(attackTime < 0.8f && attackTime > 0.5f) player.compositeBackArm.stretch = Player.CompositeArmStretchAmount.Full;
			else player.compositeBackArm.stretch = Player.CompositeArmStretchAmount.ThreeQuarters;
			player.compositeBackArm.rotation -= Vector2.UnitY.RotatedBy(attackTime * MathHelper.Pi).X * MathHelper.PiOver4 / 3f * player.direction;
			this.HoldItemFrame(player);
		}
		public override void HoldItemFrame(Player player) => player.itemLocation += new Vector2(-13f, 13f) * player.Directions;
		public override Color? GetAlpha(Color lightColor) => Color.White;
	}
}