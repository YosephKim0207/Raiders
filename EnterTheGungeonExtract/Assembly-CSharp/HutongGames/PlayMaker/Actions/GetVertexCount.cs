using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x020009BF RID: 2495
	[ActionCategory("Mesh")]
	[Tooltip("Gets the number of vertices in a GameObject's mesh. Useful in conjunction with GetVertexPosition.")]
	public class GetVertexCount : FsmStateAction
	{
		// Token: 0x060035F9 RID: 13817 RVA: 0x00114A08 File Offset: 0x00112C08
		public override void Reset()
		{
			this.gameObject = null;
			this.storeCount = null;
			this.everyFrame = false;
		}

		// Token: 0x060035FA RID: 13818 RVA: 0x00114A20 File Offset: 0x00112C20
		public override void OnEnter()
		{
			this.DoGetVertexCount();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x060035FB RID: 13819 RVA: 0x00114A3C File Offset: 0x00112C3C
		public override void OnUpdate()
		{
			this.DoGetVertexCount();
		}

		// Token: 0x060035FC RID: 13820 RVA: 0x00114A44 File Offset: 0x00112C44
		private void DoGetVertexCount()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
			if (ownerDefaultTarget != null)
			{
				MeshFilter component = ownerDefaultTarget.GetComponent<MeshFilter>();
				if (component == null)
				{
					base.LogError("Missing MeshFilter!");
					return;
				}
				this.storeCount.Value = component.mesh.vertexCount;
			}
		}

		// Token: 0x04002743 RID: 10051
		[Tooltip("The GameObject to check.")]
		[CheckForComponent(typeof(MeshFilter))]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x04002744 RID: 10052
		[UIHint(UIHint.Variable)]
		[RequiredField]
		[Tooltip("Store the vertex count in a variable.")]
		public FsmInt storeCount;

		// Token: 0x04002745 RID: 10053
		public bool everyFrame;
	}
}
