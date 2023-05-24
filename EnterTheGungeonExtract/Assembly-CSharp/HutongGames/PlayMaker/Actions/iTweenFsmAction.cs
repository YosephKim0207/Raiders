using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x020009F8 RID: 2552
	[Tooltip("iTween base action - don't use!")]
	public abstract class iTweenFsmAction : FsmStateAction
	{
		// Token: 0x060036C1 RID: 14017 RVA: 0x001177B4 File Offset: 0x001159B4
		public override void Reset()
		{
			this.startEvent = null;
			this.finishEvent = null;
			this.realTime = new FsmBool
			{
				Value = false
			};
			this.stopOnExit = new FsmBool
			{
				Value = true
			};
			this.loopDontFinish = new FsmBool
			{
				Value = true
			};
			this.itweenType = string.Empty;
		}

		// Token: 0x060036C2 RID: 14018 RVA: 0x00117818 File Offset: 0x00115A18
		protected void OnEnteriTween(FsmOwnerDefault anOwner)
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(anOwner);
			if (ownerDefaultTarget == null)
			{
				return;
			}
			this.itweenEvents = ownerDefaultTarget.AddComponent<iTweenFSMEvents>();
			this.itweenEvents.itweenFSMAction = this;
			iTweenFSMEvents.itweenIDCount++;
			this.itweenID = iTweenFSMEvents.itweenIDCount;
			this.itweenEvents.itweenID = iTweenFSMEvents.itweenIDCount;
			this.itweenEvents.donotfinish = !this.loopDontFinish.IsNone && this.loopDontFinish.Value;
		}

		// Token: 0x060036C3 RID: 14019 RVA: 0x001178AC File Offset: 0x00115AAC
		protected void IsLoop(bool aValue)
		{
			if (this.itweenEvents != null)
			{
				this.itweenEvents.islooping = aValue;
			}
		}

		// Token: 0x060036C4 RID: 14020 RVA: 0x001178CC File Offset: 0x00115ACC
		protected void OnExitiTween(FsmOwnerDefault anOwner)
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(anOwner);
			if (ownerDefaultTarget == null)
			{
				return;
			}
			if (this.itweenEvents)
			{
				UnityEngine.Object.Destroy(this.itweenEvents);
			}
			if (this.stopOnExit.IsNone)
			{
				iTween.Stop(ownerDefaultTarget, this.itweenType);
			}
			else if (this.stopOnExit.Value)
			{
				iTween.Stop(ownerDefaultTarget, this.itweenType);
			}
		}

		// Token: 0x04002821 RID: 10273
		[ActionSection("Events")]
		public FsmEvent startEvent;

		// Token: 0x04002822 RID: 10274
		public FsmEvent finishEvent;

		// Token: 0x04002823 RID: 10275
		[Tooltip("Setting this to true will allow the animation to continue independent of the current time which is helpful for animating menus after a game has been paused by setting Time.timeScale=0.")]
		public FsmBool realTime;

		// Token: 0x04002824 RID: 10276
		public FsmBool stopOnExit;

		// Token: 0x04002825 RID: 10277
		public FsmBool loopDontFinish;

		// Token: 0x04002826 RID: 10278
		internal iTweenFSMEvents itweenEvents;

		// Token: 0x04002827 RID: 10279
		protected string itweenType = string.Empty;

		// Token: 0x04002828 RID: 10280
		protected int itweenID = -1;

		// Token: 0x020009F9 RID: 2553
		public enum AxisRestriction
		{
			// Token: 0x0400282A RID: 10282
			none,
			// Token: 0x0400282B RID: 10283
			x,
			// Token: 0x0400282C RID: 10284
			y,
			// Token: 0x0400282D RID: 10285
			z,
			// Token: 0x0400282E RID: 10286
			xy,
			// Token: 0x0400282F RID: 10287
			xz,
			// Token: 0x04002830 RID: 10288
			yz
		}
	}
}
