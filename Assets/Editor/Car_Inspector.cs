using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Editor
{
    [CustomEditor(typeof(Car))]
    public class CarInspector : UnityEditor.Editor
    {
        public VisualTreeAsset m_inspectorUxml;
        public override VisualElement CreateInspectorGUI()
        {
            // Create a new VisualElement to be the root of our Inspector UI.
            VisualElement myInspector = new VisualElement();

            // Add a simple label.
            myInspector.Add(new Label("This is a custom Inspector"));

            m_inspectorUxml.CloneTree(myInspector);

            var inspectorFoldout = myInspector.Q("Default_Inspector");

            InspectorElement.FillDefaultInspector(inspectorFoldout, serializedObject, this);
            // Return the finished Inspector UI.
            return myInspector;
        }
    }
}
