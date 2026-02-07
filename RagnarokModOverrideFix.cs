using Terraria;
using Terraria.ModLoader;

namespace CalamityBardHealer
{
	public class RagnarokModOverrideFix : ModSystem
	{
		public override bool IsLoadingEnabled(Mod mod) => ModLoader.HasMod("RagnarokMod");
		public override void PostAddRecipes() {
			if(ModLoader.HasMod("ThoriamityConvergenceREDUX") && ModLoader.TryGetMod("CalamityMod", out Mod calamity)) for(int i = 0; i < Recipe.numRecipes; i++) if(Main.recipe[i].HasResult(calamity.Find<ModItem>("AngelTreads").Type)) {
				Main.recipe[i].requiredItem.Clear();
				Main.recipe[i].AddIngredient(5000, 1).AddIngredient(calamity.Find<ModItem>("HarpyRing").Type, 1).AddIngredient(calamity.Find<ModItem>("EssenceofSunlight").Type, 5).AddIngredient(547, 1).AddIngredient(548, 1).AddIngredient(549, 1);
				break;
			}
			if(ModLoader.TryGetMod("RagnarokMod", out Mod ragnarok) && ModLoader.TryGetMod("ThoriumMod", out Mod thorium)) for(int i = 0; i < Recipe.numRecipes; i++) if(Main.recipe[i].TryGetResult(ragnarok.Find<ModItem>("AnahitasArpeggioOverride").Type, out Item result)) result.ChangeItemType(ModContent.ItemType<Items.AnahitasArpeggio>());
			else if(Main.recipe[i].TryGetResult(ragnarok.Find<ModItem>("BelchingSaxophoneOverride").Type, out result)) result.ChangeItemType(ModContent.ItemType<Items.BelchingSaxophone>());
			else if(Main.recipe[i].TryGetResult(ragnarok.Find<ModItem>("FaceMelterOverride").Type, out result)) result.ChangeItemType(ModContent.ItemType<Items.FaceMelter>());
			else if(Main.recipe[i].TryGetIngredient(ragnarok.Find<ModItem>("AnahitasArpeggioOverride").Type, out Item ingredient)) ingredient.ChangeItemType(ModContent.ItemType<Items.AnahitasArpeggio>());
			else if(Main.recipe[i].TryGetIngredient(ragnarok.Find<ModItem>("BelchingSaxophoneOverride").Type, out ingredient)) ingredient.ChangeItemType(ModContent.ItemType<Items.BelchingSaxophone>());
			else if(Main.recipe[i].TryGetIngredient(ragnarok.Find<ModItem>("FaceMelterOverride").Type, out ingredient)) ingredient.ChangeItemType(ModContent.ItemType<Items.FaceMelter>());
		}
	}
}