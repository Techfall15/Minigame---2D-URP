using UnityEngine;


public class NoiseTriggerController : MonoBehaviour
{
    [SerializeField] private SpriteRenderer m_spriteRenderer;
    private static readonly int NoiseTrigger = Shader.PropertyToID("_NoiseTrigger");

    protected void Awake()
    {
        m_spriteRenderer = GetComponent<SpriteRenderer>();
    }
    protected void OnEnable()
    {
        if (m_spriteRenderer != null) SetNoiseTriggerTo(1f);
        else Debug.Log("No renderer found on gameobject: " + this.gameObject.name);
    }
    protected void OnDisable()
    {
        if (m_spriteRenderer != null) SetNoiseTriggerTo(0f);
        else Debug.Log("No renderer found on gameobject: " + this.gameObject.name);
    }



    public void SetNoiseTriggerTo(float value) => m_spriteRenderer.sharedMaterial.SetFloat(NoiseTrigger, value);
    
}
