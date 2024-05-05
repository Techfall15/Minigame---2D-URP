using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor
{
    [CustomEditor(typeof(StarSystem))]
    public class StarSystemEditor : UnityEditor.Editor
    {
        public VisualTreeAsset m_treeAsset;
        private StarSystem m_starSystem;
        private Label m_spawnPosLimitLabel;
        private FloatField m_firstPointXPosition;
        private FloatField m_firstPointYPosition;
        
        private Vector2Field m_spawnPositionLimit2;
        private Button m_respawnBtn;
        private Button m_spawnStarsBtn;
        private Button m_clearBtn;
        private Toggle m_customizeSpawnToggle;
        private DropdownField m_spawnDirectionField;
    
        private List<Vector2> m_gridIndexes;
        private int m_rowIndex;
        private int m_colIndex;
        private int m_row2Index;
        private int m_col2Index;
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
        
            if (m_customizeSpawnToggle.value == false) m_spawnPosLimitLabel.SetEnabled(true);
        
            m_spawnDirectionField.SetEnabled(m_customizeSpawnToggle.value);
            var choices = new List<string> { "As Above", "So Below"};
            m_spawnDirectionField.choices = choices;
            m_spawnDirectionField.value = choices[0];
        
            return root;
        }
        
        #region Initialize Elements
        private void InitializeElements(VisualElement root)
        {
            m_spawnPosLimitLabel = root.Q<Label>("spawnPosLimitLabel");
            m_firstPointXPosition = root.Q<FloatField>("firstPointXFloatField");
            m_firstPointYPosition = root.Q<FloatField>("firstPointYFloatField");
            m_spawnPositionLimit2 = root.Q<Vector2Field>("spawnPos2");
            m_coloredPoints = new HashSet<VisualElement>();

            m_respawnBtn = root.Q<Button>("respawnButton");
            m_spawnStarsBtn = root.Q<Button>("spawnStarsBtn");
            m_clearBtn = root.Q<Button>("clearAllStarsBtn");
            m_customizeSpawnToggle = root.Q<Toggle>("customizeSpawnToggle");
            m_spawnDirectionField = root.Q<DropdownField>("spawnDirectionDropField");
        
            InitGrid(root);
            InitMouseTrack(root);
        }
        #region Grid Initialization
        private void InitGrid(VisualElement root)
        {
            m_gridIndexes = GetGridIndices(root);
            m_rowIndex    = GetTargetRow(m_firstPointXPosition.value);                       
            m_colIndex    = GetTargetCol(m_firstPointYPosition.value);
            m_row2Index   = GetTargetRow(m_spawnPositionLimit2.value.x);
            m_col2Index   = GetTargetCol(m_spawnPositionLimit2.value.y);
            PopulateColoredList(root, m_spawnDirectionField.value == "As Above");
        }
        #endregion
        #endregion

        #region Register Callbacks
        private void RegisterCallbacks(VisualElement root)
        {
            // First Point On The Grid
            m_firstPointXPosition.RegisterValueChangedCallback(evt =>
            {
                ClearListBackgroundColor(root);
                OnFirstPositionXChange(evt, root);
                if (Application.isPlaying == false) return;
                if (StarObjectPool.SharedInstance.GetPooledObject() == null) return;
                m_starSystem.RespawnAllStars();
            });
            m_firstPointYPosition.RegisterValueChangedCallback(evt =>
            {
                ClearListBackgroundColor(root);
                OnFirstPositionYChange(evt, root);
                m_starSystem.RespawnAllStars();
            });
            
            // Second Point On The Grid
            m_spawnPositionLimit2.RegisterValueChangedCallback(evt =>
            {
                ClearListBackgroundColor(root);
                OnSpawnLimit2Change(evt, root);
                m_starSystem.RespawnAllStars();
            });
            m_spawnStarsBtn.RegisterCallback<ClickEvent>(OnSpawnStarsButtonClick);
            m_clearBtn.RegisterCallback<ClickEvent>(OnClearAllStarsButtonClick);
            m_respawnBtn.RegisterCallback<ClickEvent>(OnRespawnStarsButtonClick);
            m_customizeSpawnToggle.RegisterValueChangedCallback(evt =>
            {
                m_spawnDirectionField.SetEnabled(evt.newValue);
                m_spawnPosLimitLabel.text = (evt.newValue == true) ? (m_spawnDirectionField.value == "As Above") ?
                        "Spawns Stars Above (x,y)" : "Spawns Stars Below (x,y)" :
                    "Spawns Within (-x,x) and (-y,y)";
            
                OnCustomizeSpawnChange(evt);
            });
            m_spawnDirectionField.RegisterCallback<ChangeEvent<string>>((evt) =>
            {
                OnAsAboveSoBelowChange(evt, root);
                m_spawnPosLimitLabel.text = (evt.newValue == "As Above") ? "Spawns Stars Above (x,y)" : "Spawns Stars Below (x,y)";

            
            });
        }
        #endregion

        #region Button Events

        private void OnSpawnStarsButtonClick(ClickEvent evt) => m_starSystem.RespawnAllStars();
        private void OnClearAllStarsButtonClick(ClickEvent evt) => StarSystem.DisableAllStars();
        private void OnCustomizeSpawnChange(ChangeEvent<bool> evt)
        {
            m_starSystem.SetCustomizeSpawnTo(evt.newValue);
        }

        private void OnRespawnStarsButtonClick(ClickEvent evt) => m_starSystem.RespawnAllStars();
        
        private void OnAsAboveSoBelowChange(ChangeEvent<string> evt, VisualElement root)
        {
            if(evt.newValue == "As Above")
            {
                m_starSystem.ToggleAsAboveSoBelow(StarSystem.SpawnState.OnlySpawnAbove);
                PopulateColoredList(root, true);
                Debug.Log("Star System state value is now: " + m_starSystem.GetCurrentSpawnState());
            }
            else
            {
                m_starSystem.ToggleAsAboveSoBelow(StarSystem.SpawnState.OnlySpawnBelow);
                PopulateColoredList(root, false);
                Debug.Log("Star System state value is now: " + m_starSystem.GetCurrentSpawnState());
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

        private void ClearListBackgroundColor(VisualElement root)
        {
            foreach(var child in root.Q<VisualElement>("spawnAreaGroup").Children())
            {
                child.style.backgroundColor = Color.clear;
            }
        }

        private void OnFirstPositionXChange(ChangeEvent<float> evt, VisualElement root)
        {
            m_colIndex = GetTargetCol(evt.newValue);
            var aboveBelow = m_spawnDirectionField.value == "As Above";
            PopulateColoredList(root, aboveBelow);
        }
        private void OnFirstPositionYChange(ChangeEvent<float> evt, VisualElement root)
        {
            m_rowIndex = GetTargetRow(evt.newValue);
            var aboveBelow = m_spawnDirectionField.value == "As Above";
            PopulateColoredList(root, aboveBelow);
        }
        private void OnSpawnLimit2Change(ChangeEvent<Vector2> evt, VisualElement root)
        {
            m_col2Index = GetTargetCol(evt.newValue.x);
            m_row2Index = GetTargetRow(evt.newValue.y);

            var aboveBelow = m_spawnDirectionField.value == "As Above";
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
            var secondPoint = m_spawnPositionLimit2.value;
            m_coloredPoints.Clear();
            m_slope = (secondPoint.y - m_firstPointYPosition.value) / (secondPoint.x- m_firstPointXPosition.value);
            m_slopeField.value = m_slope;
            var yIntercept = secondPoint.y - (m_slope * secondPoint.x);
        
            /*Debug.Log("Slope of line is: " + m_slope);
        Debug.Log("Which is: " + (m_spawnPositionLimit2.value.y - m_spawnPositionLimit.value.y) + " / " + (m_spawnPositionLimit2.value.x - m_spawnPositionLimit.value.x) + " after being simplified");
        Debug.Log("Y-Intercept of line is: " + yIntercept);*/

        
            foreach (var point in root.Q<VisualElement>("spawnAreaGroup").Children())
            {
                var x3 = int.Parse(point.name[1].ToString());
                var y3 = int.Parse(point.name[0].ToString());
                if (point.name == m_rowIndex.ToString() + m_colIndex.ToString() || point.name == m_row2Index.ToString() + m_col2Index.ToString())
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
            var point1 = root.Q<VisualElement>(m_rowIndex.ToString() + m_colIndex.ToString());
            var point2 = root.Q<VisualElement>(m_row2Index.ToString() + m_col2Index.ToString());
            m_coloredPoints.Add(point1);
            m_coloredPoints.Add(point2);
            
            foreach (var point in m_coloredPoints)
            {
                if (point == null) continue;
                if (point.name != point1.name && point.name != point2.name) point.style.backgroundColor = Color.red;
                else point.style.backgroundColor = Color.black;
            }
            root.MarkDirtyRepaint();
        }

        #endregion

        private void InitMouseTrack(VisualElement root)
        {
            var mouseTrackArea = root.Q<VisualElement>("MouseTrackTest");
            var hoverCircle = mouseTrackArea.ElementAt(0);

            mouseTrackArea.RegisterCallback<PointerOverEvent>((evt) =>
            {
                if(hoverCircle.style.visibility == Visibility.Hidden) hoverCircle.style.visibility = Visibility.Visible;
                mouseTrackArea.RegisterCallback<PointerMoveEvent>((evt) =>
                {
                
                    hoverCircle.style.left = Mathf.Clamp(evt.localPosition.x-20, 0, mouseTrackArea.layout.size.x);
                    hoverCircle.style.top = Mathf.Clamp(evt.localPosition.y - 20, -1, mouseTrackArea.layout.size.y-40);
                    if (hoverCircle.style.visibility == Visibility.Visible) return;
                    hoverCircle.style.visibility = Visibility.Visible;
                });
            });
            mouseTrackArea.RegisterCallback<PointerOutEvent>((evt) => 
            {
                hoverCircle.style.visibility = Visibility.Hidden;
            });
        }
    }
}


