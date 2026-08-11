using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialFools.Misc
{
    [HarmonyPatch]
    public abstract class AdvancedModdedPostCombatResult : ModdedPostCombatResult
    {
        public override void ApplyPostCombatResult(GameInformationHolder info)
        { 
        }

        public abstract void AdvancedApplyPostCombatResult(GameInformationHolder info, PostCombatResultsExtraData extraData);

        [HarmonyPatch(typeof(GameInformationHolder), nameof(GameInformationHolder.PostCombatProcess))]
        [HarmonyILManipulator]
        private static void ProcessAdvancedPostCombatResults_Transpiler(ILContext ctx)
        {
            var crs = new ILCursor(ctx);

            if (!crs.JumpToNext(x => x.MatchCallOrCallvirt<ModdedPostCombatResult>(nameof(ModdedPostCombatResult.ApplyPostCombatResult))))
                return;

                                            // curr: void

            crs.Emit(OpCodes.Ldloc, 37);    // enumerator
            crs.Emit(OpCodes.Ldarg_0);      // infoHolder

            crs.Emit(OpCodes.Ldloc, 8);     // characterAdditions
            crs.Emit(OpCodes.Ldloc_2);      // casualtiesInfo
            crs.Emit(OpCodes.Ldloc, 9);     // lootedItemsInfo
            crs.Emit(OpCodes.Ldloc, 10);    // lootedItems
            crs.Emit(OpCodes.Ldloca, 12);   // ref gainedCurrency
            crs.Emit(OpCodes.Ldloca, 13);   // ref playerCurrency

            crs.EmitStaticDelegate(ProcessAdvancedPostCombatResults_ProcessCurrentAdvancedResult); // push: void
        }

        private static void ProcessAdvancedPostCombatResults_ProcessCurrentAdvancedResult(List<ModdedPostCombatResult>.Enumerator enumerator, GameInformationHolder infoHolder, List<SpawnedCharacterAddition> characterAdditions, List<CombatLootData> casualtiesInfo, List<CombatLootData> lootedItemsInfo, List<BaseWearableSO> lootedItems, ref int gainedCurrency, ref int playerCurrency)
        {
            var currResult = enumerator.Current;

            if (currResult is not AdvancedModdedPostCombatResult advancedRes)
                return;

            var extraDat = new PostCombatResultsExtraData()
            {
                characterAdditions = characterAdditions,
                casualtiesInfo = casualtiesInfo,
                lootedItemsInfo = lootedItemsInfo,
                lootedItems = lootedItems,
                gainedCurrency = gainedCurrency,
                playerCurrency = playerCurrency
            };
            advancedRes.AdvancedApplyPostCombatResult(infoHolder, extraDat);

            gainedCurrency = extraDat.gainedCurrency;
            playerCurrency = extraDat.playerCurrency;
        }
    }

    public class PostCombatResultsExtraData
    {
        public List<SpawnedCharacterAddition> characterAdditions;
        public List<CombatLootData> casualtiesInfo;
        public List<CombatLootData> lootedItemsInfo;
        public List<BaseWearableSO> lootedItems;
        public int gainedCurrency;
        public int playerCurrency;

        public void AddItem(BaseWearableSO item)
        {
            lootedItems.Add(item);
            lootedItemsInfo.Add(new(item));
        }

        public void AddCharacter(SpawnedCharacterAddition character)
        {
            characterAdditions.Add(character);
        }
    }
}
