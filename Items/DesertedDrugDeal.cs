using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.ModLoader;
using ThoriumMod;
using ThoriumMod.Items;
using ThoriumMod.Items.HealerItems;

namespace CalamityBardHealer.Items
{
	public class DesertedDrugDeal : ThoriumItem
	{
		public override bool IsLoadingEnabled(Mod mod) => ModLoader.HasMod("InfernumMode");
		public override void SetStaticDefaults() {
			Item.ResearchUnlockCount = 1;
			ItemID.Sets.gunProj[Type] = true;
			if(!ModLoader.TryGetMod("Redemption", out Mod mor)) return;
			mor.Call("addElementItem", 3, Item.type);
			mor.Call("addElementItem", 5, Item.type);
		}
		public override void SetDefaults() {
			Item.width = 24;
			Item.height = 76;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.useTime = 60;
			Item.useAnimation = 60;
			Item.autoReuse = true;
			Item.noUseGraphic = true;
			Item.DamageType = HealerDamage.Instance;
			Item.damage = 88;
			Item.knockBack = 3f;
			Item.value = Item.sellPrice(gold: 16);
			Item.useAmmo = AmmoID.Arrow;
			if(ModLoader.TryGetMod("InfernumMode", out Mod infernum)) {
				Item.rare = infernum.Find<ModRarity>("InfernumVassalRarity").Type;
				Item.shoot = ModContent.ProjectileType<Projectiles.DesertedDrugDeal>();
			}
			else Item.rare = ItemRarityID.Cyan;
			Item.shootSpeed = 5f;
			Item.noMelee = true;
			Item.channel = true;
			this.isHealer = true;
			this.healType = HealType.AllyAndPlayer;
			this.healAmount = 16;
			if(ModContent.GetInstance<BalanceConfig>().healing != 1) this.healAmount = (int)MathHelper.Max(ModContent.GetInstance<BalanceConfig>().healing * (float)this.healAmount, 1);
			this.healDisplay = true;
		}
		public override void HoldItem(Player player) {
			if(Main.myPlayer != player.whoAmI || player.ownedProjectileCounts[Item.shoot] > 0) return;
			int z = Projectile.NewProjectile(player.GetSource_ItemUse(Item), player.Center, Vector2.Zero, Item.shoot, player.GetWeaponDamage(Item), Item.knockBack, player.whoAmI);
			NetMessage.SendData(27, -1, -1, null, z);
		}
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) => false;
	}
}