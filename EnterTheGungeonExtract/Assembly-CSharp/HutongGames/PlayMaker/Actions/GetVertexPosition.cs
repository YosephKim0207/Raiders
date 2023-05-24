using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x020009C0 RID: 2496
	[Tooltip("Gets the position of a vertex in a GameObject's mesh. Hint: Use GetVertexCount to get the number of vertices in a mesh.")]
	[ActionCategory("Mesh")]
	public class GetVertexPosition : FsmStateAction
	{
		// Token: 0x060035FE RID: 13822 RVA: 0x00114AAC File Offset: 0x00112CAC
		public override void Reset()
		{
			this.gameObject = null;
			this.space = Space.World;
			this.storePosition = null;
			this.everyFrame = false;
		}

		// Token: 0x060035FF RID: 13823 RVA: 0x00114ACC File Offset: 0x00112CCC
		public override void OnEnter()
		{
			this.DoGetVertexPosition();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003600 RID: 13824 RVA: 0x00114AE8 File Offset: 0x00112CE8
		public override void OnUpdate()
		{
			this.DoGetVertexPosition();
		}

		// Token: 0x06003601 RID: 13825 RVA: 0x00114AF0 File Offset: 0x00112CF0
		private void DoGetVertexPosition()
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
				Space space = this.space;
				if (space != Space.World)
				{
					if (space == Space.Self)
					{
						this.storePosition.Value = component.mesh.vertices[this.vertexIndex.Value];
					}
				}
				else
				{
					Vector3 vector = component.mesh.vertices[this.vertexIndex.Value];
					this.storePosition.Value = ownerDefaultTarget.transform.TransformPoint(vector);
				}
			}
		}

		// Token: 0x04002746 RID: 10054
		[RequiredField]
		[Tooltip("The GameObject to check.")]
		[CheckForComponent(typeof(MeshFilter))]
		public FsmOwnerDefault gameObject;

		// Token: 0x04002747 RID: 10055
		[Tooltip("The index of the vertex.")]
		[RequiredField]
		public FsmInt vertexIndex;

		// Token: 0x04002748 RID: 10056
		[Tooltip("Coordinate system to use.")]
		public Space space;

		// Token: 0x04002749 RID: 10057
		[Tooltip("Store the vertex position in a variable.")]
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmVector3 storePosition;

		// Token: 0x0400274A RID: 10058
		[Tooltip("Repeat every frame. Useful if the mesh is animated.")]
		public bool everyFrame;
	}
}
