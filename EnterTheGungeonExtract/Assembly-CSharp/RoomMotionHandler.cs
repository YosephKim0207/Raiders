using System;
using System.Collections;
using Dungeonator;
using UnityEngine;

// Token: 0x02000F2F RID: 3887
public class RoomMotionHandler : MonoBehaviour
{
	// Token: 0x0600535C RID: 21340 RVA: 0x001E5E58 File Offset: 0x001E4058
	public void Initialize(RoomHandler parentRoom)
	{
		this.m_transform = base.transform;
		this.m_zOffset = this.m_transform.position.z - this.m_transform.position.y;
		this.currentCellPosition = parentRoom.area.basePosition;
	}

	// Token: 0x0600535D RID: 21341 RVA: 0x001E5EB0 File Offset: 0x001E40B0
	public void TriggerMoveTo(IntVector2 targetPosition)
	{
		if (this.m_isMoving)
		{
			return;
		}
		if (targetPosition == this.currentCellPosition)
		{
			return;
		}
		base.StartCoroutine(this.HandleMove(targetPosition));
	}

	// Token: 0x0600535E RID: 21342 RVA: 0x001E5EE0 File Offset: 0x001E40E0
	private IEnumerator HandleMove(IntVector2 targetPosition)
	{
		this.m_isMoving = true;
		IntVector2 startPosition = this.currentCellPosition;
		IntVector2 movementVector = targetPosition - startPosition;
		Vector3 worldStartPosition = this.m_transform.position;
		Vector3 worldEndPosition = this.m_transform.position + movementVector.ToVector3();
		worldEndPosition = worldEndPosition.WithZ(worldEndPosition.y + this.m_zOffset);
		float distanceToTravel = (float)IntVector2.ManhattanDistance(startPosition, targetPosition);
		float timeToTravel = distanceToTravel / this.c_roomSpeed;
		float elapsed = 0f;
		while (elapsed < timeToTravel)
		{
			elapsed += BraveTime.DeltaTime;
			float t = Mathf.SmoothStep(0f, 1f, Mathf.Clamp01(elapsed / timeToTravel));
			this.m_transform.position = Vector3.Lerp(worldStartPosition, worldEndPosition, t);
			this.currentCellPosition = this.m_transform.position.IntXY(VectorConversions.Floor);
			yield return null;
		}
		this.m_transform.position = worldEndPosition;
		this.currentCellPosition = targetPosition;
		this.m_isMoving = false;
		yield break;
	}

	// Token: 0x04004BBE RID: 19390
	private float c_roomSpeed = 3f;

	// Token: 0x04004BBF RID: 19391
	private Transform m_transform;

	// Token: 0x04004BC0 RID: 19392
	private float m_zOffset;

	// Token: 0x04004BC1 RID: 19393
	private IntVector2 currentCellPosition;

	// Token: 0x04004BC2 RID: 19394
	private bool m_isMoving;
}
