using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StarController : MonoBehaviour
{
    public Star m_starClass;
    [SerializeField] private SpriteRenderer m_spriteRenderer;
    [SerializeField] private float m_scale;
    [SerializeField] private float m_speed;
    [SerializeField] private Vector2 m_spawnPosition;

    private void Awake()
    {
        m_spriteRenderer = GetComponent<SpriteRenderer>();
        CreateStarClass();
    }


    private void OnEnable()
    {
        if (m_starClass == null) return;
        UpdateClassValues();

        SetColor(m_starClass.GetColor());
        SetPosition(m_spawnPosition);
        SetScale(m_scale);
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