using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class MicroCrewSDK_EditorWindow : EditorWindow {

    private static string newPackageName = "TemplatePackage";

    [MenuItem("MicroCrewSDK/SDK Window")]
    public static void ShowWindow() {
        EditorWindow.GetWindow(typeof(MicroCrewSDK_EditorWindow));
    }

    private void OnGUI() {
        newPackageName = EditorGUILayout.TextField("New Package Name", newPackageName);

        if(GUILayout.Button("Create")) {
            CreateNewPackage(newPackageName);
        }
    }

    private void CreateNewPackage(string packageName) {
        string firstName = "MicroCrew";
        string secondName = packageName;
        string dotName = firstName + "." + secondName;
        string underlyName = firstName + "_" + secondName;
        string comName = "com." + dotName.ToLower();

        string assetsPath = Application.dataPath;
        string packagePath = assetsPath + "/" + underlyName;

        Directory.CreateDirectory(packagePath);
        Directory.CreateDirectory(packagePath + "/Runtime");
        Directory.CreateDirectory(packagePath + "/Runtime/Prefabs");
        Directory.CreateDirectory(packagePath + "/Runtime/Scripts");

        Directory.CreateDirectory(packagePath + "/Editor");
        Directory.CreateDirectory(packagePath + "/Samples");
        Directory.CreateDirectory(packagePath + "/Samples/Sample_0");

        PackageJson packageJson = new PackageJson() {
            name = comName,
            displayName = underlyName,
            author = new PackageJson.Author(),
            samples = new PackageJson.Sample[1] {
                new PackageJson.Sample()
            }
        };

        CreateOrOverwriteJsonFile(packagePath + "\\package.json", packageJson);

        AssemblyDef runtimeAsmdf = new AssemblyDef() { name = dotName };
        AssemblyDef editorAsmdf = new AssemblyDef() { name = dotName + ".Editor" };

        CreateOrOverwriteJsonFile(packagePath + "\\Runtime\\" + runtimeAsmdf.name + ".asmdef", runtimeAsmdf);
        CreateOrOverwriteJsonFile(packagePath + "\\Editor\\" + editorAsmdf.name + ".asmdef", editorAsmdf);

        Debug.Log("Created package: " + dotName);
    }

    private void CreateOrOverwriteJsonFile(string path, object objToSerialize) {
        path = path.Replace('/', '\\');

        string jsonStr = JsonUtility.ToJson(objToSerialize, true);
        File.WriteAllText(path, jsonStr);
    }

    [System.Serializable]
    public class AssemblyDef {
        public string name;
    }

    [System.Serializable]
    public class PackageJson {
        public string name;
        public string version = "0.0.1";
        public string displayName;
        public string description = "New package";
        public string unity = "2019.1";
        public string unityRelease = "0b5";
        public Author author;
        public Sample[] samples;

        [System.Serializable]
        public class Author {
            public string name = "beatenpixel";
            public string email = "btn.pixel@gmail.com";
            public string url = "https://microcrew.com";
        }

        [System.Serializable]
        public class Sample {
            public string displayName = "Sample_0";
            public string description = "Sample_0 description";
            public string path = "Samples/Sample_0";
        }

    }    

}
