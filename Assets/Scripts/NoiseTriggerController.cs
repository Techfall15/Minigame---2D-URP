using UnityEngine;

public class NoiseTriggerController : MonoBehaviour
{
    [SerializeField] private SpriteRenderer m_Renderer;

    protected void Awake()
    {
        m_Renderer = GetComponent<SpriteRenderer>();
    }
    protected void OnEnable()
    {
        if (m_Renderer != null) SetNoiseTriggerTo(1f);
        else Debug.Log("No renderer found on gameobject: " + this.gameObject.name);
    }
    protected void OnDisable()
    {
        if (m_Renderer != null) SetNoiseTriggerTo(0f);
        else Debug.Log("No renderer found on gameobject: " + this.gameObject.name);
    }



    public void SetNoiseTriggerTo(float value) => m_Renderer.sharedMaterial.SetFloat("_NoiseTrigger", value);
    
}
