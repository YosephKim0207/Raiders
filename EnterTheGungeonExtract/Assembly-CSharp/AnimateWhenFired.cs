using System;
using Brave.BulletScript;
using UnityEngine;

// Token: 0x02000FA4 RID: 4004
public class AnimateWhenFired : BraveBehaviour
{
	// Token: 0x06005731 RID: 22321 RVA: 0x00214138 File Offset: 0x00212338
	public void Start()
	{
		if (!this.specifyAiAnimator)
		{
			this.specifyAiAnimator = base.aiAnimator;
		}
		if (this.trigger == AnimateWhenFired.TriggerType.BulletBankTransform)
		{
			if (!this.specifyBulletBank)
			{
				this.specifyBulletBank = base.bulletBank;
			}
			this.specifyBulletBank.OnBulletSpawned += this.BulletSpawned;
		}
	}

	// Token: 0x06005732 RID: 22322 RVA: 0x002141A0 File Offset: 0x002123A0
	protected override void OnDestroy()
	{
		if (this.trigger == AnimateWhenFired.TriggerType.BulletBankTransform && this.specifyBulletBank)
		{
			this.specifyBulletBank.OnBulletSpawned += this.BulletSpawned;
		}
		base.OnDestroy();
	}

	// Token: 0x06005733 RID: 22323 RVA: 0x002141DC File Offset: 0x002123DC
	private void BulletSpawned(Bullet bullet, Projectile projectile)
	{
		if (this.transformName == bullet.SpawnTransform)
		{
			this.specifyAiAnimator.PlayUntilFinished(this.fireAnim, false, null, -1f, false);
		}
	}

	// Token: 0x04005018 RID: 20504
	[Header("Trigger")]
	public AnimateWhenFired.TriggerType trigger;

	// Token: 0x04005019 RID: 20505
	[ShowInInspectorIf("trigger", 0, false)]
	public AIBulletBank specifyBulletBank;

	// Token: 0x0400501A RID: 20506
	[ShowInInspectorIf("trigger", 0, false)]
	public string transformName;

	// Token: 0x0400501B RID: 20507
	[Header("Animation")]
	public AIAnimator specifyAiAnimator;

	// Token: 0x0400501C RID: 20508
	[CheckDirectionalAnimation(null)]
	public string fireAnim;

	// Token: 0x02000FA5 RID: 4005
	public enum TriggerType
	{
		// Token: 0x0400501E RID: 20510
		BulletBankTransform
	}
}
