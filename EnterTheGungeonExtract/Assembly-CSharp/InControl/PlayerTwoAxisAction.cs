using System;
using System.Diagnostics;

namespace InControl
{
	// Token: 0x0200069B RID: 1691
	public class PlayerTwoAxisAction : TwoAxisInputControl
	{
		// Token: 0x060026FA RID: 9978 RVA: 0x000A7514 File Offset: 0x000A5714
		internal PlayerTwoAxisAction(PlayerAction negativeXAction, PlayerAction positiveXAction, PlayerAction negativeYAction, PlayerAction positiveYAction)
		{
			this.negativeXAction = negativeXAction;
			this.positiveXAction = positiveXAction;
			this.negativeYAction = negativeYAction;
			this.positiveYAction = positiveYAction;
			this.InvertXAxis = false;
			this.InvertYAxis = false;
			this.Raw = true;
		}

		// Token: 0x1700073B RID: 1851
		// (get) Token: 0x060026FB RID: 9979 RVA: 0x000A7550 File Offset: 0x000A5750
		// (set) Token: 0x060026FC RID: 9980 RVA: 0x000A7558 File Offset: 0x000A5758
		public bool InvertXAxis { get; set; }

		// Token: 0x1700073C RID: 1852
		// (get) Token: 0x060026FD RID: 9981 RVA: 0x000A7564 File Offset: 0x000A5764
		// (set) Token: 0x060026FE RID: 9982 RVA: 0x000A756C File Offset: 0x000A576C
		public bool InvertYAxis { get; set; }

		// Token: 0x14000075 RID: 117
		// (add) Token: 0x060026FF RID: 9983 RVA: 0x000A7578 File Offset: 0x000A5778
		// (remove) Token: 0x06002700 RID: 9984 RVA: 0x000A75B0 File Offset: 0x000A57B0
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action<BindingSourceType> OnLastInputTypeChanged;

		// Token: 0x1700073D RID: 1853
		// (get) Token: 0x06002701 RID: 9985 RVA: 0x000A75E8 File Offset: 0x000A57E8
		// (set) Token: 0x06002702 RID: 9986 RVA: 0x000A75F0 File Offset: 0x000A57F0
		public object UserData { get; set; }

		// Token: 0x06002703 RID: 9987 RVA: 0x000A75FC File Offset: 0x000A57FC
		internal void Update(ulong updateTick, float deltaTime)
		{
			this.ProcessActionUpdate(this.negativeXAction);
			this.ProcessActionUpdate(this.positiveXAction);
			this.ProcessActionUpdate(this.negativeYAction);
			this.ProcessActionUpdate(this.positiveYAction);
			float num = Utility.ValueFromSides(this.negativeXAction, this.positiveXAction, this.InvertXAxis);
			float num2 = Utility.ValueFromSides(this.negativeYAction, this.positiveYAction, InputManager.InvertYAxis || this.InvertYAxis);
			base.UpdateWithAxes(num, num2, updateTick, deltaTime);
		}

		// Token: 0x06002704 RID: 9988 RVA: 0x000A7694 File Offset: 0x000A5894
		private void ProcessActionUpdate(PlayerAction action)
		{
			BindingSourceType bindingSourceType = this.LastInputType;
			if (action.UpdateTick > base.UpdateTick)
			{
				base.UpdateTick = action.UpdateTick;
				bindingSourceType = action.LastInputType;
			}
			if (this.LastInputType != bindingSourceType)
			{
				this.LastInputType = bindingSourceType;
				if (this.OnLastInputTypeChanged != null)
				{
					this.OnLastInputTypeChanged(bindingSourceType);
				}
			}
		}

		// Token: 0x1700073E RID: 1854
		// (get) Token: 0x06002705 RID: 9989 RVA: 0x000A76F8 File Offset: 0x000A58F8
		// (set) Token: 0x06002706 RID: 9990 RVA: 0x000A7700 File Offset: 0x000A5900
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

		// Token: 0x1700073F RID: 1855
		// (get) Token: 0x06002707 RID: 9991 RVA: 0x000A7710 File Offset: 0x000A5910
		// (set) Token: 0x06002708 RID: 9992 RVA: 0x000A7718 File Offset: 0x000A5918
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

		// Token: 0x04001AC2 RID: 6850
		private PlayerAction negativeXAction;

		// Token: 0x04001AC3 RID: 6851
		private PlayerAction positiveXAction;

		// Token: 0x04001AC4 RID: 6852
		private PlayerAction negativeYAction;

		// Token: 0x04001AC5 RID: 6853
		private PlayerAction positiveYAction;

		// Token: 0x04001AC8 RID: 6856
		public BindingSourceType LastInputType;
	}
}
