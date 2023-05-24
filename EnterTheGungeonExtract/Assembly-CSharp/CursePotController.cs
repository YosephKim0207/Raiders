using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001139 RID: 4409
public class CursePotController : BraveBehaviour
{
	// Token: 0x0600616A RID: 24938 RVA: 0x002585B4 File Offset: 0x002567B4
	private void Start()
	{
		this.m_idleClip = base.spriteAnimator.GetClipByName(this.IdleAnim);
		this.m_activeClip = base.spriteAnimator.GetClipByName(this.AnimToPlayWhenExcited);
		SpeculativeRigidbody specRigidbody = base.specRigidbody;
		specRigidbody.OnEnterTrigger = (SpeculativeRigidbody.OnTriggerDelegate)Delegate.Combine(specRigidbody.OnEnterTrigger, new SpeculativeRigidbody.OnTriggerDelegate(this.HandleTriggerEntered));
		SpeculativeRigidbody specRigidbody2 = base.specRigidbody;
		specRigidbody2.OnExitTrigger = (SpeculativeRigidbody.OnTriggerExitDelegate)Delegate.Combine(specRigidbody2.OnExitTrigger, new SpeculativeRigidbody.OnTriggerExitDelegate(this.HandleTriggerExited));
		MinorBreakable component = base.GetComponent<MinorBreakable>();
		component.OnBreak = (Action)Delegate.Combine(component.OnBreak, new Action(this.HandleBreak));
		if (this.CircleSprite)
		{
			tk2dSpriteDefinition currentSpriteDef = this.CircleSprite.GetCurrentSpriteDef();
			Vector2 vector = new Vector2(float.MaxValue, float.MaxValue);
			Vector2 vector2 = new Vector2(float.MinValue, float.MinValue);
			for (int i = 0; i < currentSpriteDef.uvs.Length; i++)
			{
				vector = Vector2.Min(vector, currentSpriteDef.uvs[i]);
				vector2 = Vector2.Max(vector2, currentSpriteDef.uvs[i]);
			}
			Vector2 vector3 = (vector + vector2) / 2f;
			this.CircleSprite.renderer.material.SetVector("_WorldCenter", new Vector4(vector3.x, vector3.y, vector3.x - vector.x, vector3.y - vector.y));
		}
	}

	// Token: 0x0600616B RID: 24939 RVA: 0x0025874C File Offset: 0x0025694C
	private void HandleBreak()
	{
		if (this.CircleSprite)
		{
			this.CircleSprite.transform.parent = null;
		}
		if (this.ShadowSprite)
		{
			this.ShadowSprite.transform.parent = null;
		}
		GameManager.Instance.Dungeon.StartCoroutine(this.HandleBreakCR());
	}

	// Token: 0x0600616C RID: 24940 RVA: 0x002587B4 File Offset: 0x002569B4
	private IEnumerator HandleBreakCR()
	{
		float elapsed = 0f;
		float duration = 0.3f;
		while (elapsed < duration)
		{
			elapsed += BraveTime.DeltaTime;
			float t = Mathf.SmoothStep(0f, 1f, elapsed / duration);
			if (this.CircleSprite)
			{
				this.CircleSprite.scale = Vector3.Lerp(Vector3.one, Vector3.zero, t);
			}
			if (this.ShadowSprite)
			{
				this.ShadowSprite.scale = Vector3.Lerp(Vector3.one, Vector3.zero, t);
			}
			yield return null;
		}
		if (this.CircleSprite)
		{
			UnityEngine.Object.Destroy(this.CircleSprite.gameObject);
		}
		if (this.ShadowSprite)
		{
			UnityEngine.Object.Destroy(this.ShadowSprite.gameObject);
		}
		yield break;
	}

	// Token: 0x0600616D RID: 24941 RVA: 0x002587D0 File Offset: 0x002569D0
	private void Update()
	{
		if (base.minorBreakable.IsBroken)
		{
			return;
		}
		if (this.m_cursedPlayers.Count > 0)
		{
			if (!base.spriteAnimator.IsPlaying(this.m_activeClip))
			{
				base.spriteAnimator.Play(this.m_activeClip);
			}
		}
		else if (!base.spriteAnimator.IsPlaying(this.m_idleClip))
		{
			base.spriteAnimator.Play(this.m_idleClip);
		}
		for (int i = 0; i < this.m_cursedPlayers.Count; i++)
		{
			this.DoCurse(this.m_cursedPlayers[i]);
		}
	}

	// Token: 0x0600616E RID: 24942 RVA: 0x00258880 File Offset: 0x00256A80
	private void DoCurse(PlayerController targetPlayer)
	{
		if (targetPlayer.IsGhost)
		{
			return;
		}
		targetPlayer.CurrentCurseMeterValue += BraveTime.DeltaTime / this.TimeToCursePoint;
		targetPlayer.CurseIsDecaying = false;
		if (targetPlayer.CurrentCurseMeterValue > 1f)
		{
			targetPlayer.CurrentCurseMeterValue = 0f;
			StatModifier statModifier = new StatModifier();
			statModifier.amount = 1f;
			statModifier.modifyType = StatModifier.ModifyMethod.ADDITIVE;
			statModifier.statToBoost = PlayerStats.StatType.Curse;
			targetPlayer.ownerlessStatModifiers.Add(statModifier);
			targetPlayer.stats.RecalculateStats(targetPlayer, false, false);
			base.minorBreakable.Break();
		}
	}

	// Token: 0x0600616F RID: 24943 RVA: 0x0025891C File Offset: 0x00256B1C
	private void HandleTriggerExited(SpeculativeRigidbody exitRigidbody, SpeculativeRigidbody sourceSpecRigidbody)
	{
		if (exitRigidbody && exitRigidbody.gameActor && exitRigidbody.gameActor is PlayerController && this.m_cursedPlayers.Contains(exitRigidbody.gameActor as PlayerController))
		{
			PlayerController playerController = exitRigidbody.gameActor as PlayerController;
			playerController.CurseIsDecaying = true;
			this.m_cursedPlayers.Remove(playerController);
		}
	}

	// Token: 0x06006170 RID: 24944 RVA: 0x00258990 File Offset: 0x00256B90
	private void HandleTriggerEntered(SpeculativeRigidbody enteredRigidbody, SpeculativeRigidbody sourceSpecRigidbody, CollisionData collisionData)
	{
		if (GameManager.Instance.Dungeon.data.GetAbsoluteRoomFromPosition(base.specRigidbody.UnitCenter.ToIntVector2(VectorConversions.Round)) != GameManager.Instance.Dungeon.data.GetAbsoluteRoomFromPosition(enteredRigidbody.UnitCenter.ToIntVector2(VectorConversions.Round)))
		{
			return;
		}
		if (enteredRigidbody.gameActor != null && enteredRigidbody.gameActor is PlayerController)
		{
			PlayerController playerController = enteredRigidbody.gameActor as PlayerController;
			playerController.CurseIsDecaying = false;
			this.m_cursedPlayers.Add(playerController);
		}
	}

	// Token: 0x04005C29 RID: 23593
	public string IdleAnim;

	// Token: 0x04005C2A RID: 23594
	public string AnimToPlayWhenExcited;

	// Token: 0x04005C2B RID: 23595
	public float TimeToCursePoint = 3f;

	// Token: 0x04005C2C RID: 23596
	public tk2dSprite CircleSprite;

	// Token: 0x04005C2D RID: 23597
	public tk2dSprite ShadowSprite;

	// Token: 0x04005C2E RID: 23598
	private tk2dSpriteAnimationClip m_idleClip;

	// Token: 0x04005C2F RID: 23599
	private tk2dSpriteAnimationClip m_activeClip;

	// Token: 0x04005C30 RID: 23600
	private List<PlayerController> m_cursedPlayers = new List<PlayerController>();
}
