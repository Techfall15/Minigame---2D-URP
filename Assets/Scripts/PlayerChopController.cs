using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerChopController : MonoBehaviour
{

    [SerializeField] private float m_currentPower;
    [SerializeField] private GameObject m_playerPivot;
    [FormerlySerializedAs("logParticles")] [SerializeField] private ParticleSystem m_logParticles;
    [SerializeField] private SpriteRenderer m_spriteRenderer;
    [SerializeField] private Sprite[] m_spriteArray = new Sprite[3];
    [SerializeField] private Vector3 m_xPivotStateValues = new Vector3(0f, -0.25f, 0.25f);

    [SerializeField] private Vector3 m_yOffsetStateValues = new Vector3( 0.5f, 0.5f, 0.65f);

    [SerializeField] private Vector3 m_zRotationStateValues = new Vector3(0f, -90f, 45f);
    

    [SerializeField] private SwingState m_currentSwingState;
    private enum SwingState { BeforeChop, DuringChop, AfterChop };

    private void Awake()
    {
        m_currentSwingState = new SwingState();
        m_currentSwingState = SwingState.BeforeChop;
        m_spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void Start()
    {
        if(m_spriteRenderer != null) SetSwingStateValuesFor(m_currentSwingState);
    }
    public void StartRevertToDefault()
    {
        StopCoroutine(nameof(RevertToDefaultCoroutine));
        AdvanceSwingState();
        StartCoroutine(nameof(RevertToDefaultCoroutine), 0.5f);
        // Get reference to the log to start particles. Eventually make an event.
        m_logParticles.Play();
    }
    
    public void AdvanceSwingState()
    {
        SetNextSwingState((int)m_currentSwingState);
        SetSwingStateValuesFor(m_currentSwingState);
    }
    private IEnumerator RevertToDefaultCoroutine(float delay)
    {
        var newDelay = new WaitForSeconds(delay);
        yield return newDelay;

        AdvanceSwingState();

        yield return null;
    }
    public void BuildChargeMeterBy(float amount)
    {
        m_currentPower += amount;
        Debug.Log("current Power is: " + m_currentPower);
    }
    private void SetSwingStateValuesFor(SwingState currentState)
    {
        var state = (int)currentState;
        SetPivotTransform(state);
        m_spriteRenderer.sprite = m_spriteArray[state];
    }
    private void SetNextSwingState(int state)
    {
        m_currentSwingState = state switch
        {
            0 => SwingState.DuringChop,
            1 => SwingState.AfterChop,
            _ => SwingState.BeforeChop
        };
    }
    private void SetPivotTransform(int state)
    {
        var pivotTransform = m_playerPivot.transform;
        var pivotPosition = pivotTransform.localPosition;

        pivotTransform.SetLocalPositionAndRotation(
            new Vector3(m_xPivotStateValues[state], m_yOffsetStateValues[state], pivotPosition.z),
            Quaternion.Euler(0f, 0f, m_zRotationStateValues[state]));
    }
}
