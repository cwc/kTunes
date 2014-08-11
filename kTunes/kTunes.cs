using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using UnityEngine;
using System.ComponentModel;
using WindowsInput;

namespace kTunes
{
	[KSPAddon (KSPAddon.Startup.SpaceCentre, true)]
	public class kTunes : MonoBehaviour
	{
		public const string Name = "kTunes";
		public static GameObject Instance;

		// Use a random window ID so a conflict will be unlikely
		private static readonly int PlayerWindowId = UnityEngine.Random.Range (500, int.MaxValue);

		// The initial window position is near the top, centered horizontally
		private const int PlayerWindowWidth = 175;
		private const int PlayerWindowHeight = 35;
		private static readonly int PlayerWindowXOffset = Screen.width / 2 - PlayerWindowWidth / 2;
		private const int PlayerWindowYOffset = 65;

		private static Rect playerWindowRect = new Rect (PlayerWindowXOffset, PlayerWindowYOffset, PlayerWindowWidth, PlayerWindowHeight);
		private static GUIStyle playerWindowStyle = new GUIStyle(HighLogic.Skin.window);

		private static ApplicationLauncherButton appLauncherButton;
		private static bool isPlayerVisible = false;

		public void Awake ()
		{
			DontDestroyOnLoad (this);

			playerWindowStyle.padding = new RectOffset(5, 5, 5, 5);

			GameEvents.onGUIApplicationLauncherReady.Add (OnGUIApplicationLauncherReady);
		}

		private void OnGUIApplicationLauncherReady ()
		{
			CreateLauncherButton ();

			ApplicationLauncher.Instance.AddOnShowCallback (CreateLauncherButton);

			ApplicationLauncher.Instance.AddOnHideCallback (() => {
				isPlayerVisible = false;

				if (appLauncherButton != null) {
					ApplicationLauncher.Instance.RemoveModApplication (appLauncherButton);
					appLauncherButton = null;
				}
			});
		}

		private void CreateLauncherButton ()
		{
			if (appLauncherButton == null) {
				appLauncherButton = ApplicationLauncher.Instance.AddModApplication (OnAppLauncherToggleOn, OnAppLauncherToggleOff, NoOp, NoOp, NoOp, NoOp, ApplicationLauncher.AppScenes.ALWAYS, (Texture)GameDatabase.Instance.GetTexture (Name + "/Textures/app_launcher_icon", false));
			}
		}

		private void OnAppLauncherToggleOn ()
		{
			isPlayerVisible = true;
		}

		private void OnAppLauncherToggleOff ()
		{
			isPlayerVisible = false;
		}

		private void NoOp ()
		{
		}

		public void OnGUI ()
		{
			if (isPlayerVisible) {
				playerWindowRect = GUI.Window (PlayerWindowId, playerWindowRect, OnLayoutPlayerWindow, (string)null, playerWindowStyle);
			}
		}

		private void OnLayoutPlayerWindow (int id)
		{
			GUILayout.BeginHorizontal ();

			if (GUILayout.Button (GameDatabase.Instance.GetTexture (Name + "/Textures/previous_icon", false))) {
				InputSimulator.SimulateKeyPress (VirtualKeyCode.MEDIA_PREV_TRACK);
			}

			if (GUILayout.Button (GameDatabase.Instance.GetTexture (Name + "/Textures/play_pause_icon", false))) {
				InputSimulator.SimulateKeyPress (VirtualKeyCode.MEDIA_PLAY_PAUSE);
			}

			if (GUILayout.Button (GameDatabase.Instance.GetTexture (Name + "/Textures/next_icon", false))) {
				InputSimulator.SimulateKeyPress (VirtualKeyCode.MEDIA_NEXT_TRACK);
			}

			GUILayout.EndHorizontal ();

			GUI.DragWindow ();
		}

		public void OnDestroy ()
		{
			GameEvents.onGUIApplicationLauncherReady.Remove (OnGUIApplicationLauncherReady);
			if (appLauncherButton != null)
				ApplicationLauncher.Instance.RemoveModApplication (appLauncherButton);
		}
	}
}