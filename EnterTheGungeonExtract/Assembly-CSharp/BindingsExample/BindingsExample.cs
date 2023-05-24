using System;
using Brave;
using InControl;
using UnityEngine;

namespace BindingsExample
{
	// Token: 0x02000678 RID: 1656
	public class BindingsExample : MonoBehaviour
	{
		// Token: 0x060025B1 RID: 9649 RVA: 0x000A134C File Offset: 0x0009F54C
		private void OnEnable()
		{
			this.playerActions = PlayerActions.CreateWithDefaultBindings();
			this.LoadBindings();
		}

		// Token: 0x060025B2 RID: 9650 RVA: 0x000A1360 File Offset: 0x0009F560
		private void OnDisable()
		{
			this.playerActions.Destroy();
		}

		// Token: 0x060025B3 RID: 9651 RVA: 0x000A1370 File Offset: 0x0009F570
		private void Start()
		{
			this.cachedRenderer = base.GetComponent<Renderer>();
		}

		// Token: 0x060025B4 RID: 9652 RVA: 0x000A1380 File Offset: 0x0009F580
		private void Update()
		{
			base.transform.Rotate(Vector3.down, 500f * Time.deltaTime * this.playerActions.Move.X, Space.World);
			base.transform.Rotate(Vector3.right, 500f * Time.deltaTime * this.playerActions.Move.Y, Space.World);
			Color color = ((!this.playerActions.Fire.IsPressed) ? Color.white : Color.red);
			Color color2 = ((!this.playerActions.Jump.IsPressed) ? Color.white : Color.green);
			this.cachedRenderer.material.color = Color.Lerp(color, color2, 0.5f);
		}

		// Token: 0x060025B5 RID: 9653 RVA: 0x000A1450 File Offset: 0x0009F650
		private void SaveBindings()
		{
			this.saveData = this.playerActions.Save();
			Brave.PlayerPrefs.SetString("Bindings", this.saveData);
		}

		// Token: 0x060025B6 RID: 9654 RVA: 0x000A1474 File Offset: 0x0009F674
		private void LoadBindings()
		{
			if (Brave.PlayerPrefs.HasKey("Bindings"))
			{
				this.saveData = Brave.PlayerPrefs.GetString("Bindings");
				this.playerActions.Load(this.saveData, false);
			}
		}

		// Token: 0x060025B7 RID: 9655 RVA: 0x000A14A8 File Offset: 0x0009F6A8
		private void OnApplicationQuit()
		{
			Brave.PlayerPrefs.Save();
		}

		// Token: 0x060025B8 RID: 9656 RVA: 0x000A14B0 File Offset: 0x0009F6B0
		private void OnGUI()
		{
			float num = 10f;
			GUI.Label(new Rect(10f, num, 300f, num + 22f), "Last Input Type: " + this.playerActions.LastInputType);
			num += 22f;
			GUI.Label(new Rect(10f, num, 300f, num + 22f), "Last Device Class: " + this.playerActions.LastDeviceClass);
			num += 22f;
			GUI.Label(new Rect(10f, num, 300f, num + 22f), "Last Device Style: " + this.playerActions.LastDeviceStyle);
			num += 22f;
			int count = this.playerActions.Actions.Count;
			for (int i = 0; i < count; i++)
			{
				PlayerAction playerAction = this.playerActions.Actions[i];
				string text = playerAction.Name;
				if (playerAction.IsListeningForBinding)
				{
					text += " (Listening)";
				}
				text = text + " = " + playerAction.Value;
				GUI.Label(new Rect(10f, num, 500f, num + 22f), text);
				num += 22f;
				int count2 = playerAction.Bindings.Count;
				for (int j = 0; j < count2; j++)
				{
					BindingSource bindingSource = playerAction.Bindings[j];
					GUI.Label(new Rect(75f, num, 300f, num + 22f), bindingSource.DeviceName + ": " + bindingSource.Name);
					if (GUI.Button(new Rect(20f, num + 3f, 20f, 17f), "-"))
					{
						playerAction.RemoveBinding(bindingSource);
					}
					if (GUI.Button(new Rect(45f, num + 3f, 20f, 17f), "+"))
					{
						playerAction.ListenForBindingReplacing(bindingSource);
					}
					num += 22f;
				}
				if (GUI.Button(new Rect(20f, num + 3f, 20f, 17f), "+"))
				{
					playerAction.ListenForBinding();
				}
				if (GUI.Button(new Rect(50f, num + 3f, 50f, 17f), "Reset"))
				{
					playerAction.ResetBindings();
				}
				num += 25f;
			}
			if (GUI.Button(new Rect(20f, num + 3f, 50f, 22f), "Load"))
			{
				this.LoadBindings();
			}
			if (GUI.Button(new Rect(80f, num + 3f, 50f, 22f), "Save"))
			{
				this.SaveBindings();
			}
			if (GUI.Button(new Rect(140f, num + 3f, 50f, 22f), "Reset"))
			{
				this.playerActions.Reset();
			}
		}

		// Token: 0x040019A6 RID: 6566
		private Renderer cachedRenderer;

		// Token: 0x040019A7 RID: 6567
		private PlayerActions playerActions;

		// Token: 0x040019A8 RID: 6568
		private string saveData;
	}
}
