using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000922 RID: 2338
	[ActionTarget(typeof(GameObject), "gameObject", true)]
	[Tooltip("Creates a Game Object, usually using a Prefab.")]
	[ActionCategory(ActionCategory.GameObject)]
	public class CreateObject : FsmStateAction
	{
		// Token: 0x0600336D RID: 13165 RVA: 0x0010D114 File Offset: 0x0010B314
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
			this.networkInstantiate = false;
			this.networkGroup = 0;
		}

		// Token: 0x0600336E RID: 13166 RVA: 0x0010D178 File Offset: 0x0010B378
		public override void OnEnter()
		{
			GameObject value = this.gameObject.Value;
			if (value != null)
			{
				Vector3 vector = Vector3.zero;
				Vector3 vector2 = Vector3.zero;
				if (this.spawnPoint.Value != null)
				{
					vector = this.spawnPoint.Value.transform.position;
					if (!this.position.IsNone)
					{
						vector += this.position.Value;
					}
					vector2 = (this.rotation.IsNone ? this.spawnPoint.Value.transform.eulerAngles : this.rotation.Value);
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
				GameObject gameObject;
				if (!this.networkInstantiate.Value)
				{
					gameObject = UnityEngine.Object.Instantiate<GameObject>(value, vector, Quaternion.Euler(vector2));
				}
				else
				{
					gameObject = (GameObject)Network.Instantiate(value, vector, Quaternion.Euler(vector2), this.networkGroup.Value);
				}
				this.storeObject.Value = gameObject;
			}
			base.Finish();
		}

		// Token: 0x04002498 RID: 9368
		[Tooltip("GameObject to create. Usually a Prefab.")]
		[RequiredField]
		public FsmGameObject gameObject;

		// Token: 0x04002499 RID: 9369
		[Tooltip("Optional Spawn Point.")]
		public FsmGameObject spawnPoint;

		// Token: 0x0400249A RID: 9370
		[Tooltip("Position. If a Spawn Point is defined, this is used as a local offset from the Spawn Point position.")]
		public FsmVector3 position;

		// Token: 0x0400249B RID: 9371
		[Tooltip("Rotation. NOTE: Overrides the rotation of the Spawn Point.")]
		public FsmVector3 rotation;

		// Token: 0x0400249C RID: 9372
		[Tooltip("Optionally store the created object.")]
		[UIHint(UIHint.Variable)]
		public FsmGameObject storeObject;

		// Token: 0x0400249D RID: 9373
		[Tooltip("Use Network.Instantiate to create a Game Object on all clients in a networked game.")]
		public FsmBool networkInstantiate;

		// Token: 0x0400249E RID: 9374
		[Tooltip("Usually 0. The group number allows you to group together network messages which allows you to filter them if so desired.")]
		public FsmInt networkGroup;
	}
}
