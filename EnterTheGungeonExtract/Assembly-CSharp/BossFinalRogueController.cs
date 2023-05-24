using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000FE3 RID: 4067
public class BossFinalRogueController : BraveBehaviour
{
	// Token: 0x060058B1 RID: 22705 RVA: 0x0021E3E0 File Offset: 0x0021C5E0
	public void Start()
	{
		PhysicsEngine.Instance.OnPostRigidbodyMovement += this.PostRigidbodyMovement;
		base.gameObject.SetLayerRecursively(LayerMask.NameToLayer("BG_Critical"));
		this.m_pastController = UnityEngine.Object.FindObjectOfType<PilotPastController>();
	}

	// Token: 0x060058B2 RID: 22706 RVA: 0x0021E418 File Offset: 0x0021C618
	public void Update()
	{
		if (this.m_camera && this.m_cameraX != null)
		{
			float x = this.m_camera.GetCoreCurrentBasePosition().x;
			float num = Mathf.InverseLerp(this.minScrollDist, this.maxScrollDist, Mathf.Abs(x - this.m_cameraX.Value));
			this.m_pastController.BackgroundScrollSpeed.x = Mathf.Sign(x - this.m_cameraX.Value) * num * this.scrollMultiplier;
		}
		this.m_cachedCameraLowerLeftPixels = PhysicsEngine.UnitToPixel(BraveUtility.ViewportToWorldpoint(new Vector2(0f, 0f), ViewportType.Gameplay));
		this.m_cachedCameraUpperRightPixels = PhysicsEngine.UnitToPixel(BraveUtility.ViewportToWorldpoint(new Vector2(1f, 1f), ViewportType.Gameplay));
	}

	// Token: 0x060058B3 RID: 22707 RVA: 0x0021E4F4 File Offset: 0x0021C6F4
	protected override void OnDestroy()
	{
		if (PhysicsEngine.HasInstance)
		{
			PhysicsEngine.Instance.OnPostRigidbodyMovement -= this.PostRigidbodyMovement;
		}
		base.OnDestroy();
	}

	// Token: 0x060058B4 RID: 22708 RVA: 0x0021E51C File Offset: 0x0021C71C
	public void InitCamera()
	{
		if (this.m_camera)
		{
			return;
		}
		this.m_camera = GameManager.Instance.MainCameraController;
		this.m_lockCamera = true;
		this.m_camera.SetManualControl(true, true);
		this.m_camera.OverridePosition = this.CameraPos;
		this.m_cameraX = new float?(this.m_camera.OverridePosition.x);
		SpeculativeRigidbody specRigidbody = GameManager.Instance.PrimaryPlayer.specRigidbody;
		specRigidbody.MovementRestrictor = (SpeculativeRigidbody.MovementRestrictorDelegate)Delegate.Combine(specRigidbody.MovementRestrictor, new SpeculativeRigidbody.MovementRestrictorDelegate(this.CameraBoundsMovementRestrictor));
	}

	// Token: 0x060058B5 RID: 22709 RVA: 0x0021E5C0 File Offset: 0x0021C7C0
	public void EndCameraLock()
	{
		this.m_lockCamera = false;
	}

	// Token: 0x17000CB5 RID: 3253
	// (get) Token: 0x060058B6 RID: 22710 RVA: 0x0021E5CC File Offset: 0x0021C7CC
	// (set) Token: 0x060058B7 RID: 22711 RVA: 0x0021E5D4 File Offset: 0x0021C7D4
	public bool SuppressBaseGuns
	{
		get
		{
			return this.m_suppressBaseGuns;
		}
		set
		{
			if (this.m_suppressBaseGuns != value)
			{
				for (int i = 0; i < this.BaseGuns.Count; i++)
				{
					this.BaseGuns[i].fireType = ((!value) ? BossFinalRogueGunController.FireType.Timed : BossFinalRogueGunController.FireType.Triggered);
				}
			}
			this.m_suppressBaseGuns = value;
		}
	}

	// Token: 0x060058B8 RID: 22712 RVA: 0x0021E630 File Offset: 0x0021C830
	private void PostRigidbodyMovement()
	{
		if (this.m_lockCamera)
		{
			this.m_camera.OverridePosition = this.CameraPos;
		}
	}

	// Token: 0x060058B9 RID: 22713 RVA: 0x0021E654 File Offset: 0x0021C854
	private void CameraBoundsMovementRestrictor(SpeculativeRigidbody specRigidbody, IntVector2 prevPixelOffset, IntVector2 pixelOffset, ref bool validLocation)
	{
		if (!validLocation)
		{
			return;
		}
		if (specRigidbody.PixelColliders[0].LowerLeft.x < this.m_cachedCameraLowerLeftPixels.x && pixelOffset.x < prevPixelOffset.x)
		{
			validLocation = false;
		}
		else if (specRigidbody.PixelColliders[0].UpperRight.x > this.m_cachedCameraUpperRightPixels.x && pixelOffset.x > prevPixelOffset.x)
		{
			validLocation = false;
		}
		else if (specRigidbody.PixelColliders[0].LowerLeft.y < this.m_cachedCameraLowerLeftPixels.y && pixelOffset.y < prevPixelOffset.y)
		{
			validLocation = false;
		}
		else if (specRigidbody.PixelColliders[1].UpperRight.y > this.m_cachedCameraUpperRightPixels.y && pixelOffset.y > prevPixelOffset.y)
		{
			validLocation = false;
		}
	}

	// Token: 0x17000CB6 RID: 3254
	// (get) Token: 0x060058BA RID: 22714 RVA: 0x0021E77C File Offset: 0x0021C97C
	public Vector2 CameraPos
	{
		get
		{
			CameraController cameraController = this.m_camera ?? GameManager.Instance.MainCameraController;
			float num = base.specRigidbody.HitboxPixelCollider.UnitBottom - GameManager.Instance.PrimaryPlayer.specRigidbody.UnitBottom;
			float num2 = Mathf.InverseLerp(this.minPlayerDist, this.maxPlayerDist, num);
			float num3 = Mathf.SmoothStep(this.playerDistOffset, 0f, num2);
			Vector2 vector = this.cameraPoint.transform.position.XY() + new Vector2(0f, -cameraController.Camera.orthographicSize + num3);
			if (this.m_cameraX != null)
			{
				vector.x = this.m_cameraX.Value;
			}
			return vector;
		}
	}

	// Token: 0x040051D4 RID: 20948
	public GameObject cameraPoint;

	// Token: 0x040051D5 RID: 20949
	public List<BossFinalRogueGunController> BaseGuns;

	// Token: 0x040051D6 RID: 20950
	public float minPlayerDist = -10f;

	// Token: 0x040051D7 RID: 20951
	public float maxPlayerDist = 14f;

	// Token: 0x040051D8 RID: 20952
	public float playerDistOffset = 7f;

	// Token: 0x040051D9 RID: 20953
	public Vector2 worldCenter;

	// Token: 0x040051DA RID: 20954
	public float worldRadius;

	// Token: 0x040051DB RID: 20955
	[Header("Background Scrolling")]
	public float minScrollDist = 8f;

	// Token: 0x040051DC RID: 20956
	public float maxScrollDist = 20f;

	// Token: 0x040051DD RID: 20957
	public float scrollMultiplier = 0.05f;

	// Token: 0x040051DE RID: 20958
	private float? m_cameraX;

	// Token: 0x040051DF RID: 20959
	private IntVector2 m_cachedCameraLowerLeftPixels;

	// Token: 0x040051E0 RID: 20960
	private IntVector2 m_cachedCameraUpperRightPixels;

	// Token: 0x040051E1 RID: 20961
	private PilotPastController m_pastController;

	// Token: 0x040051E2 RID: 20962
	private CameraController m_camera;

	// Token: 0x040051E3 RID: 20963
	private bool m_lockCamera;

	// Token: 0x040051E4 RID: 20964
	private bool m_suppressBaseGuns;
}
