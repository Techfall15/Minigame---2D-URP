using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

[CustomEditor(typeof(Car))]
public class Car_Inspector : Editor
{
    public VisualTreeAsset m_InspectorUXML;
    public override VisualElement CreateInspectorGUI()
    {
        // Create a new VisualElement to be the root of our Inspector UI.
        VisualElement myInspector = new VisualElement();

        // Add a simple label.
        myInspector.Add(new Label("This is a custom Inspector"));

        m_InspectorUXML.CloneTree(myInspector);

        VisualElement inspectorFoldout = myInspector.Q("Default_Inspector");

        InspectorElement.FillDefaultInspector(inspectorFoldout, serializedObject, this);
        // Return the finished Inspector UI.
        return myInspector;
    }
}
