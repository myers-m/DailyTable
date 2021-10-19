using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text.RegularExpressions;
using OfficeOpenXml;
using SimpleJson;
using UnityEditor;
using UnityEngine;

namespace CommonWorkFlow.ToolEditor {
	public class ConverProcessor {
		public const string PostprocessorVersion = "1.0.0";
		public const string ExcelFileRoot = "Assets/Excel";
		public const string JsonFileRoot = "Assets/Json";
		static string corepath = Application.dataPath + "/{0}.xlsx";
		static string[] JsonKeyIgonArray = new string[1] { "Group" };
		/// <summary>
		/// 文件是否存在
		/// </summary>
		/// <param name="excelPath"></param>
		/// <returns></returns>
		public static bool FileExists (string excelPath) {
			string path = string.Format (corepath, excelPath);

			// Debug.LogError ("path " + path);
			return File.Exists (path);
		}

		/// <summary>
		/// 读取 Excel ;
		/// </summary>
		/// <param name="excelPath"></param>
		/// <returns></returns>
		public static Excel ReadExcel (string excelPath) {
			// string path = string.Format (corepath, excelPath);
			FileStream stream = File.Open (excelPath, FileMode.Open, FileAccess.Read, FileShare.Read);
			ExcelPackage ep = new ExcelPackage (stream);
			Excel excels = new Excel (ep.Workbook);
			return excels;
		}
		/// <summary>
		/// 转换excel out 出jsonobject 或者 Jsonarray
		/// </summary>
		/// <param name="input"></param>
		/// <param name="jobjRes"></param>
		/// <param name="jarrRes"></param>
		public static void ConverToJsonProcess (Excel input, out JsonObject jobjRes, out JsonArray jarrRes) {
			JsonObject jresObj = new JsonObject ();
			JsonArray jresArray = null;
			//rows 行
			bool isRepeat = false;
			for (int i = 0; i < input.Tables.Count; i++) {
				var allTables = input.Tables[i];

				//行数,列数
				int rowsCount, columnsCount;
				rowsCount = allTables.Dimension.Rows;
				columnsCount = allTables.Dimension.Columns;
				string jsonStrID = "";
				string sheetName = input.Tables[i].ToString ();

				//组建key
				List<string> jsonHeaderKey = new List<string> ();
				for (int k = 0; k < columnsCount; k++) {
					for (int u = 0; u < JsonKeyIgonArray.Length; u++) {
						var igonKey = JsonKeyIgonArray[u];

						if (!allTables.Cells[1, k + 1].Text.ToString ().Equals (igonKey)) {

							jsonHeaderKey.Add (allTables.Cells[1, k + 1].Text.ToString ());
							break;
						}
					}
				}
				JsonArray jarr = new JsonArray ();
				for (int j = 1; j < rowsCount; j++) {
					jsonStrID = allTables.Cells[j, 1].Text.ToString ();
					JsonObject jobj = new JsonObject ();
					for (int t = 0; t < jsonHeaderKey.Count; t++) {
                        // Debug.LogError (allTables.Cells[j + 1, t + 1].Value);
                        var tempStr = allTables.Cells[j + 1, t + 1].Text;
						string resstr = "";
						if (tempStr != null) {
							resstr = tempStr.ToString ();
						}

						if (resstr.Contains ("\\n")) {
							resstr = resstr.Replace ("\\n", "\n");
						}
						string pattern = @"^-?\d+$"; //整数
						if (Regex.IsMatch (resstr, pattern)) {
							//整数
							if (!jobj.ContainsKey (jsonHeaderKey[t])) {
								jobj.Add (jsonHeaderKey[t], long.Parse (resstr));
								continue;
							} else {
								Debug.LogError ("jsonHeaderKey[t] " + jsonHeaderKey[t]);
								isRepeat = true;
							}
						}
						pattern = @"^(-?\d+)(\.\d+)?$"; //浮点数
						if (Regex.IsMatch (resstr, pattern)) {
							//浮点数
							if (!jobj.ContainsKey (jsonHeaderKey[t])) {
								jobj.Add (jsonHeaderKey[t], float.Parse (resstr));
								continue;
							} else {
								Debug.LogError ("jsonHeaderKey[t] " + jsonHeaderKey[t]);
								isRepeat = true;
							}
						}

						//其他都为字符串
						if (!jobj.ContainsKey (jsonHeaderKey[t])) {
							jobj.Add (jsonHeaderKey[t], resstr);
						} else {
							Debug.LogError ("jsonHeaderKey[t] " + jsonHeaderKey[t]);
							isRepeat = true;
						}

					}
					jarr.Add (jobj);
					jresArray = jarr;
				}
				jresObj.Add (sheetName, jarr);
				// Debug.LogError ("jresObj " + jresObj);
			}

			if (isRepeat) {
				EditorUtility.DisplayDialog ("转化出错了", "有key重复了 检查表头是否重复key  keyname ", "OK");
			}

			if (input.Tables.Count > 1) {
				jobjRes = jresObj;
				jarrRes = null;
			} else {
				jobjRes = null;
				jarrRes = jresArray;
			}
		}
		public static void ConverToJsonProcess_NoSheet (Excel input, out JsonObject jobjRes, out JsonArray jarrRes) {
			JsonArray jresArray = new JsonArray ();
			//rows 行
			bool isRepeat = false;
			for (int i = 0; i < input.Tables.Count; i++) {
				var allTables = input.Tables[i];

				//行数,列数
				int rowsCount, columnsCount;
				rowsCount = allTables.Dimension.Rows;
				columnsCount = allTables.Dimension.Columns;;
				string jsonStrID = "";
				string sheetName = input.Tables[i].ToString ();

				//组建key
				List<string> jsonHeaderKey = new List<string> ();
				for (int k = 0; k < columnsCount; k++) {
					for (int u = 0; u < JsonKeyIgonArray.Length; u++) {
						var igonKey = JsonKeyIgonArray[u];

						if (!allTables.Cells[1, k + 1].Text.ToString ().Equals (igonKey)) {

							jsonHeaderKey.Add (allTables.Cells[1, k + 1].Text.ToString ());
							break;
						}
					}
				}
				JsonArray jarr = new JsonArray ();
				for (int j = 1; j < rowsCount; j++) {
					jsonStrID = allTables.Cells[j, 1].Text.ToString ();
					JsonObject jobj = new JsonObject ();
					for (int t = 0; t < jsonHeaderKey.Count; t++) {
						// Debug.LogError (allTables.Cells[j + 1, t + 1].Value);
						var tempStr = allTables.Cells[j + 1, t + 1].Text;
						string resstr = "";
						if (tempStr != null) {
							resstr = tempStr.ToString ();
						}

						if (resstr.Length == 0) continue;

						if (resstr.Contains ("\\n")) {
							resstr = resstr.Replace ("\\n", "\n");
						}
						string pattern = @"^-?\d+$"; //整数
						if (Regex.IsMatch (resstr, pattern)) {
							//整数
							if (!jobj.ContainsKey (jsonHeaderKey[t])) {
								jobj.Add (jsonHeaderKey[t], long.Parse (resstr));
								continue;
							} else {
								Debug.LogError ("jsonHeaderKey[t] " + jsonHeaderKey[t]);
								isRepeat = true;
							}
						}
						pattern = @"^(-?\d+)(\.\d+)?$"; //浮点数
						if (Regex.IsMatch (resstr, pattern)) {
							//浮点数
							if (!jobj.ContainsKey (jsonHeaderKey[t])) {
								jobj.Add (jsonHeaderKey[t], float.Parse (resstr));
								continue;
							} else {
								Debug.LogError ("jsonHeaderKey[t] " + jsonHeaderKey[t]);
								isRepeat = true;
							}
						}

						//其他都为字符串
						if (!jobj.ContainsKey (jsonHeaderKey[t])) {
							jobj.Add (jsonHeaderKey[t], resstr);
						} else {
							Debug.LogError ("jsonHeaderKey[t] " + jsonHeaderKey[t]);
							isRepeat = true;
						}

					}

					if (jobj.Keys.Count != 0) {
						jresArray.Add (jobj);
					}
				}
			}

			if (isRepeat) {
				EditorUtility.DisplayDialog ("转化出错了", "有key重复了 检查表头是否重复key  keyname ", "OK");
			}

			jobjRes = null;
			jarrRes = jresArray;
		}
		public static void ConverToJsonProcess_LanguageDICSheet (Excel input, out JsonObject jobjRes, out JsonArray jarrRes) {
			JsonObject jresObj = new JsonObject ();
			JsonObject jresArrayObj = null;
			JsonArray jresValueArray = null;
			JsonArray jresKeyArray = null;
			//rows 行
			bool isRepeat = false;
			for (int i = 0; i < input.Tables.Count; i++) {

				var allTables = input.Tables[i];

				//行数,列数
				int rowsCount, columnsCount;
				rowsCount = allTables.Dimension.Rows;
				columnsCount = allTables.Dimension.Columns;;
				string jsonStrID = "";
				string sheetName = input.Tables[i].ToString ();

				jresArrayObj = new JsonObject ();
				jresValueArray = new JsonArray ();
				jresKeyArray = new JsonArray ();

				//组建key
				List<string> jsonHeaderKey = new List<string> ();
				for (int k = 0; k < columnsCount; k++) {
					for (int u = 0; u < JsonKeyIgonArray.Length; u++) {
						var igonKey = JsonKeyIgonArray[u];

						if (!allTables.Cells[1, k + 1].Text.ToString ().Equals (igonKey)) {

							jsonHeaderKey.Add (allTables.Cells[1, k + 1].Text.ToString ());
							break;
						}
					}
				}
				JsonArray jarr = new JsonArray ();
				for (int j = 1; j < rowsCount; j++) {
					jsonStrID = allTables.Cells[j, 1].Text.ToString ();
					JsonObject jobj = new JsonObject ();
					for (int t = 0; t < jsonHeaderKey.Count; t++) {
						// Debug.LogError (allTables.Cells[j + 1, t + 1].Value);
						var tempStr = allTables.Cells[j + 1, t + 1].Text;
						string resstr = "";
						if (tempStr != null) {
							resstr = tempStr.ToString ();
						}
						

						if (t == 0) {
							jresKeyArray.Add (resstr);
						}

						if (jsonHeaderKey[t].Equals ("ID")) continue;

						if (resstr.Contains ("\\n")) {
							resstr = resstr.Replace ("\\n", "\n");
						}
						string pattern = @"^-?\d+$"; //整数
						if (Regex.IsMatch (resstr, pattern)) {
							//整数
							if (!jobj.ContainsKey (jsonHeaderKey[t])) {
								jobj.Add (jsonHeaderKey[t], long.Parse (resstr));
								continue;
							} else {
								Debug.LogError ("jsonHeaderKey[t] " + jsonHeaderKey[t]);
								isRepeat = true;
							}
						}
						pattern = @"^(-?\d+)(\.\d+)?$"; //浮点数
						if (Regex.IsMatch (resstr, pattern)) {
							//浮点数
							if (!jobj.ContainsKey (jsonHeaderKey[t])) {
								jobj.Add (jsonHeaderKey[t], float.Parse (resstr));
								continue;
							} else {
								Debug.LogError ("jsonHeaderKey[t] " + jsonHeaderKey[t]);
								isRepeat = true;
							}
						}

						//其他都为字符串
						if (!jobj.ContainsKey (jsonHeaderKey[t])) {
							jobj.Add (jsonHeaderKey[t], resstr);
						} else {
							Debug.LogError ("jsonHeaderKey[t] " + jsonHeaderKey[t]);
							isRepeat = true;
						}

					}
					jresValueArray.Add (jobj);
				}
				jresArrayObj.Add ("keys", jresKeyArray);
				jresArrayObj.Add ("values", jresValueArray);

				jresObj.Add (sheetName, jresArrayObj);
			}

			if (isRepeat) {
				EditorUtility.DisplayDialog ("转化出错了", "有key重复了 检查表头是否重复key  keyname ", "OK");
			}

			// if (input.Tables.Count > 1) {
			jobjRes = jresObj;
			jarrRes = null;
			// } else {
			// 	jobjRes = null;
			// 	jarrRes = jresValueArray;
			// }
		}

		//忽略文件
		// ProductionNewMathf 包含 version

		//常用json 嵌套关系
		//type01 {"name":[{"key":"value"}]]  

		//AllPropsConfigs
		//Language_Milestone
		//Language_Unit
		//LanguageCounselorCostData
		//LoadScreenLanguage

		//type02 [{"key":"value"}]  大部分文件是这种

		//type03 [ [{"key":"value"}] ]  CounselorCostData

		/// <summary>
		/// 写入Excel
		/// </summary>

		// public static string[] 
		public static void WriteExcel (string excelPath, string fileName, JsonObject Jobj) {
			//自定义excel的路径
			string tempPath = excelPath;
			FileInfo newFile = new FileInfo (tempPath);
			if (newFile.Exists) {
				//创建一个新的excel文件
				newFile.Delete ();
				newFile = new FileInfo (tempPath);
			}

			ProcessSingleJobj (newFile, fileName, Jobj);

			Debug.Log ("--------------- 成功转换为excel ------------------  " + excelPath);
		}

		/// <summary>
		/// 写入excel
		/// </summary>
		/// <param name="excelPath"></param>
		/// <param name="fileName"></param>
		/// <param name="Jarr"></param>
		public static void WriteExcel (string excelPath, string fileName, JsonArray Jarr) {
			//通过面板设置excel路径
			//string outputDir = EditorUtility.SaveFilePanel("Save Excel", "", "New Resource", "xlsx");

			//自定义excel的路径
			string tempPath = excelPath;
			FileInfo newFile = new FileInfo (tempPath);
			if (newFile.Exists) {
				//创建一个新的excel文件
				newFile.Delete ();
				newFile = new FileInfo (tempPath);
			}

			ProcessSingleJarr (newFile, fileName, Jarr);

			Debug.Log ("--------------- 成功转换为excel ------------------  " + excelPath);

		}
		//json 2 excel
		static void ExcuteConver (ExcelWorksheet worksheet, JsonArray Jarr) {
			//get json head key
			JsonObject jobj = Jarr[0] as JsonObject;
			string[] headkey = new string[jobj.Keys.Count];
			jobj.Keys.CopyTo (headkey, 0);
			//Create Head
			for (int i = 0; i < headkey.Length; i++) {
				worksheet.Cells[1, i + 1].Value = headkey[i];
			}

			for (int i = 0; i < Jarr.Count; i++) {
				JsonObject jtemp = Jarr[i] as JsonObject;
				for (int j = 0; j < headkey.Length; j++) {
					var tvalue = "";
					if (jtemp.ContainsKey (headkey[j])) {
						object outObj;
						if (jtemp.TryGetValue (headkey[j], out outObj)) {
							tvalue = outObj.ToString ();
						}
					}
					worksheet.Cells[1 + i + 1, j + 1].Value = tvalue;
				}
			}
		}
		//  [{"key":"value"}] 
		static void ProcessSingleJarr (FileInfo newFile, string fileName, JsonArray Jarr) {

			//通过ExcelPackage打开文件
			string sheetName = fileName; //文件名等于 sheet名

			using (ExcelPackage package = new ExcelPackage (newFile)) {
				//在excel空文件添加新sheet
				ExcelWorksheet worksheet = package.Workbook.Worksheets.Add (sheetName);

				ExcuteConver (worksheet, Jarr);

				//保存excel
				package.Save ();
			}
		}

		static void ProcessSingleJobj (FileInfo newFile, string fileName, JsonObject _Jobj) {
			//通过ExcelPackage打开文件

			using (ExcelPackage package = new ExcelPackage (newFile)) {

				//get json head sheetName

				string[] sheetName = new string[_Jobj.Keys.Count];
				_Jobj.Keys.CopyTo (sheetName, 0);

				for (int shIndex = 0; shIndex < sheetName.Length; shIndex++) {
					//在excel空文件添加新sheet
					ExcelWorksheet worksheet = package.Workbook.Worksheets.Add (sheetName[shIndex]);

					if (_Jobj.ContainsKey (sheetName[shIndex])) {
						JsonArray Jarr = _Jobj.GetJsonArray (sheetName[shIndex]);
						ExcuteConver (worksheet, Jarr);
					}
				}

				//保存excel
				package.Save ();
			}
		}
	}
}