using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum ItemDir
{
    Down, Right, Up, Left
}
public class DragableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Vector2 mySize;
    public InventoryItem item;
    public List<Slot> occupiedSlot;

    private Image _dragImg;
    private Image _sr;
    private ItemDir _curDir;

    private bool _startDrag;

    private void Awake()
    {
        _sr = GetComponent<Image>();
    }

    private void Start()
    {
        _dragImg = GameObject.FindWithTag("DragCanvas").GetComponent<Image>();
        // _dragImg.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (_startDrag && Input.GetKeyDown(KeyCode.E))
        {
            _dragImg.transform.Rotate(Vector3.forward, 90f);
            switch (_curDir)
            {
                case ItemDir.Up:
                    _curDir = ItemDir.Right;
                    break;
                case ItemDir.Right:
                    _curDir = ItemDir.Down;
                    break;
                case ItemDir.Down:
                    _curDir = ItemDir.Left;
                    break;
                case ItemDir.Left:
                    _curDir = ItemDir.Up;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _dragImg.enabled = true;
        _dragImg.transform.eulerAngles = new Vector3(0, 0, 90 * (int) _curDir);
        _startDrag = true;

        _dragImg.sprite = _sr.sprite;
        var dragTrans = (_dragImg.transform as RectTransform);
        var myTrans = (transform as RectTransform);
        dragTrans.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, myTrans.rect.width);
        dragTrans.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, myTrans.rect.height);
    }

    public void OnDrag(PointerEventData eventData)
    {
        _dragImg.transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _dragImg.transform.eulerAngles = Vector3.zero;
        _dragImg.enabled = false;

        _startDrag = false;

        var rayObj = eventData.pointerCurrentRaycast.gameObject;

        if (rayObj != null)
        {
            var slot = rayObj.GetComponent<Slot>();
            Debug.Log(slot);
            //如果单层射线被覆盖
            if (slot == null)
            {
                
            }
            Debug.Log(slot);
            if (slot != null)
            {
                if (slot.item == null || slot.item.itemId == 0)
                {
                    
                    DragableItem item = this;
                    bool success = GridManager.Instance.SetSlotItem(slot, mySize, _curDir, ref item);
                    if (success)
                    {
                        transform.eulerAngles = new Vector3(0, 0, 90 * (int) _curDir);
                        Debug.Log("Success");
                    }
                    else
                    {
                        Debug.Log("Lose");
                    }
                    
                }

                
            }
        }
    }
}
