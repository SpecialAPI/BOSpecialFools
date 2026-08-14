using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialFools
{
    public static class FreeFoolEvents
    {
        private static readonly string RoomNameFormat       = "157Fools_FreeFool_{0}_ER";
        private static readonly string DialogueFileFormat   = "{0}Dialogue";
        private static readonly string DialogueNameFormat   = "FreeFool_{0}_Dialogue".Prefix();
        private static readonly string DialogueStartFormat  = "157Fools_{0}Dialogue_FreeFool";
        private static readonly string SpeakerIDFormat      = "{0}_SpeakerData".Prefix();
        private static readonly string EventIDFormat        = "{0}_FreeFoolEncounter".Prefix();

        public static void Init()
        {
            MakeFreeFool(Wreath.ID, "Wreath", PortraitDirection.LooksLeft, new Color32(91, 65, 90, 255), [Zones.ShoreHardID]);
        }

        public static void MakeFreeFool(string characterID, string characterName, PortraitDirection portraitDir, Color textColor, string[] zones)
        {
            var ch = GetCharacter(characterID);

            // Room
            var roomName = string.Format(RoomNameFormat, characterName);
            var roomGo = Bundle.LoadAsset<GameObject>(roomName);
            var room = roomGo.AddComponent<NPCRoomHandler>();

            var chInteractableDat = roomGo.GetComponentInChildren<Basic_RoomItemModData>();
            var chInteractable = chInteractableDat.AddComponent<BasicRoomItem>();
            chInteractable.FillWithModData(chInteractableDat);
            chInteractable.SetMaterials(LoadedDBsHandler.MiscDB.GetMaterial(BrutalAPI.Misc.MaterialIDs.Outline.ToString()));

            room._npcSelectable = chInteractable;
            TryAddExternalOWRoom(room.name, room);

            // Dialogue
            var dialogueFile = string.Format(DialogueFileFormat, characterName);
            var dialogueName = string.Format(DialogueNameFormat, characterName);
            var dialogueStart = string.Format(DialogueStartFormat, characterName);
            var program = Bundle.LoadAsset<YarnProgram>(dialogueFile);
            var dialogue = Dialogues.CreateAndAddCustom_DialogueSO(dialogueName, program, characterName.Prefix(), dialogueStart);

            // Speaker
            var speaker = CreateScriptable<SpeakerData>();
            speaker.speakerName = characterName;
            speaker.portraitLooksLeft = portraitDir == PortraitDirection.LooksLeft;
            speaker.portraitLooksCenter = portraitDir == PortraitDirection.LooksCenter;
            speaker._defaultBundle = new()
            {
                portrait = ch.characterSprite,
                dialogueSound = ch.dxSound,
                bundleTextColor = textColor,
            };
            speaker._emotionBundles = [];
            speaker.name = string.Format(SpeakerIDFormat, characterName);
            LoadedDBsHandler.DialogueDB.AddNewSpeakerData(speaker.name, speaker);

            // Sign
            var signID = characterName.Prefix();
            LoadedDBsHandler.PortalDB.AddNewPortalSign(signID, ch.characterOWSprite, PortalType_GameIDs.NPC.ToString());

            // Event
            var freefool = CreateScriptable<FreeFoolEncounterSO>();
            freefool._freeFool = ch.name;
            freefool._dialogue = dialogue.name;
            freefool.encounterRoom = room.name;
            freefool.encounterEntityIDs = [ch.entityID];
            freefool.signID = signID;
            freefool.name = string.Format(EventIDFormat, characterName);
            TryAddExternalFreeFoolEncounter(freefool.name, freefool);

            foreach(var zoneID in zones)
            {
                var zone = GetZoneDB(zoneID);

                if (zone == null)
                    continue;

                if (zone is not ZoneBGDataBaseSO zoneDB)
                    continue;

                zoneDB._FreeFoolsPool.Add(freefool.name);
            }
        }

        public enum PortraitDirection
        {
            LooksLeft,
            LooksRight,
            LooksCenter
        }
    }
}
