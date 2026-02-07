using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace CalamityBardHealer
{
	public class ItemChanges : GlobalItem
	{
		public override bool AppliesToEntity(Item item, bool lateInstatiation) => ModLoader.TryGetMod("ThoriumMod", out Mod thorium) && (item.type == thorium.Find<ModItem>("SongofIceAndFire").Type || item.type == thorium.Find<ModItem>("HallowedMegaphone").Type || item.type == thorium.Find<ModItem>("LustrousBaton").Type || item.type == thorium.Find<ModItem>("BoneBaton").Type || item.type == thorium.Find<ModItem>("FallingTwilight").Type || item.type == thorium.Find<ModItem>("BloodHarvest").Type || item.type == thorium.Find<ModItem>("TrueFallingTwilight").Type || item.type == thorium.Find<ModItem>("TrueBloodHarvest").Type || item.type == thorium.Find<ModItem>("DreadTearer").Type || item.type == thorium.Find<ModItem>("DemonBloodRipper").Type || item.type == thorium.Find<ModItem>("MorningDew").Type || item.type == thorium.Find<ModItem>("TerraScythe").Type);
		public override void SetDefaults(Item item) {
			item.damage += item.damage / 2;
			if(item.rare < 4) item.rare = 4;
			item.crit += 5;
		}
		public override void ModifyShootStats(Item item, Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
			if(Collision.CanHitLine(position, 0, 0, position + Vector2.Normalize(velocity) * 36f, 0, 0) && item.type == ModLoader.GetMod("ThoriumMod").Find<ModItem>("SongofIceAndFire").Type) position += Vector2.Normalize(velocity) * 36f;
		}
	}
}