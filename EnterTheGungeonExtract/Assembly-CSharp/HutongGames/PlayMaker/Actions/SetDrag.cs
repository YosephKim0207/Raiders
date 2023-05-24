using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000ACF RID: 2767
	[HelpUrl("http://hutonggames.com/playmakerforum/index.php?topic=4734.0")]
	[Tooltip("Sets the Drag of a Game Object's Rigid Body.")]
	[ActionCategory(ActionCategory.Physics)]
	public class SetDrag : ComponentAction<Rigidbody>
	{
		// Token: 0x06003A9A RID: 15002 RVA: 0x00129984 File Offset: 0x00127B84
		public override void Reset()
		{
			this.gameObject = null;
			this.drag = 1f;
		}

		// Token: 0x06003A9B RID: 15003 RVA: 0x001299A0 File Offset: 0x00127BA0
		public override void OnEnter()
		{
			this.DoSetDrag();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003A9C RID: 15004 RVA: 0x001299BC File Offset: 0x00127BBC
		public override void OnUpdate()
		{
			this.DoSetDrag();
		}

		// Token: 0x06003A9D RID: 15005 RVA: 0x001299C4 File Offset: 0x00127BC4
		private void DoSetDrag()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
			if (base.UpdateCache(ownerDefaultTarget))
			{
				base.rigidbody.drag = this.drag.Value;
			}
		}

		// Token: 0x04002CBE RID: 11454
		[CheckForComponent(typeof(Rigidbody))]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x04002CBF RID: 11455
		[HasFloatSlider(0f, 10f)]
		[RequiredField]
		public FsmFloat drag;

		// Token: 0x04002CC0 RID: 11456
		[Tooltip("Repeat every frame. Typically this would be set to True.")]
		public bool everyFrame;
	}
}
