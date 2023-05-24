using System;
using UnityEngine;

// Token: 0x020012B9 RID: 4793
public class TurboModeController : MonoBehaviour
{
	// Token: 0x06006B3D RID: 27453 RVA: 0x002A24B8 File Offset: 0x002A06B8
	public void Update()
	{
		TurboModeController.sPlayerSpeedMultiplier = this.PlayerSpeedMultiplier;
		TurboModeController.sPlayerRollSpeedMultiplier = this.PlayerRollSpeedMultiplier;
		TurboModeController.sEnemyBulletSpeedMultiplier = this.EnemyBulletSpeedMultiplier;
		TurboModeController.sEnemyMovementSpeedMultiplier = this.EnemyMovementSpeedMultiplier;
		TurboModeController.sEnemyCooldownMultiplier = this.EnemyCooldownMultiplier;
		TurboModeController.sEnemyWakeTimeMultiplier = this.EnemyWakeTimeMultiplier;
		TurboModeController.sEnemyAnimSpeed = this.EnemyAnimSpeed;
	}

	// Token: 0x17000FE7 RID: 4071
	// (get) Token: 0x06006B3E RID: 27454 RVA: 0x002A2514 File Offset: 0x002A0714
	public static bool IsActive
	{
		get
		{
			return GameManager.IsTurboMode;
		}
	}

	// Token: 0x06006B3F RID: 27455 RVA: 0x002A251C File Offset: 0x002A071C
	public static float MaybeModifyEnemyBulletSpeed(float speed)
	{
		if (GameManager.IsTurboMode)
		{
			return speed * TurboModeController.sEnemyBulletSpeedMultiplier;
		}
		return speed;
	}

	// Token: 0x06006B40 RID: 27456 RVA: 0x002A2534 File Offset: 0x002A0734
	public static float MaybeModifyEnemyMovementSpeed(float speed)
	{
		if (GameManager.IsTurboMode)
		{
			return speed * TurboModeController.sEnemyMovementSpeedMultiplier;
		}
		return speed;
	}

	// Token: 0x04006826 RID: 26662
	public static float sPlayerSpeedMultiplier = 1.4f;

	// Token: 0x04006827 RID: 26663
	public static float sPlayerRollSpeedMultiplier = 1.4f;

	// Token: 0x04006828 RID: 26664
	public static float sEnemyBulletSpeedMultiplier = 1.3f;

	// Token: 0x04006829 RID: 26665
	public static float sEnemyMovementSpeedMultiplier = 1.5f;

	// Token: 0x0400682A RID: 26666
	public static float sEnemyCooldownMultiplier = 0.5f;

	// Token: 0x0400682B RID: 26667
	public static float sEnemyWakeTimeMultiplier = 4f;

	// Token: 0x0400682C RID: 26668
	public static float sEnemyAnimSpeed = 1f;

	// Token: 0x0400682D RID: 26669
	public float PlayerSpeedMultiplier = 1.4f;

	// Token: 0x0400682E RID: 26670
	public float PlayerRollSpeedMultiplier = 1.4f;

	// Token: 0x0400682F RID: 26671
	public float EnemyBulletSpeedMultiplier = 1.3f;

	// Token: 0x04006830 RID: 26672
	public float EnemyMovementSpeedMultiplier = 1.5f;

	// Token: 0x04006831 RID: 26673
	public float EnemyCooldownMultiplier = 0.5f;

	// Token: 0x04006832 RID: 26674
	public float EnemyWakeTimeMultiplier = 4f;

	// Token: 0x04006833 RID: 26675
	public float EnemyAnimSpeed = 1f;
}
