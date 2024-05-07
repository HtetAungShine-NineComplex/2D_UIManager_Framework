using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Canvas))]
[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(GraphicRaycaster))]
public abstract class UIBase : MonoBehaviour
{
    protected Canvas m_Canvas;
    protected RectTransform m_RectTransform;
    protected GraphicRaycaster m_GraphicRaycaster;
    public int SortOrder
    {
        set
        {
            if (m_Canvas != null)
            {
                m_Canvas.overrideSorting = true;
                m_Canvas.sortingOrder = value;
            }
        }
        get
        {
            if (m_Canvas != null)
                return m_Canvas.sortingOrder;
            return 0;
        }
    }
    public bool IsCamUI = false;
    public bool Opened
    {
        get
        {
            return gameObject.activeInHierarchy;
        }
    }
    protected virtual void OnShow(UIBaseData Data = null)
    {
        
    }
    protected virtual void OnClose()
    { }
    protected virtual void OnInit()
    { }
    protected virtual void OnStart()
    { }
    protected virtual void OnUpdate()
    { }
    protected virtual void OnFocus()
    { }
    protected virtual void OnDefocus()
    { }
    protected virtual void OnEvent()
    {}

    protected virtual void UpdateAllText()
    {
    }

    protected virtual void Awake()
    {
        m_RectTransform = GetComponent<RectTransform>();
        m_Canvas = GetComponent<Canvas>();
        m_GraphicRaycaster = GetComponent<GraphicRaycaster>();
    }

    protected virtual void Start()
    {
        m_RectTransform.offsetMax = Vector2.zero;
        m_RectTransform.offsetMin = Vector2.zero;
        m_RectTransform.anchorMax = Vector2.one;
        m_RectTransform.anchorMin = Vector2.zero;
        m_RectTransform.localScale = Vector3.one;
        OnStart();
    }

    protected virtual void Update()
    {
        OnUpdate();
    }
    public void Init()
    {
        OnInit();
    }

	public void Show(UIBaseData Data)
    {
        if (Opened)
            return;
        gameObject.SetActive(true);
        OnShow(Data);
    }
    public void Close()
    {
        if (!Opened)
            return;
        gameObject.SetActive(false);
        OnClose();
    }
    public void Focus()
    {
        if (!Opened)
            return;
        OnFocus();
    }
    public void Defocus()
    {
        if (!Opened)
            return;
        OnDefocus();
    }
}

public class UIBaseData
{

}
