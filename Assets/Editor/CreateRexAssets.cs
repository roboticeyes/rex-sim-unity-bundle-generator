using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public class CreateRexAssets
{
    //DO NOT CHANGE ANY OF THESE
    private static string modelName = "SimulationModel";
    private static string assetBundleDirectoryPath = "Assets/AssetBundles";
    private static string outputDirectoryPath = "EXPORT";

    [MenuItem ("REX/Build REX Assets %&a")]
    static void BuildAllAssetBundles()
    {
        Debug.Log ("Building Bundles.");

        #region Preparation
        ClearConsole();
        if (Directory.Exists (outputDirectoryPath))
        {
            Directory.Delete (outputDirectoryPath, true);
        }
        #endregion

        #region Checking Project Configuration
        List<string> bundleFileNames = new List<string>();
        foreach (var assetBundleName in AssetDatabase.GetAllAssetBundleNames())
        {
            var pathsInAssetBundle = AssetDatabase.GetAssetPathsFromAssetBundle (assetBundleName);

            if (pathsInAssetBundle.Length == 0)
            {
                continue;
            }
            else if (pathsInAssetBundle.Length > 1)
            {
                Debug.LogError ("Asset Bundle \"" + assetBundleName + "\" contains more than one model. " +
                                "Please make sure to have exactly one model per Asset Bundle");
                return;
            }

            foreach (var assetPathAndName in pathsInAssetBundle)
            {
                string nameWithoutPath = assetPathAndName.Substring (assetPathAndName.LastIndexOf ("/") + 1);
                string name = nameWithoutPath.Substring (0, nameWithoutPath.LastIndexOf ("."));
                if (name.Equals (modelName) == false)
                {
                    Debug.LogError ("Model name has to be \"" + modelName + "\"");
                    return;
                }
            }
            bundleFileNames.Add (assetBundleName);
            Debug.Log ("Successfully checked Bundle \"" + assetBundleName + "\".");
        }
        #endregion

        #region Build Bundles
        if (!Directory.Exists (assetBundleDirectoryPath))
        {
            Directory.CreateDirectory (assetBundleDirectoryPath);
        }

        BuildPipeline.BuildAssetBundles (assetBundleDirectoryPath, BuildAssetBundleOptions.None, BuildTarget.WSAPlayer);

        ExtractBundles (bundleFileNames);
        #endregion

        #region Cleanup
        if (Directory.Exists (assetBundleDirectoryPath))
        {
            FileUtil.DeleteFileOrDirectory (assetBundleDirectoryPath);
        }
        EditorApplication.RepaintHierarchyWindow();
        #endregion

        System.Diagnostics.Process.Start (Path.Combine (Application.dataPath, "../EXPORT"));
        Debug.Log ("Done.");
    }

    #region Helpers
    private static void ExtractBundles (List<string> fileNames)
    {
        Debug.Log ("Extracting REX Assets.");
        if (!Directory.Exists (outputDirectoryPath))
        {
            Directory.CreateDirectory (outputDirectoryPath);
        }

        foreach (var fileName in fileNames)
        {
            string bundlePath = Path.Combine (assetBundleDirectoryPath, fileName);

            File.Move (Path.Combine (assetBundleDirectoryPath, fileName), Path.Combine (outputDirectoryPath, fileName + ".rexasset"));
        }
    }
    private static void ClearConsole()
    {
        var assembly = Assembly.GetAssembly (typeof (SceneView));
        var type = assembly.GetType ("UnityEditor.LogEntries");
        var method = type.GetMethod ("Clear");
        method.Invoke (new object(), null);
    }
    #endregion
}
