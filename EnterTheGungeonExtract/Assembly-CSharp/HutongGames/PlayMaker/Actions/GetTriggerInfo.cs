using System;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x020009BB RID: 2491
	[ActionCategory(ActionCategory.Physics)]
	[Tooltip("Gets info on the last Trigger event and store in variables.")]
	public class GetTriggerInfo : FsmStateAction
	{
		// Token: 0x060035E7 RID: 13799 RVA: 0x00114738 File Offset: 0x00112938
		public override void Reset()
		{
			this.gameObjectHit = null;
			this.physicsMaterialName = null;
		}

		// Token: 0x060035E8 RID: 13800 RVA: 0x00114748 File Offset: 0x00112948
		private void StoreTriggerInfo()
		{
			if (base.Fsm.TriggerCollider == null)
			{
				return;
			}
			this.gameObjectHit.Value = base.Fsm.TriggerCollider.gameObject;
			this.physicsMaterialName.Value = base.Fsm.TriggerCollider.material.name;
		}

		// Token: 0x060035E9 RID: 13801 RVA: 0x001147A8 File Offset: 0x001129A8
		public override void OnEnter()
		{
			this.StoreTriggerInfo();
			base.Finish();
		}

		// Token: 0x04002733 RID: 10035
		[UIHint(UIHint.Variable)]
		public FsmGameObject gameObjectHit;

		// Token: 0x04002734 RID: 10036
		[UIHint(UIHint.Variable)]
		[Tooltip("Useful for triggering different effects. Audio, particles...")]
		public FsmString physicsMaterialName;
	}
}
