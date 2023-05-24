using System;
using UnityEngine;

// Token: 0x020014A0 RID: 5280
public class ShootPlayerProjectiles : MonoBehaviour
{
	// Token: 0x0600781D RID: 30749 RVA: 0x00300154 File Offset: 0x002FE354
	private void Start()
	{
		this.m_cooldown = UnityEngine.Random.Range(0f, this.ShootCooldown);
		this.m_animator = base.GetComponent<tk2dSpriteAnimator>();
	}

	// Token: 0x0600781E RID: 30750 RVA: 0x00300178 File Offset: 0x002FE378
	private void Update()
	{
		if (this.RequiresAnimation && !this.m_animator.IsPlaying(this.m_animator.CurrentClip))
		{
			UnityEngine.Object.Destroy(this);
			return;
		}
		this.m_cooldown -= BraveTime.DeltaTime;
		if (this.m_cooldown <= 0f)
		{
			VolleyUtility.FireVolley(this.Volley, this.ShootPoint.position.XY(), UnityEngine.Random.insideUnitCircle.normalized, GameManager.Instance.BestActivePlayer, false);
			this.m_cooldown += this.ShootCooldown;
		}
	}

	// Token: 0x04007A38 RID: 31288
	public ProjectileVolleyData Volley;

	// Token: 0x04007A39 RID: 31289
	public Transform ShootPoint;

	// Token: 0x04007A3A RID: 31290
	public float ShootCooldown = 1f;

	// Token: 0x04007A3B RID: 31291
	public ShootPlayerProjectiles.ArbitraryShootStyle style;

	// Token: 0x04007A3C RID: 31292
	public bool RequiresAnimation;

	// Token: 0x04007A3D RID: 31293
	private tk2dSpriteAnimator m_animator;

	// Token: 0x04007A3E RID: 31294
	private float m_cooldown;

	// Token: 0x020014A1 RID: 5281
	public enum ArbitraryShootStyle
	{
		// Token: 0x04007A40 RID: 31296
		RANDOM
	}
}
