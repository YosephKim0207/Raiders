using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000A3F RID: 2623
	[Tooltip("Creates a Game Object on all clients in a network game.")]
	[ActionCategory(ActionCategory.Network)]
	public class NetworkInstantiate : FsmStateAction
	{
		// Token: 0x060037F2 RID: 14322 RVA: 0x0011FAC8 File Offset: 0x0011DCC8
		public override void Reset()
		{
			this.prefab = null;
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
			this.networkGroup = 0;
		}

		// Token: 0x060037F3 RID: 14323 RVA: 0x0011FB20 File Offset: 0x0011DD20
		public override void OnEnter()
		{
			GameObject value = this.prefab.Value;
			if (value != null)
			{
				Vector3 vector = Vector3.zero;
				Vector3 vector2 = Vector3.up;
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
				GameObject gameObject = (GameObject)Network.Instantiate(value, vector, Quaternion.Euler(vector2), this.networkGroup.Value);
				this.storeObject.Value = gameObject;
			}
			base.Finish();
		}

		// Token: 0x040029FF RID: 10751
		[Tooltip("The prefab will be instanted on all clients in the game.")]
		[RequiredField]
		public FsmGameObject prefab;

		// Token: 0x04002A00 RID: 10752
		[Tooltip("Optional Spawn Point.")]
		public FsmGameObject spawnPoint;

		// Token: 0x04002A01 RID: 10753
		[Tooltip("Spawn Position. If a Spawn Point is defined, this is used as a local offset from the Spawn Point position.")]
		public FsmVector3 position;

		// Token: 0x04002A02 RID: 10754
		[Tooltip("Spawn Rotation. NOTE: Overrides the rotation of the Spawn Point.")]
		public FsmVector3 rotation;

		// Token: 0x04002A03 RID: 10755
		[UIHint(UIHint.Variable)]
		[Tooltip("Optionally store the created object.")]
		public FsmGameObject storeObject;

		// Token: 0x04002A04 RID: 10756
		[Tooltip("Usually 0. The group number allows you to group together network messages which allows you to filter them if so desired.")]
		public FsmInt networkGroup;
	}
}
