using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace QuickBrew
{
    class BrewPotion : InteractiveItem
    {
        public void OnMouseDown()
        {
            // Get the Y position of the parent
            float goY = this.transform.parent.transform.localPosition.y;
            Decimal dY = Math.Round((Decimal)goY, 1, MidpointRounding.AwayFromZero);
            
            // Convert it to an item index
            int dictNo = GetDictNo(dY);

            if(dictNo != 99)
            {
                // If potion is found on the page, grab it from current page potions
                Potion curPotion = Plugin.potionsOnPage[dictNo];

                // Get the amount that the player is able to brew
                int brewAmount = Plugin.GetPotionBrewAmount(curPotion);
                if(brewAmount == 0)
                {
                    // If brew amount is 0...then they can't brew it
                    Debug.Log("Not enough ingredients to brew that potion!");
                    // Let the player know they don't have enough ingredients for that potion to be brewed
                    Notification.ShowText("Quick Brew", "Not enough ingredients to brew that potion!", Notification.TextType.EventText);
                    // Play a sound
                    Sound.Play(Managers.Sound.settings.presetInterface.popularityDeteriorated, 1f, 1f, false);
                }
                else
                {
                    // They can brew it
                    Debug.Log("Brewing potion...");
                    // Remove the ingredients
                    TakeIngredientsFromInventory(curPotion);
                    // Update text
                    UpdatePotionText();
                    // Add the potion
                    Managers.Player.inventory.AddItem(curPotion, 1, true, true);

                    Debug.Log($"{curPotion.name} brewed!");
                    // Let the player know that the potion has brewed
                    Notification.ShowText("Quick Brew", curPotion.name.ToString() + " brewed!", Notification.TextType.EventText);
                    // Play a brewing sound
                    Sound.Play(Managers.Sound.settings.presetInterface.potionFinishing, 1f, 1f, false);
                }
            }
        }

        public void UpdatePotionText()
        {
            foreach(KeyValuePair<int, Potion> potion in Plugin.potionsOnPage)
            {
                string strippedName = potion.Value.name.Substring(10);
                TextMeshPro potionTMP = GameObject.Find("(Text) " + strippedName).GetComponent<TextMeshPro>();
                potionTMP.text = BrewUI.AddNewLine(strippedName + " (" + Plugin.GetPotionBrewAmount(potion.Value) + ")");
            }
        }

        // Remove ingredients from the inventory
        public void TakeIngredientsFromInventory(Potion potion)
        {
            potion.usedComponents.ForEach(delegate (Potion.UsedComponent component)
            {
                if(component.componentType == Potion.UsedComponent.ComponentType.InventoryItem)
                {
                    Managers.Player.inventory.RemoveItem((InventoryItem)component.componentObject, component.amount, false, true);
                }
            });
            Managers.Player.inventory.onItemChanged.Invoke(false);
        }

        // Grab the dictionary of potion in potions on pages
        public int GetDictNo(Decimal yPosition)
        {
            return yPosition switch
            {
                2.1M => 0,
                1.1M => 1,
                0.1M => 2,
                -0.9M => 3,
                -1.9M => 4,
                -2.9M => 5,

                _ => 99,
            };
        }
    }
}