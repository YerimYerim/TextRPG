using System;
using System.Collections.Generic;
using System.IO;
using Codice.CM.Client.Differences;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class ExcelParserEditor : EditorWindow
    {
        static List<string> _filePaths = new List<string>();
        private Vector2 _scrollPosition = Vector2.zero;
        private int _scrollItemIndex = 0;
        [MenuItem("DTTool/Excel2json %;")]
        static void Open()
        {
            var window = GetWindow<ExcelParserEditor>();
            window.titleContent.text = "excel2json";
            window.titleContent.tooltip = @"excel 파일로 json 만들어줌 by 예림";
            _filePaths.Clear();
            _filePaths = GetfileName();
        }
        private void OnGUI()
        {
            GUILayout.Label("Assets/Resource/ExcelData/ 에 파일이 있어야 합니다!\n 바로 추출되지 않을때 ctrl + r 눌러주세요" );
            GUILayout.Space(10f);
            if (GUILayout.Button("모든 excel file가져오기"))
            {
                _filePaths.Clear();
                _filePaths = GetfileName();
            }
            
            _scrollPosition = GUILayout.BeginScrollView(_scrollPosition);
            _scrollItemIndex = GUILayout.SelectionGrid(_scrollItemIndex, _filePaths.ToArray(), 1);
            GUILayout.EndScrollView();
            
            
            GUILayout.BeginHorizontal();
            var fileName = GUILayout.TextField(_filePaths?[_scrollItemIndex]?? String.Empty);
            if (GUILayout.Button("json으로 바꾸기") == true)
            {
                string path = $"Assets/Resource/ExcelData/{fileName}.xlsx";
                
                DynamicClassGenerator.ConvertExcelToJson(path, fileName);
                Debug.Log($"{fileName} json 으로 변경 되었습니다.");
            };
            GUILayout.EndHorizontal();;
        }

        private static  List<string> GetfileName()
        {
            string assetPath = "Assets";
            
            string[] xlsxFiles = Directory.GetFiles(assetPath, "*.xlsx", SearchOption.AllDirectories);
                
            for (int i = 0; i < xlsxFiles.Length; ++i)
            {
                var path = Path.GetFileNameWithoutExtension(xlsxFiles[i]);;
                _filePaths.Add(path);
            }

            return _filePaths;
        }
    }
}
