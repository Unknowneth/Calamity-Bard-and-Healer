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
	public class SongoftheAncients : BigInstrumentItemBase
	{
		public override BardInstrumentType InstrumentType => BardInstrumentType.Wind;
		public override void SafeSetStaticDefaults() {
			base.Item.ResearchUnlockCount = 1;
			this.Empowerments.AddInfo<ResourceMaximum>(1, 0);
			this.Empowerments.AddInfo<ResourceRegen>(1, 0);
			this.Empowerments.AddInfo<CriticalStrikeChance>(1, 0);
			Terraria.ID.ItemID.Sets.ItemsThatAllowRepeatedRightClick[base.Item.type] = true;
		}
		public override void SafeSetBardDefaults() {
			base.Item.width = 70;
			base.Item.height = 24;
			base.Item.useStyle = 5;
			if(!ModLoader.HasMod("Look")) base.Item.holdStyle = 3;
			base.Item.useTime = 20;
			base.Item.useAnimation = 20;
			base.Item.damage = 66;
			base.Item.crit = 6;
			base.Item.autoReuse = true;
			base.Item.knockBack = 1.25f;
			base.Item.value = Item.sellPrice(gold: 12);
			base.Item.rare = 8;
			base.Item.noMelee = true;
			base.Item.shootSpeed = 6f;
			base.Item.shoot = ModContent.ProjectileType<Projectiles.AncientSong>();
			base.Item.DamageType = BardDamage.Instance;
			base.Item.UseSound = new SoundStyle?(ThoriumSounds.Flute_Sound);
		}
		public override void SafeBardShoot(int success, int level, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
			if(Main.myPlayer == player.whoAmI) {
				position += Vector2.Normalize(velocity) * 66f;
				int index = -1;
				float maxDistSQ = 1000000f;
				float bardHit = 1f + (float)success * 0.01f;
				foreach(NPC npc in Main.ActiveNPCs) if(npc.CanBeChasedBy(null, false)) {
					float distSQ = player.DistanceSQ(npc.Center);
					if(distSQ < maxDistSQ && Collision.CanHit(player.Center, 1, 1, npc.Center, 1, 1)) {
						maxDistSQ = distSQ;
						index = npc.whoAmI;
					}
				}
				if(index > -1) velocity = Vector2.Normalize(Main.npc[index].Center - position) * velocity.Length();
				for(int i = -1; i <= 1; i++) if(i != 0) {
					int p = Projectile.NewProjectile(source, position, velocity.RotatedBy(i * MathHelper.PiOver4), type, damage, knockback, player.whoAmI, -i);
					NetMessage.SendData(27, -1, -1, null, p);
				}
			}
		}
		public override void AddRecipes() => CreateRecipe().AddIngredient(ModLoader.GetMod("ThoriumMod").Find<ModItem>("SongofIceAndFire").Type).AddIngredient(ModLoader.GetMod("ThoriumMod").Find<ModItem>("MeteoriteOboe").Type).AddIngredient(ModLoader.GetMod("ThoriumMod").Find<ModItem>("BrokenHeroFragment").Type, 5).AddTile(134).Register();
		public override Color? GetAlpha(Color lightColor) => Color.White;
		public override void UseItemFrame(Player player) => this.HoldItemFrame(player);
		public override void HoldItemFrame(Player player) => player.itemLocation -= (new Vector2(ModLoader.HasMod("Look") ? 4 : 6) * player.Directions).RotatedBy(player.itemRotation);
		public override bool AltFunctionUse(Player player) => true;
	}
}