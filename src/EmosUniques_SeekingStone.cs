using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using SideLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace OutwardModTemplate
{
    [BepInPlugin(GUID, NAME, VERSION)]
    public class EmosUniques_SeekingStone : BaseUnityPlugin
    {
        // Choose a GUID for your project. Change "myname" and "mymod".
        public const string GUID = "EmoUniques.seekingstone";
        // Choose a NAME for your project, generally the same as your Assembly Name.
        public const string NAME = "Emo's Uniques - Seeking Stone";
        // Increment the VERSION when you release a new version of your mod.
        public const string VERSION = "1.0.0";

        // For accessing your BepInEx Logger from outside of this class (MyMod.Log)
        internal static ManualLogSource Log;

        public const string EXTRA_ACTION_KEY_MOD = "Emo_ExtraAction_Toggle_Modifier";
        public const string EXTRA_ACTION_KEY = "Emo_ExtraAction_Key";

        // Awake is called when your plugin is created. Use this to set up your mod.
        internal void Awake()
        {
            Log = this.Logger;

            // Harmony is for patching methods. If you're not patching anything, you can comment-out or delete this line.
            //new Harmony(GUID).PatchAll();

            SL.OnPacksLoaded += SetUpItem;

            CustomKeybindings.AddAction(EXTRA_ACTION_KEY_MOD, KeybindingsCategory.CustomKeybindings, ControlType.Both, InputType.Button);
            CustomKeybindings.AddAction(EXTRA_ACTION_KEY, KeybindingsCategory.CustomKeybindings, ControlType.Both, InputType.Button);
        }


        private void SetUpItem()
        {
            SL_Equipment SeekingStone = new SL_Equipment()
            {
                Target_ItemID = 5100500,
                New_ItemID = -110005,
                Name = "Seeking Stone",
                Description = "Slowly vibrates in your hand and glows when it has detected something.",
                ItemVisuals = new SL_ItemVisual()
                {
                    Prefab_SLPack = "EmosUniques-SeekingStone",
                    Prefab_AssetBundle = "emoseekingstone",
                    Prefab_Name = "SeekingStone",
                    Position = new Vector3(0.073f, -0.015f, 0.094f),
                    Rotation = new Vector3(67.56058f, 138.624161f, 119.654831f)
                }
            };

            SeekingStone.OnTemplateApplied += (item) =>
            {
                EmoSeekingStone emoSeekingStone =  CheckOrAddComponent<EmoSeekingStone>(item.gameObject);

            };

            SeekingStone.ApplyTemplate();
        }

        public static T CheckOrAddComponent<T>(GameObject gameObject) where T : Component
        {
            T comp = gameObject.GetComponent<T>();

            if (comp == null)
            {
                return gameObject.AddComponent<T>();

            }

            return comp;
        }
    }


    //Just an enum Extension to get the next value
    public static class Extensions
    {
        public static T Next<T>(this T src) where T : struct
        {
            if (!typeof(T).IsEnum) throw new ArgumentException(String.Format("Argument {0} is not an Enum", typeof(T).FullName));

            T[] Arr = (T[])Enum.GetValues(src.GetType());
            int j = Array.IndexOf<T>(Arr, src) + 1;
            return (Arr.Length == j) ? Arr[0] : Arr[j];
        }
    }
}
