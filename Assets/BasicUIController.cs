using UnityEngine;
using UnityEngine.UIElements;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Serialization;


public class BasicUIController : MonoBehaviour
{

    [FormerlySerializedAs("targetDoc")] [SerializeField] private UIDocument m_targetDoc;
    [SerializeField] private List<Sprite> m_spriteList = new List<Sprite>();
    private VisualElement m_xButtonElement;
    private VisualElement m_chopButton;
    private Label m_conversationTextElement;
    [SerializeField] private float m_spriteChangeAmount = 20f;
    [Range(0.4f, 2f)]
    [SerializeField] private float m_timeBetweenSpriteChanges = 0.5f;
    [SerializeField] private PlayerChopController m_playerChopController;
    private int m_currentSpriteIndex;
    [SerializeField] private Color m_defaultColor = Color.white;
    [SerializeField] private Color m_hoverColor = Color.black;
    [SerializeField] private Color m_clickColor = Color.black;


    protected void Awake()
    {
        var root = m_targetDoc.rootVisualElement;
        InitializeElements(root); 
    }
    private void Start()
    {
        m_xButtonElement.style.backgroundImage = Background.FromSprite(m_spriteList[m_currentSpriteIndex]);
        StartCoroutine(ToggleXButtonImageCoroutine(m_timeBetweenSpriteChanges));
    }


    #region Private Interface
    private void InitializeElements(VisualElement root)
    {
        m_conversationTextElement   = root.Q<Label>("ConversationText");
        m_chopButton                = root.Q<VisualElement>("chopButton");
        m_xButtonElement            = root.Q<VisualElement>("buttonXElement");
        
        m_defaultColor = m_chopButton.resolvedStyle.backgroundColor;
        RegisterCallbacks();
    }
    private void RegisterCallbacks()
    {
        m_chopButton.RegisterCallback<PointerDownEvent>(evt =>
        {
            m_playerChopController.AdvanceSwingState();
            if (m_chopButton.resolvedStyle.backgroundColor != m_clickColor) StartCoroutine("SetBgColorToClick", evt.target);
        });
        m_chopButton.RegisterCallback<PointerUpEvent>(evt => 
        {
            m_playerChopController.StartRevertToDefault();
            if (m_chopButton.resolvedStyle.backgroundColor != m_hoverColor) StartCoroutine("SetBgColorToHover", evt.target);
        });
        m_chopButton.RegisterCallback<PointerOverEvent>(evt =>
        {
            if (m_chopButton.resolvedStyle.backgroundColor == m_defaultColor) StartCoroutine("SetBgColorToHover", evt.target);
            StartCoroutine(nameof(CheckMouseOverPosition), evt.target);
        });  
    }
    private static bool IsMouseOverElement(VisualElement element)
    {
        var mousePosition    = Input.mousePosition;
        var mouseX      = mousePosition.x;
        var mouseY      = mousePosition.y;
        var elementLB   = element.localBound;
        var elementX    = elementLB.x;
        var elementY    = elementLB.y;

        if (mouseX < elementX       || mouseX > elementX + elementLB.width) return false;
        return !(mouseY < elementY + 50f) && !(mouseY > elementY + elementLB.height + 50);
    }
    private IEnumerator CheckMouseOverPosition(VisualElement element)
    {
        var delay = new WaitForSeconds(0.1f);
        yield return delay;

        if (IsMouseOverElement(element)) yield return StartCoroutine(nameof(CheckMouseOverPosition), element);
        yield return StartCoroutine(nameof(ResetBgColor), element);
    }
    private IEnumerator ResetBgColor(VisualElement element) { yield return element.style.backgroundColor = m_defaultColor; }
    private IEnumerator SetBgColorToHover(VisualElement element) { yield return element.style.backgroundColor = m_hoverColor; }
    private IEnumerator SetBgColorToClick(VisualElement element) { yield return element.style.backgroundColor = m_clickColor; }
    private IEnumerator SetDisplayToNone(VisualElement element) { yield return element.style.display = DisplayStyle.None; }
    private IEnumerator SetDisplayToFlex(VisualElement element) { yield return element.style.display = DisplayStyle.Flex; }
    private IEnumerator ToggleXButtonImageCoroutine(float delay)
    {
        var newWait = new WaitForSeconds(delay);
        
        m_currentSpriteIndex = (m_currentSpriteIndex < m_spriteList.Count - 1) ? 1 : 0;
        m_xButtonElement.style.backgroundImage = Background.FromSprite(m_spriteList[m_currentSpriteIndex]);
        yield return newWait;
        
        if (!(m_spriteChangeAmount > 0)) yield break;
        
        m_spriteChangeAmount--;
        StartCoroutine(nameof(ToggleXButtonImageCoroutine),m_timeBetweenSpriteChanges);
    }
    
    #endregion
}
