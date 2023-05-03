using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : Singleton<GridManager>
{
    public List<Slot> slotGrid = new List<Slot>();
    public int width = 6;
    public int height = 6;
    private void Start()
    {
        for (int i = 0; i < slotGrid.Count; i++)
        {
            slotGrid[i].index = i;
        }
    }

    public bool SetSlotItem(Slot startSlot, Vector2 size, ItemDir dir, ref DragableItem item)
    {
        List<Slot> selectSlot = new List<Slot>();
        int startIndex = startSlot.index;
        //拿到所有的slot
        for (int i = 0; i <= size.x; i++)
        {
            for (int j = 0; j <= size.y; j++)
            {
                var newSize = ConvertSizeByDir(new Vector2Int(i, j), dir);
                int newX = (startIndex % width) + newSize.x;
                int newY = (startIndex / width) + newSize.y;
                if (newX >= 0 && newY >= 0 && newX < width && newY < height)
                {
                    int newIndex = newX + newY * width;
                    selectSlot.Add(slotGrid[newIndex]);
                }
                else
                {
                    return false;
                }
            }
        }
        var centerPos = Vector3.zero;
        
        foreach (var slot in selectSlot)
        {
            if (slot.item == null || slot.item.itemId == 0 || item.occupiedSlot.Contains(slot))
            {
                centerPos += slot.transform.position;
            }
            else
            {
                return false;
            }
        }

        item.transform.position = centerPos / selectSlot.Count;

        foreach (var slot in item.occupiedSlot)
        {
            slot.item = null;
        }
        
        item.occupiedSlot.Clear();
        
        foreach (var slot in selectSlot)
        {
            item.occupiedSlot.Add(slot);
            slot.item = item.item;
        }

        return true;
    }

    public Vector2Int ConvertSizeByDir(Vector2Int size, ItemDir dir)
    {
        return dir switch
        {
            ItemDir.Down => size,
            ItemDir.Left => new Vector2Int(size.y, size.x),
            ItemDir.Up => new Vector2Int(size.x, -size.y),
            ItemDir.Right => new Vector2Int(-size.x, size.y)
        };
    }
}
