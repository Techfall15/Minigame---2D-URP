using UnityEngine;

public class StarController : MonoBehaviour
{
    [SerializeField] private float m_starScale = 1f;
    [SerializeField] private Vector2 m_positionLimits;

    protected void Start()
    {
        transform.localScale = new Vector2(m_starScale,m_starScale);
        var randomXPos = Random.Range(-m_positionLimits.x, m_positionLimits.x);
        var randomYPos = Random.Range(-m_positionLimits.y, m_positionLimits.y);

        transform.position = new Vector2(randomXPos, randomYPos);
    }



}
