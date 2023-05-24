using System;
using UnityEngine;

// Token: 0x0200165D RID: 5725
public class ProjectileSpriteFollower : MonoBehaviour
{
	// Token: 0x0600858E RID: 34190 RVA: 0x00371868 File Offset: 0x0036FA68
	private void Awake()
	{
		base.transform.parent = null;
		base.transform.localRotation = Quaternion.identity;
	}

	// Token: 0x0600858F RID: 34191 RVA: 0x00371888 File Offset: 0x0036FA88
	private void LateUpdate()
	{
		if (!this.TargetSprite)
		{
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		base.transform.position = Vector3.SmoothDamp(base.transform.position, this.TargetSprite.transform.position, ref this.m_currentVelocity, this.SmoothTime);
	}

	// Token: 0x040089CC RID: 35276
	public tk2dBaseSprite TargetSprite;

	// Token: 0x040089CD RID: 35277
	public float SmoothTime = 0.25f;

	// Token: 0x040089CE RID: 35278
	private Vector3 m_currentVelocity;
}
