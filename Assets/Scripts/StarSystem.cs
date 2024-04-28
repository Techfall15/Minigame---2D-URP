using UnityEngine;
using UnityEngine.UIElements;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;


public class StarSystem : MonoBehaviour
{

    [SerializeField] private int m_spawnAmount = 10;
    [SerializeField] private float m_starScale = 1f;
    [SerializeField] private float m_starSpeed = 1f;
    [SerializeField] private Vector2 m_spawnPositionLimit = new Vector2(8.5f, 4.5f);
    [SerializeField] private Vector2 m_spawnPositionLimit2 = new Vector2(8.5f, 4.5f);
    [SerializeField] private float m_slope = 1f;
    [SerializeField] private float m_yIntercept = 0f;
    [SerializeField] private Star[] m_starList = new Star[5];
    [SerializeField] private List<Color> m_colorList = new List<Color>() {
        Color.white, Color.blue, Color.red, Color.yellow, Color.green};
    [SerializeField] private GameObject m_prefab;
    [SerializeField] private bool m_customizeSpawn = false;
    [SerializeField] private bool m_onlySpawnAbove = false;
    [SerializeField] private bool m_onlySpawnBelow = false;
    public enum m_spawnState { OnlySpawnAbove, OnlySpawnBelow };
    private m_spawnState spawnState;

    private void Awake()
    {
        spawnState = (m_customizeSpawn) ? m_spawnState.OnlySpawnAbove : m_spawnState.OnlySpawnBelow;
        m_slope = GetSlopeOfLine(m_spawnPositionLimit, m_spawnPositionLimit2);
        m_yIntercept = GetYInterceptOfLine(m_spawnPositionLimit, m_slope);
    }

    private void Start()
    {
        SpawnStars();
        if(m_customizeSpawn == true) Debug.Log("Stars are using custom spawn");
        else Debug.Log("Stars are not using custom spawn");
    }

    public void SpawnStars()
    {
        for(int i = 0; i < m_spawnAmount; i++)
        {
            GameObject star = StarObjectPool.m_SharedInstance.GetPooledObject();
            if (star != null)
            {
                StarController starController = star.GetComponent<StarController>();
                if(m_customizeSpawn == true)
                {
                    if(spawnState == m_spawnState.OnlySpawnAbove)
                    {
                        RandomizeStar(starController);
                        CustomizeSpawn(starController, new Vector2(-4.5f,4.5f), spawnState);
                    }
                }
                else RandomizeStar(starController);
                star.SetActive(true);

            }
        }
        
    }
    public void DisableAllStars()
    {
        foreach(var star in StarObjectPool.m_SharedInstance.m_PooledStars)
        {
            star.SetActive(false);
        }
    }

    #region Randomize Functions
    private void RandomizeStar(StarController star)
    {
        RandomizeScale(star);
        RandomizeSpeed(star);
        RandomizeColor(star);
        var newSpawnPos = Vector2.zero;
        do
        {
            RandomizeSpawn(star);
            newSpawnPos = star.m_starClass.GetSpawnPosition();
        }while((m_onlySpawnAbove == true) ? GetYInterceptOfLine(newSpawnPos,m_slope) < m_yIntercept : GetYInterceptOfLine(newSpawnPos, m_slope) > m_yIntercept);
    }
    private void RandomizeScale(StarController star) => star.m_starClass.SetScaleTo(Random.Range(0, m_starScale));
    private void RandomizeSpeed(StarController star) => star.m_starClass.SetSpeedTo(Random.Range(0.1f, m_starSpeed));
    private void RandomizeColor(StarController star) => star.m_starClass.SetColorTo(m_colorList[Random.Range(0, m_colorList.Count)]);
    private void RandomizeSpawn(StarController star) => star.m_starClass.SetSpawnPositionTo(new Vector2(
            Random.Range(-5f, 5f),
            Random.Range(-5f, 5f)));
    #endregion

    #region Customize Functions
    private void CustomizeSpawn(StarController star, Vector2 limits, m_spawnState state)
    {
        switch (state)
        {
            case m_spawnState.OnlySpawnBelow:
                m_onlySpawnAbove = false;
                m_onlySpawnBelow = true;
                break;
            case m_spawnState.OnlySpawnAbove:
                m_onlySpawnAbove = true;
                m_onlySpawnBelow = false;
                break;
            default:
                Debug.Log("Error customizeing spawn position");
                break;
        }
        

    }    
    #endregion
    public void ToggleAsAboveSoBelow(m_spawnState state)
    {
        switch (state)
        {
            case m_spawnState.OnlySpawnAbove:
                m_onlySpawnAbove = true;
                m_onlySpawnBelow = false;
                Debug.Log("Changed to As Above");

                break;
            case m_spawnState.OnlySpawnBelow:
                m_onlySpawnAbove = false;
                m_onlySpawnBelow = true;
                Debug.Log("Changed to So Below");

                break;
            default:
                Debug.Log("Error toggling above or below spawner");
                break;
        }
    }
    public void SetCustomizeSpawnTo(bool state) => m_customizeSpawn = state;
    private float GetSlopeOfLine(Vector2 p1,  Vector2 p2) => (p2.y - p1.y) / (p2.x - p1.x);
    private float GetYInterceptOfLine(Vector2 point, float slope) => point.y - (m_slope * point.x);
}
