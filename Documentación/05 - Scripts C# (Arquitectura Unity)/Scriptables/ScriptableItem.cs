using UnityEngine;

namespace Gaiden.Items {
    [CreateAssetMenu(fileName = "NewItem", menuName = "Gaiden/Items/Item")]
    public class ScriptableItem : ScriptableObject {
        public ItemId ItemId;
        public string ItemName;
        [TextArea] public string Description;
        public Sprite Icon;
        public bool IsKeyItem;
    }
}
