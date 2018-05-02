using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    //Considering that only one object at any point of time will be dragged.
    //This make sense to use a static accesor
    public static GameObject DraggedObject;

    [SerializeField]
    private float _dragSpeed = 1.01f;
    [SerializeField]
    private BoxCollider2D _collider;
    private Vector2 _colliderStartSize;
    private bool _lockRotation;
    private bool _lockDragging;
    private bool _lockCloning;



    void Start()
    {
        _lockRotation = true;
        _colliderStartSize = _collider.size;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if(!_lockCloning)
        {
            GameObject copyVerison = Instantiate(gameObject);
            copyVerison.transform.SetParent(gameObject.transform.parent);
            copyVerison.transform.localPosition = gameObject.transform.localPosition;
            copyVerison.name = gameObject.name;
        }
        DraggedObject = gameObject;
        _collider.size = Vector2.zero;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Vector2.Lerp(transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition), _dragSpeed);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.localPosition = Vector3.zero;
        _collider.size = _colliderStartSize;
        LockCloning();
        UnlockRotation();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        
    }

    public void LockRotation()
    {
        _lockRotation = true;
    }

    public void UnlockRotation()
    {
        _lockRotation = false;
    }

    public void LockDragging()
    {
        _lockDragging = true;
    }

    private void LockCloning()
    {
        _lockCloning = true;
    }

    public void SetPoint(Point p)
    {

    }
}
