using System;
using UnityEngine;

// Token: 0x0200127D RID: 4733
public class FriendlyFireChallengeModifier : ChallengeModifier
{
	// Token: 0x060069FB RID: 27131 RVA: 0x00298838 File Offset: 0x00296A38
	private void Start()
	{
		GameManager.PVP_ENABLED = true;
		for (int i = 0; i < GameManager.Instance.AllPlayers.Length; i++)
		{
			GameManager.Instance.AllPlayers[i].PostProcessProjectile += this.ModifyProjectile;
		}
	}

	// Token: 0x060069FC RID: 27132 RVA: 0x00298888 File Offset: 0x00296A88
	private void ModifyProjectile(Projectile proj, float somethin)
	{
		if (proj && !proj.TreatedAsNonProjectileForChallenge)
		{
			tk2dBaseSprite componentInChildren = proj.GetComponentInChildren<tk2dBaseSprite>();
			Renderer componentInChildren2 = proj.GetComponentInChildren<Renderer>();
			if (componentInChildren && !componentInChildren.GetComponent<TrailController>() && componentInChildren2 && componentInChildren2.enabled)
			{
				BounceProjModifier bounceProjModifier = proj.GetComponent<BounceProjModifier>();
				if (!bounceProjModifier)
				{
					bounceProjModifier = proj.gameObject.AddComponent<BounceProjModifier>();
					bounceProjModifier.numberOfBounces = 1;
					bounceProjModifier.onlyBounceOffTiles = true;
				}
				BounceProjModifier bounceProjModifier2 = bounceProjModifier;
				bounceProjModifier2.OnBounceContext = (Action<BounceProjModifier, SpeculativeRigidbody>)Delegate.Combine(bounceProjModifier2.OnBounceContext, new Action<BounceProjModifier, SpeculativeRigidbody>(this.OnFirstBounce));
			}
		}
	}

	// Token: 0x060069FD RID: 27133 RVA: 0x00298938 File Offset: 0x00296B38
	private void OnFirstBounce(BounceProjModifier mod, SpeculativeRigidbody otherRigidbody)
	{
		if (mod)
		{
			mod.OnBounceContext = (Action<BounceProjModifier, SpeculativeRigidbody>)Delegate.Remove(mod.OnBounceContext, new Action<BounceProjModifier, SpeculativeRigidbody>(this.OnFirstBounce));
			Projectile component = mod.GetComponent<Projectile>();
			if (component)
			{
				if (otherRigidbody && otherRigidbody.minorBreakable)
				{
					component.DieInAir(false, true, true, false);
				}
				else
				{
					component.MakeLookLikeEnemyBullet(false);
					component.baseData.speed = Mathf.Min(component.baseData.speed, 10f);
					component.Speed = Mathf.Min(component.Speed, 10f);
					component.allowSelfShooting = true;
					component.ForcePlayerBlankable = true;
					if (component.Shooter)
					{
						component.specRigidbody.DeregisterSpecificCollisionException(component.Shooter);
						component.specRigidbody.RegisterGhostCollisionException(component.Shooter);
					}
				}
			}
		}
	}

	// Token: 0x060069FE RID: 27134 RVA: 0x00298A2C File Offset: 0x00296C2C
	private void OnDestroy()
	{
		GameManager.PVP_ENABLED = false;
		for (int i = 0; i < GameManager.Instance.AllPlayers.Length; i++)
		{
			GameManager.Instance.AllPlayers[i].PostProcessProjectile -= this.ModifyProjectile;
		}
	}
}
