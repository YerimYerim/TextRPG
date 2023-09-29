using System;
using System.Collections.Generic;
using System.Data;
using Unity.Plastic.Newtonsoft.Json;

namespace Editor
{
    using System.IO;
    using ExcelDataReader;
    public static class DynamicClassGenerator
    {
        public static void ConvertExcelToJson(string filePath, string fileName)
        {
            using var stream = File.Open(filePath, FileMode.Open, FileAccess.Read);
            var reader = ExcelReaderFactory.CreateReader(stream);
            var dataSet = reader.AsDataSet(
                new ExcelDataSetConfiguration()
                {
                    UseColumnDataType = true,
                });
            var data = new List<Dictionary<string, object>>();
            for (int k = 0; k < dataSet.Tables.Count; ++k)
            {
                DataTable table = dataSet.Tables[k];

                if (table.TableName.StartsWith("#") == false)
                {
                    for (int i = 0; i < table.Rows.Count; ++i)
                    {
                        var rowData = new Dictionary<string, object>();
                        if (i >= 3)
                        {
                            var isARowHasComment = table.Rows[i].ItemArray[0].ToString().StartsWith("#");
                            if (isARowHasComment)
                            {
                                continue;
                            }

                            for (var j = 0; j < table.Rows[i].ItemArray.Length; j++)
                            {
                                var item = table.Rows[i].ItemArray[j];
                                if (item != null)
                                {
                                    if (table.Rows[0].ItemArray[j] != null &&
                                        !string.IsNullOrWhiteSpace(table.Rows[0].ItemArray[j].ToString()))
                                    {

                                        if (table.Rows[2].ItemArray[j].ToString().Contains("[]"))
                                        {
                                            rowData.Add(table.Rows[0].ItemArray[j].ToString(), "[" + item + "]");
                                            continue;
                                        }

                                        if (item.GetType().Name.Equals("Double"))
                                        {
                                            int intValue = (int) Math.Floor((double) item);
                                            rowData.Add(table.Rows[0].ItemArray[j].ToString(), intValue);
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

                    }
                }

                string json = JsonConvert.SerializeObject(data, new JsonSerializerSettings()
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    Formatting = Formatting.Indented,
                });
                
                json = json.Replace("\"[", "[");
                json = json.Replace("]\"", "]");
                
                // JSON 데이터를 파일에 쓰기
                string path =  $"Assets/Resource/Json/{fileName}.json";
                File.WriteAllText(path, json);
            }
        }
    }
}