using System;
using UnityEngine;

// Token: 0x02000FAD RID: 4013
public class BodyPartController : BraveBehaviour
{
	// Token: 0x17000C73 RID: 3187
	// (get) Token: 0x06005754 RID: 22356 RVA: 0x00215008 File Offset: 0x00213208
	// (set) Token: 0x06005755 RID: 22357 RVA: 0x00215010 File Offset: 0x00213210
	public bool OverrideFacingDirection { get; set; }

	// Token: 0x06005756 RID: 22358 RVA: 0x0021501C File Offset: 0x0021321C
	public virtual void Awake()
	{
		if (this.specifyActor)
		{
			this.m_body = this.specifyActor;
		}
		if (!this.m_body)
		{
			this.m_body = base.aiActor;
		}
		if (!this.m_body && base.transform.parent)
		{
			this.m_body = base.transform.parent.GetComponent<AIActor>();
		}
		if (this.m_body)
		{
			if (this.independentFlashOnDamage)
			{
				this.m_body.healthHaver.RegisterBodySprite(base.sprite, true, this.myPixelCollider);
			}
			else
			{
				this.m_body.healthHaver.RegisterBodySprite(base.sprite, false, 0);
			}
			this.m_bodyFound = true;
		}
	}

	// Token: 0x06005757 RID: 22359 RVA: 0x002150F8 File Offset: 0x002132F8
	public virtual void Start()
	{
		this.m_heightOffBody = base.sprite.HeightOffGround;
		if (this.hasOutlines)
		{
			SpriteOutlineManager.AddOutlineToSprite(base.sprite, Color.black, base.sprite.HeightOffGround + 0.1f, 0f, SpriteOutlineManager.OutlineType.NORMAL);
			if (this.m_body)
			{
				ObjectVisibilityManager component = this.m_body.GetComponent<ObjectVisibilityManager>();
				if (component)
				{
					component.ResetRenderersList();
				}
			}
		}
		if (!this.m_bodyFound && this.m_body)
		{
			this.m_body.healthHaver.RegisterBodySprite(base.sprite, false, 0);
			this.m_bodyFound = true;
		}
		if (!base.specRigidbody)
		{
			base.specRigidbody = this.m_body.specRigidbody;
		}
		if ((this.faceTarget & base.aiAnimator) && this.m_body.aiAnimator)
		{
			base.aiAnimator.LockFacingDirection = true;
			base.aiAnimator.FacingDirection = this.m_body.aiAnimator.FacingDirection;
		}
		if (base.specRigidbody && this.redirectHealthHaver)
		{
			base.specRigidbody.healthHaver = this.m_body.healthHaver;
		}
	}

	// Token: 0x06005758 RID: 22360 RVA: 0x00215254 File Offset: 0x00213454
	public virtual void Update()
	{
		float num;
		if (!this.OverrideFacingDirection && this.faceTarget && this.TryGetAimAngle(out num))
		{
			if (this.faceTargetTurnSpeed > 0f)
			{
				float num2 = ((!base.aiAnimator) ? base.transform.eulerAngles.z : base.aiAnimator.FacingDirection);
				num = Mathf.MoveTowardsAngle(num2, num, this.faceTargetTurnSpeed * BraveTime.DeltaTime);
			}
			if (base.aiAnimator)
			{
				base.aiAnimator.LockFacingDirection = true;
				base.aiAnimator.FacingDirection = num;
			}
			else
			{
				base.transform.rotation = Quaternion.Euler(0f, 0f, num);
			}
		}
		if (this.autoDepth && base.aiAnimator)
		{
			float num3 = BraveMathCollege.ClampAngle180(this.m_body.aiAnimator.FacingDirection);
			float num4 = BraveMathCollege.ClampAngle180(base.aiAnimator.FacingDirection);
			bool flag = num3 <= 155f && num3 >= 25f && num4 <= 155f && num4 >= 25f;
			base.sprite.HeightOffGround = ((!flag) ? this.m_heightOffBody : (-this.m_heightOffBody));
		}
	}

	// Token: 0x06005759 RID: 22361 RVA: 0x002153C4 File Offset: 0x002135C4
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x0600575A RID: 22362 RVA: 0x002153CC File Offset: 0x002135CC
	protected virtual bool TryGetAimAngle(out float angle)
	{
		angle = 0f;
		if (this.m_body.TargetRigidbody)
		{
			Vector2 unitCenter = this.m_body.TargetRigidbody.GetUnitCenter(ColliderType.HitBox);
			Vector2 vector = base.transform.position.XY();
			if (this.aimFrom == BodyPartController.AimFromType.ActorHitBoxCenter)
			{
				vector = this.m_body.specRigidbody.GetUnitCenter(ColliderType.HitBox);
			}
			angle = (unitCenter - vector).ToAngle();
			return true;
		}
		if (this.m_body.aiAnimator)
		{
			angle = this.m_body.aiAnimator.FacingDirection;
			return true;
		}
		return false;
	}

	// Token: 0x04005053 RID: 20563
	public AIActor specifyActor;

	// Token: 0x04005054 RID: 20564
	public bool hasOutlines;

	// Token: 0x04005055 RID: 20565
	public bool faceTarget;

	// Token: 0x04005056 RID: 20566
	[ShowInInspectorIf("faceTarget", true)]
	public float faceTargetTurnSpeed = -1f;

	// Token: 0x04005057 RID: 20567
	[ShowInInspectorIf("faceTarget", true)]
	public BodyPartController.AimFromType aimFrom = BodyPartController.AimFromType.Transform;

	// Token: 0x04005058 RID: 20568
	public bool autoDepth = true;

	// Token: 0x04005059 RID: 20569
	public bool redirectHealthHaver;

	// Token: 0x0400505A RID: 20570
	public bool independentFlashOnDamage;

	// Token: 0x0400505B RID: 20571
	public int myPixelCollider = -1;

	// Token: 0x0400505D RID: 20573
	protected AIActor m_body;

	// Token: 0x0400505E RID: 20574
	private float m_heightOffBody;

	// Token: 0x0400505F RID: 20575
	private bool m_bodyFound;

	// Token: 0x02000FAE RID: 4014
	public enum AimFromType
	{
		// Token: 0x04005061 RID: 20577
		Transform = 10,
		// Token: 0x04005062 RID: 20578
		ActorHitBoxCenter = 20
	}
}
