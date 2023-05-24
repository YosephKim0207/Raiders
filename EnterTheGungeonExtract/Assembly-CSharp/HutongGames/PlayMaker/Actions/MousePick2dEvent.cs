using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000A67 RID: 2663
	[Tooltip("Sends Events based on mouse interactions with a 2d Game Object: MouseOver, MouseDown, MouseUp, MouseOff.")]
	[ActionCategory(ActionCategory.Input)]
	public class MousePick2dEvent : FsmStateAction
	{
		// Token: 0x060038A6 RID: 14502 RVA: 0x00122A9C File Offset: 0x00120C9C
		public override void Reset()
		{
			this.GameObject = null;
			this.mouseOver = null;
			this.mouseDown = null;
			this.mouseUp = null;
			this.mouseOff = null;
			this.layerMask = new FsmInt[0];
			this.invertMask = false;
			this.everyFrame = true;
		}

		// Token: 0x060038A7 RID: 14503 RVA: 0x00122AEC File Offset: 0x00120CEC
		public override void OnEnter()
		{
			this.DoMousePickEvent();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x060038A8 RID: 14504 RVA: 0x00122B08 File Offset: 0x00120D08
		public override void OnUpdate()
		{
			this.DoMousePickEvent();
		}

		// Token: 0x060038A9 RID: 14505 RVA: 0x00122B10 File Offset: 0x00120D10
		private void DoMousePickEvent()
		{
			bool flag = this.DoRaycast();
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

		// Token: 0x060038AA RID: 14506 RVA: 0x00122BB8 File Offset: 0x00120DB8
		private bool DoRaycast()
		{
			GameObject gameObject = ((this.GameObject.OwnerOption != OwnerDefaultOption.UseOwner) ? this.GameObject.GameObject.Value : base.Owner);
			RaycastHit2D rayIntersection = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(Input.mousePosition), float.PositiveInfinity, ActionHelpers.LayerArrayToLayerMask(this.layerMask, this.invertMask.Value));
			Fsm.RecordLastRaycastHit2DInfo(base.Fsm, rayIntersection);
			return rayIntersection.transform != null && rayIntersection.transform.gameObject == gameObject;
		}

		// Token: 0x04002AE7 RID: 10983
		[Tooltip("The GameObject with a Collider2D attached.")]
		[CheckForComponent(typeof(Collider2D))]
		public FsmOwnerDefault GameObject;

		// Token: 0x04002AE8 RID: 10984
		[Tooltip("Event to send when the mouse is over the GameObject.")]
		public FsmEvent mouseOver;

		// Token: 0x04002AE9 RID: 10985
		[Tooltip("Event to send when the mouse is pressed while over the GameObject.")]
		public FsmEvent mouseDown;

		// Token: 0x04002AEA RID: 10986
		[Tooltip("Event to send when the mouse is released while over the GameObject.")]
		public FsmEvent mouseUp;

		// Token: 0x04002AEB RID: 10987
		[Tooltip("Event to send when the mouse moves off the GameObject.")]
		public FsmEvent mouseOff;

		// Token: 0x04002AEC RID: 10988
		[UIHint(UIHint.Layer)]
		[Tooltip("Pick only from these layers.")]
		public FsmInt[] layerMask;

		// Token: 0x04002AED RID: 10989
		[Tooltip("Invert the mask, so you pick from all layers except those defined above.")]
		public FsmBool invertMask;

		// Token: 0x04002AEE RID: 10990
		[Tooltip("Repeat every frame.")]
		public bool everyFrame;
	}
}
