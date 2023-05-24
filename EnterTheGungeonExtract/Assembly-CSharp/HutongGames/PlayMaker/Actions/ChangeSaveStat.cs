using System;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000C52 RID: 3154
	[Tooltip("Sends Events based on the data in a player save.")]
	[ActionCategory(".Brave")]
	public class ChangeSaveStat : FsmStateAction
	{
		// Token: 0x060043ED RID: 17389 RVA: 0x0015ED80 File Offset: 0x0015CF80
		public override void Reset()
		{
			this.stat = TrackedStats.BULLETS_FIRED;
			this.statChange = 0f;
		}

		// Token: 0x060043EE RID: 17390 RVA: 0x0015ED9C File Offset: 0x0015CF9C
		public override string ErrorCheck()
		{
			return string.Empty;
		}

		// Token: 0x060043EF RID: 17391 RVA: 0x0015EDB0 File Offset: 0x0015CFB0
		public override void OnEnter()
		{
			GameStatsManager.Instance.RegisterStatChange(this.stat, this.statChange.Value);
			base.Finish();
		}

		// Token: 0x0400360B RID: 13835
		public TrackedStats stat;

		// Token: 0x0400360C RID: 13836
		public FsmFloat statChange;
	}
}
