using System;
using FullInspector;
using UnityEngine;

// Token: 0x02000DDC RID: 3548
[InspectorDropdownName("Bosses/DemonWall/MovementBehavior")]
public class DemonWallMovementBehavior : MovementBehaviorBase
{
	// Token: 0x06004B28 RID: 19240 RVA: 0x0019683C File Offset: 0x00194A3C
	public override void Start()
	{
		base.Start();
		this.m_demonWallController = this.m_aiActor.GetComponent<DemonWallController>();
		this.m_updateEveryFrame = true;
	}

	// Token: 0x06004B29 RID: 19241 RVA: 0x0019685C File Offset: 0x00194A5C
	public override BehaviorResult Update()
	{
		if (this.m_deltaTime > 0f && this.m_demonWallController.IsCameraLocked)
		{
			if (!this.m_initialized)
			{
				this.m_startY = this.m_aiActor.specRigidbody.Position.UnitY;
				this.m_startCameraY = this.m_demonWallController.CameraPos.y;
				this.m_initialized = true;
			}
			this.m_timer += this.m_deltaTime;
			float num = this.m_startY - this.m_timer * this.speed;
			float num2 = this.m_startCameraY - this.m_timer * this.speed;
			num += Mathf.Sin(this.m_timer / this.sinPeriod * 3.1415927f) * this.sinMagnitude;
			num = Mathf.Min(this.lowestGoalY, num);
			this.lowestGoalY = num;
			this.m_aiActor.BehaviorOverridesVelocity = true;
			if (this.m_deltaTime > 0f)
			{
				this.m_aiActor.BehaviorVelocity = new Vector2(0f, (num - this.m_aiActor.specRigidbody.Position.UnitY) / this.m_deltaTime);
			}
			this.m_aiActor.specRigidbody.Velocity = this.m_aiActor.BehaviorVelocity;
			CameraController mainCameraController = GameManager.Instance.MainCameraController;
			Vector2 vector = this.m_demonWallController.CameraPos.WithY(num2);
			float num3 = (float)this.m_aiActor.ParentRoom.area.basePosition.y + mainCameraController.Camera.orthographicSize;
			vector.y = Mathf.Max(vector.y, num3);
			mainCameraController.OverridePosition = vector;
		}
		return BehaviorResult.Continue;
	}

	// Token: 0x04004087 RID: 16519
	public float speed = 4f;

	// Token: 0x04004088 RID: 16520
	public float sinPeriod = 2f;

	// Token: 0x04004089 RID: 16521
	public float sinMagnitude = 1f;

	// Token: 0x0400408A RID: 16522
	private DemonWallController m_demonWallController;

	// Token: 0x0400408B RID: 16523
	private bool m_initialized;

	// Token: 0x0400408C RID: 16524
	private float m_startY;

	// Token: 0x0400408D RID: 16525
	private float m_startCameraY;

	// Token: 0x0400408E RID: 16526
	private float lowestGoalY = float.MaxValue;

	// Token: 0x0400408F RID: 16527
	private float m_timer;
}
