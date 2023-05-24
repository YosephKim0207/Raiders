using System;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x020009A0 RID: 2464
	[ActionCategory(ActionCategory.Physics)]
	[Tooltip("Gets info on the last particle collision event. See Unity Particle System docs.")]
	public class GetParticleCollisionInfo : FsmStateAction
	{
		// Token: 0x06003572 RID: 13682 RVA: 0x001133C8 File Offset: 0x001115C8
		public override void Reset()
		{
			this.gameObjectHit = null;
		}

		// Token: 0x06003573 RID: 13683 RVA: 0x001133D4 File Offset: 0x001115D4
		private void StoreCollisionInfo()
		{
			this.gameObjectHit.Value = base.Fsm.ParticleCollisionGO;
		}

		// Token: 0x06003574 RID: 13684 RVA: 0x001133EC File Offset: 0x001115EC
		public override void OnEnter()
		{
			this.StoreCollisionInfo();
			base.Finish();
		}

		// Token: 0x040026C7 RID: 9927
		[Tooltip("Get the GameObject hit.")]
		[UIHint(UIHint.Variable)]
		public FsmGameObject gameObjectHit;
	}
}
