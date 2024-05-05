using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Editor
{
    [CustomPropertyDrawer(typeof(Star))]
    public class StarPropertyDrawer : PropertyDrawer
    {
    
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var root = new VisualElement();
            var popup = new UnityEngine.UIElements.PopupWindow
            {
                text = "Star Details"
            };


            var spawnPos = new PropertyField(property.FindPropertyRelative("m_spawnPosition"))
            {
                label = "Spawn Pos"
            };

            popup.Add(spawnPos);
            popup.Add(new PropertyField(property.FindPropertyRelative("m_color")));

            root.Add(popup);


            return root;
        }
    }
}