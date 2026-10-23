namespace BOSpecialFools.Characters
{
    public static class Halcyon
    {
        public static readonly string ID = "Halcyon_CH".Prefix();
        public static readonly string EntityID = "Halcyon".Prefix();
        public static readonly string[] AbilityAIDs = "HalcyonA_{0}_A".Prefix().GenerateLevels();
        public static readonly string[] AbilityBIDs = "HalcyonB_{0}_A".Prefix().GenerateLevels();
        public static readonly string[] AbilityCIDs = "HalcyonC_{0}_A".Prefix().GenerateLevels();
        
        public static readonly string ForecastDBID = "Forecast_PA".Prefix();
        public static readonly string ForecastPassiveID = "Forecast".Prefix();
        public static readonly string ForecastSVID = "Forecast".Prefix();

        public static void Init()
        {
            var forecast = NewPassive<MultiCustomTriggerEffectPassive>(ForecastDBID, ForecastPassiveID)
                .SetBasicInformation("Forecast", "Forecast")
                .AutoSetDescriptions("Permanently applies Insight to all opponents.")
                .AddToGlossary("Permanently applies Insight to all party members/enemies.")
                .AddToDatabase();

            var forecastSV = NewStoredValue<UnitStoreData_BasicSO>(ForecastSVID);
            forecast.AddEffectsToAll(new TriggerEffectAndTriggersInfo()
            {
                triggers = [TriggerCalls.OnOpponentHasSpawned.ToString()],
                immediate = false,
                doesPopup = true,
                
                effect = new PermaStatusToAllUnitsTriggerEffect(GrimoireStatusField.Insight, ForecastSVID, false, true)
            });
            
            var ch = NewCharacter(ID, EntityID)
                .SetBasicInformation("Halcyon", Pigments.Blue, "HalcyonFront", "HalcyonBack", "HalcyonOW")
                .AddPassives(forecast)
                .AddUnitTypes(UnitTypes.FemaleLooking, UnitTypes.FemaleID, UnitTypes.Sandwich_Pigment);

            ch.RankedDataSetup(4, (rank, abilityRank) =>
            {
                var health = RankedValue(15, 16, 17, 19);

                return new(health, []);
            });

            ch.AddToDatabase();
            
            //var menu = ch.GenerateMenuCharacter("HalcyonUnlocked", "HalcyonLocked");
            //menu.SetAsFullDPS();
            //menu.AddToDatabase();
        }
    }
}