using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x020009C6 RID: 2502
	[Tooltip("Performs a Hit Test on a Game Object with a GUITexture or GUIText component.")]
	[ActionCategory(ActionCategory.GUIElement)]
	public class GUIElementHitTest : FsmStateAction
	{
		// Token: 0x06003611 RID: 13841 RVA: 0x00114F4C File Offset: 0x0011314C
		public override void Reset()
		{
			this.gameObject = null;
			this.camera = null;
			this.screenPoint = new FsmVector3
			{
				UseVariable = true
			};
			this.screenX = new FsmFloat
			{
				UseVariable = true
			};
			this.screenY = new FsmFloat
			{
				UseVariable = true
			};
			this.normalized = true;
			this.hitEvent = null;
			this.everyFrame = true;
		}

		// Token: 0x06003612 RID: 13842 RVA: 0x00114FC4 File Offset: 0x001131C4
		public override void OnEnter()
		{
			this.DoHitTest();
			if (!this.everyFrame.Value)
			{
				base.Finish();
			}
		}

		// Token: 0x06003613 RID: 13843 RVA: 0x00114FE4 File Offset: 0x001131E4
		public override void OnUpdate()
		{
			this.DoHitTest();
		}

		// Token: 0x06003614 RID: 13844 RVA: 0x00114FEC File Offset: 0x001131EC
		private void DoHitTest()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
			if (ownerDefaultTarget == null)
			{
				return;
			}
			if (ownerDefaultTarget != this.gameObjectCached)
			{
				this.guiElement = ownerDefaultTarget.GetComponent<GUITexture>() ?? ownerDefaultTarget.GetComponent<GUIText>();
				this.gameObjectCached = ownerDefaultTarget;
			}
			if (this.guiElement == null)
			{
				base.Finish();
				return;
			}
			Vector3 vector = ((!this.screenPoint.IsNone) ? this.screenPoint.Value : new Vector3(0f, 0f));
			if (!this.screenX.IsNone)
			{
				vector.x = this.screenX.Value;
			}
			if (!this.screenY.IsNone)
			{
				vector.y = this.screenY.Value;
			}
			if (this.normalized.Value)
			{
				vector.x *= (float)Screen.width;
				vector.y *= (float)Screen.height;
			}
			if (this.guiElement.HitTest(vector, this.camera))
			{
				this.storeResult.Value = true;
				base.Fsm.Event(this.hitEvent);
			}
			else
			{
				this.storeResult.Value = false;
			}
		}

		// Token: 0x04002759 RID: 10073
		[Tooltip("The GameObject that has a GUITexture or GUIText component.")]
		[CheckForComponent(typeof(GUIElement))]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x0400275A RID: 10074
		[Tooltip("Specify camera or use MainCamera as default.")]
		public Camera camera;

		// Token: 0x0400275B RID: 10075
		[Tooltip("A vector position on screen. Usually stored by actions like GetTouchInfo, or World To Screen Point.")]
		public FsmVector3 screenPoint;

		// Token: 0x0400275C RID: 10076
		[Tooltip("Specify screen X coordinate.")]
		public FsmFloat screenX;

		// Token: 0x0400275D RID: 10077
		[Tooltip("Specify screen Y coordinate.")]
		public FsmFloat screenY;

		// Token: 0x0400275E RID: 10078
		[Tooltip("Whether the specified screen coordinates are normalized (0-1).")]
		public FsmBool normalized;

		// Token: 0x0400275F RID: 10079
		[Tooltip("Event to send if the Hit Test is true.")]
		public FsmEvent hitEvent;

		// Token: 0x04002760 RID: 10080
		[Tooltip("Store the result of the Hit Test in a bool variable (true/false).")]
		[UIHint(UIHint.Variable)]
		public FsmBool storeResult;

		// Token: 0x04002761 RID: 10081
		[Tooltip("Repeat every frame. Useful if you want to wait for the hit test to return true.")]
		public FsmBool everyFrame;

		// Token: 0x04002762 RID: 10082
		private GUIElement guiElement;

		// Token: 0x04002763 RID: 10083
		private GameObject gameObjectCached;
	}
}
