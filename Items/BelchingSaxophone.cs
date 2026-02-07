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
	public class BelchingSaxophone : BardItem
	{
		public override string Texture => "CalamityMod/Items/Weapons/Magic/BelchingSaxophone";
		public override BardInstrumentType InstrumentType => BardInstrumentType.Brass;
		public override void SetStaticDefaults() {
			base.Item.ResearchUnlockCount = 1;
			this.Empowerments.AddInfo<Damage>(2, 0);
			this.Empowerments.AddInfo<FlatDamage>(3, 0);
			this.Empowerments.AddInfo<AquaticAbility>(2, 0);
		}
		public override void SetBardDefaults() {
			base.Item.CloneDefaults(ModLoader.GetMod("CalamityMod").Find<ModItem>("BelchingSaxophone").Type);
			if(ModContent.GetInstance<BalanceConfig>().bard != 1f) base.Item.damage = (int)MathHelper.Max(ModContent.GetInstance<BalanceConfig>().bard * (float)base.Item.damage, 1);
			base.Item.mana = 0;
			base.Item.shoot = ModContent.ProjectileType<Projectiles.AcidicReed>();
			base.Item.DamageType = BardDamage.Instance;
			base.InspirationCost = 1;
			base.Item.UseSound = new SoundStyle("CalamityMod/Sounds/Item/Saxophone/Sax3");
			if(ModLoader.HasMod("Look")) base.Item.holdStyle = 0;
		}
		public override bool BardShoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
			if(Main.myPlayer != player.whoAmI) return false;
			if(Main.rand.NextBool()) {
				Vector2 speed = Utils.RotatedBy(velocity, (double)MathHelper.ToRadians((float)Main.rand.Next(-15, 16)), default(Vector2));
				speed.Normalize();
				speed *= 15f;
				speed.Y -= Math.Abs(speed.X) * 0.2f;
				int z = Projectile.NewProjectile(source, position, speed, ModContent.ProjectileType<Projectiles.AcidicSaxBubble>(), damage, knockback, player.whoAmI);
				NetMessage.SendData(27, -1, -1, null, z);
			}
			velocity.X += (float)Main.rand.Next(-40, 41) * 0.05f;
			velocity.Y += (float)Main.rand.Next(-40, 41) * 0.05f;
			int p = Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
			NetMessage.SendData(27, -1, -1, null, p);
			if(Main.rand.NextBool()) {
				p = Projectile.NewProjectile(source, position, velocity * 0.75f, Main.rand.Next(new int[]
				{ ModContent.ProjectileType<Projectiles.ToxicEightNote>(), ModContent.ProjectileType<Projectiles.ToxicQuarterNote>(), ModContent.ProjectileType<Projectiles.ToxicTiedEightNote>()}), (int)((double)damage * 0.75), knockback, player.whoAmI);
				NetMessage.SendData(27, -1, -1, null, p);
			}
			return false;
		}
		public override void UseItemFrame(Player player) => this.HoldItemFrame(player);
		public override void HoldItemFrame(Player player) => player.itemLocation += (new Vector2(-16, 4) * player.Directions).RotatedBy(player.itemRotation);
	}
}