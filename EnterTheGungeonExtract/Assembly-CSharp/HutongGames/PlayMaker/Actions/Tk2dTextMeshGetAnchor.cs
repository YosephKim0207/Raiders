using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000C36 RID: 3126
	[Tooltip("Get the anchor of a TextMesh. \nThe anchor is stored as a string. tk2dTextMeshSetAnchor can work with this string. \nNOTE: The Game Object must have a tk2dTextMesh attached.")]
	[ActionCategory("2D Toolkit/TextMesh")]
	public class Tk2dTextMeshGetAnchor : FsmStateAction
	{
		// Token: 0x0600435D RID: 17245 RVA: 0x0015CE5C File Offset: 0x0015B05C
		private void _getTextMesh()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
			if (ownerDefaultTarget == null)
			{
				return;
			}
			this._textMesh = ownerDefaultTarget.GetComponent<tk2dTextMesh>();
		}

		// Token: 0x0600435E RID: 17246 RVA: 0x0015CE94 File Offset: 0x0015B094
		public override void Reset()
		{
			this.gameObject = null;
			this.textAnchorAsString = string.Empty;
			this.everyframe = false;
		}

		// Token: 0x0600435F RID: 17247 RVA: 0x0015CEB4 File Offset: 0x0015B0B4
		public override void OnEnter()
		{
			this._getTextMesh();
			this.DoGetAnchor();
			if (!this.everyframe)
			{
				base.Finish();
			}
		}

		// Token: 0x06004360 RID: 17248 RVA: 0x0015CED4 File Offset: 0x0015B0D4
		public override void OnUpdate()
		{
			this.DoGetAnchor();
		}

		// Token: 0x06004361 RID: 17249 RVA: 0x0015CEDC File Offset: 0x0015B0DC
		private void DoGetAnchor()
		{
			if (this._textMesh == null)
			{
				base.LogWarning("Missing tk2dTextMesh component");
				return;
			}
			this.textAnchorAsString.Value = this._textMesh.anchor.ToString();
		}

		// Token: 0x04003580 RID: 13696
		[RequiredField]
		[Tooltip("The Game Object to work with. NOTE: The Game Object must have a tk2dTextMesh component attached.")]
		[CheckForComponent(typeof(tk2dTextMesh))]
		public FsmOwnerDefault gameObject;

		// Token: 0x04003581 RID: 13697
		[UIHint(UIHint.Variable)]
		[RequiredField]
		[Tooltip("The anchor as a string. \npossible values: LowerLeft,LowerCenter,LowerRight,MiddleLeft,MiddleCenter,MiddleRight,UpperLeft,UpperCenter or UpperRight ")]
		public FsmString textAnchorAsString;

		// Token: 0x04003582 RID: 13698
		[Tooltip("Repeat every frame.")]
		[ActionSection("")]
		public bool everyframe;

		// Token: 0x04003583 RID: 13699
		private tk2dTextMesh _textMesh;
	}
}
