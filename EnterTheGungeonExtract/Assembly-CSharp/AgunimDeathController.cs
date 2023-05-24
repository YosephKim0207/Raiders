using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000FB9 RID: 4025
public class AgunimDeathController : BraveBehaviour
{
	// Token: 0x060057A3 RID: 22435 RVA: 0x00217430 File Offset: 0x00215630
	public void Start()
	{
		base.healthHaver.ManualDeathHandling = true;
		base.healthHaver.OnPreDeath += this.OnBossDeath;
		base.healthHaver.OverrideKillCamTime = new float?(5f);
	}

	// Token: 0x060057A4 RID: 22436 RVA: 0x0021746C File Offset: 0x0021566C
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x060057A5 RID: 22437 RVA: 0x00217474 File Offset: 0x00215674
	private void OnBossDeath(Vector2 dir)
	{
		base.aiAnimator.ChildAnimator.gameObject.SetActive(false);
		base.aiAnimator.PlayUntilCancelled("death", true, null, -1f, false);
		base.StartCoroutine(this.HandlePostDeathExplosionCR());
		base.healthHaver.OnPreDeath -= this.OnBossDeath;
		base.StartCoroutine(this.HandlePostDeathExplosionCR());
	}

	// Token: 0x060057A6 RID: 22438 RVA: 0x002174E4 File Offset: 0x002156E4
	private IEnumerator HandlePostDeathExplosionCR()
	{
		yield return null;
		BossKillCam extantCam = UnityEngine.Object.FindObjectOfType<BossKillCam>();
		if (extantCam)
		{
			extantCam.ForceCancelSequence();
		}
		base.aiActor.StealthDeath = true;
		base.healthHaver.persistsOnDeath = true;
		base.healthHaver.DeathAnimationComplete(null, null);
		UnityEngine.Object.Destroy(base.GetComponent<AgunimIntroDoer>());
		base.aiActor.ToggleRenderers(false);
		if (base.specRigidbody)
		{
			base.specRigidbody.enabled = false;
		}
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
		BulletPastRoomController[] bprcs = UnityEngine.Object.FindObjectsOfType<BulletPastRoomController>();
		for (int i = 0; i < bprcs.Length; i++)
		{
			if (bprcs[i].RoomIdentifier == BulletPastRoomController.BulletRoomCategory.ROOM_C)
			{
				yield return base.StartCoroutine(bprcs[i].HandleAgunimDeath(base.transform));
				break;
			}
		}
		yield break;
	}
}
