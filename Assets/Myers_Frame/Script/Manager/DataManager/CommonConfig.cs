using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class CommonConfig {
    public Dictionary<string, List<string>> _data = new Dictionary<string, List<string>>();

    public void Load(string name, Action<Dictionary<string, List<string>>> call = null)
    {
        var handle = Addressables.LoadAssetsAsync<TextAsset>(name, (text) =>
        {
            this.TextHandle(text.text);
            call?.Invoke(this._data);
        });
    }

    void TextHandle(string playerJsonStr) {
        Regex rgx = new Regex(@"(?i)(?<=\[)(.*)(?=\])");
        string res = rgx.Match(playerJsonStr).Value;
        rgx = new Regex(@"(?<=\{)(.*?)(?=(\}\,)|(\}$))");
        var list = rgx.Matches(res);
        Regex strRgx = new Regex("(?<=\\:\")(.*?)(?=\")");
        for (int i = 0; i < list.Count; i++) {
            int index = 0;
            var strList = strRgx.Matches(list[i].Value);
            var needStr = Regex.Replace(list[i].Value, "(?<=\\:\")(.*?)(?=\")", "ssstr");
            needStr = needStr.Replace("\"", "");
            var newList = needStr.Split(',');
            for (int j = 0; j < newList.Length; j++) {
                var value = newList[j].Split(':');
                string needValue = value[1];
                if (value[1] == "ssstr") {
                    needValue = strList[index].Value;
                    index++;
                }
                if (this._data.ContainsKey(value[0])) {
                    this._data[value[0]].Add(needValue);
                } else {
                    List<string> valueList = new List<string>();
                    valueList.Add(needValue);
                    this._data.Add(value[0], valueList);
                }
            }
        }
    }
}