using System;
using Dungeonator;

// Token: 0x020010CC RID: 4300
public class TrapEnemyConfigurator : BraveBehaviour
{
	// Token: 0x06005EB1 RID: 24241 RVA: 0x00245F00 File Offset: 0x00244100
	private void Start()
	{
		this.m_parentRoom = base.transform.position.GetAbsoluteRoom();
		this.m_parentRoom.Entered += this.Activate;
	}

	// Token: 0x06005EB2 RID: 24242 RVA: 0x00245F30 File Offset: 0x00244130
	private void Activate(PlayerController p)
	{
		if (!this.m_isActive)
		{
			this.m_isActive = true;
			base.behaviorSpeculator.enabled = true;
		}
	}

	// Token: 0x06005EB3 RID: 24243 RVA: 0x00245F50 File Offset: 0x00244150
	private void Update()
	{
		if (this.m_isActive && !GameManager.Instance.IsAnyPlayerInRoom(this.m_parentRoom))
		{
			this.m_isActive = false;
			base.behaviorSpeculator.enabled = false;
		}
	}

	// Token: 0x040058E7 RID: 22759
	private bool m_isActive;

	// Token: 0x040058E8 RID: 22760
	private RoomHandler m_parentRoom;
}
