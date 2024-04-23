using UnityEngine;
using UnityEngine.UIElements;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


public class StarSystem: MonoBehaviour
{

    [SerializeField] private int m_spawnAmount = 10;
    [SerializeField] private float m_starScale = 1f;
    [SerializeField] private Vector2 m_spawnPositionLimit = new Vector2(8.5f, 4.5f);
    [SerializeField] private Star[] m_starList = new Star[5];
    [SerializeField] private List<Color> m_colorList = new List<Color>() {
        Color.white, Color.blue, Color.red, Color.yellow, Color.green};
    [SerializeField] private GameObject m_prefab;


    protected void Start()
    {
        if (m_prefab == null) return;
        SpawnStars();
    }
    private void SpawnStars()
    {
        RandomizeStars();
        foreach (var star in m_starList)
        {
            var newStar = Instantiate(m_prefab);
            var scale = star.GetScale();

            newStar.transform.position = star.GetSpawnPosition();
            newStar.GetComponent<SpriteRenderer>().color = star.GetColor();
            newStar.transform.localScale = new Vector2(scale, scale);
        }
    }
    public void PopulateList() => m_starList = new Star[m_spawnAmount];
    public void RandomizeStars()
    {
        if (m_starList.Length <= 0) return;
        foreach(var star in m_starList)
        {
            star.SetScaleTo(Random.Range(0,m_starScale));
            star.SetSpeedTo(Random.Range(0.1f, 1f));
            star.SetColorTo(m_colorList[Random.Range(0, m_colorList.Count)]);
            star.SetSpawnPositionTo(new Vector2(
                Random.Range(-m_spawnPositionLimit.x, m_spawnPositionLimit.x),
                Random.Range(-m_spawnPositionLimit.y, m_spawnPositionLimit.y)));
        }
    }
    public void ClearList() => m_starList = new Star[0];

}
