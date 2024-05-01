using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


public class PlayerChopController : MonoBehaviour
{

    [SerializeField] private float m_currentPower = 0f;
    [SerializeField] private GameObject m_playerPivot;

    [SerializeField] private float m_xPivotPositionBeforeChop = 0f;
    [SerializeField] private float m_xPivotPositionDuringChop = -0.25f;
    [SerializeField] private float m_xPivotPositionAfterChop = 0.25f;

    [SerializeField] private float m_yOffSetBeforeChop = 0.5f;
    [SerializeField] private float m_yOffsetDuringChop = 0.5f;
    [SerializeField] private float m_yOffsetAfterChop = 0.65f;
    
    [SerializeField] private float m_zRotationBeforeChop = 0f;
    [SerializeField] private float m_zRotationDuringChop = -90f;
    [SerializeField] private float m_zRotationAfterChop = 45f;

    public void BuildChargeMeterBy(float amount)
    {
        m_currentPower += amount;
        Debug.Log("current Power is: " + m_currentPower);
    }
}
