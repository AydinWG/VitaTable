using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Drop : MonoBehaviour, IDropHandler
{
    private GameObject droppedBefore;

    public void OnDrop(PointerEventData eventData)
    {
        if (transform.childCount < 1)
        {
            GameObject dropped = eventData.pointerDrag;
            Drag drag = dropped.GetComponent<Drag>();
            droppedBefore = dropped;
            drag.parentAfterDrag = transform;
        }
        else
        {
            GameObject dropped = eventData.pointerDrag;
            Drag drag = dropped.GetComponent<Drag>();
            Destroy(droppedBefore.gameObject);
            droppedBefore = dropped;
            drag.parentAfterDrag = transform;
        }
    }
}