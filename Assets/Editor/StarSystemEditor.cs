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

        Button spawnStarsBtn = root.Q<Button>("spawnStarsBtn");
        spawnStarsBtn.RegisterCallback<ClickEvent>(OnSpawnStarsButtonClick);
        Button clearBtn = root.Q<Button>("clearAllStarsBtn");
        clearBtn.RegisterCallback<ClickEvent>(OnClearAllStarsButtonClick);
        
        return root;
    }

    #region Button Events

    private void OnSpawnStarsButtonClick(ClickEvent evt) => m_starSystem.SpawnStars();
    private void OnClearAllStarsButtonClick(ClickEvent evt) => m_starSystem.DisableAllStars();

    #endregion
}