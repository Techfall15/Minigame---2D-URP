using UnityEngine;
using System.Collections.Generic;

public class StarObjectPool : MonoBehaviour
{

    public static StarObjectPool SharedInstance;
    public List<GameObject> m_PooledStars;
    public GameObject m_ObjectToPool;
    public int m_AmountToPool;

    private void Awake()
    {
        SharedInstance = this;
    }

    private void Start()
    {
        m_PooledStars = new List<GameObject>();
        for(var i = 0; i < m_AmountToPool; i++)
        {
            var tmp = Instantiate(m_ObjectToPool);
            tmp.SetActive(false);
            
            m_PooledStars.Add(tmp);
        }
    }

    public GameObject GetPooledObject()
    {
        for(var i = 0; i < m_PooledStars.Count - 1; i++)
        {
            if (!m_PooledStars[i].activeInHierarchy)
                return m_PooledStars[i];
        }
        return null;
    }


}