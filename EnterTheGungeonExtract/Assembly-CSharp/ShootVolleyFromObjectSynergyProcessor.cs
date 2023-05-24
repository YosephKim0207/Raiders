using System;
using UnityEngine;

// Token: 0x0200170C RID: 5900
public class ShootVolleyFromObjectSynergyProcessor : MonoBehaviour
{
	// Token: 0x06008927 RID: 35111 RVA: 0x0038E624 File Offset: 0x0038C824
	private void Awake()
	{
		this.m_cooldown = this.cooldown;
	}

	// Token: 0x06008928 RID: 35112 RVA: 0x0038E634 File Offset: 0x0038C834
	private void Start()
	{
		PlayerOrbital component = base.GetComponent<PlayerOrbital>();
		if (component)
		{
			this.m_player = component.Owner;
		}
		if (!this.m_player)
		{
			this.m_player = base.GetComponentInParent<PlayerController>();
		}
	}

	// Token: 0x06008929 RID: 35113 RVA: 0x0038E67C File Offset: 0x0038C87C
	private void Update()
	{
		this.m_cooldown -= BraveTime.DeltaTime;
		if (this.m_cooldown <= 0f)
		{
			bool flag = false;
			if (this.trigger == ShootVolleyFromObjectSynergyProcessor.TriggerType.CONTINUOUS)
			{
				this.m_cooldown = this.cooldown;
				flag = true;
			}
			else if (this.trigger == ShootVolleyFromObjectSynergyProcessor.TriggerType.ON_SHOOT)
			{
				flag = this.m_player && this.m_player.IsFiring;
			}
			if (flag)
			{
				int num = -1;
				bool flag2 = PlayerController.AnyoneHasActiveBonusSynergy(this.RequiredSynergy, out num);
				if (flag2)
				{
					if (this.trigger == ShootVolleyFromObjectSynergyProcessor.TriggerType.ON_SHOOT)
					{
						this.m_cooldown = this.cooldown;
					}
					Vector2 vector = ((!this.optionalShootPoint) ? base.transform.position.XY() : this.optionalShootPoint.position.XY());
					bool flag3 = false;
					Vector2 vector2 = Vector2.up;
					if (this.usePlayerAim)
					{
						flag3 = true;
						Vector2 vector3 = this.m_player.unadjustedAimPoint.XY();
						if (!BraveInput.GetInstanceForPlayer(this.m_player.PlayerIDX).IsKeyboardAndMouse(false) && this.m_player.CurrentGun)
						{
							vector3 = this.m_player.CenterPosition + BraveMathCollege.DegreesToVector(this.m_player.CurrentGun.CurrentAngle, 10f);
						}
						vector2 = vector3 - vector;
					}
					else
					{
						float num2 = -1f;
						AIActor nearestEnemy = vector.GetAbsoluteRoom().GetNearestEnemy(vector, out num2, true, false);
						if (nearestEnemy)
						{
							vector2 = nearestEnemy.CenterPosition - vector;
							flag3 = num2 < this.maxRange;
						}
					}
					if (flag3)
					{
						if (this.volley)
						{
							VolleyUtility.FireVolley(this.volley, vector, vector2, GameManager.Instance.BestActivePlayer, false);
						}
						else
						{
							VolleyUtility.ShootSingleProjectile(this.singleModule, null, vector, vector2.ToAngle(), 0f, GameManager.Instance.BestActivePlayer, false);
						}
					}
				}
			}
		}
	}

	// Token: 0x04008F05 RID: 36613
	[LongNumericEnum]
	public CustomSynergyType RequiredSynergy;

	// Token: 0x04008F06 RID: 36614
	public ShootVolleyFromObjectSynergyProcessor.TriggerType trigger;

	// Token: 0x04008F07 RID: 36615
	public bool usePlayerAim;

	// Token: 0x04008F08 RID: 36616
	public ProjectileModule singleModule;

	// Token: 0x04008F09 RID: 36617
	public ProjectileVolleyData volley;

	// Token: 0x04008F0A RID: 36618
	public float cooldown = 3f;

	// Token: 0x04008F0B RID: 36619
	public float maxRange = 30f;

	// Token: 0x04008F0C RID: 36620
	public Transform optionalShootPoint;

	// Token: 0x04008F0D RID: 36621
	private float m_cooldown;

	// Token: 0x04008F0E RID: 36622
	private PlayerController m_player;

	// Token: 0x0200170D RID: 5901
	public enum TriggerType
	{
		// Token: 0x04008F10 RID: 36624
		CONTINUOUS,
		// Token: 0x04008F11 RID: 36625
		ON_SHOOT
	}
}
