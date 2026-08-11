using System;
using System.Collections.Generic;

namespace BOSpecialFools.Effect
{
    public class SpawnRandomCharacterWithBlacklistEffect : EffectSO
    {
        public List<string> blacklist = [];
        public bool permanent = false;
        public NameAdditionLocID nameAddition = NameAdditionLocID.NameAdditionNone;
        public int rank = 0;
        public bool rankIsPreviousExit = false;

        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            exitAmount = 0;

            var chars = GetCharacters(entryVariable);
            var nameAddData = LocUtils.GameLoc.GetNameAdditionData(nameAddition);

            foreach(var ch in chars)
            {
                var chRank = rank;
                if (rankIsPreviousExit)
                    chRank = PreviousExitValue;

                CombatManager.Instance.AddSubAction(new SpawnCharacterAction(ch, -1, false, nameAddData, permanent, chRank, ch.GenerateAbilities(), ch.GetMaxHealth(chRank)));
                exitAmount++;
            }

            return exitAmount > 0;
        }

        public List<CharacterSO> GetCharacters(int amount)
        {
            var charDB = LoadedDBsHandler.CharacterDB;

            var charList = charDB.CharactersList.ToList();
            var result = new List<CharacterSO>();

            while (amount > 0 && charList.Count > 0)
            {
                var charID = charList.GetAndRemoveRandomElement();

                if(blacklist != null && blacklist.Contains(charID))
                    continue;

                var character = GetCharacter(charID);
                if (character == null)
                    continue;

                result.Add(character);
                amount--;
            }

            return result;
        }
    }
}
