using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D))]
public class DroppableTileData : AbstractTile
{
    [SerializeField]
    private GameObject _hoverSpritePrefab;

    private GameObject _hoverSprite;

    private void Start()
    {
        if(_hoverSpritePrefab != null)
        {
            _hoverSprite = Instantiate(_hoverSpritePrefab, gameObject.transform);
            _hoverSprite.transform.parent = gameObject.transform;
            _hoverSprite.SetActive(false);
        }
    }

    private void OnMouseOver()
    {
        if(_hoverSprite != null)
        {
            _hoverSprite.SetActive(true);
        }
    }

    private void OnMouseExit()
    {
        if (_hoverSprite != null)
        {
            _hoverSprite.SetActive(false);
        }
    }

}
