using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000C4A RID: 3146
	[ActionCategory("2D Toolkit/TextMesh")]
	[Tooltip("Set the scale of a TextMesh. \nChanges will not be updated if commit is OFF. This is so you can change multiple parameters without reconstructing the mesh repeatedly.\n Use tk2dtextMeshCommit or set commit to true on your last change for that mesh. \nNOTE: The Game Object must have a tk2dTextMesh attached.")]
	public class Tk2dTextMeshSetScale : FsmStateAction
	{
		// Token: 0x060043CD RID: 17357 RVA: 0x0015E488 File Offset: 0x0015C688
		private void _getTextMesh()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
			if (ownerDefaultTarget == null)
			{
				return;
			}
			this._textMesh = ownerDefaultTarget.GetComponent<tk2dTextMesh>();
		}

		// Token: 0x060043CE RID: 17358 RVA: 0x0015E4C0 File Offset: 0x0015C6C0
		public override void Reset()
		{
			this.gameObject = null;
			this.scale = null;
			this.commit = true;
			this.everyframe = false;
		}

		// Token: 0x060043CF RID: 17359 RVA: 0x0015E4E4 File Offset: 0x0015C6E4
		public override void OnEnter()
		{
			this._getTextMesh();
			this.DoSetScale();
			if (!this.everyframe)
			{
				base.Finish();
			}
		}

		// Token: 0x060043D0 RID: 17360 RVA: 0x0015E504 File Offset: 0x0015C704
		public override void OnUpdate()
		{
			this.DoSetScale();
		}

		// Token: 0x060043D1 RID: 17361 RVA: 0x0015E50C File Offset: 0x0015C70C
		private void DoSetScale()
		{
			if (this._textMesh == null)
			{
				base.LogWarning("Missing tk2dTextMesh component: " + this._textMesh.gameObject.name);
				return;
			}
			if (this._textMesh.scale != this.scale.Value)
			{
				this._textMesh.scale = this.scale.Value;
				if (this.commit.Value)
				{
					this._textMesh.Commit();
				}
			}
		}

		// Token: 0x040035E9 RID: 13801
		[RequiredField]
		[Tooltip("The Game Object to work with. NOTE: The Game Object must have a tk2dTextMesh component attached.")]
		[CheckForComponent(typeof(tk2dTextMesh))]
		public FsmOwnerDefault gameObject;

		// Token: 0x040035EA RID: 13802
		[UIHint(UIHint.FsmVector3)]
		[Tooltip("The scale")]
		public FsmVector3 scale;

		// Token: 0x040035EB RID: 13803
		[UIHint(UIHint.FsmBool)]
		[Tooltip("Commit changes")]
		public FsmBool commit;

		// Token: 0x040035EC RID: 13804
		[Tooltip("Repeat every frame.")]
		[ActionSection("")]
		public bool everyframe;

		// Token: 0x040035ED RID: 13805
		private tk2dTextMesh _textMesh;
	}
}
