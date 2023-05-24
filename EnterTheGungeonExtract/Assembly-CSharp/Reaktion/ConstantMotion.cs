using System;
using UnityEngine;

namespace Reaktion
{
	// Token: 0x02000830 RID: 2096
	public class ConstantMotion : MonoBehaviour
	{
		// Token: 0x06002D77 RID: 11639 RVA: 0x000ED244 File Offset: 0x000EB444
		private void Awake()
		{
			this.position.Initialize();
			this.rotation.Initialize();
		}

		// Token: 0x06002D78 RID: 11640 RVA: 0x000ED25C File Offset: 0x000EB45C
		private void Update()
		{
			if (this.position.mode != ConstantMotion.TransformMode.Off)
			{
				if (this.useLocalCoordinate)
				{
					base.transform.localPosition += this.position.Vector * this.position.Delta;
				}
				else
				{
					base.transform.position += this.position.Vector * this.position.Delta;
				}
			}
			if (this.rotation.mode != ConstantMotion.TransformMode.Off)
			{
				Quaternion quaternion = Quaternion.AngleAxis(this.rotation.Delta, this.rotation.Vector);
				if (this.useLocalCoordinate)
				{
					base.transform.localRotation = quaternion * base.transform.localRotation;
				}
				else
				{
					base.transform.rotation = quaternion * base.transform.rotation;
				}
			}
		}

		// Token: 0x04001EB8 RID: 7864
		public ConstantMotion.TransformElement position = new ConstantMotion.TransformElement();

		// Token: 0x04001EB9 RID: 7865
		public ConstantMotion.TransformElement rotation = new ConstantMotion.TransformElement
		{
			velocity = 30f
		};

		// Token: 0x04001EBA RID: 7866
		public bool useLocalCoordinate = true;

		// Token: 0x02000831 RID: 2097
		public enum TransformMode
		{
			// Token: 0x04001EBC RID: 7868
			Off,
			// Token: 0x04001EBD RID: 7869
			XAxis,
			// Token: 0x04001EBE RID: 7870
			YAxis,
			// Token: 0x04001EBF RID: 7871
			ZAxis,
			// Token: 0x04001EC0 RID: 7872
			Arbitrary,
			// Token: 0x04001EC1 RID: 7873
			Random
		}

		// Token: 0x02000832 RID: 2098
		[Serializable]
		public class TransformElement
		{
			// Token: 0x06002D7A RID: 11642 RVA: 0x000ED380 File Offset: 0x000EB580
			public void Initialize()
			{
				this.randomVector = UnityEngine.Random.onUnitSphere;
				this.randomScalar = UnityEngine.Random.value;
			}

			// Token: 0x17000860 RID: 2144
			// (get) Token: 0x06002D7B RID: 11643 RVA: 0x000ED398 File Offset: 0x000EB598
			public Vector3 Vector
			{
				get
				{
					switch (this.mode)
					{
					case ConstantMotion.TransformMode.XAxis:
						return Vector3.right;
					case ConstantMotion.TransformMode.YAxis:
						return Vector3.up;
					case ConstantMotion.TransformMode.ZAxis:
						return Vector3.forward;
					case ConstantMotion.TransformMode.Arbitrary:
						return this.arbitraryVector;
					case ConstantMotion.TransformMode.Random:
						return this.randomVector;
					default:
						return Vector3.zero;
					}
				}
			}

			// Token: 0x17000861 RID: 2145
			// (get) Token: 0x06002D7C RID: 11644 RVA: 0x000ED3F4 File Offset: 0x000EB5F4
			public float Delta
			{
				get
				{
					float num = 1f - this.randomness * this.randomScalar;
					return this.velocity * num * BraveTime.DeltaTime;
				}
			}

			// Token: 0x04001EC2 RID: 7874
			public ConstantMotion.TransformMode mode;

			// Token: 0x04001EC3 RID: 7875
			public float velocity = 1f;

			// Token: 0x04001EC4 RID: 7876
			public Vector3 arbitraryVector = Vector3.up;

			// Token: 0x04001EC5 RID: 7877
			public float randomness;

			// Token: 0x04001EC6 RID: 7878
			private Vector3 randomVector;

			// Token: 0x04001EC7 RID: 7879
			private float randomScalar;
		}
	}
}
