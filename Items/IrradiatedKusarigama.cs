using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using ThoriumMod;
using ThoriumMod.Items;
using ThoriumMod.Items.HealerItems;

namespace CalamityBardHealer.Items
{
	public class IrradiatedKusarigama : ScytheItem
	{
		public override void SetStaticDefaults() {
			Terraria.ID.ItemID.Sets.SkipsInitialUseSound[Type] = true;
			base.SetStaticDefaultsToScythe();
		}
		public override void SetDefaults() {
			base.SetDefaultsToScythe();
			base.Item.crit = 6;
			this.healType = HealType.AllyAndPlayer;
			this.healAmount = 11;
			if(ModContent.GetInstance<BalanceConfig>().healing != 1) this.healAmount = (int)Microsoft.Xna.Framework.MathHelper.Max(ModContent.GetInstance<BalanceConfig>().healing * (float)this.healAmount, 1);
			this.healDisplay = true;
			base.Item.damage = 876;
			this.scytheSoulCharge = 3;
			base.Item.width = 58;
			base.Item.height = 70;
			base.Item.value = Item.sellPrice(0, 35, 0, 0);
			if(ModLoader.TryGetMod("CalamityMod", out Mod calamity)) base.Item.rare = calamity.Find<ModRarity>("PureGreen").Type;
			else base.Item.rare = 11;
			base.Item.shoot = ModContent.ProjectileType<Projectiles.IrradiatedKusarigama>();
			base.Item.useAnimation = 50;
			base.Item.useTime = 25;
			if(ModContent.GetInstance<BalanceConfig>().radiant != 1f) base.Item.damage = (int)MathHelper.Max(ModContent.GetInstance<BalanceConfig>().radiant * (float)base.Item.damage, 1);
			if(ModContent.GetInstance<BalanceConfig>().soulEssence != 1) this.scytheSoulCharge = (int)MathHelper.Clamp(ModContent.GetInstance<BalanceConfig>().soulEssence * this.scytheSoulCharge, 0, 5);
		}
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
			if(Main.myPlayer == player.whoAmI) {
				int p = Projectile.NewProjectile(source, position, Vector2.Normalize(velocity), type, damage, knockback, player.whoAmI, player.itemAnimation < player.itemAnimationMax ? 0f : 1f);
				NetMessage.SendData(27, -1, -1, null, p);
			}
			return false;
		}
	}
}