using System;
using System.Collections;
using Dungeonator;
using UnityEngine;

// Token: 0x02001216 RID: 4630
public class SmashTentController : MonoBehaviour, IPlaceConfigurable
{
	// Token: 0x0600678F RID: 26511 RVA: 0x002883EC File Offset: 0x002865EC
	public void DoSmash()
	{
		PlayerController talkingPlayer = this.DrGreet.TalkingPlayer;
		this.DrGreet.GetDungeonFSM().Fsm.SuppressGlobalTransitions = true;
		this.DrSmash.GetDungeonFSM().Fsm.SuppressGlobalTransitions = true;
		base.StartCoroutine(this.HandleSmash(talkingPlayer));
	}

	// Token: 0x06006790 RID: 26512 RVA: 0x00288440 File Offset: 0x00286640
	private IEnumerator WaitForPlayerPosition(PlayerController targetPlayer)
	{
		Vector2 targetPosition = this.PlayerStandPoint.PositionVector2();
		Vector2 currentPosition = targetPlayer.CenterPosition;
		targetPlayer.ForceMoveToPoint(targetPosition, 0.5f, 2f);
		float ela = 0f;
		bool doDillywop = false;
		while (!currentPosition.Approximately(targetPosition))
		{
			ela += BraveTime.DeltaTime;
			if (ela > 3f)
			{
				break;
			}
			currentPosition = targetPlayer.CenterPosition;
			yield return null;
			if (!targetPlayer.usingForcedInput || Vector2.Distance(currentPosition, targetPosition) < 0.125f)
			{
				targetPlayer.usingForcedInput = false;
				doDillywop = true;
				break;
			}
		}
		if (ela > 3f || doDillywop)
		{
			Vector2 vector = targetPlayer.transform.position.XY() - targetPlayer.CenterPosition;
			targetPlayer.WarpToPoint(targetPosition + vector, false, false);
		}
		targetPlayer.ForceStaticFaceDirection(Vector2.right);
		yield break;
	}

	// Token: 0x06006791 RID: 26513 RVA: 0x00288464 File Offset: 0x00286664
	private IEnumerator HandleSmash(PlayerController targetPlayer)
	{
		this.IsProcessing = true;
		targetPlayer.SetInputOverride("smash test");
		this.m_targetPlayer = targetPlayer;
		yield return base.StartCoroutine(this.WaitForPlayerPosition(targetPlayer));
		this.DrGreet.aiAnimator.PlayUntilCancelled("drgreet_diagnose", false, null, -1f, false);
		yield return new WaitForSeconds(1f);
		tk2dSpriteAnimator spriteAnimator = this.DrSmash.spriteAnimator;
		spriteAnimator.AnimationEventTriggered = (Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip, int>)Delegate.Combine(spriteAnimator.AnimationEventTriggered, new Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip, int>(this.HandleSmashEvent));
		this.DrSmash.aiAnimator.PlayUntilFinished("drsmash_smash", false, null, -1f, false);
		while (!this.HasSmashed)
		{
			yield return null;
		}
		this.DrGreet.aiAnimator.PlayForDuration("drgreet_cure", 3f, false, null, -1f, false);
		while (this.DrSmash.aiAnimator.IsPlaying("drsmash_smash"))
		{
			yield return null;
		}
		this.DrSmash.aiAnimator.PlayForDuration("drsmash_clap", 3f, false, null, -1f, false);
		tk2dSpriteAnimator spriteAnimator2 = this.DrSmash.spriteAnimator;
		spriteAnimator2.AnimationEventTriggered = (Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip, int>)Delegate.Remove(spriteAnimator2.AnimationEventTriggered, new Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip, int>(this.HandleSmashEvent));
		targetPlayer.ClearInputOverride("smash test");
		this.IsProcessing = false;
		while (this.DrSmash.aiAnimator.IsPlaying("drsmash_clap"))
		{
			yield return null;
		}
		this.DrGreet.GetDungeonFSM().Fsm.SuppressGlobalTransitions = false;
		this.DrSmash.GetDungeonFSM().Fsm.SuppressGlobalTransitions = false;
		this.DrSmash.transform.position = this.DrSmash.transform.position + new Vector3(1.375f, 0f, 0f);
		this.DrSmash.specRigidbody.Reinitialize();
		PhysicsEngine.Instance.RegisterOverlappingGhostCollisionExceptions(this.DrSmash.specRigidbody, null, false);
		yield break;
	}

	// Token: 0x06006792 RID: 26514 RVA: 0x00288488 File Offset: 0x00286688
	private IEnumerator HandleHearts(PlayerController targetPlayer)
	{
		GameUIRoot.Instance.ShowCoreUI(string.Empty);
		float duration = 4.5f;
		int halfHeartsToHeal = Mathf.RoundToInt((targetPlayer.healthHaver.GetMaxHealth() - targetPlayer.healthHaver.GetCurrentHealth()) / 0.5f);
		Debug.Log(halfHeartsToHeal + " to heal!");
		float timeStep = duration / (float)halfHeartsToHeal;
		while (targetPlayer.healthHaver.GetCurrentHealth() < targetPlayer.healthHaver.GetMaxHealth())
		{
			targetPlayer.healthHaver.ApplyHealing(0.5f);
			yield return new WaitForSeconds(timeStep);
		}
		yield break;
	}

	// Token: 0x06006793 RID: 26515 RVA: 0x002884A4 File Offset: 0x002866A4
	private void HandleSmashEvent(tk2dSpriteAnimator arg1, tk2dSpriteAnimationClip arg2, int arg3)
	{
		tk2dSpriteAnimationFrame frame = arg2.GetFrame(arg3);
		if (frame.eventInfo == "grabbottle")
		{
			this.TableBottleSprite.gameObject.SetActive(false);
		}
		else if (frame.eventInfo == "smash")
		{
			UnityEngine.Object.Instantiate<GameObject>(this.BottleSmashVFX, this.DrSmash.transform.position, Quaternion.identity);
			this.m_targetPlayer.PlayFairyEffectOnActor(ResourceCache.Acquire("Global VFX/VFX_Fairy_Fly") as GameObject, Vector3.zero, 4.5f, true);
			base.StartCoroutine(this.HandleHearts(this.m_targetPlayer));
			this.HasSmashed = true;
		}
	}

	// Token: 0x06006794 RID: 26516 RVA: 0x0028855C File Offset: 0x0028675C
	public void ConfigureOnPlacement(RoomHandler room)
	{
		if (room.connectedRooms.Count == 1)
		{
			room.ShouldAttemptProceduralLock = true;
			room.AttemptProceduralLockChance = Mathf.Max(room.AttemptProceduralLockChance, UnityEngine.Random.Range(0.3f, 0.5f));
		}
	}

	// Token: 0x0400636E RID: 25454
	public TalkDoerLite DrGreet;

	// Token: 0x0400636F RID: 25455
	public TalkDoerLite DrSmash;

	// Token: 0x04006370 RID: 25456
	public tk2dBaseSprite TableBottleSprite;

	// Token: 0x04006371 RID: 25457
	public Transform PlayerStandPoint;

	// Token: 0x04006372 RID: 25458
	public GameObject BottleSmashVFX;

	// Token: 0x04006373 RID: 25459
	[NonSerialized]
	public bool IsProcessing;

	// Token: 0x04006374 RID: 25460
	[NonSerialized]
	private bool HasSmashed;

	// Token: 0x04006375 RID: 25461
	private PlayerController m_targetPlayer;
}
