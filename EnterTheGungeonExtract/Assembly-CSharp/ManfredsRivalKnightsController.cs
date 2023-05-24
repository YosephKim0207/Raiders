using System;
using System.Collections.Generic;
using Dungeonator;

// Token: 0x0200105D RID: 4189
public class ManfredsRivalKnightsController : BraveBehaviour
{
	// Token: 0x06005C19 RID: 23577 RVA: 0x00234D94 File Offset: 0x00232F94
	public void Start()
	{
		List<AIActor> activeEnemies = base.aiActor.ParentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
		for (int i = 0; i < activeEnemies.Count; i++)
		{
			if (!(activeEnemies[i] == base.aiActor))
			{
				activeEnemies[i].behaviorSpeculator.enabled = false;
				this.m_knights.Add(activeEnemies[i]);
			}
		}
	}

	// Token: 0x06005C1A RID: 23578 RVA: 0x00234E0C File Offset: 0x0023300C
	public void Update()
	{
		for (int i = 0; i < this.m_knights.Count; i++)
		{
			if (!(this.m_knights[i] == null))
			{
				if (!this.m_knights[i] || !this.m_knights[i].healthHaver || this.m_knights[i].healthHaver.IsDead)
				{
					this.m_activeKnights++;
					this.m_knights[i] = null;
				}
				else if (this.m_knights[i].healthHaver.GetCurrentHealthPercentage() < 1f)
				{
					this.ActivateKnight(i);
				}
				else
				{
					this.m_knights[i].aiAnimator.LockFacingDirection = true;
					this.m_knights[i].aiAnimator.FacingDirection = -90f;
				}
			}
		}
		for (int j = 0; j < this.HealthThresholds.Length; j++)
		{
			if (base.healthHaver.GetCurrentHealthPercentage() < this.HealthThresholds[j] && this.m_activeKnights <= j)
			{
				this.ActivateKnight(-1);
			}
		}
	}

	// Token: 0x06005C1B RID: 23579 RVA: 0x00234F60 File Offset: 0x00233160
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x06005C1C RID: 23580 RVA: 0x00234F68 File Offset: 0x00233168
	private void ActivateKnight(int index = -1)
	{
		if (index == -1)
		{
			index = 0;
			while (index < this.m_knights.Count && this.m_knights[index] == null)
			{
				index++;
			}
		}
		if (index < 0 || index >= this.m_knights.Count)
		{
			return;
		}
		this.m_knights[index].behaviorSpeculator.enabled = true;
		this.m_knights[index].aiAnimator.LockFacingDirection = false;
		this.m_knights[index].aiActor.State = AIActor.ActorState.Normal;
		this.m_activeKnights++;
		this.m_knights[index] = null;
	}

	// Token: 0x040055C4 RID: 21956
	public float[] HealthThresholds = new float[] { 0.8f, 0.6f };

	// Token: 0x040055C5 RID: 21957
	private List<AIActor> m_knights = new List<AIActor>();

	// Token: 0x040055C6 RID: 21958
	private int m_activeKnights;
}
