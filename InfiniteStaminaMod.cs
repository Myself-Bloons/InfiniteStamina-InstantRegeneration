using BTD_Mod_Helper;
using BTD_Mod_Helper.Api.ModOptions;
using HarmonyLib;
using Il2CppAssets.Scripts.Unity.UI_New.InGame;
using Il2CppAssets.Scripts.Unity.UI_New.Legends;
using InfiniteStamina;
using MelonLoader;
using UnityEngine;

[assembly: MelonInfo(typeof(InfiniteStaminaMod), ModHelperData.Name, ModHelperData.Version, ModHelperData.RepoOwner)]
[assembly: MelonGame("Ninja Kiwi", "BloonsTD6")]

namespace InfiniteStamina;

public class InfiniteStaminaMod : BloonsTD6Mod
{
    public static readonly ModSettingHotkey RestoreStamina = new(KeyCode.F9);
    public static readonly ModSettingHotkey ToggleInfiniteStamina = new(KeyCode.F10, HotkeyModifier.Shift);
    public static readonly ModSettingBool InfiniteStaminaEnabled = new(false) { button = true };

    [HarmonyPatch(typeof(FrontierMap), nameof(FrontierMap.Update))]
    internal static class FrontierMap_Update
    {
        [HarmonyPostfix]
        internal static void Postfix()
        {
            var manager = FrontierLegendsManager.instance;
            if (manager is null) return;

            if (RestoreStamina.JustPressed())
            {
                manager.HealPartyToPercent(1f);
                manager.AddHomesteadStamina(9999f, 1f);
            }

            if (ToggleInfiniteStamina.JustPressed())
                InfiniteStaminaEnabled.SetValue(!InfiniteStaminaEnabled);

            if (InfiniteStaminaEnabled)
                manager.Modifiers.staminaCost = 0f;
        }
    }
}
