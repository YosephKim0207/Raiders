using System;
using Dungeonator;

// Token: 0x02001254 RID: 4692
public class ExitController : DungeonPlaceableBehaviour, IPlaceConfigurable
{
	// Token: 0x06006938 RID: 26936 RVA: 0x00292DD0 File Offset: 0x00290FD0
	private void Start()
	{
		SpeculativeRigidbody componentInChildren = base.GetComponentInChildren<SpeculativeRigidbody>();
		if (componentInChildren)
		{
			SpeculativeRigidbody speculativeRigidbody = componentInChildren;
			speculativeRigidbody.OnRigidbodyCollision = (SpeculativeRigidbody.OnRigidbodyCollisionDelegate)Delegate.Combine(speculativeRigidbody.OnRigidbodyCollision, new SpeculativeRigidbody.OnRigidbodyCollisionDelegate(this.OnRigidbodyCollision));
		}
	}

	// Token: 0x06006939 RID: 26937 RVA: 0x00292E14 File Offset: 0x00291014
	public void ConfigureOnPlacement(RoomHandler room)
	{
		IntVector2 intVector = base.transform.position.IntXY(VectorConversions.Floor);
		for (int i = this.wallClearanceXStart; i < this.wallClearanceWidth + this.wallClearanceXStart; i++)
		{
			for (int j = this.wallClearanceYStart; j < this.wallClearanceHeight + this.wallClearanceYStart; j++)
			{
				IntVector2 intVector2 = intVector + new IntVector2(i, j);
				CellData cellData = GameManager.Instance.Dungeon.data[intVector2];
				cellData.cellVisualData.containsObjectSpaceStamp = true;
				cellData.cellVisualData.containsWallSpaceStamp = true;
				cellData.cellVisualData.shouldIgnoreWallDrawing = true;
			}
		}
	}

	// Token: 0x0600693A RID: 26938 RVA: 0x00292EC8 File Offset: 0x002910C8
	protected virtual void OnRigidbodyCollision(CollisionData rigidbodyCollision)
	{
		PlayerController component = rigidbodyCollision.OtherRigidbody.GetComponent<PlayerController>();
		if (component != null)
		{
			Pixelator.Instance.FadeToBlack(0.5f, false, 0f);
			GameManager.Instance.DelayedLoadNextLevel(0.5f);
		}
	}

	// Token: 0x0600693B RID: 26939 RVA: 0x00292F14 File Offset: 0x00291114
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x04006598 RID: 26008
	public int wallClearanceXStart = 1;

	// Token: 0x04006599 RID: 26009
	public int wallClearanceYStart = 1;

	// Token: 0x0400659A RID: 26010
	public int wallClearanceWidth = 4;

	// Token: 0x0400659B RID: 26011
	public int wallClearanceHeight = 4;
}
