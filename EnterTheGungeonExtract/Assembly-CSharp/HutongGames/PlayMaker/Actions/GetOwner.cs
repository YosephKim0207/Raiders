using System;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x0200099E RID: 2462
	[ActionCategory(ActionCategory.GameObject)]
	[Tooltip("Gets the Game Object that owns the FSM and stores it in a game object variable.")]
	public class GetOwner : FsmStateAction
	{
		// Token: 0x0600356C RID: 13676 RVA: 0x00113304 File Offset: 0x00111504
		public override void Reset()
		{
			this.storeGameObject = null;
		}

		// Token: 0x0600356D RID: 13677 RVA: 0x00113310 File Offset: 0x00111510
		public override void OnEnter()
		{
			this.storeGameObject.Value = base.Owner;
			base.Finish();
		}

		// Token: 0x040026C4 RID: 9924
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmGameObject storeGameObject;
	}
}
