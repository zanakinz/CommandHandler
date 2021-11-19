using HarmonyLib;
using Oc.Item;
using System;
using System.IO;
using System.Text;
using UnityEngine;

namespace CommandHandler
{
    [HarmonyPatch(typeof(OcItemDataMng))]
    [HarmonyPatch("SetupCraftableItems")]
    internal static class CreateItemIdFile
    {
        private static bool Prefix(OcItemDataMng __instance, ref ItemData[] ___validItemDataList)
        {
            try
            {
                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ItemId.txt");
                if (File.Exists(path))
                    File.Delete(path);
                using (StreamWriter streamWriter = new StreamWriter(path, true, Encoding.UTF8))
                {
                    foreach (ItemData itemData in ___validItemDataList)
                        streamWriter.WriteLine(string.Format("{0} {1}", (object)itemData.Id, (object)itemData.DisplayName));
                }
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
            return true;
        }
    }
}
