using UnityEngine;
using UnityEngine.UIElements;

public class FirstUIDocController : MonoBehaviour
{
    public NoiseTriggerController m_RoadNoiseTriggerLeft;
    public NoiseTriggerController m_RoadNoiseTriggerRight;
    public NoiseTriggerController m_PaintNoiseTriggerLeft;
    public NoiseTriggerController m_PaintNoiseTriggerRight;

    protected void OnEnable()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        var startShadersBtn = root.Q<Button>("StartShaders");
        var stopShadersBtn = root.Q<Button>("StopShaders");
        var logMessageBtn = root.Q<Button>("LogMessage");

        startShadersBtn.clicked += () => m_RoadNoiseTriggerLeft.SetNoiseTriggerTo(1f);
        startShadersBtn.clicked += () => m_RoadNoiseTriggerRight.SetNoiseTriggerTo(1f);
        startShadersBtn.clicked += () => m_PaintNoiseTriggerLeft.SetNoiseTriggerTo(1f);
        startShadersBtn.clicked += () => m_PaintNoiseTriggerRight.SetNoiseTriggerTo(1f);

        stopShadersBtn.clicked += () => m_RoadNoiseTriggerLeft.SetNoiseTriggerTo(0f);
        stopShadersBtn.clicked += () => m_RoadNoiseTriggerRight.SetNoiseTriggerTo(0f);
        stopShadersBtn.clicked += () => m_PaintNoiseTriggerLeft.SetNoiseTriggerTo(0f);
        stopShadersBtn.clicked += () => m_PaintNoiseTriggerRight.SetNoiseTriggerTo(0f);

        logMessageBtn.clicked += () => Debug.Log("You clicked the log message button!");
    }



}
