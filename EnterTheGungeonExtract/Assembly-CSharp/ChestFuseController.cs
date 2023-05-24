using System;
using UnityEngine;

// Token: 0x02001370 RID: 4976
public class ChestFuseController : MonoBehaviour
{
	// Token: 0x060070BE RID: 28862 RVA: 0x002CBDDC File Offset: 0x002C9FDC
	private void CalcLength()
	{
		this.totalLength = 0f;
		for (int i = 0; i < this.fuseSegmentsInOrderOfAppearance.Length; i++)
		{
			this.totalLength += this.fuseSegmentsInOrderOfAppearance[i].dimensions.x;
		}
	}

	// Token: 0x060070BF RID: 28863 RVA: 0x002CBE30 File Offset: 0x002CA030
	public Vector2? SetFuseCompletion(float t)
	{
		if (this.totalLength < 0f)
		{
			this.CalcLength();
		}
		float num = Mathf.Clamp01(1f - t) * this.totalLength;
		Vector2? vector = null;
		for (int i = 0; i < this.fuseSegmentsInOrderOfAppearance.Length; i++)
		{
			if (num < 0f)
			{
				break;
			}
			if (num > this.fuseSegmentsInOrderOfAppearance[i].dimensions.x)
			{
				num -= this.fuseSegmentsInOrderOfAppearance[i].dimensions.x;
			}
			else
			{
				this.fuseSegmentsInOrderOfAppearance[i].dimensions = this.fuseSegmentsInOrderOfAppearance[i].dimensions.WithX(num);
				this.m_accumParticles += 30f * BraveTime.DeltaTime;
				int num2 = Mathf.FloorToInt(this.m_accumParticles);
				this.m_accumParticles -= (float)num2;
				Vector3 vector2 = this.fuseSegmentsInOrderOfAppearance[i].transform.position + (Quaternion.Euler(0f, 0f, this.fuseSegmentsInOrderOfAppearance[i].transform.eulerAngles.z) * this.fuseSegmentsInOrderOfAppearance[i].dimensions.ToVector3ZUp(0f) * 0.0625f).XY().ToVector3ZisY(0f);
				vector = new Vector2?(vector2.XY());
				float num3 = ((this.fuseSegmentsInOrderOfAppearance[i].transform.eulerAngles.z != 0f) ? 0f : (-0.0625f));
				GlobalSparksDoer.DoRandomParticleBurst(num2, vector2 + new Vector3(-0.125f, -0.125f + num3, 0f), vector2 + new Vector3(0f, num3, 0f), Vector3.up, 180f, 0.25f, null, null, new Color?(Color.yellow), GlobalSparksDoer.SparksType.SPARKS_ADDITIVE_DEFAULT);
			}
		}
		return vector;
	}

	// Token: 0x04007048 RID: 28744
	public tk2dTiledSprite[] fuseSegmentsInOrderOfAppearance;

	// Token: 0x04007049 RID: 28745
	public GameObject sparksVFXPrefab;

	// Token: 0x0400704A RID: 28746
	private Transform sparksInstance;

	// Token: 0x0400704B RID: 28747
	private float totalLength = -1f;

	// Token: 0x0400704C RID: 28748
	private float m_accumParticles;
}
