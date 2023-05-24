using System;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000C5B RID: 3163
	[Tooltip("Sends an Event based on the current floor.")]
	[ActionCategory(ActionCategory.Logic)]
	public class GungeonFloorSwitch : FsmStateAction
	{
		// Token: 0x06004421 RID: 17441 RVA: 0x0016017C File Offset: 0x0015E37C
		public override void Reset()
		{
			this.compareTo = new GlobalDungeonData.ValidTilesets[1];
			this.sendEvent = new FsmEvent[1];
			this.everyFrame = false;
		}

		// Token: 0x06004422 RID: 17442 RVA: 0x001601A0 File Offset: 0x0015E3A0
		public override void OnEnter()
		{
			if (this.DoSendEvent)
			{
				this.DoFloorSwitch();
			}
			if (this.ChangeVariable)
			{
				this.DoVariableSwitch();
			}
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06004423 RID: 17443 RVA: 0x001601D8 File Offset: 0x0015E3D8
		public override void OnUpdate()
		{
			if (this.DoSendEvent)
			{
				this.DoFloorSwitch();
			}
			if (this.ChangeVariable)
			{
				this.DoVariableSwitch();
			}
		}

		// Token: 0x06004424 RID: 17444 RVA: 0x001601FC File Offset: 0x0015E3FC
		private void DoVariableSwitch()
		{
			for (int i = 0; i < this.varCompareTo.Length; i++)
			{
				if (GameManager.Instance.Dungeon.tileIndices.tilesetId == this.varCompareTo[i])
				{
					this.targetVariable.Value = this.targetStrings[i].Value;
					break;
				}
			}
			base.Finish();
		}

		// Token: 0x06004425 RID: 17445 RVA: 0x00160268 File Offset: 0x0015E468
		private void DoFloorSwitch()
		{
			for (int i = 0; i < this.compareTo.Length; i++)
			{
				if (GameManager.Instance.Dungeon.tileIndices.tilesetId == this.compareTo[i])
				{
					base.Fsm.Event(this.sendEvent[i]);
					return;
				}
			}
		}

		// Token: 0x04003632 RID: 13874
		public bool DoSendEvent = true;

		// Token: 0x04003633 RID: 13875
		public bool ChangeVariable;

		// Token: 0x04003634 RID: 13876
		public GlobalDungeonData.ValidTilesets[] compareTo;

		// Token: 0x04003635 RID: 13877
		public FsmEvent[] sendEvent;

		// Token: 0x04003636 RID: 13878
		public GlobalDungeonData.ValidTilesets[] varCompareTo;

		// Token: 0x04003637 RID: 13879
		public FsmString[] targetStrings;

		// Token: 0x04003638 RID: 13880
		public FsmString targetVariable;

		// Token: 0x04003639 RID: 13881
		public bool everyFrame;
	}
}
