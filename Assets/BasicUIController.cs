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
    [SerializeField] private VisualElement m_chopButton;
    [SerializeField] private Label m_conversationTextElement;
    [SerializeField] private float m_spriteChangeAmount = 20f;
    [Range(0.4f, 2f)]
    [SerializeField] private float m_timeBetweenSpriteChanges = 0.5f;
    [SerializeField] private PlayerChopController m_playerChopController;
    private int m_currentSpriteIndex = 0;
    [SerializeField] private Color m_defaultColor = Color.white;
    [SerializeField] private Color m_hoverColor = Color.black;
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
        m_chopButton = root.Q<VisualElement>("chopButton");
        m_defaultColor = m_chopButton.resolvedStyle.backgroundColor;
        RegisterCallbacks();
    }
    private void RegisterCallbacks()
    {
        m_chopButton.RegisterCallback<PointerDownEvent>(evt => m_playerChopController.AdvanceSwingState());
        m_chopButton.RegisterCallback<PointerUpEvent>(evt =>
        {

            m_playerChopController.StartRevertToDefault();

        });
        m_chopButton.RegisterCallback<PointerOverEvent>(evt =>
        {
            if(m_chopButton.resolvedStyle.backgroundColor == m_defaultColor)
            {
                m_chopButton.style.backgroundColor = m_hoverColor;
            }
            StartCoroutine("CheckMouseOverPosition");
        });
        
        
    }
    private IEnumerator CheckMouseOverPosition()
    {
        /*Debug.Log("mouse is at: " + Input.mousePosition);
        Debug.Log("Button xPos: " + m_chopButton.worldTransform.GetPosition().x + " yPos: " + m_chopButton.localBound + " layout: " + m_chopButton.layout);*/
        
        WaitForSeconds delay = new WaitForSeconds(0.25f);
        yield return delay;
        var mousePos = Input.mousePosition;
        if(mousePos.x > m_chopButton.localBound.position.x && mousePos.x < m_chopButton.localBound.x + m_chopButton.localBound.width)
        {
            
            if(mousePos.y > m_chopButton.localBound.y + 50f && mousePos.y < m_chopButton.localBound.y + m_chopButton.localBound.height + 50)
            {
                Debug.Log("Mouse is in the area");
                yield return StartCoroutine("CheckMouseOverPosition");
            }
            m_chopButton.style.backgroundColor = m_defaultColor;
            yield return null;
        }
        m_chopButton.style.backgroundColor = m_defaultColor;
        yield return null;
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
