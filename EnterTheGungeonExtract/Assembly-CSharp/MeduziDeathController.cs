using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200105F RID: 4191
public class MeduziDeathController : BraveBehaviour
{
	// Token: 0x06005C21 RID: 23585 RVA: 0x002350C8 File Offset: 0x002332C8
	public void Start()
	{
		base.healthHaver.ManualDeathHandling = true;
		base.healthHaver.OnPreDeath += this.OnBossDeath;
	}

	// Token: 0x06005C22 RID: 23586 RVA: 0x002350F0 File Offset: 0x002332F0
	protected override void OnDestroy()
	{
		SpeculativeRigidbody specRigidbody = base.specRigidbody;
		specRigidbody.OnRigidbodyCollision = (SpeculativeRigidbody.OnRigidbodyCollisionDelegate)Delegate.Remove(specRigidbody.OnRigidbodyCollision, new SpeculativeRigidbody.OnRigidbodyCollisionDelegate(this.OnRigidbodyCollision));
		base.OnDestroy();
	}

	// Token: 0x06005C23 RID: 23587 RVA: 0x00235120 File Offset: 0x00233320
	public void Shatter()
	{
		base.aiAnimator.enabled = false;
		base.spriteAnimator.PlayAndDestroyObject("burst", null);
		base.specRigidbody.enabled = false;
		SpeculativeRigidbody specRigidbody = base.specRigidbody;
		specRigidbody.OnRigidbodyCollision = (SpeculativeRigidbody.OnRigidbodyCollisionDelegate)Delegate.Remove(specRigidbody.OnRigidbodyCollision, new SpeculativeRigidbody.OnRigidbodyCollisionDelegate(this.OnRigidbodyCollision));
	}

	// Token: 0x06005C24 RID: 23588 RVA: 0x00235180 File Offset: 0x00233380
	private void OnBossDeath(Vector2 dir)
	{
		base.aiAnimator.PlayUntilCancelled("death", true, null, -1f, false);
		base.StartCoroutine(this.HandlePostDeathExplosionCR());
		base.healthHaver.OnPreDeath -= this.OnBossDeath;
	}

	// Token: 0x06005C25 RID: 23589 RVA: 0x002351C0 File Offset: 0x002333C0
	private IEnumerator HandlePostDeathExplosionCR()
	{
		while (base.aiAnimator.IsPlaying("death"))
		{
			yield return null;
		}
		base.aiActor.StealthDeath = true;
		base.healthHaver.persistsOnDeath = true;
		base.healthHaver.DeathAnimationComplete(null, null);
		if (base.aiActor)
		{
			UnityEngine.Object.Destroy(base.aiActor);
		}
		if (base.healthHaver)
		{
			UnityEngine.Object.Destroy(base.healthHaver);
		}
		if (base.behaviorSpeculator)
		{
			UnityEngine.Object.Destroy(base.behaviorSpeculator);
		}
		base.RegenerateCache();
		base.specRigidbody.CollideWithOthers = true;
		SpeculativeRigidbody specRigidbody = base.specRigidbody;
		specRigidbody.OnRigidbodyCollision = (SpeculativeRigidbody.OnRigidbodyCollisionDelegate)Delegate.Combine(specRigidbody.OnRigidbodyCollision, new SpeculativeRigidbody.OnRigidbodyCollisionDelegate(this.OnRigidbodyCollision));
		yield break;
	}

	// Token: 0x06005C26 RID: 23590 RVA: 0x002351DC File Offset: 0x002333DC
	private void OnRigidbodyCollision(CollisionData collision)
	{
		if (collision.OtherRigidbody.projectile)
		{
			this.Shatter();
		}
	}
}
