using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

#pragma warning disable 0618

namespace GameSparks.Editor
{
    /// <summary>
    /// Editor class for <see cref="GameSparksSettings"/>
    /// </summary>
    [CustomEditor(typeof(GameSparksSettings))]
    public class GameSparksSettingsEditor : UnityEditor.Editor
    {
        GUIContent apiSecretLabel = new GUIContent("Api Secret [?]:", "GameSparks Api Secret can be found at https://portal.gamesparks.net");
        GUIContent apiKeyLabel = new GUIContent("Api Key [?]:", "GameSparks Api Key can be found at https://portal.gamesparks.net");
		GUIContent credentialLabel = new GUIContent("Credential [?]:", "GameSparks Credential can be found at https://portal.gamesparks.net");
    	GUIContent previewLabel = new GUIContent("Preview Build [?]:", "Run app against the preview service");
    	GUIContent debugLabel = new GUIContent("Debug Build [?]:", "Run app with extended debugging");

    	const string UnityAssetFolder = "Assets";

        public static GameSparksSettings GetOrCreateSettingsAsset()
    	{
    		string fullPath = Path.Combine(Path.Combine(UnityAssetFolder, GameSparksSettings.gamesparksSettingsPath),
    		                               GameSparksSettings.gamesparksSettingsAssetName + GameSparksSettings.gamesparksSettingsAssetExtension
    		                               );

    		GameSparksSettings instance = AssetDatabase.LoadAssetAtPath(fullPath, typeof(GameSparksSettings)) as GameSparksSettings;

    		if(instance == null)
    		{
    			// no asset found, we need to create it. 

    			if(!Directory.Exists(Path.Combine(UnityAssetFolder, GameSparksSettings.gamesparksSettingsPath)))
    			{
    				AssetDatabase.CreateFolder(Path.Combine(UnityAssetFolder, "GameSparks"), "Resources");
    			}

    			instance = CreateInstance<GameSparksSettings>();
    			AssetDatabase.CreateAsset(instance, fullPath);
    			AssetDatabase.SaveAssets();
    		}
    		return instance;
    	}

    	[MenuItem("GameSparks/Edit Settings")]
        public static void Edit()
        {
            Selection.activeObject = GetOrCreateSettingsAsset();

			UpdateSDK (true);
        }

    	void OnDisable()
    	{
    		// make sure the runtime code will load the Asset from Resources when it next tries to access this. 
    		GameSparksSettings.SetInstance(null);
    	}

		static GameSparksSettingsEditor()
		{
		}

		private static void RecursiveDeleteFolders(DirectoryInfo baseDir)
        {
            if (!baseDir.Exists)
                return;

            foreach (var dir in baseDir.GetDirectories("*", SearchOption.AllDirectories))
            {
                RecursiveDeleteFolders(dir);
            }

            try
            {
                if (baseDir.GetFiles().Length == 0)
                {
                    baseDir.Delete(true);

                    AssetDatabase.Refresh();
                }
            }
            catch
            {
            }
        }
      
        static bool firstLaunch = true;

        [InitializeOnLoadMethod]
        private static void FixSDK()
		{
            if (UnityEditorInternal.InternalEditorUtility.inBatchMode)
			{
				return;
			}
			
			try
			{
				string[] oldSDKFiles = {
					"Assets/GameSparks/Platforms/IOS",

                    "Assets/Plugins/GameSparks.Api.dll.mdb.meta",
                    "Assets/Plugins/GameSparks.Api.dll.mdb",

                    "Assets/Plugins/iOS/GameSparksWebSocket.h",
					"Assets/Plugins/iOS/GameSparksWebSocket.m",
					"Assets/Plugins/iOS/SRWebSocket.h",
					"Assets/Plugins/iOS/SRWebSocket.m",
					"Assets/Plugins/iOS/SocketController.h",
					"Assets/Plugins/iOS/SocketController.m",

					"Assets/GameSparks/Editor/GameSparksPostprocessScript.cs",
					"Assets/GameSparks/Editor/mod_pbxproj.py",
					"Assets/GameSparks/Editor/mod_pbxproj.pyc",
					"Assets/GameSparks/Editor/post_process.py"
				};

				foreach (string oldSDKFile in oldSDKFiles) {
					if (File.Exists(oldSDKFile))
					{
						File.Delete(oldSDKFile);
					} else if (Directory.Exists(oldSDKFile)) {
						Directory.Delete(oldSDKFile, true);                   
					}
				}
			}
			catch
			{		
			}

            string[] oldFiles = {
				"Assets/Plugins/gamesparksunityosx.bundle/Contents/_CodeSignature/CodeResources.meta",
                "Assets/Plugins/gamesparksunityosx.bundle/Contents/_CodeSignature/CodeResources",
				"Assets/Plugins/gamesparksunityosx.bundle/Contents/MacOS/gamesparksunityosx.meta",
                "Assets/Plugins/gamesparksunityosx.bundle/Contents/MacOS/gamesparksunityosx",
				"Assets/Plugins/gamesparksunityosx.bundle/Contents/Info.plist.meta",
                "Assets/Plugins/gamesparksunityosx.bundle/Contents/Info.plist",
				"Assets/Plugins/iOS/GSExternal.h.meta",
                "Assets/Plugins/iOS/GSExternal.h",
				"Assets/Plugins/iOS/GSExternal.m.meta",
                "Assets/Plugins/iOS/GSExternal.m",
				"Assets/Plugins/Metro/GameSparks.dll.meta",
                "Assets/Plugins/Metro/GameSparks.dll",
				"Assets/Plugins/Metro/GameSparks.pri.meta",
                "Assets/Plugins/Metro/GameSparks.pri",
				"Assets/Plugins/Metro/GameSparks.XML.meta",
                "Assets/Plugins/Metro/GameSparks.XML",
				"Assets/Plugins/Metro/GameSparksRT.dll.meta",
                "Assets/Plugins/Metro/GameSparksRT.dll",
				"Assets/Plugins/Metro/GameSparksRT.pri.meta",
                "Assets/Plugins/Metro/GameSparksRT.pri",
				"Assets/Plugins/Metro/GameSparksRT.xml.meta",
                "Assets/Plugins/Metro/GameSparksRT.xml",
				"Assets/Plugins/WebGL/GameSparks.jslib.meta",
                "Assets/Plugins/WebGL/GameSparks.jslib",
				"Assets/Plugins/WP8/GameSparks.dll.meta",
                "Assets/Plugins/WP8/GameSparks.dll",
				"Assets/Plugins/WP8/GameSparks.XML.meta",
                "Assets/Plugins/WP8/GameSparks.XML",
				"Assets/Plugins/x86_64/GameSparksNative.dll.meta",
                "Assets/Plugins/x86_64/GameSparksNative.dll",
				"Assets/Plugins/XboxOne/GameSparksNative.dll.meta",
                "Assets/Plugins/XboxOne/GameSparksNative.dll",
				"Assets/Plugins/BouncyCastle_GameSparks.dll.meta",
                "Assets/Plugins/BouncyCastle_GameSparks.dll",
				"Assets/Plugins/GameSparks.Api.dll.meta",
                "Assets/Plugins/GameSparks.Api.dll",
				"Assets/Plugins/GameSparks.Api.XML.meta",
                "Assets/Plugins/GameSparks.Api.XML",
				"Assets/Plugins/GameSparks.dll.meta",
                "Assets/Plugins/GameSparks.dll",
				"Assets/Plugins/GameSparks.XML.meta",
                "Assets/Plugins/GameSparks.XML",
				"Assets/Plugins/GameSparksRT.dll.meta",
                "Assets/Plugins/GameSparksRT.dll",
				"Assets/Plugins/GameSparksRT.xml.meta",
                "Assets/Plugins/GameSparksRT.xml"
            };
     
            string baseDir = "Plugins";
            string targetDir = "Assets/GameSparks/Plugins";
            DirectoryInfo dirInfo = new DirectoryInfo(targetDir);
            DirectoryInfo dirInfo2 = new DirectoryInfo("Assets/" + baseDir);

            if (!dirInfo.Exists)
            {
                Directory.CreateDirectory(dirInfo.ToString());
            }

            foreach (string file in oldFiles)
            {
                try
                {
                    FileInfo fileInfo = new FileInfo(file);

                    string relativePath = fileInfo.DirectoryName;

                    int index = relativePath.IndexOf(baseDir);

                    if (index + baseDir.Length < relativePath.Length)
                    {
                        index++;
                    }

                    relativePath = relativePath.Substring(index + baseDir.Length);

                    DirectoryInfo dirInfo3 = new DirectoryInfo(dirInfo + "/" + relativePath);

                    if (!dirInfo3.Exists)
                    {
                        Directory.CreateDirectory(dirInfo3.ToString());
                    }

                    if ((new FileInfo(dirInfo3 + "/" + fileInfo.Name)).Exists)
					{
                        if (fileInfo.Exists)
                        {
                            fileInfo.Delete();                       
                        }
                    }
					else
                    {
                        fileInfo.MoveTo(dirInfo3 + "/" + fileInfo.Name);
                    }
                }
                catch
                {             
                }
            }
            
            AssetDatabase.Refresh();

            RecursiveDeleteFolders(dirInfo2);

            if (firstLaunch)
            {
                firstLaunch = false;

                var assembly = Assembly.GetAssembly(typeof(UnityEditor.ActiveEditorTracker));
                var type = assembly.GetType("UnityEditorInternal.LogEntries");
                var method = type.GetMethod("Clear");
                method.Invoke(new object(), null);
            }
        }
    	
    	public override void OnInspectorGUI()
    	{
    		GameSparksSettings settings = (GameSparksSettings)target;
    		GameSparksSettings.SetInstance(settings);

    		GUILayout.TextArea("SDK Version : "+GameSparks.Core.GS.Version, EditorStyles.wordWrappedLabel);

    		EditorGUILayout.HelpBox("Add the GameSparks Api Key and Secret associated with this game", MessageType.None);
    		
    		EditorGUILayout.BeginHorizontal();
    		GameSparksSettings.ApiKey = EditorGUILayout.TextField(apiKeyLabel, GameSparksSettings.ApiKey).Trim();
            EditorGUILayout.EndHorizontal();

    		
            EditorGUILayout.BeginHorizontal();
    		GameSparksSettings.ApiSecret = EditorGUILayout.TextField(apiSecretLabel, GameSparksSettings.ApiSecret).Trim();
            EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			GameSparksSettings.Credential = EditorGUILayout.TextField(credentialLabel, GameSparksSettings.Credential);
			EditorGUILayout.EndHorizontal();
    		
    		
    		EditorGUILayout.BeginHorizontal();
    		GameSparksSettings.PreviewBuild = EditorGUILayout.Toggle(previewLabel, GameSparksSettings.PreviewBuild);
            EditorGUILayout.EndHorizontal();
    		
    		EditorGUILayout.BeginHorizontal();
    		GameSparksSettings.DebugBuild = EditorGUILayout.Toggle(debugLabel, GameSparksSettings.DebugBuild);
            EditorGUILayout.EndHorizontal();

    		EditorGUILayout.Space();


    		String testScenePath = "Assets/GameSparks/TestUI/GameSparksTestUI.unity";

    		String testButtonText = "Test Configuration";

    		if(EditorApplication.currentScene.Equals(testScenePath) && EditorApplication.isPlaying){
    			testButtonText = "Stop Test";
    		}

    		if(GameSparksSettings.ApiKey != null && GameSparksSettings.ApiSecret != null){
    			String myApiPath = "Assets/GameSparks/MyGameSparks.cs";
    			GUILayout.TextArea("Download your custom data structures into your own SDK. Be sure to update this if you change the structure of Events and Leaderboards within the developer portal", EditorStyles.wordWrappedLabel);
    			if(GUILayout.Button("Get My Custom SDK")){
    				String myApi = GameSparksRestApi.getApi();
    				if(myApi != null){
    					Debug.Log("Updating GameSparks Api for game." + GameSparksSettings.ApiKey);
    					Directory.CreateDirectory(Path.GetDirectoryName(myApiPath));
    					using (StreamWriter outfile = new StreamWriter(myApiPath))
    					{
    						outfile.Write(myApi);
    					}
    				}
    				EditorUtility.SetDirty(settings);
    				AssetDatabase.Refresh();

    			}

    			GUILayout.TextArea("Get the latest GameSparks SDK version.", EditorStyles.wordWrappedLabel);
    
    			if(GUILayout.Button("Update SDK"))
				{
					UpdateSDK (false);
    			}
    		}

    		GUILayout.TextArea("Run the GameSparks test harness in the editor. ", EditorStyles.wordWrappedLabel);
    		if(GUILayout.Button(testButtonText)){
    			EditorUtility.SetDirty(settings);
    			if(EditorApplication.currentScene.Equals(testScenePath) && EditorApplication.isPlaying){
    				EditorApplication.isPlaying = false;
    			} else {
    				if(!EditorApplication.currentScene.Equals(testScenePath)){
    					if(EditorApplication.SaveCurrentSceneIfUserWantsTo()){
    						if(!EditorApplication.OpenScene(testScenePath)){
    							EditorApplication.NewScene();
    							new GameObject("GameSparks", typeof(GameSparksTestUI), typeof(GameSparksUnity));
    							EditorApplication.SaveScene(testScenePath);
    						}
    						EditorApplication.isPlaying = true;
    					}
    				} else {
    					EditorApplication.isPlaying = true;
    				}
    			}
    		}
    		if(GUI.changed)
    		{
    			EditorUtility.SetDirty(settings);
    			AssetDatabase.SaveAssets();
    		}
            
        }

		private static void UpdateSDK(Boolean silentMode)
		{
			string lastVersion = GameSparksRestApi.GetLastVersion ();

			if (lastVersion != null) {
				Debug.Log ("Latest version available: " + lastVersion);

				if (GameSparksRestApi.CompareCurrentWithLastVersion (GameSparks.Core.GS.Version, lastVersion)) {
					if (EditorUtility.DisplayDialog ("GameSparks SDK", "There is a new available SDK.\nWould you like to update it?", "Yes", "No")) {
						Debug.Log ("Updating GameSparks SDK from " + GameSparks.Core.GS.Version + " to " + lastVersion + " version");

						if (GameSparksRestApi.UpdateSDK (lastVersion)) {
							Debug.Log ("Updated GameSparks SDK!");
						}
					}
				} else {
					if (!silentMode) {
						EditorUtility.DisplayDialog ("GameSparks SDK", "Sorry, there is any new available SDK.", "OK");
					}
				}
			} else {
				if (!silentMode) {
					EditorUtility.DisplayDialog ("GameSparks SDK", "Error occured during getting last version!", "OK");
				}
			}
		}
        
    }
}