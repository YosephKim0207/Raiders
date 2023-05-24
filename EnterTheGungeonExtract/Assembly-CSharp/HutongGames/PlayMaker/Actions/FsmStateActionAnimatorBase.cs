using System;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x0200089F RID: 2207
	public abstract class FsmStateActionAnimatorBase : FsmStateAction
	{
		// Token: 0x0600311F RID: 12575
		public abstract void OnActionUpdate();

		// Token: 0x06003120 RID: 12576 RVA: 0x001049B8 File Offset: 0x00102BB8
		public override void Reset()
		{
			this.everyFrame = false;
			this.everyFrameOption = FsmStateActionAnimatorBase.AnimatorFrameUpdateSelector.OnUpdate;
		}

		// Token: 0x06003121 RID: 12577 RVA: 0x001049C8 File Offset: 0x00102BC8
		public override void OnPreprocess()
		{
			if (this.everyFrameOption == FsmStateActionAnimatorBase.AnimatorFrameUpdateSelector.OnAnimatorMove)
			{
				base.Fsm.HandleAnimatorMove = true;
			}
			if (this.everyFrameOption == FsmStateActionAnimatorBase.AnimatorFrameUpdateSelector.OnAnimatorIK)
			{
				base.Fsm.HandleAnimatorIK = true;
			}
		}

		// Token: 0x06003122 RID: 12578 RVA: 0x001049FC File Offset: 0x00102BFC
		public override void OnUpdate()
		{
			if (this.everyFrameOption == FsmStateActionAnimatorBase.AnimatorFrameUpdateSelector.OnUpdate)
			{
				this.OnActionUpdate();
			}
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003123 RID: 12579 RVA: 0x00104A20 File Offset: 0x00102C20
		public override void DoAnimatorMove()
		{
			if (this.everyFrameOption == FsmStateActionAnimatorBase.AnimatorFrameUpdateSelector.OnAnimatorMove)
			{
				this.OnActionUpdate();
			}
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003124 RID: 12580 RVA: 0x00104A48 File Offset: 0x00102C48
		public override void DoAnimatorIK(int layerIndex)
		{
			this.IklayerIndex = layerIndex;
			if (this.everyFrameOption == FsmStateActionAnimatorBase.AnimatorFrameUpdateSelector.OnAnimatorIK)
			{
				this.OnActionUpdate();
			}
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x0400222B RID: 8747
		[Tooltip("Repeat every frame.")]
		public bool everyFrame;

		// Token: 0x0400222C RID: 8748
		[Tooltip("Select when to perform the action, during OnUpdate, OnAnimatorMove, OnAnimatorIK")]
		public FsmStateActionAnimatorBase.AnimatorFrameUpdateSelector everyFrameOption;

		// Token: 0x0400222D RID: 8749
		protected int IklayerIndex;

		// Token: 0x020008A0 RID: 2208
		public enum AnimatorFrameUpdateSelector
		{
			// Token: 0x0400222F RID: 8751
			OnUpdate,
			// Token: 0x04002230 RID: 8752
			OnAnimatorMove,
			// Token: 0x04002231 RID: 8753
			OnAnimatorIK
		}
	}
}
