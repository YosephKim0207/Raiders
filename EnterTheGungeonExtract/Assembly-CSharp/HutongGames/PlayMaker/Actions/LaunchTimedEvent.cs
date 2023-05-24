using System;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000C5E RID: 3166
	[ActionCategory(".Brave")]
	public class LaunchTimedEvent : FsmStateAction
	{
		// Token: 0x0600442A RID: 17450 RVA: 0x00160390 File Offset: 0x0015E590
		public override void Reset()
		{
		}

		// Token: 0x0600442B RID: 17451 RVA: 0x00160394 File Offset: 0x0015E594
		public override void OnEnter()
		{
			GameManager.Instance.LaunchTimedEvent(this.AllotedTime, delegate(bool a)
			{
				GameStatsManager.Instance.SetFlag(this.targetFlag, a);
			});
			base.Finish();
		}

		// Token: 0x0400363E RID: 13886
		public GungeonFlags targetFlag;

		// Token: 0x0400363F RID: 13887
		public float AllotedTime = 60f;
	}
}
