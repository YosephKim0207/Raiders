using System;
using Dungeonator;
using FullInspector;
using UnityEngine;

// Token: 0x02000DC2 RID: 3522
[InspectorDropdownName("Bosses/HighPriest/SimpleMergoBehavior")]
public class HighPriestSimpleMergoBehavior : BasicAttackBehavior
{
	// Token: 0x06004AB4 RID: 19124 RVA: 0x00191EB4 File Offset: 0x001900B4
	public override void Upkeep()
	{
		base.Upkeep();
		base.DecrementTimer(ref this.m_timer, false);
	}

	// Token: 0x06004AB5 RID: 19125 RVA: 0x00191ECC File Offset: 0x001900CC
	public override BehaviorResult Update()
	{
		BehaviorResult behaviorResult = base.Update();
		if (behaviorResult != BehaviorResult.Continue)
		{
			return behaviorResult;
		}
		if (!this.IsReady())
		{
			return BehaviorResult.Continue;
		}
		for (int i = 0; i < this.numShots; i++)
		{
			this.ShootWallBulletScript();
		}
		this.UpdateCooldowns();
		return BehaviorResult.Continue;
	}

	// Token: 0x06004AB6 RID: 19126 RVA: 0x00191F1C File Offset: 0x0019011C
	private void ShootWallBulletScript()
	{
		float num;
		Vector2 vector = this.RandomWallPoint(out num);
		for (int i = 0; i < GameManager.Instance.AllPlayers.Length; i++)
		{
			PlayerController playerController = GameManager.Instance.AllPlayers[i];
			if (!playerController || playerController.healthHaver.IsDead)
			{
				return;
			}
			if (Vector2.Distance(vector, playerController.CenterPosition) < 8f)
			{
				return;
			}
		}
		GameObject gameObject = new GameObject("Mergo wall shoot point");
		BulletScriptSource orAddComponent = gameObject.GetOrAddComponent<BulletScriptSource>();
		gameObject.GetOrAddComponent<BulletSourceKiller>();
		orAddComponent.transform.position = vector;
		orAddComponent.transform.rotation = Quaternion.Euler(0f, 0f, num);
		orAddComponent.BulletManager = this.m_aiActor.bulletBank;
		orAddComponent.BulletScript = this.wallBulletScript;
		orAddComponent.Initialize();
	}

	// Token: 0x06004AB7 RID: 19127 RVA: 0x00192000 File Offset: 0x00190200
	private Vector2 RandomWallPoint(out float rotation)
	{
		float num = 4f;
		CellArea area = this.m_aiActor.ParentRoom.area;
		Vector2 vector = area.basePosition.ToVector2() + new Vector2(0.5f, 1.5f);
		Vector2 vector2 = (area.basePosition + area.dimensions).ToVector2() - new Vector2(0.5f, 0.5f);
		if (BraveUtility.RandomBool())
		{
			if (BraveUtility.RandomBool())
			{
				rotation = -90f;
				return new Vector2(UnityEngine.Random.Range(vector.x + 5f, vector2.x - 5f), vector2.y + num + 2f);
			}
			rotation = 90f;
			return new Vector2(UnityEngine.Random.Range(vector.x + 5f, vector2.x - 5f), vector.y - num);
		}
		else
		{
			if (BraveUtility.RandomBool())
			{
				rotation = 0f;
				return new Vector2(vector.x - num, UnityEngine.Random.Range(vector.y + 5f, vector2.y - 5f));
			}
			rotation = 180f;
			return new Vector2(vector2.x + num, UnityEngine.Random.Range(vector.y + 5f, vector2.y - 5f));
		}
	}

	// Token: 0x04003F9B RID: 16283
	public BulletScriptSelector wallBulletScript;

	// Token: 0x04003F9C RID: 16284
	public int numShots = 2;

	// Token: 0x04003F9D RID: 16285
	private const float c_wallBuffer = 5f;

	// Token: 0x04003F9E RID: 16286
	private float m_timer;

	// Token: 0x04003F9F RID: 16287
	private float m_wallShotTimer;
}
