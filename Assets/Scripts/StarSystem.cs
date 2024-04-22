using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StarSystem: MonoBehaviour
{

    [SerializeField] private int m_spawnAmount = 10;
    [SerializeField] private float m_starScale = 1f;
    [SerializeField] private Vector2 m_spawnPositionLimit = new Vector2(8.5f, 4.5f);
    [SerializeField] private Color m_starColor = Color.white;

}
