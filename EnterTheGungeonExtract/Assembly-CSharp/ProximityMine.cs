using System;
using System.Collections;
using System.Collections.Generic;
using Dungeonator;
using UnityEngine;

// Token: 0x0200146D RID: 5229
public class ProximityMine : BraveBehaviour
{
	// Token: 0x060076E6 RID: 30438 RVA: 0x002F67C4 File Offset: 0x002F49C4
	private void TransitionToIdle(tk2dSpriteAnimator animator, tk2dSpriteAnimationClip clip)
	{
		if (this.idleAnimName != null && !animator.IsPlaying(this.explodeAnimName))
		{
			animator.Play(this.idleAnimName);
		}
		animator.AnimationCompleted = (Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip>)Delegate.Remove(animator.AnimationCompleted, new Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip>(this.TransitionToIdle));
	}

	// Token: 0x060076E7 RID: 30439 RVA: 0x002F681C File Offset: 0x002F4A1C
	private void Update()
	{
		if (!this.MovesTowardEnemies && this.HomingTriggeredOnSynergy && GameManager.Instance.PrimaryPlayer.HasActiveBonusSynergy(this.TriggerSynergy, false))
		{
			this.MovesTowardEnemies = true;
		}
		if (this.MovesTowardEnemies)
		{
			RoomHandler absoluteRoom = base.transform.position.GetAbsoluteRoom();
			float maxValue = float.MaxValue;
			AIActor nearestEnemy = absoluteRoom.GetNearestEnemy(base.sprite.WorldCenter, out maxValue, true, false);
			if (nearestEnemy && maxValue < this.HomingRadius)
			{
				Vector2 centerPosition = nearestEnemy.CenterPosition;
				Vector2 normalized = (centerPosition - base.sprite.WorldCenter).normalized;
				if (base.debris)
				{
					base.debris.ApplyFrameVelocity(normalized * this.HomingSpeed);
				}
				else
				{
					base.transform.position = base.transform.position + normalized.ToVector3ZisY(0f) * this.HomingSpeed * BraveTime.DeltaTime;
				}
			}
		}
	}

	// Token: 0x060076E8 RID: 30440 RVA: 0x002F693C File Offset: 0x002F4B3C
	private IEnumerator Start()
	{
		if (!string.IsNullOrEmpty(this.deployAnimName))
		{
			base.spriteAnimator.Play(this.deployAnimName);
			tk2dSpriteAnimator spriteAnimator = base.spriteAnimator;
			spriteAnimator.AnimationCompleted = (Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip>)Delegate.Combine(spriteAnimator.AnimationCompleted, new Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip>(this.TransitionToIdle));
		}
		else if (!string.IsNullOrEmpty(this.idleAnimName))
		{
			base.spriteAnimator.Play(this.idleAnimName);
		}
		if (this.explosionStyle == ProximityMine.ExplosiveStyle.PROXIMITY)
		{
			Vector2 position = base.transform.position.XY();
			List<AIActor> allActors = StaticReferenceManager.AllEnemies;
			AkSoundEngine.PostEvent("Play_OBJ_mine_set_01", base.gameObject);
			while (!this.m_triggered)
			{
				if (this.MovesTowardEnemies)
				{
					position = base.transform.position.XY();
				}
				if (!GameManager.Instance.Dungeon.data.GetAbsoluteRoomFromPosition(base.transform.position.IntXY(VectorConversions.Floor)).HasActiveEnemies(RoomHandler.ActiveEnemyType.RoomClear))
				{
					this.m_triggered = true;
					this.m_disarmed = true;
					break;
				}
				bool shouldContinue = false;
				for (int i = 0; i < GameManager.Instance.AllPlayers.Length; i++)
				{
					if (GameManager.Instance.AllPlayers[i] && !GameManager.Instance.AllPlayers[i].IsGhost)
					{
						float num = Vector2.SqrMagnitude(position - GameManager.Instance.AllPlayers[i].specRigidbody.UnitCenter);
						if (num < this.detectRadius * this.detectRadius)
						{
							shouldContinue = true;
							break;
						}
					}
				}
				if (shouldContinue)
				{
					yield return null;
				}
				else
				{
					for (int j = 0; j < allActors.Count; j++)
					{
						AIActor aiactor = allActors[j];
						if (aiactor.IsNormalEnemy)
						{
							if (aiactor.gameObject.activeSelf)
							{
								if (aiactor.HasBeenEngaged)
								{
									if (!aiactor.healthHaver.IsDead)
									{
										float num2 = Vector2.SqrMagnitude(position - aiactor.specRigidbody.UnitCenter);
										if (num2 < this.detectRadius * this.detectRadius)
										{
											this.m_triggered = true;
											break;
										}
									}
								}
							}
						}
					}
					yield return null;
				}
			}
		}
		else if (this.explosionStyle == ProximityMine.ExplosiveStyle.TIMED)
		{
			yield return new WaitForSeconds(this.explosionDelay);
			if (this.MovesTowardEnemies && this.HomingDelay > this.explosionDelay)
			{
				yield return new WaitForSeconds(this.HomingDelay - this.explosionDelay);
			}
		}
		if (!this.m_disarmed)
		{
			if (!string.IsNullOrEmpty(this.explodeAnimName))
			{
				base.spriteAnimator.Play(this.explodeAnimName);
				if (this.usesCustomExplosionDelay)
				{
					yield return new WaitForSeconds(this.customExplosionDelay);
				}
				else
				{
					tk2dSpriteAnimationClip clip = base.spriteAnimator.GetClipByName(this.explodeAnimName);
					yield return new WaitForSeconds((float)clip.frames.Length / clip.fps);
				}
			}
			Exploder.Explode(base.sprite.WorldCenter.ToVector3ZUp(0f), this.explosionData, Vector2.zero, null, false, CoreDamageTypes.None, false);
			UnityEngine.Object.Destroy(base.gameObject);
		}
		else
		{
			base.spriteAnimator.StopAndResetFrame();
		}
		yield break;
	}

	// Token: 0x060076E9 RID: 30441 RVA: 0x002F6958 File Offset: 0x002F4B58
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x040078D3 RID: 30931
	public ExplosionData explosionData;

	// Token: 0x040078D4 RID: 30932
	public ProximityMine.ExplosiveStyle explosionStyle;

	// Token: 0x040078D5 RID: 30933
	[ShowInInspectorIf("explosionStyle", 0, false)]
	public float detectRadius = 2.5f;

	// Token: 0x040078D6 RID: 30934
	public float explosionDelay = 0.3f;

	// Token: 0x040078D7 RID: 30935
	public bool usesCustomExplosionDelay;

	// Token: 0x040078D8 RID: 30936
	[ShowInInspectorIf("usesCustomExplosionDelay", false)]
	public float customExplosionDelay = 0.1f;

	// Token: 0x040078D9 RID: 30937
	[CheckAnimation(null)]
	public string deployAnimName;

	// Token: 0x040078DA RID: 30938
	[CheckAnimation(null)]
	public string idleAnimName;

	// Token: 0x040078DB RID: 30939
	[CheckAnimation(null)]
	public string explodeAnimName;

	// Token: 0x040078DC RID: 30940
	[Header("Homing")]
	public bool MovesTowardEnemies;

	// Token: 0x040078DD RID: 30941
	public bool HomingTriggeredOnSynergy;

	// Token: 0x040078DE RID: 30942
	[LongNumericEnum]
	public CustomSynergyType TriggerSynergy;

	// Token: 0x040078DF RID: 30943
	public float HomingRadius = 5f;

	// Token: 0x040078E0 RID: 30944
	public float HomingSpeed = 3f;

	// Token: 0x040078E1 RID: 30945
	public float HomingDelay = 5f;

	// Token: 0x040078E2 RID: 30946
	protected bool m_triggered;

	// Token: 0x040078E3 RID: 30947
	protected bool m_disarmed;

	// Token: 0x0200146E RID: 5230
	public enum ExplosiveStyle
	{
		// Token: 0x040078E5 RID: 30949
		PROXIMITY,
		// Token: 0x040078E6 RID: 30950
		TIMED
	}
}
