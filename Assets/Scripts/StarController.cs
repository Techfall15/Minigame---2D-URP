using UnityEngine;


public class StarController : MonoBehaviour
{
    public Star m_starClass;
    [SerializeField] private SpriteRenderer m_spriteRenderer;
    private Rigidbody2D m_rigidbody2D;
    [SerializeField] private float m_scale;
    [SerializeField] private float m_speed;
    [SerializeField] private Vector2 m_spawnPosition;

    private void Awake()
    {
        m_spriteRenderer = GetComponent<SpriteRenderer>();
        m_rigidbody2D = GetComponent<Rigidbody2D>();
        CreateStarClass();
    }


    private void OnEnable()
    {
        if (m_starClass == null) return;
        UpdateClassValues();

        SetColor(m_starClass.GetColor());
        SetPosition(m_spawnPosition);
        SetScale(m_scale);
        m_rigidbody2D.velocity = Vector2.right * (m_speed * Time.deltaTime);
    }
    private void Update()
    {
        if (transform.position.x > 5f) transform.position = new Vector2(-5.25f, transform.position.y);
    }
    private void UpdateClassValues()
    {
        m_scale = m_starClass.GetScale();
        m_speed = m_starClass.GetSpeed();
        m_spawnPosition = m_starClass.GetSpawnPosition();
    }
    private void CreateStarClass() => m_starClass = new Star();
    private void SetPosition(Vector2 pos) => transform.position = pos;
    private void SetScale(float scale) => transform.localScale = new Vector2(scale, scale);
    private void SetColor(Color color) => m_spriteRenderer.color = color;
}