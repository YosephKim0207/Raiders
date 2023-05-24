using System;
using Dungeonator;
using UnityEngine;

// Token: 0x02001027 RID: 4135
public class GatlingGullCrowController : BraveBehaviour
{
	// Token: 0x17000D2B RID: 3371
	// (get) Token: 0x06005AAC RID: 23212 RVA: 0x0022A09C File Offset: 0x0022829C
	// (set) Token: 0x06005AAD RID: 23213 RVA: 0x0022A0A4 File Offset: 0x002282A4
	public Vector2 CurrentTargetPosition { get; set; }

	// Token: 0x17000D2C RID: 3372
	// (get) Token: 0x06005AAE RID: 23214 RVA: 0x0022A0B0 File Offset: 0x002282B0
	// (set) Token: 0x06005AAF RID: 23215 RVA: 0x0022A0B8 File Offset: 0x002282B8
	public float CurrentTargetHeight { get; set; }

	// Token: 0x06005AB0 RID: 23216 RVA: 0x0022A0C4 File Offset: 0x002282C4
	private void Start()
	{
		base.spriteAnimator.ClipFps = base.spriteAnimator.ClipFps * UnityEngine.Random.Range(0.7f, 1.4f);
		this.m_currentPosition = base.transform.position.XY();
		this.m_speed = UnityEngine.Random.Range(7f, 10f);
		base.sprite.UpdateZDepth();
	}

	// Token: 0x06005AB1 RID: 23217 RVA: 0x0022A130 File Offset: 0x00228330
	private void Update()
	{
		if (this.destroyOnArrival && (GameManager.Instance.CurrentGameMode == GameManager.GameMode.BOSSRUSH || GameManager.Instance.CurrentGameMode == GameManager.GameMode.SUPERBOSSRUSH))
		{
			IntVector2 intVector = base.transform.position.IntXY(VectorConversions.Floor);
			if (!GameManager.Instance.Dungeon.data.CheckInBoundsAndValid(intVector) || GameManager.Instance.Dungeon.data[intVector].type == CellType.WALL)
			{
				UnityEngine.Object.Destroy(base.gameObject);
				return;
			}
		}
		if (base.sprite.HeightOffGround != this.CurrentTargetHeight)
		{
			float num = this.CurrentTargetHeight - base.sprite.HeightOffGround;
			float num2 = Mathf.Sign(num) * 3f * BraveTime.DeltaTime;
			if (Mathf.Abs(num2) > Mathf.Abs(num))
			{
				num2 = num;
			}
			base.sprite.HeightOffGround += num2;
		}
		if (this.m_currentPosition != this.CurrentTargetPosition)
		{
			if (this.m_currentSpeed < this.m_speed)
			{
				this.m_currentSpeed = Mathf.Clamp(this.m_currentSpeed + 4f * BraveTime.DeltaTime, 0f, this.m_speed);
			}
			Vector2 vector = this.CurrentTargetPosition - this.m_currentPosition;
			base.sprite.FlipX = ((!this.useFacePoint) ? (vector.x < 0f) : ((this.facePoint - this.m_currentPosition).x < 0f));
			float magnitude = vector.magnitude;
			float num3 = Mathf.Clamp(this.m_currentSpeed * BraveTime.DeltaTime, 0f, magnitude);
			this.m_currentPosition += num3 * vector.normalized;
			base.transform.position = this.m_currentPosition.ToVector3ZUp(0f);
			base.sprite.UpdateZDepth();
		}
		else
		{
			if (this.destroyOnArrival)
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
			this.m_currentSpeed = 0f;
		}
	}

	// Token: 0x06005AB2 RID: 23218 RVA: 0x0022A35C File Offset: 0x0022855C
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x04005416 RID: 21526
	public bool useFacePoint;

	// Token: 0x04005417 RID: 21527
	public Vector2 facePoint;

	// Token: 0x04005418 RID: 21528
	public bool destroyOnArrival;

	// Token: 0x04005419 RID: 21529
	protected Vector2 m_currentPosition;

	// Token: 0x0400541A RID: 21530
	protected float m_speed;

	// Token: 0x0400541B RID: 21531
	protected float m_currentSpeed;
}
