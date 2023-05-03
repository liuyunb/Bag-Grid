using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;
using OfficeOpenXml;
using UnityEditor;
using UnityEngine;

namespace ReadExcel
{


    public abstract class ReadExcelWindow<T, T1> : EditorWindow where T : AbstractReadExcel_SO<T1> where T1 : class, new()
    {
        protected List<T1> dataList = new List<T1>();

        protected T tableData;

        protected string MyPath;

        protected bool IsLoad;

        protected bool IsCreate;

        protected abstract string FileName { get; }
        
        protected abstract string FileSavePath { get; }

        protected bool ReadExcel(out string newPath)
        {
            newPath = EditorUtility.OpenFilePanel("选择表格", "", "");

            if (newPath == "")
            {
                return false;
            }

            if (!(newPath.Contains(".xls") || newPath.Contains("xlsx")))
            {
                EditorUtility.DisplayDialog("表格读取失败", "不是正确的文件格式，请读取xls或者xlsx的格式的文件", "确认");
                return false;
            }

            return true;
        }

        protected virtual bool AnalyzeData(string path)
        {
            dataList.Clear();
            //读取路径
            FileInfo fileInfo = new FileInfo(path);
            
            using (ExcelPackage excelPackage = new ExcelPackage(fileInfo))
            {
                //对每一个工作区进行遍历
                foreach (ExcelWorksheet excelWorksheet in excelPackage.Workbook.Worksheets)
                {
                    if(excelWorksheet.Dimension == null)
                        continue;
                    for (int i = excelWorksheet.Dimension.Start.Row + 2; i <= excelWorksheet.Dimension.End.Row; i++)
                    {
                        //创建数据
                        T1 data = new T1();
                        //拿到数据类型
                        Type type = typeof(T1);
                        //开始遍历每一行的每一个元素
                        for (int j = excelWorksheet.Dimension.Start.Column;
                             j <= excelWorksheet.Dimension.End.Column;
                             j++)
                        {
                            Debug.Log("数据内容" + excelWorksheet.GetValue(i, j).ToString());
                            FieldInfo variable = type.GetField(excelWorksheet.GetValue(2, j).ToString());
                            string tableValue = excelWorksheet.GetValue(i, j).ToString();
                            variable.SetValue(data, Convert.ChangeType(tableValue, variable.FieldType));
                        }
                        //加入数据
                        //TODO：写一个抽象SO来承载加入数据函数
                        // dataInfo.AddData(data);
                        dataList.Add(data);
                    }
                }
            }

            

            return true;
        }

        protected virtual void UpdateData()
        {
            
            if (!IsCreate)
            {
                //转化成so
                T dataInfo = ScriptableObject.CreateInstance(typeof(T)) as T;
                dataInfo.Init(dataList);
                AssetDatabase.CreateAsset(dataInfo, FileSavePath);
                tableData = dataInfo;
                IsCreate = true;
            }
            else
            {
                tableData.Init(dataList);
                EditorUtility.SetDirty(tableData);
            }
            
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            EditorUtility.DisplayDialog("", "更新成功", "确认");
        }

        private void OnGUI()
        {
            bool selectExcelBtnClicked = GUILayout.Button("选择相应表格");
            GUILayout.Label($"当前表格路径为： {MyPath}");
            bool updateExcelBtnClicked = GUILayout.Button("更新表格数据到unity");

            if (selectExcelBtnClicked)
            {
                bool isSuccess = ReadExcel(out MyPath);
                bool readRes;
                if (!isSuccess)
                {
                    MyPath = "";
                    IsLoad = false;
                }
                else
                {
                    readRes = AnalyzeData(MyPath);
                    IsLoad = true;
                }
            }

            if (updateExcelBtnClicked)
            {
                if (!IsLoad)
                {
                    EditorUtility.DisplayDialog("", "当前表格数据为空", "确认");
                }
                else if (EditorUtility.DisplayDialog("更新" + FileName + "数据", "是否更新" + FileName + "数据", "是", "否"))
                {
                    UpdateData();
                }
            }
        }
    }
}
