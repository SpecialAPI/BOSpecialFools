using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialFools
{
    public static class RankedAbilitiesDatabase
    {
        public static readonly HashSet<AbilitySO>[] abilitiesByRank = [[], [], [], []];

        public static void Init()
        {
            foreach (var chId in LoadedDBsHandler.CharacterDB.CharactersList)
            {
                var ch = GetCharacter(chId);

                if (ch == null)
                    continue;

                if (ch.rankedData is List<CharacterRankedData> rankedData)
                {
                    for (var i = 0; i < Mathf.Min(rankedData.Count, abilitiesByRank.Length); i++)
                    {
                        var dataForRank = rankedData[i];

                        if (dataForRank == null || dataForRank.rankAbilities is not CharacterAbility[] abilities)
                            continue;

                        foreach (var ability in abilities)
                        {
                            if (ability == null || ability.ability == null)
                                continue;

                            abilitiesByRank[i].Add(ability.ability);

                            if (i == rankedData.Count - 1)
                            {
                                for (var j = rankedData.Count; j < abilitiesByRank.Length; j++)
                                    abilitiesByRank[j].Add(ability.ability);
                            }
                        }
                    }
                }

                if (ch.basicCharAbility != null && ch.basicCharAbility.ability != null)
                {
                    for (var i = 0; i < abilitiesByRank.Length; i++)
                        abilitiesByRank[i].Add(ch.basicCharAbility.ability);
                }
            }
        }

        public static List<AbilitySO> GetRandomAbilitiesOfRank(int rank, int amount)
        {
            if (rank < 0 || rank >= abilitiesByRank.Length)
                return [];

            var result = new List<AbilitySO>();
            var pool = new List<AbilitySO>(abilitiesByRank[rank]);

            while(result.Count < amount && pool.Count > 0)
            {
                var idx = Random.Range(0, pool.Count);
                result.Add(pool[idx]);
                pool.RemoveAt(idx);
            }

            return result;
        }
    }
}
