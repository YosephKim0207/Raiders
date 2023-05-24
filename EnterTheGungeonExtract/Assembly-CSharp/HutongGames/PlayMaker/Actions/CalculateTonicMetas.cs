using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000C51 RID: 3153
	[ActionCategory(".NPCs")]
	public class CalculateTonicMetas : FsmStateAction
	{
		// Token: 0x060043EB RID: 17387 RVA: 0x0015ECFC File Offset: 0x0015CEFC
		public override void OnEnter()
		{
			int num = Mathf.RoundToInt(GameStatsManager.Instance.GetPlayerStatValue(TrackedStats.META_CURRENCY));
			FsmInt fsmInt = base.Fsm.Variables.FindFsmInt("npcNumber1");
			FsmFloat fsmFloat = base.Fsm.Variables.FindFsmFloat("costFloat");
			fsmInt.Value = Mathf.RoundToInt(((float)num * 0.9f).Quantize(50f));
			fsmFloat.Value = (float)fsmInt.Value;
			base.Finish();
		}
	}
}
