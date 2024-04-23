using UnityEngine;
using UnityEngine.UIElements;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


public class StarSystem: MonoBehaviour
{

    [SerializeField] private int m_spawnAmount = 10;
    [SerializeField] private float m_starScale = 1f;
    [SerializeField] private float m_starSpeed = 1f;
    [SerializeField] private Vector2 m_spawnPositionLimit = new Vector2(8.5f, 4.5f);
    [SerializeField] private Star[] m_starList = new Star[5];
    [SerializeField] private List<Color> m_colorList = new List<Color>() {
        Color.white, Color.blue, Color.red, Color.yellow, Color.green};
    [SerializeField] private GameObject m_prefab;
    


    private void Start()
    {
        SpawnStars();
    }

    public void SpawnStars()
    {
        for(int i = 0; i < m_spawnAmount; i++)
        {
            GameObject star = StarObjectPool.m_SharedInstance.GetPooledObject();
            if (star != null)
            {
                StarController starController = star.GetComponent<StarController>();
                RandomizeStar(starController);
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
    
    private void RandomizeStar(StarController star)
    {
        star.m_starClass.SetScaleTo(Random.Range(0,m_starScale));
        star.m_starClass.SetSpeedTo(Random.Range(0.1f, m_starSpeed));
        star.m_starClass.SetColorTo(m_colorList[Random.Range(0, m_colorList.Count)]);
        star.m_starClass.SetSpawnPositionTo(new Vector2(
            Random.Range(-m_spawnPositionLimit.x, m_spawnPositionLimit.x),
            Random.Range(-m_spawnPositionLimit.y, m_spawnPositionLimit.y)));
        
    }
    

}
