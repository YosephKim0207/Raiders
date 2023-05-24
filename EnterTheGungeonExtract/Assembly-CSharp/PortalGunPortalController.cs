using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

// Token: 0x020012A7 RID: 4775
public class PortalGunPortalController : BraveBehaviour
{
	// Token: 0x06006AD3 RID: 27347 RVA: 0x0029E164 File Offset: 0x0029C364
	private void Awake()
	{
		this.cm_bg = (1 << LayerMask.NameToLayer("BG_Nonsense")) | (1 << LayerMask.NameToLayer("BG_Critical"));
		this.cm_fg = (1 << LayerMask.NameToLayer("FG_Nonsense")) | (1 << LayerMask.NameToLayer("ShadowCaster")) | (1 << LayerMask.NameToLayer("Default")) | (1 << LayerMask.NameToLayer("FG_Reflection")) | (1 << LayerMask.NameToLayer("FG_Critical"));
		if (this.m_renderTarget == null && this.FacewallCamera)
		{
			this.FacewallCamera.orthographicSize = 1f;
			this.FacewallCamera.opaqueSortMode = OpaqueSortMode.FrontToBack;
			this.FacewallCamera.transparencySortMode = TransparencySortMode.Orthographic;
			this.FacewallCamera.enabled = false;
			this.m_renderTarget = RenderTexture.GetTemporary(this.PixelWidth, this.PixelHeight, 24, RenderTextureFormat.Default);
			this.m_renderTarget.filterMode = FilterMode.Point;
			this.FacewallCamera.targetTexture = this.m_renderTarget;
			Material material = UnityEngine.Object.Instantiate<Material>(this.PortalRenderer.material);
			material.shader = Shader.Find("Brave/Effects/CutoutPortalInternalTilted");
			material.SetTexture("_MainTex", this.m_renderTarget);
			material.SetTexture("_MaskTex", this.FacewallMaskTexture);
			this.PortalRenderer.material = material;
		}
		PortalGunPortalController.m_portalNumber++;
	}

	// Token: 0x06006AD4 RID: 27348 RVA: 0x0029E2D4 File Offset: 0x0029C4D4
	private void LateUpdate()
	{
		if (this.m_doRender)
		{
			this.FacewallCamera.clearFlags = CameraClearFlags.Color;
			this.FacewallCamera.backgroundColor = Color.black;
			this.FacewallCamera.cullingMask = this.cm_bg;
			this.FacewallCamera.Render();
			this.FacewallCamera.clearFlags = CameraClearFlags.Depth;
			this.FacewallCamera.backgroundColor = Color.clear;
			this.FacewallCamera.cullingMask = this.cm_fg;
			this.FacewallCamera.Render();
		}
	}

	// Token: 0x06006AD5 RID: 27349 RVA: 0x0029E35C File Offset: 0x0029C55C
	private IEnumerator Start()
	{
		base.transform.position += new Vector3(0f, -0.125f, 0f);
		if (this.FacewallCamera)
		{
			base.transform.position += new Vector3(0f, 0.5f, 0f);
			base.sprite.HeightOffGround += 0.75f;
		}
		base.specRigidbody.Reinitialize();
		SpeculativeRigidbody specRigidbody = base.specRigidbody;
		specRigidbody.OnEnterTrigger = (SpeculativeRigidbody.OnTriggerDelegate)Delegate.Combine(specRigidbody.OnEnterTrigger, new SpeculativeRigidbody.OnTriggerDelegate(this.HandleTriggerCollision));
		for (int i = 0; i < StaticReferenceManager.AllPortals.Count; i++)
		{
			PortalGunPortalController portalGunPortalController = StaticReferenceManager.AllPortals[i];
			if (portalGunPortalController.IsAlternatePortal != this.IsAlternatePortal)
			{
				portalGunPortalController.BecomeLinkedTo(this);
				this.BecomeLinkedTo(portalGunPortalController);
				break;
			}
		}
		for (int j = StaticReferenceManager.AllPortals.Count - 1; j >= 0; j--)
		{
			if (StaticReferenceManager.AllPortals[j] != this && StaticReferenceManager.AllPortals[j] != this.pairedPortal)
			{
				UnityEngine.Object.Destroy(StaticReferenceManager.AllPortals[j].gameObject);
			}
		}
		if (this.pairedPortal != null)
		{
			this.pairedPortal.BecomeLinkedTo(this);
		}
		StaticReferenceManager.AllPortals.Add(this);
		yield return null;
		if (base.sprite.FlipY)
		{
			this.PortalNormal = new Vector2(-1f, 0f);
			base.specRigidbody.Reinitialize();
		}
		if (base.transform.rotation != Quaternion.identity && this.PortalNormal.y < 0f)
		{
			this.PortalNormal = new Vector2(0f, 1f);
		}
		yield break;
	}

	// Token: 0x06006AD6 RID: 27350 RVA: 0x0029E378 File Offset: 0x0029C578
	private void BecomeLinkedTo(PortalGunPortalController otherPortal)
	{
		this.pairedPortal = otherPortal;
		if (this.FacewallCamera)
		{
			this.m_doRender = true;
			this.FacewallCamera.transform.position = otherPortal.transform.position + new Vector3(otherPortal.PortalNormal.x, otherPortal.PortalNormal.y * 2f + -0.375f, -10f) + CameraController.PLATFORM_CAMERA_OFFSET;
			this.PortalRenderer.enabled = true;
		}
	}

	// Token: 0x06006AD7 RID: 27351 RVA: 0x0029E408 File Offset: 0x0029C608
	private void BecomeUnlinked()
	{
		this.pairedPortal = null;
		this.m_doRender = false;
		if (this.FacewallCamera)
		{
			this.PortalRenderer.enabled = false;
		}
	}

	// Token: 0x06006AD8 RID: 27352 RVA: 0x0029E434 File Offset: 0x0029C634
	private void HandleTriggerCollision(SpeculativeRigidbody otherRigidbody, SpeculativeRigidbody myRigidbody, CollisionData collisionData)
	{
		if (this.pairedPortal)
		{
			if (otherRigidbody.projectile)
			{
				float num = Mathf.DeltaAngle(BraveMathCollege.Atan2Degrees(-this.PortalNormal), BraveMathCollege.Atan2Degrees(this.pairedPortal.PortalNormal));
				Vector2 vector = this.pairedPortal.specRigidbody.UnitCenter;
				if (this.pairedPortal.PortalNormal.x != 0f)
				{
					vector += this.pairedPortal.PortalNormal.normalized * 0.5f;
				}
				else
				{
					vector += this.pairedPortal.PortalNormal.normalized;
				}
				otherRigidbody.transform.position = vector.ToVector3ZisY(0f);
				otherRigidbody.Reinitialize();
				PhysicsEngine.Instance.RegisterOverlappingGhostCollisionExceptions(otherRigidbody, null, false);
				otherRigidbody.RegisterGhostCollisionException(this.pairedPortal.specRigidbody);
				otherRigidbody.RegisterGhostCollisionException(base.specRigidbody);
				otherRigidbody.projectile.SendInDirection((Quaternion.Euler(0f, 0f, num) * otherRigidbody.projectile.Direction).XY(), false, true);
				PhysicsEngine.SkipCollision = true;
				otherRigidbody.projectile.LastPosition = otherRigidbody.transform.position;
			}
			else if (otherRigidbody.gameActor)
			{
				Vector2 vector2 = otherRigidbody.gameActor.transform.position.XY() - otherRigidbody.gameActor.specRigidbody.UnitCenter;
				vector2 += this.pairedPortal.PortalNormal;
				if (this.pairedPortal.PortalNormal.y < 0f)
				{
					vector2 += this.pairedPortal.PortalNormal * 2f;
				}
				if (otherRigidbody.gameActor is PlayerController)
				{
					PlayerController playerController = otherRigidbody.gameActor as PlayerController;
					playerController.WarpToPoint(this.pairedPortal.specRigidbody.UnitCenter + vector2, false, false);
					playerController.specRigidbody.RecheckTriggers = false;
				}
				else if (otherRigidbody.gameActor is AIActor)
				{
					AIActor aiactor = otherRigidbody.gameActor as AIActor;
					aiactor.transform.position = this.pairedPortal.specRigidbody.UnitCenter + vector2;
					otherRigidbody.Reinitialize();
				}
				PhysicsEngine.Instance.RegisterOverlappingGhostCollisionExceptions(otherRigidbody, null, true);
				otherRigidbody.RegisterTemporaryCollisionException(this.pairedPortal.specRigidbody, 0.5f, null);
				otherRigidbody.RegisterTemporaryCollisionException(base.specRigidbody, 0.5f, null);
				if (otherRigidbody.knockbackDoer)
				{
					otherRigidbody.knockbackDoer.ApplyKnockback(this.pairedPortal.PortalNormal, 10f, false);
				}
			}
		}
	}

	// Token: 0x06006AD9 RID: 27353 RVA: 0x0029E730 File Offset: 0x0029C930
	protected override void OnDestroy()
	{
		if (this.pairedPortal != null)
		{
			if (this.pairedPortal.pairedPortal == this)
			{
				this.pairedPortal.BecomeUnlinked();
			}
			this.BecomeUnlinked();
		}
		StaticReferenceManager.AllPortals.Remove(this);
		if (this.m_renderTarget != null)
		{
			RenderTexture.ReleaseTemporary(this.m_renderTarget);
		}
		base.OnDestroy();
	}

	// Token: 0x06006ADA RID: 27354 RVA: 0x0029E7A4 File Offset: 0x0029C9A4
	private void HandleRigidbodyCollision(CollisionData rigidbodyCollision)
	{
		throw new NotImplementedException();
	}

	// Token: 0x0400676B RID: 26475
	public bool IsAlternatePortal;

	// Token: 0x0400676C RID: 26476
	public Vector2 PortalNormal;

	// Token: 0x0400676D RID: 26477
	public PortalGunPortalController pairedPortal;

	// Token: 0x0400676E RID: 26478
	public Camera FacewallCamera;

	// Token: 0x0400676F RID: 26479
	public MeshRenderer PortalRenderer;

	// Token: 0x04006770 RID: 26480
	public Texture2D FacewallMaskTexture;

	// Token: 0x04006771 RID: 26481
	public int PixelWidth = 16;

	// Token: 0x04006772 RID: 26482
	public int PixelHeight = 32;

	// Token: 0x04006773 RID: 26483
	private RenderTexture m_renderTarget;

	// Token: 0x04006774 RID: 26484
	private bool m_doRender;

	// Token: 0x04006775 RID: 26485
	private int cm_bg;

	// Token: 0x04006776 RID: 26486
	private int cm_fg;

	// Token: 0x04006777 RID: 26487
	private static int m_portalNumber;
}
