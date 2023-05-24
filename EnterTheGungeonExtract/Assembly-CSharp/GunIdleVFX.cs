using System;
using UnityEngine;

// Token: 0x02001415 RID: 5141
public class GunIdleVFX : MonoBehaviour
{
	// Token: 0x060074A6 RID: 29862 RVA: 0x002E6F5C File Offset: 0x002E515C
	private void Awake()
	{
		this.m_gun = base.GetComponent<Gun>();
	}

	// Token: 0x060074A7 RID: 29863 RVA: 0x002E6F6C File Offset: 0x002E516C
	private void Update()
	{
		if (this.idleVFX.gameObject.activeSelf && this.m_gun && this.m_gun.sprite)
		{
			if (!this.idleVFX.IsPlaying(this.idleVFX.DefaultClip))
			{
				this.idleVFX.Play();
			}
			this.idleVFX.sprite.FlipY = this.m_gun.sprite.FlipY;
			this.idleVFX.transform.localPosition = this.idleVFX.transform.localPosition.WithY(Mathf.Abs(this.idleVFX.transform.localPosition.y) * (float)((!this.idleVFX.sprite.FlipY) ? 1 : (-1)));
			this.idleVFX.renderer.enabled = this.m_gun.renderer.enabled;
		}
	}

	// Token: 0x04007680 RID: 30336
	public tk2dSpriteAnimator idleVFX;

	// Token: 0x04007681 RID: 30337
	private Gun m_gun;
}
