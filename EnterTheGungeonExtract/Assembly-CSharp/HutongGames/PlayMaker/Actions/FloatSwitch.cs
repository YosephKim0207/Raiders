using System;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000957 RID: 2391
	[ActionCategory(ActionCategory.Logic)]
	[Tooltip("Sends an Event based on the value of a Float Variable. The float could represent distance, angle to a target, health left... The array sets up float ranges that correspond to Events.")]
	public class FloatSwitch : FsmStateAction
	{
		// Token: 0x06003438 RID: 13368 RVA: 0x0010F754 File Offset: 0x0010D954
		public override void Reset()
		{
			this.floatVariable = null;
			this.lessThan = new FsmFloat[1];
			this.sendEvent = new FsmEvent[1];
			this.everyFrame = false;
		}

		// Token: 0x06003439 RID: 13369 RVA: 0x0010F77C File Offset: 0x0010D97C
		public override void OnEnter()
		{
			this.DoFloatSwitch();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x0600343A RID: 13370 RVA: 0x0010F798 File Offset: 0x0010D998
		public override void OnUpdate()
		{
			this.DoFloatSwitch();
		}

		// Token: 0x0600343B RID: 13371 RVA: 0x0010F7A0 File Offset: 0x0010D9A0
		private void DoFloatSwitch()
		{
			if (this.floatVariable.IsNone)
			{
				return;
			}
			for (int i = 0; i < this.lessThan.Length; i++)
			{
				if (this.floatVariable.Value < this.lessThan[i].Value)
				{
					base.Fsm.Event(this.sendEvent[i]);
					return;
				}
			}
		}

		// Token: 0x0400255B RID: 9563
		[UIHint(UIHint.Variable)]
		[Tooltip("The float variable to test.")]
		[RequiredField]
		public FsmFloat floatVariable;

		// Token: 0x0400255C RID: 9564
		[CompoundArray("Float Switches", "Less Than", "Send Event")]
		public FsmFloat[] lessThan;

		// Token: 0x0400255D RID: 9565
		public FsmEvent[] sendEvent;

		// Token: 0x0400255E RID: 9566
		[Tooltip("Repeat every frame. Useful if the variable is changing.")]
		public bool everyFrame;
	}
}
