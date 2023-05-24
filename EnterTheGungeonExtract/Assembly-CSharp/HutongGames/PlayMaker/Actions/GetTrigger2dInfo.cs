using System;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000A5E RID: 2654
	[Tooltip("Gets info on the last Trigger 2d event and store in variables.  See Unity and PlayMaker docs on Unity 2D physics.")]
	[ActionCategory(ActionCategory.Physics2D)]
	public class GetTrigger2dInfo : FsmStateAction
	{
		// Token: 0x0600387A RID: 14458 RVA: 0x00121EC4 File Offset: 0x001200C4
		public override void Reset()
		{
			this.gameObjectHit = null;
			this.shapeCount = null;
			this.physics2dMaterialName = null;
		}

		// Token: 0x0600387B RID: 14459 RVA: 0x00121EDC File Offset: 0x001200DC
		private void StoreTriggerInfo()
		{
			if (base.Fsm.TriggerCollider2D == null)
			{
				return;
			}
			this.gameObjectHit.Value = base.Fsm.TriggerCollider2D.gameObject;
			this.shapeCount.Value = base.Fsm.TriggerCollider2D.shapeCount;
			this.physics2dMaterialName.Value = ((!(base.Fsm.TriggerCollider2D.sharedMaterial != null)) ? string.Empty : base.Fsm.TriggerCollider2D.sharedMaterial.name);
		}

		// Token: 0x0600387C RID: 14460 RVA: 0x00121F7C File Offset: 0x0012017C
		public override void OnEnter()
		{
			this.StoreTriggerInfo();
			base.Finish();
		}

		// Token: 0x04002AA6 RID: 10918
		[Tooltip("Get the GameObject hit.")]
		[UIHint(UIHint.Variable)]
		public FsmGameObject gameObjectHit;

		// Token: 0x04002AA7 RID: 10919
		[Tooltip("The number of separate shaped regions in the collider.")]
		[UIHint(UIHint.Variable)]
		public FsmInt shapeCount;

		// Token: 0x04002AA8 RID: 10920
		[Tooltip("Useful for triggering different effects. Audio, particles...")]
		[UIHint(UIHint.Variable)]
		public FsmString physics2dMaterialName;
	}
}
