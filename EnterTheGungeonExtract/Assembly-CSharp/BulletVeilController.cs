using System;
using System.Collections;
using Dungeonator;
using UnityEngine;

// Token: 0x02001117 RID: 4375
public class BulletVeilController : BraveBehaviour
{
	// Token: 0x06006084 RID: 24708 RVA: 0x002527A8 File Offset: 0x002509A8
	private IEnumerator Start()
	{
		while (Dungeon.IsGenerating)
		{
			yield return null;
		}
		this.m_parentRoom = GameManager.Instance.Dungeon.data.GetAbsoluteRoomFromPosition(base.transform.position.IntXY(VectorConversions.Floor));
		WarpPointHandler wph = base.GetComponent<WarpPointHandler>();
		WarpPointHandler warpPointHandler = wph;
		warpPointHandler.OnPreWarp = (Func<PlayerController, float>)Delegate.Combine(warpPointHandler.OnPreWarp, new Func<PlayerController, float>(this.OnPreWarp));
		WarpPointHandler warpPointHandler2 = wph;
		warpPointHandler2.OnWarping = (Func<PlayerController, float>)Delegate.Combine(warpPointHandler2.OnWarping, new Func<PlayerController, float>(this.OnWarping));
		WarpPointHandler warpPointHandler3 = wph;
		warpPointHandler3.OnWarpDone = (Func<PlayerController, float>)Delegate.Combine(warpPointHandler3.OnWarpDone, new Func<PlayerController, float>(this.OnWarpDone));
		UnityEngine.Object.Instantiate(BraveResources.Load("temp programmer art/Madness/EndTimes", ".prefab"), new Vector3(-1000f, 0f, 0f), Quaternion.identity);
		yield break;
	}

	// Token: 0x06006085 RID: 24709 RVA: 0x002527C4 File Offset: 0x002509C4
	private float OnPreWarp(PlayerController p)
	{
		if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER)
		{
			PlayerController otherPlayer = GameManager.Instance.GetOtherPlayer(p);
			if (otherPlayer && otherPlayer.IsGhost)
			{
				otherPlayer.ResurrectFromBossKill();
			}
		}
		p.IsOnFire = false;
		p.CurrentFireMeterValue = 0f;
		p.CurrentPoisonMeterValue = 0f;
		if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER)
		{
			PlayerController otherPlayer2 = GameManager.Instance.GetOtherPlayer(p);
			if (otherPlayer2)
			{
				otherPlayer2.IsOnFire = false;
				otherPlayer2.CurrentFireMeterValue = 0f;
				otherPlayer2.CurrentPoisonMeterValue = 0f;
			}
		}
		p.specRigidbody.Velocity = Vector2.zero;
		p.SetInputOverride("bullet veil");
		p.ToggleHandRenderers(false, "bullet veil");
		p.ToggleGunRenderers(false, "bullet veil");
		p.ToggleShadowVisiblity(false);
		p.ForceMoveToPoint(p.CenterPosition + Vector2.up * 3f, 0f, 0.5f);
		if (this.DepartureVFX != null)
		{
			SpawnManager.SpawnVFX(this.DepartureVFX, p.CenterPosition.ToVector3ZisY(0f), Quaternion.identity);
		}
		Minimap.Instance.ToggleMinimap(false, false);
		Minimap.Instance.TemporarilyPreventMinimap = true;
		GameUIRoot.Instance.HideCoreUI(string.Empty);
		GameUIRoot.Instance.ToggleLowerPanels(false, false, string.Empty);
		Pixelator.Instance.FadeToBlack(0.25f, false, 0.25f);
		return 0.5f;
	}

	// Token: 0x06006086 RID: 24710 RVA: 0x00252954 File Offset: 0x00250B54
	private void HandleDoorwayAnimationComplete(tk2dSpriteAnimator anim, tk2dSpriteAnimationClip clip)
	{
		anim.AnimationCompleted = (Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip>)Delegate.Remove(anim.AnimationCompleted, new Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip>(this.HandleDoorwayAnimationComplete));
		anim.transform.parent.GetComponent<PlayerController>().IsVisible = false;
	}

	// Token: 0x06006087 RID: 24711 RVA: 0x00252990 File Offset: 0x00250B90
	private void Update()
	{
		if (!this.m_isOpen && !this.m_hasWarped)
		{
			PlayerController activePlayerClosestToPoint = GameManager.Instance.GetActivePlayerClosestToPoint(base.specRigidbody.UnitCenter, false);
			if (activePlayerClosestToPoint != null && activePlayerClosestToPoint.CurrentRoom == this.m_parentRoom && Vector2.Distance(base.specRigidbody.UnitCenter, activePlayerClosestToPoint.CenterPosition) < 6f)
			{
				this.m_isOpen = true;
				this.VeilAnimator.Play(this.OpenVeilAnimName);
				base.StartCoroutine(this.HandleVeilParticles(false));
			}
		}
		else if (this.m_isOpen && !this.m_hasWarped)
		{
			PlayerController activePlayerClosestToPoint2 = GameManager.Instance.GetActivePlayerClosestToPoint(base.specRigidbody.UnitCenter, false);
			if (activePlayerClosestToPoint2 != null && activePlayerClosestToPoint2.CurrentRoom == this.m_parentRoom && Vector2.Distance(base.specRigidbody.UnitCenter, activePlayerClosestToPoint2.CenterPosition) > 6f)
			{
				this.m_isOpen = false;
				base.StartCoroutine(this.HandleVeilParticles(true));
			}
		}
	}

	// Token: 0x06006088 RID: 24712 RVA: 0x00252AB4 File Offset: 0x00250CB4
	private IEnumerator HandleVeilParticles(bool reverse)
	{
		AkSoundEngine.PostEvent("Play_OBJ_shells_shower_01", base.gameObject);
		float ela = 0f;
		float duration = ((!reverse) ? 0.5f : 1.5f);
		while (ela < duration)
		{
			ela += BraveTime.DeltaTime;
			float t = ela / duration;
			if (reverse)
			{
				t = 1f - t;
			}
			for (int i = 0; i < this.ParticleControllers.Length; i++)
			{
				if (this.ParticleControllers[i])
				{
					this.ParticleControllers[i].LocalYMax = Mathf.Lerp(0f, 2.5f, t);
				}
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06006089 RID: 24713 RVA: 0x00252AD8 File Offset: 0x00250CD8
	private void ActivateEndTimes()
	{
		Minimap.Instance.ToggleMinimap(false, false);
		GameManager.Instance.Dungeon.IsEndTimes = true;
		Minimap.Instance.TemporarilyPreventMinimap = true;
		GameUIRoot.Instance.HideCoreUI(string.Empty);
		GameUIRoot.Instance.ToggleLowerPanels(false, false, string.Empty);
		for (int i = 0; i < GameManager.Instance.AllPlayers.Length; i++)
		{
			GameManager.Instance.AllPlayers[i].CurrentInputState = PlayerInputState.FoyerInputOnly;
		}
		EndTimesNebulaController endTimesNebulaController = UnityEngine.Object.FindObjectOfType<EndTimesNebulaController>();
		endTimesNebulaController.BecomeActive();
		Pixelator.Instance.DoOcclusionLayer = false;
	}

	// Token: 0x0600608A RID: 24714 RVA: 0x00252B74 File Offset: 0x00250D74
	private float OnWarping(PlayerController player)
	{
		player.ClearInputOverride("bullet veil");
		player.ToggleShadowVisiblity(true);
		player.ToggleHandRenderers(true, "bullet veil");
		player.ToggleGunRenderers(true, "bullet veil");
		player.IsVisible = true;
		base.GetComponent<WarpPointHandler>().OnWarping = null;
		this.ActivateEndTimes();
		TimeTubeCreditsController.AcquireTunnelInstanceInAdvance();
		GameManager.Instance.DungeonMusicController.SwitchToEndTimesMusic();
		return 0.5f;
	}

	// Token: 0x0600608B RID: 24715 RVA: 0x00252BE0 File Offset: 0x00250DE0
	private float OnWarpDone(PlayerController player)
	{
		AkSoundEngine.PostEvent("State_ENV_Dimension_01", base.gameObject);
		Pixelator.Instance.FadeToBlack(0.25f, true, 0.1f);
		base.GetComponent<WarpPointHandler>().OnWarpDone = null;
		for (int i = 0; i < GameManager.Instance.AllPlayers.Length; i++)
		{
			GameManager.Instance.AllPlayers[i].DoSpinfallSpawn(0.1f * (float)(i + 1));
		}
		if (this.ArrivalVFX != null)
		{
			SpawnManager.SpawnVFX(this.ArrivalVFX, player.CenterPosition.ToVector3ZisY(0f), Quaternion.identity);
		}
		return 0.4f;
	}

	// Token: 0x0600608C RID: 24716 RVA: 0x00252C90 File Offset: 0x00250E90
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x04005B2B RID: 23339
	public tk2dSpriteAnimator VeilAnimator;

	// Token: 0x04005B2C RID: 23340
	public string OpenVeilAnimName;

	// Token: 0x04005B2D RID: 23341
	public string CloseVeilAnimName;

	// Token: 0x04005B2E RID: 23342
	public BulletCurtainParticleController[] ParticleControllers;

	// Token: 0x04005B2F RID: 23343
	public GameObject DepartureVFX;

	// Token: 0x04005B30 RID: 23344
	public GameObject ArrivalVFX;

	// Token: 0x04005B31 RID: 23345
	private bool m_isOpen;

	// Token: 0x04005B32 RID: 23346
	private bool m_hasWarped;

	// Token: 0x04005B33 RID: 23347
	private RoomHandler m_parentRoom;
}
