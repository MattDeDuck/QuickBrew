using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace QuickBrew
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, "1.0.0.0")]
    public class Plugin : BaseUnityPlugin
    {
        public ManualLogSource Log { get; set; }
        public string pluginLoc = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        // List of gameobjects created
        public static List<GameObject> brewPotions = new();

        // Holds the current page number
        public static int curPageNumber = 0;

        public static int amountOfPages;

        // Brew panel GO reference
        public static GameObject QuickBrewPanel;
        public static GameObject QuickBrewNexPageButton;
        public static GameObject QuickBrewPrevButton;

        // Dictionary for current potions being displayed on the page
        public static Dictionary<int, Potion> potionsOnPage = new();
        
        // Brew panel bool controls
        public static bool brewPanelCreated = false;
        public static bool brewPanelactive = false;

        // Texture and sprite setup
        public Texture2D qb_brew_button_texture;
        public static Sprite qb_brew_button_sprite;
        public Texture2D qb_brew_panel_texture;
        public static Sprite qb_brew_panel_sprite;
        public Texture2D qb_brew__button_background_texture;
        public static Sprite qb_brew__button_background_sprite;
        public Texture2D qb_page_next_texture;
        public static Sprite qb_page_next_sprite;
        public Texture2D qb_page_prev_texture;
        public static Sprite qb_page_prev_sprite;

        private void Awake()
        {
            Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");

            Log = this.Logger;

            qb_brew_button_texture = LoadTextureFromFile(pluginLoc + "/qb_button.png");
            qb_brew_button_sprite = Sprite.Create(qb_brew_button_texture, new Rect(0, 0, qb_brew_button_texture.width, qb_brew_button_texture.height), new Vector2(0.5f, 0.5f));
            qb_brew_panel_texture = LoadTextureFromFile(pluginLoc + "/qb_panel_background.png");
            qb_brew_panel_sprite = Sprite.Create(qb_brew_panel_texture, new Rect(0, 0, qb_brew_panel_texture.width, qb_brew_panel_texture.height), new Vector2(0.5f, 0.5f));
            qb_brew__button_background_texture = LoadTextureFromFile(pluginLoc + "/qb_button_background.png");
            qb_brew__button_background_sprite = Sprite.Create(qb_brew__button_background_texture, new Rect(0, 0, qb_brew__button_background_texture.width, qb_brew__button_background_texture.height), new Vector2(0.5f, 0.5f));
            qb_page_next_texture = LoadTextureFromFile(pluginLoc + "/qb_page_next.png");
            qb_page_next_sprite = Sprite.Create(qb_page_next_texture, new Rect(0, 0, qb_page_next_texture.width, qb_page_next_texture.height), new Vector2(0.5f, 0.5f));
            qb_page_prev_texture = LoadTextureFromFile(pluginLoc + "/qb_page_prev.png");
            qb_page_prev_sprite = Sprite.Create(qb_page_prev_texture, new Rect(0, 0, qb_page_prev_texture.width, qb_page_prev_texture.height), new Vector2(0.5f, 0.5f));

            Harmony.CreateAndPatchAll(typeof(Plugin));
        }

        [HarmonyPostfix, HarmonyPatch(typeof(InputManager), "Update")]
        public static void Init_Postfix()
        {
            if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.B))
            {
                

                if(!brewPanelCreated)
                {
                    // Create background panel
                    BrewPanel.CreateQuickBrewPanel();
                    // Add the prev page button
                    BrewPanel.AddPagePrevButton();
                    // Add the next page button
                    BrewPanel.AddPageNextButton();
                    // Add the first page
                    BrewUI.CreatePageUI();
                    GameObject.Find("Quick Brew Panel").SetActive(true);
                    brewPanelCreated = true;
                    brewPanelactive = true;
                }else
                {
                    if (!brewPanelactive)
                    {
                        QuickBrewPanel.SetActive(true);
                        brewPanelactive = true;
                    }
                    else
                    {
                        QuickBrewPanel.SetActive(false);
                        brewPanelactive = false;
                    }
                }
            }
        }

        // Add the potions from the page
        public static void GetThePotions(List<Potion> items)
        {
            float yPos = 2.1f;
            int curPotion = 0;

            for(var i = 0; i < items.Count; i++)
            {
                potionsOnPage.Add(curPotion, items[i]);
                BrewUI.AddQuickBrewPotion(items[i], yPos);
                yPos -= 1f;
                curPotion += 1;
            }
        }

        // Get the amount of potions you can brew
        public static int GetPotionBrewAmount(Potion sPotion)
        {
            // Grab players inventory
            Inventory inventory = Managers.Player.inventory;
            // Set max potion amount
            int maxPotionsCount = int.MaxValue;
            // Loop through used components
            sPotion.usedComponents.ForEach(delegate (Potion.UsedComponent component)
            {
                if (component.componentType == Potion.UsedComponent.ComponentType.InventoryItem && component.amount > 0)
                {
                    maxPotionsCount = Mathf.Min(maxPotionsCount, inventory.GetItemCount((InventoryItem)component.componentObject) / component.amount);
                }
            });
            // Return the amount that can be brewed
            return maxPotionsCount;
        }

        public static Texture2D LoadTextureFromFile(string filePath)
        {
            var data = File.ReadAllBytes(filePath);

            // Do not create mip levels for this texture, use it as-is.
            var tex = new Texture2D(0, 0, TextureFormat.ARGB32, false, false)
            {
                filterMode = FilterMode.Bilinear,
            };

            if (!tex.LoadImage(data))
            {
                throw new Exception($"Failed to load image from file at \"{filePath}\".");
            }

            return tex;
        }
    }

    class PrevPage : InteractiveItem
    {
        public void OnMouseDown()
        {
            if (Plugin.curPageNumber >= 1)
            {
                Plugin.curPageNumber -= 1;
                BrewUI.CreatePageUI();
                Plugin.QuickBrewPrevButton.SetActive(true);
            }
            else
            {
                Debug.Log("Hit the first page...can't go back!");
                
            }            
            
            Sound.Play(Managers.Sound.settings.presetInterface.buttonPress, 1f, 1f, false);
        }
    }

    public class NextPage : InteractiveItem
    {
        public void OnMouseDown()
        {
            if (Plugin.curPageNumber < Plugin.amountOfPages)
            {
                Plugin.curPageNumber += 1;
                BrewUI.CreatePageUI();
                Plugin.QuickBrewPrevButton.SetActive(true);
            }
            else
            {
                
                Debug.Log("Hit the last page...can't go further!");
            }            

            Sound.Play(Managers.Sound.settings.presetInterface.buttonPress, 1f, 1f, false);
        }
    }
}