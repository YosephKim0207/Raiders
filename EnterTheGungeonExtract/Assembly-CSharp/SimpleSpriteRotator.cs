using System;
using UnityEngine;

// Token: 0x020016C2 RID: 5826
public class SimpleSpriteRotator : MonoBehaviour
{
	// Token: 0x06008774 RID: 34676 RVA: 0x00382B20 File Offset: 0x00380D20
	private void Start()
	{
		this.m_transform = ((!this.RotateParent) ? base.transform : base.transform.parent);
		this.m_sprite = base.GetComponent<tk2dSprite>();
		if (this.RandomStartingAngle)
		{
			this.DoRotation((float)UnityEngine.Random.Range(0, 360));
		}
	}

	// Token: 0x06008775 RID: 34677 RVA: 0x00382B80 File Offset: 0x00380D80
	private void Update()
	{
		float num = BraveTime.DeltaTime;
		if (this.RotateDuringBossIntros && GameManager.IsBossIntro)
		{
			num = GameManager.INVARIANT_DELTA_TIME;
		}
		this.angularVelocity += this.acceleration * num;
		this.DoRotation(this.angularVelocity * num);
	}

	// Token: 0x06008776 RID: 34678 RVA: 0x00382BD4 File Offset: 0x00380DD4
	private void DoRotation(float degrees)
	{
		if (this.UseWorldCenter)
		{
			this.m_transform.RotateAround(this.m_sprite.WorldCenter, Vector3.forward, degrees);
		}
		else
		{
			this.m_transform.Rotate(Vector3.forward, degrees);
		}
		if (this.ForceUpdateZDepth)
		{
			this.m_sprite.ForceRotationRebuild();
			this.m_sprite.UpdateZDepth();
		}
	}

	// Token: 0x04008C9B RID: 35995
	public float angularVelocity;

	// Token: 0x04008C9C RID: 35996
	public float acceleration;

	// Token: 0x04008C9D RID: 35997
	public bool UseWorldCenter = true;

	// Token: 0x04008C9E RID: 35998
	public bool ForceUpdateZDepth;

	// Token: 0x04008C9F RID: 35999
	public bool RotateParent;

	// Token: 0x04008CA0 RID: 36000
	public bool RotateDuringBossIntros;

	// Token: 0x04008CA1 RID: 36001
	public bool RandomStartingAngle;

	// Token: 0x04008CA2 RID: 36002
	private Transform m_transform;

	// Token: 0x04008CA3 RID: 36003
	private tk2dSprite m_sprite;
}
