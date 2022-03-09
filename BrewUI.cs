using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

namespace QuickBrew
{
    class BrewUI
    {
        public static void CreatePageUI()
        {
            ClearPageUI();
            List<Potion> recipes = new();
            foreach (Potion savedPotion in Managers.Potion.recipeBook.savedRecipes)
            {
                if (savedPotion != null && savedPotion.effects.Length != 0)
                {
                    recipes.Add(savedPotion);
                }
            }

            Plugin.amountOfPages = recipes.Count / 6;

            List<Potion> pages = recipes.Skip(Plugin.curPageNumber * 6).Take(6).ToList();

            Plugin.GetThePotions(pages);
        }

        public static void ClearPageUI()
        {
            foreach (GameObject gObject in Plugin.brewPotions)
            {
                UnityEngine.Object.Destroy(gObject);
            }

            Plugin.brewPotions.Clear();
            Plugin.potionsOnPage.Clear();
        }

        // Add the potion to the panel
        public static void AddQuickBrewPotion(Potion savedPotion, float yPosition)
        {
            // Strip the "Potion of " from the name
            string strippedName = savedPotion.name.Substring(10);
            // Create the potion holder
            CreateQuickBrewPotion(strippedName, yPosition);
            // Create potion background
            CreateQuickBrewBackground(strippedName);
            // Create the recipe icon
            CreateQuickBrewIcon(strippedName, savedPotion.IconSprite);
            // Grab the text and format it, adding brew amount
            AddPotionText(strippedName, AddNewLine(strippedName + " (" + Plugin.GetPotionBrewAmount(savedPotion) + ")"));
            // Add the button to brew the potion
            AddQuickBrewButton(strippedName);
        }

        // Arrange the text so it doesn't overflow
        public static string AddNewLine(string ogString)
        {
            string converted = string.Join(Environment.NewLine, ogString.Split()
                    .Select((word, index) => new { word, index })
                    .GroupBy(x => x.index / 2)
                    .Select(grp => string.Join(" ", grp.Select(x => x.word))));
            return converted;
        }

        // Potion holder
        public static void CreateQuickBrewPotion(string name, float ypos)
        {
            // Set the name
            GameObject QuickBrewPotionHolder = new("QB " + name);
            // Attach it to the brew panel object
            var parent = GameObject.Find("Quick Brew Panel").transform;
            QuickBrewPotionHolder.transform.parent = parent;
            // Set the position
            QuickBrewPotionHolder.transform.localPosition = new Vector3(-2.5f, ypos, 0);
            // Set it to active
            QuickBrewPotionHolder.SetActive(true);
            // Add the gameobject to the list
            Plugin.brewPotions.Add(QuickBrewPotionHolder);
        }

        // Create the potion background
        public static void CreateQuickBrewBackground(string name)
        {
            // Give it a name
            GameObject QuickBrewPotionBG = new("(BG) " + name);
            // Attach it to the potion holder object
            var parent = GameObject.Find("QB " + name).transform;
            QuickBrewPotionBG.transform.parent = parent;
            // Give it a background sprite
            var sr = QuickBrewPotionBG.AddComponent<SpriteRenderer>();
            sr.sprite = Plugin.qb_brew__button_background_sprite;
            // Give it a sorting layer
            var sg = QuickBrewPotionBG.AddComponent<SortingGroup>();
            sg.sortingLayerID = -1758066705;
            sg.sortingLayerName = "GuiBackground";
            QuickBrewPotionBG.layer = LayerMask.NameToLayer("UI");
            // Set the position
            QuickBrewPotionBG.transform.localPosition = new Vector3(2.49f, 0, 0);
            // Make it active
            QuickBrewPotionBG.SetActive(true);
            // Add the gameobject to the list
            Plugin.brewPotions.Add(QuickBrewPotionBG);
        }

        // Potion icon
        public static void CreateQuickBrewIcon(string name, Sprite sprite)
        {
            // Give it a name
            GameObject QuickBrewPotionIcon = new("(Icon) " + name);
            // Attach it to the potion holder object
            var parent = GameObject.Find("QB " + name).transform;
            QuickBrewPotionIcon.transform.parent = parent;
            // Give it the icon sprite
            var sr = QuickBrewPotionIcon.AddComponent<SpriteRenderer>();
            sr.sprite = sprite;
            // Give it a sorting layer
            var sg = QuickBrewPotionIcon.AddComponent<SortingGroup>();
            sg.sortingLayerID = -1758066705;
            sg.sortingLayerName = "GuiBackground";
            QuickBrewPotionIcon.layer = LayerMask.NameToLayer("UI");
            // Set the position
            QuickBrewPotionIcon.transform.localPosition = new Vector3(0, 0, 0);
            // Make it active
            QuickBrewPotionIcon.SetActive(true);
            // Add the gameobject to the list
            Plugin.brewPotions.Add(QuickBrewPotionIcon);
        }

        public static TextMeshPro potionText;
        // Potion text
        public static void AddPotionText(string name, string text)
        {
            // Give it a name
            GameObject QuickBrewPotionText = new("(Text) " + name);
            // Attach it to the potion holder object
            GameObject parent = GameObject.Find("QB " + name);
            QuickBrewPotionText.transform.SetParent(parent.transform);
            // Move it to the centre of the shop name background object
            QuickBrewPotionText.transform.Translate(parent.transform.position + new Vector3(10.5f, -0.02f));
            // Give it a sorting layer
            var sg = QuickBrewPotionText.AddComponent<SortingGroup>();
            sg.sortingLayerID = -1758066705;
            sg.sortingLayerName = "GuiBackground";
            QuickBrewPotionText.layer = LayerMask.NameToLayer("UI");
            // Add the TextMeshPro component to the text object
            potionText = QuickBrewPotionText.AddComponent<TextMeshPro>();
            // Customise the TextMeshPro settings
            potionText.alignment = TextAlignmentOptions.Left;
            potionText.enableAutoSizing = true;
            potionText.sortingLayerID = -1650695527;
            potionText.sortingOrder = 100;
            potionText.fontSize = 2.5f;
            potionText.fontSizeMin = 2f;
            potionText.fontSizeMax = 2.5f;
            potionText.color = new Color32(57, 30, 20, 255);
            potionText.lineSpacing = -20f;
            // Set the text to the formatted text
            potionText.text = text;
            // Set it to active
            QuickBrewPotionText.SetActive(true);
            // Add the gameobject to the list
            Plugin.brewPotions.Add(QuickBrewPotionText);
        }

        // Quick brew button
        public static void AddQuickBrewButton(string name)
        {
            // Give it a name
            GameObject QuickBrewPotionButton = new("(Button) " + name);
            // Attach it to the potion holder object
            var parent = GameObject.Find("QB " + name).transform;
            QuickBrewPotionButton.transform.parent = parent;
            // Add the cauldron sprite
            var sr = QuickBrewPotionButton.AddComponent<SpriteRenderer>();
            sr.sprite = Plugin.qb_brew_button_sprite;
            // Add a BoxCollider2D
            var bc = QuickBrewPotionButton.AddComponent<BoxCollider2D>();
            bc.size = new Vector2(0.75f, 0.75f);
            bc.enabled = true;
            bc.isTrigger = true;
            // Add the behaviour for the button
            var pb = QuickBrewPotionButton.AddComponent<BrewPotion>();
            pb.enabled = true;
            // Add a sorting layer
            var sg = QuickBrewPotionButton.AddComponent<SortingGroup>();
            sg.sortingLayerID = -1758066705;
            sg.sortingLayerName = "GuiBackground";
            QuickBrewPotionButton.layer = LayerMask.NameToLayer("UI");
            // Set the position
            QuickBrewPotionButton.transform.localPosition = new Vector3(4.75f, 0, 0);
            // Set it to active
            QuickBrewPotionButton.SetActive(true);
            // Add the gameobject to the list
            Plugin.brewPotions.Add(QuickBrewPotionButton);
        }
    }
}
