using Terraria;
using Terraria.ModLoader;

namespace CalamityBardHealer
{
	public class ItemOverrides : GlobalItem
	{
		public override bool AppliesToEntity(Item item, bool lateInstatiation) => ModLoader.TryGetMod("CalamityMod", out Mod calamity) && (item.type == calamity.Find<ModItem>("AnahitasArpeggio").Type || item.type == calamity.Find<ModItem>("BelchingSaxophone").Type || item.type == calamity.Find<ModItem>("FaceMelter").Type);
		public override void PostUpdate(Item item) {
			if(ModLoader.TryGetMod("CalamityMod", out Mod calamity)) if(item.type == calamity.Find<ModItem>("AnahitasArpeggio").Type) item.ChangeItemType(ModContent.ItemType<Items.AnahitasArpeggio>());
			else if(item.type == calamity.Find<ModItem>("BelchingSaxophone").Type) item.ChangeItemType(ModContent.ItemType<Items.BelchingSaxophone>());
			else if(item.type == calamity.Find<ModItem>("FaceMelter").Type) item.ChangeItemType(ModContent.ItemType<Items.FaceMelter>());
		}
		public override void UpdateInventory(Item item, Player player) => this.PostUpdate(item);
	}
}