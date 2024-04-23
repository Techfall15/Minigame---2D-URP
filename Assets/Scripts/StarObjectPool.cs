using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

public class StarObjectPool : MonoBehaviour
{

    public static StarObjectPool m_SharedInstance;
    public List<GameObject> m_PooledStars;
    public GameObject m_ObjectToPool;
    public int m_AmountToPool;

    private void Awake()
    {
        m_SharedInstance = this;
    }

    private void Start()
    {
        m_PooledStars = new List<GameObject>();
        GameObject tmp;
        for(int i = 0; i < m_AmountToPool; i++)
        {
            tmp = Instantiate(m_ObjectToPool);
            tmp.SetActive(false);
            
            m_PooledStars.Add(tmp);
        }
    }

    public GameObject GetPooledObject()
    {
        for(int i = 0; i < m_AmountToPool; i++)
        {
            if (!m_PooledStars[i].activeInHierarchy)
                return m_PooledStars[i];
        }
        return null;
    }


}