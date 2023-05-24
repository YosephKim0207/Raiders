using System;
using System.Collections;
using Dungeonator;
using UnityEngine;

// Token: 0x0200127A RID: 4730
public class FloorShockwaveChallengeModifier : ChallengeModifier
{
	// Token: 0x060069EB RID: 27115 RVA: 0x002982B0 File Offset: 0x002964B0
	private IEnumerator Start()
	{
		this.m_room = GameManager.Instance.PrimaryPlayer.CurrentRoom;
		yield return null;
		if (ChallengeManager.Instance)
		{
			for (int i = 0; i < ChallengeManager.Instance.ActiveChallenges.Count; i++)
			{
				if (ChallengeManager.Instance.ActiveChallenges[i] is CircleBurstChallengeModifier)
				{
					float num = this.TimeBetweenGaze;
					if (!this.Preprocessed)
					{
						CircleBurstChallengeModifier circleBurstChallengeModifier = ChallengeManager.Instance.ActiveChallenges[i] as CircleBurstChallengeModifier;
						float num2 = Mathf.Max(this.TimeBetweenGaze, circleBurstChallengeModifier.TimeBetweenWaves);
						num = num2 * 1.25f;
						this.TimeBetweenGaze = num;
						circleBurstChallengeModifier.TimeBetweenWaves = num;
						this.Preprocessed = true;
						circleBurstChallengeModifier.Preprocessed = true;
					}
					this.m_waveTimer = num * 0.25f;
				}
			}
		}
		yield break;
	}

	// Token: 0x060069EC RID: 27116 RVA: 0x002982CC File Offset: 0x002964CC
	private void Update()
	{
		this.m_waveTimer -= BraveTime.DeltaTime;
		if (this.m_waveTimer <= 0f)
		{
			this.m_waveTimer = this.TimeBetweenGaze;
			IntVector2? appropriateSpawnPointForChallengeBurst = CircleBurstChallengeModifier.GetAppropriateSpawnPointForChallengeBurst(this.m_room, this.NearRadius, this.FarRadius);
			if (appropriateSpawnPointForChallengeBurst != null)
			{
				ChallengeManager.Instance.StartCoroutine(this.LaunchWave(appropriateSpawnPointForChallengeBurst.Value.ToCenterVector2()));
			}
		}
	}

	// Token: 0x060069ED RID: 27117 RVA: 0x0029834C File Offset: 0x0029654C
	private IEnumerator LaunchWave(Vector2 startPoint)
	{
		float m_prevWaveDist = 0f;
		float distortionMaxRadius = 20f;
		float distortionDuration = 1.5f;
		float distortionIntensity = 0.5f;
		float distortionThickness = 0.04f;
		GameObject instanceVFX = SpawnManager.SpawnVFX(this.EyesVFX, startPoint.ToVector3ZUp(0f) + new Vector3(-3.1875f, -3f, 0f), Quaternion.identity);
		tk2dSprite instanceSprite = instanceVFX.GetComponent<tk2dSprite>();
		float elapsedTime = 0f;
		while (instanceVFX && instanceVFX.activeSelf)
		{
			elapsedTime += BraveTime.DeltaTime;
			if (instanceSprite)
			{
				instanceSprite.PlaceAtPositionByAnchor(startPoint, tk2dBaseSprite.Anchor.MiddleCenter);
			}
			if (elapsedTime > 0.75f)
			{
				AkSoundEngine.PostEvent("Play_ENM_gorgun_gaze_01", instanceVFX.gameObject);
				elapsedTime -= 1000f;
			}
			yield return null;
		}
		Exploder.DoDistortionWave(startPoint, distortionIntensity, distortionThickness, distortionMaxRadius, distortionDuration);
		float waveRemaining = distortionDuration - BraveTime.DeltaTime;
		while (waveRemaining > 0f)
		{
			waveRemaining -= BraveTime.DeltaTime;
			float waveDist = BraveMathCollege.LinearToSmoothStepInterpolate(0f, distortionMaxRadius, 1f - waveRemaining / distortionDuration);
			for (int i = 0; i < GameManager.Instance.AllPlayers.Length; i++)
			{
				PlayerController playerController = GameManager.Instance.AllPlayers[i];
				if (!playerController.healthHaver.IsDead)
				{
					if (!playerController.spriteAnimator.QueryInvulnerabilityFrame() && playerController.healthHaver.IsVulnerable)
					{
						Vector2 unitCenter = playerController.specRigidbody.GetUnitCenter(ColliderType.HitBox);
						float num = Vector2.Distance(unitCenter, startPoint);
						if (num >= m_prevWaveDist - 0.25f && num <= waveDist + 0.25f)
						{
							float num2 = (unitCenter - startPoint).ToAngle();
							if (BraveMathCollege.AbsAngleBetween(playerController.FacingDirection, num2) >= 60f)
							{
								playerController.CurrentStoneGunTimer = this.StoneDuration;
							}
						}
					}
				}
			}
			m_prevWaveDist = waveDist;
			yield return null;
		}
		yield break;
	}

	// Token: 0x04006665 RID: 26213
	public GameObject EyesVFX;

	// Token: 0x04006666 RID: 26214
	public float NearRadius = 5f;

	// Token: 0x04006667 RID: 26215
	public float FarRadius = 9f;

	// Token: 0x04006668 RID: 26216
	public float StoneDuration = 3.5f;

	// Token: 0x04006669 RID: 26217
	public float TimeBetweenGaze = 8f;

	// Token: 0x0400666A RID: 26218
	[NonSerialized]
	public bool Preprocessed;

	// Token: 0x0400666B RID: 26219
	private RoomHandler m_room;

	// Token: 0x0400666C RID: 26220
	private float m_waveTimer = 5f;
}
