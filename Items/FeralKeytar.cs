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
	public class FeralKeytar : BardItem
	{
		public override BardInstrumentType InstrumentType => BardInstrumentType.Electronic;
		public override void SetStaticDefaults() {
			Main.RegisterItemAnimation(base.Item.type, new DrawAnimationVertical(3, 4));
			base.Item.ResearchUnlockCount = 1;
			this.Empowerments.AddInfo<EmpowermentProlongation>(3, 0);
			this.Empowerments.AddInfo<FlatDamage>(4, 0);
			this.Empowerments.AddInfo<Damage>(3, 0);
			this.Empowerments.AddInfo<AttackSpeed>(3, 0);
		}
		public override void SetBardDefaults() {
			base.Item.width = 62;
			base.Item.height = 58;
			base.Item.holdStyle = 5;
			base.Item.useStyle = 12;
			base.Item.useTime = 20;
			base.Item.useAnimation = 20;
			base.Item.damage = 189;
			if(ModContent.GetInstance<BalanceConfig>().bard != 1f) base.Item.damage = (int)MathHelper.Max(ModContent.GetInstance<BalanceConfig>().bard * (float)base.Item.damage, 1);
			base.Item.autoReuse = true;
			base.Item.knockBack = 2.65f;
			base.Item.value = Item.sellPrice(gold: 26);
			if(ModLoader.TryGetMod("CalamityMod", out Mod calamity)) base.Item.rare = calamity.Find<ModRarity>("PureGreen").Type;
			else base.Item.rare = 10;
			base.Item.noMelee = true;
			base.Item.shootSpeed = 12f;
			base.Item.shoot = ModContent.ProjectileType<Projectiles.FeralKeytar>();
			base.Item.DamageType = BardDamage.Instance;
			base.Item.UseSound = new SoundStyle("CalamityBardHealer/Sounds/FeralKeytar") { SoundLimitBehavior = SoundLimitBehavior.ReplaceOldest, MaxInstances = 5 };
			base.InspirationCost = 1;
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
			if(ModLoader.TryGetMod("CalamityMod", out Mod calamity)) CreateRecipe().AddIngredient(ModContent.ItemType<Items.ScuffedKeytar>()).AddIngredient(calamity.Find<ModItem>("RuinousSoul").Type, 2).AddTile(412).Register();
		}
		public override void HoldItemFrame(Player player) => player.itemLocation += new Vector2(-14, 8) * player.Directions;
		public override void UseItemFrame(Player player) => this.HoldItemFrame(player);
		public override Color? GetAlpha(Color lightColor) => Color.White;
	}
}