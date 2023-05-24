using System;
using System.Diagnostics;

namespace InControl
{
	// Token: 0x0200069A RID: 1690
	public class PlayerOneAxisAction : OneAxisInputControl
	{
		// Token: 0x060026EF RID: 9967 RVA: 0x000A7390 File Offset: 0x000A5590
		internal PlayerOneAxisAction(PlayerAction negativeAction, PlayerAction positiveAction)
		{
			this.negativeAction = negativeAction;
			this.positiveAction = positiveAction;
			this.Raw = true;
		}

		// Token: 0x14000074 RID: 116
		// (add) Token: 0x060026F0 RID: 9968 RVA: 0x000A73B0 File Offset: 0x000A55B0
		// (remove) Token: 0x060026F1 RID: 9969 RVA: 0x000A73E8 File Offset: 0x000A55E8
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action<BindingSourceType> OnLastInputTypeChanged;

		// Token: 0x17000738 RID: 1848
		// (get) Token: 0x060026F2 RID: 9970 RVA: 0x000A7420 File Offset: 0x000A5620
		// (set) Token: 0x060026F3 RID: 9971 RVA: 0x000A7428 File Offset: 0x000A5628
		public object UserData { get; set; }

		// Token: 0x060026F4 RID: 9972 RVA: 0x000A7434 File Offset: 0x000A5634
		internal void Update(ulong updateTick, float deltaTime)
		{
			this.ProcessActionUpdate(this.negativeAction);
			this.ProcessActionUpdate(this.positiveAction);
			float num = Utility.ValueFromSides(this.negativeAction, this.positiveAction);
			base.CommitWithValue(num, updateTick, deltaTime);
		}

		// Token: 0x060026F5 RID: 9973 RVA: 0x000A7480 File Offset: 0x000A5680
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

		// Token: 0x17000739 RID: 1849
		// (get) Token: 0x060026F6 RID: 9974 RVA: 0x000A74E4 File Offset: 0x000A56E4
		// (set) Token: 0x060026F7 RID: 9975 RVA: 0x000A74EC File Offset: 0x000A56EC
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

		// Token: 0x1700073A RID: 1850
		// (get) Token: 0x060026F8 RID: 9976 RVA: 0x000A74FC File Offset: 0x000A56FC
		// (set) Token: 0x060026F9 RID: 9977 RVA: 0x000A7504 File Offset: 0x000A5704
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

		// Token: 0x04001ABD RID: 6845
		private PlayerAction negativeAction;

		// Token: 0x04001ABE RID: 6846
		private PlayerAction positiveAction;

		// Token: 0x04001ABF RID: 6847
		public BindingSourceType LastInputType;
	}
}
