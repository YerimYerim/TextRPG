

using Unity.VisualScripting.YamlDotNet.Serialization;
using UnityEngine;

namespace Editor
{
    using System.IO;
    using ExcelDataReader;
    public static class DynamicClassGenerator
    {
        public static void ReadExcelFile(string filePath)
        {
            using var stream = File.Open(filePath, FileMode.Open, FileAccess.Read);
            var reader = ExcelReaderFactory.CreateReader(stream);
            var dataSet = reader.AsDataSet();
            // 워크시트
            var dataTable = dataSet.Tables[0];
            int rowNumber = 2;    
            int columnNumber = 2; 
            var cellValue = dataTable.Rows[rowNumber - 1][columnNumber - 1];
            Debug.Log(($"max Row {dataTable.Rows.Count}, max columns {dataTable.Columns.Count},Cell at row {rowNumber}, column {columnNumber}: {cellValue}"));
        }

        public static void ConvertingExcelToYaml(string excelFilePath)
        {
            using var stream = File.Open(excelFilePath, FileMode.Open, FileAccess.Read);
            using var reader = ExcelReaderFactory.CreateReader(stream);
            var dataSet = reader.AsDataSet(new ExcelDataSetConfiguration
            {
                ConfigureDataTable = _ => new ExcelDataTableConfiguration
                {
                    UseHeaderRow = true // 엑셀 파일의 첫 번째 행을 열 헤더로 사용
                }
            });
            var dataTable = dataSet.Tables[0]; // 첫 번째 워크시트

            // DataTable을 YAML 문자열로 변환
            var serializer = new SerializerBuilder().Build();
            string yamlData = serializer.Serialize(dataTable);

            // YAML 데이터를 파일에 저장
            File.WriteAllText("Assets/Resource/ExcelData", yamlData);

            Debug.Log("YAML data saved to: " +" Assets/Resource/ExcelData");
        }
    }
}