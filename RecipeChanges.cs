using Terraria;
using Terraria.ModLoader;

namespace CalamityBardHealer
{
	public class RecipeChanges : ModSystem
	{
		public static RecipeGroup InstrumentAccessories;
		public override void Unload() => InstrumentAccessories = null;
		public override void AddRecipeGroups() {
			if(ModLoader.TryGetMod("ThoriumMod", out Mod thorium)) InstrumentAccessories = new RecipeGroup(() => $"{Terraria.Localization.Language.GetTextValue("LegacyMisc.37")} tier-2 instrument-specific accessories", thorium.Find<ModItem>("EpicMouthpiece").Type, thorium.Find<ModItem>("StraightMute").Type, thorium.Find<ModItem>("GuitarPickClaw").Type, thorium.Find<ModItem>("DigitalTuner").Type);
			if(InstrumentAccessories is not null) RecipeGroup.RegisterGroup("CalamityBardHealer:InstrumentAccessories", InstrumentAccessories);
		}
		public override void AddRecipes() {
			if(ModLoader.TryGetMod("CalamityMod", out Mod calamity) && ModLoader.TryGetMod("ThoriumMod", out Mod thorium)) {
				Recipe.Create(calamity.Find<ModItem>("GalacticaSingularity").Type, 1).AddIngredient(thorium.Find<ModItem>("ShootingStarFragment").Type, 1).AddIngredient(thorium.Find<ModItem>("CelestialFragment").Type, 1).AddIngredient(thorium.Find<ModItem>("WhiteDwarfFragment").Type, 1).AddIngredient(calamity.Find<ModItem>("MeldBlob").Type, 1).AddTile(412).Register();
				Recipe.Create(thorium.Find<ModItem>("BloomWeave").Type, 20).AddIngredient(thorium.Find<ModItem>("Cloth").Type, 20).AddIngredient(calamity.Find<ModItem>("LivingShard").Type, 1).AddTile(86).Register();
				Recipe.Create(thorium.Find<ModItem>("UnholyShards").Type, 1).AddIngredient(calamity.Find<ModItem>("BloodOrb").Type, 1).Register();
				Recipe.Create(calamity.Find<ModItem>("BloodOrb").Type, 1).AddIngredient(thorium.Find<ModItem>("UnholyShards").Type, 1).Register();
				Recipe.Create(thorium.Find<ModItem>("BoneReaper").Type, 1).AddIngredient(calamity.Find<ModItem>("AerialiteBar").Type, 10).AddIngredient(154, 4).AddTile(16).Register();
				thorium.Call("AddPlayerDoTBuffID", calamity.Find<ModBuff>("AbsorberAffliction").Type);
				thorium.Call("AddPlayerDoTBuffID", calamity.Find<ModBuff>("AstralInfectionDebuff").Type);
				thorium.Call("AddPlayerDoTBuffID", calamity.Find<ModBuff>("BanishingFire").Type);
				thorium.Call("AddPlayerDoTBuffID", calamity.Find<ModBuff>("BrainRot").Type);
				thorium.Call("AddPlayerDoTBuffID", calamity.Find<ModBuff>("BrimstoneFlames").Type);
				thorium.Call("AddPlayerDoTBuffID", calamity.Find<ModBuff>("BurningBlood").Type);
				thorium.Call("AddPlayerDoTBuffID", calamity.Find<ModBuff>("SnapClamDebuff").Type);
				thorium.Call("AddPlayerDoTBuffID", calamity.Find<ModBuff>("CrushDepth").Type);
				thorium.Call("AddPlayerDoTBuffID", calamity.Find<ModBuff>("Dragonfire").Type);
				thorium.Call("AddPlayerDoTBuffID", calamity.Find<ModBuff>("ElementalMix").Type);
				thorium.Call("AddPlayerDoTBuffID", calamity.Find<ModBuff>("Eutrophication").Type);
				thorium.Call("AddPlayerDoTBuffID", calamity.Find<ModBuff>("GalvanicCorrosion").Type);
				thorium.Call("AddPlayerDoTBuffID", calamity.Find<ModBuff>("GodSlayerInferno").Type);
				thorium.Call("AddPlayerDoTBuffID", calamity.Find<ModBuff>("HolyFlames").Type);
				thorium.Call("AddPlayerDoTBuffID", calamity.Find<ModBuff>("Irradiated").Type);
				thorium.Call("AddPlayerDoTBuffID", calamity.Find<ModBuff>("MiracleBlight").Type);
				thorium.Call("AddPlayerDoTBuffID", calamity.Find<ModBuff>("Nightwither").Type);
				thorium.Call("AddPlayerDoTBuffID", calamity.Find<ModBuff>("Plague").Type);
				thorium.Call("AddPlayerDoTBuffID", calamity.Find<ModBuff>("RiptideDebuff").Type);
				thorium.Call("AddPlayerDoTBuffID", calamity.Find<ModBuff>("SagePoison").Type);
				thorium.Call("AddPlayerDoTBuffID", calamity.Find<ModBuff>("ShellfishClaps").Type);
				thorium.Call("AddPlayerDoTBuffID", calamity.Find<ModBuff>("Shred").Type);
				thorium.Call("AddPlayerDoTBuffID", calamity.Find<ModBuff>("SulphuricPoisoning").Type);
				thorium.Call("AddPlayerDoTBuffID", calamity.Find<ModBuff>("TemporalSadness").Type);
				thorium.Call("AddPlayerDoTBuffID", calamity.Find<ModBuff>("Vaporfied").Type);
				thorium.Call("AddPlayerDoTBuffID", calamity.Find<ModBuff>("VulnerabilityHex").Type);
				thorium.Call("AddPlayerDoTBuffID", calamity.Find<ModBuff>("WeakBrimstoneFlames").Type);
				thorium.Call("AddPlayerDoTBuffID", calamity.Find<ModBuff>("WhisperingDeath").Type);
				thorium.Call("AddPlayerStatusBuffID", calamity.Find<ModBuff>("ArmorCrunch").Type);
				thorium.Call("AddPlayerStatusBuffID", calamity.Find<ModBuff>("BrainRot").Type);
				thorium.Call("AddPlayerStatusBuffID", calamity.Find<ModBuff>("BurningBlood").Type);
				thorium.Call("AddPlayerStatusBuffID", calamity.Find<ModBuff>("GlacialState").Type);
				thorium.Call("AddPlayerStatusBuffID", calamity.Find<ModBuff>("SulphuricPoisoning").Type);
				thorium.Call("AddPlayerStatusBuffID", calamity.Find<ModBuff>("VulnerabilityHex").Type);
				thorium.Call("AddPlayerStatusBuffID", calamity.Find<ModBuff>("Withered").Type);
				calamity.Call("MakeItemExhumable", thorium.Find<ModItem>("DreamCatcher").Type, ModContent.ItemType<Items.OathofSacrifice>());
				calamity.Call("MakeItemExhumable", thorium.Find<ModItem>("TheGoodBook").Type, ModContent.ItemType<Items.PledgeofSelflessness>());
				calamity.Call("MakeItemExhumable", thorium.Find<ModItem>("HellBell").Type, ModContent.ItemType<Items.Gashadokuro>());
				calamity.Call("MakeItemExhumable", ModContent.ItemType<Items.FireHazard>(), ModContent.ItemType<Items.Disaster>());
				int[] countsAsTiles = calamity.Find<ModTile>("DraedonsForge").AdjTiles;
				System.Array.Resize(ref countsAsTiles, countsAsTiles.Length + 2);
				countsAsTiles[countsAsTiles.Length - 2] = thorium.Find<ModTile>("SoulForgeNew").Type;
				countsAsTiles[countsAsTiles.Length - 1] = thorium.Find<ModTile>("ThoriumAnvil").Type;
				calamity.Find<ModTile>("DraedonsForge").AdjTiles = countsAsTiles;
				int[] countsAsTiles2 = calamity.Find<ModTile>("CosmicAnvil").AdjTiles;
				System.Array.Resize(ref countsAsTiles2, countsAsTiles2.Length + 1);
				countsAsTiles2[countsAsTiles2.Length - 1] = thorium.Find<ModTile>("ThoriumAnvil").Type;
				calamity.Find<ModTile>("CosmicAnvil").AdjTiles = countsAsTiles2;
			}
		}
		public override void PostAddRecipes() {
			if(ModLoader.TryGetMod("CalamityMod", out Mod calamity) && ModLoader.TryGetMod("ThoriumMod", out Mod thorium)) for(int i = 0; i < Recipe.numRecipes; i++) if(Main.recipe[i].TryGetResult(calamity.Find<ModItem>("AnahitasArpeggio").Type, out Item result)) result.ChangeItemType(ModContent.ItemType<Items.AnahitasArpeggio>());
			else if(Main.recipe[i].TryGetResult(calamity.Find<ModItem>("BelchingSaxophone").Type, out result)) result.ChangeItemType(ModContent.ItemType<Items.BelchingSaxophone>());
			else if(Main.recipe[i].TryGetResult(calamity.Find<ModItem>("FaceMelter").Type, out result)) result.ChangeItemType(ModContent.ItemType<Items.FaceMelter>());
			else if(Main.recipe[i].TryGetIngredient(calamity.Find<ModItem>("AnahitasArpeggio").Type, out Item ingredient)) ingredient.ChangeItemType(ModContent.ItemType<Items.AnahitasArpeggio>());
			else if(Main.recipe[i].TryGetIngredient(calamity.Find<ModItem>("BelchingSaxophone").Type, out ingredient)) ingredient.ChangeItemType(ModContent.ItemType<Items.BelchingSaxophone>());
			else if(Main.recipe[i].TryGetIngredient(calamity.Find<ModItem>("FaceMelter").Type, out ingredient)) ingredient.ChangeItemType(ModContent.ItemType<Items.FaceMelter>());
			else if(Main.recipe[i].HasResult(thorium.Find<ModItem>("FallingTwilight").Type) || Main.recipe[i].HasResult(thorium.Find<ModItem>("BloodHarvest").Type) || Main.recipe[i].HasResult(thorium.Find<ModItem>("LightAnguish").Type) || Main.recipe[i].HasResult(thorium.Find<ModItem>("Embowelment").Type) || Main.recipe[i].HasResult(thorium.Find<ModItem>("SongofIceAndFire").Type)) Main.recipe[i].AddIngredient(calamity.Find<ModItem>("PurifiedGel").Type, 5);
			else if((Main.recipe[i].HasResult(thorium.Find<ModItem>("TrueFallingTwilight").Type) || Main.recipe[i].HasResult(thorium.Find<ModItem>("TrueBloodHarvest").Type) || Main.recipe[i].HasResult(thorium.Find<ModItem>("TrueLightAnguish").Type) || Main.recipe[i].HasResult(thorium.Find<ModItem>("TrueEmbowelment").Type) || Main.recipe[i].HasResult(thorium.Find<ModItem>("TrueHallowedScythe").Type) || Main.recipe[i].HasResult(thorium.Find<ModItem>("TrueCarnwennan").Type)) && Main.recipe[i].HasIngredient(thorium.Find<ModItem>("BrokenHeroFragment").Type)) {
				if(Main.recipe[i].HasResult(thorium.Find<ModItem>("TrueHallowedScythe").Type) || Main.recipe[i].HasResult(thorium.Find<ModItem>("TrueCarnwennan").Type)) Main.recipe[i].AddIngredient(1006, 24);
				else {
					Main.recipe[i].AddIngredient(547, 20);
					Main.recipe[i].AddIngredient(548, 20);
					Main.recipe[i].AddIngredient(549, 20);
				}
				Main.recipe[i].RemoveIngredient(thorium.Find<ModItem>("BrokenHeroFragment").Type);
			}
			else if(Main.recipe[i].HasResult(thorium.Find<ModItem>("TerraScythe").Type) || Main.recipe[i].HasResult(thorium.Find<ModItem>("TerraKnife").Type)) {
				Main.recipe[i].AddIngredient(thorium.Find<ModItem>("BrokenHeroFragment").Type, 3);
				Main.recipe[i].AddIngredient(calamity.Find<ModItem>("LivingShard").Type, 12);
			}
			else if(Main.recipe[i].HasResult(calamity.Find<ModItem>("TerraDisk").Type) || Main.recipe[i].HasResult(calamity.Find<ModItem>("TerraRay").Type) || Main.recipe[i].HasResult(calamity.Find<ModItem>("TerraLance").Type) || Main.recipe[i].HasResult(calamity.Find<ModItem>("TerraFlameburster").Type) || Main.recipe[i].HasResult(calamity.Find<ModItem>("CosmicBolter").Type) || Main.recipe[i].HasResult(calamity.Find<ModItem>("PlantationStaff").Type)) {
				Main.recipe[i].RemoveIngredient(calamity.Find<ModItem>("LivingShard").Type);
				Main.recipe[i].AddIngredient(thorium.Find<ModItem>("BrokenHeroFragment").Type, 3);
				Main.recipe[i].AddIngredient(calamity.Find<ModItem>("LivingShard").Type, 12);
			}
			else if(Main.recipe[i].HasResult(thorium.Find<ModItem>("LustrousBaton").Type) || Main.recipe[i].HasIngredient(thorium.Find<ModItem>("UnfathomableFlesh").Type) || Main.recipe[i].HasIngredient(thorium.Find<ModItem>("GreenDragonScale").Type) || Main.recipe[i].HasIngredient(thorium.Find<ModItem>("CrystalGeode").Type) || Main.recipe[i].HasIngredient(thorium.Find<ModItem>("HallowedCharm").Type) || Main.recipe[i].HasIngredient(thorium.Find<ModItem>("ValadiumIngot").Type) || Main.recipe[i].HasIngredient(thorium.Find<ModItem>("LodeStoneIngot").Type)) {
				Main.recipe[i].RemoveTile(134);
				Main.recipe[i].AddTile(16);
			}
			else if(Main.recipe[i].HasResult(calamity.Find<ModItem>("DraedonsForge").Type)) {
				Main.recipe[i].requiredItem.Clear();
				Main.recipe[i].AddIngredient(calamity.Find<ModItem>("CosmicAnvilItem").Type);
				Main.recipe[i].AddIngredient(thorium.Find<ModItem>("SoulForge").Type);
				Main.recipe[i].AddIngredient(398);
				Main.recipe[i].AddIngredient(3549);
				Main.recipe[i].AddIngredient(calamity.Find<ModItem>("AuricBar").Type, 15);
				Main.recipe[i].AddIngredient(calamity.Find<ModItem>("ExoPrism").Type, 12);
				Main.recipe[i].AddIngredient(calamity.Find<ModItem>("AscendantSpiritEssence").Type, 25);
				if(ModContent.GetInstance<BalanceConfig>().expensiveDraedonsForge) Main.recipe[i].AddIngredient(thorium.Find<ModItem>("TerrariumCore").Type, 3);
			}
			else if(Main.recipe[i].HasResult(calamity.Find<ModItem>("ReaverCuisses").Type) || Main.recipe[i].HasResult(calamity.Find<ModItem>("ReaverScaleMail").Type) || Main.recipe[i].HasResult(calamity.Find<ModItem>("ReaverVisage").Type) || Main.recipe[i].HasResult(calamity.Find<ModItem>("ReaverHelm").Type) || Main.recipe[i].HasResult(calamity.Find<ModItem>("ReaverHeadgear").Type) || Main.recipe[i].HasResult(calamity.Find<ModItem>("StygianShield").Type)) Main.recipe[i].AddIngredient(thorium.Find<ModItem>("BloomWeave").Type, 2);
		}
	}
}