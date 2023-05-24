using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000931 RID: 2353
	[Tooltip("Destroys GameObjects in an array.")]
	[ActionCategory(ActionCategory.GameObject)]
	public class DestroyObjects : FsmStateAction
	{
		// Token: 0x0600339B RID: 13211 RVA: 0x0010DA84 File Offset: 0x0010BC84
		public override void Reset()
		{
			this.gameObjects = null;
			this.delay = 0f;
		}

		// Token: 0x0600339C RID: 13212 RVA: 0x0010DAA0 File Offset: 0x0010BCA0
		public override void OnEnter()
		{
			if (this.gameObjects.Values != null)
			{
				foreach (GameObject gameObject in this.gameObjects.Values)
				{
					if (gameObject != null)
					{
						if (this.delay.Value <= 0f)
						{
							UnityEngine.Object.Destroy(gameObject);
						}
						else
						{
							UnityEngine.Object.Destroy(gameObject, this.delay.Value);
						}
						if (this.detachChildren.Value)
						{
							gameObject.transform.DetachChildren();
						}
					}
				}
			}
			base.Finish();
		}

		// Token: 0x040024C5 RID: 9413
		[Tooltip("The GameObjects to destroy.")]
		[ArrayEditor(VariableType.GameObject, "", 0, 0, 65536)]
		[RequiredField]
		public FsmArray gameObjects;

		// Token: 0x040024C6 RID: 9414
		[Tooltip("Optional delay before destroying the Game Objects.")]
		[HasFloatSlider(0f, 5f)]
		public FsmFloat delay;

		// Token: 0x040024C7 RID: 9415
		[Tooltip("Detach children before destroying the Game Objects.")]
		public FsmBool detachChildren;
	}
}
