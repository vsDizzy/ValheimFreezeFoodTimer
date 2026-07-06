// SPDX-License-Identifier: MIT
// Copyright (c) 2026 vsDizzy

using System;
using HarmonyLib;
using UnityEngine;
using BepInEx;
using System.Collections.Generic;

namespace Valheim
{
    [BepInPlugin("com.vortex.valheim.freezefoodtimer", "Valheim Freeze Food Timer", "${VERSION}")]
    public class FreezeFoodTimerPlugin : BaseUnityPlugin
    {
        private void Awake()
        {
            FreezeFoodTimer.Main();
            Logger.LogInfo("Freeze Food Timer plugin initialized!");
        }

        private void OnDestroy()
        {
            FreezeFoodTimer.Unload();
        }
    }

    public static class FreezeFoodTimer
    {
        private static Harmony harmony;

        public static void Main()
        {
            if (harmony != null) return;
            harmony = new Harmony("com.vortex.valheim.freezefoodtimer");
            harmony.PatchAll(typeof(FreezeFoodTimer));
        }

        public static void Unload()
        {
            harmony?.UnpatchSelf();
            harmony = null;
        }

        [HarmonyPatch(typeof(Player), "UpdateFood")]
        [HarmonyPostfix]
        public static void PostfixUpdateFood(Player __instance)
        {
            var foods = __instance?.GetFoods();
            if (foods == null) return;

            foreach (var food in foods)
            {
                if (food?.m_item?.m_shared != null)
                {
                    // Reset food time to max duration to freeze the timer
                    food.m_time = food.m_item.m_shared.m_foodBurnTime;
                }
            }
        }
    }
}