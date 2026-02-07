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
	public class OathofSacrifice : ThoriumItem
	{
		public override void SetDefaults() {
			this.healType = HealType.Ally;
			this.healAmount = 1;
			if(ModContent.GetInstance<BalanceConfig>().healing != 1) this.healAmount = (int)MathHelper.Max(ModContent.GetInstance<BalanceConfig>().healing * (float)this.healAmount, 1);
			this.healDisplay = true;
			this.isHealer = true;
			this.radiantLifeCost = 2;
			base.Item.DamageType = ThoriumDamageBase<HealerTool>.Instance;
			base.Item.mana = 2;
			base.Item.width = 42;
			base.Item.height = 128;
			base.Item.useTime = 1;
			base.Item.useAnimation = 1;
			base.Item.useStyle = 14;
			base.Item.holdStyle = 6;
			base.Item.noMelee = true;
			base.Item.knockBack = 1f;
			base.Item.channel = true;
			base.Item.value = Item.sellPrice(gold: 30);
			if(ModLoader.TryGetMod("CalamityMod", out Mod calamity)) base.Item.rare = calamity.Find<ModRarity>("CalamityRed").Type;
			else base.Item.rare = 10;
			base.Item.UseSound = SoundID.Item20;
			base.Item.autoReuse = true;
			base.Item.shoot = ModContent.ProjectileType<Projectiles.OathofSacrifice>();
			base.Item.shootSpeed = 0f;
		}
		public override void HoldItem(Player player) {
			if(player.lifeRegen > 0) player.lifeRegen = 0;
			player.statDefense *= 0;
			player.endurance *= 0f;
		}
		public override void HoldStyle(Player player, Rectangle itemFrame) => player.itemLocation += new Vector2(-12f, 22f) * player.Directions;
		public override void UseStyle(Player player, Rectangle itemFrame) => player.itemLocation += new Vector2(-12f, 22f) * player.Directions;
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) => player.ownedProjectileCounts[type] == 0;
		public override Color? GetAlpha(Color lightColor) => Color.White;
	}
}