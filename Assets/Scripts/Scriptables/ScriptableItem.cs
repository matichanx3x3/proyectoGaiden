using UnityEngine;

namespace Game.Items {
    public enum ItemId {
        None          = 0,
        GreenHerb     = 1,
        RedHerb       = 2,
        MixedHerb     = 3,
        FirstAidMed   = 4,
        BodyArmor     = 5,
        KeyCard_Level1 = 6,
        KeyCard_Level2 = 7,
        MaintenanceKey = 8,
        CustomItem    = 99
    }

    [CreateAssetMenu(fileName = "NewItem", menuName = "Game/Items/Item")]
    public class ScriptableItem : ScriptableObject {
        public ItemId ItemId;
        public string ItemName;
        [TextArea] public string Description;
        public Sprite Icon;
        public bool IsKeyItem;
    }
}
