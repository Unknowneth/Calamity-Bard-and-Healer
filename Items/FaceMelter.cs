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
using System;

namespace CalamityBardHealer.Items
{
	public class FaceMelter : BardItem
	{
		public override BardInstrumentType InstrumentType => BardInstrumentType.String;
		public override void SetStaticDefaults() {
			base.Item.ResearchUnlockCount = 1;
			this.Empowerments.AddInfo<LifeRegeneration>(2, 0);
			this.Empowerments.AddInfo<DamageReduction>(2, 0);
			this.Empowerments.AddInfo<Defense>(3, 0);
			this.Empowerments.AddInfo<EmpowermentProlongation>(4, 0);
		}
		public override void SetBardDefaults() {
			base.Item.CloneDefaults(ModLoader.GetMod("CalamityMod").Find<ModItem>("FaceMelter").Type);
			if(ModContent.GetInstance<BalanceConfig>().bard != 1f) base.Item.damage = (int)MathHelper.Max(ModContent.GetInstance<BalanceConfig>().bard * (float)base.Item.damage, 1);
			base.Item.shootSpeed = 16f;
			base.Item.mana = 0;
			base.Item.shoot = ModContent.ProjectileType<Projectiles.MelterNote1>();
			base.Item.DamageType = BardDamage.Instance;
			base.InspirationCost = 1;
			base.Item.holdStyle = 5;
			base.Item.useStyle = 12;
		}
		public override bool BardShoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
			if(player.altFunctionUse == 2) {
				int z = Projectile.NewProjectile(source, Main.MouseWorld, Vector2.Zero, ModContent.ProjectileType<Projectiles.MelterAmp>(), damage, knockback, player.whoAmI);
				NetMessage.SendData(27, -1, -1, null, z);
				return false;
			}
			if(Main.rand.NextBool(2)) {
				damage = (int)((float)damage * 1.5f);
				type = ModContent.ProjectileType<Projectiles.MelterNote1>();
			}
			else {
				velocity *= 1.5f;
				type = ModContent.ProjectileType<Projectiles.MelterNote2>();
			}
			int p = Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
			NetMessage.SendData(27, -1, -1, null, p);
			return false;
		}
		public override bool CanPlayInstrument(Player player) {
			if(player.altFunctionUse == 2) {
				base.Item.useTime = 20;
				base.Item.useAnimation = 20;
			}
			else {
				base.Item.useTime = 5;
				base.Item.useAnimation = 10;
			}
			return true;
		}
		public override bool AltFunctionUse(Player player) => true;
		public override void HoldItemFrame(Player player) => player.itemLocation += new Vector2(-14, 12) * player.Directions;
		public override void UseItemFrame(Player player) => this.HoldItemFrame(player);
	}
}