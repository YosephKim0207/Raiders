using System;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000C6B RID: 3179
	[ActionCategory(".Brave")]
	[Tooltip("Sets a flag on the player's save data.")]
	public class SetSaveFlag : FsmStateAction
	{
		// Token: 0x06004457 RID: 17495 RVA: 0x0016136C File Offset: 0x0015F56C
		public override string ErrorCheck()
		{
			string text = string.Empty;
			if (this.targetFlag == GungeonFlags.NONE)
			{
				text += "Target flag is NONE. This is a mistake.";
			}
			return text;
		}

		// Token: 0x06004458 RID: 17496 RVA: 0x00161398 File Offset: 0x0015F598
		public override void Reset()
		{
			this.targetFlag = GungeonFlags.NONE;
			this.value = false;
		}

		// Token: 0x06004459 RID: 17497 RVA: 0x001613B0 File Offset: 0x0015F5B0
		public override void OnEnter()
		{
			GameStatsManager.Instance.SetFlag(this.targetFlag, this.value.Value);
			base.Finish();
		}

		// Token: 0x04003667 RID: 13927
		[Tooltip("The flag.")]
		public GungeonFlags targetFlag;

		// Token: 0x04003668 RID: 13928
		[Tooltip("The value to set the flag to.")]
		public FsmBool value;
	}
}
