using System;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000C56 RID: 3158
	[Tooltip("Delays for a specified amount of time.")]
	[ActionCategory(".Brave")]
	public class Delay : FsmStateAction
	{
		// Token: 0x06004408 RID: 17416 RVA: 0x0015F80C File Offset: 0x0015DA0C
		public override void OnEnter()
		{
			this.timer = 0f;
			this.firstFrame = true;
			if (this.time.Value <= 0f)
			{
				base.Finish();
			}
		}

		// Token: 0x06004409 RID: 17417 RVA: 0x0015F83C File Offset: 0x0015DA3C
		public override void OnUpdate()
		{
			if (this.firstFrame)
			{
				this.firstFrame = false;
				return;
			}
			this.timer += BraveTime.DeltaTime;
			if (this.timer >= this.time.Value)
			{
				base.Finish();
			}
		}

		// Token: 0x0400361E RID: 13854
		[Tooltip("How many seconds to delay for (this action will not finish until the time has passed).")]
		public FsmFloat time;

		// Token: 0x0400361F RID: 13855
		private bool firstFrame;

		// Token: 0x04003620 RID: 13856
		private float timer;
	}
}
