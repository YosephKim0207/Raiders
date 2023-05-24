using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020012B7 RID: 4791
public class TrashLauncher : BraveBehaviour
{
	// Token: 0x06006B39 RID: 27449 RVA: 0x002A21AC File Offset: 0x002A03AC
	private void Start()
	{
		this.m_player = base.GetComponentInParent<PlayerController>();
	}

	// Token: 0x06006B3A RID: 27450 RVA: 0x002A21BC File Offset: 0x002A03BC
	private void Update()
	{
		Vector2 worldCenter = base.sprite.WorldCenter;
		for (int i = 0; i < StaticReferenceManager.AllDebris.Count; i++)
		{
			DebrisObject debrisObject = StaticReferenceManager.AllDebris[i];
			if (debrisObject && !debrisObject.IsPickupObject && debrisObject.Priority != EphemeralObject.EphemeralPriority.Critical)
			{
				Vector2 vector = ((!debrisObject.sprite) ? debrisObject.transform.position.XY() : debrisObject.sprite.WorldCenter);
				Vector2 vector2 = worldCenter - vector;
				float sqrMagnitude = vector2.sqrMagnitude;
				TrashLauncher.TrashManipulateMode trashManipulateMode = this.mode;
				if (trashManipulateMode != TrashLauncher.TrashManipulateMode.GATHER_AND_TOSS)
				{
					if (trashManipulateMode == TrashLauncher.TrashManipulateMode.DRAGONBALL_Z)
					{
						if (sqrMagnitude < 100f)
						{
							if (!this.m_debris.Contains(debrisObject))
							{
								this.m_debris.Add(debrisObject);
							}
							if (debrisObject.HasBeenTriggered && debrisObject.UnadjustedDebrisPosition.z < 0.75f)
							{
								debrisObject.IncrementZHeight(this.liftIntensity * BraveTime.DeltaTime);
							}
						}
					}
				}
				else if (sqrMagnitude < 100f)
				{
					if (!this.m_debris.Contains(debrisObject))
					{
						this.m_debris.Add(debrisObject);
					}
					if (debrisObject.HasBeenTriggered)
					{
						debrisObject.ApplyVelocity(vector2.normalized * 25f * debrisObject.inertialMass * BraveTime.DeltaTime);
						debrisObject.PreventFallingInPits = true;
					}
				}
			}
		}
	}

	// Token: 0x06006B3B RID: 27451 RVA: 0x002A2354 File Offset: 0x002A0554
	public void OnDespawned()
	{
		TrashLauncher.TrashManipulateMode trashManipulateMode = this.mode;
		if (trashManipulateMode == TrashLauncher.TrashManipulateMode.GATHER_AND_TOSS)
		{
			Vector2 vector = UnityEngine.Random.insideUnitCircle;
			if (this.m_player)
			{
				vector = this.m_player.unadjustedAimPoint.XY() - this.m_player.CenterPosition;
			}
			vector = vector.normalized;
			foreach (DebrisObject debrisObject in this.m_debris)
			{
				if (debrisObject)
				{
					Vector2 vector2 = Quaternion.Euler(0f, 0f, (float)UnityEngine.Random.Range(-15, 15)) * vector;
					debrisObject.ApplyVelocity(vector2 * (float)UnityEngine.Random.Range(45, 55));
				}
			}
		}
		base.OnDestroy();
	}

	// Token: 0x0400681F RID: 26655
	public TrashLauncher.TrashManipulateMode mode;

	// Token: 0x04006820 RID: 26656
	private HashSet<DebrisObject> m_debris = new HashSet<DebrisObject>();

	// Token: 0x04006821 RID: 26657
	private PlayerController m_player;

	// Token: 0x04006822 RID: 26658
	public float liftIntensity = 2f;

	// Token: 0x020012B8 RID: 4792
	public enum TrashManipulateMode
	{
		// Token: 0x04006824 RID: 26660
		GATHER_AND_TOSS,
		// Token: 0x04006825 RID: 26661
		DRAGONBALL_Z
	}
}
