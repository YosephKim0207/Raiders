using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x020009CB RID: 2507
	[Tooltip("Begin a GUILayout area that follows the specified game object. Useful for overlays (e.g., playerName). NOTE: Block must end with a corresponding GUILayoutEndArea.")]
	[ActionCategory(ActionCategory.GUILayout)]
	public class GUILayoutBeginAreaFollowObject : FsmStateAction
	{
		// Token: 0x06003621 RID: 13857 RVA: 0x0011553C File Offset: 0x0011373C
		public override void Reset()
		{
			this.gameObject = null;
			this.offsetLeft = 0f;
			this.offsetTop = 0f;
			this.width = 1f;
			this.height = 1f;
			this.normalized = true;
			this.style = string.Empty;
		}

		// Token: 0x06003622 RID: 13858 RVA: 0x001155AC File Offset: 0x001137AC
		public override void OnGUI()
		{
			GameObject value = this.gameObject.Value;
			if (value == null || Camera.main == null)
			{
				GUILayoutBeginAreaFollowObject.DummyBeginArea();
				return;
			}
			Vector3 position = value.transform.position;
			if (Camera.main.transform.InverseTransformPoint(position).z < 0f)
			{
				GUILayoutBeginAreaFollowObject.DummyBeginArea();
				return;
			}
			Vector2 vector = Camera.main.WorldToScreenPoint(position);
			float num = vector.x + ((!this.normalized.Value) ? this.offsetLeft.Value : (this.offsetLeft.Value * (float)Screen.width));
			float num2 = vector.y + ((!this.normalized.Value) ? this.offsetTop.Value : (this.offsetTop.Value * (float)Screen.width));
			Rect rect = new Rect(num, num2, this.width.Value, this.height.Value);
			if (this.normalized.Value)
			{
				rect.width *= (float)Screen.width;
				rect.height *= (float)Screen.height;
			}
			rect.y = (float)Screen.height - rect.y;
			GUILayout.BeginArea(rect, this.style.Value);
		}

		// Token: 0x06003623 RID: 13859 RVA: 0x00115724 File Offset: 0x00113924
		private static void DummyBeginArea()
		{
			GUILayout.BeginArea(default(Rect));
		}

		// Token: 0x04002773 RID: 10099
		[RequiredField]
		[Tooltip("The GameObject to follow.")]
		public FsmGameObject gameObject;

		// Token: 0x04002774 RID: 10100
		[RequiredField]
		public FsmFloat offsetLeft;

		// Token: 0x04002775 RID: 10101
		[RequiredField]
		public FsmFloat offsetTop;

		// Token: 0x04002776 RID: 10102
		[RequiredField]
		public FsmFloat width;

		// Token: 0x04002777 RID: 10103
		[RequiredField]
		public FsmFloat height;

		// Token: 0x04002778 RID: 10104
		[Tooltip("Use normalized screen coordinates (0-1).")]
		public FsmBool normalized;

		// Token: 0x04002779 RID: 10105
		[Tooltip("Optional named style in the current GUISkin")]
		public FsmString style;
	}
}
