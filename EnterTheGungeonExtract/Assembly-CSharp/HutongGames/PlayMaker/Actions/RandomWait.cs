using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000AA2 RID: 2722
	[ActionCategory(ActionCategory.Time)]
	[Tooltip("Delays a State from finishing by a random time. NOTE: Other actions continue, but FINISHED can't happen before Time.")]
	public class RandomWait : FsmStateAction
	{
		// Token: 0x060039C3 RID: 14787 RVA: 0x0012691C File Offset: 0x00124B1C
		public override void Reset()
		{
			this.min = 0f;
			this.max = 1f;
			this.finishEvent = null;
			this.realTime = false;
		}

		// Token: 0x060039C4 RID: 14788 RVA: 0x0012694C File Offset: 0x00124B4C
		public override void OnEnter()
		{
			this.time = UnityEngine.Random.Range(this.min.Value, this.max.Value);
			if (this.time <= 0f)
			{
				base.Fsm.Event(this.finishEvent);
				base.Finish();
				return;
			}
			this.startTime = FsmTime.RealtimeSinceStartup;
			this.timer = 0f;
		}

		// Token: 0x060039C5 RID: 14789 RVA: 0x001269B8 File Offset: 0x00124BB8
		public override void OnUpdate()
		{
			if (this.realTime)
			{
				this.timer = FsmTime.RealtimeSinceStartup - this.startTime;
			}
			else
			{
				this.timer += Time.deltaTime;
			}
			if (this.timer >= this.time)
			{
				base.Finish();
				if (this.finishEvent != null)
				{
					base.Fsm.Event(this.finishEvent);
				}
			}
		}

		// Token: 0x04002BEA RID: 11242
		[Tooltip("Minimum amount of time to wait.")]
		[RequiredField]
		public FsmFloat min;

		// Token: 0x04002BEB RID: 11243
		[Tooltip("Maximum amount of time to wait.")]
		[RequiredField]
		public FsmFloat max;

		// Token: 0x04002BEC RID: 11244
		[Tooltip("Event to send when timer is finished.")]
		public FsmEvent finishEvent;

		// Token: 0x04002BED RID: 11245
		[Tooltip("Ignore time scale.")]
		public bool realTime;

		// Token: 0x04002BEE RID: 11246
		private float startTime;

		// Token: 0x04002BEF RID: 11247
		private float timer;

		// Token: 0x04002BF0 RID: 11248
		private float time;
	}
}
