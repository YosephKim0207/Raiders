using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000B6D RID: 2925
	[ActionCategory(ActionCategory.Time)]
	[Tooltip("Delays a State from finishing by the specified time. NOTE: Other actions continue, but FINISHED can't happen before Time.")]
	public class Wait : FsmStateAction
	{
		// Token: 0x06003D3B RID: 15675 RVA: 0x0013287C File Offset: 0x00130A7C
		public override void Reset()
		{
			this.time = 1f;
			this.finishEvent = null;
			this.realTime = false;
		}

		// Token: 0x06003D3C RID: 15676 RVA: 0x0013289C File Offset: 0x00130A9C
		public override void OnEnter()
		{
			if (this.time.Value <= 0f)
			{
				base.Fsm.Event(this.finishEvent);
				base.Finish();
				return;
			}
			this.startTime = FsmTime.RealtimeSinceStartup;
			this.timer = 0f;
		}

		// Token: 0x06003D3D RID: 15677 RVA: 0x001328EC File Offset: 0x00130AEC
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
			if (this.timer >= this.time.Value)
			{
				base.Finish();
				if (this.finishEvent != null)
				{
					base.Fsm.Event(this.finishEvent);
				}
			}
		}

		// Token: 0x04002F8E RID: 12174
		[RequiredField]
		public FsmFloat time;

		// Token: 0x04002F8F RID: 12175
		public FsmEvent finishEvent;

		// Token: 0x04002F90 RID: 12176
		public bool realTime;

		// Token: 0x04002F91 RID: 12177
		private float startTime;

		// Token: 0x04002F92 RID: 12178
		private float timer;
	}
}
