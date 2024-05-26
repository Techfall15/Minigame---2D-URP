using System;
using UnityEngine;
using System.Collections.Generic;
using Random = UnityEngine.Random;


public class StarSystem : MonoBehaviour
{
    
    [SerializeField] private int m_spawnAmount = 10;
    [SerializeField] private float m_starScale = 1f;
    [SerializeField] private float m_starSpeed = 1f;
    [SerializeField] private float m_slope = 1f;
    [SerializeField] private float m_yIntercept;
    [SerializeField] private bool m_customizeSpawn;
    [SerializeField] private bool m_onlySpawnAbove;
    [SerializeField] private bool m_onlySpawnBelow;
    [SerializeField] private GameObject m_prefab;
    [Range(-5f, 5f)]
    [SerializeField] private float m_firstPointXPos = 0f;
    [Range(-5f, 5f)]
    [SerializeField] private float m_firstPointYPos = 0f;
    [Range(-5f, 5f)]
    [SerializeField] private float m_secondPointXPos = 0f;
    [Range(-5f, 5f)]
    [SerializeField] private float m_secondPointYPos = 0f;
    
    [SerializeField] private Vector2 m_spawnPositionLimit2 = new Vector2(8.5f, 4.5f);
    [SerializeField] private List<Color> m_colorList = new List<Color>() {
        Color.white, Color.blue, Color.red, Color.yellow, Color.green};
    public enum SpawnState { OnlySpawnAbove, OnlySpawnBelow };
    private SpawnState m_spawnState;

    private void OnDrawGizmos()
    {
        // Get reference for first and second point
        var firstPoint = new Vector2(m_firstPointXPos, m_firstPointYPos);
        var secondPoint = new Vector2(m_secondPointXPos, m_secondPointYPos);
        // Get slope of line created by first and second point
        var slope = GetSlopeOfLine(firstPoint, secondPoint);
        // Create variable for endpoints of line
        var endPointOne = firstPoint;
        var endPointTwo = secondPoint;
        endPointOne = CalculateEndPoint(100f, endPointOne, slope);
        endPointTwo = CalculateEndPoint(100f, endPointTwo, slope);
        
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(firstPoint, 0.25f);
        Gizmos.DrawSphere(secondPoint, 0.25f);
        Gizmos.DrawLine(secondPoint, endPointOne);
        Gizmos.DrawLine(firstPoint, endPointTwo);
        if(m_firstPointXPos > m_secondPointXPos) Gizmos.DrawLine(firstPoint, secondPoint);

        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(endPointOne, 0.1f);
        Gizmos.DrawSphere(endPointTwo, 0.1f);
    }

    private Vector2 CalculateEndPoint(float mod, Vector2 endPoint, float slope)
    {
        var point = endPoint;
        var secondPoint = new Vector2(m_secondPointXPos, m_secondPointYPos);
        var isInScreen = true;
        do
        {
            if (point.x is <= -5f or >= 5f) isInScreen = false;
            if (point.y is <= -5f or >= 5f) isInScreen = false;
            if (endPoint == secondPoint)
            {
                var newX = Mathf.Clamp(point.x + (1 / mod), -5f, 5f);
                var newY = (slope < 0)
                    ? Mathf.Clamp(point.y - (Mathf.Abs(slope) / mod), -5f, 5f)
                    : Mathf.Clamp(point.y + (Mathf.Abs(slope) / mod), -5f, 5f);
                point = new Vector2(newX, newY);
            }
            else
            {
                var newX = Mathf.Clamp(point.x - (1 / mod), -5f, 5f);
                var newY = (slope > 0)
                    ? Mathf.Clamp(point.y - (Mathf.Abs(slope) / mod), -5f, 5f)
                    : Mathf.Clamp(point.y + (Mathf.Abs(slope) / mod), -5f, 5f);
                point = new Vector2(newX, newY);
            }
            
        } while (isInScreen);

        return point;
    }
    private void Awake()
    {
        UpdateSlopeAndYIntercept();
    }

    private void Start()
    {
        SpawnStars();
    }

    #region Public Interface
    private void SpawnStars()
    {
        UpdateSlopeAndYIntercept();

        if (StarObjectPool.SharedInstance == null) return;
        for(var i = 0; i < m_spawnAmount; i++)
        {
            var star = StarObjectPool.SharedInstance.GetPooledObject();
            if (star == null) continue;
            var starController = star.GetComponent<StarController>();
            if(m_customizeSpawn)
            {
                CustomizeSpawn(m_spawnState);
                RandomizeStar(starController);
            }
            else RandomizeStar(starController);
            star.SetActive(true);
        }
        
    }

    public void RespawnAllStars()
    {
        DisableAllStars();
        if (StarObjectPool.SharedInstance == null) return;
        SpawnStars();
    }
    public static void DisableAllStars()
    {
        if (StarObjectPool.SharedInstance == null) return;
        foreach(var star in StarObjectPool.SharedInstance.m_PooledStars)
        {
            star.SetActive(false);
        }
    }
    public SpawnState GetCurrentSpawnState() => m_spawnState;
    public void ToggleAsAboveSoBelow(SpawnState state)
    {
        switch (state)
        {
            case SpawnState.OnlySpawnAbove:
                m_onlySpawnAbove = true;
                m_onlySpawnBelow = false;
                m_spawnState = state;
                //Debug.Log("Changed to As Above");
                if (StarObjectPool.SharedInstance != null)
                {
                    DisableAllStars();
                    SpawnStars();
                }


                break;
            case SpawnState.OnlySpawnBelow:
                m_onlySpawnAbove = false;
                m_onlySpawnBelow = true;
                m_spawnState = state;
                //Debug.Log("Changed to So Below");
                if (StarObjectPool.SharedInstance != null)
                {
                    DisableAllStars();
                    SpawnStars();
                }
                break;
            default:
                Debug.Log("Error toggling above or below spawner");
                break;
        }
    }
    public void SetCustomizeSpawnTo(bool state) => m_customizeSpawn = state;
    #endregion

    #region Private Interface
    private static float GetSlopeOfLine(Vector2 p1, Vector2 p2) => (p2.y - p1.y) / (p2.x - p1.x);
    private static float GetYInterceptOfLine(Vector2 point, float slope) => point.y - (slope * point.x);

    private void UpdateSlopeAndYIntercept()
    {
        var firstPoint = new Vector2(m_firstPointXPos, m_firstPointYPos);
        var secondPoint = new Vector2(m_secondPointXPos, m_secondPointYPos);
        m_slope = GetSlopeOfLine(firstPoint, secondPoint);
        m_yIntercept = GetYInterceptOfLine(secondPoint, m_slope);
    }

    #region Randomize Functions
    private void RandomizeStar(StarController star)
    {
        RandomizeScale(star);
        RandomizeSpeed(star);
        RandomizeColor(star);
        Vector2 newSpawnPosition;
        
        if (m_onlySpawnAbove)
        { 
            do
            {
                RandomizeSpawn(star);
                newSpawnPosition = star.m_starClass.GetSpawnPosition();
            } while (GetYInterceptOfLine(newSpawnPosition, m_slope) < m_yIntercept);
        }
        else if(m_onlySpawnBelow)
        {
            do
            {
                RandomizeSpawn(star);
                newSpawnPosition = star.m_starClass.GetSpawnPosition();
            } while (GetYInterceptOfLine(newSpawnPosition, m_slope) > m_yIntercept);
        }
    }
    private void RandomizeScale(StarController star) => star.m_starClass.SetScaleTo(Random.Range(0, m_starScale));
    private void RandomizeSpeed(StarController star) => star.m_starClass.SetSpeedTo(Random.Range(0.1f, m_starSpeed));
    private void RandomizeColor(StarController star) => star.m_starClass.SetColorTo(
        m_colorList[Random.Range(0, m_colorList.Count)]);
    // Update RandomizeSpawn(). Right now the limits are set numbers and should be variables.
    private static void RandomizeSpawn(StarController star) => star.m_starClass.SetSpawnPositionTo(new Vector2(
            Random.Range(-5f, 5f),
            Random.Range(-5f, 5f)));
    
    #endregion

    #region Customize Functions
    private void CustomizeSpawn(SpawnState state)
    {
        switch (state)
        {
            
            case SpawnState.OnlySpawnBelow:
                m_onlySpawnAbove = false;
                m_onlySpawnBelow = true;
                break;
            case SpawnState.OnlySpawnAbove:
                m_onlySpawnAbove = true;
                m_onlySpawnBelow = false;
                break;
            default:
                Debug.Log("Error customizing spawn position");
                break;
        }
        

    }    
    #endregion

    #endregion
}
