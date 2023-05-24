using System;
using UnityEngine;

// Token: 0x020014C5 RID: 5317
public class SuperhotItem : PassiveItem
{
	// Token: 0x060078DB RID: 30939 RVA: 0x0030547C File Offset: 0x0030367C
	protected override void Update()
	{
		base.Update();
		if (this.m_pickedUp && !GameManager.Instance.IsLoadingLevel && this.m_owner != null && (this.m_owner.CurrentInputState == PlayerInputState.AllInput || this.m_owner.CurrentInputState == PlayerInputState.OnlyMovement) && !this.m_owner.IsFalling && this.m_owner.healthHaver && !this.m_owner.healthHaver.IsDead)
		{
			this.m_active = true;
			float num = Mathf.Clamp01(this.m_owner.specRigidbody.Velocity.magnitude / this.m_owner.stats.MovementSpeed);
			if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER)
			{
				PlayerController otherPlayer = GameManager.Instance.GetOtherPlayer(this.m_owner);
				if (otherPlayer && otherPlayer.specRigidbody)
				{
					num = Mathf.Max(num, Mathf.Clamp01(otherPlayer.specRigidbody.Velocity.magnitude / otherPlayer.stats.MovementSpeed));
				}
			}
			float num2 = Mathf.Lerp(0.01f, 1f, num);
			if (this.m_owner.IsDodgeRolling)
			{
				num2 = 1f;
			}
			BraveTime.SetTimeScaleMultiplier(num2, base.gameObject);
		}
		else if (this.m_active)
		{
			this.m_active = false;
			BraveTime.ClearMultiplier(base.gameObject);
		}
	}

	// Token: 0x060078DC RID: 30940 RVA: 0x00305604 File Offset: 0x00303804
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x04007B21 RID: 31521
	private bool m_active;
}
