using System;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000C68 RID: 3176
	[Tooltip("Sets a flag on the player's save data.")]
	[ActionCategory(".Brave")]
	public class SetCharacterSpecificSaveFlag : FsmStateAction
	{
		// Token: 0x0600444B RID: 17483 RVA: 0x001610D4 File Offset: 0x0015F2D4
		public override string ErrorCheck()
		{
			string text = string.Empty;
			if (this.targetFlag == CharacterSpecificGungeonFlags.NONE)
			{
				text += "Target flag is NONE. This is a mistake.";
			}
			return text;
		}

		// Token: 0x0600444C RID: 17484 RVA: 0x00161100 File Offset: 0x0015F300
		public override void Reset()
		{
			this.targetFlag = CharacterSpecificGungeonFlags.NONE;
			this.value = false;
		}

		// Token: 0x0600444D RID: 17485 RVA: 0x00161118 File Offset: 0x0015F318
		public override void OnEnter()
		{
			GameStatsManager.Instance.SetCharacterSpecificFlag(this.targetFlag, this.value.Value);
			base.Finish();
		}

		// Token: 0x04003660 RID: 13920
		[Tooltip("The flag.")]
		public CharacterSpecificGungeonFlags targetFlag;

		// Token: 0x04003661 RID: 13921
		[Tooltip("The value to set the flag to.")]
		public FsmBool value;
	}
}
