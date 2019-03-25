using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class GUIWindow : MonoBehaviour
{
    public virtual void OnOpen(params object[] args) { }
    public virtual void OnClose() { }

    public virtual void Redraw(params object[] args) { }

    public virtual void Close()
    {
        GUIManager.Singleton.Close(GetType().Name);
    }
}

public abstract class GUIWidget : MonoBehaviour
{
    public virtual void Initialize(params object[] args) { }

    public virtual void Show()
    {
        gameObject.SetActive(true);
    }

    public virtual void Hide()
    {
        gameObject.SetActive(false);
    }
}

public class GUIManager : MonoBehaviour
{
    /// <summary>
    /// The unique instance
    /// </summary>
    public static GUIManager Singleton { get; private set; }

    /// <summary>
    /// The way to open the new window
    /// </summary>
    public enum UIMode : int
    {
        /// <summary>
        /// The window should be closed in a short period of time, therefore making the viewport not clear when it is not closed
        /// </summary>
        DEFAULT = 0,

        /// <summary>
        /// The windows is considered to be a part of the viewport
        /// </summary>
        PERMANENT,
    }

    [Header("References")]
    public Sprite emptyHeart;
    public Sprite halfHeart;
    public Sprite fullHeart;

    private Stack<string> uiWindowStack = new Stack<string>();
    private Dictionary<string, GUIWindow> uiWindowsOpened = new Dictionary<string, GUIWindow>();

    private Stack<FloatingText> reusableFloatingTexts;

    /// <summary>
    /// Whether the UI window is opened in the viewport (not considering the UIMode)
    /// </summary>
    /// <param name="name"> The name of the UI window </param>
    /// <returns> Whether the window is opened in the viewport </returns>
    public bool IsInViewport(string name)
    {
        return uiWindowsOpened.ContainsKey(name);
    }

    /// <summary>
    /// Whether any UI windows are opened with UIMode.DEFAULT
    /// </summary>
    /// <returns> Whether any UI windows are opened with UIMode.DEFAULT </returns>
    public bool IsViewportClear()
    {
        return uiWindowStack.Count != 0;
    }

    /// <summary>
    /// Open a new UI window
    /// </summary>
    /// <param name="name"> The name of the UI window to be opened </param>
    /// <param name="mode"> The mode to be used to open the UI window </param>
    /// <param name="args"> Extra arguments passed to UIWindow.OnOpen() </param>
    /// <returns></returns>
    public GUIWindow Open(string name, UIMode mode = UIMode.DEFAULT, params object[] args)
    {
#if UNITY_EDITOR
        Debug.Log(LogUtility.MakeLogStringFormat("UI", IsInViewport(name) ? name + " is already in viewport" : "Open " + name));
#endif

        if (IsInViewport(name))
            return uiWindowsOpened[name];

        GUIWindow uiWindow = Instantiate(ResourceUtility.GetGUIPrefab<GUIWindow>(name), transform, false);

        uiWindow.transform.SetAsFirstSibling();

        uiWindowsOpened.Add(name, uiWindow);

        uiWindow.OnOpen(args);

        if (mode != UIMode.PERMANENT)
            uiWindowStack.Push(name);

        return uiWindow;
    }

    public void Redraw(string name, params object[] args)
    {
        if (IsInViewport(name))
            uiWindowsOpened[name].Redraw(args);
    }

    /// <summary>
    /// Close a previously opened UI window
    /// </summary>
    /// <param name="name"> The name of the UI window to be closed </param>
    public void Close(string name)
    {
#if UNITY_EDITOR
        Debug.Log(LogUtility.MakeLogStringFormat("UI", IsInViewport(name) ? "Close " + name : name + " is not in viewport"));
#endif

        if (IsInViewport(name))
        {
            GUIWindow ui = uiWindowsOpened[name];

            ui.OnClose();

            Stack<string> s = new Stack<string>();

            while (uiWindowStack.Peek().CompareTo(name) != 0)
                s.Push(uiWindowStack.Pop());

            uiWindowStack.Pop();

            while (s.Count > 0)
                uiWindowStack.Push(s.Pop());

            Destroy(ui.gameObject);

            uiWindowsOpened.Remove(name);
        }
    }

    /// <summary>
    /// Open the named UI window if it is not opened or close it if it was previously opened
    /// </summary>
    /// <param name="name"> The name of the UI window to be toggled </param>
    public void Toggle(string name)
    {
        if (uiWindowsOpened.ContainsKey(name))
            Close(name);
        else
            Open(name);
    }

    /// <summary>
    /// Convert pixel position to canvas position (can be used to assign transform.localPosition)
    /// </summary>
    /// <param name="position"> The pixel position to be converted </param>
    /// <returns> The converted canvas position </returns>
    public Vector3 GetCanvasPosition(Vector3 position)
    {
        Vector2 referenceResolution = GetComponent<CanvasScaler>().referenceResolution;
        return new Vector3((position.x / Screen.width - 0.5f) * referenceResolution.x, (position.y / Screen.height - 0.5f) * referenceResolution.y, 0);
    }

    public void CreateFloatingTextForDamage(float damage, Vector3 worldPosition)
    {
        FloatingText floatingText = reusableFloatingTexts.Count > 0 ? reusableFloatingTexts.Pop() : Instantiate(ResourceUtility.GetGUIPrefab<FloatingText>("FloatingText"), transform);

        floatingText.Text = Mathf.FloorToInt(damage).ToString();
        floatingText.transform.position = Camera.main.WorldToScreenPoint(worldPosition);
        floatingText.gameObject.SetActive(true);
    }

    public void DestroyFloatingText(FloatingText floatingText)
    {
        floatingText.gameObject.SetActive(false);

        reusableFloatingTexts.Push(floatingText);
    }

    private void Awake()
    {
        if (!Singleton)
        {
            Singleton = this;

            reusableFloatingTexts = new Stack<FloatingText>();
        }
        else if (Singleton != this)
            Destroy(gameObject);
    }
}
