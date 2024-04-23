using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEditor.Tilemaps;

[CustomPropertyDrawer(typeof(Star))]
public class StarPropertyDrawer : PropertyDrawer
{
    
    public override VisualElement CreatePropertyGUI(SerializedProperty property)
    {
        var root = new VisualElement();
        var popup = new UnityEngine.UIElements.PopupWindow();
        

        popup.text = "Star Details";
        PropertyField spawnPos = new PropertyField(property.FindPropertyRelative("m_spawnPosition"));
        spawnPos.label = "Spawn Pos";
        
        popup.Add(spawnPos);
        popup.Add(new PropertyField(property.FindPropertyRelative("m_color")));

        root.Add(popup);


        return root;
    }
}