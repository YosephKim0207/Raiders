using System;
using System.Collections;
using System.Collections.Generic;
using Dungeonator;
using UnityEngine;

// Token: 0x02000FA7 RID: 4007
public class AshManEngageDoer : CustomEngageDoer
{
	// Token: 0x06005739 RID: 22329 RVA: 0x00214360 File Offset: 0x00212560
	public void Awake()
	{
		if (UnityEngine.Random.value > this.FromStatueChance)
		{
			this.m_isFinished = true;
			return;
		}
		base.aiActor.HasDonePlayerEnterCheck = true;
		base.aiActor.CollisionDamage = 0f;
	}

	// Token: 0x0600573A RID: 22330 RVA: 0x00214398 File Offset: 0x00212598
	public override void StartIntro()
	{
		if (this.m_isFinished)
		{
			return;
		}
		List<MinorBreakable> list = new List<MinorBreakable>();
		RoomHandler parentRoom = base.aiActor.ParentRoom;
		List<MinorBreakable> allMinorBreakables = StaticReferenceManager.AllMinorBreakables;
		DungeonData data = GameManager.Instance.Dungeon.data;
		for (int i = 0; i < allMinorBreakables.Count; i++)
		{
			MinorBreakable minorBreakable = allMinorBreakables[i];
			if (minorBreakable.name.StartsWith(this.BreakablePrefix))
			{
				RoomHandler absoluteRoomFromPosition = data.GetAbsoluteRoomFromPosition(minorBreakable.transform.position.IntXY(VectorConversions.Floor));
				if (absoluteRoomFromPosition == parentRoom)
				{
					list.Add(minorBreakable);
				}
			}
		}
		if (list.Count == 0)
		{
			this.m_isFinished = true;
			base.aiActor.invisibleUntilAwaken = false;
			base.aiActor.ToggleRenderers(true);
			base.aiAnimator.PlayDefaultAwakenedState();
			base.aiActor.State = AIActor.ActorState.Normal;
			return;
		}
		base.StartCoroutine(this.DoIntro(BraveUtility.RandomElement<MinorBreakable>(list)));
	}

	// Token: 0x0600573B RID: 22331 RVA: 0x0021449C File Offset: 0x0021269C
	private IEnumerator DoIntro(MinorBreakable breakable)
	{
		base.aiActor.enabled = false;
		base.behaviorSpeculator.enabled = false;
		base.aiActor.ToggleRenderers(false);
		base.specRigidbody.enabled = false;
		base.aiActor.IsGone = true;
		base.specRigidbody.Initialize();
		Vector2 offset = base.specRigidbody.UnitBottomCenter - base.transform.position.XY();
		base.transform.position = breakable.specRigidbody.UnitBottomCenter - offset;
		base.specRigidbody.Reinitialize();
		yield return null;
		base.aiActor.ToggleRenderers(false);
		if (base.aiShooter)
		{
			base.aiShooter.ToggleGunAndHandRenderers(false, "AshManEngageDoer");
		}
		breakable.OnBreak = (Action)Delegate.Combine(breakable.OnBreak, new Action(this.OnBreak));
		float delay = UnityEngine.Random.Range(this.MinSpawnDelay, this.MaxSpawnDelay);
		float timer = 0f;
		while (timer < delay && !this.m_brokeEarly)
		{
			yield return null;
			timer += BraveTime.DeltaTime;
		}
		if (!this.m_brokeEarly)
		{
			breakable.Break();
		}
		base.aiActor.enabled = true;
		base.behaviorSpeculator.enabled = true;
		base.aiActor.ToggleRenderers(true);
		if (base.aiShooter)
		{
			base.aiShooter.ToggleGunAndHandRenderers(true, "AshManEngageDoer");
		}
		base.specRigidbody.enabled = true;
		base.aiActor.IsGone = false;
		base.aiAnimator.PlayDefaultAwakenedState();
		base.aiActor.State = AIActor.ActorState.Normal;
		foreach (PlayerController playerController in GameManager.Instance.AllPlayers)
		{
			if (playerController && Vector2.Distance(playerController.specRigidbody.UnitCenter, base.specRigidbody.UnitCenter) < 8f)
			{
				base.behaviorSpeculator.AttackCooldown = 0.5f;
				break;
			}
		}
		breakable.OnBreak = (Action)Delegate.Remove(breakable.OnBreak, new Action(this.OnBreak));
		this.m_isFinished = true;
		yield return new WaitForSeconds(1f);
		base.aiActor.CollisionDamage = 0.5f;
		yield break;
	}

	// Token: 0x17000C6E RID: 3182
	// (get) Token: 0x0600573C RID: 22332 RVA: 0x002144C0 File Offset: 0x002126C0
	public override bool IsFinished
	{
		get
		{
			return this.m_isFinished;
		}
	}

	// Token: 0x0600573D RID: 22333 RVA: 0x002144C8 File Offset: 0x002126C8
	private void OnBreak()
	{
		this.m_brokeEarly = true;
	}

	// Token: 0x0400501F RID: 20511
	public float FromStatueChance = 0.5f;

	// Token: 0x04005020 RID: 20512
	public string BreakablePrefix = "Forge_Ash_Bullet";

	// Token: 0x04005021 RID: 20513
	public float MinSpawnDelay = 2f;

	// Token: 0x04005022 RID: 20514
	public float MaxSpawnDelay = 6f;

	// Token: 0x04005023 RID: 20515
	private bool m_isFinished;

	// Token: 0x04005024 RID: 20516
	private bool m_brokeEarly;
}
