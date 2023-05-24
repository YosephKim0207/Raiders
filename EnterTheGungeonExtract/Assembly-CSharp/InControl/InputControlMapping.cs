using System;
using UnityEngine;

namespace InControl
{
	// Token: 0x020006A3 RID: 1699
	public class InputControlMapping
	{
		// Token: 0x0600274C RID: 10060 RVA: 0x000A81B0 File Offset: 0x000A63B0
		public float MapValue(float value)
		{
			if (this.Raw)
			{
				value *= this.Scale;
				value = ((!this.SourceRange.Excludes(value)) ? value : 0f);
			}
			else
			{
				value = Mathf.Clamp(value * this.Scale, -1f, 1f);
				value = InputRange.Remap(value, this.SourceRange, this.TargetRange);
			}
			if (this.Invert)
			{
				value = -value;
			}
			return value;
		}

		// Token: 0x17000752 RID: 1874
		// (get) Token: 0x0600274D RID: 10061 RVA: 0x000A8234 File Offset: 0x000A6434
		// (set) Token: 0x0600274E RID: 10062 RVA: 0x000A8264 File Offset: 0x000A6464
		public string Handle
		{
			get
			{
				return (!string.IsNullOrEmpty(this.handle)) ? this.handle : this.Target.ToString();
			}
			set
			{
				this.handle = value;
			}
		}

		// Token: 0x04001AF1 RID: 6897
		public InputControlSource Source;

		// Token: 0x04001AF2 RID: 6898
		public InputControlType Target;

		// Token: 0x04001AF3 RID: 6899
		public bool Invert;

		// Token: 0x04001AF4 RID: 6900
		public float Scale = 1f;

		// Token: 0x04001AF5 RID: 6901
		public bool Raw;

		// Token: 0x04001AF6 RID: 6902
		public bool Passive;

		// Token: 0x04001AF7 RID: 6903
		public bool IgnoreInitialZeroValue;

		// Token: 0x04001AF8 RID: 6904
		public float Sensitivity = 1f;

		// Token: 0x04001AF9 RID: 6905
		public float LowerDeadZone;

		// Token: 0x04001AFA RID: 6906
		public float UpperDeadZone = 1f;

		// Token: 0x04001AFB RID: 6907
		public InputRange SourceRange = InputRange.MinusOneToOne;

		// Token: 0x04001AFC RID: 6908
		public InputRange TargetRange = InputRange.MinusOneToOne;

		// Token: 0x04001AFD RID: 6909
		private string handle;
	}
}
