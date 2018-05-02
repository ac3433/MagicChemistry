using UnityEngine;
using System.Collections;

public class CanvasManager : MonoBehaviour
{

    #region Singleton
    private static LevelManager_v2 _instance;
    //Used only once to ensure when one thread have access to create the instance
    private static readonly object _Lock = new object();

    public static LevelManager_v2 Instance
    {
        get
        {
            //thread safe!
            lock (_Lock)
            {
                if (_instance != null)
                    return _instance;
                LevelManager_v2[] instances = FindObjectsOfType<LevelManager_v2>();
                //see if there are any already more instance of this
                if (instances.Length > 0)
                {
                    //yay only 1 instance so give it back
                    if (instances.Length == 1)
                        return _instance = instances[0];

                    //remove all other instance of it other than the 1st one
                    for (int i = 1; i < instances.Length; i++)
                        Destroy(instances[i]);
                    return _instance = instances[0];
                }

                GameObject manage = new GameObject("LevelManager_v2");
                manage.AddComponent<GameController>();

                return _instance = manage.GetComponent<LevelManager_v2>();
            }
        }
    }
    #endregion

    [SerializeField]
    private Canvas _canvas;

    private RectTransform _canvasRect;

    private void Start()
    {
        if (_canvas != null)
            _canvasRect = _canvas.GetComponent<RectTransform>();
    }

    public Vector2 WorldToCanvasPoint(Vector3 a_position)
    {
        Vector2 viewport = Camera.main.WorldToViewportPoint(a_position);
        return Vector2.right * (viewport.x - 0.5f) * _canvasRect.rect.width + Vector2.up * (viewport.y - 0.5f) * _canvasRect.rect.width;
    }
}
