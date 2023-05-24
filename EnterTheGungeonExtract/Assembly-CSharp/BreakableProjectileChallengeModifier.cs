using System;
using Dungeonator;
using UnityEngine;

// Token: 0x02001263 RID: 4707
public class BreakableProjectileChallengeModifier : ChallengeModifier
{
	// Token: 0x0600697C RID: 27004 RVA: 0x0029516C File Offset: 0x0029336C
	private void Start()
	{
		this.m_bulletBank = base.GetComponent<AIBulletBank>();
		RoomHandler currentRoom = GameManager.Instance.PrimaryPlayer.CurrentRoom;
		for (int i = 0; i < StaticReferenceManager.AllMinorBreakables.Count; i++)
		{
			MinorBreakable minorBreakable = StaticReferenceManager.AllMinorBreakables[i];
			if (minorBreakable && !minorBreakable.IsBroken && minorBreakable.CenterPoint.GetAbsoluteRoom() == currentRoom && !minorBreakable.IgnoredForPotShotsModifier)
			{
				MinorBreakable minorBreakable2 = minorBreakable;
				minorBreakable2.OnBreakContext = (Action<MinorBreakable>)Delegate.Combine(minorBreakable2.OnBreakContext, new Action<MinorBreakable>(this.HandleBroken));
			}
		}
	}

	// Token: 0x0600697D RID: 27005 RVA: 0x00295210 File Offset: 0x00293410
	private void HandleBroken(MinorBreakable mb)
	{
		if (!this)
		{
			return;
		}
		if (Time.realtimeSinceStartup - GameManager.Instance.BestActivePlayer.RealtimeEnteredCurrentRoom < 3f)
		{
			return;
		}
		if (mb)
		{
			if (this.AimAtPlayer)
			{
				PlayerController activePlayerClosestToPoint = GameManager.Instance.GetActivePlayerClosestToPoint(mb.CenterPoint, false);
				if (activePlayerClosestToPoint && (activePlayerClosestToPoint.CenterPosition - mb.CenterPoint).magnitude > 1f)
				{
					this.FireBullet(mb.CenterPoint, activePlayerClosestToPoint.CenterPosition - mb.CenterPoint);
				}
			}
			else
			{
				this.FireBullet(mb.CenterPoint, UnityEngine.Random.insideUnitCircle.normalized);
			}
		}
	}

	// Token: 0x0600697E RID: 27006 RVA: 0x002952E4 File Offset: 0x002934E4
	private void FireBullet(Vector3 shootPoint, Vector2 direction)
	{
		this.m_bulletBank.CreateProjectileFromBank(shootPoint, BraveMathCollege.Atan2Degrees(direction), "default", null, false, true, false);
	}

	// Token: 0x0600697F RID: 27007 RVA: 0x00295308 File Offset: 0x00293508
	private void OnDestroy()
	{
		if (Dungeon.IsGenerating || !GameManager.HasInstance || GameManager.Instance.IsLoadingLevel || !GameManager.Instance.PrimaryPlayer || GameManager.Instance.PrimaryPlayer.CurrentRoom == null)
		{
			return;
		}
		RoomHandler currentRoom = GameManager.Instance.PrimaryPlayer.CurrentRoom;
		for (int i = 0; i < StaticReferenceManager.AllMinorBreakables.Count; i++)
		{
			MinorBreakable minorBreakable = StaticReferenceManager.AllMinorBreakables[i];
			if (minorBreakable && minorBreakable.CenterPoint.GetAbsoluteRoom() == currentRoom)
			{
				MinorBreakable minorBreakable2 = minorBreakable;
				minorBreakable2.OnBreakContext = (Action<MinorBreakable>)Delegate.Remove(minorBreakable2.OnBreakContext, new Action<MinorBreakable>(this.HandleBroken));
			}
		}
	}

	// Token: 0x040065E4 RID: 26084
	public bool AimAtPlayer = true;

	// Token: 0x040065E5 RID: 26085
	private AIBulletBank m_bulletBank;
}
