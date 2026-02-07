using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Utilities;
using Terraria.DataStructures;
using Terraria.ModLoader;
using ThoriumMod;
using ThoriumMod.Items.HealerItems;
using System.IO;

namespace CalamityBardHealer.Items
{
	public class TimesOldRoman : ScytheItem
	{
		public override bool IsLoadingEnabled(Mod mod) => ModLoader.HasMod("CalamityHunt");
		internal Vector2? oldVelocity = null;
		private int comboCounter = 0;
		public override void SetStaticDefaults() => base.SetStaticDefaultsToScythe();
		public override void SetDefaults() {
			base.SetDefaultsToScythe();
			Item.width = 94;
			Item.height = 94;
			Item.useStyle = 1;
			Item.useTime = 36;
			Item.useAnimation = 36;
			Item.autoReuse = true;
			Item.DamageType = HealerDamage.Instance;
			base.Item.value = Item.sellPrice(gold: 30);
			if(ModLoader.TryGetMod("CalamityMod", out Mod calamity)) base.Item.rare = calamity.Find<ModRarity>("BurnishedAuric").Type;
			else base.Item.rare = 11;
			Item.damage = 1660;
			Item.knockBack = 6.5f;
			Item.noUseGraphic = true;
			Item.noMelee = true;
			Item.crit = 26;
			if(ModLoader.HasMod("CalamityHunt")) Item.shoot = ModContent.ProjectileType<Projectiles.TimesOldRoman>();
			Item.shootSpeed = 1f;
			scytheSoulCharge = 5;
			if(ModContent.GetInstance<BalanceConfig>().soulEssence != 1) this.scytheSoulCharge = (int)MathHelper.Clamp(ModContent.GetInstance<BalanceConfig>().soulEssence * this.scytheSoulCharge, 0, 5);
		}
		public override void UpdateInventory(Player player) {
			if(Main.myPlayer == player.whoAmI && player.HeldItem.type != Type) comboCounter = 0;
		}
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
			if(Main.myPlayer == player.whoAmI) {
				int z = Projectile.NewProjectile(source, position, oldVelocity ?? velocity, type, damage, knockback, player.whoAmI, comboCounter, player.itemAnimationMax > 0 ? player.itemAnimationMax > player.itemTimeMax ? player.itemTimeMax : player.itemAnimationMax : Item.useAnimation);
				Main.projectile[z].ai[1] *= 2f;
				Main.projectile[z].originalDamage = damage;
				NetMessage.SendData(27, -1, -1, null, z);
			}
			if(++comboCounter > 1) comboCounter = 0;
			oldVelocity = null;
			return false;
		}
		public override bool MeleePrefix() => true;
		public override void NetSend(BinaryWriter writer) => writer.Write(comboCounter);
		public override void NetReceive(BinaryReader reader) => comboCounter = reader.ReadInt32();
	}
}