using System;
using System.Collections;
using System.Collections.Generic;
using Dungeonator;
using UnityEngine;

// Token: 0x020011F6 RID: 4598
public class RobotPastController : MonoBehaviour
{
	// Token: 0x060066A8 RID: 26280 RVA: 0x0027F4A0 File Offset: 0x0027D6A0
	private void Start()
	{
		RoomHandler entrance = GameManager.Instance.Dungeon.data.Entrance;
		this.innerRectMin = entrance.area.basePosition.ToVector2() + new Vector2(1f, 3f);
		this.innerRectMax = this.innerRectMin + entrance.area.dimensions.ToVector2() + new Vector2(-3f, -8.75f);
		this.outerRectMin = this.innerRectMin + new Vector2(-5f, -5f);
		this.outerRectMax = this.innerRectMax + new Vector2(5f, 5f);
		this.excludedRect = new Rect(this.EmperorSprite.WorldBottomLeft + new Vector2(-0.75f, -0.75f), this.EmperorSprite.WorldTopRight - this.EmperorSprite.WorldBottomLeft + new Vector2(1.25f, 1.25f));
		BraveUtility.DrawDebugSquare(this.innerRectMin, this.innerRectMax, Color.cyan, 1000f);
		BraveUtility.DrawDebugSquare(this.outerRectMin, this.outerRectMax, Color.cyan, 1000f);
		this.DistributePoints();
		float[] array = new float[] { 0f, 0.5f, 0.7f, 0.9f, 1f };
		for (int i = 0; i < 5; i++)
		{
			Material material = new Material(this.RobotPrefab.GetComponent<Renderer>().sharedMaterial);
			material.SetColor("_OverrideColor", new Color(0.05f, 0.05f, 0.05f, array[i]));
			this.m_fadeMaterials.Add(material);
		}
		this.RobotPrefab.transform.position = new Vector3(1000f, -100f, -100f);
		if (this.InstantBossFight)
		{
			PlayerController primaryPlayer = GameManager.Instance.PrimaryPlayer;
			List<HealthHaver> allHealthHavers = StaticReferenceManager.AllHealthHavers;
			for (int j = 0; j < allHealthHavers.Count; j++)
			{
				if (allHealthHavers[j].IsBoss)
				{
					allHealthHavers[j].GetComponent<ObjectVisibilityManager>().ChangeToVisibility(RoomHandler.VisibilityStatus.CURRENT, true);
					allHealthHavers[j].GetComponent<GenericIntroDoer>().TriggerSequence(primaryPlayer);
				}
			}
			return;
		}
		base.StartCoroutine(this.HandlePastIntro());
	}

	// Token: 0x060066A9 RID: 26281 RVA: 0x0027F708 File Offset: 0x0027D908
	private TerminatorPanelController HandleTerminatorUIOverlay()
	{
		dfControl dfControl = GameUIRoot.Instance.Manager.AddPrefab(ResourceCache.Acquire("Global Prefabs/TerminatorPanel") as GameObject);
		(dfControl as dfPanel).Size = GameUIRoot.Instance.Manager.GetScreenSize() * GameUIRoot.Instance.Manager.UIScale;
		TerminatorPanelController component = dfControl.GetComponent<TerminatorPanelController>();
		base.StartCoroutine(this.HandleTerminatorUIOverlay_CR(component));
		return component;
	}

	// Token: 0x060066AA RID: 26282 RVA: 0x0027F778 File Offset: 0x0027D978
	private IEnumerator HandleTerminatorUIOverlay_CR(TerminatorPanelController tpc)
	{
		float elapsed = 0f;
		tpc.Trigger();
		while (elapsed < 0.5f)
		{
			elapsed += BraveTime.DeltaTime;
			float t = elapsed / 0.5f;
			Pixelator.Instance.SetSaturationColorPower(Color.red, t);
			Pixelator.Instance.fade = Mathf.Lerp(1f, 2.5f, (t - 0.5f) * 2f);
			Pixelator.Instance.GetComponent<SENaturalBloomAndDirtyLens>().bloomIntensity = Mathf.Lerp(0.05f, 0.25f, (t - 0.5f) * 2f);
			yield return null;
		}
		while (tpc.IsActive)
		{
			yield return null;
		}
		elapsed = 0f;
		float duration = 1.25f;
		while (elapsed < duration)
		{
			elapsed += BraveTime.DeltaTime;
			float t2 = 1f - elapsed / duration;
			Pixelator.Instance.SetSaturationColorPower(Color.red, t2);
			Pixelator.Instance.fade = Mathf.Lerp(1f, 2.5f, t2);
			Pixelator.Instance.GetComponent<SENaturalBloomAndDirtyLens>().bloomIntensity = Mathf.Lerp(0.05f, 0.25f, t2);
			yield return null;
		}
		yield break;
	}

	// Token: 0x060066AB RID: 26283 RVA: 0x0027F794 File Offset: 0x0027D994
	private IEnumerator HandlePastIntro()
	{
		while (Dungeon.IsGenerating)
		{
			yield return null;
		}
		PlayerController m_robot = GameManager.Instance.PrimaryPlayer;
		PastCameraUtility.LockConversation(m_robot.CenterPosition);
		m_robot.IsVisible = false;
		yield return new WaitForSeconds(2f);
		SpawnManager.SpawnVFX(this.WarpVFX, m_robot.CenterPosition, Quaternion.identity, true);
		AkSoundEngine.PostEvent("Play_OBJ_chestwarp_use_01", base.gameObject);
		yield return new WaitForSeconds(1f);
		m_robot.IsVisible = true;
		GameManager.Instance.MainCameraController.OverridePosition = m_robot.CenterPosition;
		yield return new WaitForSeconds(1f);
		TerminatorPanelController tpc = this.HandleTerminatorUIOverlay();
		if (tpc)
		{
			while (tpc.IsActive)
			{
				yield return null;
			}
		}
		this.WelcomeBot.Interact(m_robot);
		GameManager.Instance.MainCameraController.OverridePosition = m_robot.CenterPosition;
		m_robot.ForceIdleFacePoint(Vector2.down, true);
		while (this.WelcomeBot.IsTalking)
		{
			yield return null;
		}
		PastCameraUtility.UnlockConversation();
		while (m_robot.CenterPosition.y < this.EmperorBot.transform.position.y - 12f)
		{
			yield return null;
		}
		PastCameraUtility.LockConversation(m_robot.CenterPosition);
		GameManager.Instance.MainCameraController.OverridePosition = this.EmperorSprite.WorldCenter + new Vector2(0f, -3f);
		yield return null;
		this.EmperorBot.Interact(m_robot);
		GameManager.Instance.MainCameraController.OverridePosition = this.EmperorSprite.WorldCenter + new Vector2(0f, -3f);
		m_robot.ForceIdleFacePoint(Vector2.down, true);
		this.WelcomeBot.gameObject.SetActive(false);
		this.WelcomeBot.specRigidbody.enabled = false;
		bool recentered = false;
		for (int i = 0; i < GameManager.Instance.AllPlayers.Length; i++)
		{
			GameManager.Instance.AllPlayers[i].ForceMoveInDirectionUntilThreshold(Vector2.down, GameManager.Instance.AllPlayers[i].CenterPosition.y - 5f, 0.25f, 1f, null);
		}
		while (this.EmperorBot.IsTalking)
		{
			if (!recentered && this.EmperorBot.GetDungeonFSM().FsmVariables.GetFsmBool("recenter").Value)
			{
				recentered = true;
				base.StartCoroutine(this.LaunchRecenter(m_robot.CenterPosition));
			}
			yield return null;
		}
		PastCameraUtility.UnlockConversation();
		GameManager.Instance.MainCameraController.SetManualControl(true, true);
		Vector3 sarahSpawnPos = this.EmperorBot.transform.position + new Vector3(6f, -10f, 0f);
		GameManager.Instance.MainCameraController.OverridePosition = sarahSpawnPos;
		yield return new WaitForSeconds(1f);
		if (this.DoWavesOfEnemies)
		{
			m_robot.CurrentRoom.TriggerReinforcementLayersOnEvent(RoomEventTriggerCondition.NPC_TRIGGER_A, false);
			yield return new WaitForSeconds(5f);
			while (m_robot.CurrentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All).Count > 1)
			{
				yield return new WaitForSeconds(1f);
			}
		}
		List<HealthHaver> healthHavers = StaticReferenceManager.AllHealthHavers;
		for (int j = 0; j < healthHavers.Count; j++)
		{
			if (healthHavers[j].IsBoss)
			{
				base.StartCoroutine(this.StartBossFight(healthHavers[j], m_robot));
			}
		}
		yield break;
	}

	// Token: 0x060066AC RID: 26284 RVA: 0x0027F7B0 File Offset: 0x0027D9B0
	private IEnumerator StartBossFight(HealthHaver boss, PlayerController m_robot)
	{
		AkSoundEngine.PostEvent("Play_OBJ_chestwarp_use_01", base.gameObject);
		SpawnManager.SpawnVFX(this.WarpVFX, boss.specRigidbody.UnitCenter, Quaternion.identity, true);
		yield return new WaitForSeconds(0.25f);
		boss.GetComponent<ObjectVisibilityManager>().ChangeToVisibility(RoomHandler.VisibilityStatus.CURRENT, true);
		yield return new WaitForSeconds(1f);
		boss.healthHaver.OverrideKillCamTime = new float?(5f);
		boss.GetComponent<GenericIntroDoer>().TriggerSequence(m_robot);
		yield break;
	}

	// Token: 0x060066AD RID: 26285 RVA: 0x0027F7DC File Offset: 0x0027D9DC
	private IEnumerator LaunchRecenter(Vector2 targetPosition)
	{
		float elapsed = 0f;
		float duration = 0.4f;
		Vector2 startPosition = GameManager.Instance.MainCameraController.OverridePosition;
		while (elapsed < duration)
		{
			elapsed += BraveTime.DeltaTime;
			float t = Mathf.SmoothStep(0f, 1f, elapsed / duration);
			GameManager.Instance.MainCameraController.OverridePosition = Vector2.Lerp(startPosition, targetPosition, t);
			yield return null;
		}
		yield break;
	}

	// Token: 0x060066AE RID: 26286 RVA: 0x0027F7F8 File Offset: 0x0027D9F8
	public void OnBossKilled(Transform bossTransform)
	{
		base.StartCoroutine(this.OnBossKilled_CR(bossTransform));
	}

	// Token: 0x060066AF RID: 26287 RVA: 0x0027F808 File Offset: 0x0027DA08
	private IEnumerator OnBossKilled_CR(Transform bossTransform)
	{
		yield return new WaitForSeconds(2f);
		BossKillCam extantCam = UnityEngine.Object.FindObjectOfType<BossKillCam>();
		if (extantCam)
		{
			extantCam.ForceCancelSequence();
		}
		GameStatsManager.Instance.SetCharacterSpecificFlag(PlayableCharacters.Robot, CharacterSpecificGungeonFlags.KILLED_PAST, true);
		PlayerController m_robot = GameManager.Instance.PrimaryPlayer;
		PastCameraUtility.LockConversation(m_robot.CenterPosition);
		yield return null;
		this.EmperorBot.Interact(m_robot);
		GameManager.Instance.MainCameraController.OverridePosition = this.EmperorSprite.WorldCenter + new Vector2(0f, -3f);
		m_robot.ForceIdleFacePoint(Vector2.down, true);
		while (this.EmperorBot.IsTalking)
		{
			yield return null;
		}
		this.TurnRobotsOff();
		yield return new WaitForSeconds(2f);
		Vector2 idealPlayerPos = this.EmperorBot.transform.position + new Vector3(3.5f, -22f, 0f);
		if (m_robot.transform.position.y < this.EmperorBot.transform.position.y - 6f)
		{
			m_robot.transform.position = idealPlayerPos;
			m_robot.specRigidbody.Reinitialize();
			DeadlyDeadlyGoopManager.DelayedClearGoopsInRadius(m_robot.specRigidbody.UnitCenter, 1f);
		}
		PastCameraUtility.UnlockConversation();
		GameManager.Instance.MainCameraController.SetManualControl(false, false);
		GameManager.Instance.MainCameraController.SetManualControl(true, true);
		GameManager.Instance.MainCameraController.OverridePosition = m_robot.specRigidbody.HitboxPixelCollider.UnitCenter;
		PlayerController[] players = GameManager.Instance.AllPlayers;
		for (int i = 0; i < players.Length; i++)
		{
			players[i].CurrentInputState = PlayerInputState.NoInput;
		}
		for (int j = 0; j < 10; j++)
		{
			AIActor orLoadByGuid = EnemyDatabase.GetOrLoadByGuid(this.CritterIds[UnityEngine.Random.Range(0, this.CritterIds.Length)]);
			AIActor.Spawn(orLoadByGuid, m_robot.CenterPosition.ToIntVector2(VectorConversions.Floor) + new IntVector2(UnityEngine.Random.Range(-5, 5), UnityEngine.Random.Range(-5, 5)), m_robot.CurrentRoom, true, AIActor.AwakenAnimationType.Default, true);
		}
		yield return new WaitForSeconds(3f);
		m_robot.QueueSpecificAnimation("select_choose_long");
		yield return new WaitForSeconds(2f);
		Pixelator.Instance.FreezeFrame();
		BraveTime.RegisterTimeScaleMultiplier(0f, base.gameObject);
		float ela = 0f;
		while (ela < ConvictPastController.FREEZE_FRAME_DURATION)
		{
			ela += GameManager.INVARIANT_DELTA_TIME;
			yield return null;
		}
		BraveTime.ClearMultiplier(base.gameObject);
		PastCameraUtility.LockConversation(GameManager.Instance.PrimaryPlayer.CenterPosition);
		TimeTubeCreditsController ttcc = new TimeTubeCreditsController();
		Pixelator.Instance.FadeToColor(0.15f, Color.white, true, 0.15f);
		ttcc.ClearDebris();
		yield return base.StartCoroutine(ttcc.HandleTimeTubeCredits(GameManager.Instance.PrimaryPlayer.sprite.WorldCenter, false, null, -1, false));
		AmmonomiconController.Instance.OpenAmmonomicon(true, true);
		yield break;
	}

	// Token: 0x060066B0 RID: 26288 RVA: 0x0027F824 File Offset: 0x0027DA24
	private void DistributePoints()
	{
		Vector2 vector = (this.innerRectMin + this.innerRectMax) / 2f;
		for (int i = 0; i < this.validPrefixes.Length; i++)
		{
			this.m_points.Add(new List<Vector2>());
			this.m_ids.Add(new List<int>());
			this.m_activePoints.Add(new List<Vector2>());
			this.m_activeIds.Add(new List<int>());
			this.m_directionalAnimations.Add(new List<string>());
			this.m_directionalOffAnimations.Add(new List<string>());
			for (int j = 0; j < this.directionalAffixes.Length; j++)
			{
				this.m_directionalAnimations[i].Add(this.validPrefixes[i] + this.directionalAffixes[j]);
				this.m_directionalOffAnimations[i].Add(this.validPrefixes[i] + "_off" + this.directionalAffixes[j]);
			}
		}
		List<Vector2> list = new List<Vector2>();
		for (int k = 0; k < 1500; k++)
		{
			Vector2 normalized = UnityEngine.Random.insideUnitCircle.normalized;
			Vector2 vector2 = vector;
			while (BraveMathCollege.AABBContains(this.innerRectMin, this.innerRectMax, vector2))
			{
				vector2 += normalized * UnityEngine.Random.Range(2f, 5f);
			}
			if (!this.excludedRect.Contains(vector2))
			{
				list.Add(vector2);
			}
		}
		for (int l = 0; l < list.Count; l++)
		{
			for (int m = 0; m < list.Count; m++)
			{
				if (l != m)
				{
					float sqrMagnitude = (list[l] - list[m]).sqrMagnitude;
					if (sqrMagnitude < 0.25f)
					{
						list.RemoveAt(m);
						m--;
					}
				}
			}
		}
		for (int n = 0; n < list.Count; n++)
		{
			int num = UnityEngine.Random.Range(0, this.validPrefixes.Length);
			this.m_points[num].Add(list[n]);
			this.m_ids[num].Add(n);
		}
	}

	// Token: 0x060066B1 RID: 26289 RVA: 0x0027FA98 File Offset: 0x0027DC98
	private tk2dSpriteAnimator GetRobotAtPosition(Vector2 point)
	{
		if (this.m_unusedRobots.Count > 0)
		{
			tk2dSpriteAnimator tk2dSpriteAnimator = this.m_unusedRobots[0];
			this.m_unusedRobots.RemoveAt(0);
			tk2dSpriteAnimator.gameObject.SetActive(true);
			tk2dSpriteAnimator.transform.position = point;
			return tk2dSpriteAnimator;
		}
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.RobotPrefab, point, Quaternion.identity);
		return gameObject.GetComponent<tk2dSpriteAnimator>();
	}

	// Token: 0x060066B2 RID: 26290 RVA: 0x0027FB0C File Offset: 0x0027DD0C
	public void TurnRobotsOff()
	{
		this.RobotsOff = true;
		base.StartCoroutine(this.TurnRobotsOffCR());
	}

	// Token: 0x060066B3 RID: 26291 RVA: 0x0027FB24 File Offset: 0x0027DD24
	private IEnumerator TurnRobotsOffCR()
	{
		yield return null;
		this.EmperorBot.aiAnimator.PlayUntilCancelled("EMP_eye_off", false, null, -1f, false);
		this.EmperorBot.transform.Find("pet").GetComponent<tk2dSpriteAnimator>().Play("EMP_pet_off");
		while (this.EmperorBot.spriteAnimator.IsPlaying("EMP_eye_off"))
		{
			yield return null;
		}
		this.EmperorBot.sprite.renderer.enabled = false;
		yield break;
	}

	// Token: 0x060066B4 RID: 26292 RVA: 0x0027FB40 File Offset: 0x0027DD40
	private void LateUpdate()
	{
		if (!this.RobotsOff)
		{
			this.m_centerPoint = GameManager.Instance.PrimaryPlayer.CenterPosition;
		}
		Vector2 vector = GameManager.Instance.MainCameraController.MinVisiblePoint + new Vector2(-2f, -2f);
		Vector2 vector2 = GameManager.Instance.MainCameraController.MaxVisiblePoint + new Vector2(2f, 2f);
		for (int i = 0; i < this.m_points.Count; i++)
		{
			for (int j = 0; j < this.m_activePoints[i].Count; j++)
			{
				int num = this.m_activeIds[i][j];
				Vector2 vector3 = this.m_activePoints[i][j];
				if (vector.x > vector3.x || vector.y > vector3.y || vector2.x < vector3.x || vector2.y < vector3.y)
				{
					tk2dSpriteAnimator tk2dSpriteAnimator = this.m_extantRobots[num];
					tk2dSpriteAnimator.gameObject.SetActive(false);
					this.m_unusedRobots.Add(tk2dSpriteAnimator);
					this.m_extantRobots.Remove(num);
					this.m_activePoints[i].RemoveAt(j);
					this.m_activeIds[i].RemoveAt(j);
					j--;
				}
				else if (Mathf.FloorToInt(Time.realtimeSinceStartup) % this.m_points.Count == i && Time.frameCount % this.m_activePoints[i].Count == j)
				{
					int num2 = BraveMathCollege.VectorToSextant(vector3 - this.m_centerPoint);
					string text = ((!this.RobotsOff) ? this.m_directionalAnimations[i][num2] : this.m_directionalOffAnimations[i][num2]);
					tk2dSpriteAnimator tk2dSpriteAnimator2 = this.m_extantRobots[num];
					if (!tk2dSpriteAnimator2.IsPlaying(text) && !this.m_offPoints.Contains(num))
					{
						if (this.RobotsOff)
						{
							this.m_offPoints.Add(num);
						}
						tk2dSpriteAnimator2.Play(text);
					}
				}
			}
			for (int k = 0; k < this.m_points[i].Count; k++)
			{
				int num3 = this.m_ids[i][k];
				Vector2 vector4 = this.m_points[i][k];
				if (!this.m_extantRobots.ContainsKey(num3))
				{
					if (vector.x < vector4.x && vector.y < vector4.y && vector2.x > vector4.x && vector2.y > vector4.y)
					{
						tk2dSpriteAnimator robotAtPosition = this.GetRobotAtPosition(vector4);
						this.m_extantRobots.Add(num3, robotAtPosition);
						this.m_activePoints[i].Add(vector4);
						this.m_activeIds[i].Add(num3);
						int num4 = BraveMathCollege.VectorToSextant(vector4 - this.m_centerPoint);
						if (!this.m_offPoints.Contains(num3))
						{
							robotAtPosition.Play((!this.RobotsOff) ? this.m_directionalAnimations[i][num4] : this.m_directionalOffAnimations[i][num4]);
							if (this.RobotsOff)
							{
								this.m_offPoints.Add(num3);
							}
						}
						else if (!robotAtPosition.IsPlaying(this.m_directionalOffAnimations[i][num4]))
						{
							robotAtPosition.Stop();
							robotAtPosition.sprite.SetSprite(robotAtPosition.GetClipByName(this.m_directionalOffAnimations[i][num4]).GetFrame(robotAtPosition.GetClipByName(this.m_directionalOffAnimations[i][num4]).frames.Length - 1).spriteId);
						}
						float num5 = BraveMathCollege.DistToRectangle(vector4, this.innerRectMin, this.innerRectMax - this.innerRectMin) * 1.5f;
						num5 -= UnityEngine.Random.value;
						int num6 = Mathf.Max(Mathf.Min(Mathf.FloorToInt(num5), 4), 0);
						robotAtPosition.sprite.usesOverrideMaterial = true;
						robotAtPosition.renderer.material = this.m_fadeMaterials[num6];
						robotAtPosition.sprite.UpdateZDepth();
					}
				}
			}
		}
	}

	// Token: 0x0400626E RID: 25198
	public bool InstantBossFight;

	// Token: 0x0400626F RID: 25199
	public bool DoWavesOfEnemies;

	// Token: 0x04006270 RID: 25200
	public TalkDoerLite WelcomeBot;

	// Token: 0x04006271 RID: 25201
	public TalkDoerLite EmperorBot;

	// Token: 0x04006272 RID: 25202
	public string[] validPrefixes;

	// Token: 0x04006273 RID: 25203
	public string[] directionalAffixes;

	// Token: 0x04006274 RID: 25204
	public GameObject RobotPrefab;

	// Token: 0x04006275 RID: 25205
	public GameObject WarpVFX;

	// Token: 0x04006276 RID: 25206
	public Vector2 outerRectMin;

	// Token: 0x04006277 RID: 25207
	public Vector2 outerRectMax;

	// Token: 0x04006278 RID: 25208
	public Vector2 innerRectMin;

	// Token: 0x04006279 RID: 25209
	public Vector2 innerRectMax;

	// Token: 0x0400627A RID: 25210
	public tk2dSprite EmperorSprite;

	// Token: 0x0400627B RID: 25211
	public Rect excludedRect;

	// Token: 0x0400627C RID: 25212
	[EnemyIdentifier]
	public string[] CritterIds;

	// Token: 0x0400627D RID: 25213
	[NonSerialized]
	public List<Vector2> m_cachedPositions = new List<Vector2>();

	// Token: 0x0400627E RID: 25214
	private List<List<Vector2>> m_points = new List<List<Vector2>>();

	// Token: 0x0400627F RID: 25215
	private List<List<int>> m_ids = new List<List<int>>();

	// Token: 0x04006280 RID: 25216
	private List<List<Vector2>> m_activePoints = new List<List<Vector2>>();

	// Token: 0x04006281 RID: 25217
	private List<List<int>> m_activeIds = new List<List<int>>();

	// Token: 0x04006282 RID: 25218
	private Dictionary<int, tk2dSpriteAnimator> m_extantRobots = new Dictionary<int, tk2dSpriteAnimator>();

	// Token: 0x04006283 RID: 25219
	private List<tk2dSpriteAnimator> m_unusedRobots = new List<tk2dSpriteAnimator>();

	// Token: 0x04006284 RID: 25220
	private List<List<string>> m_directionalAnimations = new List<List<string>>();

	// Token: 0x04006285 RID: 25221
	private List<List<string>> m_directionalOffAnimations = new List<List<string>>();

	// Token: 0x04006286 RID: 25222
	private List<Material> m_fadeMaterials = new List<Material>();

	// Token: 0x04006287 RID: 25223
	[NonSerialized]
	private bool RobotsOff;

	// Token: 0x04006288 RID: 25224
	private List<int> m_offPoints = new List<int>();

	// Token: 0x04006289 RID: 25225
	private Vector2 m_centerPoint;
}
