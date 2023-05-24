using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000939 RID: 2361
	[ActionCategory(ActionCategory.Debug)]
	[Tooltip("Draws a line from a Start point in a direction. Specify the start point as Game Objects or Vector3 world positions. If both are specified, position is used as a local offset from the Object's position.")]
	public class DrawDebugRay : FsmStateAction
	{
		// Token: 0x060033B6 RID: 13238 RVA: 0x0010DDE0 File Offset: 0x0010BFE0
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
			this.direction = new FsmVector3
			{
				UseVariable = true
			};
			this.color = Color.white;
		}

		// Token: 0x060033B7 RID: 13239 RVA: 0x0010DE3C File Offset: 0x0010C03C
		public override void OnUpdate()
		{
			Vector3 position = ActionHelpers.GetPosition(this.fromObject, this.fromPosition);
			Debug.DrawRay(position, this.direction.Value, this.color.Value);
		}

		// Token: 0x040024D5 RID: 9429
		[Tooltip("Draw ray from a GameObject.")]
		public FsmGameObject fromObject;

		// Token: 0x040024D6 RID: 9430
		[Tooltip("Draw ray from a world position, or local offset from GameObject if provided.")]
		public FsmVector3 fromPosition;

		// Token: 0x040024D7 RID: 9431
		[Tooltip("Direction vector of ray.")]
		public FsmVector3 direction;

		// Token: 0x040024D8 RID: 9432
		[Tooltip("The color of the ray.")]
		public FsmColor color;
	}
}
