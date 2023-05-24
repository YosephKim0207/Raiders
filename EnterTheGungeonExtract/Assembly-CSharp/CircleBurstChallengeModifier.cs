using System;
using System.Collections;
using System.Collections.Generic;
using Brave.BulletScript;
using Dungeonator;
using Pathfinding;
using UnityEngine;

// Token: 0x0200126C RID: 4716
public class CircleBurstChallengeModifier : ChallengeModifier
{
	// Token: 0x060069A5 RID: 27045 RVA: 0x002966F8 File Offset: 0x002948F8
	public override bool IsValid(RoomHandler room)
	{
		return room.Cells.Count >= 150 && !room.area.IsProceduralRoom && base.IsValid(room);
	}

	// Token: 0x060069A6 RID: 27046 RVA: 0x00296728 File Offset: 0x00294928
	private IEnumerator Start()
	{
		this.m_room = GameManager.Instance.PrimaryPlayer.CurrentRoom;
		this.m_waveTimer = this.StartDelay;
		yield return null;
		if (ChallengeManager.Instance)
		{
			for (int i = 0; i < ChallengeManager.Instance.ActiveChallenges.Count; i++)
			{
				if (ChallengeManager.Instance.ActiveChallenges[i] is FloorShockwaveChallengeModifier)
				{
					float num = this.TimeBetweenWaves;
					if (!this.Preprocessed)
					{
						FloorShockwaveChallengeModifier floorShockwaveChallengeModifier = ChallengeManager.Instance.ActiveChallenges[i] as FloorShockwaveChallengeModifier;
						float num2 = Mathf.Max(floorShockwaveChallengeModifier.TimeBetweenGaze, this.TimeBetweenWaves);
						num = num2 * 1.25f;
						this.TimeBetweenWaves = num;
						floorShockwaveChallengeModifier.TimeBetweenGaze = num;
						this.Preprocessed = true;
						floorShockwaveChallengeModifier.Preprocessed = true;
					}
					this.m_waveTimer = num * 0.75f;
				}
			}
		}
		yield break;
	}

	// Token: 0x060069A7 RID: 27047 RVA: 0x00296744 File Offset: 0x00294944
	public static IntVector2? GetAppropriateSpawnPointForChallengeBurst(RoomHandler room, float tooCloseRadius, float tooFarRadius)
	{
		CellValidator cellValidator = delegate(IntVector2 c)
		{
			for (int i = 0; i < GameManager.Instance.AllPlayers.Length; i++)
			{
				float num = Vector2.Distance(c.ToCenterVector2(), GameManager.Instance.AllPlayers[i].CenterPosition);
				if (num < tooCloseRadius || num > tooFarRadius)
				{
					return false;
				}
			}
			return true;
		};
		return room.GetRandomAvailableCell(new IntVector2?(IntVector2.One), new CellTypes?(CellTypes.FLOOR | CellTypes.PIT), true, cellValidator);
	}

	// Token: 0x060069A8 RID: 27048 RVA: 0x0029678C File Offset: 0x0029498C
	private void Update()
	{
		this.m_waveTimer -= BraveTime.DeltaTime;
		if (this.m_waveTimer <= 0f)
		{
			this.Cleanup();
			IntVector2? appropriateSpawnPointForChallengeBurst = CircleBurstChallengeModifier.GetAppropriateSpawnPointForChallengeBurst(this.m_room, this.NearRadius, this.FarRadius);
			if (appropriateSpawnPointForChallengeBurst != null)
			{
				this.m_waveTimer = this.TimeBetweenWaves;
				this.SpawnBulletScript(null, this.BulletScript, base.GetComponent<AIBulletBank>(), appropriateSpawnPointForChallengeBurst.Value.ToCenterVector2(), StringTableManager.GetEnemiesString("#TRAP", -1));
			}
		}
	}

	// Token: 0x060069A9 RID: 27049 RVA: 0x00296820 File Offset: 0x00294A20
	private void OnDestroy()
	{
		this.Cleanup();
	}

	// Token: 0x060069AA RID: 27050 RVA: 0x00296828 File Offset: 0x00294A28
	private void Cleanup()
	{
		for (int i = this.m_clms.Count - 1; i >= 0; i--)
		{
			ChainLightningModifier chainLightningModifier = this.m_clms[i];
			if (chainLightningModifier)
			{
				chainLightningModifier.ForcedLinkProjectile = null;
				if (chainLightningModifier.projectile)
				{
					chainLightningModifier.projectile.ForceDestruction();
				}
			}
		}
		this.m_clms.Clear();
	}

	// Token: 0x060069AB RID: 27051 RVA: 0x00296898 File Offset: 0x00294A98
	private void SpawnBulletScript(AIActor aiActor, BulletScriptSelector bulletScript, AIBulletBank bank, Vector2 position, string ownerName)
	{
		base.StartCoroutine(this.HandleSpawnBulletScript(aiActor, bulletScript, bank, position, ownerName));
	}

	// Token: 0x060069AC RID: 27052 RVA: 0x002968B0 File Offset: 0x00294AB0
	private IEnumerator HandleSpawnBulletScript(AIActor aiActor, BulletScriptSelector bulletScript, AIBulletBank bank, Vector2 position, string ownerName)
	{
		this.m_firstCLM = null;
		this.m_lastCLM = null;
		if (this.tellVFX)
		{
			GameObject instanceVFX = SpawnManager.SpawnVFX(this.tellVFX, position, Quaternion.identity);
			tk2dBaseSprite instanceSprite = instanceVFX.GetComponent<tk2dBaseSprite>();
			while (instanceVFX && instanceVFX.activeSelf)
			{
				if (instanceSprite)
				{
					instanceSprite.PlaceAtPositionByAnchor(position, tk2dBaseSprite.Anchor.MiddleCenter);
				}
				yield return null;
			}
		}
		this.m_firstCLM = null;
		this.m_lastCLM = null;
		SpawnManager.SpawnBulletScript(aiActor, position, bank, bulletScript, ownerName, null, null, false, new Action<Bullet, Projectile>(this.OnBulletCreated));
		this.m_firstCLM.ForcedLinkProjectile = this.m_lastCLM.projectile;
		this.m_lastCLM.BackLinkProjectile = this.m_firstCLM.projectile;
		yield break;
	}

	// Token: 0x060069AD RID: 27053 RVA: 0x002968F0 File Offset: 0x00294AF0
	private void OnBulletCreated(Bullet b, Projectile p)
	{
		ChainLightningModifier orAddComponent = p.gameObject.GetOrAddComponent<ChainLightningModifier>();
		orAddComponent.DamagesPlayers = true;
		orAddComponent.DamagesEnemies = false;
		orAddComponent.RequiresSameProjectileClass = true;
		orAddComponent.LinkVFXPrefab = this.ChainLightningVFX;
		orAddComponent.damageTypes = CoreDamageTypes.Electric;
		orAddComponent.maximumLinkDistance = 100f;
		orAddComponent.damagePerHit = 0.5f;
		orAddComponent.damageCooldown = 1f;
		orAddComponent.UsesDispersalParticles = false;
		orAddComponent.UseForcedLinkProjectile = true;
		if (this.m_lastCLM != null)
		{
			orAddComponent.ForcedLinkProjectile = this.m_lastCLM.projectile;
			this.m_lastCLM.BackLinkProjectile = orAddComponent.projectile;
		}
		if (this.m_firstCLM == null)
		{
			this.m_firstCLM = orAddComponent;
		}
		this.m_lastCLM = orAddComponent;
		this.m_clms.Add(orAddComponent);
	}

	// Token: 0x04006618 RID: 26136
	public BulletScriptSelector BulletScript;

	// Token: 0x04006619 RID: 26137
	public GameObject tellVFX;

	// Token: 0x0400661A RID: 26138
	public GameObject ChainLightningVFX;

	// Token: 0x0400661B RID: 26139
	public float NearRadius = 5f;

	// Token: 0x0400661C RID: 26140
	public float FarRadius = 9f;

	// Token: 0x0400661D RID: 26141
	public float StartDelay = 3f;

	// Token: 0x0400661E RID: 26142
	public float TimeBetweenWaves = 10f;

	// Token: 0x0400661F RID: 26143
	private RoomHandler m_room;

	// Token: 0x04006620 RID: 26144
	[NonSerialized]
	public bool Preprocessed;

	// Token: 0x04006621 RID: 26145
	private float m_waveTimer = 5f;

	// Token: 0x04006622 RID: 26146
	private List<ChainLightningModifier> m_clms = new List<ChainLightningModifier>();

	// Token: 0x04006623 RID: 26147
	private ChainLightningModifier m_firstCLM;

	// Token: 0x04006624 RID: 26148
	private ChainLightningModifier m_lastCLM;
}
