using System;
using System.Collections;
using Dungeonator;
using UnityEngine;

// Token: 0x0200100D RID: 4109
public class DemonWallDeathController : BraveBehaviour
{
	// Token: 0x060059EF RID: 23023 RVA: 0x00225DEC File Offset: 0x00223FEC
	public void Start()
	{
		base.healthHaver.ManualDeathHandling = true;
		base.healthHaver.OnPreDeath += this.OnBossDeath;
		tk2dSpriteAnimator spriteAnimator = base.spriteAnimator;
		spriteAnimator.AnimationEventTriggered = (Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip, int>)Delegate.Combine(spriteAnimator.AnimationEventTriggered, new Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip, int>(this.AnimationEventTriggered));
	}

	// Token: 0x060059F0 RID: 23024 RVA: 0x00225E44 File Offset: 0x00224044
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x060059F1 RID: 23025 RVA: 0x00225E4C File Offset: 0x0022404C
	private void OnBossDeath(Vector2 dir)
	{
		IntVector2 intVector = (base.specRigidbody.HitboxPixelCollider.UnitBottomLeft + new Vector2(0f, -1f)).ToIntVector2(VectorConversions.Floor);
		IntVector2 intVector2 = base.specRigidbody.HitboxPixelCollider.UnitTopRight.ToIntVector2(VectorConversions.Ceil);
		DungeonData data = GameManager.Instance.Dungeon.data;
		for (int i = intVector.x; i <= intVector2.x; i++)
		{
			if (i != (intVector2.x + intVector.x) / 2)
			{
				if (i != (intVector2.x + intVector.x) / 2 - 1)
				{
					for (int j = intVector.y; j <= intVector2.y; j++)
					{
						if (data.CheckInBoundsAndValid(new IntVector2(i, j)))
						{
							CellData cellData = data[i, j];
							if (cellData.type == CellType.FLOOR)
							{
								cellData.isOccupied = true;
							}
						}
					}
				}
			}
		}
		base.aiActor.ParentRoom.OverrideBossPedestalLocation = new IntVector2?(base.specRigidbody.UnitCenter.ToIntVector2(VectorConversions.Round) + new IntVector2(-1, 7));
		base.StartCoroutine(this.OnDeathAnimationCR());
	}

	// Token: 0x060059F2 RID: 23026 RVA: 0x00225F98 File Offset: 0x00224198
	private IEnumerator OnDeathAnimationCR()
	{
		this.m_isDying = true;
		base.aiAnimator.EndAnimation();
		this.deathEyes.SetActive(true);
		tk2dSpriteAnimator deathEyesAnimator = this.deathEyes.GetComponent<tk2dSpriteAnimator>();
		deathEyesAnimator.Play();
		while (deathEyesAnimator.IsPlaying(deathEyesAnimator.DefaultClip))
		{
			yield return null;
		}
		this.deathEyes.SetActive(false);
		base.aiAnimator.PlayUntilCancelled("death", false, null, -1f, false);
		while (base.aiAnimator.IsPlaying("death"))
		{
			yield return null;
		}
		tk2dSpriteAnimator deathOilAnimator = this.deathOil.GetComponent<tk2dSpriteAnimator>();
		while (deathOilAnimator.IsPlaying(deathOilAnimator.DefaultClip))
		{
			yield return null;
		}
		this.deathOil.SetActive(false);
		base.sprite.HeightOffGround = -1.5f;
		base.sprite.UpdateZDepth();
		base.aiActor.StealthDeath = true;
		base.healthHaver.persistsOnDeath = true;
		base.healthHaver.DeathAnimationComplete(null, null);
		for (int i = 0; i < base.specRigidbody.PixelColliders.Count; i++)
		{
			base.specRigidbody.PixelColliders[i].Enabled = !base.specRigidbody.PixelColliders[i].Enabled;
		}
		base.specRigidbody.Reinitialize();
		PhysicsEngine.Instance.RegisterOverlappingGhostCollisionExceptions(base.specRigidbody, null, false);
		base.specRigidbody.CanBePushed = false;
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
		this.m_isDying = false;
		yield break;
	}

	// Token: 0x060059F3 RID: 23027 RVA: 0x00225FB4 File Offset: 0x002241B4
	private void AnimationEventTriggered(tk2dSpriteAnimator spriteAnimator, tk2dSpriteAnimationClip clip, int frame)
	{
		if (this.m_isDying && clip.GetFrame(frame).eventInfo == "oil")
		{
			this.deathOil.SetActive(true);
			tk2dSpriteAnimator component = this.deathOil.GetComponent<tk2dSpriteAnimator>();
			component.Play();
		}
	}

	// Token: 0x0400535A RID: 21338
	public GameObject deathEyes;

	// Token: 0x0400535B RID: 21339
	public GameObject deathOil;

	// Token: 0x0400535C RID: 21340
	private bool m_isDying;
}
