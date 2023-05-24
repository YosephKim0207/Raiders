using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001625 RID: 5669
public abstract class BeamController : BraveBehaviour
{
	// Token: 0x170013DC RID: 5084
	// (get) Token: 0x0600843C RID: 33852 RVA: 0x00367960 File Offset: 0x00365B60
	// (set) Token: 0x0600843D RID: 33853 RVA: 0x00367968 File Offset: 0x00365B68
	public GameActor Owner { get; set; }

	// Token: 0x170013DD RID: 5085
	// (get) Token: 0x0600843E RID: 33854 RVA: 0x00367974 File Offset: 0x00365B74
	// (set) Token: 0x0600843F RID: 33855 RVA: 0x0036797C File Offset: 0x00365B7C
	public Gun Gun { get; set; }

	// Token: 0x170013DE RID: 5086
	// (get) Token: 0x06008440 RID: 33856 RVA: 0x00367988 File Offset: 0x00365B88
	// (set) Token: 0x06008441 RID: 33857 RVA: 0x00367990 File Offset: 0x00365B90
	public bool HitsPlayers { get; set; }

	// Token: 0x170013DF RID: 5087
	// (get) Token: 0x06008442 RID: 33858 RVA: 0x0036799C File Offset: 0x00365B9C
	// (set) Token: 0x06008443 RID: 33859 RVA: 0x003679A4 File Offset: 0x00365BA4
	public bool HitsEnemies { get; set; }

	// Token: 0x170013E0 RID: 5088
	// (get) Token: 0x06008444 RID: 33860 RVA: 0x003679B0 File Offset: 0x00365BB0
	// (set) Token: 0x06008445 RID: 33861 RVA: 0x003679B8 File Offset: 0x00365BB8
	public Vector2 Origin { get; set; }

	// Token: 0x170013E1 RID: 5089
	// (get) Token: 0x06008446 RID: 33862 RVA: 0x003679C4 File Offset: 0x00365BC4
	// (set) Token: 0x06008447 RID: 33863 RVA: 0x003679CC File Offset: 0x00365BCC
	public Vector2 Direction { get; set; }

	// Token: 0x170013E2 RID: 5090
	// (get) Token: 0x06008448 RID: 33864 RVA: 0x003679D8 File Offset: 0x00365BD8
	// (set) Token: 0x06008449 RID: 33865 RVA: 0x003679E0 File Offset: 0x00365BE0
	public float DamageModifier { get; set; }

	// Token: 0x170013E3 RID: 5091
	// (get) Token: 0x0600844A RID: 33866
	public abstract bool ShouldUseAmmo { get; }

	// Token: 0x170013E4 RID: 5092
	// (get) Token: 0x0600844B RID: 33867 RVA: 0x003679EC File Offset: 0x00365BEC
	// (set) Token: 0x0600844C RID: 33868 RVA: 0x003679F4 File Offset: 0x00365BF4
	public float ChanceBasedHomingRadius { get; set; }

	// Token: 0x170013E5 RID: 5093
	// (get) Token: 0x0600844D RID: 33869 RVA: 0x00367A00 File Offset: 0x00365C00
	// (set) Token: 0x0600844E RID: 33870 RVA: 0x00367A08 File Offset: 0x00365C08
	public float ChanceBasedHomingAngularVelocity { get; set; }

	// Token: 0x170013E6 RID: 5094
	// (get) Token: 0x0600844F RID: 33871 RVA: 0x00367A14 File Offset: 0x00365C14
	// (set) Token: 0x06008450 RID: 33872 RVA: 0x00367A1C File Offset: 0x00365C1C
	public bool ChanceBasedShadowBullet { get; set; }

	// Token: 0x170013E7 RID: 5095
	// (get) Token: 0x06008451 RID: 33873 RVA: 0x00367A28 File Offset: 0x00365C28
	// (set) Token: 0x06008452 RID: 33874 RVA: 0x00367A30 File Offset: 0x00365C30
	public bool IsReflectedBeam { get; set; }

	// Token: 0x06008453 RID: 33875 RVA: 0x00367A3C File Offset: 0x00365C3C
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x06008454 RID: 33876
	public abstract void LateUpdatePosition(Vector3 origin);

	// Token: 0x06008455 RID: 33877
	public abstract void CeaseAttack();

	// Token: 0x06008456 RID: 33878
	public abstract void DestroyBeam();

	// Token: 0x06008457 RID: 33879
	public abstract void AdjustPlayerBeamTint(Color targetTintColor, int priority, float lerpTime = 0f);

	// Token: 0x06008458 RID: 33880 RVA: 0x00367A44 File Offset: 0x00365C44
	protected bool HandleChanceTick()
	{
		bool flag = false;
		if (this.m_chanceTick <= 0f)
		{
			this.ChanceBasedHomingRadius = 0f;
			this.ChanceBasedHomingAngularVelocity = 0f;
			this.ChanceBasedShadowBullet = false;
			(this.Owner as PlayerController).DoPostProcessBeamChanceTick(this);
			this.m_chanceTick += 1f;
			flag = true;
		}
		this.m_chanceTick -= BraveTime.DeltaTime;
		return flag;
	}

	// Token: 0x06008459 RID: 33881 RVA: 0x00367AB8 File Offset: 0x00365CB8
	protected SpeculativeRigidbody[] GetIgnoreRigidbodies()
	{
		PlayerController playerController = this.Owner as PlayerController;
		int num = this.IgnoreRigidbodes.Count + this.TimedIgnoreRigidbodies.Count;
		if (this.Owner && this.Owner.specRigidbody)
		{
			num++;
		}
		if (playerController && playerController.IsInMinecart)
		{
			num++;
		}
		if (this.m_ignoreRigidbodiesList == null || this.m_ignoreRigidbodiesList.Length != num)
		{
			this.m_ignoreRigidbodiesList = new SpeculativeRigidbody[num];
		}
		int num2 = 0;
		for (int i = 0; i < this.IgnoreRigidbodes.Count; i++)
		{
			this.m_ignoreRigidbodiesList[num2++] = this.IgnoreRigidbodes[i];
		}
		for (int j = 0; j < this.TimedIgnoreRigidbodies.Count; j++)
		{
			this.m_ignoreRigidbodiesList[num2++] = this.TimedIgnoreRigidbodies[j].First;
		}
		if (this.Owner && this.Owner.specRigidbody)
		{
			this.m_ignoreRigidbodiesList[num2++] = this.Owner.specRigidbody;
		}
		if (playerController && playerController.IsInMinecart)
		{
			this.m_ignoreRigidbodiesList[num2++] = playerController.currentMineCart.specRigidbody;
		}
		return this.m_ignoreRigidbodiesList;
	}

	// Token: 0x0600845A RID: 33882 RVA: 0x00367C34 File Offset: 0x00365E34
	public static BeamController FreeFireBeam(Projectile projectileToSpawn, PlayerController source, float targetAngle, float duration, bool skipChargeTime = false)
	{
		GameObject gameObject = SpawnManager.SpawnProjectile(projectileToSpawn.gameObject, source.CenterPosition, Quaternion.identity, true);
		Projectile component = gameObject.GetComponent<Projectile>();
		component.Owner = source;
		BeamController component2 = gameObject.GetComponent<BeamController>();
		if (skipChargeTime)
		{
			component2.chargeDelay = 0f;
			component2.usesChargeDelay = false;
		}
		component2.Owner = source;
		component2.HitsPlayers = false;
		component2.HitsEnemies = true;
		Vector3 vector = BraveMathCollege.DegreesToVector(targetAngle, 1f);
		component2.Direction = vector;
		component2.Origin = source.CenterPosition;
		GameManager.Instance.Dungeon.StartCoroutine(BeamController.HandleFiringBeam(component2, source, targetAngle, duration));
		return component2;
	}

	// Token: 0x0600845B RID: 33883 RVA: 0x00367CE4 File Offset: 0x00365EE4
	private static IEnumerator HandleFiringBeam(BeamController beam, PlayerController source, float targetAngle, float duration)
	{
		float elapsed = 0f;
		yield return null;
		while (elapsed < duration)
		{
			elapsed += BraveTime.DeltaTime;
			beam.Origin = source.CenterPosition;
			beam.LateUpdatePosition(source.CenterPosition);
			yield return null;
		}
		beam.CeaseAttack();
		yield break;
	}

	// Token: 0x040087DB RID: 34779
	public const float c_chanceTick = 1f;

	// Token: 0x040087DC RID: 34780
	[TogglesProperty("knockbackStrength", "Knockback")]
	public bool knocksShooterBack;

	// Token: 0x040087DD RID: 34781
	[HideInInspector]
	public float knockbackStrength = 10f;

	// Token: 0x040087DE RID: 34782
	[TogglesProperty("chargeDelay", "Charge Delay")]
	public bool usesChargeDelay;

	// Token: 0x040087DF RID: 34783
	[HideInInspector]
	public float chargeDelay;

	// Token: 0x040087E0 RID: 34784
	public float statusEffectChance = 1f;

	// Token: 0x040087E1 RID: 34785
	public float statusEffectAccumulateMultiplier = 1f;

	// Token: 0x040087E2 RID: 34786
	[NonSerialized]
	public List<SpeculativeRigidbody> IgnoreRigidbodes = new List<SpeculativeRigidbody>();

	// Token: 0x040087E3 RID: 34787
	[NonSerialized]
	public List<Tuple<SpeculativeRigidbody, float>> TimedIgnoreRigidbodies = new List<Tuple<SpeculativeRigidbody, float>>();

	// Token: 0x040087EF RID: 34799
	private SpeculativeRigidbody[] m_ignoreRigidbodiesList;

	// Token: 0x040087F0 RID: 34800
	protected float m_chanceTick = -1000f;
}
