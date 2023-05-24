using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200106E RID: 4206
public class ResourcefulRatDeathController : BraveBehaviour
{
	// Token: 0x06005C8C RID: 23692 RVA: 0x00237414 File Offset: 0x00235614
	public void Start()
	{
		base.healthHaver.ManualDeathHandling = true;
		base.healthHaver.OnPreDeath += this.OnBossDeath;
		base.healthHaver.OverrideKillCamTime = new float?(5f);
		base.healthHaver.TrackDuringDeath = true;
	}

	// Token: 0x06005C8D RID: 23693 RVA: 0x00237468 File Offset: 0x00235668
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x06005C8E RID: 23694 RVA: 0x00237470 File Offset: 0x00235670
	private void OnBossDeath(Vector2 dir)
	{
		base.StartCoroutine(this.BossDeathCR());
		base.healthHaver.OnPreDeath -= this.OnBossDeath;
	}

	// Token: 0x06005C8F RID: 23695 RVA: 0x00237498 File Offset: 0x00235698
	private IEnumerator BossDeathCR()
	{
		yield return new WaitForSeconds(0.66f);
		ResourcefulRatBossRoomController roomController = UnityEngine.Object.FindObjectOfType<ResourcefulRatBossRoomController>();
		roomController.OpenGrate();
		yield return new WaitForSeconds(0.33f);
		Vector2 target = base.aiActor.ParentRoom.area.UnitBottomLeft + new Vector2(17f, 12f);
		Vector2 toTarget = target - base.specRigidbody.UnitCenter;
		base.aiAnimator.LockFacingDirection = true;
		base.aiAnimator.FacingDirection = toTarget.ToAngle();
		bool ratOnGrate = !base.specRigidbody.UnitCenter.IsWithin(target + new Vector2(-2f, -2f), target + new Vector2(2f, 2f));
		if (ratOnGrate)
		{
			base.aiAnimator.PlayUntilCancelled("move", false, null, -1f, false);
			float moveSpeed = 7f;
			bool hasDove = false;
			Vector2 velocity = toTarget.normalized * moveSpeed;
			float timer = toTarget.magnitude / moveSpeed;
			while (timer > 0f)
			{
				base.specRigidbody.Velocity = velocity;
				timer -= BraveTime.DeltaTime;
				if (!hasDove)
				{
					float magnitude = (target - base.specRigidbody.UnitCenter).magnitude;
					if (magnitude < 2.5f)
					{
						base.aiAnimator.PlayUntilCancelled("dodge", false, null, -1f, false);
						hasDove = true;
					}
				}
				yield return null;
			}
		}
		base.specRigidbody.Velocity = Vector2.zero;
		base.aiAnimator.PlayUntilCancelled("pitfall", false, null, -1f, false);
		yield return new WaitForSeconds(base.aiAnimator.CurrentClipLength);
		roomController.EnablePitfalls(true);
		BossKillCam extantCam = UnityEngine.Object.FindObjectOfType<BossKillCam>();
		if (extantCam)
		{
			extantCam.SetPhaseCountdown(0.5f);
		}
		base.aiActor.StealthDeath = true;
		base.healthHaver.DeathAnimationComplete(null, null);
		UnityEngine.Object.Destroy(base.gameObject);
		yield break;
	}
}
