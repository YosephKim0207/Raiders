using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using UnityEngine;

namespace InControl
{
	// Token: 0x02000698 RID: 1688
	public class PlayerAction : OneAxisInputControl
	{
		// Token: 0x06002690 RID: 9872 RVA: 0x000A58F0 File Offset: 0x000A3AF0
		public PlayerAction(string name, PlayerActionSet owner)
		{
			this.Raw = true;
			this.Name = name;
			this.Owner = owner;
			this.bindings = new ReadOnlyCollection<BindingSource>(this.visibleBindings);
			this.unfilteredBindings = new ReadOnlyCollection<BindingSource>(this.regularBindings);
			owner.AddPlayerAction(this);
		}

		// Token: 0x17000720 RID: 1824
		// (get) Token: 0x06002691 RID: 9873 RVA: 0x000A5970 File Offset: 0x000A3B70
		// (set) Token: 0x06002692 RID: 9874 RVA: 0x000A5978 File Offset: 0x000A3B78
		public string Name { get; private set; }

		// Token: 0x17000721 RID: 1825
		// (get) Token: 0x06002693 RID: 9875 RVA: 0x000A5984 File Offset: 0x000A3B84
		// (set) Token: 0x06002694 RID: 9876 RVA: 0x000A598C File Offset: 0x000A3B8C
		public PlayerActionSet Owner { get; private set; }

		// Token: 0x14000072 RID: 114
		// (add) Token: 0x06002695 RID: 9877 RVA: 0x000A5998 File Offset: 0x000A3B98
		// (remove) Token: 0x06002696 RID: 9878 RVA: 0x000A59D0 File Offset: 0x000A3BD0
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action<BindingSourceType> OnLastInputTypeChanged;

		// Token: 0x17000722 RID: 1826
		// (get) Token: 0x06002697 RID: 9879 RVA: 0x000A5A08 File Offset: 0x000A3C08
		// (set) Token: 0x06002698 RID: 9880 RVA: 0x000A5A10 File Offset: 0x000A3C10
		public object UserData { get; set; }

		// Token: 0x06002699 RID: 9881 RVA: 0x000A5A1C File Offset: 0x000A3C1C
		public void AddDefaultBinding(BindingSource binding)
		{
			if (binding == null)
			{
				return;
			}
			if (binding.BoundTo != null)
			{
				throw new InControlException("Binding source is already bound to action " + binding.BoundTo.Name);
			}
			if (!this.defaultBindings.Contains(binding))
			{
				this.defaultBindings.Add(binding);
				binding.BoundTo = this;
			}
			if (!this.regularBindings.Contains(binding))
			{
				this.regularBindings.Add(binding);
				binding.BoundTo = this;
				if (binding.IsValid)
				{
					this.visibleBindings.Add(binding);
				}
			}
		}

		// Token: 0x0600269A RID: 9882 RVA: 0x000A5ABC File Offset: 0x000A3CBC
		public void AddDefaultBinding(params Key[] keys)
		{
			this.AddDefaultBinding(new KeyBindingSource(keys));
		}

		// Token: 0x0600269B RID: 9883 RVA: 0x000A5ACC File Offset: 0x000A3CCC
		public void AddDefaultBinding(KeyCombo keyCombo)
		{
			this.AddDefaultBinding(new KeyBindingSource(keyCombo));
		}

		// Token: 0x0600269C RID: 9884 RVA: 0x000A5ADC File Offset: 0x000A3CDC
		public void AddDefaultBinding(Mouse control)
		{
			this.AddDefaultBinding(new MouseBindingSource(control));
		}

		// Token: 0x0600269D RID: 9885 RVA: 0x000A5AEC File Offset: 0x000A3CEC
		public void AddDefaultBinding(InputControlType control)
		{
			this.AddDefaultBinding(new DeviceBindingSource(control));
		}

		// Token: 0x0600269E RID: 9886 RVA: 0x000A5AFC File Offset: 0x000A3CFC
		public void RemoveBindingOfType(InputControlType control)
		{
			for (int i = 0; i < this.bindings.Count; i++)
			{
				if (this.bindings[i] != null && this.bindings[i].BindingSourceType == BindingSourceType.DeviceBindingSource)
				{
					DeviceBindingSource deviceBindingSource = this.bindings[i] as DeviceBindingSource;
					if (deviceBindingSource.Control == control)
					{
						this.RemoveBinding(deviceBindingSource);
						break;
					}
				}
			}
			this.ForceUpdateVisibleBindings();
		}

		// Token: 0x0600269F RID: 9887 RVA: 0x000A5B84 File Offset: 0x000A3D84
		public bool AddBinding(BindingSource binding)
		{
			if (binding == null)
			{
				return false;
			}
			if (binding.BoundTo != null)
			{
				UnityEngine.Debug.LogWarning("Binding source is already bound to action " + binding.BoundTo.Name);
				return false;
			}
			if (this.regularBindings.Contains(binding))
			{
				return false;
			}
			this.regularBindings.Add(binding);
			binding.BoundTo = this;
			if (binding.IsValid)
			{
				this.visibleBindings.Add(binding);
			}
			return true;
		}

		// Token: 0x060026A0 RID: 9888 RVA: 0x000A5C04 File Offset: 0x000A3E04
		public bool InsertBindingAt(int index, BindingSource binding)
		{
			if (index < 0 || index > this.visibleBindings.Count)
			{
				throw new InControlException("Index is out of range for bindings on this action.");
			}
			if (index == this.visibleBindings.Count)
			{
				return this.AddBinding(binding);
			}
			if (binding == null)
			{
				return false;
			}
			if (binding.BoundTo != null)
			{
				UnityEngine.Debug.LogWarning("Binding source is already bound to action " + binding.BoundTo.Name);
				return false;
			}
			if (this.regularBindings.Contains(binding))
			{
				return false;
			}
			int num = ((index != 0) ? this.regularBindings.IndexOf(this.visibleBindings[index]) : 0);
			this.regularBindings.Insert(num, binding);
			binding.BoundTo = this;
			if (binding.IsValid)
			{
				this.visibleBindings.Insert(index, binding);
			}
			return true;
		}

		// Token: 0x060026A1 RID: 9889 RVA: 0x000A5CE8 File Offset: 0x000A3EE8
		public bool ReplaceBinding(BindingSource findBinding, BindingSource withBinding)
		{
			if (findBinding == null || withBinding == null)
			{
				return false;
			}
			if (withBinding.BoundTo != null)
			{
				UnityEngine.Debug.LogWarning("Binding source is already bound to action " + withBinding.BoundTo.Name);
				return false;
			}
			int num = this.regularBindings.IndexOf(findBinding);
			if (num < 0)
			{
				UnityEngine.Debug.LogWarning("Binding source to replace is not present in this action.");
				return false;
			}
			findBinding.BoundTo = null;
			this.regularBindings[num] = withBinding;
			withBinding.BoundTo = this;
			num = this.visibleBindings.IndexOf(findBinding);
			if (num >= 0)
			{
				this.visibleBindings[num] = withBinding;
			}
			return true;
		}

		// Token: 0x060026A2 RID: 9890 RVA: 0x000A5D94 File Offset: 0x000A3F94
		public bool HasBinding(BindingSource binding)
		{
			if (binding == null)
			{
				return false;
			}
			BindingSource bindingSource = this.FindBinding(binding);
			return !(bindingSource == null) && bindingSource.BoundTo == this;
		}

		// Token: 0x060026A3 RID: 9891 RVA: 0x000A5DD0 File Offset: 0x000A3FD0
		public BindingSource FindBinding(BindingSource binding)
		{
			if (binding == null)
			{
				return null;
			}
			int num = this.regularBindings.IndexOf(binding);
			if (num >= 0)
			{
				return this.regularBindings[num];
			}
			return null;
		}

		// Token: 0x060026A4 RID: 9892 RVA: 0x000A5E10 File Offset: 0x000A4010
		private void HardRemoveBinding(BindingSource binding)
		{
			if (binding == null)
			{
				return;
			}
			int num = this.regularBindings.IndexOf(binding);
			if (num >= 0)
			{
				BindingSource bindingSource = this.regularBindings[num];
				if (bindingSource.BoundTo == this)
				{
					bindingSource.BoundTo = null;
					this.regularBindings.RemoveAt(num);
					this.UpdateVisibleBindings();
				}
			}
		}

		// Token: 0x060026A5 RID: 9893 RVA: 0x000A5E70 File Offset: 0x000A4070
		public void RemoveBinding(BindingSource binding)
		{
			BindingSource bindingSource = this.FindBinding(binding);
			if (bindingSource != null && bindingSource.BoundTo == this)
			{
				bindingSource.BoundTo = null;
			}
		}

		// Token: 0x060026A6 RID: 9894 RVA: 0x000A5EA4 File Offset: 0x000A40A4
		public void ClearSpecificBindingByType(int index, params BindingSourceType[] types)
		{
			int num = 0;
			for (int i = 0; i < this.regularBindings.Count; i++)
			{
				if (Array.IndexOf<BindingSourceType>(types, this.regularBindings[i].BindingSourceType) >= 0)
				{
					if (num == index)
					{
						this.regularBindings.RemoveAt(i);
						break;
					}
					num++;
				}
			}
			this.UpdateVisibleBindings();
		}

		// Token: 0x060026A7 RID: 9895 RVA: 0x000A5F10 File Offset: 0x000A4110
		public void SetBindingOfTypeByNumber(BindingSource binding, BindingSourceType sourceType, int index, Action<PlayerAction, BindingSource> OnBindingAdded = null)
		{
			List<BindingSource> list = new List<BindingSource>();
			List<BindingSourceType> list2 = new List<BindingSourceType>();
			list2.Add(sourceType);
			if (sourceType == BindingSourceType.KeyBindingSource)
			{
				list2.Add(BindingSourceType.MouseBindingSource);
			}
			if (sourceType == BindingSourceType.MouseBindingSource)
			{
				list2.Add(BindingSourceType.KeyBindingSource);
			}
			if (sourceType == BindingSourceType.DeviceBindingSource)
			{
				list2.Add(BindingSourceType.UnknownDeviceBindingSource);
			}
			if (sourceType == BindingSourceType.UnknownDeviceBindingSource)
			{
				list2.Add(BindingSourceType.DeviceBindingSource);
			}
			for (int i = 0; i < this.regularBindings.Count; i++)
			{
				if (list2.Contains(this.regularBindings[i].BindingSourceType))
				{
					list.Add(this.regularBindings[i]);
					this.regularBindings.RemoveAt(i);
					i--;
				}
			}
			this.UpdateVisibleBindings();
			for (int j = 0; j < list.Count; j++)
			{
				list[j].BoundTo = null;
			}
			if (list.Count <= index)
			{
				list.Add(binding);
			}
			else if (list.Count > index)
			{
				list[index] = binding;
			}
			for (int k = 0; k < list.Count; k++)
			{
				this.AddBinding(list[k]);
			}
			this.UpdateVisibleBindings();
			if (OnBindingAdded != null)
			{
				OnBindingAdded(this, binding);
			}
		}

		// Token: 0x060026A8 RID: 9896 RVA: 0x000A6058 File Offset: 0x000A4258
		public void IgnoreBindingsOfType(BindingSourceType sourceType)
		{
			this.m_ignoredBindingSources.Add(sourceType);
		}

		// Token: 0x060026A9 RID: 9897 RVA: 0x000A6068 File Offset: 0x000A4268
		public void ClearBindingsOfType(BindingSourceType sourceType)
		{
			for (int i = 0; i < this.regularBindings.Count; i++)
			{
				if (this.regularBindings[i].BindingSourceType == sourceType)
				{
					this.regularBindings.RemoveAt(i);
					i--;
				}
			}
			this.UpdateVisibleBindings();
		}

		// Token: 0x060026AA RID: 9898 RVA: 0x000A60C0 File Offset: 0x000A42C0
		public void RemoveBindingAt(int index)
		{
			if (index < 0 || index >= this.regularBindings.Count)
			{
				throw new InControlException("Index is out of range for bindings on this action.");
			}
			this.regularBindings[index].BoundTo = null;
		}

		// Token: 0x060026AB RID: 9899 RVA: 0x000A60F8 File Offset: 0x000A42F8
		private int CountBindingsOfType(BindingSourceType bindingSourceType)
		{
			int num = 0;
			int count = this.regularBindings.Count;
			for (int i = 0; i < count; i++)
			{
				BindingSource bindingSource = this.regularBindings[i];
				if (bindingSource.BoundTo == this && bindingSource.BindingSourceType == bindingSourceType)
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x060026AC RID: 9900 RVA: 0x000A6150 File Offset: 0x000A4350
		private void RemoveFirstBindingOfType(BindingSourceType bindingSourceType)
		{
			int count = this.regularBindings.Count;
			for (int i = 0; i < count; i++)
			{
				BindingSource bindingSource = this.regularBindings[i];
				if (bindingSource.BoundTo == this && bindingSource.BindingSourceType == bindingSourceType)
				{
					bindingSource.BoundTo = null;
					this.regularBindings.RemoveAt(i);
					return;
				}
			}
		}

		// Token: 0x060026AD RID: 9901 RVA: 0x000A61B4 File Offset: 0x000A43B4
		private int IndexOfFirstInvalidBinding()
		{
			int count = this.regularBindings.Count;
			for (int i = 0; i < count; i++)
			{
				if (!this.regularBindings[i].IsValid)
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x060026AE RID: 9902 RVA: 0x000A61F8 File Offset: 0x000A43F8
		public void ClearBindings()
		{
			int count = this.regularBindings.Count;
			for (int i = 0; i < count; i++)
			{
				this.regularBindings[i].BoundTo = null;
			}
			this.regularBindings.Clear();
			this.visibleBindings.Clear();
		}

		// Token: 0x060026AF RID: 9903 RVA: 0x000A624C File Offset: 0x000A444C
		public void ResetBindings()
		{
			this.ClearBindings();
			this.regularBindings.AddRange(this.defaultBindings);
			int count = this.regularBindings.Count;
			for (int i = 0; i < count; i++)
			{
				BindingSource bindingSource = this.regularBindings[i];
				bindingSource.BoundTo = this;
				if (bindingSource.IsValid)
				{
					this.visibleBindings.Add(bindingSource);
				}
			}
		}

		// Token: 0x060026B0 RID: 9904 RVA: 0x000A62BC File Offset: 0x000A44BC
		public void ListenForBinding()
		{
			this.ListenForBindingReplacing(null);
		}

		// Token: 0x060026B1 RID: 9905 RVA: 0x000A62C8 File Offset: 0x000A44C8
		public void ListenForBindingReplacing(BindingSource binding)
		{
			BindingListenOptions bindingListenOptions = this.ListenOptions ?? this.Owner.ListenOptions;
			bindingListenOptions.ReplaceBinding = binding;
			this.Owner.listenWithAction = this;
			int num = PlayerAction.bindingSourceListeners.Length;
			for (int i = 0; i < num; i++)
			{
				PlayerAction.bindingSourceListeners[i].Reset();
			}
		}

		// Token: 0x060026B2 RID: 9906 RVA: 0x000A6328 File Offset: 0x000A4528
		public void StopListeningForBinding()
		{
			if (this.IsListeningForBinding)
			{
				this.Owner.listenWithAction = null;
			}
		}

		// Token: 0x17000723 RID: 1827
		// (get) Token: 0x060026B3 RID: 9907 RVA: 0x000A6344 File Offset: 0x000A4544
		public bool IsListeningForBinding
		{
			get
			{
				return this.Owner.listenWithAction == this;
			}
		}

		// Token: 0x17000724 RID: 1828
		// (get) Token: 0x060026B4 RID: 9908 RVA: 0x000A6354 File Offset: 0x000A4554
		public ReadOnlyCollection<BindingSource> Bindings
		{
			get
			{
				return this.bindings;
			}
		}

		// Token: 0x17000725 RID: 1829
		// (get) Token: 0x060026B5 RID: 9909 RVA: 0x000A635C File Offset: 0x000A455C
		public ReadOnlyCollection<BindingSource> UnfilteredBindings
		{
			get
			{
				return this.unfilteredBindings;
			}
		}

		// Token: 0x060026B6 RID: 9910 RVA: 0x000A6364 File Offset: 0x000A4564
		private void RemoveOrphanedBindings()
		{
			int count = this.regularBindings.Count;
			for (int i = count - 1; i >= 0; i--)
			{
				if (this.regularBindings[i].BoundTo != this)
				{
					this.regularBindings.RemoveAt(i);
				}
			}
		}

		// Token: 0x060026B7 RID: 9911 RVA: 0x000A63B4 File Offset: 0x000A45B4
		internal void Update(ulong updateTick, float deltaTime, InputDevice device)
		{
			this.Device = device;
			this.UpdateBindings(updateTick, deltaTime);
			this.DetectBindings();
		}

		// Token: 0x060026B8 RID: 9912 RVA: 0x000A63CC File Offset: 0x000A45CC
		private void UpdateBindings(ulong updateTick, float deltaTime)
		{
			bool flag = this.IsListeningForBinding || (this.Owner.IsListeningForBinding && this.Owner.PreventInputWhileListeningForBinding);
			BindingSourceType bindingSourceType = this.LastInputType;
			ulong num = this.LastInputTypeChangedTick;
			ulong updateTick2 = base.UpdateTick;
			InputDeviceClass inputDeviceClass = this.LastDeviceClass;
			InputDeviceStyle inputDeviceStyle = this.LastDeviceStyle;
			int count = this.regularBindings.Count;
			for (int i = count - 1; i >= 0; i--)
			{
				BindingSource bindingSource = this.regularBindings[i];
				bool flag2 = false;
				for (int j = 0; j < this.m_ignoredBindingSources.Count; j++)
				{
					if (this.m_ignoredBindingSources[j] == bindingSource.BindingSourceType)
					{
						flag2 = true;
					}
				}
				if (!flag2)
				{
					if (bindingSource.BoundTo != this)
					{
						this.regularBindings.RemoveAt(i);
						this.visibleBindings.Remove(bindingSource);
					}
					else if (!flag)
					{
						float value = bindingSource.GetValue(this.Device);
						if (base.UpdateWithValue(value, updateTick, deltaTime))
						{
							bindingSourceType = bindingSource.BindingSourceType;
							num = updateTick;
							inputDeviceClass = bindingSource.DeviceClass;
							inputDeviceStyle = bindingSource.DeviceStyle;
						}
					}
				}
			}
			if (flag || count == 0)
			{
				base.UpdateWithValue(0f, updateTick, deltaTime);
			}
			base.Commit();
			this.Enabled = this.Owner.Enabled;
			if (num > this.LastInputTypeChangedTick && (bindingSourceType != BindingSourceType.MouseBindingSource || Utility.Abs(base.LastValue - base.Value) >= MouseBindingSource.JitterThreshold))
			{
				bool flag3 = bindingSourceType != this.LastInputType;
				this.LastInputType = bindingSourceType;
				this.LastInputTypeChangedTick = num;
				this.LastDeviceClass = inputDeviceClass;
				this.LastDeviceStyle = inputDeviceStyle;
				if (this.OnLastInputTypeChanged != null && flag3)
				{
					this.OnLastInputTypeChanged(bindingSourceType);
				}
			}
			if (base.UpdateTick > updateTick2)
			{
				this.activeDevice = ((!this.LastInputTypeIsDevice) ? null : this.Device);
			}
		}

		// Token: 0x060026B9 RID: 9913 RVA: 0x000A65EC File Offset: 0x000A47EC
		private void DetectBindings()
		{
			if (this.IsListeningForBinding)
			{
				BindingSource bindingSource = null;
				BindingListenOptions bindingListenOptions = this.ListenOptions ?? this.Owner.ListenOptions;
				int num = PlayerAction.bindingSourceListeners.Length;
				for (int i = 0; i < num; i++)
				{
					bindingSource = PlayerAction.bindingSourceListeners[i].Listen(bindingListenOptions, this.device);
					if (bindingSource != null)
					{
						break;
					}
				}
				if (bindingSource == null)
				{
					return;
				}
				if (!bindingListenOptions.CallOnBindingFound(this, bindingSource))
				{
					return;
				}
				if (this.HasBinding(bindingSource))
				{
					if (bindingListenOptions.RejectRedundantBindings)
					{
						bindingListenOptions.CallOnBindingRejected(this, bindingSource, BindingSourceRejectionType.DuplicateBindingOnActionSet);
						return;
					}
					this.StopListeningForBinding();
					bindingListenOptions.CallOnBindingAdded(this, bindingSource);
					return;
				}
				else
				{
					if (bindingListenOptions.UnsetDuplicateBindingsOnSet)
					{
						int count = this.Owner.Actions.Count;
						for (int j = 0; j < count; j++)
						{
							this.Owner.Actions[j].HardRemoveBinding(bindingSource);
						}
					}
					if (!bindingListenOptions.AllowDuplicateBindingsPerSet && this.Owner.HasBinding(bindingSource))
					{
						bindingListenOptions.CallOnBindingRejected(this, bindingSource, BindingSourceRejectionType.DuplicateBindingOnActionSet);
						return;
					}
					this.StopListeningForBinding();
					if (bindingListenOptions.ReplaceBinding == null)
					{
						if (bindingListenOptions.MaxAllowedBindingsPerType > 0U)
						{
							while ((long)this.CountBindingsOfType(bindingSource.BindingSourceType) >= (long)((ulong)bindingListenOptions.MaxAllowedBindingsPerType))
							{
								this.RemoveFirstBindingOfType(bindingSource.BindingSourceType);
							}
						}
						else if (bindingListenOptions.MaxAllowedBindings > 0U)
						{
							while ((long)this.regularBindings.Count >= (long)((ulong)bindingListenOptions.MaxAllowedBindings))
							{
								int num2 = Mathf.Max(0, this.IndexOfFirstInvalidBinding());
								this.regularBindings.RemoveAt(num2);
							}
						}
						this.AddBinding(bindingSource);
					}
					else
					{
						this.ReplaceBinding(bindingListenOptions.ReplaceBinding, bindingSource);
					}
					this.UpdateVisibleBindings();
					bindingListenOptions.CallOnBindingAdded(this, bindingSource);
				}
			}
		}

		// Token: 0x060026BA RID: 9914 RVA: 0x000A67DC File Offset: 0x000A49DC
		public void ForceUpdateVisibleBindings()
		{
			this.UpdateVisibleBindings();
		}

		// Token: 0x060026BB RID: 9915 RVA: 0x000A67E4 File Offset: 0x000A49E4
		private void UpdateVisibleBindings()
		{
			this.visibleBindings.Clear();
			int count = this.regularBindings.Count;
			for (int i = 0; i < count; i++)
			{
				BindingSource bindingSource = this.regularBindings[i];
				if (bindingSource.IsValid)
				{
					this.visibleBindings.Add(bindingSource);
				}
			}
		}

		// Token: 0x17000726 RID: 1830
		// (get) Token: 0x060026BC RID: 9916 RVA: 0x000A6840 File Offset: 0x000A4A40
		// (set) Token: 0x060026BD RID: 9917 RVA: 0x000A686C File Offset: 0x000A4A6C
		internal InputDevice Device
		{
			get
			{
				if (this.device == null)
				{
					this.device = this.Owner.Device;
					this.UpdateVisibleBindings();
				}
				return this.device;
			}
			set
			{
				if (this.device != value)
				{
					this.device = value;
					this.UpdateVisibleBindings();
				}
			}
		}

		// Token: 0x17000727 RID: 1831
		// (get) Token: 0x060026BE RID: 9918 RVA: 0x000A6888 File Offset: 0x000A4A88
		public InputDevice ActiveDevice
		{
			get
			{
				return (this.activeDevice != null) ? this.activeDevice : InputDevice.Null;
			}
		}

		// Token: 0x17000728 RID: 1832
		// (get) Token: 0x060026BF RID: 9919 RVA: 0x000A68A8 File Offset: 0x000A4AA8
		private bool LastInputTypeIsDevice
		{
			get
			{
				return this.LastInputType == BindingSourceType.DeviceBindingSource || this.LastInputType == BindingSourceType.UnknownDeviceBindingSource;
			}
		}

		// Token: 0x17000729 RID: 1833
		// (get) Token: 0x060026C0 RID: 9920 RVA: 0x000A68C4 File Offset: 0x000A4AC4
		// (set) Token: 0x060026C1 RID: 9921 RVA: 0x000A68CC File Offset: 0x000A4ACC
		[Obsolete("Please set this property on device controls directly. It does nothing here.")]
		public new float LowerDeadZone
		{
			get
			{
				return 0f;
			}
			set
			{
			}
		}

		// Token: 0x1700072A RID: 1834
		// (get) Token: 0x060026C2 RID: 9922 RVA: 0x000A68DC File Offset: 0x000A4ADC
		// (set) Token: 0x060026C3 RID: 9923 RVA: 0x000A68E4 File Offset: 0x000A4AE4
		[Obsolete("Please set this property on device controls directly. It does nothing here.")]
		public new float UpperDeadZone
		{
			get
			{
				return 0f;
			}
			set
			{
			}
		}

		// Token: 0x060026C4 RID: 9924 RVA: 0x000A68F4 File Offset: 0x000A4AF4
		internal void Load(BinaryReader reader, ushort dataFormatVersion, bool upgrade)
		{
			this.ClearBindings();
			int num = reader.ReadInt32();
			int i = 0;
			while (i < num)
			{
				BindingSourceType bindingSourceType = (BindingSourceType)reader.ReadInt32();
				BindingSource bindingSource;
				switch (bindingSourceType)
				{
				case BindingSourceType.None:
					break;
				case BindingSourceType.DeviceBindingSource:
					bindingSource = new DeviceBindingSource();
					goto IL_81;
				case BindingSourceType.KeyBindingSource:
					bindingSource = new KeyBindingSource();
					goto IL_81;
				case BindingSourceType.MouseBindingSource:
					bindingSource = new MouseBindingSource();
					goto IL_81;
				case BindingSourceType.UnknownDeviceBindingSource:
					bindingSource = new UnknownDeviceBindingSource();
					goto IL_81;
				default:
					throw new InControlException("Don't know how to load BindingSourceType: " + bindingSourceType);
				}
				IL_92:
				i++;
				continue;
				IL_81:
				bindingSource.Load(reader, dataFormatVersion, upgrade);
				this.AddBinding(bindingSource);
				goto IL_92;
			}
		}

		// Token: 0x060026C5 RID: 9925 RVA: 0x000A69A0 File Offset: 0x000A4BA0
		internal void Save(BinaryWriter writer)
		{
			this.RemoveOrphanedBindings();
			writer.Write(this.Name);
			int count = this.regularBindings.Count;
			writer.Write(count);
			for (int i = 0; i < count; i++)
			{
				BindingSource bindingSource = this.regularBindings[i];
				writer.Write((int)bindingSource.BindingSourceType);
				bindingSource.Save(writer);
			}
		}

		// Token: 0x04001A97 RID: 6807
		public BindingListenOptions ListenOptions;

		// Token: 0x04001A98 RID: 6808
		public BindingSourceType LastInputType;

		// Token: 0x04001A9A RID: 6810
		public ulong LastInputTypeChangedTick;

		// Token: 0x04001A9B RID: 6811
		public InputDeviceClass LastDeviceClass;

		// Token: 0x04001A9C RID: 6812
		public InputDeviceStyle LastDeviceStyle;

		// Token: 0x04001A9E RID: 6814
		private List<BindingSource> defaultBindings = new List<BindingSource>();

		// Token: 0x04001A9F RID: 6815
		private List<BindingSource> regularBindings = new List<BindingSource>();

		// Token: 0x04001AA0 RID: 6816
		private List<BindingSource> visibleBindings = new List<BindingSource>();

		// Token: 0x04001AA1 RID: 6817
		private readonly ReadOnlyCollection<BindingSource> bindings;

		// Token: 0x04001AA2 RID: 6818
		private readonly ReadOnlyCollection<BindingSource> unfilteredBindings;

		// Token: 0x04001AA3 RID: 6819
		private static readonly BindingSourceListener[] bindingSourceListeners = new BindingSourceListener[]
		{
			new DeviceBindingSourceListener(),
			new UnknownDeviceBindingSourceListener(),
			new KeyBindingSourceListener(),
			new MouseBindingSourceListener()
		};

		// Token: 0x04001AA4 RID: 6820
		private List<BindingSourceType> m_ignoredBindingSources = new List<BindingSourceType>();

		// Token: 0x04001AA5 RID: 6821
		private InputDevice device;

		// Token: 0x04001AA6 RID: 6822
		private InputDevice activeDevice;
	}
}
