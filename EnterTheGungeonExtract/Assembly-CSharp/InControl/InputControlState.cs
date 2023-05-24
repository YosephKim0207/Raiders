using System;

namespace InControl
{
	// Token: 0x020006A5 RID: 1701
	public struct InputControlState
	{
		// Token: 0x06002751 RID: 10065 RVA: 0x000A8270 File Offset: 0x000A6470
		public void Reset()
		{
			this.State = false;
			this.Value = 0f;
			this.RawValue = 0f;
		}

		// Token: 0x06002752 RID: 10066 RVA: 0x000A8290 File Offset: 0x000A6490
		public void Set(float value)
		{
			this.Value = value;
			this.State = Utility.IsNotZero(value);
		}

		// Token: 0x06002753 RID: 10067 RVA: 0x000A82A8 File Offset: 0x000A64A8
		public void Set(float value, float threshold)
		{
			this.Value = value;
			this.State = Utility.AbsoluteIsOverThreshold(value, threshold);
		}

		// Token: 0x06002754 RID: 10068 RVA: 0x000A82C0 File Offset: 0x000A64C0
		public void Set(bool state)
		{
			this.State = state;
			this.Value = ((!state) ? 0f : 1f);
			this.RawValue = this.Value;
		}

		// Token: 0x06002755 RID: 10069 RVA: 0x000A82F0 File Offset: 0x000A64F0
		public static implicit operator bool(InputControlState state)
		{
			return state.State;
		}

		// Token: 0x06002756 RID: 10070 RVA: 0x000A82FC File Offset: 0x000A64FC
		public static implicit operator float(InputControlState state)
		{
			return state.Value;
		}

		// Token: 0x06002757 RID: 10071 RVA: 0x000A8308 File Offset: 0x000A6508
		public static bool operator ==(InputControlState a, InputControlState b)
		{
			return a.State == b.State && Utility.Approximately(a.Value, b.Value);
		}

		// Token: 0x06002758 RID: 10072 RVA: 0x000A8334 File Offset: 0x000A6534
		public static bool operator !=(InputControlState a, InputControlState b)
		{
			return a.State != b.State || !Utility.Approximately(a.Value, b.Value);
		}

		// Token: 0x04001AFE RID: 6910
		public bool State;

		// Token: 0x04001AFF RID: 6911
		public float Value;

		// Token: 0x04001B00 RID: 6912
		public float RawValue;
	}
}
