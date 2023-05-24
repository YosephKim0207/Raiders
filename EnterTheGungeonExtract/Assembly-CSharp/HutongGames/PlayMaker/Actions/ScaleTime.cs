using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000AB2 RID: 2738
	[Tooltip("Scales time: 1 = normal, 0.5 = half speed, 2 = double speed.")]
	[ActionCategory(ActionCategory.Time)]
	public class ScaleTime : FsmStateAction
	{
		// Token: 0x06003A1E RID: 14878 RVA: 0x00127EA0 File Offset: 0x001260A0
		public override void Reset()
		{
			this.timeScale = 1f;
			this.adjustFixedDeltaTime = true;
			this.everyFrame = false;
		}

		// Token: 0x06003A1F RID: 14879 RVA: 0x00127EC8 File Offset: 0x001260C8
		public override void OnEnter()
		{
			this.DoTimeScale();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003A20 RID: 14880 RVA: 0x00127EE4 File Offset: 0x001260E4
		public override void OnUpdate()
		{
			this.DoTimeScale();
		}

		// Token: 0x06003A21 RID: 14881 RVA: 0x00127EEC File Offset: 0x001260EC
		private void DoTimeScale()
		{
			Time.timeScale = this.timeScale.Value;
			Time.fixedDeltaTime = 0.02f * Time.timeScale;
		}

		// Token: 0x04002C46 RID: 11334
		[Tooltip("Scales time: 1 = normal, 0.5 = half speed, 2 = double speed.")]
		[HasFloatSlider(0f, 4f)]
		[RequiredField]
		public FsmFloat timeScale;

		// Token: 0x04002C47 RID: 11335
		[Tooltip("Adjust the fixed physics time step to match the time scale.")]
		public FsmBool adjustFixedDeltaTime;

		// Token: 0x04002C48 RID: 11336
		[Tooltip("Repeat every frame. Useful when animating the value.")]
		public bool everyFrame;
	}
}
