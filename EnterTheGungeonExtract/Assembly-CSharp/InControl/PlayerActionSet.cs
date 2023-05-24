using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Text;
using UnityEngine;

namespace InControl
{
	// Token: 0x02000699 RID: 1689
	public abstract class PlayerActionSet
	{
		// Token: 0x060026C7 RID: 9927 RVA: 0x000A6A34 File Offset: 0x000A4C34
		protected PlayerActionSet()
		{
			this.Enabled = true;
			this.PreventInputWhileListeningForBinding = true;
			this.Device = null;
			this.IncludeDevices = new List<InputDevice>();
			this.ExcludeDevices = new List<InputDevice>();
			this.Actions = new ReadOnlyCollection<PlayerAction>(this.actions);
			InputManager.AttachPlayerActionSet(this);
		}

		// Token: 0x1700072B RID: 1835
		// (get) Token: 0x060026C8 RID: 9928 RVA: 0x000A6AC0 File Offset: 0x000A4CC0
		// (set) Token: 0x060026C9 RID: 9929 RVA: 0x000A6AC8 File Offset: 0x000A4CC8
		public InputDevice Device { get; set; }

		// Token: 0x1700072C RID: 1836
		// (get) Token: 0x060026CA RID: 9930 RVA: 0x000A6AD4 File Offset: 0x000A4CD4
		// (set) Token: 0x060026CB RID: 9931 RVA: 0x000A6ADC File Offset: 0x000A4CDC
		public bool ForceDisable
		{
			get
			{
				return this.m_forceDisable;
			}
			set
			{
				this.m_forceDisable = value;
				if (this.m_forceDisable)
				{
					for (int i = 0; i < this.actions.Count; i++)
					{
						this.actions[i].CommitWithValue(0f, InputManager.GetCurrentTick(), 0f);
					}
				}
			}
		}

		// Token: 0x1700072D RID: 1837
		// (get) Token: 0x060026CC RID: 9932 RVA: 0x000A6B38 File Offset: 0x000A4D38
		// (set) Token: 0x060026CD RID: 9933 RVA: 0x000A6B40 File Offset: 0x000A4D40
		public List<InputDevice> IncludeDevices { get; private set; }

		// Token: 0x1700072E RID: 1838
		// (get) Token: 0x060026CE RID: 9934 RVA: 0x000A6B4C File Offset: 0x000A4D4C
		// (set) Token: 0x060026CF RID: 9935 RVA: 0x000A6B54 File Offset: 0x000A4D54
		public List<InputDevice> ExcludeDevices { get; private set; }

		// Token: 0x1700072F RID: 1839
		// (get) Token: 0x060026D0 RID: 9936 RVA: 0x000A6B60 File Offset: 0x000A4D60
		// (set) Token: 0x060026D1 RID: 9937 RVA: 0x000A6B68 File Offset: 0x000A4D68
		public ReadOnlyCollection<PlayerAction> Actions { get; private set; }

		// Token: 0x17000730 RID: 1840
		// (get) Token: 0x060026D2 RID: 9938 RVA: 0x000A6B74 File Offset: 0x000A4D74
		// (set) Token: 0x060026D3 RID: 9939 RVA: 0x000A6B7C File Offset: 0x000A4D7C
		public ulong UpdateTick { get; protected set; }

		// Token: 0x14000073 RID: 115
		// (add) Token: 0x060026D4 RID: 9940 RVA: 0x000A6B88 File Offset: 0x000A4D88
		// (remove) Token: 0x060026D5 RID: 9941 RVA: 0x000A6BC0 File Offset: 0x000A4DC0
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action<BindingSourceType> OnLastInputTypeChanged;

		// Token: 0x17000731 RID: 1841
		// (get) Token: 0x060026D6 RID: 9942 RVA: 0x000A6BF8 File Offset: 0x000A4DF8
		// (set) Token: 0x060026D7 RID: 9943 RVA: 0x000A6C00 File Offset: 0x000A4E00
		public bool Enabled { get; set; }

		// Token: 0x17000732 RID: 1842
		// (get) Token: 0x060026D8 RID: 9944 RVA: 0x000A6C0C File Offset: 0x000A4E0C
		// (set) Token: 0x060026D9 RID: 9945 RVA: 0x000A6C14 File Offset: 0x000A4E14
		public bool PreventInputWhileListeningForBinding { get; set; }

		// Token: 0x17000733 RID: 1843
		// (get) Token: 0x060026DA RID: 9946 RVA: 0x000A6C20 File Offset: 0x000A4E20
		// (set) Token: 0x060026DB RID: 9947 RVA: 0x000A6C28 File Offset: 0x000A4E28
		public object UserData { get; set; }

		// Token: 0x060026DC RID: 9948 RVA: 0x000A6C34 File Offset: 0x000A4E34
		public void Destroy()
		{
			this.OnLastInputTypeChanged = null;
			InputManager.DetachPlayerActionSet(this);
		}

		// Token: 0x060026DD RID: 9949 RVA: 0x000A6C44 File Offset: 0x000A4E44
		protected PlayerAction CreatePlayerAction(string name)
		{
			return new PlayerAction(name, this);
		}

		// Token: 0x060026DE RID: 9950 RVA: 0x000A6C50 File Offset: 0x000A4E50
		internal void AddPlayerAction(PlayerAction action)
		{
			action.Device = this.FindActiveDevice();
			if (this.actionsByName.ContainsKey(action.Name))
			{
				throw new InControlException("Action '" + action.Name + "' already exists in this set.");
			}
			this.actions.Add(action);
			this.actionsByName.Add(action.Name, action);
		}

		// Token: 0x060026DF RID: 9951 RVA: 0x000A6CB8 File Offset: 0x000A4EB8
		protected PlayerOneAxisAction CreateOneAxisPlayerAction(PlayerAction negativeAction, PlayerAction positiveAction)
		{
			PlayerOneAxisAction playerOneAxisAction = new PlayerOneAxisAction(negativeAction, positiveAction);
			this.oneAxisActions.Add(playerOneAxisAction);
			return playerOneAxisAction;
		}

		// Token: 0x060026E0 RID: 9952 RVA: 0x000A6CDC File Offset: 0x000A4EDC
		protected PlayerTwoAxisAction CreateTwoAxisPlayerAction(PlayerAction negativeXAction, PlayerAction positiveXAction, PlayerAction negativeYAction, PlayerAction positiveYAction)
		{
			PlayerTwoAxisAction playerTwoAxisAction = new PlayerTwoAxisAction(negativeXAction, positiveXAction, negativeYAction, positiveYAction);
			this.twoAxisActions.Add(playerTwoAxisAction);
			return playerTwoAxisAction;
		}

		// Token: 0x17000734 RID: 1844
		public PlayerAction this[string actionName]
		{
			get
			{
				PlayerAction playerAction;
				if (this.actionsByName.TryGetValue(actionName, out playerAction))
				{
					return playerAction;
				}
				throw new KeyNotFoundException("Action '" + actionName + "' does not exist in this action set.");
			}
		}

		// Token: 0x060026E2 RID: 9954 RVA: 0x000A6D3C File Offset: 0x000A4F3C
		public PlayerAction GetPlayerActionByName(string actionName)
		{
			PlayerAction playerAction;
			if (this.actionsByName.TryGetValue(actionName, out playerAction))
			{
				return playerAction;
			}
			return null;
		}

		// Token: 0x060026E3 RID: 9955 RVA: 0x000A6D60 File Offset: 0x000A4F60
		internal void Update(ulong updateTick, float deltaTime)
		{
			if (this.ForceDisable)
			{
				return;
			}
			InputDevice inputDevice = this.Device ?? this.FindActiveDevice();
			BindingSourceType bindingSourceType = this.LastInputType;
			ulong num = this.LastInputTypeChangedTick;
			InputDeviceClass inputDeviceClass = this.LastDeviceClass;
			InputDeviceStyle inputDeviceStyle = this.LastDeviceStyle;
			int count = this.actions.Count;
			for (int i = 0; i < count; i++)
			{
				PlayerAction playerAction = this.actions[i];
				playerAction.Update(updateTick, deltaTime, inputDevice);
				if (playerAction.UpdateTick > this.UpdateTick)
				{
					this.UpdateTick = playerAction.UpdateTick;
					this.activeDevice = playerAction.ActiveDevice;
				}
				if (playerAction.LastInputTypeChangedTick > num)
				{
					bindingSourceType = playerAction.LastInputType;
					num = playerAction.LastInputTypeChangedTick;
					inputDeviceClass = playerAction.LastDeviceClass;
					inputDeviceStyle = playerAction.LastDeviceStyle;
				}
			}
			int count2 = this.oneAxisActions.Count;
			for (int j = 0; j < count2; j++)
			{
				this.oneAxisActions[j].Update(updateTick, deltaTime);
			}
			int count3 = this.twoAxisActions.Count;
			for (int k = 0; k < count3; k++)
			{
				this.twoAxisActions[k].Update(updateTick, deltaTime);
			}
			if (num > this.LastInputTypeChangedTick)
			{
				bool flag = bindingSourceType != this.LastInputType;
				this.LastInputType = bindingSourceType;
				this.LastInputTypeChangedTick = num;
				this.LastDeviceClass = inputDeviceClass;
				this.LastDeviceStyle = inputDeviceStyle;
				if (this.OnLastInputTypeChanged != null && flag)
				{
					this.OnLastInputTypeChanged(bindingSourceType);
				}
			}
		}

		// Token: 0x060026E4 RID: 9956 RVA: 0x000A6F08 File Offset: 0x000A5108
		public void Reset()
		{
			int count = this.actions.Count;
			for (int i = 0; i < count; i++)
			{
				this.actions[i].ResetBindings();
			}
		}

		// Token: 0x060026E5 RID: 9957 RVA: 0x000A6F44 File Offset: 0x000A5144
		private InputDevice FindActiveDevice()
		{
			bool flag = this.IncludeDevices.Count > 0;
			bool flag2 = this.ExcludeDevices.Count > 0;
			if (flag || flag2)
			{
				InputDevice inputDevice = InputDevice.Null;
				int count = InputManager.Devices.Count;
				for (int i = 0; i < count; i++)
				{
					InputDevice inputDevice2 = InputManager.Devices[i];
					if (inputDevice2 != inputDevice && inputDevice2.LastChangedAfter(inputDevice))
					{
						if (!flag2 || !this.ExcludeDevices.Contains(inputDevice2))
						{
							if (!flag || this.IncludeDevices.Contains(inputDevice2))
							{
								inputDevice = inputDevice2;
							}
						}
					}
				}
				return inputDevice;
			}
			return InputManager.ActiveDevice;
		}

		// Token: 0x060026E6 RID: 9958 RVA: 0x000A7004 File Offset: 0x000A5204
		public void ClearInputState()
		{
			int count = this.actions.Count;
			for (int i = 0; i < count; i++)
			{
				this.actions[i].ClearInputState();
			}
			int count2 = this.oneAxisActions.Count;
			for (int j = 0; j < count2; j++)
			{
				this.oneAxisActions[j].ClearInputState();
			}
			int count3 = this.twoAxisActions.Count;
			for (int k = 0; k < count3; k++)
			{
				this.twoAxisActions[k].ClearInputState();
			}
		}

		// Token: 0x060026E7 RID: 9959 RVA: 0x000A70A8 File Offset: 0x000A52A8
		public bool HasBinding(BindingSource binding)
		{
			if (binding == null)
			{
				return false;
			}
			int count = this.actions.Count;
			for (int i = 0; i < count; i++)
			{
				if (this.actions[i].HasBinding(binding))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060026E8 RID: 9960 RVA: 0x000A70FC File Offset: 0x000A52FC
		public void RemoveBinding(BindingSource binding)
		{
			if (binding == null)
			{
				return;
			}
			int count = this.actions.Count;
			for (int i = 0; i < count; i++)
			{
				this.actions[i].RemoveBinding(binding);
			}
		}

		// Token: 0x17000735 RID: 1845
		// (get) Token: 0x060026E9 RID: 9961 RVA: 0x000A7148 File Offset: 0x000A5348
		public bool IsListeningForBinding
		{
			get
			{
				return this.listenWithAction != null;
			}
		}

		// Token: 0x17000736 RID: 1846
		// (get) Token: 0x060026EA RID: 9962 RVA: 0x000A7158 File Offset: 0x000A5358
		// (set) Token: 0x060026EB RID: 9963 RVA: 0x000A7160 File Offset: 0x000A5360
		public BindingListenOptions ListenOptions
		{
			get
			{
				return this.listenOptions;
			}
			set
			{
				this.listenOptions = value ?? new BindingListenOptions();
			}
		}

		// Token: 0x17000737 RID: 1847
		// (get) Token: 0x060026EC RID: 9964 RVA: 0x000A7178 File Offset: 0x000A5378
		public InputDevice ActiveDevice
		{
			get
			{
				return (this.activeDevice != null) ? this.activeDevice : InputDevice.Null;
			}
		}

		// Token: 0x060026ED RID: 9965 RVA: 0x000A7198 File Offset: 0x000A5398
		public string Save()
		{
			string text;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				using (BinaryWriter binaryWriter = new BinaryWriter(memoryStream, Encoding.UTF8))
				{
					binaryWriter.Write(66);
					binaryWriter.Write(73);
					binaryWriter.Write(78);
					binaryWriter.Write(68);
					binaryWriter.Write(2);
					int count = this.actions.Count;
					binaryWriter.Write(count);
					for (int i = 0; i < count; i++)
					{
						this.actions[i].Save(binaryWriter);
					}
				}
				text = Convert.ToBase64String(memoryStream.ToArray());
			}
			return text;
		}

		// Token: 0x060026EE RID: 9966 RVA: 0x000A7264 File Offset: 0x000A5464
		public void Load(string data, bool upgrade = false)
		{
			if (data == null)
			{
				return;
			}
			try
			{
				using (MemoryStream memoryStream = new MemoryStream(Convert.FromBase64String(data)))
				{
					using (BinaryReader binaryReader = new BinaryReader(memoryStream))
					{
						if (binaryReader.ReadUInt32() != 1145981250U)
						{
							throw new Exception("Unknown data format.");
						}
						ushort num = binaryReader.ReadUInt16();
						if (num < 1 || num > 2)
						{
							throw new Exception("Unknown data format version: " + num);
						}
						int num2 = binaryReader.ReadInt32();
						for (int i = 0; i < num2; i++)
						{
							PlayerAction playerAction;
							if (this.actionsByName.TryGetValue(binaryReader.ReadString(), out playerAction))
							{
								playerAction.Load(binaryReader, num, upgrade);
							}
						}
						UnityEngine.Debug.Log("FINISHED LOADING SERIALIZED KEYBINDINGS.");
					}
				}
			}
			catch (Exception ex)
			{
				UnityEngine.Debug.LogError("Provided state could not be loaded:\n" + ex.Message);
				this.Reset();
			}
		}

		// Token: 0x04001AA8 RID: 6824
		private bool m_forceDisable;

		// Token: 0x04001AAD RID: 6829
		public BindingSourceType LastInputType;

		// Token: 0x04001AAF RID: 6831
		public ulong LastInputTypeChangedTick;

		// Token: 0x04001AB0 RID: 6832
		public InputDeviceClass LastDeviceClass;

		// Token: 0x04001AB1 RID: 6833
		public InputDeviceStyle LastDeviceStyle;

		// Token: 0x04001AB5 RID: 6837
		private List<PlayerAction> actions = new List<PlayerAction>();

		// Token: 0x04001AB6 RID: 6838
		private List<PlayerOneAxisAction> oneAxisActions = new List<PlayerOneAxisAction>();

		// Token: 0x04001AB7 RID: 6839
		private List<PlayerTwoAxisAction> twoAxisActions = new List<PlayerTwoAxisAction>();

		// Token: 0x04001AB8 RID: 6840
		private Dictionary<string, PlayerAction> actionsByName = new Dictionary<string, PlayerAction>();

		// Token: 0x04001AB9 RID: 6841
		private BindingListenOptions listenOptions = new BindingListenOptions();

		// Token: 0x04001ABA RID: 6842
		internal PlayerAction listenWithAction;

		// Token: 0x04001ABB RID: 6843
		private InputDevice activeDevice;

		// Token: 0x04001ABC RID: 6844
		private const ushort currentDataFormatVersion = 2;
	}
}
