using System.Collections.Generic;
using UnityEngine;

namespace ReadExcel
{
    public class AbstractReadExcel_SO<T> : ScriptableObject
    {
        public List<T> dataList = new List<T>();

        public void Init(List<T> list)
        {
            Clear();
            foreach (var data in list)
            {
                dataList.Add(data);
            }
        }

        public void Clear()
        {
            dataList.Clear();
        }
    }
}