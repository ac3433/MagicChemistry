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
    private Vector3 _startPositon;
    private Transform _startParent;

    private bool _lockRotation;
    private bool _lockDragging;
    private bool _lockCloning;

    private AbstractTube data;

    void Start()
    {
        _lockRotation = true;
        _colliderStartSize = _collider.size;
        data = GetComponent<AbstractTube>();
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
        _startPositon = transform.position;
        _startParent = transform.parent;
        LevelManager_v2.Instance.RemoveTubeOnGrid(data.GetPoint());
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Vector2.Lerp(transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition), _dragSpeed);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if(data.Replacable && data.Valid())
        {
            transform.localPosition = Vector3.zero;
            
            LockCloning();
            UnlockRotation();
        }
        else
        {
            transform.SetParent(_startParent);
            transform.position = _startPositon;
        }
        LevelManager_v2.Instance.PlaceTubeOnGrid(gameObject, data.GetPoint());
        _collider.size = _colliderStartSize;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(Input.GetMouseButtonUp(0))
            if(!_lockRotation)
            {
                data.RotateClockwise();
            }

        if (Input.GetMouseButtonUp(1))
        {
            LevelManager_v2.Instance.RemoveTubeOnGrid(data.GetPoint());
            DestroyObject(gameObject);
        }
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
        data.SetPoint(p);
    }
}
