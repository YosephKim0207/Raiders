using System;
using Dungeonator;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000CA4 RID: 3236
	public class ModifyVariableByInflationRate : FsmStateAction
	{
		// Token: 0x06004527 RID: 17703 RVA: 0x0016679C File Offset: 0x0016499C
		public override void Reset()
		{
		}

		// Token: 0x06004528 RID: 17704 RVA: 0x001667A0 File Offset: 0x001649A0
		public override string ErrorCheck()
		{
			return string.Empty;
		}

		// Token: 0x06004529 RID: 17705 RVA: 0x001667B4 File Offset: 0x001649B4
		public override void OnEnter()
		{
			GameLevelDefinition lastLoadedLevelDefinition = GameManager.Instance.GetLastLoadedLevelDefinition();
			float num = ((lastLoadedLevelDefinition == null) ? 1f : lastLoadedLevelDefinition.priceMultiplier);
			this.TargetVariable.Value = Mathf.RoundToInt((float)this.TargetVariable.Value * num * this.AdditionalMultiplier.Value);
			if (base.Owner)
			{
				RoomHandler absoluteRoom = base.Owner.transform.position.GetAbsoluteRoom();
				if (absoluteRoom != null && absoluteRoom.connectedRooms != null && absoluteRoom.connectedRooms.Count == 1 && absoluteRoom.connectedRooms[0].area.PrototypeRoomName.Contains("Black Market"))
				{
					this.TargetVariable.Value = Mathf.RoundToInt((float)this.TargetVariable.Value * 0.5f);
				}
			}
			base.Finish();
		}

		// Token: 0x04003752 RID: 14162
		public FsmInt TargetVariable;

		// Token: 0x04003753 RID: 14163
		public FsmFloat AdditionalMultiplier = 1f;
	}
}
