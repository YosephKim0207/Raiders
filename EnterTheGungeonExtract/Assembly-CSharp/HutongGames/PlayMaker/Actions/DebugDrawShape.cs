using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000925 RID: 2341
	[Tooltip("Draw Gizmos in the Scene View.")]
	[ActionCategory(ActionCategory.Debug)]
	public class DebugDrawShape : FsmStateAction
	{
		// Token: 0x06003378 RID: 13176 RVA: 0x0010D420 File Offset: 0x0010B620
		public override void Reset()
		{
			this.gameObject = null;
			this.shape = DebugDrawShape.ShapeType.Sphere;
			this.color = Color.grey;
			this.radius = 1f;
			this.size = new Vector3(1f, 1f, 1f);
		}

		// Token: 0x06003379 RID: 13177 RVA: 0x0010D47C File Offset: 0x0010B67C
		public override void OnDrawActionGizmos()
		{
			Transform transform = base.Fsm.GetOwnerDefaultTarget(this.gameObject).transform;
			if (transform == null)
			{
				return;
			}
			Gizmos.color = this.color.Value;
			switch (this.shape)
			{
			case DebugDrawShape.ShapeType.Sphere:
				Gizmos.DrawSphere(transform.position, this.radius.Value);
				break;
			case DebugDrawShape.ShapeType.Cube:
				Gizmos.DrawCube(transform.position, this.size.Value);
				break;
			case DebugDrawShape.ShapeType.WireSphere:
				Gizmos.DrawWireSphere(transform.position, this.radius.Value);
				break;
			case DebugDrawShape.ShapeType.WireCube:
				Gizmos.DrawWireCube(transform.position, this.size.Value);
				break;
			}
		}

		// Token: 0x040024A5 RID: 9381
		[RequiredField]
		[Tooltip("Draw the Gizmo at a GameObject's position.")]
		public FsmOwnerDefault gameObject;

		// Token: 0x040024A6 RID: 9382
		[Tooltip("The type of Gizmo to draw:\nSphere, Cube, WireSphere, or WireCube.")]
		public DebugDrawShape.ShapeType shape;

		// Token: 0x040024A7 RID: 9383
		[Tooltip("The color to use.")]
		public FsmColor color;

		// Token: 0x040024A8 RID: 9384
		[Tooltip("Use this for sphere gizmos")]
		public FsmFloat radius;

		// Token: 0x040024A9 RID: 9385
		[Tooltip("Use this for cube gizmos")]
		public FsmVector3 size;

		// Token: 0x02000926 RID: 2342
		public enum ShapeType
		{
			// Token: 0x040024AB RID: 9387
			Sphere,
			// Token: 0x040024AC RID: 9388
			Cube,
			// Token: 0x040024AD RID: 9389
			WireSphere,
			// Token: 0x040024AE RID: 9390
			WireCube
		}
	}
}
