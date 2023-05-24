using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000CE2 RID: 3298
public class TowerBossController : DungeonPlaceableBehaviour
{
	// Token: 0x06004605 RID: 17925 RVA: 0x0016C674 File Offset: 0x0016A874
	private void Start()
	{
		TowerBossBatteryController[] array = (TowerBossBatteryController[])UnityEngine.Object.FindObjectsOfType(typeof(TowerBossBatteryController));
		BraveUtility.Assert(array.Length != 2, "Trying to initialize TowerBoss with more or less than 2 batteries in world.", false);
		if (array[0].transform.position.x < array[1].transform.position.x)
		{
			this.m_batteryLeft = array[0];
			this.m_batteryRight = array[1];
		}
		else
		{
			this.m_batteryLeft = array[1];
			this.m_batteryRight = array[0];
		}
		this.m_batteryLeft.tower = this;
		this.m_batteryRight.tower = this;
		this.m_batteryLeft.linkedIris = this.irisLeft;
		this.m_batteryRight.linkedIris = this.irisRight;
		for (int i = 0; i < base.transform.childCount; i++)
		{
			Transform child = base.transform.GetChild(i);
			MeshRenderer componentInChildren = child.GetComponentInChildren<MeshRenderer>();
			if (child.GetComponent<Renderer>() != null)
			{
				DepthLookupManager.PinRendererToRenderer(componentInChildren, this.towerSprite.GetComponent<MeshRenderer>());
			}
		}
		this.towerSprite.IsPerpendicular = false;
		this.cockpitHealthHaver.IsVulnerable = false;
		this.cockpitHealthHaver.OnDeath += this.Die;
		base.StartCoroutine(this.HandleBatteryCycle());
		base.StartCoroutine(this.HandleBeamCycle());
	}

	// Token: 0x06004606 RID: 17926 RVA: 0x0016C7DC File Offset: 0x0016A9DC
	private void Die(Vector2 lastDirection)
	{
		this.m_alive = false;
	}

	// Token: 0x06004607 RID: 17927 RVA: 0x0016C7E8 File Offset: 0x0016A9E8
	private void Update()
	{
		if (this.m_alive)
		{
			for (int i = 0; i < this.laserEmitters.Length; i++)
			{
				float num = this.laserEmitters[i].currentAngle + this.spinSpeed * BraveTime.DeltaTime * 0.017453292f;
				Vector3 vector = base.transform.position.XY() + this.ellipseCenter;
				float num2 = vector.x + this.ellipseAxes.x * Mathf.Cos(num);
				float num3 = vector.y + this.ellipseAxes.y * Mathf.Sin(num);
				this.laserEmitters[i].transform.position = BraveUtility.QuantizeVector(new Vector3(num2, num3, this.laserEmitters[i].transform.position.z));
				this.laserEmitters[i].UpdateAngle(num % 6.2831855f);
			}
		}
	}

	// Token: 0x06004608 RID: 17928 RVA: 0x0016C8E0 File Offset: 0x0016AAE0
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x06004609 RID: 17929 RVA: 0x0016C8E8 File Offset: 0x0016AAE8
	private IEnumerator HandleBeamCycle()
	{
		while (this.m_alive)
		{
			yield return new WaitForSeconds(5f);
			for (int i = 0; i < this.laserEmitters.Length; i++)
			{
			}
			yield return new WaitForSeconds(5f);
		}
		yield break;
	}

	// Token: 0x0600460A RID: 17930 RVA: 0x0016C904 File Offset: 0x0016AB04
	private IEnumerator HandleBatteryCycle()
	{
		float left_elapsed = 0f;
		float right_elapsed = 0f;
		float cycleLength = this.m_batteryLeft.cycleTime;
		this.m_batteryLeft.IsVulnerable = true;
		while (this.m_alive)
		{
			if (this.m_batteryLeft.linkedIris.fuseAlive && !this.m_batteryLeft.linkedIris.IsOpen)
			{
				left_elapsed += BraveTime.DeltaTime;
				if (left_elapsed > cycleLength)
				{
					left_elapsed -= cycleLength;
					this.m_batteryLeft.IsVulnerable = !this.m_batteryLeft.IsVulnerable;
				}
			}
			else
			{
				this.m_batteryLeft.IsVulnerable = false;
			}
			if (this.m_batteryRight.linkedIris.fuseAlive && !this.m_batteryRight.linkedIris.IsOpen)
			{
				right_elapsed += BraveTime.DeltaTime;
				if (right_elapsed > cycleLength)
				{
					right_elapsed -= cycleLength;
					this.m_batteryRight.IsVulnerable = !this.m_batteryRight.IsVulnerable;
				}
			}
			else
			{
				this.m_batteryRight.IsVulnerable = false;
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600460B RID: 17931 RVA: 0x0016C920 File Offset: 0x0016AB20
	private void PhaseTransition()
	{
		this.m_batteryLeft.linkedIris = this.irisRight;
		this.m_batteryRight.linkedIris = this.irisLeft;
		this.irisLeft.fuseAlive = true;
		this.irisRight.fuseAlive = true;
		this.currentPhase = TowerBossController.TowerBossPhase.PHASE_TWO;
	}

	// Token: 0x0600460C RID: 17932 RVA: 0x0016C970 File Offset: 0x0016AB70
	public void NotifyFuseDestruction(TowerBossIrisController source)
	{
		if (!this.irisLeft.fuseAlive && !this.irisRight.fuseAlive)
		{
			this.cockpitHealthHaver.IsVulnerable = true;
			GameManager.Instance.Dungeon.GetRoomFromPosition(base.transform.position.IntXY(VectorConversions.Floor)).HandleRoomAction(RoomEventTriggerAction.UNSEAL_ROOM);
		}
	}

	// Token: 0x04003889 RID: 14473
	public HealthHaver cockpitHealthHaver;

	// Token: 0x0400388A RID: 14474
	public tk2dSprite towerSprite;

	// Token: 0x0400388B RID: 14475
	public TowerBossIrisController irisLeft;

	// Token: 0x0400388C RID: 14476
	public TowerBossIrisController irisRight;

	// Token: 0x0400388D RID: 14477
	public TowerBossEmitterController[] laserEmitters;

	// Token: 0x0400388E RID: 14478
	public Vector2 ellipseCenter;

	// Token: 0x0400388F RID: 14479
	public Vector2 ellipseAxes;

	// Token: 0x04003890 RID: 14480
	public Projectile beamProjectile;

	// Token: 0x04003891 RID: 14481
	public float spinSpeed = 60f;

	// Token: 0x04003892 RID: 14482
	private TowerBossBatteryController m_batteryLeft;

	// Token: 0x04003893 RID: 14483
	private TowerBossBatteryController m_batteryRight;

	// Token: 0x04003894 RID: 14484
	private bool m_alive = true;

	// Token: 0x04003895 RID: 14485
	public TowerBossController.TowerBossPhase currentPhase;

	// Token: 0x02000CE3 RID: 3299
	public enum TowerBossPhase
	{
		// Token: 0x04003897 RID: 14487
		PHASE_ONE,
		// Token: 0x04003898 RID: 14488
		PHASE_TWO
	}
}
