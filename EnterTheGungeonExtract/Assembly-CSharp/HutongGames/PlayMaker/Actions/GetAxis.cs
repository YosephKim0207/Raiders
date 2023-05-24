using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000967 RID: 2407
	[ActionCategory(ActionCategory.Input)]
	[Tooltip("Gets the value of the specified Input Axis and stores it in a Float Variable. See Unity Input Manager docs.")]
	public class GetAxis : FsmStateAction
	{
		// Token: 0x0600347F RID: 13439 RVA: 0x00110418 File Offset: 0x0010E618
		public override void Reset()
		{
			this.axisName = string.Empty;
			this.multiplier = 1f;
			this.store = null;
			this.everyFrame = true;
		}

		// Token: 0x06003480 RID: 13440 RVA: 0x00110448 File Offset: 0x0010E648
		public override void OnEnter()
		{
			this.DoGetAxis();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003481 RID: 13441 RVA: 0x00110464 File Offset: 0x0010E664
		public override void OnUpdate()
		{
			this.DoGetAxis();
		}

		// Token: 0x06003482 RID: 13442 RVA: 0x0011046C File Offset: 0x0010E66C
		private void DoGetAxis()
		{
			if (FsmString.IsNullOrEmpty(this.axisName))
			{
				return;
			}
			float num = Input.GetAxis(this.axisName.Value);
			if (!this.multiplier.IsNone)
			{
				num *= this.multiplier.Value;
			}
			this.store.Value = num;
		}

		// Token: 0x040025AD RID: 9645
		[Tooltip("The name of the axis. Set in the Unity Input Manager.")]
		[RequiredField]
		public FsmString axisName;

		// Token: 0x040025AE RID: 9646
		[Tooltip("Axis values are in the range -1 to 1. Use the multiplier to set a larger range.")]
		public FsmFloat multiplier;

		// Token: 0x040025AF RID: 9647
		[Tooltip("Store the result in a float variable.")]
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmFloat store;

		// Token: 0x040025B0 RID: 9648
		[Tooltip("Repeat every frame. Typically this would be set to True.")]
		public bool everyFrame;
	}
}
