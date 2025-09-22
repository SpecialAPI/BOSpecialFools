global using System.Reflection;
global using System.IO;
global using System.Text;
global using System.Text.RegularExpressions;
global using System.Collections;
global using System.Collections.Generic;
global using System.Linq;

global using UnityEngine;

global using BepInEx;
global using HarmonyLib;

global using MonoMod;
global using MonoMod.Cil;
global using Mono.Cecil;
global using Mono.Cecil.Cil;

global using BrutalAPI;
global using Tools;

global using Pentacle;
global using Pentacle.Tools;
global using Pentacle.Builders;
global using Pentacle.TriggerEffects;
global using Pentacle.TriggerEffects.BasicTriggerEffects;
global using Pentacle.Triggers;
global using Pentacle.Triggers.Args;
global using Pentacle.Advanced;
global using Pentacle.Misc;
global using Pentacle.UnitExtension;
global using Pentacle.CombatActions;

global using static Pentacle.Builders.IntentBuilder;
global using static Pentacle.Builders.ItemBuilder;
global using static Pentacle.Builders.PassiveBuilder;
global using static Pentacle.Builders.StatusEffectBuilder;
global using static Pentacle.Builders.CharacterBuilder;
global using static Pentacle.Builders.StoredValueBuilder;
global using static Pentacle.Builders.AbilityBuilder;
global using static Pentacle.Builders.EnemyBuilder;

global using static Pentacle.Tools.ScriptableObjectTools;
global using static Pentacle.Tools.IntentTools;
global using static Pentacle.Tools.IntTools;

global using Grimoire.Content.StatusEffect;
global using Grimoire.Content.Passive;
global using Grimoire.Content.Intent;

global using Object = UnityEngine.Object;
global using Random = UnityEngine.Random;

global using static LoadedAssetsHandler;