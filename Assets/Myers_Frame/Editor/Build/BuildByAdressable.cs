using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
public class BuildByAdressable {

    static void PerExp() {
        //clean 当前所有组
        AddressableAssetSettings.CleanPlayerContent(AddressableAssetSettingsDefaultObject.Settings.ActivePlayerDataBuilder);
        //打包addressable 所有Group
        AddressableAssetSettings.BuildPlayerContent();
    }

    #region 绑定在build界面的会调中
    [InitializeOnLoadMethod]
    private static void Initialize() {
        BuildPlayerWindow.RegisterBuildPlayerHandler(BuildPlayerHandler);
    }
    private static void BuildPlayerHandler(BuildPlayerOptions options) {
        if (EditorUtility.DisplayDialog("Build with Addressables",
                "Do you want to build a clean addressables before export?",
                "Build with Addressables", "Skip")) {
            PerExp();
        }
        BuildPlayerWindow.DefaultBuildMethods.BuildPlayer(options);
    }
    #endregion
}