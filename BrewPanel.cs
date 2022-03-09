using UnityEngine;
using UnityEngine.Rendering;

namespace QuickBrew
{
    class BrewPanel
    {
        // Main panel
        public static void CreateQuickBrewPanel()
        {
            // Set the name
            Plugin.QuickBrewPanel = new("Quick Brew Panel");
            // Attach it to the camera object
            var parent = GameObject.Find("Camera").transform;
            Plugin.QuickBrewPanel.transform.parent = parent;
            // Add a sprite to it
            var sr = Plugin.QuickBrewPanel.AddComponent<SpriteRenderer>();
            sr.sprite = Plugin.qb_brew_panel_sprite;
            // Set the position of the sprite
            sr.transform.localPosition = new Vector3(3f, 4f, 0f);
            // Add a sorting layer
            var sg = Plugin.QuickBrewPanel.AddComponent<SortingGroup>();
            sg.sortingLayerID = -1758066705;
            sg.sortingLayerName = "GuiBackground";
            // Set the position of the panel
            Plugin.QuickBrewPanel.transform.localPosition = new Vector3(-8f, 1f, 0f);
            // Make it active
            Plugin.QuickBrewPanel.SetActive(true);
        }

        // Page next button
        public static void AddPageNextButton()
        {
            // Give it a name
            Plugin.QuickBrewNexPageButton = new("(Button) Page Next");
            // Attach it to the panel object
            var parent = GameObject.Find("Quick Brew Panel").transform;
            Plugin.QuickBrewNexPageButton.transform.parent = parent;
            // Give it a sprite
            var sr = Plugin.QuickBrewNexPageButton.AddComponent<SpriteRenderer>();
            sr.sprite = Plugin.qb_page_next_sprite;
            // Add a BoxCollider2D
            var bc = Plugin.QuickBrewNexPageButton.AddComponent<BoxCollider2D>();
            bc.size = new Vector2(0.75f, 0.75f);
            bc.enabled = true;
            bc.isTrigger = true;
            // Add the button behaviour
            var pb = Plugin.QuickBrewNexPageButton.AddComponent<NextPage>();
            pb.enabled = true;
            // Add a sorting layer
            var sg = Plugin.QuickBrewNexPageButton.AddComponent<SortingGroup>();
            sg.sortingLayerID = -1758066705;
            sg.sortingLayerName = "GuiBackground";
            Plugin.QuickBrewNexPageButton.layer = LayerMask.NameToLayer("UI");
            // Set the position
            Plugin.QuickBrewNexPageButton.transform.localPosition = new Vector3(2.45f, 3.05f, 0);
            // Make it active
            Plugin.QuickBrewNexPageButton.SetActive(true);
        }

        // Add previous button
        public static void AddPagePrevButton()
        {
            // Give it a name
            Plugin.QuickBrewPrevButton = new("(Button) Page Prev");
            // Attach it to the panel object
            var parent = GameObject.Find("Quick Brew Panel").transform;
            Plugin.QuickBrewPrevButton.transform.parent = parent;
            // Give it a sprite
            var sr = Plugin.QuickBrewPrevButton.AddComponent<SpriteRenderer>();
            sr.sprite = Plugin.qb_page_prev_sprite;
            // Add a BoxCollider2D
            var bc = Plugin.QuickBrewPrevButton.AddComponent<BoxCollider2D>();
            bc.size = new Vector2(1.75f, 0.75f);
            bc.enabled = true;
            bc.isTrigger = true;
            // Add the button behaviour
            var pb = Plugin.QuickBrewPrevButton.AddComponent<PrevPage>();
            pb.enabled = true;
            // Add a sorting layer
            var sg = Plugin.QuickBrewPrevButton.AddComponent<SortingGroup>();
            sg.sortingLayerID = -1758066705;
            sg.sortingLayerName = "GuiBackground";
            Plugin.QuickBrewPrevButton.layer = LayerMask.NameToLayer("UI");
            // Set the position
            Plugin.QuickBrewPrevButton.transform.localPosition = new Vector3(-2.45f, 3.05f, 0);
            // Make it active
            Plugin.QuickBrewPrevButton.SetActive(true);
        }
    }
}
