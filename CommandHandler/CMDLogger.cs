using System;
using Object = UnityEngine.Object;
using UnityEngine;
using Oc;
using Oc.Item;
using Oc.Item.UI;

namespace CommandHandler
{
    public class CMDLogger
    {
        public static string Final = CommandHandler.FinalizedText;
        public static int godModeToggle = 1;
        

        public static void Awake()
        {
            Debug.Log("Commands have been enabled");
        }


        public static void Commands(String cmd)
        {
            try
            {
                string cmdName = cmd.ToLower();
                string[] strArray = cmd.Split(' ');
                int result;
                  if (cmdName.Contains("spawnitem"))
                    {
                    if (int.TryParse(strArray[1], out result))
                    {
                        if(strArray.Length == 3)
                        {
                            Debug.Log("Spawned ITEM ID: " + strArray[1] + "AMOUNT: " + strArray[2]);
                            OcItem ocItem = SingletonMonoBehaviour<OcItemDataMng>.Inst.CreateItem(result, 0);
                            UISceneSingleton<OcItemUI_InventoryMng>.InstUISS.TryTakeItem(ocItem, Convert.ToInt32(strArray[2]), (OcItemUI_InventoryMng.ItemTakeTrigger)3);
                        } else if(strArray.Length == 2)
                        {
                            Debug.Log("Spawned ITEM ID: " + strArray[1]);
                            OcItem ocItem = SingletonMonoBehaviour<OcItemDataMng>.Inst.CreateItem(result, 0);
                            UISceneSingleton<OcItemUI_InventoryMng>.InstUISS.TryTakeItem(ocItem, 1, (OcItemUI_InventoryMng.ItemTakeTrigger)3);
                        } else
                        {
                            Debug.Log("Invalid amount of arguments.  Example: spawnitem [id] [amt]");
                        }
                    }
                    //ocItem.ForceSetLevel(ocItem.data.Refine_MaxLv);  -- Sets level to maximum refinement level (199 I believe)
                    Console.BackgroundColor = ConsoleColor.DarkRed;
                    Debug.Log("Item spawn enabled");
                    Console.BackgroundColor = ConsoleColor.Black;
                    return;
                }
                if (cmdName.Contains("balance"))
                {
                    Console.BackgroundColor = ConsoleColor.DarkRed;
                    long money = OcPlMaster.Inst.HealthPl.Money;
                    Debug.Log(money);
                    Console.BackgroundColor = ConsoleColor.Black;
                    return;
                }
                if (cmdName.Contains("addmoney"))
                {
                    long money = long.Parse(strArray[1]);
                    if (int.TryParse(strArray[1], out result))
                    {
                        if (strArray.Length >= 1)
                        {

                            OcPlMaster.Inst.HealthPl.AddMoney(money);
                        }
                        else
                        {
                            Debug.Log("IMPROPER ARGUMENTS, USE: addmoney [amt]");
                        }
                        Console.BackgroundColor = ConsoleColor.DarkRed;
                        Debug.Log("Added money");
                        Console.BackgroundColor = ConsoleColor.Black;

                        return;

                    }
                }
                if (cmdName.Contains("godmode"))
                {
                    godMe();
                    Console.BackgroundColor = ConsoleColor.DarkRed;
                    Debug.Log("Godmode");
                    Console.BackgroundColor = ConsoleColor.Black;
                    return;
                }
                if (cmdName.Contains("restore"))
                {
                    Console.BackgroundColor = ConsoleColor.DarkRed;
                    Debug.Log("Restored players health");
                    Console.BackgroundColor = ConsoleColor.Black;
                    return;
                }
            } 

            catch(Exception ex)
            {
                Debug.Log(ex);
            }
        }

        private static void godMe()
        {
            OcHealth[] health = Object.FindObjectsOfType(typeof(OcHealth)) as OcHealth[];
            if (godModeToggle == 1)
            {

                foreach (var t in health)
                {
                    t.setImmortal(true);
                }
                Debug.Log("Godmode ENABLED");
                godModeToggle = 2;
            }
            else if (godModeToggle == 2)
            {
                foreach (var t in health)
                {
                    t.setImmortal(false);
                }
                Debug.Log("Godmode DISABLED");
                godModeToggle = 1;
            }
        }
    }
}
