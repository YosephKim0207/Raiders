using System;
using System.Collections;
using UnityEngine;

// Token: 0x02001485 RID: 5253
public class RemoteMineController : BraveBehaviour
{
	// Token: 0x06007772 RID: 30578 RVA: 0x002F9E9C File Offset: 0x002F809C
	public void Detonate()
	{
		if (!string.IsNullOrEmpty(this.explodeAnimName))
		{
			base.StartCoroutine(this.DelayDetonateFrame());
		}
		else
		{
			Exploder.Explode(base.transform.position, this.explosionData, Vector2.zero, null, false, CoreDamageTypes.None, false);
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x06007773 RID: 30579 RVA: 0x002F9EF8 File Offset: 0x002F80F8
	private IEnumerator DelayDetonateFrame()
	{
		base.spriteAnimator.Play(this.explodeAnimName);
		yield return new WaitForSeconds(0.05f);
		if (this.explosionData.damageToPlayer > 0f)
		{
			this.explosionData.damageToPlayer = 0f;
		}
		Exploder.Explode(base.sprite.WorldCenter.ToVector3ZUp(0f), this.explosionData, Vector2.zero, null, false, CoreDamageTypes.None, false);
		UnityEngine.Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x06007774 RID: 30580 RVA: 0x002F9F14 File Offset: 0x002F8114
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x04007969 RID: 31081
	public ExplosionData explosionData;

	// Token: 0x0400796A RID: 31082
	[CheckAnimation(null)]
	public string explodeAnimName;
}
