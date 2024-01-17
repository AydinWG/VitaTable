using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Drag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Image image;

    private Image imageClone;

    private Image[] gamePlanSlotArr;

    [HideInInspector] public Transform parentAfterDrag;

    public void OnBeginDrag(PointerEventData eventData)
    {
        imageClone = Instantiate(image);
        imageClone.transform.SetParent(transform.parent);
        imageClone.transform.localScale = new Vector3(1, 1, 1);

        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        image.raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (imageClone.transform.childCount > 0 || image.transform.childCount > 0)
        {
            Destroy(imageClone.gameObject);
        }

        transform.SetParent(parentAfterDrag);
        image.raycastTarget = true;

        //else if (imageClone.transform.childCount <= 1 || image.transform.childCount <= 1)
        //{
        //    transform.SetParent(parentAfterDrag);
        //    image.raycastTarget = true;
        //}
    }
}
