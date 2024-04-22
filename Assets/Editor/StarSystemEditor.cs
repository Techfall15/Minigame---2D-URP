using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;


[CustomEditor(typeof(StarSystem))]
public class StarSystemEditor : Editor
{
    public VisualTreeAsset m_treeAsset;

    public override VisualElement CreateInspectorGUI()
    {
        var root = new VisualElement();

        m_treeAsset.CloneTree(root);


        return root;
    }
}