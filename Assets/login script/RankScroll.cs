using System.Collections;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using UnityEngine;
using UnityEngine.EventSystems;
public class RankScroll : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public RectTransform content;
    public List<Info> infos = new List<Info>();
    private Vector3 lastPosition;
    private float offset;

    private float size;
    private float index0y;

    public void Awake()
    {
        size = infos[0].GetComponent<RectTransform>().rect.size.y;
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        lastPosition = eventData.position;
        index0y = infos[0].transform.localPosition.y;
    }

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
            // scroll up
            // 使
            
            if (infos[0].transform.localPosition.y >= index0y)
            {
                Debug.Log(infos[0].transform.localPosition.y + "        " + index0y);
                infos[infos.Count - 1].transform.localPosition = infos[infos.Count - 2].transform.localPosition - Vector3.up * size;
            }

            //if (infos[0].transform.localPosition.y > index0y + size)
            //{
            //    Info tmp = infos[0];
            //    tmp.transform.localPosition = infos[infos.Count - 1].transform.localPosition - Vector3.up * size;
            //    for (int i = 1; i < infos.Count; i++)
            //    {
            //        infos[i - 1] = infos[i];
            //    }
            //    infos[infos.Count - 1] = tmp;
            //}
        }
        else if (offset < 0)
        {
            // scroll down
        }

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        
    }
}
