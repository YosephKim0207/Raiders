using System;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000A94 RID: 2708
	public abstract class QuaternionBaseAction : FsmStateAction
	{
		// Token: 0x0600397B RID: 14715 RVA: 0x00125DE4 File Offset: 0x00123FE4
		public override void Awake()
		{
			if (this.everyFrame && this.everyFrameOption == QuaternionBaseAction.everyFrameOptions.FixedUpdate)
			{
				base.Fsm.HandleFixedUpdate = true;
			}
		}

		// Token: 0x04002BBC RID: 11196
		[Tooltip("Repeat every frame. Useful if any of the values are changing.")]
		public bool everyFrame;

		// Token: 0x04002BBD RID: 11197
		[Tooltip("Defines how to perform the action when 'every Frame' is enabled.")]
		public QuaternionBaseAction.everyFrameOptions everyFrameOption;

		// Token: 0x02000A95 RID: 2709
		public enum everyFrameOptions
		{
			// Token: 0x04002BBF RID: 11199
			Update,
			// Token: 0x04002BC0 RID: 11200
			FixedUpdate,
			// Token: 0x04002BC1 RID: 11201
			LateUpdate
		}
	}
}
