using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000B56 RID: 2902
	[Tooltip("Moves a Vector2 towards a Target. Optionally sends an event when successful.")]
	[ActionCategory(ActionCategory.Vector2)]
	public class Vector2MoveTowards : FsmStateAction
	{
		// Token: 0x06003CE2 RID: 15586 RVA: 0x001314B8 File Offset: 0x0012F6B8
		public override void Reset()
		{
			this.source = null;
			this.target = null;
			this.maxSpeed = 10f;
			this.finishDistance = 1f;
			this.finishEvent = null;
		}

		// Token: 0x06003CE3 RID: 15587 RVA: 0x001314F0 File Offset: 0x0012F6F0
		public override void OnUpdate()
		{
			this.DoMoveTowards();
		}

		// Token: 0x06003CE4 RID: 15588 RVA: 0x001314F8 File Offset: 0x0012F6F8
		private void DoMoveTowards()
		{
			this.source.Value = Vector2.MoveTowards(this.source.Value, this.target.Value, this.maxSpeed.Value * Time.deltaTime);
			float magnitude = (this.source.Value - this.target.Value).magnitude;
			if (magnitude < this.finishDistance.Value)
			{
				base.Fsm.Event(this.finishEvent);
				base.Finish();
			}
		}

		// Token: 0x04002F26 RID: 12070
		[Tooltip("The Vector2 to Move")]
		[RequiredField]
		public FsmVector2 source;

		// Token: 0x04002F27 RID: 12071
		[Tooltip("A target Vector2 to move towards.")]
		public FsmVector2 target;

		// Token: 0x04002F28 RID: 12072
		[HasFloatSlider(0f, 20f)]
		[Tooltip("The maximum movement speed. HINT: You can make this a variable to change it over time.")]
		public FsmFloat maxSpeed;

		// Token: 0x04002F29 RID: 12073
		[Tooltip("Distance at which the move is considered finished, and the Finish Event is sent.")]
		[HasFloatSlider(0f, 5f)]
		public FsmFloat finishDistance;

		// Token: 0x04002F2A RID: 12074
		[Tooltip("Event to send when the Finish Distance is reached.")]
		public FsmEvent finishEvent;
	}
}
