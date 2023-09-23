using System;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class ExcelParserEditor : EditorWindow
    {
        [MenuItem("DTTool/Excel2Yaml %;")]
        static void Open()
        {
            var window = GetWindow<ExcelParserEditor>();
            window.titleContent.text = "excel2Yaml";
            window.titleContent.tooltip = @"excel 파일로 c# class 및 yaml 만들어줌 by 예림";
        }

        private void OnGUI()
        {
            GUILayout.TextField("Excel 경로를 입력해주세요");
            
            if (GUILayout.Button("C# Class 로 바꾸기"))
            {
                DynamicClassGenerator.ReadExcelFile("Assets/Resource/ExcelData/Team_newbie_sample.xlsx");
                Debug.Log("class 생성 되었습니다?");
            };            
            
            GUILayout.TextField("yaml 를 저장할 경로를 입력해주세요");
            
            if (GUILayout.Button("yaml 로 파일 바꾸기"))
            {
                DynamicClassGenerator.ConvertingExcelToYaml("Assets/Resource/ExcelData/Team_newbie_sample.xlsx");
                Debug.Log("yaml생성 되었습니다");
            };
        }

        private void ConvertingExcel()
        {
            
        }
    }
}
