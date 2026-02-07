using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.Audio;
using Terraria.ModLoader;
using ThoriumMod;
using ThoriumMod.Sounds;
using ThoriumMod.Empowerments;
using ThoriumMod.Items;
using ThoriumMod.Items.BardItems;

namespace CalamityBardHealer.Items
{
	public class InfestedCastanet : BardItem
	{
		private int comboCounter = 0;
		public override BardInstrumentType InstrumentType => BardInstrumentType.Percussion;
		public override void SetStaticDefaults() {
			base.Item.ResearchUnlockCount = 1;
			this.Empowerments.AddInfo<JumpHeight>(1, 0);
		}
		public override void SetBardDefaults() {
			base.Item.width = 30;
			base.Item.height = 32;
			base.Item.useStyle = 13;
			base.Item.useTime = 9;
			base.Item.useAnimation = 9;
			base.Item.damage = 18;
			if(ModContent.GetInstance<BalanceConfig>().bard != 1f) base.Item.damage = (int)MathHelper.Max(ModContent.GetInstance<BalanceConfig>().bard * (float)base.Item.damage, 1);
			base.Item.autoReuse = true;
			base.Item.knockBack = 0.5f;
			base.Item.value = Item.sellPrice(silver: 40);
			base.Item.rare = 2;
			base.Item.noMelee = true;
			base.Item.noUseGraphic = true;
			base.Item.shootSpeed = 8f;
			base.Item.shoot = ModContent.ProjectileType<Projectiles.InfestedCastanet>();
			base.Item.DamageType = BardDamage.Instance;
			base.Item.UseSound = new SoundStyle?(ThoriumSounds.Castanet);
			base.InspirationCost = 1;
		}
		public override bool BardShoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
			if(Main.myPlayer == player.whoAmI) {
				int p = Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI, comboCounter, player.itemTimeMax * 2);
				NetMessage.SendData(27, -1, -1, null, p);
			}
			if(++comboCounter > 1) comboCounter = 0;
			return false;
		}
	}
}