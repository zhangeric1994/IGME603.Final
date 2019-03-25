using System;
using UnityEngine;

public class GUIGrid : GUIWidget
{
    [SerializeField] private Vector2 margin = new Vector2(10, 10);
    [SerializeField] private int row = 1;
    [SerializeField] private int column = 1;
    [SerializeField] private Vector2Int direction = new Vector2Int(1, -1);
    [SerializeField] private Vector2 itemSize;
    [SerializeField] private bool hideInactives = true;

    public override void Initialize(params object[] args)
    {
        int itemIndex = 0;
        int N = transform.childCount;

        RectTransform rectTransform = GetComponent<RectTransform>();

        float maxX = -1;
        float maxY = -1;

        for (int childIndex = 0; childIndex < N; childIndex++)
        {
            RectTransform item = transform.GetChild(childIndex).GetComponent<RectTransform>();

            if (hideInactives && !item.gameObject.activeSelf)
                continue;

            int x = itemIndex % column;
            int y = itemIndex / column;

            if (row > 0 && y > row)
                break;

            item.anchorMin = rectTransform.anchorMin;
            item.anchorMax = rectTransform.anchorMax;

            item.pivot = new Vector2Int(-(direction.x - 1) / 2, -(direction.y - 1) / 2);
            item.localPosition = new Vector3(direction.x * ((itemSize.x + margin.x) * x + margin.x), direction.y * ((itemSize.y + margin.y) * y + margin.y), 0);

            if (item.sizeDelta.x > maxX)
                maxX = item.sizeDelta.x;

            if (item.sizeDelta.y > maxY)
                maxY = item.sizeDelta.y;

            itemIndex++;
        }

        if (itemIndex == 0)
            rectTransform.sizeDelta = new Vector2(0, 0);
        else
            rectTransform.sizeDelta = new Vector2(margin.x + Math.Min(column, itemIndex) * ((itemSize.x == 0 ? maxX : itemSize.x) + margin.x), margin.y + ((itemIndex - 1) / column + 1) * ((itemSize.y == 0 ? maxY : itemSize.y) + margin.x));
    }

    [ContextMenu("Refresh")]
    private void ResetName()
    {
        Initialize();
    }
}
