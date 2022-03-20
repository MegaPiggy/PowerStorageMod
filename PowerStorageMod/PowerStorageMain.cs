using SRML;
using SRML.SR;
using SRML.SR.SaveSystem;
using SRML.SR.Translation;
using SRML.Utils;
using System.Reflection;
using UnityEngine;

namespace PowerStorageMod
{
    public class PowerStorageMain : ModEntryPoint
    {
        private static Assembly execAssembly;

        public override void PreLoad()
        {
            execAssembly = Assembly.GetExecutingAssembly();
            HarmonyInstance.PatchAll(execAssembly);
            PowerStorageIds.ENERGY_GENERATOR.GetTranslation().SetNameTranslation("Energy Generator").SetDescriptionTranslation("An engine that generates energy for quicksilver slimes.");
            GadgetRegistry.ClassifyGadget(PowerStorageIds.ENERGY_GENERATOR, GadgetRegistry.GadgetClassification.MISC);
        }

        public override void Load()
        {
            GameObject powerStorage = GameObject.CreatePrimitive(PrimitiveType.Cube);
            GameObjectUtils.Prefabitize(powerStorage);
            PowerStorageGadget psg = powerStorage.AddComponent<PowerStorageGadget>();
            Sprite icon = SRSingleton<GameContext>.Instance.LookupDirector.GetGadgetDefinition(Gadget.Id.TELEPORTER_GOLD).icon;
            GadgetDefinition prefab = ScriptableObject.CreateInstance<GadgetDefinition>();
            prefab.id = PowerStorageIds.ENERGY_GENERATOR;
            prefab.pediaLink = PediaDirector.Id.UTILITIES;
            prefab.blueprintCost = 30000;
            prefab.buyCountLimit = -1;
            prefab.icon = icon;
            prefab.craftCosts = new GadgetDefinition.CraftCost[]{
				new GadgetDefinition.CraftCost
				{
					id = Identifiable.Id.QUICKSILVER_PLORT,
					amount = 12
				},
				new GadgetDefinition.CraftCost
				{
					id = Identifiable.Id.QUANTUM_PLORT,
					amount = 12
				}
			};
            LookupRegistry.RegisterGadget(prefab);
        }

        public override void PostLoad()
        {
            GadgetRegistry.RegisterBlueprintLock(PowerStorageIds.ENERGY_GENERATOR, x => x.CreateBasicLock(PowerStorageIds.ENERGY_GENERATOR, Gadget.Id.NONE, ProgressDirector.ProgressType.MOCHI_SEEN_FINAL_CHAT, 3f));
        }

    }
}
