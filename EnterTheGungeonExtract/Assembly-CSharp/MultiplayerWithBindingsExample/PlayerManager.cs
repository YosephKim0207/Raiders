using System;
using System.Collections.Generic;
using InControl;
using UnityEngine;

namespace MultiplayerWithBindingsExample
{
	// Token: 0x02000682 RID: 1666
	public class PlayerManager : MonoBehaviour
	{
		// Token: 0x060025E4 RID: 9700 RVA: 0x000A25B4 File Offset: 0x000A07B4
		private void OnEnable()
		{
			InputManager.OnDeviceDetached += this.OnDeviceDetached;
			this.keyboardListener = PlayerActions.CreateWithKeyboardBindings();
			this.joystickListener = PlayerActions.CreateWithJoystickBindings();
		}

		// Token: 0x060025E5 RID: 9701 RVA: 0x000A25E0 File Offset: 0x000A07E0
		private void OnDisable()
		{
			InputManager.OnDeviceDetached -= this.OnDeviceDetached;
			this.joystickListener.Destroy();
			this.keyboardListener.Destroy();
		}

		// Token: 0x060025E6 RID: 9702 RVA: 0x000A260C File Offset: 0x000A080C
		private void Update()
		{
			if (this.JoinButtonWasPressedOnListener(this.joystickListener))
			{
				InputDevice activeDevice = InputManager.ActiveDevice;
				if (this.ThereIsNoPlayerUsingJoystick(activeDevice))
				{
					this.CreatePlayer(activeDevice);
				}
			}
			if (this.JoinButtonWasPressedOnListener(this.keyboardListener) && this.ThereIsNoPlayerUsingKeyboard())
			{
				this.CreatePlayer(null);
			}
		}

		// Token: 0x060025E7 RID: 9703 RVA: 0x000A2668 File Offset: 0x000A0868
		private bool JoinButtonWasPressedOnListener(PlayerActions actions)
		{
			return actions.Green.WasPressed || actions.Red.WasPressed || actions.Blue.WasPressed || actions.Yellow.WasPressed;
		}

		// Token: 0x060025E8 RID: 9704 RVA: 0x000A26A8 File Offset: 0x000A08A8
		private Player FindPlayerUsingJoystick(InputDevice inputDevice)
		{
			int count = this.players.Count;
			for (int i = 0; i < count; i++)
			{
				Player player = this.players[i];
				if (player.Actions.Device == inputDevice)
				{
					return player;
				}
			}
			return null;
		}

		// Token: 0x060025E9 RID: 9705 RVA: 0x000A26F4 File Offset: 0x000A08F4
		private bool ThereIsNoPlayerUsingJoystick(InputDevice inputDevice)
		{
			return this.FindPlayerUsingJoystick(inputDevice) == null;
		}

		// Token: 0x060025EA RID: 9706 RVA: 0x000A2704 File Offset: 0x000A0904
		private Player FindPlayerUsingKeyboard()
		{
			int count = this.players.Count;
			for (int i = 0; i < count; i++)
			{
				Player player = this.players[i];
				if (player.Actions == this.keyboardListener)
				{
					return player;
				}
			}
			return null;
		}

		// Token: 0x060025EB RID: 9707 RVA: 0x000A2750 File Offset: 0x000A0950
		private bool ThereIsNoPlayerUsingKeyboard()
		{
			return this.FindPlayerUsingKeyboard() == null;
		}

		// Token: 0x060025EC RID: 9708 RVA: 0x000A2760 File Offset: 0x000A0960
		private void OnDeviceDetached(InputDevice inputDevice)
		{
			Player player = this.FindPlayerUsingJoystick(inputDevice);
			if (player != null)
			{
				this.RemovePlayer(player);
			}
		}

		// Token: 0x060025ED RID: 9709 RVA: 0x000A2788 File Offset: 0x000A0988
		private Player CreatePlayer(InputDevice inputDevice)
		{
			if (this.players.Count < 4)
			{
				Vector3 vector = this.playerPositions[0];
				this.playerPositions.RemoveAt(0);
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.playerPrefab, vector, Quaternion.identity);
				Player component = gameObject.GetComponent<Player>();
				if (inputDevice == null)
				{
					component.Actions = this.keyboardListener;
				}
				else
				{
					PlayerActions playerActions = PlayerActions.CreateWithJoystickBindings();
					playerActions.Device = inputDevice;
					component.Actions = playerActions;
				}
				this.players.Add(component);
				return component;
			}
			return null;
		}

		// Token: 0x060025EE RID: 9710 RVA: 0x000A2814 File Offset: 0x000A0A14
		private void RemovePlayer(Player player)
		{
			this.playerPositions.Insert(0, player.transform.position);
			this.players.Remove(player);
			player.Actions = null;
			UnityEngine.Object.Destroy(player.gameObject);
		}

		// Token: 0x060025EF RID: 9711 RVA: 0x000A284C File Offset: 0x000A0A4C
		private void OnGUI()
		{
			float num = 10f;
			GUI.Label(new Rect(10f, num, 300f, num + 22f), string.Concat(new object[]
			{
				"Active players: ",
				this.players.Count,
				"/",
				4
			}));
			num += 22f;
			if (this.players.Count < 4)
			{
				GUI.Label(new Rect(10f, num, 300f, num + 22f), "Press a button or a/s/d/f key to join!");
				num += 22f;
			}
		}

		// Token: 0x040019CB RID: 6603
		public GameObject playerPrefab;

		// Token: 0x040019CC RID: 6604
		private const int maxPlayers = 4;

		// Token: 0x040019CD RID: 6605
		private List<Vector3> playerPositions = new List<Vector3>
		{
			new Vector3(-1f, 1f, -10f),
			new Vector3(1f, 1f, -10f),
			new Vector3(-1f, -1f, -10f),
			new Vector3(1f, -1f, -10f)
		};

		// Token: 0x040019CE RID: 6606
		private List<Player> players = new List<Player>(4);

		// Token: 0x040019CF RID: 6607
		private PlayerActions keyboardListener;

		// Token: 0x040019D0 RID: 6608
		private PlayerActions joystickListener;
	}
}
