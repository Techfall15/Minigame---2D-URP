using System.Collections.Generic;
using UnityEngine;

public class StarSpawnerController : MonoBehaviour
{

    [SerializeField] private StarController star;
    [SerializeField] private int spawnAmount;
    [SerializeField] private List<StarController> m_starList = new List<StarController>();

    private StarController m_star;


    protected void Awake()
    {
        m_star = star;
    }

    protected void Start()
    {
        

        for(int i = 0; i < spawnAmount; i++)
        {
            var newStar = Instantiate(m_star);
            newStar.transform.position = Vector2.zero;
            m_starList.Add(newStar);
        }


    }

}
