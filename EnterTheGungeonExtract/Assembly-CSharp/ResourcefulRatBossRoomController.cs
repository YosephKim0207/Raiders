using System;
using System.Collections;
using Dungeonator;
using UnityEngine;

// Token: 0x020011EA RID: 4586
public class ResourcefulRatBossRoomController : MonoBehaviour
{
	// Token: 0x0600665C RID: 26204 RVA: 0x0027CD0C File Offset: 0x0027AF0C
	public IEnumerator Start()
	{
		while (Dungeon.IsGenerating)
		{
			yield return null;
		}
		GameManager.Instance.Dungeon.StartCoroutine(this.LateStart());
		yield break;
	}

	// Token: 0x0600665D RID: 26205 RVA: 0x0027CD28 File Offset: 0x0027AF28
	public IEnumerator LateStart()
	{
		yield return null;
		ResourcefulRatController rat = UnityEngine.Object.FindObjectOfType<ResourcefulRatController>();
		MetalGearRatController metalGearRat = UnityEngine.Object.FindObjectOfType<MetalGearRatController>();
		this.m_ratRoom = rat.aiActor.ParentRoom;
		RoomHandler metalGearRatRoom = ((!metalGearRat) ? null : metalGearRat.aiActor.ParentRoom);
		if (metalGearRatRoom != null)
		{
			this.m_ratRoom.TargetPitfallRoom = metalGearRatRoom;
			this.m_ratRoom.ForcePitfallForFliers = true;
			RoomHandler ratRoom = this.m_ratRoom;
			ratRoom.OnTargetPitfallRoom = (Action)Delegate.Combine(ratRoom.OnTargetPitfallRoom, new Action(this.PitfallIntoMetalGearRatRoom));
			RoomHandler ratRoom2 = this.m_ratRoom;
			ratRoom2.OnDarkSoulsReset = (Action)Delegate.Combine(ratRoom2.OnDarkSoulsReset, new Action(this.OnDarkSoulsReset));
			metalGearRatRoom.AddDarkSoulsRoomResetDependency(this.m_ratRoom);
		}
		this.EnablePitfalls(false);
		yield break;
	}

	// Token: 0x0600665E RID: 26206 RVA: 0x0027CD44 File Offset: 0x0027AF44
	public void Update()
	{
		if (GameManager.HasInstance && this.m_isRoomSealed)
		{
			PlayerController bestActivePlayer = GameManager.Instance.BestActivePlayer;
			if (bestActivePlayer)
			{
				CellArea area = this.m_ratRoom.area;
				Vector2 unitCenter = bestActivePlayer.specRigidbody.GetUnitCenter(ColliderType.Ground);
				Vector2 vector = area.UnitBottomLeft + new Vector2(-2f, -8f);
				Vector2 vector2 = area.UnitTopRight + new Vector2(2f, 2f);
				if (!unitCenter.IsWithin(vector, vector2))
				{
					this.SpecialSealRoom(false);
				}
			}
		}
	}

	// Token: 0x0600665F RID: 26207 RVA: 0x0027CDE4 File Offset: 0x0027AFE4
	public void OpenGrate()
	{
		this.SpecialSealRoom(true);
		this.grateAnimator.Play("rat_grate");
	}

	// Token: 0x06006660 RID: 26208 RVA: 0x0027CE00 File Offset: 0x0027B000
	public void OnDarkSoulsReset()
	{
		this.SpecialSealRoom(false);
		this.grateAnimator.StopAndResetFrameToDefault();
		this.EnablePitfalls(false);
	}

	// Token: 0x06006661 RID: 26209 RVA: 0x0027CE1C File Offset: 0x0027B01C
	private void SpecialSealRoom(bool seal)
	{
		this.m_isRoomSealed = seal;
		this.m_ratRoom.npcSealState = ((!seal) ? RoomHandler.NPCSealState.SealNone : RoomHandler.NPCSealState.SealAll);
		foreach (DungeonDoorController dungeonDoorController in this.m_ratRoom.connectedDoors)
		{
			dungeonDoorController.KeepBossDoorSealed = seal;
		}
	}

	// Token: 0x06006662 RID: 26210 RVA: 0x0027CE9C File Offset: 0x0027B09C
	public void EnablePitfalls(bool value)
	{
		RoomHandler absoluteRoom = base.transform.position.GetAbsoluteRoom();
		IntVector2 intVector = absoluteRoom.area.basePosition + new IntVector2(16, 10);
		for (int i = 0; i < 4; i++)
		{
			for (int j = 0; j < 4; j++)
			{
				IntVector2 intVector2 = intVector + new IntVector2(i, j);
				GameManager.Instance.Dungeon.data[intVector2].fallingPrevented = !value;
			}
		}
	}

	// Token: 0x06006663 RID: 26211 RVA: 0x0027CF28 File Offset: 0x0027B128
	private void PitfallIntoMetalGearRatRoom()
	{
		for (int i = 0; i < GameManager.Instance.AllPlayers.Length; i++)
		{
			PlayerController playerController = GameManager.Instance.AllPlayers[i];
			playerController.IsOnFire = false;
			playerController.CurrentFireMeterValue = 0f;
			playerController.CurrentPoisonMeterValue = 0f;
		}
		this.SpecialSealRoom(false);
		GameManager.Instance.StartCoroutine(this.DoMetalGearPitfall());
	}

	// Token: 0x06006664 RID: 26212 RVA: 0x0027CF94 File Offset: 0x0027B194
	private IEnumerator DoMetalGearPitfall()
	{
		BossKillCam extantCam = UnityEngine.Object.FindObjectOfType<BossKillCam>();
		if (extantCam)
		{
			extantCam.ForceCancelSequence();
		}
		int numPlayers = GameManager.Instance.AllPlayers.Length;
		AIActor metalGearRat = StaticReferenceManager.AllEnemies.Find((AIActor e) => e.healthHaver && e.healthHaver.IsBoss && e.GetComponent<MetalGearRatIntroDoer>());
		if (metalGearRat == null)
		{
			yield break;
		}
		metalGearRat.GetComponent<GenericIntroDoer>().triggerType = GenericIntroDoer.TriggerType.BossTriggerZone;
		metalGearRat.visibilityManager.SuppressPlayerEnteredRoom = true;
		metalGearRat.aiAnimator.PlayUntilCancelled("intro_idle", false, null, -1f, false);
		CameraController camera = GameManager.Instance.MainCameraController;
		camera.SetZoomScaleImmediate(0.66f);
		camera.LockToRoom = true;
		for (int i = 0; i < numPlayers; i++)
		{
			GameManager.Instance.AllPlayers[i].SetInputOverride("lich transition");
		}
		metalGearRat.specRigidbody.Initialize();
		Vector2 idealCameraPosition = metalGearRat.GetComponent<GenericIntroDoer>().BossCenter;
		camera.SetManualControl(true, false);
		camera.OverridePosition = idealCameraPosition + new Vector2(0f, 4f);
		Minimap.Instance.TemporarilyPreventMinimap = true;
		GameUIRoot.Instance.HideCoreUI(string.Empty);
		GameUIRoot.Instance.ToggleLowerPanels(false, false, string.Empty);
		metalGearRat.GetComponent<MetalGearRatIntroDoer>().Init();
		Pixelator.Instance.FadeToBlack(0.5f, true, 0f);
		metalGearRat.visibilityManager.ChangeToVisibility(RoomHandler.VisibilityStatus.CURRENT, true);
		foreach (PlayerController playerController in GameManager.Instance.AllPlayers)
		{
			playerController.DoSpinfallSpawn(3f);
			playerController.WarpFollowersToPlayer(false);
		}
		float timer = 0f;
		float duration = 3f;
		while (timer < duration)
		{
			yield return null;
			timer += BraveTime.DeltaTime;
			if (camera)
			{
				camera.OverridePosition = idealCameraPosition + new Vector2(0f, Mathf.SmoothStep(4f, 0f, timer / duration));
			}
		}
		yield return new WaitForSeconds(2.5f);
		if (GameManager.HasInstance)
		{
			for (int k = 0; k < numPlayers; k++)
			{
				GameManager.Instance.AllPlayers[k].ClearInputOverride("lich transition");
			}
		}
		GenericIntroDoer metalGearIntroDoer = metalGearRat.GetComponent<GenericIntroDoer>();
		metalGearIntroDoer.SkipWalkIn();
		if (metalGearRat)
		{
			metalGearIntroDoer.TriggerSequence(GameManager.Instance.BestActivePlayer);
		}
		yield break;
	}

	// Token: 0x04006236 RID: 25142
	public tk2dSpriteAnimator grateAnimator;

	// Token: 0x04006237 RID: 25143
	private RoomHandler m_ratRoom;

	// Token: 0x04006238 RID: 25144
	private bool m_isRoomSealed;
}
