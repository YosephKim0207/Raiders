using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000FC1 RID: 4033
public class BashelliskDeathController : BraveBehaviour
{
	// Token: 0x060057DA RID: 22490 RVA: 0x00218590 File Offset: 0x00216790
	public void Start()
	{
		base.healthHaver.ManualDeathHandling = true;
		base.healthHaver.OnPreDeath += this.OnBossDeath;
	}

	// Token: 0x060057DB RID: 22491 RVA: 0x002185B8 File Offset: 0x002167B8
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x060057DC RID: 22492 RVA: 0x002185C0 File Offset: 0x002167C0
	private void OnBossDeath(Vector2 dir)
	{
		base.StartCoroutine(this.OnDeathCR());
	}

	// Token: 0x060057DD RID: 22493 RVA: 0x002185D0 File Offset: 0x002167D0
	private IEnumerator OnDeathCR()
	{
		BashelliskHeadController head = base.GetComponent<BashelliskHeadController>();
		head.behaviorSpeculator.enabled = false;
		head.enabled = false;
		head.StopAllCoroutines();
		while (head.AvailablePickups.Count > 0)
		{
			BashelliskBodyPickupController value = head.AvailablePickups.First.Value;
			if (value && value.healthHaver)
			{
				value.healthHaver.ApplyDamage(100000f, Vector2.zero, "death", CoreDamageTypes.None, DamageCategory.Unstoppable, false, null, false);
			}
			head.AvailablePickups.RemoveFirst();
		}
		head.aiAnimator.PlayUntilCancelled("die", true, null, -1f, false);
		LinkedListNode<BashelliskSegment> node = head.Body.Last;
		float delay = 0.3f;
		while (node != null)
		{
			if (node.Value is BashelliskBodyController)
			{
				BashelliskBodyController bashelliskBodyController = node.Value as BashelliskBodyController;
				AkSoundEngine.PostEvent("Play_WPN_grenade_blast_01", base.gameObject);
				bashelliskBodyController.enabled = false;
				bashelliskBodyController.majorBreakable.breakVfx.SpawnAtPosition(bashelliskBodyController.specRigidbody.GetUnitCenter(ColliderType.HitBox), 0f, null, null, null, null, false, null, null, false);
				UnityEngine.Object.Destroy(bashelliskBodyController.gameObject);
			}
			else if (node.Value == head)
			{
				head.enabled = false;
				AkSoundEngine.PostEvent("Play_ENM_Kali_explode_01", base.gameObject);
				this.HeadVfx.SpawnAtPosition(head.specRigidbody.GetUnitCenter(ColliderType.HitBox), 0f, null, null, null, null, false, null, null, false);
			}
			node = node.Previous;
			if (node != null)
			{
				yield return new WaitForSeconds(delay);
			}
			delay *= 0.9f;
		}
		base.aiActor.StealthDeath = true;
		base.healthHaver.DeathAnimationComplete(null, null);
		UnityEngine.Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x040050D5 RID: 20693
	public VFXPool HeadVfx;
}
