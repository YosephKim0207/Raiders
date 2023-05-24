using System;
using System.Collections;
using System.Collections.Generic;
using Dungeonator;
using UnityEngine;

// Token: 0x02000FD5 RID: 4053
[RequireComponent(typeof(GenericIntroDoer))]
public class BossDoorMimicIntroDoer : SpecificIntroDoer
{
	// Token: 0x17000CA8 RID: 3240
	// (get) Token: 0x06005864 RID: 22628 RVA: 0x0021C914 File Offset: 0x0021AB14
	// (set) Token: 0x06005865 RID: 22629 RVA: 0x0021C91C File Offset: 0x0021AB1C
	public DungeonDoorSubsidiaryBlocker PhantomDoorBlocker { get; set; }

	// Token: 0x06005866 RID: 22630 RVA: 0x0021C928 File Offset: 0x0021AB28
	protected override void OnDestroy()
	{
		if (GameManager.HasInstance)
		{
			if (this.PhantomDoorBlocker)
			{
				this.PhantomDoorBlocker.Unseal();
			}
			if (this.m_bossDoor)
			{
				tk2dBaseSprite sprite = this.m_bossDoor.sealAnimators[0].sprite;
				foreach (Renderer renderer in sprite.GetComponentsInChildren<Renderer>())
				{
					renderer.enabled = true;
				}
			}
		}
		base.OnDestroy();
	}

	// Token: 0x17000CA9 RID: 3241
	// (get) Token: 0x06005867 RID: 22631 RVA: 0x0021C9AC File Offset: 0x0021ABAC
	public override Vector2? OverrideIntroPosition
	{
		get
		{
			return new Vector2?(this.m_bossDoor.transform.position);
		}
	}

	// Token: 0x06005868 RID: 22632 RVA: 0x0021C9C8 File Offset: 0x0021ABC8
	public override void PlayerWalkedIn(PlayerController player, List<tk2dSpriteAnimator> animators)
	{
		if (this.m_bossDoor != null)
		{
			return;
		}
		this.m_bossDoor = null;
		float num = float.MaxValue;
		foreach (DungeonDoorController dungeonDoorController in UnityEngine.Object.FindObjectsOfType<DungeonDoorController>())
		{
			if (dungeonDoorController.name.StartsWith("GungeonBossDoor"))
			{
				SpeculativeRigidbody componentInChildren = dungeonDoorController.GetComponentInChildren<SpeculativeRigidbody>();
				float num2 = Vector2.Distance(player.specRigidbody.UnitCenter, componentInChildren.UnitCenter);
				if (this.m_bossDoor == null || num2 < num)
				{
					this.m_bossDoor = dungeonDoorController;
					num = num2;
				}
			}
		}
		foreach (tk2dSpriteAnimator tk2dSpriteAnimator in this.m_bossDoor.GetComponentsInChildren<tk2dSpriteAnimator>())
		{
			if (tk2dSpriteAnimator.name == "Eye Fire")
			{
				animators.Add(tk2dSpriteAnimator);
			}
		}
		this.m_enteringPlayer = player;
		this.m_bossStartingPosition = base.transform.position;
		this.m_cachedHeightOffGround = base.sprite.HeightOffGround;
	}

	// Token: 0x06005869 RID: 22633 RVA: 0x0021CAEC File Offset: 0x0021ACEC
	public override void StartIntro(List<tk2dSpriteAnimator> animators)
	{
		foreach (tk2dSpriteAnimator tk2dSpriteAnimator in base.GetComponentsInChildren<tk2dSpriteAnimator>(true))
		{
			if (tk2dSpriteAnimator != base.spriteAnimator)
			{
				animators.Add(tk2dSpriteAnimator);
			}
		}
		base.StartCoroutine(this.DoIntro());
	}

	// Token: 0x17000CAA RID: 3242
	// (get) Token: 0x0600586A RID: 22634 RVA: 0x0021CB40 File Offset: 0x0021AD40
	public override bool IsIntroFinished
	{
		get
		{
			return this.m_finished;
		}
	}

	// Token: 0x0600586B RID: 22635 RVA: 0x0021CB48 File Offset: 0x0021AD48
	public override void EndIntro()
	{
		base.StopAllCoroutines();
		tk2dBaseSprite sprite = this.m_bossDoor.sealAnimators[0].sprite;
		foreach (Renderer renderer in sprite.GetComponentsInChildren<Renderer>())
		{
			renderer.enabled = false;
		}
		base.transform.position = this.m_bossStartingPosition;
		base.specRigidbody.Reinitialize();
		tk2dBaseSprite component = base.aiActor.ShadowObject.GetComponent<tk2dBaseSprite>();
		component.color = component.color.WithAlpha(1f);
		base.aiAnimator.LockFacingDirection = false;
		base.aiAnimator.FacingDirection = 90f;
		base.aiAnimator.EndAnimation();
		base.sprite.HeightOffGround = this.m_cachedHeightOffGround;
		base.sprite.UpdateZDepth();
		this.SpawnDoorBlocker();
	}

	// Token: 0x0600586C RID: 22636 RVA: 0x0021CC2C File Offset: 0x0021AE2C
	private IEnumerator DoIntro()
	{
		base.specRigidbody.Initialize();
		Vector3 offset = base.specRigidbody.UnitBottomLeft - base.transform.position.XY();
		Vector2 goalPosition = base.specRigidbody.UnitCenter;
		float startingDirection = 0f;
		float playerDirection = 0f;
		Vector2 majorAxisToPlayer = this.m_enteringPlayer.specRigidbody.UnitCenter - base.specRigidbody.UnitCenter;
		if (majorAxisToPlayer.y < -0.5f)
		{
			startingDirection = 270f;
			playerDirection = 90f;
		}
		else if (majorAxisToPlayer.x > 0.5f)
		{
			startingDirection = 360f;
			playerDirection = 180f;
			base.sprite.HeightOffGround += 2f;
			base.sprite.UpdateZDepth();
		}
		else if (majorAxisToPlayer.x < -0.5f)
		{
			startingDirection = 180f;
			playerDirection = 360f;
			base.sprite.HeightOffGround += 2f;
			base.sprite.UpdateZDepth();
		}
		else
		{
			Debug.LogError("UNSUPPORTED BOSS DOOR MIMIC ENTER DIRECTION!");
		}
		tk2dBaseSprite doorSprite = this.m_bossDoor.sealAnimators[0].sprite;
		base.transform.position = doorSprite.transform.position - offset;
		base.aiAnimator.LockFacingDirection = true;
		base.aiAnimator.FacingDirection = startingDirection;
		base.aiAnimator.Update();
		tk2dBaseSprite shadowSprite = base.aiActor.ShadowObject.GetComponent<tk2dBaseSprite>();
		shadowSprite.color = shadowSprite.color.WithAlpha(0f);
		foreach (Renderer renderer in doorSprite.GetComponentsInChildren<Renderer>())
		{
			renderer.enabled = false;
		}
		float elapsed = 0f;
		float duration = 1f;
		while (elapsed < duration)
		{
			yield return null;
			elapsed += GameManager.INVARIANT_DELTA_TIME;
		}
		elapsed = 0f;
		duration = BraveMathCollege.AbsAngleBetween(playerDirection, startingDirection) / 360f * 2f;
		while (elapsed < duration)
		{
			yield return null;
			elapsed += GameManager.INVARIANT_DELTA_TIME;
			base.aiAnimator.FacingDirection = Mathf.Lerp(startingDirection, playerDirection, elapsed / duration);
			base.aiAnimator.Update();
		}
		elapsed = 0f;
		duration = 1f;
		while (elapsed < duration)
		{
			yield return null;
			elapsed += GameManager.INVARIANT_DELTA_TIME;
		}
		base.aiAnimator.PlayUntilCancelled("teleport_out", false, null, -1f, false);
		while (base.aiAnimator.IsPlaying("teleport_out"))
		{
			yield return null;
		}
		base.aiAnimator.renderer.enabled = false;
		base.specRigidbody.Reinitialize();
		Vector2 offsetPosition = base.specRigidbody.UnitCenter - base.transform.position.XY();
		CameraController cameraController = GameManager.Instance.MainCameraController;
		Vector2 cameraStartPosition = cameraController.transform.position;
		elapsed = 0f;
		duration = 2f;
		while (elapsed < duration)
		{
			yield return null;
			elapsed += GameManager.INVARIANT_DELTA_TIME;
			cameraController.OverridePosition = Vector2.Lerp(cameraStartPosition, goalPosition, Mathf.SmoothStep(0f, 1f, elapsed / duration));
		}
		base.transform.position = goalPosition - offsetPosition;
		base.aiAnimator.renderer.enabled = true;
		base.aiAnimator.FacingDirection = -90f;
		base.aiAnimator.PlayUntilCancelled("teleport_in", false, null, -1f, false);
		base.aiAnimator.Update();
		while (base.aiAnimator.IsPlaying("teleport_in"))
		{
			yield return null;
			shadowSprite.color = shadowSprite.color.WithAlpha(base.aiAnimator.CurrentClipProgress);
		}
		base.aiAnimator.PlayUntilCancelled("intro", false, null, -1f, false);
		elapsed = 0f;
		duration = 2f;
		while (elapsed < duration)
		{
			yield return null;
			elapsed += GameManager.INVARIANT_DELTA_TIME;
		}
		base.aiAnimator.EndAnimation();
		base.aiAnimator.Update();
		this.m_finished = true;
		yield break;
	}

	// Token: 0x0600586D RID: 22637 RVA: 0x0021CC48 File Offset: 0x0021AE48
	private void SpawnDoorBlocker()
	{
		if (GameManager.Instance.Dungeon.phantomBlockerDoorObjects == null || this.PhantomDoorBlocker)
		{
			return;
		}
		DungeonData.Direction direction = DungeonData.Direction.NORTH;
		IntVector2 intVector = new IntVector2(22, -5);
		GameObject gameObject = GameManager.Instance.Dungeon.phantomBlockerDoorObjects.InstantiateObjectDirectional(base.aiActor.ParentRoom, intVector, direction);
		this.PhantomDoorBlocker = gameObject.GetComponent<DungeonDoorSubsidiaryBlocker>();
		this.PhantomDoorBlocker.Seal();
	}

	// Token: 0x0400518F RID: 20879
	private bool m_finished;

	// Token: 0x04005190 RID: 20880
	private PlayerController m_enteringPlayer;

	// Token: 0x04005191 RID: 20881
	private DungeonDoorController m_bossDoor;

	// Token: 0x04005192 RID: 20882
	private Vector2 m_bossStartingPosition;

	// Token: 0x04005193 RID: 20883
	private float m_cachedHeightOffGround;
}
