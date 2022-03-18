// Project:         RoleplayRealism:Items mod for Daggerfall Unity (http://www.dfworkshop.net)
// Copyright:       Copyright (C) 2020 Hazelnut
// License:         MIT License (http://www.opensource.org/licenses/mit-license.php)
// Author:          Hazelnut

using DaggerfallConnect;
using DaggerfallWorkshop;
using DaggerfallWorkshop.Game;
using DaggerfallWorkshop;
using DaggerfallWorkshop.Game;
using DaggerfallWorkshop.Game.Items;
using DaggerfallWorkshop.Game.Serialization;
using DaggerfallWorkshop.Game.UserInterfaceWindows;
using System.Collections.Generic;

namespace Minimap
{
    public class ItemRepairKit : DaggerfallUnityItem
    {
        public const int templateIndex = 723;

        public ItemRepairKit() : base(ItemGroups.MiscItems, templateIndex)
        {
        }

        // Always use same archive for both genders as the same image set is used
        public override int InventoryTextureArchive
        {
            get { return templateIndex; }
        }

        public override int InventoryTextureRecord
        {
            get { return 0; }
        }

        public override bool UseItem(ItemCollection collection)
        {
            List<DaggerfallUnityItem> dwemerGearsList = GameManager.Instance.PlayerEntity.Items.SearchItems(ItemGroups.MiscItems, ItemDwemerGears.templateIndex);
            List<DaggerfallUnityItem> cutGlassList = GameManager.Instance.PlayerEntity.Items.SearchItems(ItemGroups.MiscItems, ItemCutGlass.templateIndex);
            if(Minimap.currentEquippedCompass.ConditionPercentage >= 90)
            {
                DaggerfallMessageBox confirmBox = new DaggerfallMessageBox(DaggerfallUI.UIManager, DaggerfallMessageBox.CommonMessageBoxButtons.Nothing, "Your compass is in fine shape");
                confirmBox.Show();
                return false;
            }

            if (dwemerGearsList.Count != 0 && cutGlassList.Count != 0)
            {
                DaggerfallMessageBox confirmBox = new DaggerfallMessageBox(DaggerfallUI.UIManager, DaggerfallMessageBox.CommonMessageBoxButtons.Nothing, "You steady your hands and concentrate on repairing the compass. Don't move or you'll drop something.");
                confirmBox.Show();
                if (EffectManager.compassDirty)
                    EffectManager.cleaningCompass = true;

                EffectManager.repairingCompass = true;
                Minimap.MinimapInstance.minimapActive = false;
            }
            else
            {
                string missingItems = "";

                if (cutGlassList.Count == 0)
                    missingItems = "Cut Glass";
                if (missingItems == "")
                    missingItems = "Dwemer Gears";
                else
                    missingItems = missingItems + " and Dwemer Gears";

                DaggerfallUI.MessageBox("You do not have " + missingItems + " to fix your compass");
            }

            return true;
        }

        public override ItemData_v1 GetSaveData()
        {
            ItemData_v1 data = base.GetSaveData();
            data.className = typeof(ItemRepairKit).ToString();
            return data;
        }
    }
}

