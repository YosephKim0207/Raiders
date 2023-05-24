using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000AC1 RID: 2753
	[ActionCategory(ActionCategory.RenderSettings)]
	[Tooltip("Sets the Ambient Light Color for the scene.")]
	public class SetAmbientLight : FsmStateAction
	{
		// Token: 0x06003A59 RID: 14937 RVA: 0x00128F80 File Offset: 0x00127180
		public override void Reset()
		{
			this.ambientColor = Color.gray;
			this.everyFrame = false;
		}

		// Token: 0x06003A5A RID: 14938 RVA: 0x00128F9C File Offset: 0x0012719C
		public override void OnEnter()
		{
			this.DoSetAmbientColor();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003A5B RID: 14939 RVA: 0x00128FB8 File Offset: 0x001271B8
		public override void OnUpdate()
		{
			this.DoSetAmbientColor();
		}

		// Token: 0x06003A5C RID: 14940 RVA: 0x00128FC0 File Offset: 0x001271C0
		private void DoSetAmbientColor()
		{
			RenderSettings.ambientLight = this.ambientColor.Value;
		}

		// Token: 0x04002C8F RID: 11407
		[RequiredField]
		public FsmColor ambientColor;

		// Token: 0x04002C90 RID: 11408
		public bool everyFrame;
	}
}
