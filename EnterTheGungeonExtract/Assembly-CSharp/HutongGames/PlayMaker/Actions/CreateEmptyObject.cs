using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000921 RID: 2337
	[Tooltip("Creates a Game Object at a spawn point.\nUse a Game Object and/or Position/Rotation for the Spawn Point. If you specify a Game Object, Position is used as a local offset, and Rotation will override the object's rotation.")]
	[ActionCategory(ActionCategory.GameObject)]
	public class CreateEmptyObject : FsmStateAction
	{
		// Token: 0x0600336A RID: 13162 RVA: 0x0010CF68 File Offset: 0x0010B168
		public override void Reset()
		{
			this.gameObject = null;
			this.spawnPoint = null;
			this.position = new FsmVector3
			{
				UseVariable = true
			};
			this.rotation = new FsmVector3
			{
				UseVariable = true
			};
			this.storeObject = null;
		}

		// Token: 0x0600336B RID: 13163 RVA: 0x0010CFB4 File Offset: 0x0010B1B4
		public override void OnEnter()
		{
			GameObject value = this.gameObject.Value;
			Vector3 vector = Vector3.zero;
			Vector3 vector2 = Vector3.zero;
			if (this.spawnPoint.Value != null)
			{
				vector = this.spawnPoint.Value.transform.position;
				if (!this.position.IsNone)
				{
					vector += this.position.Value;
				}
				if (!this.rotation.IsNone)
				{
					vector2 = this.rotation.Value;
				}
				else
				{
					vector2 = this.spawnPoint.Value.transform.eulerAngles;
				}
			}
			else
			{
				if (!this.position.IsNone)
				{
					vector = this.position.Value;
				}
				if (!this.rotation.IsNone)
				{
					vector2 = this.rotation.Value;
				}
			}
			GameObject gameObject = this.storeObject.Value;
			if (value != null)
			{
				gameObject = UnityEngine.Object.Instantiate<GameObject>(value);
				this.storeObject.Value = gameObject;
			}
			else
			{
				gameObject = new GameObject("EmptyObjectFromNull");
				this.storeObject.Value = gameObject;
			}
			if (gameObject != null)
			{
				gameObject.transform.position = vector;
				gameObject.transform.eulerAngles = vector2;
			}
			base.Finish();
		}

		// Token: 0x04002493 RID: 9363
		[Tooltip("Optional GameObject to create. Usually a Prefab.")]
		public FsmGameObject gameObject;

		// Token: 0x04002494 RID: 9364
		[Tooltip("Optional Spawn Point.")]
		public FsmGameObject spawnPoint;

		// Token: 0x04002495 RID: 9365
		[Tooltip("Position. If a Spawn Point is defined, this is used as a local offset from the Spawn Point position.")]
		public FsmVector3 position;

		// Token: 0x04002496 RID: 9366
		[Tooltip("Rotation. NOTE: Overrides the rotation of the Spawn Point.")]
		public FsmVector3 rotation;

		// Token: 0x04002497 RID: 9367
		[Tooltip("Optionally store the created object.")]
		[UIHint(UIHint.Variable)]
		public FsmGameObject storeObject;
	}
}
