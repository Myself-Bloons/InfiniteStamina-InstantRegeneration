using BTD_Mod_Helper;
using BTD_Mod_Helper.Api.ModOptions;
using HarmonyLib;
using Il2CppAssets.Scripts.Unity.UI_New.Legends;
using MelonLoader;
using UnityEngine;

[assembly: MelonInfo(typeof(InfiniteStamina.InfiniteStaminaMod), InfiniteStamina.ModHelperData.Name,
    InfiniteStamina.ModHelperData.Version, InfiniteStamina.ModHelperData.RepoOwner)]
[assembly: MelonGame("Ninja Kiwi", "BloonsTD6")]

namespace InfiniteStamina;

public class InfiniteStaminaMod : BloonsTD6Mod
{
    public static readonly ModSettingBool InfiniteStaminaEnabled = new(false)
    {
        button = true,
        description = "Eliminates all stamina costs on the Frontier map",
    };

    public static readonly ModSettingBool InstantRegenEnabled = new(false)
    {
        button = true,
        description = "Massively increases passive stamina regeneration speed",
    };

    public static readonly ModSettingHotkey RestoreStamina = new(KeyCode.F9)
    {
        description = "Instantly restores all party stamina and revives knocked monkeys",
    };

    public override void OnUpdate()
    {
        if (!RestoreStamina.JustPressed()) return;

        var manager = FrontierLegendsManager.instance;
        if (manager is null) return;

        manager.ClearKnockedMonkeys();
        manager.HealPartyToPercent(1f);
        manager.AddHomesteadStamina(9999f, true);
    }

    [HarmonyPatch(typeof(FrontierLegendsManager), nameof(FrontierLegendsManager.UpdateModifiers))]
    internal static class FrontierLegendsManager_UpdateModifiers
    {
        [HarmonyPostfix]
        internal static void Postfix(FrontierLegendsManager __instance)
        {
            if (!InfiniteStaminaEnabled && !InstantRegenEnabled) return;

            var modifiers = __instance.Modifiers;
            if (modifiers is null) return;

            if (InfiniteStaminaEnabled)
                modifiers.staminaCost = 0f;

            if (InstantRegenEnabled)
                modifiers.passiveStaminaRegenMultiplier = 9999f;
        }
    }
}
