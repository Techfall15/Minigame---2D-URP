using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;


[CustomEditor(typeof(StarSystem))]
public class StarSystemEditor : Editor
{
    public VisualTreeAsset m_treeAsset;
    private StarSystem m_starSystem;

    private void OnEnable()
    {
        m_starSystem = (StarSystem)target;
    }
    public override VisualElement CreateInspectorGUI()
    {
        var root = new VisualElement();

        m_treeAsset.CloneTree(root);

        Button populateBtn = root.Q<Button>("populateListBtn");
        populateBtn.RegisterCallback<ClickEvent>(OnPopulateButtonClick);
        Button randomizeBtn = root.Q<Button>("randomizeStarsBtn");
        randomizeBtn.RegisterCallback<ClickEvent>(OnRandomButtonClick);
        Button clearBtn = root.Q<Button>("clearListBtn");
        clearBtn.RegisterCallback<ClickEvent>(OnClearButtonClick);

        return root;
    }

    #region Button Events

    private void OnPopulateButtonClick(ClickEvent evt) => m_starSystem.PopulateList();
    private void OnRandomButtonClick(ClickEvent evt) => m_starSystem.RandomizeStars();
    private void OnClearButtonClick(ClickEvent evt) => m_starSystem.ClearList();

    #endregion
}