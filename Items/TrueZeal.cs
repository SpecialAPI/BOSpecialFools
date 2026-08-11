using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialFools.Items
{
    public static class TrueZeal
    {
        public static readonly string ID = "TrueZeal_ExtraW".Prefix();

        public static void Init()
        {
            var name = "True Zeal";
            var flav = "\"Talia plagiarized my mod.\"";
            var desc = "At the start of combat, destroy all Sacred Shrubs, Royal Pines, Arachnid Aphrodisiacs, Wailing Whistles, Cloth Cocks, Gift Box!es, Burn-Bottle Batches, Coelacanths, Little Clown Dolls, Abused Clown Dolls and True Zeals and instantly kills all Shelly K.s, Formosuses and Cadaver Synods. Produce a shop item for each shop item destroyed, a treasure item for each treasure item destroyed and a fish item for each fish item destroyed. Spawn a permanent party member for each party member killed and an enemy for each enemy killed.";

            var item = NewItem<MultiCustomTriggerEffectWearable>(ID)
                .SetBasicInformation(name, flav, desc, "TrueZeal")
                .SetPrice(14)
                .AddWithoutItemPools();

            var charBlacklist = new List<string>()
            {
                "LongLiver_CH",
                "Clive_CH",
                "Cranes_CH",
                "Gospel_CH",
                "Mordrake_CH",

                "ShellyK_CH",
                "Formosus_CH"
            };
            var itemBlacklist = new List<string>()
            {
                "SacredShrub_TW",
                "RoyalPine_TW",
                "ArachnidAphrodisiac_TW",
                "WailingWhistle_SW",
                "ClothCock_SW",
                "GiftBox_SW",
                "BurnBottleBatch_SW",
                "Coelacanth_ExtraW",
                "LittleClownDoll_ExtraW",
                "AbusedClownDoll_ExtraW",
                "TrueZeal_TW",
                "TrueZeal_SW",
                "TrueZeal_ExtraW",
                ID
            };

            var genTreasure = Effects.GenerateEffect(CreateScriptable<ExtraLootWithBlacklistEffect>(x =>
            {
                x.blacklist = itemBlacklist;
                x.treasure = true;
            }), 1);
            var genShop = Effects.GenerateEffect(CreateScriptable<ExtraLootWithBlacklistEffect>(x =>
            {
                x.blacklist = itemBlacklist;
                x.treasure = false;
            }), 1);
            var genFish = Effects.GenerateEffect(CreateScriptable<ExtraLootListWithBlacklistEffect>(x =>
            {
                x.blacklist = itemBlacklist;
                x.lootPool = ItemUtils.GetLootPool(PoolList_GameIDs.CanOfWorms_WelsCatfish.ToString());
            }), 1);
            var genBleach = Effects.GenerateEffect(CreateScriptable<ExtraLootOptionsEffect>(x => x._itemName = OldBleach.ID));
            var genZeal = Effects.GenerateEffect(CreateScriptable<ExtraLootOptionsEffect>(x => x._itemName = ID));

            var spawnRandomChar = Effects.GenerateEffect(CreateScriptable<SpawnRandomCharacterWithBlacklistEffect>(x =>
            {
                x.blacklist = charBlacklist;

                x.rankIsPreviousExit = true;
                x.nameAddition = NameAdditionLocID.NameAdditionNone;
                x.permanent = true;
            }), 1);
            var spawnSepulchre = Effects.GenerateEffect(CreateScriptable<SpawnEnemyAnywhereEffect>(x =>
            {
                x.enemy = LoadEnemy("Sepulchre_EN");
                x.givesExperience = true;
                x._spawnTypeID = CombatType_GameIDs.Spawn_Basic.ToString();
            }));

            item.SetTriggerEffects(new()
            {
                new()
                {
                    trigger = TriggerCalls.OnCombatStart.ToString(),
                    immediate = false,
                    doesPopup = true,

                    effect = new PerformEffectTriggerEffect(new()
                    {
                        Effects.GenerateEffect(CreateScriptable<DestroyItemsWithIDPerformEffectOnSuccessEffect>(x =>
                        {
                            x.itemIDs = ["SacredShrub_TW", "RoyalPine_TW", "ArachnidAphrodisiac_TW"];
                            x.effects = [genTreasure];
                        }), 0, Targeting.AllUnits),
                        Effects.GenerateEffect(CreateScriptable<DestroyItemsWithIDPerformEffectOnSuccessEffect>(x =>
                        {
                            x.itemIDs = ["WailingWhistle_SW"];
                            x.effects = [genBleach];
                        }), 0, Targeting.AllUnits),
                        Effects.GenerateEffect(CreateScriptable<DestroyItemsWithIDPerformEffectOnSuccessEffect>(x =>
                        {
                            x.itemIDs = ["ClothCock_SW", "GiftBox_SW", "BurnBottleBatch_SW"];
                            x.effects = [genShop];
                        }), 0, Targeting.AllUnits),
                        Effects.GenerateEffect(CreateScriptable<DestroyItemsWithIDPerformEffectOnSuccessEffect>(x =>
                        {
                            x.itemIDs = ["Coelacanth_ExtraW"];
                            x.effects = [genFish];
                        }), 0, Targeting.AllUnits),
                        Effects.GenerateEffect(CreateScriptable<DestroyItemsWithIDPerformEffectOnSuccessEffect>(x =>
                        {
                            x.itemIDs = ["TrueZeal_TW", "TrueZeal_SW", "TrueZeal_ExtraW"];
                            x.effects = [genZeal];
                        }), 0, Targeting.AllUnits),
                        Effects.GenerateEffect(CreateScriptable<DestroyItemsWithIDPerformEffectOnSuccessEffect>(x =>
                        {
                            x.itemIDs = [item.name];
                            x.effects = [genTreasure];
                        }), 0, Targeting.AllUnits),

                        Effects.GenerateEffect(CreateScriptable<KillTargetsPerformEffectOnSuccessEffect>(x =>
                        {
                            x.effects = [spawnRandomChar];
                            x.startResultIsKilledRank = true;
                        }), 0, Targeting.AllUnits.FilterUnitByEntityID(Entity_GameIDs.ShellyK.ToString(), Entity_GameIDs.Formosus.ToString())),
                        Effects.GenerateEffect(CreateScriptable<KillTargetsPerformEffectOnSuccessEffect>(x =>
                        {
                            x.effects = [spawnSepulchre];
                        }), 0, Targeting.AllUnits.FilterUnitByEntityID("CadaverSynod_EN")),

                        Effects.GenerateEffect(CreateScriptable<FleeTargetEffect>(), 0, Targeting.AllUnits.FilterUnitByEntityID(Entity_GameIDs.Gospel.ToString()))
                    })
                }
            });
        }
    }
}
