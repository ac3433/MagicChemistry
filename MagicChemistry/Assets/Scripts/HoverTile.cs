using UnityEngine;
using System.Collections;

public class HoverTile : MonoBehaviour
{
    [SerializeField]
    private GameObject _hoverSprite;

    private GameObject _hoverSpriteExist;

    private void Start()
    {
        if (_hoverSprite == null)
            Debug.LogError(string.Format("GameObject: {0}\nScript: TileData\nError: Missing hover sprite.", gameObject.name));
        else
        {
            _hoverSpriteExist = Instantiate(_hoverSprite, gameObject.transform);
            _hoverSpriteExist.transform.parent = gameObject.transform;
            _hoverSpriteExist.SetActive(false);
        }
    }
    private void OnMouseOver()
    {
        _hoverSpriteExist.SetActive(true);
    }

    private void OnMouseExit()
    {
        _hoverSpriteExist.SetActive(false);
    }
}
