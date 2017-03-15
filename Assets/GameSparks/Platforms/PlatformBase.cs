using UnityEngine;
using System.Collections;
using GameSparks.Core;
using System.Collections.Generic;
using System;

namespace GameSparks.Platforms
{
	/// <summary>
	/// This is the base class for all platform specific implementations.
	/// Depending on your BuildTarget in Unity, GameSparks will automatically determine
	/// which implementation to use for platform specific code.
	/// </summary>
	public abstract class PlatformBase : MonoBehaviour, GameSparks.Core.IGSPlatform {

		static string PLAYER_PREF_AUTHTOKEN_KEY = "gamesparks.authtoken";
		static string PLAYER_PREF_USERID_KEY = "gamesparks.userid";
        static string PLAYER_PREF_DEVICEID_KEY = "gamesparks.deviceid";


		virtual protected void Start()
		{

			DeviceName = SystemInfo.deviceName.ToString();
			DeviceType = SystemInfo.deviceType.ToString();
            if (Application.platform == RuntimePlatform.PS4 || Application.platform == RuntimePlatform.XboxOne)
            {
#if GS_DONT_USE_PLAYER_PREFS
                DeviceId = SystemInfo.deviceUniqueIdentifier.ToString();
#else
                DeviceId = PlayerPrefs.GetString(PLAYER_PREF_DEVICEID_KEY);
                if (DeviceId.Equals(""))
                {
                    DeviceId = System.Guid.NewGuid().ToString();

                    PlayerPrefs.SetString(PLAYER_PREF_DEVICEID_KEY, DeviceId);
                    PlayerPrefs.Save();
                }
#endif
            }
            else
            {
                DeviceId = SystemInfo.deviceUniqueIdentifier.ToString();
            }
#if !GS_DONT_USE_PLAYER_PREFS
			AuthToken = PlayerPrefs.GetString(PLAYER_PREF_AUTHTOKEN_KEY);
			UserId = PlayerPrefs.GetString(PLAYER_PREF_USERID_KEY);
#endif
			Platform = Application.platform.ToString();
			ExtraDebug = GameSparksSettings.DebugBuild;

#if !UNITY_WEBPLAYER
			PersistentDataPath = Application.persistentDataPath;
#endif
			RequestTimeoutSeconds = 10;

			GS.Initialise(this);

			DontDestroyOnLoad (this);

		}
		private List<Action> _actions = new List<Action>();
		List<Action> _currentActions = new List<Action>();

		/// <summary>
		/// Executes the given Action the on main thread of Unity.
		/// </summary>
		public void ExecuteOnMainThread(Action action){
			lock(_actions){
				_actions.Add(action);
			}
		}

		virtual protected void Update(){

			lock (_actions)
			{
				if (_actions.Count > 0) {
					_currentActions.AddRange (_actions);
					_actions.Clear ();
				}
			}
			var count = _currentActions.Count;

			if (count > 0) {
				for (var index = 0; index < count; ++index) {
					var a = _currentActions [index];
					if (a != null) {
						a ();
					}
				}

				_currentActions.Clear ();
			}
		}

		virtual protected void OnApplicationPause(bool paused)
		{
			if(paused)
			{
#if UNITY_EDITOR
				GS.Disconnect();
#endif
			}
			else
			{
				try{
					GS.Reconnect();
				}catch(Exception e) {
					if(ExceptionReporter != null) {
						ExceptionReporter(e);
					}
				}
			}
		}

		virtual protected void OnApplicationQuit(){
			GS.Disconnect();
		}

		virtual protected void OnDestroy () {
			Update();
			GS.ShutDown();
		}

		public String DeviceOS {
			get{
                switch (Application.platform)
                {
                    case RuntimePlatform.OSXEditor:
                    case RuntimePlatform.OSXPlayer:
                    case RuntimePlatform.OSXDashboardPlayer:             
                        return "MACOS";

                    case RuntimePlatform.WindowsPlayer:
                    case RuntimePlatform.WindowsEditor:
                        return "WINDOWS";

                    case RuntimePlatform.IPhonePlayer:
                        return "IOS";         

                    case RuntimePlatform.Android:
                        return "ANDROID";

                    case RuntimePlatform.LinuxPlayer:
                        return "LINUX";

                    case RuntimePlatform.WebGLPlayer:
                        return "WEBGL";

                    case RuntimePlatform.WSAPlayerX86:
                    case RuntimePlatform.WSAPlayerX64:
                    case RuntimePlatform.WSAPlayerARM:
                        return "WSA";

                    case RuntimePlatform.TizenPlayer:
                        return "TIZEN";

                    case RuntimePlatform.PS4:
                        return "PS4";

                    case RuntimePlatform.XboxOne:
                        return "XBOXONE";

                    case RuntimePlatform.SamsungTVPlayer:
                        return "SAMSUNGTV";

                    case RuntimePlatform.WiiU:
                        return "WIIU";

                    case RuntimePlatform.tvOS:
                        return "TVOS";

                    default:
                        return "UNKNOWN";
                }
			}
		}

		public String DeviceName  {get; private set;}
		public String DeviceType {get; private set;}
		public virtual String DeviceId  {get; private set;}
		public String Platform {get; private set;}

		/// <summary>
		/// Allow for extra debug output. To set it use the GameSparksSettings editor window. <see cref="GameSparksSettings.DebugBuild"/>
		/// </summary>
		public bool ExtraDebug {get; private set;}

		public string ApiKey { get {
				return GameSparksSettings.ApiKey;
			}
		}

		public string ApiSecret { get {
				return GameSparksSettings.ApiSecret;
			}
		}

		public string ApiCredential { get {
				return GameSparksSettings.Credential;
			}
		}

		public string ApiStage { get {
				return GameSparksSettings.PreviewBuild ? "preview" : "live";
			}
		}

		public String ApiDomain { get { return null; } }

		public int RequestTimeoutSeconds  {get; set;}
		public String PersistentDataPath{get; private set;}


		/// <summary>
		/// Logs the given string to the Unity debug console.
		/// </summary>
		public void DebugMsg(String message){
			ExecuteOnMainThread(() => {
				if (message.Length < 1500)
				{
					Debug.Log("GS: " + message);
				} else
				{
					Debug.Log("GS: " + message.Substring(0, 1500) + "...");
				}
			});
		}

		public String SDK{get;set;}

		private String m_authToken="0";

		public String AuthToken {
			get {return m_authToken;}
			set {
				m_authToken = value;
#if !GS_DONT_USE_PLAYER_PREFS
				PlayerPrefs.SetString(PLAYER_PREF_AUTHTOKEN_KEY, value);
#endif
			}
		}

		private String m_userId="";

		public String UserId {
			get {return m_userId;}
			set {
				m_userId = value;
#if !GS_DONT_USE_PLAYER_PREFS
				PlayerPrefs.SetString(PLAYER_PREF_USERID_KEY, value);
#endif
			}
		}

		public Action<Exception> ExceptionReporter {
			get;
			set;
		}

		/// <summary>
		/// Returns a (platform specific) timer implementation.
		/// </summary>
		/// <returns>The timer.</returns>
		public abstract IGameSparksTimer GetTimer();

		/// <summary>
		/// Returns a hmac created with SHA-256 based on the given parameters.
		/// </summary>
		public abstract string MakeHmac(string stringToHmac, string secret);

		/// <summary>
		/// Creates a (platform specific) Websocket and returns the instance.
		/// </summary>
		public abstract IGameSparksWebSocket GetSocket(string url, Action<string> messageReceived, Action closed, Action opened, Action<string> error);
	}

}