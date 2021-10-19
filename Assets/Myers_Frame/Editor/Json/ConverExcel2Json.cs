using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Data;
using System.IO;
using System.Reflection;
using System.Text;
using SimpleJson;
using UnityEditor;

namespace CommonWorkFlow.ToolEditor {
    /// <summary>
    /// 配置表正向工具
    /// excel 转换为 json
    /// 拥有功能：
    /// 1.Excel 进入工程后能马上识别并转换 JsoConfig Sheet 标签下配置的路径中
    /// 2.导入项目后右键 能单选或多选目标excel文件
    /// 3.尝试构建选取中的文件
    /// 4.自动查找项目路径上的所有文件并尝试构建
    /// </summary>
    public class ConverExcel2Json : AssetPostprocessor {

        /// <summary>
        /// 文件忽略列表
        /// </summary>
        /// <value></value>
        public static string[] unlegalFileName = new string[] {
            "语言配置表_config",
        };
        /// <summary>
        /// 导入资源完成后 进度条完成之前调用的函数
        /// </summary>
        /// <param name="importedAssets">传入的都为文件路径</param>
        /// <param name="deletedAssets">传入的都为文件路径</param>
        /// <param name="movedAssets">传入的都为文件路径</param>
        /// <param name="movedFromAssetPaths">传入的都为文件路径</param>
        static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths) {
            foreach (string str in importedAssets) {
                // Debug.Log ("Reimported Asset: " + str);
                // DoImportExcel (str);
            }
        }

        public static bool IslegalExcel(string path) {
            return path.EndsWith(".xlsx", StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// 导入时处理
        /// </summary>
        /// <param name="excelPath"></param>
        public static void DoImportExcel(string excelPath) {
            if (!IslegalExcel(excelPath)) return;

            var res = ConverProcessor.ReadExcel(excelPath);
            JsonObject jobj = null;
            JsonArray jarr = null;
            if (excelPath.Contains("LanguageConfigs")) {
                // Debug.LogError ("此Excel判定为语言表类型");
                ConverProcessor.ConverToJsonProcess_NoSheet(res, out jobj, out jarr);
            } else {
                ConverProcessor.ConverToJsonProcess(res, out jobj, out jarr);
            }

            string JsonFileName = Path.GetFileNameWithoutExtension(excelPath);
            string excelRootPath = Path.GetDirectoryName(excelPath);
            string rootPath = ConverProcessor.JsonFileRoot;

            //处理当在window时 路径使用的时 \
            if (excelRootPath.Contains(@"\")) {
                excelRootPath = excelRootPath.Replace(@"\", "/");
            }

            string newDirectoryName = excelRootPath.Replace(ConverProcessor.ExcelFileRoot + "/", "");
            if (!excelRootPath.Equals(newDirectoryName)) {
                //存在目录结构变化
                rootPath += "/" + newDirectoryName;
                // Debug.LogError (newDirectoryName + " rootPath " + rootPath);
            }
            string jsonPath = string.Format("{0}/{1}.json", rootPath, JsonFileName);

            //存放excel的文件目录是否存在
            if (!Directory.Exists(rootPath)) {
                Directory.CreateDirectory(rootPath);
                //资源刷新
                AssetDatabase.Refresh();
            }

            if (jobj != null) {
                jobj = UnitJsonFile(jobj);
                writeJson(jsonPath, jobj.ToString());
            }
            if (jarr != null) {
                var temp = UnitJsonFile(jarr);
                writeJson(jsonPath, temp.ToString());
            }

            AssetDatabase.Refresh();
        }
        /// <summary>
        /// 统一json格式 用于序列化json物体
        /// </summary>
        public static JsonObject UnitJsonFile(JsonObject jobj) {
            JsonObject junit = new JsonObject();
            junit.Add("config", jobj);
            junit.Add("Type", "JsonObject");
            junit.Add("Version", ConverProcessor.PostprocessorVersion + "_" + DateTime.Now);
            return junit;
        }
        public static JsonObject UnitJsonFile(JsonArray jarr) {
            JsonObject junit = new JsonObject();
            junit.Add("config", jarr);
            junit.Add("Type", "JsonArray");
            junit.Add("Version", ConverProcessor.PostprocessorVersion + "_" + DateTime.Now);
            return junit;
        }
        public static void writeJson(string path, string content) {
            Debug.Log("path ---- 转换json成功----- " + path);
            File.WriteAllText(path, content, Encoding.UTF8);
        }

    }

    public class ConverExcelMenuTool {

        //检测函数
        [MenuItem("Assets/ConverExcel2Json/Conver Excel", true)]
        public static bool CheckRebuildExcel() {
            if (EditorApplication.isPlaying) return false;

            var selected = Selection.activeObject;
            if (selected == null) return false;

            return ConverExcel2Json.IslegalExcel(AssetDatabase.GetAssetPath(selected));

        }
        //构建选中的文件
        [MenuItem("Assets/ConverExcel2Json/Conver Excel")]
        public static void RebuildExcel() {
            var selected = Selection.activeObject;
            ConverExcel2Json.DoImportExcel(AssetDatabase.GetAssetPath(selected));
        }

        //该函数调用于 item同名函数之前 作为一个检测安全的函数
        [MenuItem("Assets/ConverExcel2Json/Conver All Excel", true)]
        public static bool CheckRebuildAllExcel() {
            return !EditorApplication.isPlaying;
        }

        [MenuItem("Assets/ConverExcel2Json/Clean All Json File")]
        public static void CleanAllJsonFile() {
            if (EditorUtility.DisplayDialog("CleanAllJsonFile", "是否删除全部输出的json文件 ", "OK", "Cnacel")) {
                string dataPath = Application.dataPath;
                int startPos = dataPath.Length - "Assets".Length;
                string[] files = Directory.GetFiles(Application.dataPath + ConverProcessor.ExcelFileRoot.Replace("Assets", ""), "*.xlsx", SearchOption.AllDirectories);
                for (int i = 0; i < files.Length; i++) {
                    Directory.Delete(files[i].Substring(startPos));
                }
                Debug.LogError("成功删除了 " + files.Length + "个文件");
                //资源刷新
                AssetDatabase.Refresh();
            }
        }
        //构建所有文件
        [MenuItem("Assets/ConverExcel2Json/Conver All Excel")]
        public static void RebuildAllExcel() {
            string dataPath = Application.dataPath;
            int startPos = dataPath.Length - "Assets".Length;
            string[] files = Directory.GetFiles(Application.dataPath + ConverProcessor.ExcelFileRoot.Replace("Assets", ""), "*.xlsx", SearchOption.AllDirectories);
            for (int i = 0; i < files.Length; i++) {
                ConverExcel2Json.DoImportExcel(files[i].Substring(startPos));
            }
            //资源刷新
            AssetDatabase.Refresh();
        }
    }
}