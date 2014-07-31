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
	[KSPAddon(KSPAddon.Startup.SpaceCentre, true)]
	public class kTunes : MonoBehaviour
	{
		public static GameObject Instance;
		private static Rect playerWindow = new Rect(50, 50, 300, 50);
		private static int playerWindowId = UnityEngine.Random.Range(101, int.MaxValue); // Use a random window ID so a conflict will be unlikely

		public void Awake()
		{
			DontDestroyOnLoad(this);
		}

		public void OnGUI()
		{
			if (ShouldDrawGui)
			{
				playerWindow = GUI.Window(playerWindowId, playerWindow, OnLayoutPlayerWindow, "kTunes", HighLogic.Skin.window);
			}
		}

		private void OnLayoutPlayerWindow(int id)
		{
			GUILayout.BeginHorizontal();

			if (GUILayout.Button("P"))
			{
				InputSimulator.SimulateKeyPress(VirtualKeyCode.MEDIA_PREV_TRACK);
			}

			if (GUILayout.Button("P/P"))
			{
				InputSimulator.SimulateKeyPress(VirtualKeyCode.MEDIA_PLAY_PAUSE);
			}

			if (GUILayout.Button("N"))
			{
				InputSimulator.SimulateKeyPress(VirtualKeyCode.MEDIA_NEXT_TRACK);
			}

			GUILayout.EndHorizontal();

			GUI.DragWindow();
		}

		public bool ShouldDrawGui
		{
            get
            {
                return true;
            }
		}
	}
}