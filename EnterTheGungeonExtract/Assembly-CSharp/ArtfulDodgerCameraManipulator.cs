using System;
using System.Collections;
using Dungeonator;

// Token: 0x020010F3 RID: 4339
public class ArtfulDodgerCameraManipulator : DungeonPlaceableBehaviour, IEventTriggerable, IPlaceConfigurable
{
	// Token: 0x06005F9E RID: 24478 RVA: 0x0024D13C File Offset: 0x0024B33C
	private IEnumerator Start()
	{
		yield return null;
		this.m_dodgerRoom = this.m_room.GetComponentsAbsoluteInRoom<ArtfulDodgerRoomController>()[0];
		this.m_dodgerRoom.RegisterCameraZone(this);
		yield break;
	}

	// Token: 0x06005F9F RID: 24479 RVA: 0x0024D158 File Offset: 0x0024B358
	public void Trigger(int index)
	{
		if (!this.m_dodgerRoom.Completed && this.Active)
		{
			this.m_triggeredFrame = true;
		}
	}

	// Token: 0x06005FA0 RID: 24480 RVA: 0x0024D17C File Offset: 0x0024B37C
	public void LateUpdate()
	{
		if (!this.m_triggeredFrame)
		{
			if (this.m_triggered)
			{
				this.m_triggered = false;
				Minimap.Instance.TemporarilyPreventMinimap = false;
				GameManager.Instance.MainCameraController.SetManualControl(false, true);
				GameManager.Instance.MainCameraController.OverrideZoomScale = 1f;
			}
		}
		else if (!this.m_triggered)
		{
			this.m_triggered = true;
			Minimap.Instance.TemporarilyPreventMinimap = true;
			GameManager.Instance.MainCameraController.OverridePosition = base.transform.position.XY();
			GameManager.Instance.MainCameraController.SetManualControl(true, true);
			GameManager.Instance.MainCameraController.OverrideZoomScale = this.OverrideZoomScale;
		}
		this.m_triggeredFrame = false;
	}

	// Token: 0x06005FA1 RID: 24481 RVA: 0x0024D24C File Offset: 0x0024B44C
	public void ConfigureOnPlacement(RoomHandler room)
	{
		this.m_room = room;
	}

	// Token: 0x06005FA2 RID: 24482 RVA: 0x0024D258 File Offset: 0x0024B458
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x04005A25 RID: 23077
	[DwarfConfigurable]
	public float OverrideZoomScale = 0.75f;

	// Token: 0x04005A26 RID: 23078
	[NonSerialized]
	public bool Active;

	// Token: 0x04005A27 RID: 23079
	private bool m_triggered;

	// Token: 0x04005A28 RID: 23080
	private bool m_triggeredFrame;

	// Token: 0x04005A29 RID: 23081
	private ArtfulDodgerRoomController m_dodgerRoom;

	// Token: 0x04005A2A RID: 23082
	protected RoomHandler m_room;
}
