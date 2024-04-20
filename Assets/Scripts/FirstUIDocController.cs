using UnityEngine;
using UnityEngine.UIElements;

public class FirstUIDocController : MonoBehaviour
{
    public NoiseTriggerController roadNoiseTriggerLeft;
    public NoiseTriggerController roadNoiseTriggerRight;
    public NoiseTriggerController paintNoiseTriggerLeft;
    public NoiseTriggerController paintNoiseTriggerRight;

    protected void OnEnable()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;

        Button startShadersBtn = root.Q<Button>("StartShaders");
        Button stopShadersBtn = root.Q<Button>("StopShaders");
        Button logMessageBtn = root.Q<Button>("LogMessage");

        startShadersBtn.clicked += () => roadNoiseTriggerLeft.SetNoiseTriggerTo(1f);
        startShadersBtn.clicked += () => roadNoiseTriggerRight.SetNoiseTriggerTo(1f);
        startShadersBtn.clicked += () => paintNoiseTriggerLeft.SetNoiseTriggerTo(1f);
        startShadersBtn.clicked += () => paintNoiseTriggerRight.SetNoiseTriggerTo(1f);

        stopShadersBtn.clicked += () => roadNoiseTriggerLeft.SetNoiseTriggerTo(0f);
        stopShadersBtn.clicked += () => roadNoiseTriggerRight.SetNoiseTriggerTo(0f);
        stopShadersBtn.clicked += () => paintNoiseTriggerLeft.SetNoiseTriggerTo(0f);
        stopShadersBtn.clicked += () => paintNoiseTriggerRight.SetNoiseTriggerTo(0f);

        logMessageBtn.clicked += () => Debug.Log("You clicked the log message button!");
    }



}
