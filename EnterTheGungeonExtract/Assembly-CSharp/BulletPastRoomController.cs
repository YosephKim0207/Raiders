using System;
using System.Collections;
using System.Collections.Generic;
using Dungeonator;
using HutongGames.PlayMaker;
using UnityEngine;

// Token: 0x0200110D RID: 4365
public class BulletPastRoomController : MonoBehaviour
{
	// Token: 0x06006041 RID: 24641 RVA: 0x0025110C File Offset: 0x0024F30C
	private IEnumerator Start()
	{
		while (Dungeon.IsGenerating)
		{
			yield return null;
		}
		if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER && GameManager.Instance.SecondaryPlayer.CurrentGun)
		{
			GameManager.Instance.SecondaryPlayer.inventory.DestroyCurrentGun();
		}
		if (this.RoomIdentifier == BulletPastRoomController.BulletRoomCategory.ROOM_A)
		{
			BulletPastRoomController[] componentsInChildren = base.transform.root.GetComponentsInChildren<BulletPastRoomController>(true);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				if (componentsInChildren[i].RoomIdentifier == BulletPastRoomController.BulletRoomCategory.ROOM_B)
				{
					this.RoomB = componentsInChildren[i];
				}
				if (componentsInChildren[i].RoomIdentifier == BulletPastRoomController.BulletRoomCategory.ROOM_C)
				{
					this.RoomC = componentsInChildren[i];
				}
				if (componentsInChildren[i].RoomIdentifier == BulletPastRoomController.BulletRoomCategory.ROOM_D)
				{
					this.RoomD = componentsInChildren[i];
				}
			}
			this.RoomB.ExitWarp.DISABLED_TEMPORARILY = true;
			RoomHandler absoluteRoom = this.RoomC.transform.position.GetAbsoluteRoom();
			absoluteRoom.TargetPitfallRoom = this.RoomD.transform.position.GetAbsoluteRoom();
			RoomHandler roomHandler = absoluteRoom;
			roomHandler.OnTargetPitfallRoom = (Action)Delegate.Combine(roomHandler.OnTargetPitfallRoom, new Action(this.PitfallIntoGunonRoom));
			if (this.OldBulletTalkTrigger)
			{
				RoomHandler absoluteRoom2 = base.transform.position.GetAbsoluteRoom();
				if (absoluteRoom2 != null)
				{
					List<TalkDoerLite> componentsInRoom = absoluteRoom2.GetComponentsInRoom<TalkDoerLite>();
					if (componentsInRoom.Count > 0)
					{
						this.OldBulletTalkDoer = componentsInRoom[0];
					}
				}
				SpeculativeRigidbody oldBulletTalkTrigger = this.OldBulletTalkTrigger;
				oldBulletTalkTrigger.OnEnterTrigger = (SpeculativeRigidbody.OnTriggerDelegate)Delegate.Combine(oldBulletTalkTrigger.OnEnterTrigger, new SpeculativeRigidbody.OnTriggerDelegate(this.WalkedByOldBullet));
			}
		}
		if (this.RoomIdentifier == BulletPastRoomController.BulletRoomCategory.ROOM_B)
		{
			RoomHandler absoluteRoom3 = base.transform.position.GetAbsoluteRoom();
			absoluteRoom3.Entered += this.PlayerEntered;
			RoomHandler roomHandler2 = absoluteRoom3;
			roomHandler2.OnEnemiesCleared = (Action)Delegate.Combine(roomHandler2.OnEnemiesCleared, new Action(this.HandleRoomBCleared));
			this.ThroneRoomDoorTrigger.enabled = false;
			SpeculativeRigidbody throneRoomDoorTrigger = this.ThroneRoomDoorTrigger;
			throneRoomDoorTrigger.OnEnterTrigger = (SpeculativeRigidbody.OnTriggerDelegate)Delegate.Combine(throneRoomDoorTrigger.OnEnterTrigger, new SpeculativeRigidbody.OnTriggerDelegate(this.EnteredThroneDoorTrigger));
		}
		if (this.RoomIdentifier == BulletPastRoomController.BulletRoomCategory.ROOM_C)
		{
			this.m_agunimFloorBasePosition = this.AgunimFloorChunk.transform.position.IntXY(VectorConversions.Round) + new IntVector2(2, 2);
			for (int j = 0; j < 4; j++)
			{
				for (int k = 0; k < 4; k++)
				{
					IntVector2 intVector = this.m_agunimFloorBasePosition + new IntVector2(j, k);
					GameManager.Instance.Dungeon.data[intVector].fallingPrevented = true;
				}
			}
		}
		yield return new WaitForSeconds(1f);
		if (this.RoomIdentifier == BulletPastRoomController.BulletRoomCategory.ROOM_A)
		{
			this.ExitWarp.DISABLED_TEMPORARILY = true;
		}
		this.m_readyForTests = true;
		yield break;
	}

	// Token: 0x06006042 RID: 24642 RVA: 0x00251128 File Offset: 0x0024F328
	private void HandleFlightCollider(SpeculativeRigidbody specRigidbody, SpeculativeRigidbody sourceSpecRigidbody, CollisionData collisionData)
	{
		if (!GameManager.Instance.IsLoadingLevel)
		{
			PlayerController component = specRigidbody.GetComponent<PlayerController>();
			if (component && component.IsFlying && !GameManager.Instance.IsLoadingLevel)
			{
				this.m_timeHovering += BraveTime.DeltaTime;
				if (this.m_timeHovering > 0.5f)
				{
					component.ForceFall();
				}
			}
		}
	}

	// Token: 0x06006043 RID: 24643 RVA: 0x00251198 File Offset: 0x0024F398
	private void PitfallIntoGunonRoom()
	{
		base.StartCoroutine(this.DoGunonPitfall());
	}

	// Token: 0x06006044 RID: 24644 RVA: 0x002511A8 File Offset: 0x0024F3A8
	private IEnumerator DoGunonPitfall()
	{
		Pixelator.Instance.FadeToBlack(5f, true, 0f);
		GunonIntroDoer gunonIntroDoer = null;
		List<AIActor> allActors = StaticReferenceManager.AllEnemies;
		for (int i = 0; i < allActors.Count; i++)
		{
			if (allActors[i])
			{
				gunonIntroDoer = allActors[i].GetComponent<GunonIntroDoer>();
				if (gunonIntroDoer)
				{
					break;
				}
			}
		}
		GenericIntroDoer genericIntroDoer = gunonIntroDoer.GetComponent<GenericIntroDoer>();
		genericIntroDoer.cameraMoveSpeed = 100f;
		genericIntroDoer.SuppressSkipping = true;
		float timer = 5f;
		while (timer > 0f)
		{
			timer -= GameManager.INVARIANT_DELTA_TIME;
			yield return null;
		}
		genericIntroDoer.cameraMoveSpeed = 15f;
		genericIntroDoer.SuppressSkipping = false;
		yield break;
	}

	// Token: 0x06006045 RID: 24645 RVA: 0x002511BC File Offset: 0x0024F3BC
	private void PlayerEntered(PlayerController p)
	{
		RoomHandler absoluteRoom = base.transform.position.GetAbsoluteRoom();
		if (absoluteRoom != null)
		{
			absoluteRoom.SealRoom();
		}
	}

	// Token: 0x06006046 RID: 24646 RVA: 0x002511E8 File Offset: 0x0024F3E8
	private void WalkedByOldBullet(SpeculativeRigidbody specrigidbody, SpeculativeRigidbody sourcespecrigidbody, CollisionData collisiondata)
	{
		if (!this.OldBulletTalkDoer)
		{
			return;
		}
		FsmBool fsmBool = this.OldBulletTalkDoer.playmakerFsm.FsmVariables.FindFsmBool("giftGiven");
		if (fsmBool != null && !fsmBool.Value)
		{
			this.OldBulletTalkDoer.playmakerFsm.SendEvent("playerInteract");
		}
	}

	// Token: 0x06006047 RID: 24647 RVA: 0x00251248 File Offset: 0x0024F448
	private void EnteredThroneDoorTrigger(SpeculativeRigidbody specrigidbody, SpeculativeRigidbody sourcespecrigidbody, CollisionData collisiondata)
	{
		if (this.ThroneRoomDoor)
		{
			base.StartCoroutine(this.MoveThroneRoomDoor());
		}
		SpeculativeRigidbody throneRoomDoorTrigger = this.ThroneRoomDoorTrigger;
		throneRoomDoorTrigger.OnEnterTrigger = (SpeculativeRigidbody.OnTriggerDelegate)Delegate.Remove(throneRoomDoorTrigger.OnEnterTrigger, new SpeculativeRigidbody.OnTriggerDelegate(this.EnteredThroneDoorTrigger));
	}

	// Token: 0x06006048 RID: 24648 RVA: 0x0025129C File Offset: 0x0024F49C
	private void HandleRoomBCleared()
	{
		if (this.ThroneRoomDoorTrigger)
		{
			this.ThroneRoomDoorTrigger.enabled = true;
		}
		this.ExitWarp.DISABLED_TEMPORARILY = false;
	}

	// Token: 0x06006049 RID: 24649 RVA: 0x002512C8 File Offset: 0x0024F4C8
	private IEnumerator MoveThroneRoomDoor()
	{
		float elapsed = 0f;
		float duration = 10f;
		SpeculativeRigidbody doorRigidbody = this.ThroneRoomDoor.specRigidbody;
		float poofDelta = 0.2f;
		float nextPoofTime = 0f;
		GameManager.Instance.MainCameraController.DoContinuousScreenShake(this.ThroneRoomDoorShake, this, false);
		AkSoundEngine.PostEvent("Play_ENV_quake_loop_01", base.gameObject);
		while (elapsed < duration)
		{
			elapsed += BraveTime.DeltaTime;
			float t = BraveMathCollege.DoubleLerpSmooth(0f, 1f, 0f, elapsed / duration);
			this.ThroneRoomDoor.Velocity = new Vector2(-1f * t, 0f);
			while (elapsed > nextPoofTime)
			{
				float num = BraveMathCollege.DoubleLerpSmooth(0f, 1f, 0f, nextPoofTime / duration);
				nextPoofTime += poofDelta;
				if (UnityEngine.Random.value < num)
				{
					this.ThroneRoomDoorVfx.SpawnAtPosition(BraveUtility.RandomVector2(doorRigidbody.UnitBottomLeft, doorRigidbody.UnitBottomRight), 0f, null, null, null, null, false, null, null, false);
				}
			}
			yield return null;
		}
		GameManager.Instance.MainCameraController.StopContinuousScreenShake(this);
		AkSoundEngine.PostEvent("Stop_ENV_quake_loop_01", base.gameObject);
		this.ThroneRoomDoor.Velocity = Vector2.zero;
		yield break;
	}

	// Token: 0x0600604A RID: 24650 RVA: 0x002512E4 File Offset: 0x0024F4E4
	private void Update()
	{
		if (Dungeon.IsGenerating)
		{
			return;
		}
		if (this.RoomIdentifier == BulletPastRoomController.BulletRoomCategory.ROOM_A && this.m_readyForTests && this.ExitWarp.DISABLED_TEMPORARILY && GameManager.Instance.PrimaryPlayer.CurrentGun != null)
		{
			Debug.LogError(GameManager.Instance.PrimaryPlayer.CurrentGun);
			this.ExitWarp.DISABLED_TEMPORARILY = false;
		}
	}

	// Token: 0x0600604B RID: 24651 RVA: 0x0025135C File Offset: 0x0024F55C
	public IEnumerator HandleAgunimIntro(Transform bossTransform)
	{
		bossTransform.GetComponent<AIActor>().ToggleRenderers(false);
		this.AgunimPreDeathTalker.gameObject.SetActive(true);
		this.AgunimPreDeathTalker.transform.position = bossTransform.position;
		this.AgunimPreDeathTalker.specRigidbody.Reinitialize();
		this.AgunimPreDeathTalker.sprite.IsPerpendicular = true;
		this.AgunimPreDeathTalker.sprite.HeightOffGround = -1f;
		this.AgunimPreDeathTalker.sprite.UpdateZDepth();
		this.AgunimPreDeathTalker.Interact(GameManager.Instance.PrimaryPlayer);
		GameManager.Instance.MainCameraController.OverridePosition = this.AgunimPreDeathTalker.specRigidbody.UnitCenter;
		while (this.AgunimPreDeathTalker.IsTalking)
		{
			yield return null;
		}
		this.AgunimPreDeathTalker.specRigidbody.enabled = false;
		this.AgunimPreDeathTalker.gameObject.SetActive(false);
		bossTransform.GetComponent<AIActor>().ToggleRenderers(true);
		List<AIActor> enemiesInRoom = this.AgunimPreDeathTalker.ParentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
		for (int i = 0; i < enemiesInRoom.Count; i++)
		{
			GenericIntroDoer component = enemiesInRoom[i].GetComponent<GenericIntroDoer>();
			if (component)
			{
				component.TriggerSequence(GameManager.Instance.PrimaryPlayer);
			}
		}
		yield break;
	}

	// Token: 0x0600604C RID: 24652 RVA: 0x00251380 File Offset: 0x0024F580
	public void OnGanonDeath(Transform bossTransform)
	{
		base.StartCoroutine(this.HandleGanonDeath(bossTransform));
	}

	// Token: 0x0600604D RID: 24653 RVA: 0x00251390 File Offset: 0x0024F590
	public IEnumerator HandleGanonDeath(Transform bossTransform)
	{
		yield return null;
		yield break;
	}

	// Token: 0x0600604E RID: 24654 RVA: 0x002513A4 File Offset: 0x0024F5A4
	public IEnumerator HandleAgunimDeath(Transform bossTransform)
	{
		this.AgunimPostDeathTalker.gameObject.SetActive(true);
		this.AgunimPostDeathTalker.transform.position = bossTransform.position;
		this.AgunimPostDeathTalker.specRigidbody.Reinitialize();
		this.AgunimPostDeathTalker.sprite.IsPerpendicular = true;
		this.AgunimPostDeathTalker.sprite.HeightOffGround = -1f;
		this.AgunimPostDeathTalker.sprite.UpdateZDepth();
		this.AgunimPostDeathTalker.Interact(GameManager.Instance.PrimaryPlayer);
		while (this.AgunimPostDeathTalker.IsTalking)
		{
			yield return null;
		}
		GameManager.Instance.MainCameraController.StartTrackingPlayer();
		GameManager.Instance.MainCameraController.SetManualControl(false, true);
		yield return new WaitForSeconds(1f);
		while (this.AgunimPostDeathTalker.aiAnimator.IsPlaying("die"))
		{
			yield return null;
		}
		UnityEngine.Object.Destroy(this.AgunimPostDeathTalker.gameObject);
		GameManager.Instance.MainCameraController.DoContinuousScreenShake(this.AgunimPostDeathShake, this, false);
		AkSoundEngine.PostEvent("Play_ENV_quake_loop_01", base.gameObject);
		yield return new WaitForSeconds(3f);
		this.TriggerAgumimFloorBreak();
		AkSoundEngine.PostEvent("Stop_ENV_quake_loop_01", base.gameObject);
		yield return new WaitForSeconds(1f);
		if (this.AgunimFlightCollider)
		{
			SpeculativeRigidbody agunimFlightCollider = this.AgunimFlightCollider;
			agunimFlightCollider.OnTriggerCollision = (SpeculativeRigidbody.OnTriggerDelegate)Delegate.Combine(agunimFlightCollider.OnTriggerCollision, new SpeculativeRigidbody.OnTriggerDelegate(this.HandleFlightCollider));
		}
		GameManager.Instance.MainCameraController.StopContinuousScreenShake(this);
		yield break;
	}

	// Token: 0x0600604F RID: 24655 RVA: 0x002513C8 File Offset: 0x0024F5C8
	public void TriggerAgumimFloorBreak()
	{
		if (this.AgunimFloorChunk != null)
		{
			this.AgunimFloorChunk.transform.localPosition = new Vector3(9.75f, 10.625f, 12.375f);
			this.AgunimFloorChunk.PlayAndDisableRenderer("agunim_lair_burst");
			for (int i = 0; i < 4; i++)
			{
				for (int j = 0; j < 4; j++)
				{
					IntVector2 intVector = this.m_agunimFloorBasePosition + new IntVector2(i, j);
					GameManager.Instance.Dungeon.data[intVector].fallingPrevented = false;
				}
			}
		}
	}

	// Token: 0x06006050 RID: 24656 RVA: 0x0025146C File Offset: 0x0024F66C
	public void TriggerBulletmanEnding()
	{
		if (this.RoomIdentifier == BulletPastRoomController.BulletRoomCategory.ROOM_D)
		{
			GameStatsManager.Instance.SetCharacterSpecificFlag(CharacterSpecificGungeonFlags.KILLED_PAST, true);
			base.StartCoroutine(this.TriggerBulletmanEnding_CR());
		}
	}

	// Token: 0x06006051 RID: 24657 RVA: 0x00251498 File Offset: 0x0024F698
	public IEnumerator TriggerBulletmanEnding_CR()
	{
		PastCameraUtility.LockConversation(GameManager.Instance.MainCameraController.transform.position.XY());
		tk2dSpriteFromTexture animSprite = null;
		yield return new WaitForSeconds(1.5f);
		Pixelator.Instance.SetVignettePower(5f);
		if (this.RoomIdentifier == BulletPastRoomController.BulletRoomCategory.ROOM_D && this.BulletmanEndingFrames.Length > 0 && this.BulletmanEndingFrames[0] != null)
		{
			GameObject gameObject = new GameObject("ending sprite");
			gameObject.AddComponent<tk2dSprite>();
			tk2dSpriteFromTexture tk2dSpriteFromTexture = gameObject.AddComponent<tk2dSpriteFromTexture>();
			tk2dSpriteCollectionSize tk2dSpriteCollectionSize = tk2dSpriteCollectionSize.Default();
			animSprite = tk2dSpriteFromTexture;
			tk2dSpriteFromTexture.Create(tk2dSpriteCollectionSize, this.BulletmanEndingFrames[0], tk2dBaseSprite.Anchor.MiddleCenter);
			tk2dSpriteFromTexture.spriteCollectionSize.type = tk2dSpriteCollectionSize.Type.Explicit;
			tk2dSpriteFromTexture.spriteCollectionSize.orthoSize = 0.5f;
			tk2dSpriteFromTexture.spriteCollectionSize.height = 16f;
			tk2dSpriteFromTexture.gameObject.SetLayerRecursively(LayerMask.NameToLayer("FG_Critical"));
			tk2dSpriteFromTexture.transform.position = GameManager.Instance.PrimaryPlayer.CenterPosition.ToVector3ZUp(1f);
			AdditionalBraveLight additionalBraveLight = tk2dSpriteFromTexture.gameObject.AddComponent<AdditionalBraveLight>();
			additionalBraveLight.LightIntensity = 1f;
			additionalBraveLight.LightRadius = 20f;
			base.StartCoroutine(this.AnimateBulletmanEnding(tk2dSpriteFromTexture));
		}
		Pixelator.Instance.DoOcclusionLayer = false;
		for (int i = 0; i < GameManager.Instance.AllPlayers.Length; i++)
		{
			GameManager.Instance.AllPlayers[i].SetInputOverride("ending");
		}
		GameObject instanceQuad = UnityEngine.Object.Instantiate<GameObject>(this.BulletmanEndingQuad);
		instanceQuad.transform.position = GameManager.Instance.PrimaryPlayer.CenterPosition.ToVector3ZUp(0f);
		animSprite.GetComponent<tk2dSprite>().HeightOffGround = animSprite.transform.position.y + 3f;
		animSprite.GetComponent<tk2dSprite>().UpdateZDepth();
		GameManager.Instance.MainCameraController.SetManualControl(true, false);
		GameManager.Instance.MainCameraController.OverridePosition = GameManager.Instance.PrimaryPlayer.CenterPosition;
		Pixelator.Instance.FadeToBlack(1f, true, 0.5f);
		yield return new WaitForSeconds(5.5f);
		Pixelator.Instance.FreezeFrame();
		BraveTime.RegisterTimeScaleMultiplier(0f, base.gameObject);
		float ela = 0f;
		while (ela < ConvictPastController.FREEZE_FRAME_DURATION)
		{
			ela += GameManager.INVARIANT_DELTA_TIME;
			yield return null;
		}
		BraveTime.ClearMultiplier(base.gameObject);
		GameManager.Instance.MainCameraController.SetManualControl(false, false);
		TimeTubeCreditsController ttcc = new TimeTubeCreditsController();
		ttcc.ClearDebris();
		Pixelator.Instance.FadeToColor(0.15f, Color.white, true, 0.15f);
		Pixelator.Instance.SetVignettePower(1f);
		yield return base.StartCoroutine(ttcc.HandleTimeTubeCredits(GameManager.Instance.PrimaryPlayer.sprite.WorldCenter, false, null, -1, false));
		AmmonomiconController.Instance.OpenAmmonomicon(true, true);
		yield break;
	}

	// Token: 0x06006052 RID: 24658 RVA: 0x002514B4 File Offset: 0x0024F6B4
	private IEnumerator AnimateBulletmanEnding(tk2dSpriteFromTexture sft)
	{
		float elapsed = 0f;
		int currentIndex = 0;
		for (;;)
		{
			elapsed += BraveTime.DeltaTime;
			if (elapsed > 0.175f)
			{
				elapsed -= 0.175f;
				currentIndex = (currentIndex + 1) % this.BulletmanEndingFrames.Length;
				sft.Create(sft.spriteCollectionSize, this.BulletmanEndingFrames[currentIndex], tk2dBaseSprite.Anchor.MiddleCenter);
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x04005ADE RID: 23262
	public BulletPastRoomController.BulletRoomCategory RoomIdentifier;

	// Token: 0x04005ADF RID: 23263
	public WarpPointHandler EntranceWarp;

	// Token: 0x04005AE0 RID: 23264
	public WarpPointHandler ExitWarp;

	// Token: 0x04005AE1 RID: 23265
	private TalkDoerLite OldBulletTalkDoer;

	// Token: 0x04005AE2 RID: 23266
	public SpeculativeRigidbody OldBulletTalkTrigger;

	// Token: 0x04005AE3 RID: 23267
	public TalkDoerLite AgunimPreDeathTalker;

	// Token: 0x04005AE4 RID: 23268
	public TalkDoerLite AgunimPostDeathTalker;

	// Token: 0x04005AE5 RID: 23269
	public tk2dSpriteAnimator AgunimFloorChunk;

	// Token: 0x04005AE6 RID: 23270
	public ScreenShakeSettings AgunimPostDeathShake;

	// Token: 0x04005AE7 RID: 23271
	public SpeculativeRigidbody AgunimFlightCollider;

	// Token: 0x04005AE8 RID: 23272
	public SpeculativeRigidbody ThroneRoomDoor;

	// Token: 0x04005AE9 RID: 23273
	public SpeculativeRigidbody ThroneRoomDoorTrigger;

	// Token: 0x04005AEA RID: 23274
	public VFXPool ThroneRoomDoorVfx;

	// Token: 0x04005AEB RID: 23275
	public ScreenShakeSettings ThroneRoomDoorShake;

	// Token: 0x04005AEC RID: 23276
	private IntVector2 m_agunimFloorBasePosition;

	// Token: 0x04005AED RID: 23277
	private BulletPastRoomController RoomB;

	// Token: 0x04005AEE RID: 23278
	private BulletPastRoomController RoomC;

	// Token: 0x04005AEF RID: 23279
	private BulletPastRoomController RoomD;

	// Token: 0x04005AF0 RID: 23280
	public GameObject BulletmanEndingQuad;

	// Token: 0x04005AF1 RID: 23281
	public Texture2D[] BulletmanEndingFrames;

	// Token: 0x04005AF2 RID: 23282
	private bool m_readyForTests;

	// Token: 0x04005AF3 RID: 23283
	private float m_timeHovering;

	// Token: 0x0200110E RID: 4366
	public enum BulletRoomCategory
	{
		// Token: 0x04005AF5 RID: 23285
		ROOM_A,
		// Token: 0x04005AF6 RID: 23286
		ROOM_B,
		// Token: 0x04005AF7 RID: 23287
		ROOM_C,
		// Token: 0x04005AF8 RID: 23288
		ROOM_D
	}
}
