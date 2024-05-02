using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


public class PlayerChopController : MonoBehaviour
{

    [SerializeField] private float m_currentPower = 0f;
    [SerializeField] private GameObject m_playerPivot;
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
        if(m_spriteRenderer != null) InitializeSwingStateValues(m_currentSwingState);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            AdvanceSwingState();
        }
    }
    public void StartRevertToDefault()
    {
        AdvanceSwingState();
        StartCoroutine("RevertToDefaultCoroutine", 0.5f);
    }
    public void AdvanceSwingState()
    {
        IncrementSwingState(m_currentSwingState);
        Debug.Log("m_currentSwingState changed to: " + m_currentSwingState);
        InitializeSwingStateValues(m_currentSwingState);
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
    private void InitializeSwingStateValues(m_swingState currentState)
    {
        var pivotTransform = m_playerPivot.transform;
        var pivotPos = pivotTransform.localPosition;
        
        switch (currentState)
        {
            case m_swingState.BeforeChop:
                pivotTransform.localPosition = new Vector3(m_xPivotStateValues[0], m_yOffsetStateValues[0], pivotPos.z);
                pivotTransform.localRotation = Quaternion.Euler(0f, 0f, m_zRotationStateValues[0]);
                m_spriteRenderer.sprite = m_spriteArray[(int)currentState];
                break;
            case m_swingState.DuringChop:
                pivotTransform.localPosition = new Vector3(m_xPivotStateValues[1], m_yOffsetStateValues[1], pivotPos.z);
                pivotTransform.localRotation = Quaternion.Euler(0f, 0f, m_zRotationStateValues[1]);
                m_spriteRenderer.sprite = m_spriteArray[(int)currentState];
                break;
            case m_swingState.AfterChop:
                pivotTransform.localPosition = new Vector3(m_xPivotStateValues[2], m_yOffsetStateValues[2], pivotPos.z);
                pivotTransform.localRotation = Quaternion.Euler(0f, 0f, m_zRotationStateValues[2]);
                m_spriteRenderer.sprite = m_spriteArray[(int)currentState];
                break;
            default:
                Debug.Log("Error Initializing Swing State Values");
                break;
        }
    }
    private void IncrementSwingState(m_swingState currentState)
    {
        var state = (int)currentState;
        if (state == 0) m_currentSwingState = m_swingState.DuringChop;
        else if (state == 1) m_currentSwingState = m_swingState.AfterChop;
        else m_currentSwingState = m_swingState.BeforeChop;
    }
}
