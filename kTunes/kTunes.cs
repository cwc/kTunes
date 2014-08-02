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
		private static int playerWindowId = UnityEngine.Random.Range (500, int.MaxValue);
		private static Rect playerWindow = new Rect (50, 50, 300, 50);

		private static ApplicationLauncherButton appLauncherButton;
		private static bool isPlayerVisible = false;

		public void Awake ()
		{
			DontDestroyOnLoad (this);

			GameEvents.onGUIApplicationLauncherReady.Add (OnGUIApplicationLauncherReady);
		}

		private void OnGUIApplicationLauncherReady ()
		{
			appLauncherButton = ApplicationLauncher.Instance.AddModApplication (
				OnAppLauncherToggleOn,
				OnAppLauncherToggleOff,
				NoOp,
				NoOp,
				NoOp,
				NoOp,
				ApplicationLauncher.AppScenes.ALWAYS,
				(Texture)GameDatabase.Instance.GetTexture (Name + "/Textures/app_launcher_icon", false));
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
				playerWindow = GUI.Window (playerWindowId, playerWindow, OnLayoutPlayerWindow, Name, HighLogic.Skin.window);
			}
		}

		private void OnLayoutPlayerWindow (int id)
		{
			GUILayout.BeginHorizontal ();

			if (GUILayout.Button ("P")) {
				InputSimulator.SimulateKeyPress (VirtualKeyCode.MEDIA_PREV_TRACK);
			}

			if (GUILayout.Button ("P/P")) {
				InputSimulator.SimulateKeyPress (VirtualKeyCode.MEDIA_PLAY_PAUSE);
			}

			if (GUILayout.Button ("N")) {
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