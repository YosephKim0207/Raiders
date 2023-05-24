using System;
using UnityEngine;

namespace InControl
{
	// Token: 0x020006A9 RID: 1705
	public class OneAxisInputControl : IInputControl
	{
		// Token: 0x17000753 RID: 1875
		// (get) Token: 0x06002761 RID: 10081 RVA: 0x000A8658 File Offset: 0x000A6858
		// (set) Token: 0x06002762 RID: 10082 RVA: 0x000A8660 File Offset: 0x000A6860
		public ulong UpdateTick { get; protected set; }

		// Token: 0x06002763 RID: 10083 RVA: 0x000A866C File Offset: 0x000A686C
		private void PrepareForUpdate(ulong updateTick)
		{
			if (this.IsNull)
			{
				return;
			}
			if (updateTick < this.pendingTick)
			{
				throw new InvalidOperationException("Cannot be updated with an earlier tick.");
			}
			if (this.pendingCommit && updateTick != this.pendingTick)
			{
				throw new InvalidOperationException("Cannot be updated for a new tick until pending tick is committed.");
			}
			if (updateTick > this.pendingTick)
			{
				this.lastState = this.thisState;
				this.nextState.Reset();
				this.pendingTick = updateTick;
				this.pendingCommit = true;
			}
		}

		// Token: 0x06002764 RID: 10084 RVA: 0x000A86F0 File Offset: 0x000A68F0
		public bool UpdateWithState(bool state, ulong updateTick, float deltaTime)
		{
			if (this.IsNull)
			{
				return false;
			}
			this.PrepareForUpdate(updateTick);
			this.nextState.Set(state || this.nextState.State);
			return state;
		}

		// Token: 0x06002765 RID: 10085 RVA: 0x000A8728 File Offset: 0x000A6928
		public bool UpdateWithValue(float value, ulong updateTick, float deltaTime)
		{
			if (this.IsNull)
			{
				return false;
			}
			this.PrepareForUpdate(updateTick);
			if (Utility.Abs(value) > Utility.Abs(this.nextState.RawValue))
			{
				this.nextState.RawValue = value;
				if (!this.Raw)
				{
					value = Utility.ApplyDeadZone(value, this.lowerDeadZone, this.upperDeadZone);
				}
				this.nextState.Set(value, this.stateThreshold);
				return true;
			}
			return false;
		}

		// Token: 0x06002766 RID: 10086 RVA: 0x000A87A4 File Offset: 0x000A69A4
		internal bool UpdateWithRawValue(float value, ulong updateTick, float deltaTime)
		{
			if (this.IsNull)
			{
				return false;
			}
			this.Raw = true;
			this.PrepareForUpdate(updateTick);
			if (Utility.Abs(value) > Utility.Abs(this.nextState.RawValue))
			{
				this.nextState.RawValue = value;
				this.nextState.Set(value, this.stateThreshold);
				return true;
			}
			return false;
		}

		// Token: 0x06002767 RID: 10087 RVA: 0x000A8808 File Offset: 0x000A6A08
		internal void SetValue(float value, ulong updateTick)
		{
			if (this.IsNull)
			{
				return;
			}
			if (updateTick > this.pendingTick)
			{
				this.lastState = this.thisState;
				this.nextState.Reset();
				this.pendingTick = updateTick;
				this.pendingCommit = true;
			}
			this.nextState.RawValue = value;
			this.nextState.Set(value, this.StateThreshold);
		}

		// Token: 0x06002768 RID: 10088 RVA: 0x000A8870 File Offset: 0x000A6A70
		public void ClearInputState()
		{
			this.lastState.Reset();
			this.thisState.Reset();
			this.nextState.Reset();
			this.wasRepeated = false;
			this.clearInputState = true;
		}

		// Token: 0x06002769 RID: 10089 RVA: 0x000A88A4 File Offset: 0x000A6AA4
		public void Commit()
		{
			if (this.IsNull)
			{
				return;
			}
			this.pendingCommit = false;
			this.thisState = this.nextState;
			if (this.Suppressed && !this.thisState.State)
			{
				this.ClearInputState();
				this.Suppressed = false;
			}
			if (this.clearInputState)
			{
				this.lastState = this.nextState;
				this.UpdateTick = this.pendingTick;
				this.clearInputState = false;
				return;
			}
			bool state = this.lastState.State;
			bool state2 = this.thisState.State;
			this.wasRepeated = false;
			if (state && !state2)
			{
				this.nextRepeatTime = 0f;
				this.lastReleasedTime = Time.realtimeSinceStartup;
			}
			else if (state2)
			{
				if (state != state2)
				{
					this.lastPressedTime = Time.realtimeSinceStartup;
					this.nextRepeatTime = Time.realtimeSinceStartup + this.FirstRepeatDelay;
				}
				else if (Time.realtimeSinceStartup >= this.nextRepeatTime)
				{
					this.wasRepeated = true;
					this.nextRepeatTime = Time.realtimeSinceStartup + this.RepeatDelay;
				}
			}
			if (this.thisState != this.lastState)
			{
				this.UpdateTick = this.pendingTick;
			}
		}

		// Token: 0x0600276A RID: 10090 RVA: 0x000A89E4 File Offset: 0x000A6BE4
		public void CommitWithState(bool state, ulong updateTick, float deltaTime)
		{
			this.UpdateWithState(state, updateTick, deltaTime);
			this.Commit();
		}

		// Token: 0x0600276B RID: 10091 RVA: 0x000A89F8 File Offset: 0x000A6BF8
		public void CommitWithValue(float value, ulong updateTick, float deltaTime)
		{
			this.UpdateWithValue(value, updateTick, deltaTime);
			this.Commit();
		}

		// Token: 0x0600276C RID: 10092 RVA: 0x000A8A0C File Offset: 0x000A6C0C
		internal void CommitWithSides(InputControl negativeSide, InputControl positiveSide, ulong updateTick, float deltaTime)
		{
			this.LowerDeadZone = Mathf.Max(negativeSide.LowerDeadZone, positiveSide.LowerDeadZone);
			this.UpperDeadZone = Mathf.Min(negativeSide.UpperDeadZone, positiveSide.UpperDeadZone);
			this.Raw = negativeSide.Raw || positiveSide.Raw;
			float num = Utility.ValueFromSides(negativeSide.RawValue, positiveSide.RawValue);
			this.CommitWithValue(num, updateTick, deltaTime);
		}

		// Token: 0x17000754 RID: 1876
		// (get) Token: 0x0600276D RID: 10093 RVA: 0x000A8A80 File Offset: 0x000A6C80
		public bool State
		{
			get
			{
				return this.Enabled && !this.Suppressed && this.thisState.State;
			}
		}

		// Token: 0x17000755 RID: 1877
		// (get) Token: 0x0600276E RID: 10094 RVA: 0x000A8AA8 File Offset: 0x000A6CA8
		// (set) Token: 0x0600276F RID: 10095 RVA: 0x000A8AD0 File Offset: 0x000A6CD0
		public bool LastState
		{
			get
			{
				return this.Enabled && !this.Suppressed && this.lastState.State;
			}
			set
			{
				this.lastState.State = value;
			}
		}

		// Token: 0x17000756 RID: 1878
		// (get) Token: 0x06002770 RID: 10096 RVA: 0x000A8AE0 File Offset: 0x000A6CE0
		public float Value
		{
			get
			{
				return (!this.Enabled || this.Suppressed) ? 0f : this.thisState.Value;
			}
		}

		// Token: 0x17000757 RID: 1879
		// (get) Token: 0x06002771 RID: 10097 RVA: 0x000A8B10 File Offset: 0x000A6D10
		public float LastValue
		{
			get
			{
				return (!this.Enabled || this.Suppressed) ? 0f : this.lastState.Value;
			}
		}

		// Token: 0x17000758 RID: 1880
		// (get) Token: 0x06002772 RID: 10098 RVA: 0x000A8B40 File Offset: 0x000A6D40
		public float RawValue
		{
			get
			{
				return (!this.Enabled || this.Suppressed) ? 0f : this.thisState.RawValue;
			}
		}

		// Token: 0x17000759 RID: 1881
		// (get) Token: 0x06002773 RID: 10099 RVA: 0x000A8B70 File Offset: 0x000A6D70
		internal float NextRawValue
		{
			get
			{
				return (!this.Enabled || this.Suppressed) ? 0f : this.nextState.RawValue;
			}
		}

		// Token: 0x1700075A RID: 1882
		// (get) Token: 0x06002774 RID: 10100 RVA: 0x000A8BA0 File Offset: 0x000A6DA0
		public bool HasChanged
		{
			get
			{
				return this.Enabled && !this.Suppressed && this.thisState != this.lastState;
			}
		}

		// Token: 0x1700075B RID: 1883
		// (get) Token: 0x06002775 RID: 10101 RVA: 0x000A8BCC File Offset: 0x000A6DCC
		public bool IsPressed
		{
			get
			{
				return this.Enabled && !this.Suppressed && this.thisState.State;
			}
		}

		// Token: 0x1700075C RID: 1884
		// (get) Token: 0x06002776 RID: 10102 RVA: 0x000A8BF4 File Offset: 0x000A6DF4
		public bool WasPressed
		{
			get
			{
				return this.Enabled && !this.Suppressed && this.thisState && !this.lastState;
			}
		}

		// Token: 0x1700075D RID: 1885
		// (get) Token: 0x06002777 RID: 10103 RVA: 0x000A8C30 File Offset: 0x000A6E30
		public bool WasPressedRepeating
		{
			get
			{
				if (this.thisState)
				{
					if (!this.lastState)
					{
						this.nextTimeForRepeat = Time.realtimeSinceStartup + 0.5f;
						this.startRepeatTime = Time.realtimeSinceStartup;
						return true;
					}
					if (Time.realtimeSinceStartup > this.startRepeatTime + 5f)
					{
						return false;
					}
					if (Time.realtimeSinceStartup >= this.nextTimeForRepeat)
					{
						this.nextTimeForRepeat = Time.realtimeSinceStartup + 0.1f;
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x06002778 RID: 10104 RVA: 0x000A8CB8 File Offset: 0x000A6EB8
		public void ResetRepeating()
		{
			this.nextTimeForRepeat = Time.realtimeSinceStartup + 0.5f;
		}

		// Token: 0x1700075E RID: 1886
		// (get) Token: 0x06002779 RID: 10105 RVA: 0x000A8CCC File Offset: 0x000A6ECC
		public bool WasPressedAsDpad
		{
			get
			{
				return this.Enabled && !this.Suppressed && this.thisState.RawValue >= 0.5f && this.lastState.RawValue < 0.5f;
			}
		}

		// Token: 0x1700075F RID: 1887
		// (get) Token: 0x0600277A RID: 10106 RVA: 0x000A8D1C File Offset: 0x000A6F1C
		public bool WasPressedAsDpadRepeating
		{
			get
			{
				if (this.thisState.RawValue >= 0.5f)
				{
					if (this.lastState.RawValue < 0.5f)
					{
						this.nextTimeForRepeat = Time.realtimeSinceStartup + 0.5f;
						this.startRepeatTime = Time.realtimeSinceStartup;
						return true;
					}
					if (Time.realtimeSinceStartup > this.startRepeatTime + 5f)
					{
						return false;
					}
					if (Time.realtimeSinceStartup >= this.nextTimeForRepeat)
					{
						this.nextTimeForRepeat = Time.realtimeSinceStartup + 0.1f;
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x17000760 RID: 1888
		// (get) Token: 0x0600277B RID: 10107 RVA: 0x000A8DB0 File Offset: 0x000A6FB0
		public float PressedDuration
		{
			get
			{
				if (this.lastReleasedTime > this.lastPressedTime)
				{
					return this.lastReleasedTime - this.lastPressedTime;
				}
				return Time.realtimeSinceStartup - this.lastPressedTime;
			}
		}

		// Token: 0x17000761 RID: 1889
		// (get) Token: 0x0600277C RID: 10108 RVA: 0x000A8DE0 File Offset: 0x000A6FE0
		public bool WasReleased
		{
			get
			{
				return this.Enabled && !this.Suppressed && !this.thisState && this.lastState;
			}
		}

		// Token: 0x17000762 RID: 1890
		// (get) Token: 0x0600277D RID: 10109 RVA: 0x000A8E18 File Offset: 0x000A7018
		public bool WasRepeated
		{
			get
			{
				return this.Enabled && !this.Suppressed && this.wasRepeated;
			}
		}

		// Token: 0x0600277E RID: 10110 RVA: 0x000A8E3C File Offset: 0x000A703C
		public void Suppress()
		{
			this.Suppressed = true;
		}

		// Token: 0x17000763 RID: 1891
		// (get) Token: 0x0600277F RID: 10111 RVA: 0x000A8E48 File Offset: 0x000A7048
		// (set) Token: 0x06002780 RID: 10112 RVA: 0x000A8E50 File Offset: 0x000A7050
		public float Sensitivity
		{
			get
			{
				return this.sensitivity;
			}
			set
			{
				this.sensitivity = Mathf.Clamp01(value);
			}
		}

		// Token: 0x17000764 RID: 1892
		// (get) Token: 0x06002781 RID: 10113 RVA: 0x000A8E60 File Offset: 0x000A7060
		// (set) Token: 0x06002782 RID: 10114 RVA: 0x000A8E68 File Offset: 0x000A7068
		public float LowerDeadZone
		{
			get
			{
				return this.lowerDeadZone;
			}
			set
			{
				this.lowerDeadZone = Mathf.Clamp01(value);
			}
		}

		// Token: 0x17000765 RID: 1893
		// (get) Token: 0x06002783 RID: 10115 RVA: 0x000A8E78 File Offset: 0x000A7078
		// (set) Token: 0x06002784 RID: 10116 RVA: 0x000A8E80 File Offset: 0x000A7080
		public float UpperDeadZone
		{
			get
			{
				return this.upperDeadZone;
			}
			set
			{
				this.upperDeadZone = Mathf.Clamp01(value);
			}
		}

		// Token: 0x17000766 RID: 1894
		// (get) Token: 0x06002785 RID: 10117 RVA: 0x000A8E90 File Offset: 0x000A7090
		// (set) Token: 0x06002786 RID: 10118 RVA: 0x000A8E98 File Offset: 0x000A7098
		public float StateThreshold
		{
			get
			{
				return this.stateThreshold;
			}
			set
			{
				this.stateThreshold = Mathf.Clamp01(value);
			}
		}

		// Token: 0x17000767 RID: 1895
		// (get) Token: 0x06002787 RID: 10119 RVA: 0x000A8EA8 File Offset: 0x000A70A8
		public bool IsNull
		{
			get
			{
				return object.ReferenceEquals(this, InputControl.Null);
			}
		}

		// Token: 0x06002788 RID: 10120 RVA: 0x000A8EB8 File Offset: 0x000A70B8
		public static implicit operator bool(OneAxisInputControl instance)
		{
			return instance.State;
		}

		// Token: 0x06002789 RID: 10121 RVA: 0x000A8EC0 File Offset: 0x000A70C0
		public static implicit operator float(OneAxisInputControl instance)
		{
			return instance.Value;
		}

		// Token: 0x04001B95 RID: 7061
		private float sensitivity = 1f;

		// Token: 0x04001B96 RID: 7062
		private float lowerDeadZone;

		// Token: 0x04001B97 RID: 7063
		private float upperDeadZone = 1f;

		// Token: 0x04001B98 RID: 7064
		private float stateThreshold;

		// Token: 0x04001B99 RID: 7065
		public float FirstRepeatDelay = 0.8f;

		// Token: 0x04001B9A RID: 7066
		public float RepeatDelay = 0.1f;

		// Token: 0x04001B9B RID: 7067
		public bool Raw;

		// Token: 0x04001B9C RID: 7068
		internal bool Enabled = true;

		// Token: 0x04001B9D RID: 7069
		internal bool Suppressed;

		// Token: 0x04001B9E RID: 7070
		private ulong pendingTick;

		// Token: 0x04001B9F RID: 7071
		private bool pendingCommit;

		// Token: 0x04001BA0 RID: 7072
		private float nextRepeatTime;

		// Token: 0x04001BA1 RID: 7073
		private float lastPressedTime;

		// Token: 0x04001BA2 RID: 7074
		private float lastReleasedTime;

		// Token: 0x04001BA3 RID: 7075
		private bool wasRepeated;

		// Token: 0x04001BA4 RID: 7076
		private bool clearInputState;

		// Token: 0x04001BA5 RID: 7077
		private InputControlState lastState;

		// Token: 0x04001BA6 RID: 7078
		private InputControlState nextState;

		// Token: 0x04001BA7 RID: 7079
		private InputControlState thisState;

		// Token: 0x04001BA8 RID: 7080
		private float startRepeatTime;

		// Token: 0x04001BA9 RID: 7081
		private float nextTimeForRepeat;
	}
}
