using UnityEditor;

namespace ReadExcel
{
    public class ReadExample : ReadExcelWindow<ExampleData, ExampleExcelData>
    {
        protected override string FileName => "测试";
        protected override string FileSavePath => "Assets/Editor/Resources/" + FileName + ".asset";
        
        [MenuItem("CustomUtility/ReadExcel/ReadExample")]
        public static void ShowWindow()
        {
            GetWindow<ReadExample>();
        }


    }
}