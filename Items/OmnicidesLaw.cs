using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod;
using ThoriumMod.Items;
using ThoriumMod.Items.HealerItems;

namespace CalamityBardHealer.Items
{
	public class OmnicidesLaw : ThoriumItem
	{
		public override void SetStaticDefaults() => Item.staff[base.Item.type] = true;
		public override void SetDefaults() {
			base.Item.DamageType = HealerDamage.Instance;
			base.Item.damage = 64;
			if(ModContent.GetInstance<BalanceConfig>().radiant != 1f) base.Item.damage = (int)MathHelper.Max(ModContent.GetInstance<BalanceConfig>().radiant * (float)base.Item.damage, 1);
			base.Item.mana = 18;
			this.radiantLifeCost = 12;
			this.isHealer = true;
			base.Item.width = 60;
			base.Item.height = 60;
			base.Item.useTime = 16;
			base.Item.useAnimation = 16;
			base.Item.knockBack = 2f;
			base.Item.value = Item.sellPrice(gold: 12);
			base.Item.rare = 8;
			base.Item.UseSound = SoundID.Item8;
			base.Item.useStyle = 5;
			base.Item.autoReuse = true;
			base.Item.noMelee = true;
			base.Item.shoot = ModContent.ProjectileType<Projectiles.OmnicidesLaw>();
			base.Item.shootSpeed = 8f;
		}
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
			if(Main.myPlayer == player.whoAmI) for(int i = 0; i < Main.rand.Next(3, 5); i++) {
				float rotOff = Main.rand.Next(-99, 100) * 0.015f;
				int p = Projectile.NewProjectile(source, position + Vector2.Normalize(velocity) * base.Item.Size.Length(), velocity.RotatedBy(rotOff * -0.5f), type, damage, knockback, player.whoAmI, rotOff, -Main.rand.Next(20) - 1f);
				NetMessage.SendData(27, -1, -1, null, p);
			}
			return false;
		}
	}
}