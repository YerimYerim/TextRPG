using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Unity.Plastic.Newtonsoft.Json;
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
            var dataSet = reader.AsDataSet(
                new ExcelDataSetConfiguration()
                {
                    UseColumnDataType = true,
                });
            DataTable table = dataSet.Tables[0];

            var data = new List<Dictionary<string, object>>();


            for (int i = 0; i < table.Rows.Count; ++i)
            {
                var rowData = new Dictionary<string, object>();
                if (i >= 3)
                {
                    for (var j = 0; j < table.Rows[i].ItemArray.Length; j++)
                    {
                        var item = table.Rows[i].ItemArray[j];
                        if (item != null)
                        {
                            if (table.Rows[0].ItemArray[j] != null &&
                                !string.IsNullOrWhiteSpace(table.Rows[0].ItemArray[j].ToString()))
                            {
                                if (item.GetType().Name.Equals("Double"))
                                {
                                    int intValue = (int) Math.Floor((double) item);
                                    rowData.Add(table.Rows[0].ItemArray[j].ToString(), intValue);
                                }
                                else if (table.Rows[2].ItemArray[j].ToString().Contains("[]"))
                                {
                                    rowData.Add(table.Rows[0].ItemArray[j].ToString(), "[" + item + "]");
                                }
                                else
                                {
                                    rowData.Add(table.Rows[0].ItemArray[j].ToString(), item);
                                }
                            }
                        }
                    }
                    data.Add(rowData);
                }
                string json = JsonConvert.SerializeObject(data, new JsonSerializerSettings()
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    Formatting = Formatting.Indented,
                });
                
                json = json.Replace("\"[", "[");
                json = json.Replace("]\"", "]");
                // JSON 데이터를 파일에 쓰기
                string Path = "Assets/Resource/ExcelData/test.json";
                File.WriteAllText(Path, json);
            }
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

            Debug.Log("YAML data saved to: " + " Assets/Resource/ExcelData");
        }
    }
}