using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


public class PlayerChopController : MonoBehaviour
{

    [SerializeField] private float m_currentPower = 0f;
    [SerializeField] private GameObject m_playerPivot;
    [SerializeField] private ParticleSystem logParticles;
    [SerializeField] private SpriteRenderer m_spriteRenderer;
    [SerializeField] private Sprite[] m_spriteArray = new Sprite[3];
    [SerializeField] private Vector3 m_xPivotStateValues = new Vector3(0f, -0.25f, 0.25f);

    [SerializeField] private Vector3 m_yOffsetStateValues = new Vector3( 0.5f, 0.5f, 0.65f);

    [SerializeField] private Vector3 m_zRotationStateValues = new Vector3(0f, -90f, 45f);
    

    [SerializeField] private enum m_swingState { BeforeChop, DuringChop, AfterChop };
    [SerializeField] private m_swingState m_currentSwingState;

    private void Awake()
    {
        m_currentSwingState = new m_swingState();
        m_currentSwingState = m_swingState.BeforeChop;
        m_spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void Start()
    {
        if(m_spriteRenderer != null) SetSwingStateValuesFor(m_currentSwingState);
    }
    public void StartRevertToDefault()
    {
        StopCoroutine("RevertToDefault");
        AdvanceSwingState();
        StartCoroutine("RevertToDefaultCoroutine", 0.5f);
        // Get reference to the log to start particles. Eventually make an event.
        logParticles.Play();
    }
    
    public void AdvanceSwingState()
    {
        SetNextSwingState((int)m_currentSwingState);
        SetSwingStateValuesFor(m_currentSwingState);
    }
    private IEnumerator RevertToDefaultCoroutine(float delay)
    {
        WaitForSeconds newDelay = new WaitForSeconds(delay);
        yield return newDelay;

        AdvanceSwingState();

        yield return null;
    }
    public void BuildChargeMeterBy(float amount)
    {
        m_currentPower += amount;
        Debug.Log("current Power is: " + m_currentPower);
    }
    private void SetSwingStateValuesFor(m_swingState currentState)
    {
        var state = (int)currentState;
        SetPivotTransform(state);
        m_spriteRenderer.sprite = m_spriteArray[state];
    }
    private void SetNextSwingState(int state)
    {
        if (state == 0) m_currentSwingState = m_swingState.DuringChop;
        else if (state == 1) m_currentSwingState = m_swingState.AfterChop;
        else m_currentSwingState = m_swingState.BeforeChop;
    }
    private void SetPivotTransform(int state)
    {
        var pivotTransform = m_playerPivot.transform;
        var pivotPos = pivotTransform.localPosition;

        pivotTransform.localPosition = new Vector3(m_xPivotStateValues[state], m_yOffsetStateValues[state], pivotPos.z);
        pivotTransform.localRotation = Quaternion.Euler(0f, 0f, m_zRotationStateValues[state]);
    }
}
