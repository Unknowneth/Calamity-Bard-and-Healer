using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ModLoader;
using ThoriumMod;
using ThoriumMod.Sounds;
using ThoriumMod.Empowerments;
using ThoriumMod.Items;
using ThoriumMod.Items.BardItems;

namespace CalamityBardHealer.Items
{
	public class ChristmasCarol : BigInstrumentItemBase
	{
		private int trumpetTimer = 0;
		public override BardInstrumentType InstrumentType => BardInstrumentType.Percussion;
		public override void SafeSetStaticDefaults() {
			base.Item.ResearchUnlockCount = 1;
			this.Empowerments.AddInfo<LifeRegeneration>(2, 0);
			this.Empowerments.AddInfo<Defense>(2, 0);
			this.Empowerments.AddInfo<DamageReduction>(2, 0);
			this.Empowerments.AddInfo<FlightTime>(2, 0);
			this.Empowerments.AddInfo<JumpHeight>(2, 0);
			this.Empowerments.AddInfo<MovementSpeed>(2, 0);
		}
		public override void SafeSetBardDefaults() {
			base.Item.width = 46;
			base.Item.height = 46;
			base.Item.useStyle = 1;
			base.Item.useTime = 30;
			base.Item.useAnimation = 30;
			base.Item.damage = 567;
			if(ModContent.GetInstance<BalanceConfig>().bard != 1f && base.Item.DamageType == BardDamage.Instance) base.Item.damage = (int)MathHelper.Max(ModContent.GetInstance<BalanceConfig>().bard * (float)base.Item.damage, 1);
			base.Item.crit = 16;
			base.Item.autoReuse = true;
			base.Item.knockBack = 2f;
			base.Item.value = Item.sellPrice(gold: 28);
			if(ModLoader.TryGetMod("CalamityMod", out Mod calamity)) base.Item.rare = calamity.Find<ModRarity>("CosmicPurple").Type;
			else base.Item.rare = 11;
			base.Item.noMelee = true;
			base.Item.noUseGraphic = true;
			base.Item.shootSpeed = 12f;
			base.Item.shoot = ModContent.ProjectileType<Projectiles.ChristmasCarol>();
			base.Item.DamageType = BardDamage.Instance;
			base.Item.UseSound = new SoundStyle?(ThoriumSounds.TambourineSound);
		}
		public override void SafeBardShoot(int success, int level, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
			if(Main.myPlayer == player.whoAmI) {
				int index = -1;
				float maxDistSQ = 1000000f;
				foreach(NPC npc in Main.ActiveNPCs) if(npc.CanBeChasedBy(null, false)) {
					float distSQ = player.DistanceSQ(npc.Center);
					if(distSQ < maxDistSQ && Collision.CanHit(player.Center, 1, 1, npc.Center, 1, 1)) {
						maxDistSQ = distSQ;
						index = npc.whoAmI;;
					}
				}
				if(index > -1) velocity = Vector2.Normalize(Main.npc[index].Center - position) * velocity.Length();
				int p = Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI, level);
				NetMessage.SendData(27, -1, -1, null, p);
				for(int i = -1; i <= 1; i++) {
					p = Projectile.NewProjectile(source, position, velocity.RotatedBy(i * MathHelper.PiOver4 * 0.25f), ModLoader.GetMod("ThoriumMod").Find<ModProjectile>("JingleBellsPro").Type, damage, knockback, player.whoAmI, 0f, 1f);
					NetMessage.SendData(27, -1, -1, null, p);
				}
			}
		}
		public override void AddRecipes() {
			if(ModLoader.TryGetMod("CalamityMod", out Mod calamity) && ModLoader.TryGetMod("ThoriumMod", out Mod thorium)) CreateRecipe().AddIngredient(thorium.Find<ModItem>("JingleBells").Type).AddIngredient(thorium.Find<ModItem>("Tambourine").Type).AddIngredient(calamity.Find<ModItem>("CosmiliteBar").Type, 8).AddIngredient(calamity.Find<ModItem>("EndothermicEnergy").Type, 20).AddTile(calamity.Find<ModTile>("CosmicAnvil").Type).Register();
		}
	}
}