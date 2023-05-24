using System;
using System.Collections;
using System.Collections.Generic;
using InControl;
using UnityEngine;

// Token: 0x02000360 RID: 864
public class BraveInput : MonoBehaviour
{
	// Token: 0x06000DAC RID: 3500 RVA: 0x0004107C File Offset: 0x0003F27C
	private static void DoStartupAssignmentOfControllers(int lastActiveDeviceIndex = -1)
	{
		if (GameManager.PreventGameManagerExistence || GameManager.Instance.CurrentGameType == GameManager.GameType.SINGLE_PLAYER)
		{
			GameManager.Options.PlayerIDtoDeviceIndexMap.Clear();
		}
		else if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER)
		{
			GameManager.Options.PlayerIDtoDeviceIndexMap.Clear();
			if (Application.platform == RuntimePlatform.PS4)
			{
				GameManager.Options.PlayerIDtoDeviceIndexMap.Add(0, 0);
				GameManager.Options.PlayerIDtoDeviceIndexMap.Add(1, 1);
			}
			else if (InputManager.Devices.Count == 1)
			{
				if (lastActiveDeviceIndex != 0)
				{
					GameManager.Options.PlayerIDtoDeviceIndexMap.Add(1, 0);
					GameManager.Options.PlayerIDtoDeviceIndexMap.Add(0, 1);
				}
				else
				{
					GameManager.Options.PlayerIDtoDeviceIndexMap.Add(0, 0);
					GameManager.Options.PlayerIDtoDeviceIndexMap.Add(1, 1);
				}
			}
			else if (InputManager.Devices.Count == 2)
			{
				if (lastActiveDeviceIndex >= 1)
				{
					GameManager.Options.PlayerIDtoDeviceIndexMap.Add(0, lastActiveDeviceIndex);
					GameManager.Options.PlayerIDtoDeviceIndexMap.Add(1, 0);
				}
				else
				{
					GameManager.Options.PlayerIDtoDeviceIndexMap.Add(0, 0);
					GameManager.Options.PlayerIDtoDeviceIndexMap.Add(1, 1);
				}
			}
			else if (lastActiveDeviceIndex >= 1)
			{
				GameManager.Options.PlayerIDtoDeviceIndexMap.Add(0, lastActiveDeviceIndex);
				GameManager.Options.PlayerIDtoDeviceIndexMap.Add(1, -1);
				GameManager.Instance.StartCoroutine(BraveInput.AssignPlayerTwoToNextActiveDevice());
			}
			else
			{
				GameManager.Options.PlayerIDtoDeviceIndexMap.Add(0, 0);
				GameManager.Options.PlayerIDtoDeviceIndexMap.Add(1, 1);
			}
		}
	}

	// Token: 0x06000DAD RID: 3501 RVA: 0x00041234 File Offset: 0x0003F434
	private static IEnumerator AssignPlayerTwoToNextActiveDevice()
	{
		int lastActiveDeviceIndex;
		for (;;)
		{
			InputDevice lastActiveDevice = InputManager.ActiveDevice;
			lastActiveDeviceIndex = -1;
			for (int i = 0; i < InputManager.Devices.Count; i++)
			{
				if (InputManager.Devices[i] == lastActiveDevice)
				{
					lastActiveDeviceIndex = i;
				}
			}
			if (GameManager.Options.PlayerIDtoDeviceIndexMap.ContainsKey(0) && GameManager.Options.PlayerIDtoDeviceIndexMap.ContainsKey(1) && GameManager.Options.PlayerIDtoDeviceIndexMap[0] != lastActiveDeviceIndex)
			{
				break;
			}
			yield return null;
		}
		BraveInput.ReassignPlayerPort(1, lastActiveDeviceIndex);
		yield break;
		yield break;
	}

	// Token: 0x06000DAE RID: 3502 RVA: 0x00041248 File Offset: 0x0003F448
	public static void ReassignAllControllers(InputDevice overrideLastActiveDevice = null)
	{
		Debug.LogWarning("Reassigning all controllers.");
		InputDevice inputDevice = overrideLastActiveDevice ?? InputManager.ActiveDevice;
		int num = -1;
		for (int i = 0; i < InputManager.Devices.Count; i++)
		{
			if (InputManager.Devices[i] == inputDevice)
			{
				num = i;
			}
		}
		for (int j = 0; j < BraveInput.m_instances.Count; j++)
		{
			if (BraveInput.m_instances[j].m_activeGungeonActions != null)
			{
				BraveInput.m_instances[j].m_activeGungeonActions.Destroy();
			}
			BraveInput.m_instances[j].m_activeGungeonActions = null;
		}
		BraveInput.DoStartupAssignmentOfControllers(num);
		for (int k = 0; k < BraveInput.m_instances.Count; k++)
		{
			if (BraveInput.m_instances[k].m_activeGungeonActions == null)
			{
				BraveInput.m_instances[k].m_activeGungeonActions = new GungeonActions();
				BraveInput.m_instances[k].AssignActionsDevice();
				BraveInput.m_instances[k].m_activeGungeonActions.InitializeDefaults();
				if ((GameManager.Instance.PrimaryPlayer == null && BraveInput.m_instances[k].m_playerID == 0) || BraveInput.m_instances[k].m_playerID == GameManager.Instance.PrimaryPlayer.PlayerIDX)
				{
					BraveInput.TryLoadBindings(0, BraveInput.m_instances[k].ActiveActions);
				}
				else
				{
					BraveInput.TryLoadBindings(1, BraveInput.m_instances[k].ActiveActions);
				}
			}
			BraveInput.m_instances[k].AssignActionsDevice();
		}
		for (int l = 0; l < BraveInput.m_instances.Count; l++)
		{
			if (GameManager.Instance.AllPlayers.Length > 1)
			{
				if (BraveInput.m_instances[l].m_activeGungeonActions.Device == null)
				{
					BraveInput.m_instances[l].m_activeGungeonActions.IgnoreBindingsOfType(BindingSourceType.DeviceBindingSource);
				}
				else if (BraveInput.m_instances[l].m_playerID == GameManager.Instance.PrimaryPlayer.PlayerIDX)
				{
					if (BraveInput.GetInstanceForPlayer(GameManager.Instance.SecondaryPlayer.PlayerIDX).m_activeGungeonActions.Device == null)
					{
						BraveInput.m_instances[l].m_activeGungeonActions.IgnoreBindingsOfType(BindingSourceType.KeyBindingSource);
						BraveInput.m_instances[l].m_activeGungeonActions.IgnoreBindingsOfType(BindingSourceType.MouseBindingSource);
					}
				}
				else
				{
					BraveInput.m_instances[l].m_activeGungeonActions.IgnoreBindingsOfType(BindingSourceType.KeyBindingSource);
					BraveInput.m_instances[l].m_activeGungeonActions.IgnoreBindingsOfType(BindingSourceType.MouseBindingSource);
				}
			}
		}
	}

	// Token: 0x06000DAF RID: 3503 RVA: 0x00041510 File Offset: 0x0003F710
	public static void ForceLoadBindingInfoFromOptions()
	{
		if (GameManager.Options == null)
		{
			return;
		}
		for (int i = 0; i < BraveInput.m_instances.Count; i++)
		{
			if (GameManager.PreventGameManagerExistence || GameManager.Instance.PrimaryPlayer == null)
			{
				if (BraveInput.m_instances[i].m_playerID == 0)
				{
					BraveInput.TryLoadBindings(0, BraveInput.m_instances[i].ActiveActions);
				}
			}
			else if (BraveInput.m_instances[i].m_playerID == GameManager.Instance.PrimaryPlayer.PlayerIDX)
			{
				BraveInput.TryLoadBindings(0, BraveInput.m_instances[i].ActiveActions);
			}
			else
			{
				BraveInput.TryLoadBindings(1, BraveInput.m_instances[i].ActiveActions);
			}
		}
	}

	// Token: 0x06000DB0 RID: 3504 RVA: 0x000415E8 File Offset: 0x0003F7E8
	public static void SavePlayerlessBindingsToOptions()
	{
		if (GameManager.Options == null || GameManager.Instance.PrimaryPlayer != null || BraveInput.PlayerlessInstance == null)
		{
			return;
		}
		GameManager.Options.playerOneBindingDataV2 = BraveInput.PlayerlessInstance.ActiveActions.Save();
	}

	// Token: 0x06000DB1 RID: 3505 RVA: 0x00041640 File Offset: 0x0003F840
	public static void SaveBindingInfoToOptions()
	{
		if (GameManager.Options == null || GameManager.Instance.PrimaryPlayer == null)
		{
			return;
		}
		Debug.Log("Saving Binding Info To Options");
		for (int i = 0; i < BraveInput.m_instances.Count; i++)
		{
			if (BraveInput.m_instances[i].m_playerID == GameManager.Instance.PrimaryPlayer.PlayerIDX)
			{
				GameManager.Options.playerOneBindingDataV2 = BraveInput.m_instances[i].ActiveActions.Save();
			}
			else
			{
				GameManager.Options.playerTwoBindingDataV2 = BraveInput.m_instances[i].ActiveActions.Save();
			}
		}
	}

	// Token: 0x06000DB2 RID: 3506 RVA: 0x000416FC File Offset: 0x0003F8FC
	public static void OnLanguageChanged()
	{
		for (int i = 0; i < BraveInput.m_instances.Count; i++)
		{
			if (BraveInput.m_instances[i] && BraveInput.m_instances[i].ActiveActions != null)
			{
				BraveInput.m_instances[i].ActiveActions.ReinitializeMenuDefaults();
			}
		}
	}

	// Token: 0x06000DB3 RID: 3507 RVA: 0x00041764 File Offset: 0x0003F964
	public static void ResetBindingsToDefaults()
	{
		GameManager.Options.playerOneBindingData = string.Empty;
		GameManager.Options.playerOneBindingDataV2 = string.Empty;
		GameManager.Options.playerTwoBindingData = string.Empty;
		GameManager.Options.playerTwoBindingDataV2 = string.Empty;
		BraveInput.DoStartupAssignmentOfControllers(-1);
		for (int i = 0; i < BraveInput.m_instances.Count; i++)
		{
			if (BraveInput.m_instances[i].m_activeGungeonActions != null)
			{
				BraveInput.m_instances[i].m_activeGungeonActions.Destroy();
			}
			BraveInput.m_instances[i].m_activeGungeonActions = null;
			BraveInput.m_instances[i].CheckForActionInitialization();
		}
		BraveInput.SaveBindingInfoToOptions();
	}

	// Token: 0x06000DB4 RID: 3508 RVA: 0x00041820 File Offset: 0x0003FA20
	public static int GetDeviceIndex(InputDevice device)
	{
		int num = -1;
		for (int i = 0; i < InputManager.Devices.Count; i++)
		{
			if (InputManager.Devices[i] == device)
			{
				num = i;
			}
		}
		return num;
	}

	// Token: 0x06000DB5 RID: 3509 RVA: 0x00041860 File Offset: 0x0003FA60
	public static XInputDevice GetXInputDeviceInSlot(int xInputSlot)
	{
		for (int i = 0; i < InputManager.Devices.Count; i++)
		{
			if (InputManager.Devices[i] is XInputDevice)
			{
				XInputDevice xinputDevice = InputManager.Devices[i] as XInputDevice;
				if (xinputDevice.DeviceIndex == xInputSlot)
				{
					return xinputDevice;
				}
			}
		}
		return null;
	}

	// Token: 0x06000DB6 RID: 3510 RVA: 0x000418C0 File Offset: 0x0003FAC0
	public static void ReassignPlayerPort(int playerID, int portNum)
	{
		GameManager.Options.PlayerIDtoDeviceIndexMap.Remove(playerID);
		GameManager.Options.PlayerIDtoDeviceIndexMap.Add(playerID, portNum);
		for (int i = 0; i < BraveInput.m_instances.Count; i++)
		{
			if (BraveInput.m_instances[i].m_activeGungeonActions != null)
			{
				BraveInput.m_instances[i].m_activeGungeonActions.Destroy();
			}
			BraveInput.m_instances[i].m_activeGungeonActions = null;
		}
		InControlInputAdapter.SkipInputForRestOfFrame = true;
	}

	// Token: 0x1700031B RID: 795
	// (get) Token: 0x06000DB7 RID: 3511 RVA: 0x0004194C File Offset: 0x0003FB4C
	public static BraveInput PlayerlessInstance
	{
		get
		{
			if (BraveInput.m_instances == null || BraveInput.m_instances.Count < 1)
			{
				return null;
			}
			return BraveInput.m_instances[0];
		}
	}

	// Token: 0x1700031C RID: 796
	// (get) Token: 0x06000DB8 RID: 3512 RVA: 0x00041978 File Offset: 0x0003FB78
	public static BraveInput PrimaryPlayerInstance
	{
		get
		{
			if (BraveInput.m_instances == null || BraveInput.m_instances.Count < 1)
			{
				return null;
			}
			if (GameManager.Instance.PrimaryPlayer == null)
			{
				return BraveInput.m_instances[0];
			}
			return BraveInput.GetInstanceForPlayer(GameManager.Instance.PrimaryPlayer.PlayerIDX);
		}
	}

	// Token: 0x1700031D RID: 797
	// (get) Token: 0x06000DB9 RID: 3513 RVA: 0x000419D8 File Offset: 0x0003FBD8
	public static BraveInput SecondaryPlayerInstance
	{
		get
		{
			if (BraveInput.m_instances == null || BraveInput.m_instances.Count < 2)
			{
				return null;
			}
			if (GameManager.Instance.CurrentGameType == GameManager.GameType.SINGLE_PLAYER)
			{
				return null;
			}
			if (GameManager.Instance.SecondaryPlayer == null)
			{
				return null;
			}
			return BraveInput.GetInstanceForPlayer(GameManager.Instance.SecondaryPlayer.PlayerIDX);
		}
	}

	// Token: 0x06000DBA RID: 3514 RVA: 0x00041A40 File Offset: 0x0003FC40
	public static bool HasInstanceForPlayer(int id)
	{
		return BraveInput.m_instances.ContainsKey(id) && BraveInput.m_instances[id] != null && BraveInput.m_instances[id];
	}

	// Token: 0x06000DBB RID: 3515 RVA: 0x00041A7C File Offset: 0x0003FC7C
	public static BraveInput GetInstanceForPlayer(int id)
	{
		if (BraveInput.m_instances.ContainsKey(id) && (BraveInput.m_instances[id] == null || !BraveInput.m_instances[id]))
		{
			BraveInput.m_instances.Remove(id);
		}
		if (!BraveInput.m_instances.ContainsKey(id))
		{
			if (BraveInput.m_instances.ContainsKey(0))
			{
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(BraveInput.m_instances[0].gameObject);
				BraveInput component = gameObject.GetComponent<BraveInput>();
				component.m_playerID = id;
				BraveInput.m_instances.Add(id, component);
			}
			else
			{
				Debug.LogError("Player " + id + " is attempting to get a BraveInput instance, and player 0's doesn't exist.");
			}
		}
		if (!BraveInput.m_instances.ContainsKey(id))
		{
			return null;
		}
		return BraveInput.m_instances[id];
	}

	// Token: 0x1700031E RID: 798
	// (get) Token: 0x06000DBC RID: 3516 RVA: 0x00041B5C File Offset: 0x0003FD5C
	// (set) Token: 0x06000DBD RID: 3517 RVA: 0x00041B70 File Offset: 0x0003FD70
	public static BraveInput.AutoAim AutoAimMode
	{
		get
		{
			return BraveInput.m_instances[0].autoAimMode;
		}
		set
		{
			BraveInput.m_instances[0].autoAimMode = value;
		}
	}

	// Token: 0x1700031F RID: 799
	// (get) Token: 0x06000DBE RID: 3518 RVA: 0x00041B84 File Offset: 0x0003FD84
	// (set) Token: 0x06000DBF RID: 3519 RVA: 0x00041B98 File Offset: 0x0003FD98
	public static bool ShowCursor
	{
		get
		{
			return BraveInput.m_instances[0].showCursor;
		}
		set
		{
			BraveInput.m_instances[0].showCursor = value;
		}
	}

	// Token: 0x17000320 RID: 800
	// (get) Token: 0x06000DC0 RID: 3520 RVA: 0x00041BAC File Offset: 0x0003FDAC
	public static MagnetAngles MagnetAngles
	{
		get
		{
			return BraveInput.m_instances[0].magnetAngles;
		}
	}

	// Token: 0x17000321 RID: 801
	// (get) Token: 0x06000DC1 RID: 3521 RVA: 0x00041BC0 File Offset: 0x0003FDC0
	public static float ControllerAutoAimDegrees
	{
		get
		{
			float num = BraveInput.m_instances[0].controllerAutoAimDegrees;
			if (GameManager.Options != null)
			{
				num *= GameManager.Options.controllerAimAssistMultiplier;
			}
			return num;
		}
	}

	// Token: 0x17000322 RID: 802
	// (get) Token: 0x06000DC2 RID: 3522 RVA: 0x00041BF8 File Offset: 0x0003FDF8
	public static float ControllerSuperAutoAimDegrees
	{
		get
		{
			float num = BraveInput.m_instances[0].controllerSuperAutoAimDegrees;
			if (GameManager.Options != null)
			{
				num *= GameManager.Options.controllerAimAssistMultiplier;
			}
			return num;
		}
	}

	// Token: 0x17000323 RID: 803
	// (get) Token: 0x06000DC3 RID: 3523 RVA: 0x00041C30 File Offset: 0x0003FE30
	public static float ControllerFakeSemiAutoCooldown
	{
		get
		{
			return BraveInput.m_instances[0].controllerFakeSemiAutoCooldown;
		}
	}

	// Token: 0x17000324 RID: 804
	// (get) Token: 0x06000DC4 RID: 3524 RVA: 0x00041C44 File Offset: 0x0003FE44
	public GungeonActions ActiveActions
	{
		get
		{
			return this.m_activeGungeonActions;
		}
	}

	// Token: 0x06000DC5 RID: 3525 RVA: 0x00041C4C File Offset: 0x0003FE4C
	public void Awake()
	{
		if (!BraveInput.m_instances.ContainsKey(0))
		{
			BraveInput.m_instances.Add(0, this);
		}
	}

	// Token: 0x06000DC6 RID: 3526 RVA: 0x00041C6C File Offset: 0x0003FE6C
	public void OnDestroy()
	{
		if (this.m_activeGungeonActions != null)
		{
			this.m_activeGungeonActions.Destroy();
			this.m_activeGungeonActions = null;
		}
		if (BraveInput.m_instances.ContainsValue(this))
		{
			BraveInput.m_instances.Remove(this.m_playerID);
		}
	}

	// Token: 0x06000DC7 RID: 3527 RVA: 0x00041CAC File Offset: 0x0003FEAC
	private void AssignActionsDevice()
	{
		if (GameManager.PreventGameManagerExistence || GameManager.Instance.AllPlayers.Length < 2)
		{
			this.m_activeGungeonActions.Device = InputManager.ActiveDevice;
		}
		else
		{
			this.m_activeGungeonActions.Device = InputManager.GetActiveDeviceForPlayer(this.m_playerID);
			if (this.m_playerID != 0 && this.m_activeGungeonActions.Device == InputManager.GetActiveDeviceForPlayer(0))
			{
				this.m_activeGungeonActions.ForceDisable = true;
			}
		}
	}

	// Token: 0x06000DC8 RID: 3528 RVA: 0x00041D30 File Offset: 0x0003FF30
	private static void TryLoadBindings(int playerNum, GungeonActions actions)
	{
		string text;
		string text2;
		if (playerNum == 0)
		{
			text = GameManager.Options.playerOneBindingData;
			text2 = GameManager.Options.playerOneBindingDataV2;
		}
		else
		{
			if (playerNum != 1)
			{
				return;
			}
			text = GameManager.Options.playerTwoBindingData;
			text2 = GameManager.Options.playerTwoBindingDataV2;
		}
		if (!string.IsNullOrEmpty(text2))
		{
			actions.Load(text2, false);
		}
		else if (!string.IsNullOrEmpty(text))
		{
			actions.Load(text, true);
		}
		actions.PostProcessAdditionalBlankControls(playerNum);
	}

	// Token: 0x06000DC9 RID: 3529 RVA: 0x00041DB4 File Offset: 0x0003FFB4
	public void CheckForActionInitialization()
	{
		if (this.m_activeGungeonActions == null)
		{
			this.m_activeGungeonActions = new GungeonActions();
			this.AssignActionsDevice();
			this.m_activeGungeonActions.InitializeDefaults();
			if (GameManager.PreventGameManagerExistence || (GameManager.Instance.PrimaryPlayer == null && this.m_playerID == 0) || this.m_playerID == GameManager.Instance.PrimaryPlayer.PlayerIDX)
			{
				BraveInput.TryLoadBindings(0, this.ActiveActions);
			}
			else
			{
				BraveInput.TryLoadBindings(1, this.ActiveActions);
			}
			if (!GameManager.PreventGameManagerExistence && GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER)
			{
				if (this.m_playerID == 0 && BraveInput.GetInstanceForPlayer(GameManager.Instance.SecondaryPlayer.PlayerIDX).m_activeGungeonActions == null)
				{
					BraveInput.GetInstanceForPlayer(GameManager.Instance.SecondaryPlayer.PlayerIDX).CheckForActionInitialization();
				}
				if (this.m_activeGungeonActions.Device == null)
				{
					this.m_activeGungeonActions.IgnoreBindingsOfType(BindingSourceType.DeviceBindingSource);
				}
				else if (this.m_playerID == GameManager.Instance.PrimaryPlayer.PlayerIDX)
				{
					if (BraveInput.GetInstanceForPlayer(GameManager.Instance.SecondaryPlayer.PlayerIDX).m_activeGungeonActions.Device == null)
					{
						this.m_activeGungeonActions.IgnoreBindingsOfType(BindingSourceType.KeyBindingSource);
						this.m_activeGungeonActions.IgnoreBindingsOfType(BindingSourceType.MouseBindingSource);
					}
				}
				else
				{
					this.m_activeGungeonActions.IgnoreBindingsOfType(BindingSourceType.KeyBindingSource);
					this.m_activeGungeonActions.IgnoreBindingsOfType(BindingSourceType.MouseBindingSource);
				}
			}
		}
		this.AssignActionsDevice();
	}

	// Token: 0x06000DCA RID: 3530 RVA: 0x00041F40 File Offset: 0x00040140
	public void Update()
	{
		if (GameManager.Options.PlayerIDtoDeviceIndexMap == null || GameManager.Options.PlayerIDtoDeviceIndexMap.Count == 0)
		{
			BraveInput.DoStartupAssignmentOfControllers(-1);
		}
		this.CheckForActionInitialization();
		LinkedListNode<BraveInput.PressAction> linkedListNode = this.m_pressActions.First;
		while (linkedListNode != null)
		{
			BraveInput.PressAction value = linkedListNode.Value;
			value.Timer += GameManager.INVARIANT_DELTA_TIME;
			if (value.Timer >= value.Buffer)
			{
				LinkedListNode<BraveInput.PressAction> next = linkedListNode.Next;
				BraveInput.PressAction.Pool.Free(ref value);
				this.m_pressActions.Remove(linkedListNode, true);
				linkedListNode = next;
			}
			else
			{
				linkedListNode = linkedListNode.Next;
			}
		}
		for (int i = 0; i < this.PressActions.Length; i++)
		{
			if (this.m_activeGungeonActions.GetActionFromType(this.PressActions[i].Control).WasPressed)
			{
				BraveInput.PressAction pressAction = BraveInput.PressAction.Pool.Allocate();
				pressAction.SetAll(this.PressActions[i]);
				this.m_pressActions.AddLast(pressAction);
			}
		}
		LinkedListNode<BraveInput.HoldAction> linkedListNode2 = this.m_holdActions.First;
		while (linkedListNode2 != null)
		{
			BraveInput.HoldAction value2 = linkedListNode2.Value;
			value2.DownTimer += GameManager.INVARIANT_DELTA_TIME;
			if (!value2.Held)
			{
				value2.UpTimer += GameManager.INVARIANT_DELTA_TIME;
			}
			else if (!this.m_activeGungeonActions.GetActionFromType(value2.Control).IsPressed)
			{
				value2.Held = false;
			}
			LinkedListNode<BraveInput.HoldAction> linkedListNode3 = linkedListNode2;
			linkedListNode2 = linkedListNode2.Next;
			if (!value2.Held)
			{
				if (value2.ConsumedDown && value2.ConsumedUp)
				{
					BraveInput.HoldAction value3 = linkedListNode3.Value;
					this.m_holdActions.Remove(linkedListNode3, true);
					BraveInput.HoldAction.Pool.Free(ref value3);
				}
				else if (!value2.ConsumedDown && value2.UpTimer >= value2.Buffer)
				{
					BraveInput.HoldAction value4 = linkedListNode3.Value;
					this.m_holdActions.Remove(linkedListNode3, true);
					BraveInput.HoldAction.Pool.Free(ref value4);
				}
			}
		}
		for (int j = 0; j < this.HoldActions.Length; j++)
		{
			if (this.m_activeGungeonActions.GetActionFromType(this.HoldActions[j].Control).WasPressed)
			{
				BraveInput.HoldAction holdAction = BraveInput.HoldAction.Pool.Allocate();
				holdAction.SetAll(this.HoldActions[j]);
				this.m_holdActions.AddLast(holdAction);
			}
		}
	}

	// Token: 0x06000DCB RID: 3531 RVA: 0x000421D0 File Offset: 0x000403D0
	public void LateUpdate()
	{
		if (!GameManager.Options.RumbleEnabled || GameManager.Instance.IsLoadingLevel)
		{
			this.SetVibration(0f, 0f);
			this.m_currentVibrations.Clear();
		}
		else if (GameManager.Instance.IsPaused && !BraveInput.AllowPausedRumble)
		{
			this.SetVibration(0f, 0f);
		}
		else
		{
			float num = 0f;
			float num2 = 0f;
			float num3 = Vibration.ConvertFromShakeMagnitude(GameManager.Instance.MainCameraController.ScreenShakeVibration);
			num = Mathf.Max(num3, num);
			num2 = Mathf.Max(num3, num2);
			num = Mathf.Max(this.m_sustainedLargeVibration, num);
			num2 = Mathf.Max(this.m_sustainedSmallVibration, num2);
			for (int i = this.m_currentVibrations.Count - 1; i >= 0; i--)
			{
				BraveInput.TimedVibration timedVibration = this.m_currentVibrations[i];
				num = Mathf.Max(timedVibration.largeMotor, num);
				num2 = Mathf.Max(timedVibration.smallMotor, num2);
				if (GameManager.Instance.IsPaused && BraveInput.AllowPausedRumble)
				{
					timedVibration.timer -= GameManager.INVARIANT_DELTA_TIME;
				}
				else
				{
					timedVibration.timer -= BraveTime.DeltaTime;
				}
				if (timedVibration.timer < 0f)
				{
					this.m_currentVibrations.RemoveAt(i);
				}
			}
			this.SetVibration(num, num2);
		}
		GameManager.Instance.MainCameraController.MarkScreenShakeVibrationDirty();
		this.m_sustainedLargeVibration = 0f;
		this.m_sustainedSmallVibration = 0f;
	}

	// Token: 0x06000DCC RID: 3532 RVA: 0x0004236C File Offset: 0x0004056C
	public BindingSourceType GetLastInputType()
	{
		if (this.m_activeGungeonActions == null)
		{
			return BindingSourceType.None;
		}
		return this.m_activeGungeonActions.LastInputType;
	}

	// Token: 0x06000DCD RID: 3533 RVA: 0x00042388 File Offset: 0x00040588
	public bool IsKeyboardAndMouse(bool includeNone = false)
	{
		return this.m_activeGungeonActions == null || (includeNone && this.m_activeGungeonActions.LastInputType == BindingSourceType.None) || this.m_activeGungeonActions.LastInputType == BindingSourceType.KeyBindingSource || this.m_activeGungeonActions.LastInputType == BindingSourceType.MouseBindingSource;
	}

	// Token: 0x06000DCE RID: 3534 RVA: 0x000423DC File Offset: 0x000405DC
	public bool HasMouse()
	{
		return this.m_activeGungeonActions == null || this.m_activeGungeonActions.LastInputType == BindingSourceType.KeyBindingSource || this.m_activeGungeonActions.LastInputType == BindingSourceType.MouseBindingSource;
	}

	// Token: 0x17000325 RID: 805
	// (get) Token: 0x06000DCF RID: 3535 RVA: 0x00042410 File Offset: 0x00040610
	public Vector2 MousePosition
	{
		get
		{
			return Input.mousePosition.XY();
		}
	}

	// Token: 0x06000DD0 RID: 3536 RVA: 0x0004241C File Offset: 0x0004061C
	public static void FlushAll()
	{
		for (int i = 0; i < BraveInput.m_instances.Count; i++)
		{
			BraveInput.m_instances[i].Flush();
		}
	}

	// Token: 0x06000DD1 RID: 3537 RVA: 0x00042454 File Offset: 0x00040654
	public void Flush()
	{
		while (this.m_pressActions.Count > 0)
		{
			BraveInput.PressAction value = this.m_pressActions.First.Value;
			BraveInput.PressAction.Pool.Free(ref value);
			this.m_pressActions.RemoveFirst();
		}
		while (this.m_holdActions.Count > 0)
		{
			BraveInput.HoldAction value2 = this.m_holdActions.First.Value;
			BraveInput.HoldAction.Pool.Free(ref value2);
			this.m_holdActions.RemoveFirst();
		}
	}

	// Token: 0x06000DD2 RID: 3538 RVA: 0x000424E0 File Offset: 0x000406E0
	public void DoVibration(Vibration.Time time, Vibration.Strength strength)
	{
		this.m_currentVibrations.Add(new BraveInput.TimedVibration(Vibration.ConvertTime(time), Vibration.ConvertStrength(strength)));
	}

	// Token: 0x06000DD3 RID: 3539 RVA: 0x00042500 File Offset: 0x00040700
	public void DoVibration(float time, Vibration.Strength strength)
	{
		this.m_currentVibrations.Add(new BraveInput.TimedVibration(time, Vibration.ConvertStrength(strength)));
	}

	// Token: 0x06000DD4 RID: 3540 RVA: 0x0004251C File Offset: 0x0004071C
	public void DoVibration(Vibration.Time time, Vibration.Strength largeMotor, Vibration.Strength smallMotor)
	{
		this.m_currentVibrations.Add(new BraveInput.TimedVibration(Vibration.ConvertTime(time), Vibration.ConvertStrength(largeMotor), Vibration.ConvertStrength(smallMotor)));
	}

	// Token: 0x06000DD5 RID: 3541 RVA: 0x00042540 File Offset: 0x00040740
	public void DoScreenShakeVibration(float time, float magnitude)
	{
		this.m_currentVibrations.Add(new BraveInput.TimedVibration(time, Vibration.ConvertFromShakeMagnitude(magnitude)));
	}

	// Token: 0x06000DD6 RID: 3542 RVA: 0x0004255C File Offset: 0x0004075C
	public void DoSustainedVibration(Vibration.Strength strength)
	{
		this.m_sustainedLargeVibration = Mathf.Max(this.m_sustainedLargeVibration, Vibration.ConvertStrength(strength));
	}

	// Token: 0x06000DD7 RID: 3543 RVA: 0x00042578 File Offset: 0x00040778
	public void DoSustainedVibration(Vibration.Strength largeMotor, Vibration.Strength smallMotor)
	{
		this.m_sustainedLargeVibration = Mathf.Max(this.m_sustainedLargeVibration, Vibration.ConvertStrength(largeMotor));
		this.m_sustainedSmallVibration = Mathf.Max(this.m_sustainedSmallVibration, Vibration.ConvertStrength(smallMotor));
	}

	// Token: 0x06000DD8 RID: 3544 RVA: 0x000425A8 File Offset: 0x000407A8
	public static void DoVibrationForAllPlayers(Vibration.Time time, Vibration.Strength strength)
	{
		for (int i = 0; i < BraveInput.m_instances.Count; i++)
		{
			if (BraveInput.m_instances[i] != null)
			{
				BraveInput.m_instances[i].DoVibration(time, strength);
			}
		}
	}

	// Token: 0x06000DD9 RID: 3545 RVA: 0x000425F8 File Offset: 0x000407F8
	public static void DoVibrationForAllPlayers(Vibration.Time time, Vibration.Strength largeMotor, Vibration.Strength smallMotor)
	{
		for (int i = 0; i < BraveInput.m_instances.Count; i++)
		{
			if (BraveInput.m_instances[i] != null)
			{
				BraveInput.m_instances[i].DoVibration(time, largeMotor, smallMotor);
			}
		}
	}

	// Token: 0x06000DDA RID: 3546 RVA: 0x0004264C File Offset: 0x0004084C
	public static void DoSustainedScreenShakeVibration(float magnitude)
	{
		for (int i = 0; i < BraveInput.m_instances.Count; i++)
		{
			if (BraveInput.m_instances[i] != null)
			{
				BraveInput.m_instances[i].m_sustainedLargeVibration = Mathf.Max(BraveInput.m_instances[i].m_sustainedLargeVibration, Vibration.ConvertFromShakeMagnitude(magnitude));
				BraveInput.m_instances[i].m_sustainedSmallVibration = Mathf.Max(BraveInput.m_instances[i].m_sustainedSmallVibration, Vibration.ConvertFromShakeMagnitude(magnitude));
			}
		}
	}

	// Token: 0x06000DDB RID: 3547 RVA: 0x000426E0 File Offset: 0x000408E0
	private void SetVibration(float largeMotor, float smallMotor)
	{
		if (this.m_activeGungeonActions != null && this.m_activeGungeonActions.Device != null)
		{
			this.m_activeGungeonActions.Device.Vibrate(largeMotor, smallMotor);
		}
	}

	// Token: 0x06000DDC RID: 3548 RVA: 0x00042710 File Offset: 0x00040910
	private bool CheckBufferedActionsForControlType(BraveInput.BufferedInput[] bufferedInputs, GungeonActions.GungeonActionType controlType)
	{
		for (int i = 0; i < bufferedInputs.Length; i++)
		{
			if (bufferedInputs[i].Control == controlType)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06000DDD RID: 3549 RVA: 0x00042744 File Offset: 0x00040944
	private bool CheckPressActionsForControlType(GungeonActions.GungeonActionType controlType)
	{
		for (LinkedListNode<BraveInput.PressAction> linkedListNode = this.m_pressActions.First; linkedListNode != null; linkedListNode = linkedListNode.Next)
		{
			if (linkedListNode.Value.Control == controlType)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06000DDE RID: 3550 RVA: 0x00042784 File Offset: 0x00040984
	private BraveInput.PressAction GetPressActionForControlType(GungeonActions.GungeonActionType controlType)
	{
		for (LinkedListNode<BraveInput.PressAction> linkedListNode = this.m_pressActions.First; linkedListNode != null; linkedListNode = linkedListNode.Next)
		{
			if (linkedListNode.Value.Control == controlType)
			{
				return linkedListNode.Value;
			}
		}
		return null;
	}

	// Token: 0x06000DDF RID: 3551 RVA: 0x000427C8 File Offset: 0x000409C8
	private BraveInput.HoldAction GetHoldActionForControlType(GungeonActions.GungeonActionType controlType)
	{
		for (LinkedListNode<BraveInput.HoldAction> linkedListNode = this.m_holdActions.First; linkedListNode != null; linkedListNode = linkedListNode.Next)
		{
			if (linkedListNode.Value.Control == controlType)
			{
				return linkedListNode.Value;
			}
		}
		return null;
	}

	// Token: 0x06000DE0 RID: 3552 RVA: 0x0004280C File Offset: 0x00040A0C
	public bool GetButtonDown(GungeonActions.GungeonActionType controlType)
	{
		if (this.CheckBufferedActionsForControlType(this.PressActions, controlType))
		{
			return this.CheckPressActionsForControlType(controlType);
		}
		if (this.CheckBufferedActionsForControlType(this.HoldActions, controlType))
		{
			BraveInput.HoldAction holdActionForControlType = this.GetHoldActionForControlType(controlType);
			return holdActionForControlType != null && !holdActionForControlType.ConsumedDown;
		}
		Debug.LogError(string.Format("BraveInput.GetButtonDown(): {0} isn't registered with the BraveInput object", controlType));
		return false;
	}

	// Token: 0x06000DE1 RID: 3553 RVA: 0x00042878 File Offset: 0x00040A78
	public void ConsumeButtonDown(GungeonActions.GungeonActionType controlType)
	{
		if (this.CheckBufferedActionsForControlType(this.PressActions, controlType))
		{
			BraveInput.PressAction pressActionForControlType = this.GetPressActionForControlType(controlType);
			if (pressActionForControlType != null)
			{
				this.m_pressActions.Remove(pressActionForControlType);
				BraveInput.PressAction.Pool.Free(ref pressActionForControlType);
				return;
			}
			Debug.LogError(string.Format("BraveInput.ConsumeButtonDown(): No action for {0} was found", controlType.ToString()));
			return;
		}
		else
		{
			if (!this.CheckBufferedActionsForControlType(this.HoldActions, controlType))
			{
				Debug.LogError(string.Format("BraveInput.ConsumeButtonDown(): {0} isn't registered with the BraveInput object", controlType.ToString()));
				return;
			}
			BraveInput.HoldAction holdActionForControlType = this.GetHoldActionForControlType(controlType);
			if (holdActionForControlType != null)
			{
				holdActionForControlType.ConsumedDown = true;
				return;
			}
			if (!MemoryTester.HasInstance)
			{
				Debug.LogError(string.Format("BraveInput.ConsumeButtonDown(): No action for {0} was found", controlType.ToString()));
			}
			return;
		}
	}

	// Token: 0x06000DE2 RID: 3554 RVA: 0x00042948 File Offset: 0x00040B48
	public bool GetButton(GungeonActions.GungeonActionType controlType)
	{
		if (this.CheckBufferedActionsForControlType(this.HoldActions, controlType))
		{
			BraveInput.HoldAction holdActionForControlType = this.GetHoldActionForControlType(controlType);
			return holdActionForControlType != null && holdActionForControlType.ConsumedDown && holdActionForControlType.Held;
		}
		if (!MemoryTester.HasInstance)
		{
			Debug.LogError(string.Format("BraveInput.GetButtonDown(): {0} isn't a registered hold action with the BraveInput object", controlType.ToString()));
		}
		return false;
	}

	// Token: 0x06000DE3 RID: 3555 RVA: 0x000429B4 File Offset: 0x00040BB4
	public bool GetButtonUp(GungeonActions.GungeonActionType controlType)
	{
		if (this.CheckBufferedActionsForControlType(this.HoldActions, controlType))
		{
			BraveInput.HoldAction holdActionForControlType = this.GetHoldActionForControlType(controlType);
			return holdActionForControlType != null && (!holdActionForControlType.Held && holdActionForControlType.ConsumedDown) && !holdActionForControlType.ConsumedUp;
		}
		if (!MemoryTester.HasInstance)
		{
			Debug.LogError(string.Format("BraveInput.GetButtonDown(): {0} isn't a registered hold action with the BraveInput object", controlType.ToString()));
		}
		return false;
	}

	// Token: 0x06000DE4 RID: 3556 RVA: 0x00042A30 File Offset: 0x00040C30
	public void ConsumeButtonUp(GungeonActions.GungeonActionType controlType)
	{
		if (!this.CheckBufferedActionsForControlType(this.HoldActions, controlType))
		{
			Debug.LogError(string.Format("BraveInput.ConsumeButtonUp(): {0} isn't registered with the BraveInput object", controlType.ToString()));
			return;
		}
		BraveInput.HoldAction holdActionForControlType = this.GetHoldActionForControlType(controlType);
		if (holdActionForControlType != null)
		{
			holdActionForControlType.ConsumedUp = true;
			return;
		}
		if (!MemoryTester.HasInstance)
		{
			Debug.LogError(string.Format("BraveInput.ConsumeButtonUp(): No action for {0} was found", controlType.ToString()));
		}
	}

	// Token: 0x06000DE5 RID: 3557 RVA: 0x00042AA8 File Offset: 0x00040CA8
	public static void ConsumeAllAcrossInstances(GungeonActions.GungeonActionType controlType)
	{
		for (int i = 0; i < BraveInput.m_instances.Count; i++)
		{
			BraveInput.m_instances[i].ConsumeAll(controlType);
		}
	}

	// Token: 0x06000DE6 RID: 3558 RVA: 0x00042AE4 File Offset: 0x00040CE4
	public void ConsumeAll(GungeonActions.GungeonActionType controlType)
	{
		LinkedListNode<BraveInput.PressAction> linkedListNode = this.m_pressActions.First;
		while (linkedListNode != null)
		{
			LinkedListNode<BraveInput.PressAction> linkedListNode2 = linkedListNode;
			linkedListNode = linkedListNode.Next;
			if (linkedListNode2.Value.Control == controlType)
			{
				BraveInput.PressAction value = linkedListNode2.Value;
				this.m_pressActions.Remove(linkedListNode2, true);
				BraveInput.PressAction.Pool.Free(ref value);
			}
		}
		LinkedListNode<BraveInput.HoldAction> linkedListNode3 = this.m_holdActions.First;
		while (linkedListNode3 != null)
		{
			LinkedListNode<BraveInput.HoldAction> linkedListNode4 = linkedListNode3;
			linkedListNode3 = linkedListNode3.Next;
			if (linkedListNode4.Value.Control == controlType && !linkedListNode4.Value.ConsumedDown)
			{
				BraveInput.HoldAction value2 = linkedListNode4.Value;
				this.m_holdActions.Remove(linkedListNode4, true);
				BraveInput.HoldAction.Pool.Free(ref value2);
			}
		}
	}

	// Token: 0x17000326 RID: 806
	// (get) Token: 0x06000DE7 RID: 3559 RVA: 0x00042BAC File Offset: 0x00040DAC
	public static GameOptions.ControllerSymbology PlayerOneCurrentSymbology
	{
		get
		{
			return BraveInput.GetCurrentSymbology(0);
		}
	}

	// Token: 0x17000327 RID: 807
	// (get) Token: 0x06000DE8 RID: 3560 RVA: 0x00042BB4 File Offset: 0x00040DB4
	public static GameOptions.ControllerSymbology PlayerTwoCurrentSymbology
	{
		get
		{
			return BraveInput.GetCurrentSymbology(1);
		}
	}

	// Token: 0x17000328 RID: 808
	// (get) Token: 0x06000DE9 RID: 3561 RVA: 0x00042BBC File Offset: 0x00040DBC
	public bool MenuInteractPressed
	{
		get
		{
			return this.ActiveActions != null && (this.ActiveActions.InteractAction.WasPressed || this.ActiveActions.MenuSelectAction.WasPressed);
		}
	}

	// Token: 0x06000DEA RID: 3562 RVA: 0x00042BF4 File Offset: 0x00040DF4
	public bool WasAdvanceDialoguePressed(out bool suppressThisClick)
	{
		suppressThisClick = false;
		if (this.MenuInteractPressed)
		{
			return true;
		}
		if (!this.IsKeyboardAndMouse(false))
		{
			return false;
		}
		if (Input.GetMouseButtonDown(0))
		{
			suppressThisClick = true;
			return true;
		}
		return Input.GetKeyDown(KeyCode.Return);
	}

	// Token: 0x06000DEB RID: 3563 RVA: 0x00042C2C File Offset: 0x00040E2C
	public bool WasAdvanceDialoguePressed()
	{
		return this.MenuInteractPressed || (this.IsKeyboardAndMouse(false) && (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Return)));
	}

	// Token: 0x06000DEC RID: 3564 RVA: 0x00042C60 File Offset: 0x00040E60
	public static GameOptions.ControllerSymbology GetCurrentSymbology(int id)
	{
		GameOptions.ControllerSymbology controllerSymbology = ((id != 0) ? GameManager.Options.PlayerTwoPreferredSymbology : GameManager.Options.PlayerOnePreferredSymbology);
		if (controllerSymbology == GameOptions.ControllerSymbology.AutoDetect)
		{
			BraveInput instanceForPlayer = BraveInput.GetInstanceForPlayer(id);
			if (instanceForPlayer != null && !instanceForPlayer.IsKeyboardAndMouse(false))
			{
				InputDevice device = instanceForPlayer.ActiveActions.Device;
				if (device != null)
				{
					controllerSymbology = device.ControllerSymbology;
				}
			}
		}
		if (controllerSymbology == GameOptions.ControllerSymbology.AutoDetect)
		{
			controllerSymbology = GameOptions.ControllerSymbology.Xbox;
		}
		return controllerSymbology;
	}

	// Token: 0x06000DED RID: 3565 RVA: 0x00042CD8 File Offset: 0x00040ED8
	public static bool WasSelectPressed(InputDevice device = null)
	{
		if (device == null)
		{
			device = InputManager.ActiveDevice;
		}
		return device.Action1.WasPressed || (GameManager.HasInstance && GameManager.Options.allowUnknownControllers && BraveInput.GetInstanceForPlayer(0).ActiveActions.MenuSelectAction.WasPressed);
	}

	// Token: 0x06000DEE RID: 3566 RVA: 0x00042D3C File Offset: 0x00040F3C
	public static bool WasCancelPressed(InputDevice device = null)
	{
		if (device == null)
		{
			device = InputManager.ActiveDevice;
		}
		return device.Action2.WasPressed || (GameManager.HasInstance && GameManager.Options.allowUnknownControllers && BraveInput.GetInstanceForPlayer(0).ActiveActions.CancelAction.WasPressed);
	}

	// Token: 0x04000E03 RID: 3587
	public static bool AllowPausedRumble = false;

	// Token: 0x04000E04 RID: 3588
	public BraveInput.AutoAim autoAimMode;

	// Token: 0x04000E05 RID: 3589
	public bool showCursor;

	// Token: 0x04000E06 RID: 3590
	public MagnetAngles magnetAngles;

	// Token: 0x04000E07 RID: 3591
	public float controllerAutoAimDegrees = 15f;

	// Token: 0x04000E08 RID: 3592
	public float controllerSuperAutoAimDegrees = 25f;

	// Token: 0x04000E09 RID: 3593
	public float controllerFakeSemiAutoCooldown = 0.25f;

	// Token: 0x04000E0A RID: 3594
	[BetterList]
	public BraveInput.BufferedInput[] PressActions;

	// Token: 0x04000E0B RID: 3595
	[BetterList]
	public BraveInput.BufferedInput[] HoldActions;

	// Token: 0x04000E0C RID: 3596
	private GungeonActions m_activeGungeonActions;

	// Token: 0x04000E0D RID: 3597
	[NonSerialized]
	private int m_playerID;

	// Token: 0x04000E0E RID: 3598
	private PooledLinkedList<BraveInput.PressAction> m_pressActions = new PooledLinkedList<BraveInput.PressAction>();

	// Token: 0x04000E0F RID: 3599
	private PooledLinkedList<BraveInput.HoldAction> m_holdActions = new PooledLinkedList<BraveInput.HoldAction>();

	// Token: 0x04000E10 RID: 3600
	private List<BraveInput.TimedVibration> m_currentVibrations = new List<BraveInput.TimedVibration>();

	// Token: 0x04000E11 RID: 3601
	private float m_sustainedLargeVibration;

	// Token: 0x04000E12 RID: 3602
	private float m_sustainedSmallVibration;

	// Token: 0x04000E13 RID: 3603
	private static Dictionary<int, BraveInput> m_instances = new Dictionary<int, BraveInput>();

	// Token: 0x02000361 RID: 865
	public enum AutoAim
	{
		// Token: 0x04000E15 RID: 3605
		AutoAim,
		// Token: 0x04000E16 RID: 3606
		SuperAutoAim
	}

	// Token: 0x02000362 RID: 866
	[Serializable]
	public class BufferedInput
	{
		// Token: 0x04000E17 RID: 3607
		public GungeonActions.GungeonActionType Control;

		// Token: 0x04000E18 RID: 3608
		public float BufferTime = 0.3f;
	}

	// Token: 0x02000363 RID: 867
	public class PressAction
	{
		// Token: 0x06000DF1 RID: 3569 RVA: 0x00042DC8 File Offset: 0x00040FC8
		private PressAction()
		{
		}

		// Token: 0x17000329 RID: 809
		// (get) Token: 0x06000DF2 RID: 3570 RVA: 0x00042DD0 File Offset: 0x00040FD0
		public float Buffer
		{
			get
			{
				return this.m_bufferedInput.BufferTime;
			}
		}

		// Token: 0x1700032A RID: 810
		// (get) Token: 0x06000DF3 RID: 3571 RVA: 0x00042DE0 File Offset: 0x00040FE0
		public GungeonActions.GungeonActionType Control
		{
			get
			{
				return this.m_bufferedInput.Control;
			}
		}

		// Token: 0x06000DF4 RID: 3572 RVA: 0x00042DF0 File Offset: 0x00040FF0
		public void SetAll(BraveInput.BufferedInput bufferedInput)
		{
			this.m_bufferedInput = bufferedInput;
			this.Timer = 0f;
		}

		// Token: 0x06000DF5 RID: 3573 RVA: 0x00042E04 File Offset: 0x00041004
		public static void Cleanup(BraveInput.PressAction pressAction)
		{
			pressAction.m_bufferedInput = null;
		}

		// Token: 0x04000E19 RID: 3609
		public float Timer;

		// Token: 0x04000E1A RID: 3610
		private BraveInput.BufferedInput m_bufferedInput;

		// Token: 0x04000E1B RID: 3611
		public static ObjectPool<BraveInput.PressAction> Pool = new ObjectPool<BraveInput.PressAction>(() => new BraveInput.PressAction(), 10, new ObjectPool<BraveInput.PressAction>.Cleanup(BraveInput.PressAction.Cleanup));
	}

	// Token: 0x02000364 RID: 868
	public class HoldAction
	{
		// Token: 0x06000DF8 RID: 3576 RVA: 0x00042E50 File Offset: 0x00041050
		private HoldAction()
		{
		}

		// Token: 0x1700032B RID: 811
		// (get) Token: 0x06000DF9 RID: 3577 RVA: 0x00042E60 File Offset: 0x00041060
		public float Buffer
		{
			get
			{
				return this.m_bufferedInput.BufferTime;
			}
		}

		// Token: 0x1700032C RID: 812
		// (get) Token: 0x06000DFA RID: 3578 RVA: 0x00042E70 File Offset: 0x00041070
		public GungeonActions.GungeonActionType Control
		{
			get
			{
				return this.m_bufferedInput.Control;
			}
		}

		// Token: 0x06000DFB RID: 3579 RVA: 0x00042E80 File Offset: 0x00041080
		public void SetAll(BraveInput.BufferedInput bufferedInput)
		{
			this.m_bufferedInput = bufferedInput;
			this.DownTimer = 0f;
			this.UpTimer = 0f;
			this.Held = true;
			this.ConsumedDown = false;
			this.ConsumedUp = false;
		}

		// Token: 0x06000DFC RID: 3580 RVA: 0x00042EB4 File Offset: 0x000410B4
		public static void Cleanup(BraveInput.HoldAction holdAction)
		{
			holdAction.m_bufferedInput = null;
		}

		// Token: 0x04000E1D RID: 3613
		public float DownTimer;

		// Token: 0x04000E1E RID: 3614
		public float UpTimer;

		// Token: 0x04000E1F RID: 3615
		public bool Held = true;

		// Token: 0x04000E20 RID: 3616
		public bool ConsumedDown;

		// Token: 0x04000E21 RID: 3617
		public bool ConsumedUp;

		// Token: 0x04000E22 RID: 3618
		private BraveInput.BufferedInput m_bufferedInput;

		// Token: 0x04000E23 RID: 3619
		public static ObjectPool<BraveInput.HoldAction> Pool = new ObjectPool<BraveInput.HoldAction>(() => new BraveInput.HoldAction(), 10, new ObjectPool<BraveInput.HoldAction>.Cleanup(BraveInput.HoldAction.Cleanup));
	}

	// Token: 0x02000365 RID: 869
	private class TimedVibration
	{
		// Token: 0x06000DFF RID: 3583 RVA: 0x00042F00 File Offset: 0x00041100
		public TimedVibration(float timer, float intensity)
		{
			this.timer = timer;
			this.largeMotor = intensity;
			this.smallMotor = intensity;
		}

		// Token: 0x06000E00 RID: 3584 RVA: 0x00042F20 File Offset: 0x00041120
		public TimedVibration(float timer, float largeMotor, float smallMotor)
		{
			this.timer = timer;
			this.largeMotor = largeMotor;
			this.smallMotor = smallMotor;
		}

		// Token: 0x04000E25 RID: 3621
		public float timer;

		// Token: 0x04000E26 RID: 3622
		public float largeMotor;

		// Token: 0x04000E27 RID: 3623
		public float smallMotor;
	}
}
