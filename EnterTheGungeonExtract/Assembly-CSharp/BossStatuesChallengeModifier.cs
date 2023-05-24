using System;
using Dungeonator;
using UnityEngine;

// Token: 0x02001262 RID: 4706
public class BossStatuesChallengeModifier : ChallengeModifier
{
	// Token: 0x06006979 RID: 27001 RVA: 0x00294FD8 File Offset: 0x002931D8
	private void Start()
	{
		this.m_room = GameManager.Instance.PrimaryPlayer.CurrentRoom;
		this.m_timer = this.InitialDelayTime;
		this.m_manager = DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(this.Goop);
	}

	// Token: 0x0600697A RID: 27002 RVA: 0x0029500C File Offset: 0x0029320C
	private void Update()
	{
		this.m_timer -= BraveTime.DeltaTime;
		if (this.m_timer <= 0f)
		{
			this.m_timer = this.Goop.lifespan + this.AdditionalDelayTime;
			Vector2 vector = new Vector2((float)this.m_room.area.dimensions.x / (1f * (float)this.chessSquaresX), (float)this.m_room.area.dimensions.y / (1f * (float)this.chessSquaresY));
			for (int i = 0; i < this.chessSquaresX; i++)
			{
				for (int j = 0; j < this.chessSquaresY; j++)
				{
					Vector2 vector2 = this.m_room.area.basePosition.ToVector2() + new Vector2(vector.x * (float)i, vector.y * (float)j);
					Vector2 vector3 = vector2 + vector;
					int num = (i + j) % 2;
					if ((num == 1 && this.m_firstQuadrant) || (num == 0 && !this.m_firstQuadrant))
					{
						this.m_manager.TimedAddGoopRect(vector2, vector3, 0.5f);
					}
				}
			}
			this.m_firstQuadrant = !this.m_firstQuadrant;
		}
	}

	// Token: 0x040065DB RID: 26075
	public GoopDefinition Goop;

	// Token: 0x040065DC RID: 26076
	public int chessSquaresX = 4;

	// Token: 0x040065DD RID: 26077
	public int chessSquaresY = 4;

	// Token: 0x040065DE RID: 26078
	public float InitialDelayTime = 4f;

	// Token: 0x040065DF RID: 26079
	public float AdditionalDelayTime = 1.5f;

	// Token: 0x040065E0 RID: 26080
	private RoomHandler m_room;

	// Token: 0x040065E1 RID: 26081
	private DeadlyDeadlyGoopManager m_manager;

	// Token: 0x040065E2 RID: 26082
	private bool m_firstQuadrant;

	// Token: 0x040065E3 RID: 26083
	private float m_timer;
}
