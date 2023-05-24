using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200047D RID: 1149
[AddComponentMenu("Character/Character Motor (CSharp)")]
[RequireComponent(typeof(CharacterController))]
public class dfCharacterMotorCS : MonoBehaviour
{
	// Token: 0x06001A70 RID: 6768 RVA: 0x0007B09C File Offset: 0x0007929C
	private void Awake()
	{
		this.controller = base.GetComponent<CharacterController>();
		this.tr = base.transform;
	}

	// Token: 0x06001A71 RID: 6769 RVA: 0x0007B0B8 File Offset: 0x000792B8
	private void UpdateFunction()
	{
		Vector3 vector = this.movement.velocity;
		vector = this.ApplyInputVelocityChange(vector);
		vector = this.ApplyGravityAndJumping(vector);
		Vector3 vector2 = Vector3.zero;
		if (this.MoveWithPlatform())
		{
			Vector3 vector3 = this.movingPlatform.activePlatform.TransformPoint(this.movingPlatform.activeLocalPoint);
			vector2 = vector3 - this.movingPlatform.activeGlobalPoint;
			if (vector2 != Vector3.zero)
			{
				this.controller.Move(vector2);
			}
			Quaternion quaternion = this.movingPlatform.activePlatform.rotation * this.movingPlatform.activeLocalRotation;
			float y = (quaternion * Quaternion.Inverse(this.movingPlatform.activeGlobalRotation)).eulerAngles.y;
			if (y != 0f)
			{
				this.tr.Rotate(0f, y, 0f);
			}
		}
		Vector3 position = this.tr.position;
		Vector3 vector4 = vector * BraveTime.DeltaTime;
		float stepOffset = this.controller.stepOffset;
		Vector3 vector5 = new Vector3(vector4.x, 0f, vector4.z);
		float num = Mathf.Max(stepOffset, vector5.magnitude);
		if (this.grounded)
		{
			vector4 -= num * Vector3.up;
		}
		this.movingPlatform.hitPlatform = null;
		this.groundNormal = Vector3.zero;
		this.movement.collisionFlags = this.controller.Move(vector4);
		this.movement.lastHitPoint = this.movement.hitPoint;
		this.lastGroundNormal = this.groundNormal;
		if (this.movingPlatform.enabled && this.movingPlatform.activePlatform != this.movingPlatform.hitPlatform && this.movingPlatform.hitPlatform != null)
		{
			this.movingPlatform.activePlatform = this.movingPlatform.hitPlatform;
			this.movingPlatform.lastMatrix = this.movingPlatform.hitPlatform.localToWorldMatrix;
			this.movingPlatform.newPlatform = true;
		}
		Vector3 vector6 = new Vector3(vector.x, 0f, vector.z);
		this.movement.velocity = (this.tr.position - position) / BraveTime.DeltaTime;
		Vector3 vector7 = new Vector3(this.movement.velocity.x, 0f, this.movement.velocity.z);
		if (vector6 == Vector3.zero)
		{
			this.movement.velocity = new Vector3(0f, this.movement.velocity.y, 0f);
		}
		else
		{
			float num2 = Vector3.Dot(vector7, vector6) / vector6.sqrMagnitude;
			this.movement.velocity = vector6 * Mathf.Clamp01(num2) + this.movement.velocity.y * Vector3.up;
		}
		if ((double)this.movement.velocity.y < (double)vector.y - 0.001)
		{
			if (this.movement.velocity.y < 0f)
			{
				this.movement.velocity.y = vector.y;
			}
			else
			{
				this.jumping.holdingJumpButton = false;
			}
		}
		if (this.grounded && !this.IsGroundedTest())
		{
			this.grounded = false;
			if (this.movingPlatform.enabled && (this.movingPlatform.movementTransfer == dfCharacterMotorCS.MovementTransferOnJump.InitTransfer || this.movingPlatform.movementTransfer == dfCharacterMotorCS.MovementTransferOnJump.PermaTransfer))
			{
				this.movement.frameVelocity = this.movingPlatform.platformVelocity;
				this.movement.velocity += this.movingPlatform.platformVelocity;
			}
			base.SendMessage("OnFall", SendMessageOptions.DontRequireReceiver);
			this.tr.position += num * Vector3.up;
		}
		else if (!this.grounded && this.IsGroundedTest())
		{
			this.grounded = true;
			this.jumping.jumping = false;
			this.SubtractNewPlatformVelocity();
			base.SendMessage("OnLand", SendMessageOptions.DontRequireReceiver);
		}
		if (this.MoveWithPlatform())
		{
			this.movingPlatform.activeGlobalPoint = this.tr.position + Vector3.up * (this.controller.center.y - this.controller.height * 0.5f + this.controller.radius);
			this.movingPlatform.activeLocalPoint = this.movingPlatform.activePlatform.InverseTransformPoint(this.movingPlatform.activeGlobalPoint);
			this.movingPlatform.activeGlobalRotation = this.tr.rotation;
			this.movingPlatform.activeLocalRotation = Quaternion.Inverse(this.movingPlatform.activePlatform.rotation) * this.movingPlatform.activeGlobalRotation;
		}
	}

	// Token: 0x06001A72 RID: 6770 RVA: 0x0007B614 File Offset: 0x00079814
	private void FixedUpdate()
	{
		if (this.movingPlatform.enabled)
		{
			if (this.movingPlatform.activePlatform != null)
			{
				if (!this.movingPlatform.newPlatform)
				{
					this.movingPlatform.platformVelocity = (this.movingPlatform.activePlatform.localToWorldMatrix.MultiplyPoint3x4(this.movingPlatform.activeLocalPoint) - this.movingPlatform.lastMatrix.MultiplyPoint3x4(this.movingPlatform.activeLocalPoint)) / BraveTime.DeltaTime;
				}
				this.movingPlatform.lastMatrix = this.movingPlatform.activePlatform.localToWorldMatrix;
				this.movingPlatform.newPlatform = false;
			}
			else
			{
				this.movingPlatform.platformVelocity = Vector3.zero;
			}
		}
		if (this.useFixedUpdate)
		{
			this.UpdateFunction();
		}
	}

	// Token: 0x06001A73 RID: 6771 RVA: 0x0007B6FC File Offset: 0x000798FC
	private void Update()
	{
		if (!this.useFixedUpdate)
		{
			this.UpdateFunction();
		}
	}

	// Token: 0x06001A74 RID: 6772 RVA: 0x0007B710 File Offset: 0x00079910
	private Vector3 ApplyInputVelocityChange(Vector3 velocity)
	{
		if (!this.canControl)
		{
			this.inputMoveDirection = Vector3.zero;
		}
		Vector3 vector2;
		if (this.grounded && this.TooSteep())
		{
			Vector3 vector = new Vector3(this.groundNormal.x, 0f, this.groundNormal.z);
			vector2 = vector.normalized;
			Vector3 vector3 = Vector3.Project(this.inputMoveDirection, vector2);
			vector2 = vector2 + vector3 * this.sliding.speedControl + (this.inputMoveDirection - vector3) * this.sliding.sidewaysControl;
			vector2 *= this.sliding.slidingSpeed;
		}
		else
		{
			vector2 = this.GetDesiredHorizontalVelocity();
		}
		if (this.movingPlatform.enabled && this.movingPlatform.movementTransfer == dfCharacterMotorCS.MovementTransferOnJump.PermaTransfer)
		{
			vector2 += this.movement.frameVelocity;
			vector2.y = 0f;
		}
		if (this.grounded)
		{
			vector2 = this.AdjustGroundVelocityToNormal(vector2, this.groundNormal);
		}
		else
		{
			velocity.y = 0f;
		}
		float num = this.GetMaxAcceleration(this.grounded) * BraveTime.DeltaTime;
		Vector3 vector4 = vector2 - velocity;
		if (vector4.sqrMagnitude > num * num)
		{
			vector4 = vector4.normalized * num;
		}
		if (this.grounded || this.canControl)
		{
			velocity += vector4;
		}
		if (this.grounded)
		{
			velocity.y = Mathf.Min(velocity.y, 0f);
		}
		return velocity;
	}

	// Token: 0x06001A75 RID: 6773 RVA: 0x0007B8C0 File Offset: 0x00079AC0
	private Vector3 ApplyGravityAndJumping(Vector3 velocity)
	{
		if (!this.inputJump || !this.canControl)
		{
			this.jumping.holdingJumpButton = false;
			this.jumping.lastButtonDownTime = -100f;
		}
		if (this.inputJump && this.jumping.lastButtonDownTime < 0f && this.canControl)
		{
			this.jumping.lastButtonDownTime = Time.time;
		}
		if (this.grounded)
		{
			velocity.y = Mathf.Min(0f, velocity.y) - this.movement.gravity * BraveTime.DeltaTime;
		}
		else
		{
			velocity.y = this.movement.velocity.y - this.movement.gravity * BraveTime.DeltaTime;
			if (this.jumping.jumping && this.jumping.holdingJumpButton && Time.time < this.jumping.lastStartTime + this.jumping.extraHeight / this.CalculateJumpVerticalSpeed(this.jumping.baseHeight))
			{
				velocity += this.jumping.jumpDir * this.movement.gravity * BraveTime.DeltaTime;
			}
			velocity.y = Mathf.Max(velocity.y, -this.movement.maxFallSpeed);
		}
		if (this.grounded)
		{
			if (this.jumping.enabled && this.canControl && (double)(Time.time - this.jumping.lastButtonDownTime) < 0.2)
			{
				this.grounded = false;
				this.jumping.jumping = true;
				this.jumping.lastStartTime = Time.time;
				this.jumping.lastButtonDownTime = -100f;
				this.jumping.holdingJumpButton = true;
				if (this.TooSteep())
				{
					this.jumping.jumpDir = Vector3.Slerp(Vector3.up, this.groundNormal, this.jumping.steepPerpAmount);
				}
				else
				{
					this.jumping.jumpDir = Vector3.Slerp(Vector3.up, this.groundNormal, this.jumping.perpAmount);
				}
				velocity.y = 0f;
				velocity += this.jumping.jumpDir * this.CalculateJumpVerticalSpeed(this.jumping.baseHeight);
				if (this.movingPlatform.enabled && (this.movingPlatform.movementTransfer == dfCharacterMotorCS.MovementTransferOnJump.InitTransfer || this.movingPlatform.movementTransfer == dfCharacterMotorCS.MovementTransferOnJump.PermaTransfer))
				{
					this.movement.frameVelocity = this.movingPlatform.platformVelocity;
					velocity += this.movingPlatform.platformVelocity;
				}
				base.SendMessage("OnJump", SendMessageOptions.DontRequireReceiver);
			}
			else
			{
				this.jumping.holdingJumpButton = false;
			}
		}
		return velocity;
	}

	// Token: 0x06001A76 RID: 6774 RVA: 0x0007BBCC File Offset: 0x00079DCC
	private void OnControllerColliderHit(ControllerColliderHit hit)
	{
		if (hit.normal.y > 0f && hit.normal.y > this.groundNormal.y && hit.moveDirection.y < 0f)
		{
			if ((double)(hit.point - this.movement.lastHitPoint).sqrMagnitude > 0.001 || this.lastGroundNormal == Vector3.zero)
			{
				this.groundNormal = hit.normal;
			}
			else
			{
				this.groundNormal = this.lastGroundNormal;
			}
			this.movingPlatform.hitPlatform = hit.collider.transform;
			this.movement.hitPoint = hit.point;
			this.movement.frameVelocity = Vector3.zero;
		}
	}

	// Token: 0x06001A77 RID: 6775 RVA: 0x0007BCC0 File Offset: 0x00079EC0
	private IEnumerator SubtractNewPlatformVelocity()
	{
		if (this.movingPlatform.enabled && (this.movingPlatform.movementTransfer == dfCharacterMotorCS.MovementTransferOnJump.InitTransfer || this.movingPlatform.movementTransfer == dfCharacterMotorCS.MovementTransferOnJump.PermaTransfer))
		{
			if (this.movingPlatform.newPlatform)
			{
				Transform platform = this.movingPlatform.activePlatform;
				yield return new WaitForFixedUpdate();
				yield return new WaitForFixedUpdate();
				if (this.grounded && platform == this.movingPlatform.activePlatform)
				{
					yield break;
				}
			}
			this.movement.velocity -= this.movingPlatform.platformVelocity;
		}
		yield break;
	}

	// Token: 0x06001A78 RID: 6776 RVA: 0x0007BCDC File Offset: 0x00079EDC
	private bool MoveWithPlatform()
	{
		return this.movingPlatform.enabled && (this.grounded || this.movingPlatform.movementTransfer == dfCharacterMotorCS.MovementTransferOnJump.PermaLocked) && this.movingPlatform.activePlatform != null;
	}

	// Token: 0x06001A79 RID: 6777 RVA: 0x0007BD2C File Offset: 0x00079F2C
	private Vector3 GetDesiredHorizontalVelocity()
	{
		Vector3 vector = this.tr.InverseTransformDirection(this.inputMoveDirection);
		float num = this.MaxSpeedInDirection(vector);
		if (this.grounded)
		{
			float num2 = Mathf.Asin(this.movement.velocity.normalized.y) * 57.29578f;
			num *= this.movement.slopeSpeedMultiplier.Evaluate(num2);
		}
		return this.tr.TransformDirection(vector * num);
	}

	// Token: 0x06001A7A RID: 6778 RVA: 0x0007BDA8 File Offset: 0x00079FA8
	private Vector3 AdjustGroundVelocityToNormal(Vector3 hVelocity, Vector3 groundNormal)
	{
		Vector3 vector = Vector3.Cross(Vector3.up, hVelocity);
		return Vector3.Cross(vector, groundNormal).normalized * hVelocity.magnitude;
	}

	// Token: 0x06001A7B RID: 6779 RVA: 0x0007BDDC File Offset: 0x00079FDC
	private bool IsGroundedTest()
	{
		return (double)this.groundNormal.y > 0.01;
	}

	// Token: 0x06001A7C RID: 6780 RVA: 0x0007BDF8 File Offset: 0x00079FF8
	private float GetMaxAcceleration(bool grounded)
	{
		if (grounded)
		{
			return this.movement.maxGroundAcceleration;
		}
		return this.movement.maxAirAcceleration;
	}

	// Token: 0x06001A7D RID: 6781 RVA: 0x0007BE18 File Offset: 0x0007A018
	private float CalculateJumpVerticalSpeed(float targetJumpHeight)
	{
		return Mathf.Sqrt(2f * targetJumpHeight * this.movement.gravity);
	}

	// Token: 0x06001A7E RID: 6782 RVA: 0x0007BE34 File Offset: 0x0007A034
	private bool IsJumping()
	{
		return this.jumping.jumping;
	}

	// Token: 0x06001A7F RID: 6783 RVA: 0x0007BE44 File Offset: 0x0007A044
	private bool IsSliding()
	{
		return this.grounded && this.sliding.enabled && this.TooSteep();
	}

	// Token: 0x06001A80 RID: 6784 RVA: 0x0007BE6C File Offset: 0x0007A06C
	private bool IsTouchingCeiling()
	{
		return (this.movement.collisionFlags & CollisionFlags.Above) != CollisionFlags.None;
	}

	// Token: 0x06001A81 RID: 6785 RVA: 0x0007BE84 File Offset: 0x0007A084
	private bool IsGrounded()
	{
		return this.grounded;
	}

	// Token: 0x06001A82 RID: 6786 RVA: 0x0007BE8C File Offset: 0x0007A08C
	private bool TooSteep()
	{
		return this.groundNormal.y <= Mathf.Cos(this.controller.slopeLimit * 0.017453292f);
	}

	// Token: 0x06001A83 RID: 6787 RVA: 0x0007BEB4 File Offset: 0x0007A0B4
	private Vector3 GetDirection()
	{
		return this.inputMoveDirection;
	}

	// Token: 0x06001A84 RID: 6788 RVA: 0x0007BEBC File Offset: 0x0007A0BC
	private void SetControllable(bool controllable)
	{
		this.canControl = controllable;
	}

	// Token: 0x06001A85 RID: 6789 RVA: 0x0007BEC8 File Offset: 0x0007A0C8
	private float MaxSpeedInDirection(Vector3 desiredMovementDirection)
	{
		if (desiredMovementDirection == Vector3.zero)
		{
			return 0f;
		}
		float num = ((desiredMovementDirection.z <= 0f) ? this.movement.maxBackwardsSpeed : this.movement.maxForwardSpeed) / this.movement.maxSidewaysSpeed;
		Vector3 vector = new Vector3(desiredMovementDirection.x, 0f, desiredMovementDirection.z / num);
		Vector3 normalized = vector.normalized;
		Vector3 vector2 = new Vector3(normalized.x, 0f, normalized.z * num);
		return vector2.magnitude * this.movement.maxSidewaysSpeed;
	}

	// Token: 0x06001A86 RID: 6790 RVA: 0x0007BF7C File Offset: 0x0007A17C
	private void SetVelocity(Vector3 velocity)
	{
		this.grounded = false;
		this.movement.velocity = velocity;
		this.movement.frameVelocity = Vector3.zero;
		base.SendMessage("OnExternalVelocity");
	}

	// Token: 0x040014A8 RID: 5288
	public bool canControl = true;

	// Token: 0x040014A9 RID: 5289
	public bool useFixedUpdate = true;

	// Token: 0x040014AA RID: 5290
	[NonSerialized]
	public Vector3 inputMoveDirection = Vector3.zero;

	// Token: 0x040014AB RID: 5291
	[NonSerialized]
	public bool inputJump;

	// Token: 0x040014AC RID: 5292
	public bool inputSprint;

	// Token: 0x040014AD RID: 5293
	public dfCharacterMotorCS.CharacterMotorMovement movement = new dfCharacterMotorCS.CharacterMotorMovement();

	// Token: 0x040014AE RID: 5294
	public dfCharacterMotorCS.CharacterMotorJumping jumping = new dfCharacterMotorCS.CharacterMotorJumping();

	// Token: 0x040014AF RID: 5295
	public dfCharacterMotorCS.CharacterMotorMovingPlatform movingPlatform = new dfCharacterMotorCS.CharacterMotorMovingPlatform();

	// Token: 0x040014B0 RID: 5296
	public dfCharacterMotorCS.CharacterMotorSliding sliding = new dfCharacterMotorCS.CharacterMotorSliding();

	// Token: 0x040014B1 RID: 5297
	[NonSerialized]
	public bool grounded = true;

	// Token: 0x040014B2 RID: 5298
	[NonSerialized]
	public Vector3 groundNormal = Vector3.zero;

	// Token: 0x040014B3 RID: 5299
	private Vector3 lastGroundNormal = Vector3.zero;

	// Token: 0x040014B4 RID: 5300
	private Transform tr;

	// Token: 0x040014B5 RID: 5301
	private CharacterController controller;

	// Token: 0x0200047E RID: 1150
	[Serializable]
	public class CharacterMotorMovement
	{
		// Token: 0x040014B6 RID: 5302
		public float maxForwardSpeed = 3f;

		// Token: 0x040014B7 RID: 5303
		public float maxSidewaysSpeed = 2f;

		// Token: 0x040014B8 RID: 5304
		public float maxBackwardsSpeed = 2f;

		// Token: 0x040014B9 RID: 5305
		public AnimationCurve slopeSpeedMultiplier = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(-90f, 1f),
			new Keyframe(0f, 1f),
			new Keyframe(90f, 0f)
		});

		// Token: 0x040014BA RID: 5306
		public float maxGroundAcceleration = 30f;

		// Token: 0x040014BB RID: 5307
		public float maxAirAcceleration = 20f;

		// Token: 0x040014BC RID: 5308
		public float gravity = 9.81f;

		// Token: 0x040014BD RID: 5309
		public float maxFallSpeed = 20f;

		// Token: 0x040014BE RID: 5310
		[NonSerialized]
		public CollisionFlags collisionFlags;

		// Token: 0x040014BF RID: 5311
		[NonSerialized]
		public Vector3 velocity;

		// Token: 0x040014C0 RID: 5312
		[NonSerialized]
		public Vector3 frameVelocity = Vector3.zero;

		// Token: 0x040014C1 RID: 5313
		[NonSerialized]
		public Vector3 hitPoint = Vector3.zero;

		// Token: 0x040014C2 RID: 5314
		[NonSerialized]
		public Vector3 lastHitPoint = new Vector3(float.PositiveInfinity, 0f, 0f);
	}

	// Token: 0x0200047F RID: 1151
	public enum MovementTransferOnJump
	{
		// Token: 0x040014C4 RID: 5316
		None,
		// Token: 0x040014C5 RID: 5317
		InitTransfer,
		// Token: 0x040014C6 RID: 5318
		PermaTransfer,
		// Token: 0x040014C7 RID: 5319
		PermaLocked
	}

	// Token: 0x02000480 RID: 1152
	[Serializable]
	public class CharacterMotorJumping
	{
		// Token: 0x040014C8 RID: 5320
		public bool enabled = true;

		// Token: 0x040014C9 RID: 5321
		public float baseHeight = 1f;

		// Token: 0x040014CA RID: 5322
		public float extraHeight = 4.1f;

		// Token: 0x040014CB RID: 5323
		public float perpAmount;

		// Token: 0x040014CC RID: 5324
		public float steepPerpAmount = 0.5f;

		// Token: 0x040014CD RID: 5325
		[NonSerialized]
		public bool jumping;

		// Token: 0x040014CE RID: 5326
		[NonSerialized]
		public bool holdingJumpButton;

		// Token: 0x040014CF RID: 5327
		[NonSerialized]
		public float lastStartTime;

		// Token: 0x040014D0 RID: 5328
		[NonSerialized]
		public float lastButtonDownTime = -100f;

		// Token: 0x040014D1 RID: 5329
		[NonSerialized]
		public Vector3 jumpDir = Vector3.up;
	}

	// Token: 0x02000481 RID: 1153
	[Serializable]
	public class CharacterMotorMovingPlatform
	{
		// Token: 0x040014D2 RID: 5330
		public bool enabled = true;

		// Token: 0x040014D3 RID: 5331
		public dfCharacterMotorCS.MovementTransferOnJump movementTransfer = dfCharacterMotorCS.MovementTransferOnJump.PermaTransfer;

		// Token: 0x040014D4 RID: 5332
		[NonSerialized]
		public Transform hitPlatform;

		// Token: 0x040014D5 RID: 5333
		[NonSerialized]
		public Transform activePlatform;

		// Token: 0x040014D6 RID: 5334
		[NonSerialized]
		public Vector3 activeLocalPoint;

		// Token: 0x040014D7 RID: 5335
		[NonSerialized]
		public Vector3 activeGlobalPoint;

		// Token: 0x040014D8 RID: 5336
		[NonSerialized]
		public Quaternion activeLocalRotation;

		// Token: 0x040014D9 RID: 5337
		[NonSerialized]
		public Quaternion activeGlobalRotation;

		// Token: 0x040014DA RID: 5338
		[NonSerialized]
		public Matrix4x4 lastMatrix;

		// Token: 0x040014DB RID: 5339
		[NonSerialized]
		public Vector3 platformVelocity;

		// Token: 0x040014DC RID: 5340
		[NonSerialized]
		public bool newPlatform;
	}

	// Token: 0x02000482 RID: 1154
	[Serializable]
	public class CharacterMotorSliding
	{
		// Token: 0x040014DD RID: 5341
		public bool enabled = true;

		// Token: 0x040014DE RID: 5342
		public float slidingSpeed = 15f;

		// Token: 0x040014DF RID: 5343
		public float sidewaysControl = 1f;

		// Token: 0x040014E0 RID: 5344
		public float speedControl = 0.4f;
	}
}
