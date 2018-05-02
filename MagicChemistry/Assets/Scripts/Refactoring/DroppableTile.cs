using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class DroppableTile : MonoBehaviour, IDropHandler
{
    private GameObject Tube
    {
        
        get
        {
            if (transform.childCount > 0)
            {
                foreach (Transform child in transform)
                {
                    if (string.Equals(child.tag, "Tube"))
                        return child.gameObject;
                }
            }
            return null;
        }
    }
    public void OnDrop(PointerEventData eventData)
    {
        if(Tube != null)
        {
            DestroyObject(Tube);
        }

        Draggable.DraggedObject.transform.SetParent(transform);
        //eventData.pointerDrag.transform.GetComponent<MouseInteraction>()
    }
}
