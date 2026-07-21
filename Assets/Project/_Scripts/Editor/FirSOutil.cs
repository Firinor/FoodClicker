using Newtonsoft.Json;
using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;
using System.Linq;

public class FirSOutil : EditorWindow
{
    private ScriptableObject targetObject;
    private string outputText = "";
    private string inputJson = "";
    private Vector2 scrollPosition;
    private Vector2 inputScrollPosition;
    private Type selectedType;
    private List<Type> types;
    private string[] typeNames;
    private int selectedTypeIndex = 0;
    private string creationStatus = "";

    [MenuItem("Tools/FirTools/Scriptable Object Viewer")]
    public static void ShowWindow()
    {
        GetWindow<FirSOutil>("Fir SO util");
    }

    private void OnEnable()
    {
        RefreshTypeList();
    }

    private void RefreshTypeList()
    {
        var allTypes = TypeCache.GetTypesDerivedFrom<ScriptableObject>();
        types = new List<Type>();
        
        foreach (var type in allTypes)
        {
            if (IsUserType(type))
            {
                types.Add(type);
            }
        }
        
        typeNames = types.Select(t => t.Name).ToArray();
        
        if (typeNames.Length > 0 && types.Count > 0)
        {
            selectedType = types[0];
            selectedTypeIndex = 0;
        }
        else
        {
            typeNames = new string[] { "No user types found" };
            selectedType = null;
            selectedTypeIndex = 0;
        }
    }

    private bool IsUserType(Type type)
    {
        var assembly = type.Assembly;
        string assemblyName = assembly.GetName().Name;
        
        if (assemblyName.StartsWith("Unity") || 
            assemblyName.StartsWith("System") || 
            assemblyName.StartsWith("mscorlib") ||
            assemblyName.StartsWith("netstandard") ||
            assemblyName == "Assembly-CSharp-firstpass" ||
            assemblyName == "Assembly-CSharp-Editor" ||
            assemblyName == "Assembly-CSharp-Editor-firstpass")
        {
            return false;
        }
        
        if (type.IsAbstract)
        {
            return false;
        }
        
        if (!type.IsPublic || type.IsNested)
        {
            return false;
        }

        return true;
    }

    private void OnGUI()
    {
        GUILayout.Label("Scriptable Object Viewer & Creator", EditorStyles.boldLabel);
        
        EditorGUILayout.Space();
        
        GUILayout.Label("View Existing SO", EditorStyles.boldLabel);
        
        targetObject = (ScriptableObject)EditorGUILayout.ObjectField(
            "Target SO", 
            targetObject, 
            typeof(ScriptableObject), 
            false
        );

        if (GUILayout.Button("Show Data", GUILayout.Height(30)))
        {
            if (targetObject != null)
            {
                var settings = new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                };
                outputText = JsonConvert.SerializeObject(
                    targetObject, 
                    Formatting.Indented,
                    settings
                );
                Debug.Log(outputText);
            }
            else
            {
                outputText = "Please select a ScriptableObject!";
                Debug.LogWarning(outputText);
            }
        }

        EditorGUILayout.Space();
        
        GUILayout.Label("Create SO from JSON", EditorStyles.boldLabel);
        
        if (typeNames.Length > 0)
        {
            selectedTypeIndex = EditorGUILayout.Popup(
                "Select SO Type", 
                selectedTypeIndex, 
                typeNames
            );
            
            if (selectedTypeIndex >= 0 && selectedTypeIndex < typeNames.Length)
            {
                selectedType = types[selectedTypeIndex];
            }
        }
        else
        {
            EditorGUILayout.HelpBox(
                "No ScriptableObject types found in the project!", 
                MessageType.Warning
            );
        }

        EditorGUILayout.Space();
        
        GUILayout.Label("Input JSON:", EditorStyles.boldLabel);
        inputScrollPosition = EditorGUILayout.BeginScrollView(
            inputScrollPosition, 
            GUILayout.Height(150)
        );
        inputJson = EditorGUILayout.TextArea(
            inputJson, 
            GUILayout.ExpandHeight(true)
        );
        EditorGUILayout.EndScrollView();

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Create from JSON", GUILayout.Height(30)))
        {
            CreateSOFromJson();
        }
        
        if (GUILayout.Button("Clear JSON", GUILayout.Height(30)))
        {
            inputJson = "";
            creationStatus = "";
        }
        EditorGUILayout.EndHorizontal();

        if (!string.IsNullOrEmpty(creationStatus))
        {
            EditorGUILayout.HelpBox(creationStatus, MessageType.Info);
        }

        EditorGUILayout.Space();
        
        if (!string.IsNullOrEmpty(outputText))
        {
            GUILayout.Label("Output:", EditorStyles.boldLabel);
            scrollPosition = EditorGUILayout.BeginScrollView(
                scrollPosition, 
                GUILayout.Height(200)
            );
            EditorGUILayout.TextArea(outputText, GUILayout.ExpandHeight(true));
            EditorGUILayout.EndScrollView();

            if (GUILayout.Button("Copy to Clipboard"))
            {
                EditorGUIUtility.systemCopyBuffer = outputText;
                Debug.Log("Data copied to clipboard!");
            }
        }
    }

    private void CreateSOFromJson()
    {
        if (string.IsNullOrEmpty(inputJson))
        {
            creationStatus = "Please enter JSON data!";
            Debug.LogWarning(creationStatus);
            return;
        }

        if (selectedType == null)
        {
            creationStatus = "Please select a valid ScriptableObject type!";
            Debug.LogWarning(creationStatus);
            return;
        }

        try
        {
            var newSO = ScriptableObject.CreateInstance(selectedType);
            
            var settings = new JsonSerializerSettings
            {
                ObjectCreationHandling = ObjectCreationHandling.Replace
            };
            
            JsonConvert.PopulateObject(inputJson, newSO, settings);
            
            var path = EditorUtility.SaveFilePanelInProject(
                "Save ScriptableObject",
                selectedType.Name + "_New",
                "asset",
                "Please enter a file name to save the new ScriptableObject"
            );
            
            if (!string.IsNullOrEmpty(path))
            {
                AssetDatabase.CreateAsset(newSO, path);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                
                creationStatus = $"Successfully created {selectedType.Name} at {path}";
                Debug.Log(creationStatus);
                EditorGUIUtility.PingObject(newSO);
            }
            else
            {
                DestroyImmediate(newSO);
                creationStatus = "Creation cancelled by user.";
            }
        }
        catch (Exception e)
        {
            creationStatus = $"Error creating SO: {e.Message}";
            Debug.LogError(creationStatus);
        }
    }
}