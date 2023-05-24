using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000930 RID: 2352
	[Tooltip("Destroys a Game Object.")]
	[ActionCategory(ActionCategory.GameObject)]
	public class DestroyObject : FsmStateAction
	{
		// Token: 0x06003397 RID: 13207 RVA: 0x0010D9E4 File Offset: 0x0010BBE4
		public override void Reset()
		{
			this.gameObject = null;
			this.delay = 0f;
		}

		// Token: 0x06003398 RID: 13208 RVA: 0x0010DA00 File Offset: 0x0010BC00
		public override void OnEnter()
		{
			GameObject value = this.gameObject.Value;
			if (value != null)
			{
				if (this.delay.Value <= 0f)
				{
					UnityEngine.Object.Destroy(value);
				}
				else
				{
					UnityEngine.Object.Destroy(value, this.delay.Value);
				}
				if (this.detachChildren.Value)
				{
					value.transform.DetachChildren();
				}
			}
			base.Finish();
		}

		// Token: 0x06003399 RID: 13209 RVA: 0x0010DA78 File Offset: 0x0010BC78
		public override void OnUpdate()
		{
		}

		// Token: 0x040024C2 RID: 9410
		[RequiredField]
		[Tooltip("The GameObject to destroy.")]
		public FsmGameObject gameObject;

		// Token: 0x040024C3 RID: 9411
		[Tooltip("Optional delay before destroying the Game Object.")]
		[HasFloatSlider(0f, 5f)]
		public FsmFloat delay;

		// Token: 0x040024C4 RID: 9412
		[Tooltip("Detach children before destroying the Game Object.")]
		public FsmBool detachChildren;
	}
}
