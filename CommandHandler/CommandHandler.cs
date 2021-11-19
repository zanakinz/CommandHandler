using BepInEx;
using HarmonyLib;
using System;
using UnityEngine;
using Oc.OcInput;
using Cursor = UnityEngine.Cursor;

namespace CommandHandler
{
    [BepInPlugin(pluginGuid, pluginName, pluginVersion)]
    public class CommandHandler : BaseUnityPlugin
    {
        public const string pluginGuid = "zanakinz.craftopia.CommandHandler";
        public const string pluginName = "Command Handler";
        public const string pluginVersion = "0.0.0.1";

        public static bool openChat = false;
        public static bool isChatOpen = false;
        private GUIStyle currentStyle = null;
        public string stringToEdit = "";
        public static string FinalizedText = "";
        public static string eFinalizedText = "";
        public string lastString = "";


        public void Awake()
        {
            try
            {
                Console.BackgroundColor = ConsoleColor.DarkGreen;
                Logger.LogInfo("Command Handler by Zanakinz - For Gamepass");
                CMDLogger.Awake();
                Console.BackgroundColor = ConsoleColor.Black;


                //pAction = GetComponent<PlayerAction.MenuActions>();


                new Harmony("Zanakinz.Craftopia.CommandHandler").PatchAll();
            }
                catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }

        //intentionally made it not in Update for code cleanup in the future
        public void ButtonTriggers()
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                Cursor.lockState = (CursorLockMode)0;
                Logger.LogInfo("ENTER Pressed");
                stringToEdit = "";
                CommandHandler.isChatOpen = true;
                return;
            }
            else
            {
                CommandHandler.openChat = !CommandHandler.openChat;
            }
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                stringToEdit = lastString;
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                //add code for when window is closed
                Logger.LogInfo("PLUS Pressed");
                CommandHandler.isChatOpen = false;
                Logger.LogInfo(FinalizedText);
                return;
            }
        }
        public void Update()
        {
            ButtonTriggers();
        }

        public static Vector3 GUIScale
        {
            get
            {
                float normalWidth = 1920; //Whatever design resolution you want
                float normalHeight = 1080;
                return new Vector3(Screen.width / normalWidth, Screen.height / normalHeight, 1);
            }
        }


        public static Matrix4x4 AdjustedMatrix
        {
            get
            {
                return Matrix4x4.TRS(Vector3.zero, Quaternion.identity, GUIScale);
            }
        }

        private static void DisablePlayerInteraction()
        {
            SingletonMonoBehaviour<OcInputTrack>.Inst.DisableMenuControll();

        }
        private void OnGUI()
        {
            if (CommandHandler.isChatOpen)
            {

                Cursor.visible = true;
                // down/up, up/down, upper right-hand corner, bottom left hand corner
                InitStyles();
                GUI.matrix = AdjustedMatrix;

                Rect windowRect = new Rect(500, 775, 925, 50);
                windowRect = GUI.Window(0, windowRect, myGUI, "Command Handler", currentStyle);
                //GUI.Box(new Rect(500, 775, 925, 50), "Command Handler", currentStyle);
                Cursor.lockState = CursorLockMode.None;

            }
        }

        public static bool ExteriorMod = false;
        private void myGUI(int windowID)
        {
            GUIStyle textStyle = new GUIStyle(GUI.skin.textField);
            textStyle.normal.textColor = Color.black;
            GUI.SetNextControlName("TextField");
            stringToEdit = GUILayout.TextField(stringToEdit, textStyle);
            GUI.FocusControl("TextField");

            if (Event.current.Equals(Event.KeyboardEvent("None")) && ExteriorMod)
            {
                eFinalizedText = stringToEdit.ToString();
                lastString = eFinalizedText;
                Logger.LogInfo("COMMAND EXECUTED:" + eFinalizedText);
                SingletonMonoBehaviour<OcInputTrack>.Inst.EnableMenuControll();
                Cursor.lockState = CursorLockMode.Locked;
                CommandHandler.isChatOpen = false;
            }
            if(Event.current.Equals(Event.KeyboardEvent("None")))
            {
                FinalizedText = stringToEdit.ToString();
                lastString = FinalizedText;
                CMDLogger.Commands(FinalizedText);
                Logger.LogInfo("COMMAND EXECUTED:" + FinalizedText);
                SingletonMonoBehaviour<OcInputTrack>.Inst.EnableMenuControll();
                Cursor.lockState = CursorLockMode.Locked;
                CommandHandler.isChatOpen = false;
            }
            DisablePlayerInteraction();
        }

        public static void ExternalMod()
        {
            ExteriorMod = false;
            eFinalizedText = "";
            FinalizedText = "";
            return;
        }
        private void InitStyles()
        {
            if (currentStyle == null)
            {
                currentStyle = new GUIStyle(GUI.skin.window);
                currentStyle.richText = true;
                currentStyle.normal.textColor = Color.red;
                currentStyle.normal.background = MakeTex(2, 2, new Color(0.5f, 0.5f, 0.5f, 1f));
            }
        }

        private Texture2D MakeTex(int width, int height, Color col)
        {
            Color[] pix = new Color[width * height];
            for (int i = 0; i < pix.Length; ++i)
            {
                pix[i] = col;
            }
            Texture2D result = new Texture2D(width, height);
            result.SetPixels(pix);
            result.Apply();
            return result;
        }

    }
}
