using System;
using System.Collections;
using System.Collections.Generic;
using Dungeonator;
using UnityEngine;

// Token: 0x02001061 RID: 4193
public class MegalichDeathController : BraveBehaviour
{
	// Token: 0x06005C2E RID: 23598 RVA: 0x002353AC File Offset: 0x002335AC
	public IEnumerator Start()
	{
		while (Dungeon.IsGenerating)
		{
			yield return null;
		}
		GameManager.Instance.Dungeon.StartCoroutine(this.LateStart());
		yield break;
	}

	// Token: 0x06005C2F RID: 23599 RVA: 0x002353C8 File Offset: 0x002335C8
	public IEnumerator LateStart()
	{
		yield return null;
		List<AIActor> allActors = StaticReferenceManager.AllEnemies;
		for (int i = 0; i < allActors.Count; i++)
		{
			if (allActors[i])
			{
				InfinilichDeathController component = allActors[i].GetComponent<InfinilichDeathController>();
				if (component)
				{
					this.m_infinilich = component;
					break;
				}
			}
		}
		RoomHandler megalichRoom = base.aiActor.ParentRoom;
		RoomHandler infinilichRoom = this.m_infinilich.aiActor.ParentRoom;
		infinilichRoom.AddDarkSoulsRoomResetDependency(megalichRoom);
		base.healthHaver.ManualDeathHandling = true;
		base.healthHaver.OnPreDeath += this.OnBossDeath;
		base.healthHaver.OverrideKillCamTime = new float?(3.5f);
		yield break;
	}

	// Token: 0x06005C30 RID: 23600 RVA: 0x002353E4 File Offset: 0x002335E4
	protected override void OnDestroy()
	{
		if (ChallengeManager.CHALLENGE_MODE_ACTIVE && this.m_challengesSuppressed)
		{
			ChallengeManager.Instance.SuppressChallengeStart = false;
			this.m_challengesSuppressed = false;
		}
		base.OnDestroy();
	}

	// Token: 0x06005C31 RID: 23601 RVA: 0x00235414 File Offset: 0x00233614
	private void OnBossDeath(Vector2 dir)
	{
		base.aiAnimator.PlayUntilCancelled("death", false, null, -1f, false);
		base.aiAnimator.StopVfx("double_pound");
		base.aiAnimator.StopVfx("left_pound");
		base.aiAnimator.StopVfx("right_pound");
		GameManager.Instance.StartCoroutine(this.OnDeathExplosionsCR());
		GameManager.Instance.StartCoroutine(this.OnDeathCR());
	}

	// Token: 0x06005C32 RID: 23602 RVA: 0x0023548C File Offset: 0x0023368C
	private IEnumerator OnDeathExplosionsCR()
	{
		PixelCollider collider = base.specRigidbody.HitboxPixelCollider;
		for (int i = 0; i < this.explosionCount; i++)
		{
			Vector2 minPos = collider.UnitBottomLeft;
			Vector2 maxPos = collider.UnitTopRight;
			GameObject vfxPrefab = BraveUtility.RandomElement<GameObject>(this.explosionVfx);
			Vector2 pos = BraveUtility.RandomVector2(minPos, maxPos, new Vector2(0.2f, 0.2f));
			GameObject vfxObj = SpawnManager.SpawnVFX(vfxPrefab, pos, Quaternion.identity);
			tk2dBaseSprite vfxSprite = vfxObj.GetComponent<tk2dBaseSprite>();
			vfxSprite.HeightOffGround = 0.8f;
			base.sprite.AttachRenderer(vfxSprite);
			base.sprite.UpdateZDepth();
			yield return new WaitForSeconds(this.explosionMidDelay);
		}
		yield break;
	}

	// Token: 0x06005C33 RID: 23603 RVA: 0x002354A8 File Offset: 0x002336A8
	private bool IsAnyPlayerFalling()
	{
		foreach (PlayerController playerController in GameManager.Instance.AllPlayers)
		{
			if (playerController && playerController.healthHaver.IsAlive && playerController.IsFalling)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06005C34 RID: 23604 RVA: 0x00235504 File Offset: 0x00233704
	private IEnumerator OnDeathCR()
	{
		if (ChallengeManager.CHALLENGE_MODE_ACTIVE)
		{
			ChallengeManager.Instance.ForceStop();
		}
		SuperReaperController.PreventShooting = true;
		yield return new WaitForSeconds(2f);
		while (this.IsAnyPlayerFalling())
		{
			yield return null;
		}
		Pixelator.Instance.FadeToColor(0.75f, Color.white, false, 0f);
		Minimap.Instance.TemporarilyPreventMinimap = true;
		GameUIRoot.Instance.HideCoreUI(string.Empty);
		GameUIRoot.Instance.ToggleLowerPanels(false, false, string.Empty);
		yield return new WaitForSeconds(3f);
		MegalichIntroDoer introDoer = base.GetComponent<MegalichIntroDoer>();
		introDoer.ModifyCamera(false);
		introDoer.BlockPitTiles(false);
		yield return new WaitForSeconds(0.75f);
		base.aiActor.StealthDeath = true;
		base.healthHaver.persistsOnDeath = true;
		base.healthHaver.DeathAnimationComplete(null, null);
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
		if (base.aiAnimator.ChildAnimator)
		{
			UnityEngine.Object.Destroy(base.aiAnimator.ChildAnimator.gameObject);
		}
		if (base.aiAnimator)
		{
			UnityEngine.Object.Destroy(base.aiAnimator);
		}
		if (base.specRigidbody)
		{
			UnityEngine.Object.Destroy(base.specRigidbody);
		}
		base.RegenerateCache();
		Minimap.Instance.TemporarilyPreventMinimap = true;
		if (ChallengeManager.CHALLENGE_MODE_ACTIVE)
		{
			ChallengeManager.Instance.SuppressChallengeStart = true;
			this.m_challengesSuppressed = true;
		}
		AIActor infinilich = this.m_infinilich.GetComponent<AIActor>();
		RoomHandler infinilichRoom = GameManager.Instance.Dungeon.data.rooms.Find((RoomHandler r) => r.GetRoomName() == "LichRoom03");
		int numPlayers = GameManager.Instance.AllPlayers.Length;
		infinilich.visibilityManager.SuppressPlayerEnteredRoom = true;
		for (int i = 0; i < numPlayers; i++)
		{
			GameManager.Instance.AllPlayers[i].SetInputOverride("lich transition");
		}
		while (this.IsAnyPlayerFalling())
		{
			yield return null;
		}
		yield return new WaitForSeconds(0.1f);
		TimeTubeCreditsController.AcquireTunnelInstanceInAdvance();
		TimeTubeCreditsController.AcquirePastDioramaInAdvance();
		yield return null;
		PlayerController player = GameManager.Instance.PrimaryPlayer;
		Vector2 targetPoint = infinilichRoom.area.Center + new Vector2(0f, -5f);
		if (player)
		{
			player.WarpToPoint(targetPoint, false, false);
			player.DoSpinfallSpawn(0.5f);
		}
		if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER)
		{
			PlayerController otherPlayer = GameManager.Instance.GetOtherPlayer(player);
			if (otherPlayer)
			{
				otherPlayer.ReuniteWithOtherPlayer(player, false);
				otherPlayer.DoSpinfallSpawn(0.5f);
			}
		}
		this.m_infinilich.GetComponent<InfinilichIntroDoer>().ModifyWorld(true);
		Vector2 idealCameraPosition = infinilich.GetComponent<GenericIntroDoer>().BossCenter;
		CameraController camera = GameManager.Instance.MainCameraController;
		camera.SetManualControl(true, false);
		camera.OverridePosition = idealCameraPosition;
		Pixelator.Instance.FadeToColor(1f, Color.white, true, 0f);
		yield return new WaitForSeconds(0.4f);
		Vector2 center = infinilich.specRigidbody.UnitCenter + new Vector2(0f, 10f);
		for (int j = 0; j < 150; j++)
		{
			this.SpawnShellCasingAtPosition(center + UnityEngine.Random.insideUnitCircle.Scale(2f, 1f) * 5f);
		}
		yield return new WaitForSeconds(2f);
		for (int k = 0; k < numPlayers; k++)
		{
			GameManager.Instance.AllPlayers[k].ClearInputOverride("lich transition");
		}
		if (ChallengeManager.CHALLENGE_MODE_ACTIVE)
		{
			ChallengeManager.Instance.SuppressChallengeStart = false;
			this.m_challengesSuppressed = false;
			ChallengeManager.Instance.EnteredCombat();
		}
		infinilich.visibilityManager.ChangeToVisibility(RoomHandler.VisibilityStatus.CURRENT, true);
		Minimap.Instance.TemporarilyPreventMinimap = false;
		infinilich.GetComponent<GenericIntroDoer>().TriggerSequence(player);
		UnityEngine.Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x06005C35 RID: 23605 RVA: 0x00235520 File Offset: 0x00233720
	private void SpawnShellCasingAtPosition(Vector3 position)
	{
		if (this.shellCasing != null)
		{
			float num = UnityEngine.Random.Range(-100f, -80f);
			GameObject gameObject = SpawnManager.SpawnDebris(this.shellCasing, position, Quaternion.Euler(0f, 0f, num));
			ShellCasing component = gameObject.GetComponent<ShellCasing>();
			if (component != null)
			{
				component.Trigger();
			}
			DebrisObject component2 = gameObject.GetComponent<DebrisObject>();
			if (component2 != null)
			{
				Vector3 vector = BraveMathCollege.DegreesToVector(num, UnityEngine.Random.Range(0.5f, 1f)).ToVector3ZUp(UnityEngine.Random.value * 1.5f + 1f);
				component2.Trigger(vector, UnityEngine.Random.Range(8f, 10f), 1f);
			}
		}
	}

	// Token: 0x040055CC RID: 21964
	public List<GameObject> explosionVfx;

	// Token: 0x040055CD RID: 21965
	public float explosionMidDelay = 0.3f;

	// Token: 0x040055CE RID: 21966
	public int explosionCount = 10;

	// Token: 0x040055CF RID: 21967
	public GameObject shellCasing;

	// Token: 0x040055D0 RID: 21968
	private InfinilichDeathController m_infinilich;

	// Token: 0x040055D1 RID: 21969
	private bool m_challengesSuppressed;
}
