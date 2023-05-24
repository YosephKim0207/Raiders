using System;

namespace InControl
{
	// Token: 0x020006A2 RID: 1698
	public class InputControl : OneAxisInputControl
	{
		// Token: 0x0600273C RID: 10044 RVA: 0x000A8068 File Offset: 0x000A6268
		private InputControl()
		{
			this.Handle = "None";
			this.Target = InputControlType.None;
			this.Passive = false;
			this.IsButton = false;
			this.IsAnalog = false;
		}

		// Token: 0x0600273D RID: 10045 RVA: 0x000A8098 File Offset: 0x000A6298
		public InputControl(string handle, InputControlType target)
		{
			this.Handle = handle;
			this.Target = target;
			this.Passive = false;
			this.IsButton = Utility.TargetIsButton(target);
			this.IsAnalog = !this.IsButton;
		}

		// Token: 0x0600273E RID: 10046 RVA: 0x000A80D0 File Offset: 0x000A62D0
		public InputControl(string handle, InputControlType target, bool passive)
			: this(handle, target)
		{
			this.Passive = passive;
		}

		// Token: 0x1700074C RID: 1868
		// (get) Token: 0x0600273F RID: 10047 RVA: 0x000A80E4 File Offset: 0x000A62E4
		// (set) Token: 0x06002740 RID: 10048 RVA: 0x000A80EC File Offset: 0x000A62EC
		public string Handle { get; protected set; }

		// Token: 0x1700074D RID: 1869
		// (get) Token: 0x06002741 RID: 10049 RVA: 0x000A80F8 File Offset: 0x000A62F8
		// (set) Token: 0x06002742 RID: 10050 RVA: 0x000A8100 File Offset: 0x000A6300
		public InputControlType Target { get; protected set; }

		// Token: 0x1700074E RID: 1870
		// (get) Token: 0x06002743 RID: 10051 RVA: 0x000A810C File Offset: 0x000A630C
		// (set) Token: 0x06002744 RID: 10052 RVA: 0x000A8114 File Offset: 0x000A6314
		public bool IsButton { get; protected set; }

		// Token: 0x1700074F RID: 1871
		// (get) Token: 0x06002745 RID: 10053 RVA: 0x000A8120 File Offset: 0x000A6320
		// (set) Token: 0x06002746 RID: 10054 RVA: 0x000A8128 File Offset: 0x000A6328
		public bool IsAnalog { get; protected set; }

		// Token: 0x06002747 RID: 10055 RVA: 0x000A8134 File Offset: 0x000A6334
		internal void SetZeroTick()
		{
			this.zeroTick = base.UpdateTick;
		}

		// Token: 0x17000750 RID: 1872
		// (get) Token: 0x06002748 RID: 10056 RVA: 0x000A8144 File Offset: 0x000A6344
		internal bool IsOnZeroTick
		{
			get
			{
				return base.UpdateTick == this.zeroTick;
			}
		}

		// Token: 0x17000751 RID: 1873
		// (get) Token: 0x06002749 RID: 10057 RVA: 0x000A8154 File Offset: 0x000A6354
		public bool IsStandard
		{
			get
			{
				return Utility.TargetIsStandard(this.Target);
			}
		}

		// Token: 0x04001AEA RID: 6890
		public static readonly InputControl Null = new InputControl();

		// Token: 0x04001AED RID: 6893
		public bool Passive;

		// Token: 0x04001AF0 RID: 6896
		private ulong zeroTick;
	}
}
