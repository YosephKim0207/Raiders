using System;
using System.Collections.Generic;
using InControl;
using UnityEngine;

namespace MultiplayerBasicExample
{
	// Token: 0x0200067F RID: 1663
	public class PlayerManager : MonoBehaviour
	{
		// Token: 0x060025D0 RID: 9680 RVA: 0x000A1F24 File Offset: 0x000A0124
		private void Start()
		{
			InputManager.OnDeviceDetached += this.OnDeviceDetached;
		}

		// Token: 0x060025D1 RID: 9681 RVA: 0x000A1F38 File Offset: 0x000A0138
		private void Update()
		{
			InputDevice activeDevice = InputManager.ActiveDevice;
			if (this.JoinButtonWasPressedOnDevice(activeDevice) && this.ThereIsNoPlayerUsingDevice(activeDevice))
			{
				this.CreatePlayer(activeDevice);
			}
		}

		// Token: 0x060025D2 RID: 9682 RVA: 0x000A1F6C File Offset: 0x000A016C
		private bool JoinButtonWasPressedOnDevice(InputDevice inputDevice)
		{
			return inputDevice.Action1.WasPressed || inputDevice.Action2.WasPressed || inputDevice.Action3.WasPressed || inputDevice.Action4.WasPressed;
		}

		// Token: 0x060025D3 RID: 9683 RVA: 0x000A1FAC File Offset: 0x000A01AC
		private Player FindPlayerUsingDevice(InputDevice inputDevice)
		{
			int count = this.players.Count;
			for (int i = 0; i < count; i++)
			{
				Player player = this.players[i];
				if (player.Device == inputDevice)
				{
					return player;
				}
			}
			return null;
		}

		// Token: 0x060025D4 RID: 9684 RVA: 0x000A1FF4 File Offset: 0x000A01F4
		private bool ThereIsNoPlayerUsingDevice(InputDevice inputDevice)
		{
			return this.FindPlayerUsingDevice(inputDevice) == null;
		}

		// Token: 0x060025D5 RID: 9685 RVA: 0x000A2004 File Offset: 0x000A0204
		private void OnDeviceDetached(InputDevice inputDevice)
		{
			Player player = this.FindPlayerUsingDevice(inputDevice);
			if (player != null)
			{
				this.RemovePlayer(player);
			}
		}

		// Token: 0x060025D6 RID: 9686 RVA: 0x000A202C File Offset: 0x000A022C
		private Player CreatePlayer(InputDevice inputDevice)
		{
			if (this.players.Count < 4)
			{
				Vector3 vector = this.playerPositions[0];
				this.playerPositions.RemoveAt(0);
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.playerPrefab, vector, Quaternion.identity);
				Player component = gameObject.GetComponent<Player>();
				component.Device = inputDevice;
				this.players.Add(component);
				return component;
			}
			return null;
		}

		// Token: 0x060025D7 RID: 9687 RVA: 0x000A2094 File Offset: 0x000A0294
		private void RemovePlayer(Player player)
		{
			this.playerPositions.Insert(0, player.transform.position);
			this.players.Remove(player);
			player.Device = null;
			UnityEngine.Object.Destroy(player.gameObject);
		}

		// Token: 0x060025D8 RID: 9688 RVA: 0x000A20CC File Offset: 0x000A02CC
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
				GUI.Label(new Rect(10f, num, 300f, num + 22f), "Press a button to join!");
				num += 22f;
			}
		}

		// Token: 0x040019BC RID: 6588
		public GameObject playerPrefab;

		// Token: 0x040019BD RID: 6589
		private const int maxPlayers = 4;

		// Token: 0x040019BE RID: 6590
		private List<Vector3> playerPositions = new List<Vector3>
		{
			new Vector3(-1f, 1f, -10f),
			new Vector3(1f, 1f, -10f),
			new Vector3(-1f, -1f, -10f),
			new Vector3(1f, -1f, -10f)
		};

		// Token: 0x040019BF RID: 6591
		private List<Player> players = new List<Player>(4);
	}
}
