using UnityEngine;
using UnityEngine.UI;

public class FloatingText : GUIWidget
{
    [Header("Config")]
    public float floatingHeight = 100f;
    public float lifeSpan = 1f;

    private Text uiText;

    private float t0;
    private Vector3 v0;

    public string Text
    {
        get
        {
            return uiText.text;
        }

        set
        {
            uiText.text = value;
        }
    }

    public Color TextColor
    {
        get
        {
            return uiText.color;
        }

        set
        {
            uiText.color = value;
        }
    }

    private void Awake()
    {
        uiText = GetComponent<Text>();
    }

    private void OnEnable()
    {
        t0 = Time.time;
        v0 = transform.position;
    }

    private void Update()
    {
        float alpha = (Time.time - t0) / lifeSpan;

        if (alpha > 1)
            GUIManager.Singleton.DestroyFloatingText(this);
        else
        {
            transform.position = v0 + new Vector3(0, floatingHeight * alpha, 0);

            Color color = TextColor;
            color.a = alpha;

            uiText.color = color;
        }
    }
}
