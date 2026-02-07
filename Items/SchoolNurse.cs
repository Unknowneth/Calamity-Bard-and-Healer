using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ModLoader;
using ThoriumMod;
using ThoriumMod.Items;
using ThoriumMod.Items.HealerItems;

namespace CalamityBardHealer.Items
{
	public class SchoolNurse : ThoriumItem
	{
		public override void SetStaticDefaults() => Item.staff[base.Item.type] = true;
		public override void SetDefaults() {
			this.healType = HealType.Ally;
			this.healAmount = 7;
			if(ModContent.GetInstance<BalanceConfig>().healing != 1) this.healAmount = (int)MathHelper.Max(ModContent.GetInstance<BalanceConfig>().healing * (float)this.healAmount, 1);
			this.healDisplay = true;
			this.isHealer = true;
			base.Item.DamageType = ThoriumDamageBase<HealerTool>.Instance;
			base.Item.mana = 30;
			base.Item.width = 52;
			base.Item.height = 52;
			base.Item.useTime = 42;
			base.Item.useAnimation = 42;
			base.Item.useStyle = 5;
			base.Item.noMelee = true;
			base.Item.knockBack = 1f;
			base.Item.channel = true;
			base.Item.value = Item.sellPrice(gold: 7, silver: 20);
			base.Item.rare = 5;
			base.Item.UseSound = Terraria.ID.SoundID.Item8;
			base.Item.autoReuse = true;
			base.Item.shoot = ModContent.ProjectileType<Projectiles.IceCube>();
			base.Item.shootSpeed = 9f;
		}
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
			if(Main.myPlayer == player.whoAmI) for(int i = 0; i < 5; i++) {
				int p = Projectile.NewProjectile(source, position + Vector2.Normalize(velocity) * (base.Item.Size.Length() - 9f), velocity + Main.rand.NextVector2Circular(2f, 5f).RotatedBy(velocity.ToRotation()), type, 0, 0f, player.whoAmI, Main.MouseWorld.Y);
				NetMessage.SendData(27, -1, -1, null, p);
			}
			return false;
		}
	}
}