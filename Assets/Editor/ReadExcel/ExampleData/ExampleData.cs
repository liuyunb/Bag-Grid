using System;
using System.Collections.Generic;
using UnityEngine;

namespace ReadExcel
{
    public class ExampleData : AbstractReadExcel_SO<ExampleExcelData>
    {
        
    }
    [Serializable]
    public class ExampleExcelData
    {
        public string id;
    }
}