using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;


public class BasicUIController : MonoBehaviour
{

    [SerializeField] private UIDocument targetDoc;
    [SerializeField] private List<Sprite> m_spriteList = new List<Sprite>();
    [SerializeField] private VisualElement m_xButtonElement;
    [SerializeField] private Button m_chopButton;
    [SerializeField] private Label m_conversationTextElement;
    [SerializeField] private float m_spriteChangeAmount = 20f;
    [Range(0.4f, 2f)]
    [SerializeField] private float m_timeBetweenSpriteChanges = 0.5f;
    [SerializeField] private PlayerChopController m_playerChopController;
    private int m_currentSpriteIndex = 0;
    protected void Awake()
    {
        var root = targetDoc.rootVisualElement;
        InitializeElements(root);
        
    }
    // Start is called before the first frame update
    void Start()
    {
        m_xButtonElement.style.backgroundImage = Background.FromSprite(m_spriteList[m_currentSpriteIndex]);
        StartCoroutine(ToggleXButtonImageCoroutine(m_timeBetweenSpriteChanges));
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.X))
        {
            m_conversationTextElement.text = (m_conversationTextElement.text == "") ? "This is a basic text ui that could be used for conversations in game.You could also display item or stat info here..." : "";
            m_conversationTextElement.style.display = DisplayStyle.None;
            m_chopButton.style.display = DisplayStyle.Flex;
        }
        
    }

    #region Private Interface
    private void InitializeElements(VisualElement root)
    {
        m_xButtonElement = root.Q<VisualElement>("buttonXElement");
        m_conversationTextElement = root.Q<Label>("ConversationText");
        m_chopButton = root.Q<Button>("chopButton");
    }
     
    private IEnumerator ToggleXButtonImageCoroutine(float delay)
    {
        WaitForSeconds newWait = new WaitForSeconds(delay);
        m_currentSpriteIndex = (m_currentSpriteIndex < m_spriteList.Count - 1) ? 1 : 0;
        m_xButtonElement.style.backgroundImage = Background.FromSprite(m_spriteList[m_currentSpriteIndex]);
        yield return newWait;
        if(m_spriteChangeAmount > 0)
        {
            m_spriteChangeAmount--;
            StartCoroutine(ToggleXButtonImageCoroutine(m_timeBetweenSpriteChanges));
        }
    }
    #endregion
}
