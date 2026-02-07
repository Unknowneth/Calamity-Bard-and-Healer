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
	public class StarCluster : BardItem
	{
		public override BardInstrumentType InstrumentType => BardInstrumentType.Brass;
		public override void SetStaticDefaults() {
			base.Item.ResearchUnlockCount = 1;
			this.Empowerments.AddInfo<Damage>(3, 0);
			this.Empowerments.AddInfo<FlatDamage>(2, 0);
		}
		public override void SetBardDefaults() {
			base.Item.width = 58;
			base.Item.height = 36;
			base.Item.useStyle = 5;
			if(!ModLoader.HasMod("Look")) base.Item.holdStyle = 3;
			base.Item.useTime = 10;
			base.Item.useAnimation = 10;
			base.Item.damage = 34;
			if(ModContent.GetInstance<BalanceConfig>().bard != 1f) base.Item.damage = (int)MathHelper.Max(ModContent.GetInstance<BalanceConfig>().bard * (float)base.Item.damage, 1);
			base.Item.autoReuse = true;
			base.Item.knockBack = 3.5f;
			base.Item.value = Item.sellPrice(gold: 9, silver: 60);
			base.Item.rare = 7;
			base.Item.noMelee = true;
			base.Item.shootSpeed = 6f;
			base.Item.shoot = ModContent.ProjectileType<Projectiles.StarCluster>();
			base.Item.DamageType = BardDamage.Instance;
			base.Item.UseSound = new SoundStyle?(ThoriumSounds.Bard_Horn);
			base.Item.channel = true;
			base.InspirationCost = 1;
		}
		public override Color? GetAlpha(Color lightColor) => Color.White;
		public override bool BardShoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
			if(Main.myPlayer == player.whoAmI) {
				int cluster = Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
				NetMessage.SendData(27, -1, -1, null, cluster);
			}
			return false;
		}
		public override void UseItemFrame(Player player) => this.HoldItemFrame(player);
		public override void HoldItemFrame(Player player) => player.itemLocation += (new Vector2(ModLoader.HasMod("Look") ? -4 : -6, ModLoader.HasMod("Look") ? -2 : -4) * player.Directions).RotatedBy(player.itemRotation);
	}
}