using System;
using UnityEngine;

namespace Reaktion
{
	// Token: 0x02000833 RID: 2099
	public class JitterMotion : MonoBehaviour
	{
		// Token: 0x06002D7E RID: 11646 RVA: 0x000ED4C0 File Offset: 0x000EB6C0
		public Vector3 GetInitialPosition()
		{
			return this.initialPosition;
		}

		// Token: 0x06002D7F RID: 11647 RVA: 0x000ED4C8 File Offset: 0x000EB6C8
		public Quaternion GetInitialRotation()
		{
			return this.initialRotation;
		}

		// Token: 0x06002D80 RID: 11648 RVA: 0x000ED4D0 File Offset: 0x000EB6D0
		private void Awake()
		{
			this.timePosition = UnityEngine.Random.value * 10f;
			this.timeRotation = UnityEngine.Random.value * 10f;
			this.noiseVectors = new Vector2[6];
			for (int i = 0; i < 6; i++)
			{
				float num = UnityEngine.Random.value * 3.1415927f * 2f;
				this.noiseVectors[i].Set(Mathf.Cos(num), Mathf.Sin(num));
			}
			this.initialPosition = base.transform.localPosition;
			this.initialRotation = base.transform.localRotation;
		}

		// Token: 0x06002D81 RID: 11649 RVA: 0x000ED570 File Offset: 0x000EB770
		private void Update()
		{
			this.timePosition += BraveTime.DeltaTime * this.positionFrequency;
			this.timeRotation += BraveTime.DeltaTime * this.rotationFrequency;
			if (this.positionAmount != 0f)
			{
				Vector3 vector = new Vector3(JitterMotion.Fbm(this.noiseVectors[0] * this.timePosition, this.positionOctave), JitterMotion.Fbm(this.noiseVectors[1] * this.timePosition, this.positionOctave), JitterMotion.Fbm(this.noiseVectors[2] * this.timePosition, this.positionOctave));
				vector = Vector3.Scale(vector, this.positionComponents) * this.positionAmount * 2f;
				base.transform.localPosition = this.initialPosition + vector + GameManager.Instance.MainCameraController.ScreenShakeVector * 5f;
			}
			if (this.rotationAmount != 0f)
			{
				Vector3 vector2 = new Vector3(JitterMotion.Fbm(this.noiseVectors[3] * this.timeRotation, this.rotationOctave), JitterMotion.Fbm(this.noiseVectors[4] * this.timeRotation, this.rotationOctave), JitterMotion.Fbm(this.noiseVectors[5] * this.timeRotation, this.rotationOctave));
				vector2 = Vector3.Scale(vector2, this.rotationComponents) * this.rotationAmount * 2f;
				base.transform.localRotation = Quaternion.Euler(vector2) * this.initialRotation;
			}
			if (this.AllowCameraInfluence)
			{
				Vector2 vector3 = Vector2.zero;
				if (BraveInput.PrimaryPlayerInstance != null)
				{
					if (BraveInput.PrimaryPlayerInstance.IsKeyboardAndMouse(false))
					{
						vector3 = GameManager.Instance.MainCameraController.Camera.ScreenToViewportPoint(Input.mousePosition);
						vector3 = (vector3 + new Vector2(-0.5f, -0.5f)) * 2f;
					}
					else
					{
						vector3 = BraveInput.PrimaryPlayerInstance.ActiveActions.Aim.Vector;
					}
				}
				this.m_currentInfluenceVec = Vector2.MoveTowards(this.m_currentInfluenceVec, vector3, 1.25f * GameManager.INVARIANT_DELTA_TIME);
				float num = this.m_currentInfluenceVec.x * this.InfluenceAxialX;
				float num2 = this.m_currentInfluenceVec.y * this.InfluenceAxialY * -1f;
				base.transform.RotateAround(base.transform.position + base.transform.forward * 10f, Vector3.up, num);
				base.transform.RotateAround(base.transform.position + base.transform.forward * 10f, Vector3.right, num2);
			}
		}

		// Token: 0x06002D82 RID: 11650 RVA: 0x000ED8AC File Offset: 0x000EBAAC
		public static float Fbm(Vector2 coord, int octave)
		{
			float num = 0f;
			float num2 = 1f;
			for (int i = 0; i < octave; i++)
			{
				num += num2 * (Mathf.PerlinNoise(coord.x, coord.y) - 0.5f);
				coord *= 2f;
				num2 *= 0.5f;
			}
			return num;
		}

		// Token: 0x04001EC8 RID: 7880
		public bool AllowCameraInfluence;

		// Token: 0x04001EC9 RID: 7881
		public float InfluenceAxialX = 20f;

		// Token: 0x04001ECA RID: 7882
		public float InfluenceAxialY = 20f;

		// Token: 0x04001ECB RID: 7883
		public float positionFrequency = 0.2f;

		// Token: 0x04001ECC RID: 7884
		public float rotationFrequency = 0.2f;

		// Token: 0x04001ECD RID: 7885
		public float positionAmount = 1f;

		// Token: 0x04001ECE RID: 7886
		public float rotationAmount = 30f;

		// Token: 0x04001ECF RID: 7887
		public Vector3 positionComponents = Vector3.one;

		// Token: 0x04001ED0 RID: 7888
		public Vector3 rotationComponents = new Vector3(1f, 1f, 0f);

		// Token: 0x04001ED1 RID: 7889
		public int positionOctave = 3;

		// Token: 0x04001ED2 RID: 7890
		public int rotationOctave = 3;

		// Token: 0x04001ED3 RID: 7891
		public bool UseMainCameraShakeAmount = true;

		// Token: 0x04001ED4 RID: 7892
		private float timePosition;

		// Token: 0x04001ED5 RID: 7893
		private float timeRotation;

		// Token: 0x04001ED6 RID: 7894
		private Vector2[] noiseVectors;

		// Token: 0x04001ED7 RID: 7895
		private Vector3 initialPosition;

		// Token: 0x04001ED8 RID: 7896
		private Quaternion initialRotation;

		// Token: 0x04001ED9 RID: 7897
		private Vector2 m_currentInfluenceVec = Vector2.zero;
	}
}
