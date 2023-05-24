using System;
using System.Collections;
using Dungeonator;
using UnityEngine;

// Token: 0x020010B8 RID: 4280
public class PotFairyEngageDoer : CustomEngageDoer
{
	// Token: 0x06005E58 RID: 24152 RVA: 0x00242BA0 File Offset: 0x00240DA0
	public void Awake()
	{
		if (BraveUtility.RandomBool())
		{
			base.aiAnimator.IdleAnimation.Prefix = base.aiAnimator.IdleAnimation.Prefix.Replace("pink", "blue");
			for (int i = 0; i < base.aiAnimator.OtherAnimations.Count; i++)
			{
				base.aiAnimator.OtherAnimations[i].anim.Prefix = base.aiAnimator.OtherAnimations[i].anim.Prefix.Replace("pink", "blue");
			}
		}
		if (PotFairyEngageDoer.InstantSpawn)
		{
			this.StartIntro();
		}
	}

	// Token: 0x06005E59 RID: 24153 RVA: 0x00242C5C File Offset: 0x00240E5C
	public void Update()
	{
		if (!this.m_hasDonePotCheck)
		{
			if (!PotFairyEngageDoer.InstantSpawn && !base.aiActor.IsInReinforcementLayer)
			{
				base.specRigidbody.Initialize();
				IntVector2 intVector = base.specRigidbody.UnitCenter.ToIntVector2(VectorConversions.Floor);
				RoomHandler roomFromPosition = GameManager.Instance.Dungeon.GetRoomFromPosition(intVector);
				GameObject gameObject = DungeonPlaceableUtility.InstantiateDungeonPlaceable(BraveUtility.RandomElement<GameObject>(this.PotPrefabs), roomFromPosition, intVector - roomFromPosition.area.basePosition, true, AIActor.AwakenAnimationType.Default, false);
				this.m_minorBreakable = gameObject.GetComponent<MinorBreakable>();
			}
			this.m_hasDonePotCheck = true;
		}
		if (base.specRigidbody && base.specRigidbody.enabled)
		{
			RoomHandler roomFromPosition2 = GameManager.Instance.Dungeon.GetRoomFromPosition(base.specRigidbody.UnitCenter.ToIntVector2(VectorConversions.Floor));
			foreach (PlayerController playerController in GameManager.Instance.AllPlayers)
			{
				if (playerController.healthHaver.IsAlive)
				{
					if (playerController.CurrentRoom != null && playerController.CurrentRoom.IsSealed && playerController.CurrentRoom != roomFromPosition2)
					{
						base.aiActor.CanDropCurrency = false;
						base.aiActor.CanDropItems = false;
						base.healthHaver.ApplyDamage(10000f, Vector2.zero, "Lonely Suicide", CoreDamageTypes.None, DamageCategory.Unstoppable, true, null, false);
						break;
					}
				}
			}
		}
	}

	// Token: 0x06005E5A RID: 24154 RVA: 0x00242DE0 File Offset: 0x00240FE0
	public override void StartIntro()
	{
		if (this.m_isFinished)
		{
			return;
		}
		base.StartCoroutine(this.DoIntro());
	}

	// Token: 0x06005E5B RID: 24155 RVA: 0x00242DFC File Offset: 0x00240FFC
	private IEnumerator DoIntro()
	{
		base.aiActor.enabled = false;
		base.behaviorSpeculator.enabled = false;
		base.aiActor.ToggleRenderers(false);
		base.specRigidbody.enabled = false;
		base.aiActor.IgnoreForRoomClear = true;
		base.aiActor.IsGone = true;
		base.aiActor.ToggleRenderers(false);
		if (base.aiShooter)
		{
			base.aiShooter.ToggleGunAndHandRenderers(false, "PotFairyEngageDoer");
		}
		if (this.m_minorBreakable)
		{
			yield return null;
		}
		while (this.m_minorBreakable && !this.m_minorBreakable.IsBroken)
		{
			if (!base.aiActor.ParentRoom.IsSealed || base.healthHaver.IsDead)
			{
				UnityEngine.Object.Destroy(base.gameObject);
				yield break;
			}
			base.aiActor.ToggleRenderers(false);
			if (base.aiShooter)
			{
				base.aiShooter.ToggleGunAndHandRenderers(false, "PotFairyEngageDoer");
			}
			yield return null;
		}
		base.aiActor.enabled = true;
		base.behaviorSpeculator.enabled = true;
		base.specRigidbody.enabled = true;
		base.aiActor.IsGone = false;
		base.aiActor.IgnoreForRoomClear = false;
		base.aiActor.ToggleRenderers(true);
		base.aiAnimator.PlayDefaultAwakenedState();
		base.aiActor.State = AIActor.ActorState.Normal;
		this.m_isFinished = true;
		int playerMask = CollisionMask.LayerToMask(CollisionLayer.PlayerCollider, CollisionLayer.PlayerHitBox);
		base.aiActor.specRigidbody.AddCollisionLayerIgnoreOverride(playerMask);
		while (base.aiAnimator.IsPlaying("awaken"))
		{
			if (base.aiShooter)
			{
				base.aiShooter.ToggleGunAndHandRenderers(false, "PotFairyEngageDoer");
			}
			yield return null;
		}
		if (base.aiShooter)
		{
			base.aiShooter.ToggleGunAndHandRenderers(true, "PotFairyEngageDoer");
		}
		yield return new WaitForSeconds(0.5f);
		base.aiActor.specRigidbody.RemoveCollisionLayerIgnoreOverride(playerMask);
		yield break;
	}

	// Token: 0x17000DE1 RID: 3553
	// (get) Token: 0x06005E5C RID: 24156 RVA: 0x00242E18 File Offset: 0x00241018
	public override bool IsFinished
	{
		get
		{
			return this.m_isFinished;
		}
	}

	// Token: 0x04005867 RID: 22631
	public static bool InstantSpawn;

	// Token: 0x04005868 RID: 22632
	public GameObject[] PotPrefabs;

	// Token: 0x04005869 RID: 22633
	private bool m_isFinished;

	// Token: 0x0400586A RID: 22634
	private MinorBreakable m_minorBreakable;

	// Token: 0x0400586B RID: 22635
	private bool m_hasDonePotCheck;
}
