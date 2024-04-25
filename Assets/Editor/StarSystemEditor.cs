using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;
using System;
using System.Xml;
using System.Net.NetworkInformation;



[CustomEditor(typeof(StarSystem))]
public class StarSystemEditor : Editor
{
    public VisualTreeAsset m_treeAsset;
    private StarSystem m_starSystem;
    private Label m_spawnPosLimitLabel;
    private Vector2Field m_spawnPositionLimit;
    private Vector2Field m_spawnPositionLimit2;
    private Button spawnStarsBtn;
    private Button clearBtn;
    private Toggle customizeSpawnToggle;
    private DropdownField spawnDirectionField;
    
    private List<Vector2> gridIndexes;
    private int rowIndex;
    private int colIndex;
    private int row2Index;
    private int col2Index;
    private HashSet<VisualElement> m_coloredPoints;
    private float m_slope;
    private FloatField m_slopeField;
    
    private void OnEnable()
    {
        m_starSystem = (StarSystem)target;
    }
    public override VisualElement CreateInspectorGUI()
    {
        var root = new VisualElement();
        m_treeAsset.CloneTree(root);
        m_slopeField = root.Q<FloatField>("slopeFloatField");
        m_slopeField.SetEnabled(false);
        InitializeElements(root);
        RegisterCallbacks(root);

        if (customizeSpawnToggle.value == false) m_spawnPosLimitLabel.SetEnabled(true);
        
        spawnDirectionField.SetEnabled(customizeSpawnToggle.value);
        var choices = new List<string> { "As Above", "So Below"};
        spawnDirectionField.choices = choices;
        spawnDirectionField.value = choices[0];
        
        return root;
    }
    #region Initialize Elements
    private void InitializeElements(VisualElement root)
    {
        m_spawnPosLimitLabel = root.Q<Label>("spawnPosLimitLabel");
        m_spawnPositionLimit = root.Q<Vector2Field>("spawnPos1");
        m_spawnPositionLimit2 = root.Q<Vector2Field>("spawnPos2");
        m_coloredPoints = new HashSet<VisualElement>();

        spawnStarsBtn = root.Q<Button>("spawnStarsBtn");
        clearBtn = root.Q<Button>("clearAllStarsBtn");
        customizeSpawnToggle = root.Q<Toggle>("customizeSpawnToggle");
        spawnDirectionField = root.Q<DropdownField>("spawnDirectionDropField");
        m_spawnPosLimitLabel.SetEnabled(customizeSpawnToggle.value);
        InitGrid(root);

    }
    #region Grid Initialization
    private void InitGrid(VisualElement root)
    {
        gridIndexes = GetGridIndices(root);
        rowIndex    = GetTargetRow(m_spawnPositionLimit.value.x);                       // This stuff could probaly end up in a list with one function passing in the value. TBD
        colIndex    = GetTargetCol(m_spawnPositionLimit.value.y);
        row2Index   = GetTargetRow(m_spawnPositionLimit2.value.x);
        col2Index   = GetTargetCol(m_spawnPositionLimit2.value.y);
        PopulateColoredList(root, spawnDirectionField.value == "As Above");
    }
    #endregion
    #endregion

    #region Register Callbacks
    private void RegisterCallbacks(VisualElement root)
    {
        m_spawnPositionLimit.RegisterValueChangedCallback(evt =>
        {
            foreach(var child in root.Q<VisualElement>("spawnAreaGroup").Children())
            {
                child.style.backgroundColor = Color.clear;
            }
            OnSpawnLimit1Change(evt, root);
            
        });
        m_spawnPositionLimit2.RegisterValueChangedCallback(evt =>
        {
            foreach (var child in root.Q<VisualElement>("spawnAreaGroup").Children())
            {
                child.style.backgroundColor = Color.clear;
            }
            OnSpawnLimit2Change(evt, root);
            
        });
        spawnStarsBtn.RegisterCallback<ClickEvent>(OnSpawnStarsButtonClick);
        clearBtn.RegisterCallback<ClickEvent>(OnClearAllStarsButtonClick);
        
        customizeSpawnToggle.RegisterValueChangedCallback(evt =>
        {
            spawnDirectionField.SetEnabled(evt.newValue);
            m_spawnPosLimitLabel.text = (evt.newValue == true) ? (spawnDirectionField.value == "As Above") ?
                "Spawns Stars Above (x,y)" : "Spawns Stars Below (x,y)" :
                "Spawns Within (-x,x) and (-y,y)";
            
            OnCustomizeSpawnChange(evt);
        });
        spawnDirectionField.RegisterCallback<ChangeEvent<string>>((evt) =>
        {
            OnAsAboveSoBelowChange(evt, root);
            m_spawnPosLimitLabel.text = (evt.newValue == "As Above") ? "Spawns Stars Above (x,y)" : "Spawns Stars Below (x,y)";
        });
    }
    #endregion

    #region Button Events

    private void OnSpawnStarsButtonClick(ClickEvent evt) => m_starSystem.SpawnStars();
    private void OnClearAllStarsButtonClick(ClickEvent evt) => m_starSystem.DisableAllStars();
    private void OnCustomizeSpawnChange(ChangeEvent<bool> evt)
    {
        m_starSystem.SetCustomizeSpawnTo(evt.newValue);
    }
    private void OnAsAboveSoBelowChange(ChangeEvent<string> evt, VisualElement root)
    {
        if(evt.newValue == "As Above")
        {
            m_starSystem.ToggleAsAboveSoBelow(StarSystem.m_spawnState.OnlySpawnAbove);
            PopulateColoredList(root, true);
        }
        else
        {
            m_starSystem.ToggleAsAboveSoBelow(StarSystem.m_spawnState.OnlySpawnBelow);
            PopulateColoredList(root, false);
        }
    }

    #endregion

    #region Grid Events

    private List<Vector2> GetGridIndices(VisualElement root)
    {
        var indexes = new List<Vector2>();
        var points = new List<VisualElement>();
        var grid = root.Q<VisualElement>("spawnAreaGroup");
        for (int i = 0; i < grid.childCount; i++)
        {
            var newPoint = grid.ElementAt(i);
            if (i < 10) newPoint.name = "0" + i.ToString();
            else newPoint.name = i.ToString();
            points.Add(newPoint);
        }
        for (int i = 0; i < points.Count; i++)
        {
            if (i < 10) indexes.Add(new Vector2(0, int.Parse(points[i].name)));
            else indexes.Add(new Vector2(int.Parse(points[i].name[0].ToString()), int.Parse(points[i].name[1].ToString())));
        }
        return indexes;
    }
    private void OnSpawnLimit1Change(ChangeEvent<Vector2> evt, VisualElement root)
    {
        colIndex = GetTargetCol(evt.newValue.x);
        rowIndex = GetTargetRow(evt.newValue.y);

        var aboveBelow = spawnDirectionField.value == "As Above";
        PopulateColoredList(root, aboveBelow);
        
    }
    private void OnSpawnLimit2Change(ChangeEvent<Vector2> evt, VisualElement root)
    {
        col2Index = GetTargetCol(evt.newValue.x);
        row2Index = GetTargetRow(evt.newValue.y);

        var aboveBelow = spawnDirectionField.value == "As Above";
        PopulateColoredList(root, aboveBelow);

    }
    private int GetTargetCol(float x)
    {
        var xPoint = x;
        var xMod = (xPoint >= 1) ? 0:1;
        var xPointString = xPoint.ToString("0.00");
        var xSplit = xPointString.ToString().Split('.');
        var xIntString = (xPoint < 0) ? xSplit[0].ToString().Substring(0, 2) : xSplit[0].ToString().Substring(0, 1);
        var xInt = int.Parse(xIntString.ToString());
        return 4 + xMod + xInt;
    }
    private int GetTargetRow(float y)
    {
        var yPoint = y;
        var yMod = (yPoint < 0) ? 1 : 0;
        
        var yPointString = yPoint.ToString("0.00");
        var ySplit = yPointString.ToString().Split('.');
        var yIntString = (yPoint < 0) ? ySplit[0].ToString().Substring(0, 2) : ySplit[0].ToString().Substring(0, 1);
        var yInt = int.Parse(yIntString.ToString()) * -1;
        if (yPoint <= -1) yInt = yInt - 1;
        if (yPoint >= 1) yInt = yInt + 1;
        return 4 + yMod + yInt;
    }
    private void PopulateColoredList(VisualElement root, bool aboveBelow)
    {
        m_coloredPoints.Clear();
        m_slope = (m_spawnPositionLimit2.value.y - m_spawnPositionLimit.value.y) / (m_spawnPositionLimit2.value.x- m_spawnPositionLimit.value.x);
        m_slopeField.value = m_slope;
        var yIntercept = m_spawnPositionLimit2.value.y - (m_slope * m_spawnPositionLimit2.value.x);
        
        Debug.Log("Slope of line is: " + m_slope);
        Debug.Log("Which is: " + (m_spawnPositionLimit2.value.y - m_spawnPositionLimit.value.y) + " / " + (m_spawnPositionLimit2.value.x - m_spawnPositionLimit.value.x) + " after being simplified");
        Debug.Log("Y-Intercept of line is: " + yIntercept);

        
        foreach (var point in root.Q<VisualElement>("spawnAreaGroup").Children())
        {
            var x3 = int.Parse(point.name[1].ToString());
            var y3 = int.Parse(point.name[0].ToString());
            if (point.name == rowIndex.ToString() + colIndex.ToString() || point.name == row2Index.ToString() + col2Index.ToString())
            {
                m_coloredPoints.Add(point);
            }
            x3 = x3 - 5;
            y3 = 5 - y3;

            var newIntercept = y3 - (m_slope * x3);

            if(aboveBelow == true)
            {
                if (newIntercept > yIntercept) m_coloredPoints.Add(point);
                
            }
            else
            {
                if (newIntercept < yIntercept) m_coloredPoints.Add(point);
                
            }

        }
        foreach (var point in root.Q<VisualElement>("spawnAreaGroup").Children()) { point.style.backgroundColor = Color.clear; }
        var point1 = root.Q<VisualElement>(rowIndex.ToString() + colIndex.ToString());
        var point2 = root.Q<VisualElement>(row2Index.ToString() + col2Index.ToString());
        m_coloredPoints.Add(point1);
        m_coloredPoints.Add(point2);
        foreach (var point in m_coloredPoints)
        {
            if (point.name != point1.name && point.name != point2.name) point.style.backgroundColor = Color.red;
            else point.style.backgroundColor = Color.black;
        }
        root.MarkDirtyRepaint();
    }

    #endregion


}


