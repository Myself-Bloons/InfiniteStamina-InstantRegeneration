using BTD_Mod_Helper;
using BTD_Mod_Helper.Api.ModOptions;
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
    public static readonly ModSettingHotkey RestoreStaminaHotkey = new(KeyCode.F9)
    {
        displayName = "Restore Stamina",
        description = "Press to instantly restore all posse stamina to maximum"
    };

    public static readonly ModSettingHotkey ToggleInfiniteHotkey = new(KeyCode.F10, HotkeyModifier.Shift)
    {
        displayName = "Toggle Infinite Stamina",
        description = "Press Shift+F10 to toggle infinite stamina mode"
    };

    public static readonly ModSettingBool InfiniteStaminaEnabled = new(false)
    {
        displayName = "Infinite Stamina",
        description = "When enabled, stamina never depletes",
        button = true
    };

    public override void OnUpdate()
    {
        var manager = FrontierLegendsManager.instance;
        if (manager == null) return;

        if (RestoreStaminaHotkey.JustPressed())
        {
            manager.HealPartyToPercent(1.0f);
            manager.AddHomesteadStamina(9999f, 1.0f);
        }

        if (ToggleInfiniteHotkey.JustPressed())
        {
            InfiniteStaminaEnabled.SetValueAndSave(!InfiniteStaminaEnabled);
        }

        if (InfiniteStaminaEnabled && manager.Modifiers != null)
        {
            manager.Modifiers.staminaCost = 0f;
        }
    }
}
