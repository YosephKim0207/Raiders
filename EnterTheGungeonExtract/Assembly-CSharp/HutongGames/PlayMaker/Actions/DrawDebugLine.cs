using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000938 RID: 2360
	[ActionCategory(ActionCategory.Debug)]
	[Tooltip("Draws a line from a Start point to an End point. Specify the points as Game Objects or Vector3 world positions. If both are specified, position is used as a local offset from the Object's position.")]
	public class DrawDebugLine : FsmStateAction
	{
		// Token: 0x060033B3 RID: 13235 RVA: 0x0010DD24 File Offset: 0x0010BF24
		public override void Reset()
		{
			this.fromObject = new FsmGameObject
			{
				UseVariable = true
			};
			this.fromPosition = new FsmVector3
			{
				UseVariable = true
			};
			this.toObject = new FsmGameObject
			{
				UseVariable = true
			};
			this.toPosition = new FsmVector3
			{
				UseVariable = true
			};
			this.color = Color.white;
		}

		// Token: 0x060033B4 RID: 13236 RVA: 0x0010DD94 File Offset: 0x0010BF94
		public override void OnUpdate()
		{
			Vector3 position = ActionHelpers.GetPosition(this.fromObject, this.fromPosition);
			Vector3 position2 = ActionHelpers.GetPosition(this.toObject, this.toPosition);
			Debug.DrawLine(position, position2, this.color.Value);
		}

		// Token: 0x040024D0 RID: 9424
		[Tooltip("Draw line from a GameObject.")]
		public FsmGameObject fromObject;

		// Token: 0x040024D1 RID: 9425
		[Tooltip("Draw line from a world position, or local offset from GameObject if provided.")]
		public FsmVector3 fromPosition;

		// Token: 0x040024D2 RID: 9426
		[Tooltip("Draw line to a GameObject.")]
		public FsmGameObject toObject;

		// Token: 0x040024D3 RID: 9427
		[Tooltip("Draw line to a world position, or local offset from GameObject if provided.")]
		public FsmVector3 toPosition;

		// Token: 0x040024D4 RID: 9428
		[Tooltip("The color of the line.")]
		public FsmColor color;
	}
}
