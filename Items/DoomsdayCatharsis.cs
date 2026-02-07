using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ModLoader;
using ThoriumMod;
using ThoriumMod.Empowerments;
using ThoriumMod.Items;
using ThoriumMod.Items.BardItems;

namespace CalamityBardHealer.Items
{
	public class DoomsdayCatharsis : BigInstrumentItemBase
	{
		internal int trumpetTimer = 0;
		public override BardInstrumentType InstrumentType => BardInstrumentType.Brass;
		public override void SafeSetStaticDefaults() {
			base.Item.ResearchUnlockCount = 1;
			this.Empowerments.AddInfo<AttackSpeed>(2, 0);
			this.Empowerments.AddInfo<CriticalStrikeChance>(2, 0);
			this.Empowerments.AddInfo<Damage>(2, 0);
			this.Empowerments.AddInfo<FlatDamage>(2, 0);
			this.Empowerments.AddInfo<FlightTime>(2, 0);
			this.Empowerments.AddInfo<MovementSpeed>(2, 0);
		}
		public override void SafeSetBardDefaults() {
			base.Item.width = 76;
			base.Item.height = 46;
			base.Item.useStyle = 5;
			if(!ModLoader.HasMod("Look")) base.Item.holdStyle = 3;
			base.Item.useTime = 12;
			base.Item.useAnimation = 12;
			base.Item.damage = 215;
			if(ModContent.GetInstance<BalanceConfig>().bard != 1f) base.Item.damage = (int)MathHelper.Max(ModContent.GetInstance<BalanceConfig>().bard * (float)base.Item.damage, 1);
			base.Item.crit = 56;
			base.Item.autoReuse = true;
			base.Item.knockBack = 2.5f;
			base.Item.value = Item.sellPrice(gold: 30);
			if(ModLoader.TryGetMod("CalamityMod", out Mod calamity)) base.Item.rare = calamity.Find<ModRarity>("BurnishedAuric").Type;
			else base.Item.rare = 11;
			base.Item.noMelee = true;
			base.Item.shootSpeed = 10f;
			base.Item.shoot = ModContent.ProjectileType<Projectiles.DoomsdayToot>();
			base.Item.DamageType = BardDamage.Instance;
			base.Item.UseSound = new SoundStyle("CalamityBardHealer/Sounds/DoomsdayCatharsis") { SoundLimitBehavior = SoundLimitBehavior.ReplaceOldest, MaxInstances = 5 };
		}
		public override void SafeBardShoot(int success, int level, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
			if(Main.myPlayer != player.whoAmI) return;
			int p = Projectile.NewProjectile(source, position + Vector2.Normalize(velocity) * 76f, velocity, type, (int)(damage * (1f + MathHelper.Min(10, success) * 0.1f)), knockback, player.whoAmI, MathHelper.Lerp(160f, 800f, (float)MathHelper.Min(10, success) / 10f));
			NetMessage.SendData(27, -1, -1, null, p);
			if(trumpetTimer <= 25) return;
			trumpetTimer = 0;
			p = Projectile.NewProjectile(source, position, -MathHelper.ToRadians(30f).ToRotationVector2() * velocity.Length(), ModContent.ProjectileType<Projectiles.DoomsdayCatharsis>(), (int)(damage * (1f + MathHelper.Min(10, success) * 0.1f)), knockback, player.whoAmI, MathHelper.Lerp(160f, 800f, (float)MathHelper.Min(10, success) / 10f), 640f, 340f);
			NetMessage.SendData(27, -1, -1, null, p);
			p = Projectile.NewProjectile(source, position, -MathHelper.ToRadians(150f).ToRotationVector2() * velocity.Length(), ModContent.ProjectileType<Projectiles.DoomsdayCatharsis>(), (int)(damage * (1f + MathHelper.Min(10, success) * 0.1f)), knockback, player.whoAmI, MathHelper.Lerp(160f, 800f, (float)MathHelper.Min(10, success) / 10f), -640f, 340f);
			NetMessage.SendData(27, -1, -1, null, p);
			if(success < 20) return;
			p = Projectile.NewProjectile(source, position, -MathHelper.ToRadians(40f).ToRotationVector2() * velocity.Length(), ModContent.ProjectileType<Projectiles.DoomsdayCatharsis>(), (int)(damage * (1f + MathHelper.Min(10, success) * 0.1f)), knockback, player.whoAmI, MathHelper.Lerp(160f, 800f, (float)MathHelper.Min(10, success) / 10f), 672f, 300f);
			NetMessage.SendData(27, -1, -1, null, p);
			p = Projectile.NewProjectile(source, position, -MathHelper.ToRadians(140f).ToRotationVector2() * velocity.Length(), ModContent.ProjectileType<Projectiles.DoomsdayCatharsis>(), (int)(damage * (1f + MathHelper.Min(10, success) * 0.1f)), knockback, player.whoAmI, MathHelper.Lerp(160f, 800f, (float)MathHelper.Min(10, success) / 10f), -672f, 300f);
			NetMessage.SendData(27, -1, -1, null, p);
			if(success < 30) return;
			p = Projectile.NewProjectile(source, position, -MathHelper.ToRadians(50f).ToRotationVector2() * velocity.Length(), ModContent.ProjectileType<Projectiles.DoomsdayCatharsis>(), (int)(damage * (1f + MathHelper.Min(10, success) * 0.1f)), knockback, player.whoAmI, MathHelper.Lerp(160f, 800f, (float)MathHelper.Min(10, success) / 10f), 702f, 260f);
			NetMessage.SendData(27, -1, -1, null, p);
			p = Projectile.NewProjectile(source, position, -MathHelper.ToRadians(130f).ToRotationVector2() * velocity.Length(), ModContent.ProjectileType<Projectiles.DoomsdayCatharsis>(), (int)(damage * (1f + MathHelper.Min(10, success) * 0.1f)), knockback, player.whoAmI, MathHelper.Lerp(160f, 800f, (float)MathHelper.Min(10, success) / 10f), -702f, 260f);
			NetMessage.SendData(27, -1, -1, null, p);
		}
		public override void Shoot_OnSuccess(Player player) {
			if(Main.myPlayer == player.whoAmI) trumpetTimer++;
		}
		public override Color? GetAlpha(Color lightColor) => Color.White;
		public override void UseItemFrame(Player player) => this.HoldItemFrame(player);
		public override void HoldItemFrame(Player player) => player.itemLocation += (new Vector2(ModLoader.HasMod("Look") ? -4 : -6, ModLoader.HasMod("Look") ? 4 : 2) * player.Directions).RotatedBy(player.itemRotation);
	}
}