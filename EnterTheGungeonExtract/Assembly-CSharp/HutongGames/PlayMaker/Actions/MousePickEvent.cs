using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000A1E RID: 2590
	[Tooltip("Sends Events based on mouse interactions with a Game Object: MouseOver, MouseDown, MouseUp, MouseOff. Use Ray Distance to set how close the camera must be to pick the object.\n\nNOTE: Picking uses the Main Camera.")]
	[ActionTarget(typeof(GameObject), "GameObject", false)]
	[ActionCategory(ActionCategory.Input)]
	public class MousePickEvent : FsmStateAction
	{
		// Token: 0x06003775 RID: 14197 RVA: 0x0011DD3C File Offset: 0x0011BF3C
		public override void Reset()
		{
			this.GameObject = null;
			this.rayDistance = 100f;
			this.mouseOver = null;
			this.mouseDown = null;
			this.mouseUp = null;
			this.mouseOff = null;
			this.layerMask = new FsmInt[0];
			this.invertMask = false;
			this.everyFrame = true;
		}

		// Token: 0x06003776 RID: 14198 RVA: 0x0011DD9C File Offset: 0x0011BF9C
		public override void OnEnter()
		{
			this.DoMousePickEvent();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003777 RID: 14199 RVA: 0x0011DDB8 File Offset: 0x0011BFB8
		public override void OnUpdate()
		{
			this.DoMousePickEvent();
		}

		// Token: 0x06003778 RID: 14200 RVA: 0x0011DDC0 File Offset: 0x0011BFC0
		private void DoMousePickEvent()
		{
			bool flag = this.DoRaycast();
			base.Fsm.RaycastHitInfo = ActionHelpers.mousePickInfo;
			if (flag)
			{
				if (this.mouseDown != null && Input.GetMouseButtonDown(0))
				{
					base.Fsm.Event(this.mouseDown);
				}
				if (this.mouseOver != null)
				{
					base.Fsm.Event(this.mouseOver);
				}
				if (this.mouseUp != null && Input.GetMouseButtonUp(0))
				{
					base.Fsm.Event(this.mouseUp);
				}
			}
			else if (this.mouseOff != null)
			{
				base.Fsm.Event(this.mouseOff);
			}
		}

		// Token: 0x06003779 RID: 14201 RVA: 0x0011DE78 File Offset: 0x0011C078
		private bool DoRaycast()
		{
			GameObject gameObject = ((this.GameObject.OwnerOption != OwnerDefaultOption.UseOwner) ? this.GameObject.GameObject.Value : base.Owner);
			return ActionHelpers.IsMouseOver(gameObject, this.rayDistance.Value, ActionHelpers.LayerArrayToLayerMask(this.layerMask, this.invertMask.Value));
		}

		// Token: 0x0600377A RID: 14202 RVA: 0x0011DED8 File Offset: 0x0011C0D8
		public override string ErrorCheck()
		{
			string text = string.Empty;
			text += ActionHelpers.CheckRayDistance(this.rayDistance.Value);
			return text + ActionHelpers.CheckPhysicsSetup(this.GameObject);
		}

		// Token: 0x04002963 RID: 10595
		[CheckForComponent(typeof(Collider))]
		public FsmOwnerDefault GameObject;

		// Token: 0x04002964 RID: 10596
		[Tooltip("Length of the ray to cast from the camera.")]
		public FsmFloat rayDistance = 100f;

		// Token: 0x04002965 RID: 10597
		[Tooltip("Event to send when the mouse is over the GameObject.")]
		public FsmEvent mouseOver;

		// Token: 0x04002966 RID: 10598
		[Tooltip("Event to send when the mouse is pressed while over the GameObject.")]
		public FsmEvent mouseDown;

		// Token: 0x04002967 RID: 10599
		[Tooltip("Event to send when the mouse is released while over the GameObject.")]
		public FsmEvent mouseUp;

		// Token: 0x04002968 RID: 10600
		[Tooltip("Event to send when the mouse moves off the GameObject.")]
		public FsmEvent mouseOff;

		// Token: 0x04002969 RID: 10601
		[UIHint(UIHint.Layer)]
		[Tooltip("Pick only from these layers.")]
		public FsmInt[] layerMask;

		// Token: 0x0400296A RID: 10602
		[Tooltip("Invert the mask, so you pick from all layers except those defined above.")]
		public FsmBool invertMask;

		// Token: 0x0400296B RID: 10603
		[Tooltip("Repeat every frame.")]
		public bool everyFrame;
	}
}
