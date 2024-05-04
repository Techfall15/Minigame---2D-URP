using UnityEngine;


[System.Serializable]
public class Star
{
    [SerializeField] private float m_scale = 1f;
    [SerializeField] private float m_speed = 1f;
    [SerializeField] private Color m_color = Color.white;
    [SerializeField] private Vector2 m_spawnPosition;
    [SerializeField] private int m_starIndex;

    public void SetScaleTo(float scale) => m_scale = scale;
    public float GetScale() => m_scale;


    public void SetSpeedTo(float speed) => m_speed = speed;
    public float GetSpeed() => m_speed;


    public void SetColorTo(Color color) => m_color = color;
    public Color GetColor() => m_color;


    public void SetSpawnPositionTo(Vector2 position) => m_spawnPosition = position;
    public Vector2 GetSpawnPosition() => m_spawnPosition;


    public void SetIndexTo(int value) => m_starIndex = value;
    public int GetIndex() => m_starIndex;
}