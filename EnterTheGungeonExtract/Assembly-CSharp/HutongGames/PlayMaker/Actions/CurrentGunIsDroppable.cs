using System;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000C97 RID: 3223
	[Tooltip("Toss the current gun into the gunper monper and (hopefully) get an upgrade.")]
	[ActionCategory(".NPCs")]
	public class CurrentGunIsDroppable : FsmStateAction
	{
		// Token: 0x060044FE RID: 17662 RVA: 0x00164C54 File Offset: 0x00162E54
		public override void Reset()
		{
			this.isTrue = null;
			this.isFalse = null;
			this.everyFrame = false;
		}

		// Token: 0x060044FF RID: 17663 RVA: 0x00164C6C File Offset: 0x00162E6C
		private bool TestGun()
		{
			TalkDoerLite component = base.Owner.GetComponent<TalkDoerLite>();
			bool flag = false;
			if (component && component.TalkingPlayer)
			{
				if (component.TalkingPlayer.CharacterUsesRandomGuns)
				{
					return false;
				}
				Gun currentGun = component.TalkingPlayer.CurrentGun;
				if (currentGun && currentGun.CanActuallyBeDropped(component.TalkingPlayer) && !currentGun.InfiniteAmmo)
				{
					flag = true;
				}
			}
			return flag;
		}

		// Token: 0x06004500 RID: 17664 RVA: 0x00164CEC File Offset: 0x00162EEC
		public override void OnEnter()
		{
			base.Fsm.Event((!this.TestGun()) ? this.isFalse : this.isTrue);
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06004501 RID: 17665 RVA: 0x00164D28 File Offset: 0x00162F28
		public override void OnUpdate()
		{
			base.Fsm.Event((!this.TestGun()) ? this.isFalse : this.isTrue);
		}

		// Token: 0x04003712 RID: 14098
		[Tooltip("Event to send if the player is in the foyer.")]
		public FsmEvent isTrue;

		// Token: 0x04003713 RID: 14099
		[Tooltip("Event to send if the player is not in the foyer.")]
		public FsmEvent isFalse;

		// Token: 0x04003714 RID: 14100
		[Tooltip("Repeat every frame while the state is active.")]
		public bool everyFrame;
	}
}
