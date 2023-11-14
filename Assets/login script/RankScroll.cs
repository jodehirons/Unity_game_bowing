using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 处理排行榜显示的滚动功能。
/// </summary>
public class RankScroll : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public RectTransform content;
    public List<Info> infos = new List<Info>();
    private Vector3 lastPosition;
    private float offset;

    private float size;
    private float index0y;

    /// <summary>
    /// 初始化 RankScroll 组件。
    /// </summary>
    public void Awake()
    {
        size = infos[0].GetComponent<RectTransform>().rect.size.y;
    }

    /// <summary>
    /// 当拖动开始时调用。
    /// </summary>
    public void OnBeginDrag(PointerEventData eventData)
    {
        lastPosition = eventData.position;
        index0y = infos[0].transform.localPosition.y;
    }

    /// <summary>
    /// 当对象被拖动时调用。
    /// </summary>
    public void OnDrag(PointerEventData eventData)
    {
        offset = eventData.position.y - lastPosition.y;
        lastPosition = eventData.position;

        for (int i = 0; i < infos.Count; i++)
        {
            infos[i].transform.localPosition += Vector3.up * offset;
        }

        if (offset > 0)
        {
            // 向上滚动逻辑
            if (infos[0].transform.localPosition.y >= index0y)
            {
                // 实现向上滚动的逻辑
                infos[infos.Count - 1].transform.localPosition = infos[infos.Count - 2].transform.localPosition - Vector3.up * size;
            }
        }
        else if (offset < 0)
        {
            // 向下滚动逻辑
        }
    }

    /// <summary>
    /// 当拖动结束时调用。
    /// </summary>
    public void OnEndDrag(PointerEventData eventData)
    {
        // 当拖动结束时的逻辑
    }
}
