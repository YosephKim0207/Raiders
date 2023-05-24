using System;
using UnityEngine;

// Token: 0x02001644 RID: 5700
public class ExplosionDebrisLauncher : BraveBehaviour
{
	// Token: 0x0600851B RID: 34075 RVA: 0x0036DEE8 File Offset: 0x0036C0E8
	private void Start()
	{
		if (this.LaunchOnActorDeath)
		{
			if (!this.SpecifyActor)
			{
				this.SpecifyActor = base.aiActor;
			}
			this.SpecifyActor.healthHaver.OnDeath += this.OnDeath;
		}
		if (this.LaunchOnActorPreDeath || this.LaunchOnAnimationEvent)
		{
			if (!this.SpecifyActor)
			{
				this.SpecifyActor = base.aiActor;
			}
			this.SpecifyActor.healthHaver.OnPreDeath += this.OnPreDeath;
		}
		if (this.LaunchOnAnimationEvent)
		{
			if (!this.SpecifyAnimator)
			{
				this.SpecifyAnimator = base.spriteAnimator;
			}
			tk2dSpriteAnimator specifyAnimator = this.SpecifyAnimator;
			specifyAnimator.AnimationEventTriggered = (Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip, int>)Delegate.Combine(specifyAnimator.AnimationEventTriggered, new Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip, int>(this.AnimationEventTriggered));
		}
	}

	// Token: 0x0600851C RID: 34076 RVA: 0x0036DFD4 File Offset: 0x0036C1D4
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x0600851D RID: 34077 RVA: 0x0036DFDC File Offset: 0x0036C1DC
	private void OnPreDeath(Vector2 deathDir)
	{
		this.m_deathDir = deathDir;
		if (this.LaunchOnActorPreDeath)
		{
			this.Launch(deathDir);
		}
	}

	// Token: 0x0600851E RID: 34078 RVA: 0x0036DFF8 File Offset: 0x0036C1F8
	private void OnDeath(Vector2 deathDir)
	{
		this.Launch(deathDir);
	}

	// Token: 0x0600851F RID: 34079 RVA: 0x0036E004 File Offset: 0x0036C204
	private void AnimationEventTriggered(tk2dSpriteAnimator spriteAnimator, tk2dSpriteAnimationClip clip, int frame)
	{
		if (clip.GetFrame(frame).eventInfo == this.EventName)
		{
			Vector2 vector = Vector2.zero;
			if (!this.UseDeathDir)
			{
				vector = base.transform.position.XY() - this.SpecifyAnimator.aiActor.CenterPosition;
			}
			if (this.UseDeathDir || vector == Vector2.zero)
			{
				vector = this.m_deathDir;
			}
			this.Launch(vector);
		}
	}

	// Token: 0x06008520 RID: 34080 RVA: 0x0036E090 File Offset: 0x0036C290
	public void Launch()
	{
		int num = UnityEngine.Random.Range(this.minShards, this.maxShards + 1);
		float num2 = UnityEngine.Random.Range(0f, 360f);
		float num3 = 360f / (float)num;
		for (int i = 0; i < num; i++)
		{
			GameObject gameObject = SpawnManager.SpawnDebris(this.debrisSources[UnityEngine.Random.Range(0, this.debrisSources.Length)].gameObject, base.transform.position, Quaternion.identity);
			DebrisObject component = gameObject.GetComponent<DebrisObject>();
			Vector3 vector = Quaternion.Euler(0f, 0f, num2 + num3 * (float)i + UnityEngine.Random.Range(-this.angleVariance, this.angleVariance)) * Vector3.right * UnityEngine.Random.Range(this.minExpulsionForce, this.maxExpulsionForce);
			vector = vector.WithZ(2f);
			if (this.UsesCustomAxialVelocity)
			{
				vector = Vector3.Scale(vector, this.CustomAxialVelocity);
			}
			component.Trigger(vector, 1f, 1f);
			component.additionalHeightBoost = -3f;
		}
	}

	// Token: 0x06008521 RID: 34081 RVA: 0x0036E1A8 File Offset: 0x0036C3A8
	public void Launch(Vector2 surfaceNormal)
	{
		int num = UnityEngine.Random.Range(this.minShards, this.maxShards + 1);
		if (num == 0)
		{
			return;
		}
		float num2 = surfaceNormal.ToAngle();
		float num3 = 0f;
		if (this.specifyArcDegrees)
		{
			num2 -= this.arcDegrees / 2f;
			num3 = this.arcDegrees / (float)(num - 1);
		}
		else if (num == 2)
		{
			num2 -= 45f;
			num3 = 90f;
		}
		else if (num > 2)
		{
			num2 -= 90f;
			num3 = 180f / (float)(num - 1);
		}
		for (int i = 0; i < num; i++)
		{
			GameObject gameObject = SpawnManager.SpawnDebris(this.debrisSources[UnityEngine.Random.Range(0, this.debrisSources.Length)].gameObject, base.transform.position, Quaternion.identity);
			DebrisObject component = gameObject.GetComponent<DebrisObject>();
			float num4 = num2 + num3 * (float)i + UnityEngine.Random.Range(-this.angleVariance, this.angleVariance);
			num4 = Mathf.Clamp(num4, num2, num2 + 180f);
			Vector3 vector = Quaternion.Euler(0f, 0f, num4) * Vector3.right * UnityEngine.Random.Range(this.minExpulsionForce, this.maxExpulsionForce);
			vector = vector.WithZ(UnityEngine.Random.Range(1.5f, 3f));
			if (this.UsesCustomAxialVelocity)
			{
				vector = Vector3.Scale(vector, this.CustomAxialVelocity);
			}
			component.Trigger(vector, UnityEngine.Random.Range(1f, 2f), 1f);
		}
	}

	// Token: 0x040088FD RID: 35069
	public int minShards = 4;

	// Token: 0x040088FE RID: 35070
	public int maxShards = 4;

	// Token: 0x040088FF RID: 35071
	public float minExpulsionForce = 15f;

	// Token: 0x04008900 RID: 35072
	public float maxExpulsionForce = 15f;

	// Token: 0x04008901 RID: 35073
	public bool specifyArcDegrees;

	// Token: 0x04008902 RID: 35074
	[ShowInInspectorIf("specifyArcDegrees", true)]
	public float arcDegrees;

	// Token: 0x04008903 RID: 35075
	public float angleVariance = 20f;

	// Token: 0x04008904 RID: 35076
	public DebrisObject[] debrisSources;

	// Token: 0x04008905 RID: 35077
	public bool UsesCustomAxialVelocity;

	// Token: 0x04008906 RID: 35078
	[ShowInInspectorIf("UsesCustomAxialVelocity", false)]
	public Vector3 CustomAxialVelocity = Vector3.zero;

	// Token: 0x04008907 RID: 35079
	public AIActor SpecifyActor;

	// Token: 0x04008908 RID: 35080
	public bool LaunchOnActorPreDeath;

	// Token: 0x04008909 RID: 35081
	public bool LaunchOnActorDeath;

	// Token: 0x0400890A RID: 35082
	public bool LaunchOnAnimationEvent;

	// Token: 0x0400890B RID: 35083
	[ShowInInspectorIf("LaunchOnAnimationEvent", true)]
	public tk2dSpriteAnimator SpecifyAnimator;

	// Token: 0x0400890C RID: 35084
	[ShowInInspectorIf("LaunchOnAnimationEvent", true)]
	public string EventName;

	// Token: 0x0400890D RID: 35085
	[ShowInInspectorIf("LaunchOnAnimationEvent", true)]
	public bool UseDeathDir = true;

	// Token: 0x0400890E RID: 35086
	private Vector2 m_deathDir;
}
