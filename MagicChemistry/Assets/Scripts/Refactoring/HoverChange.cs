using UnityEngine;
using System.Collections;

public class HoverChange : MonoBehaviour
{
    [SerializeField]
    private GameObject _hoverSprite;

    private void OnMouseOver()
    {
        if (_hoverSprite != null)
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
