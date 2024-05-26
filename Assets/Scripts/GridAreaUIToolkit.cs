using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class GridAreaUIToolkit : VisualElement
{
    private const string TemplatePath = "UXML Files/GridAreaGroup.uxml";
    

    private VisualTreeAsset m_gridAreaAsset;
    private VisualElement GroupBox => this.Q<VisualElement>("gridAreaMain");

    public GridAreaUIToolkit()
    {

        m_gridAreaAsset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/UXML Files/GridAreaGroup.uxml");
        if (m_gridAreaAsset != null)
        {
            var gridAreaElement = m_gridAreaAsset.CloneTree();
            Add(gridAreaElement);
            Debug.Log("Grid Asset Found");
        }

    }
    public new class UxmlFactory : UxmlFactory<GridAreaUIToolkit, UxmlTraits> { }
}
