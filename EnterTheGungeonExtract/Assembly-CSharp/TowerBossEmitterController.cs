using System;
using UnityEngine;

// Token: 0x02000CE6 RID: 3302
public class TowerBossEmitterController : MonoBehaviour
{
	// Token: 0x0600461A RID: 17946 RVA: 0x0016CCDC File Offset: 0x0016AEDC
	private void Start()
	{
		this.m_sprite = base.GetComponent<tk2dSprite>();
	}

	// Token: 0x0600461B RID: 17947 RVA: 0x0016CCEC File Offset: 0x0016AEEC
	public void UpdateAngle(float newAngle)
	{
		this.currentAngle = newAngle;
		float num = this.currentAngle / 3.1415927f;
		if (num > 0.05f && num < 0.95f)
		{
			this.m_sprite.renderer.enabled = false;
		}
		else if (num > 1.75f || num <= 0.05f)
		{
			this.m_sprite.renderer.enabled = true;
			this.m_sprite.SetSprite(this.eastSpriteName);
		}
		else if (num > 1.25f)
		{
			this.m_sprite.renderer.enabled = true;
			this.m_sprite.SetSprite(this.southSpriteName);
		}
		else
		{
			this.m_sprite.renderer.enabled = true;
			this.m_sprite.SetSprite(this.westSpriteName);
		}
	}

	// Token: 0x040038A4 RID: 14500
	public float currentAngle;

	// Token: 0x040038A5 RID: 14501
	public string eastSpriteName;

	// Token: 0x040038A6 RID: 14502
	public string westSpriteName;

	// Token: 0x040038A7 RID: 14503
	public string southSpriteName;

	// Token: 0x040038A8 RID: 14504
	private tk2dSprite m_sprite;
}
