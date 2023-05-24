using System;
using System.Collections.Generic;
using System.Diagnostics;
using Dungeonator;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x0200162A RID: 5674
public class BounceProjModifier : BraveBehaviour
{
	// Token: 0x140000D0 RID: 208
	// (add) Token: 0x06008476 RID: 33910 RVA: 0x00368838 File Offset: 0x00366A38
	// (remove) Token: 0x06008477 RID: 33911 RVA: 0x00368870 File Offset: 0x00366A70
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event Action OnBounce;

	// Token: 0x06008478 RID: 33912 RVA: 0x003688A8 File Offset: 0x00366AA8
	public void OnSpawned()
	{
		this.m_cachedNumberOfBounces = this.numberOfBounces;
		this.m_lastBouncePos = Vector2.zero;
	}

	// Token: 0x06008479 RID: 33913 RVA: 0x003688C4 File Offset: 0x00366AC4
	public void OnDespawned()
	{
		this.numberOfBounces = this.m_cachedNumberOfBounces;
	}

	// Token: 0x0600847A RID: 33914 RVA: 0x003688D4 File Offset: 0x00366AD4
	public Vector2 AdjustBounceVector(Projectile source, Vector2 inVec, SpeculativeRigidbody hitRigidbody)
	{
		Vector2 vector = inVec;
		if (this.bouncesTrackEnemies && UnityEngine.Random.value < this.TrackEnemyChance)
		{
			RoomHandler absoluteRoomFromPosition = GameManager.Instance.Dungeon.data.GetAbsoluteRoomFromPosition(source.specRigidbody.UnitCenter.ToIntVector2(VectorConversions.Round));
			Vector2 unitCenter = source.specRigidbody.UnitCenter;
			Vector2 vector2 = unitCenter + inVec.normalized * 50f;
			List<AIActor> activeEnemies = absoluteRoomFromPosition.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
			float num = this.bounceTrackRadius * this.bounceTrackRadius;
			AIActor aiactor = null;
			float num2 = float.MaxValue;
			if (activeEnemies != null)
			{
				for (int i = 0; i < activeEnemies.Count; i++)
				{
					if (activeEnemies[i])
					{
						if (!(activeEnemies[i] == this.m_lastSmartBounceTarget))
						{
							if (!(activeEnemies[i].specRigidbody == hitRigidbody))
							{
								Vector2 vector3 = BraveMathCollege.ClosestPointOnLineSegment(activeEnemies[i].CenterPosition, unitCenter, vector2);
								float num3 = Vector2.SqrMagnitude(activeEnemies[i].CenterPosition - vector3);
								if (num3 < num && num3 < num2)
								{
									num2 = num3;
									aiactor = activeEnemies[i];
								}
							}
						}
					}
				}
			}
			this.m_lastSmartBounceTarget = aiactor;
			if (aiactor != null)
			{
				Vector2 centerPosition = aiactor.CenterPosition;
				vector = (centerPosition - unitCenter).normalized * inVec.magnitude;
			}
		}
		return vector;
	}

	// Token: 0x0600847B RID: 33915 RVA: 0x00368A74 File Offset: 0x00366C74
	public bool FarFromPreviousBounce(Vector2 pos)
	{
		return Vector2.Distance(pos, this.m_lastBouncePos) > 0.25f;
	}

	// Token: 0x0600847C RID: 33916 RVA: 0x00368A8C File Offset: 0x00366C8C
	public void Bounce(Projectile p, Vector2 bouncePos, SpeculativeRigidbody otherRigidbody = null)
	{
		this.m_lastBouncePos = bouncePos;
		this.Bounce(p, otherRigidbody);
	}

	// Token: 0x0600847D RID: 33917 RVA: 0x00368AA0 File Offset: 0x00366CA0
	public void Bounce(Projectile p, SpeculativeRigidbody otherRigidbody = null)
	{
		if (!this || !p)
		{
			return;
		}
		PierceProjModifier pierceProjModifier = null;
		if (otherRigidbody && otherRigidbody.projectile)
		{
			pierceProjModifier = otherRigidbody.GetComponent<PierceProjModifier>();
		}
		if (pierceProjModifier && pierceProjModifier.BeastModeLevel == PierceProjModifier.BeastModeStatus.BEAST_MODE_LEVEL_ONE)
		{
			this.numberOfBounces -= 2;
		}
		else
		{
			this.numberOfBounces--;
		}
		p.baseData.damage *= this.damageMultiplierOnBounce;
		if (this.usesAdditionalScreenShake)
		{
			GameManager.Instance.MainCameraController.DoScreenShake(this.additionalScreenShake, new Vector2?(p.specRigidbody.UnitCenter), false);
		}
		if (this.numberOfBounces <= 0 && p.TrailRendererController)
		{
			p.TrailRendererController.Stop();
		}
		if (this.OnBounce != null)
		{
			this.OnBounce();
		}
		if (this.OnBounceContext != null)
		{
			this.OnBounceContext(this, otherRigidbody);
		}
	}

	// Token: 0x0600847E RID: 33918 RVA: 0x00368BC0 File Offset: 0x00366DC0
	public void HandleChanceToDie()
	{
		if (this.chanceToDieOnBounce > 0f && UnityEngine.Random.value < this.chanceToDieOnBounce)
		{
			this.numberOfBounces = 0;
		}
	}

	// Token: 0x0400880B RID: 34827
	public int numberOfBounces = 1;

	// Token: 0x0400880C RID: 34828
	public float chanceToDieOnBounce;

	// Token: 0x0400880D RID: 34829
	public float percentVelocityToLoseOnBounce;

	// Token: 0x0400880E RID: 34830
	public float damageMultiplierOnBounce = 1f;

	// Token: 0x0400880F RID: 34831
	public bool usesAdditionalScreenShake;

	// Token: 0x04008810 RID: 34832
	[ShowInInspectorIf("usesAdditionalScreenShake", false)]
	public ScreenShakeSettings additionalScreenShake;

	// Token: 0x04008811 RID: 34833
	public bool useLayerLimit;

	// Token: 0x04008812 RID: 34834
	[ShowInInspectorIf("useLayerLimit", false)]
	public CollisionLayer layerLimit;

	// Token: 0x04008814 RID: 34836
	public Action<BounceProjModifier, SpeculativeRigidbody> OnBounceContext;

	// Token: 0x04008815 RID: 34837
	public bool ExplodeOnEnemyBounce;

	// Token: 0x04008816 RID: 34838
	[FormerlySerializedAs("removeBulletMlControl")]
	public bool removeBulletScriptControl = true;

	// Token: 0x04008817 RID: 34839
	public bool suppressHitEffectsOnBounce;

	// Token: 0x04008818 RID: 34840
	public bool onlyBounceOffTiles;

	// Token: 0x04008819 RID: 34841
	public bool bouncesTrackEnemies;

	// Token: 0x0400881A RID: 34842
	public float bounceTrackRadius = 5f;

	// Token: 0x0400881B RID: 34843
	public float TrackEnemyChance = 1f;

	// Token: 0x0400881C RID: 34844
	private AIActor m_lastSmartBounceTarget;

	// Token: 0x0400881D RID: 34845
	private int m_cachedNumberOfBounces;

	// Token: 0x0400881E RID: 34846
	private Vector2 m_lastBouncePos;
}
