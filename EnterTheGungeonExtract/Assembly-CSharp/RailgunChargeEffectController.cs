using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020012AB RID: 4779
public class RailgunChargeEffectController : BraveBehaviour
{
	// Token: 0x06006AEA RID: 27370 RVA: 0x0029EE70 File Offset: 0x0029D070
	private void Start()
	{
		this.m_sprite = base.GetComponent<tk2dTiledSprite>();
		this.m_modTraversalTime = this.LineTraversalTime;
		this.m_sprite.color = this.ColorGradient.Evaluate(1f);
		this.m_ownerGun = base.gameObject.GetComponentInParent<Gun>();
		this.m_childLines = new List<tk2dTiledSprite>();
		this.UpdateAngleAndLength();
		if (this.lineMode == RailgunChargeEffectController.LineChargeMode.VERTICAL_CONVERGE)
		{
			AkSoundEngine.PostEvent("Play_WPN_dawnhammer_charge_01", base.gameObject);
		}
	}

	// Token: 0x06006AEB RID: 27371 RVA: 0x0029EEF0 File Offset: 0x0029D0F0
	private void OnEnable()
	{
		this.m_cachedParentTransform = base.transform.parent;
	}

	// Token: 0x06006AEC RID: 27372 RVA: 0x0029EF04 File Offset: 0x0029D104
	private tk2dTiledSprite CreateDuplicate(bool forceVisible = false)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.childLinePrefab);
		gameObject.transform.parent = base.transform;
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localRotation = Quaternion.identity;
		gameObject.GetComponent<Renderer>().enabled = forceVisible || base.renderer.enabled;
		return gameObject.GetComponent<tk2dTiledSprite>();
	}

	// Token: 0x06006AED RID: 27373 RVA: 0x0029EF78 File Offset: 0x0029D178
	private void UpdateAngleAndLength()
	{
		if (this.m_cachedParentTransform != base.transform.parent)
		{
			this.m_ownerGun = base.gameObject.GetComponentInParent<Gun>();
			this.m_cachedParentTransform = base.transform.parent;
		}
		if (this.lineMode == RailgunChargeEffectController.LineChargeMode.VERTICAL_CONVERGE)
		{
			this.m_sprite.dimensions = new Vector2(270f, this.Width);
			if (this.ImpactParticles && this.ImpactParticles.isPlaying)
			{
				this.ImpactParticles.Stop();
			}
		}
		else
		{
			if (this.lineMode == RailgunChargeEffectController.LineChargeMode.SCALING_PARALLEL && this.m_ownerGun && !this.m_ownerGun.IsFiring)
			{
				SpawnManager.Despawn(base.gameObject);
				return;
			}
			if (this.m_ownerGun)
			{
				base.transform.rotation = Quaternion.Euler(0f, 0f, this.m_ownerGun.CurrentAngle);
			}
			int num = CollisionMask.LayerToMask(CollisionLayer.HighObstacle);
			bool flag = false;
			RaycastResult raycastResult;
			if (this.overrideBeamLength != null)
			{
				raycastResult = null;
			}
			else
			{
				flag = PhysicsEngine.Instance.Raycast(base.transform.position.XY(), base.transform.right, 30f, out raycastResult, true, false, num, null, false, null, null);
				if (this.UseRaycast)
				{
					RaycastResult.Pool.Free(ref raycastResult);
					flag |= PhysicsEngine.Instance.Raycast(base.transform.position.XY(), base.transform.right, 30f, out raycastResult, false, true, num, null, false, null, null);
				}
			}
			if (flag && raycastResult != null)
			{
				if (this.m_sprite)
				{
					this.m_sprite.dimensions = new Vector2(raycastResult.Distance / 0.0625f, this.Width);
				}
				if (this.ImpactParticles)
				{
					this.ImpactParticles.transform.position = raycastResult.Contact.ToVector3ZUp(raycastResult.Contact.y - this.TargetHeightOffGround);
					if (this.m_hasConverged)
					{
						if (!this.ImpactParticles.isPlaying)
						{
							this.ImpactParticles.Play();
						}
					}
					else if (this.ImpactParticles.isPlaying)
					{
						this.ImpactParticles.Stop();
					}
				}
			}
			else if (this.overrideBeamLength != null)
			{
				if (this.m_sprite)
				{
					this.m_sprite.dimensions = new Vector2(this.overrideBeamLength.Value * 16f, this.Width);
				}
				if (this.ImpactParticles && this.m_sprite)
				{
					this.ImpactParticles.transform.position = this.m_sprite.transform.position + new Vector3(0f, -this.overrideBeamLength.Value, -this.overrideBeamLength.Value);
					if (!this.ImpactParticles.isPlaying)
					{
						this.ImpactParticles.Play();
					}
				}
			}
			else
			{
				if (this.m_sprite)
				{
					this.m_sprite.dimensions = new Vector2(480f, this.Width);
				}
				if (this.ImpactParticles && this.ImpactParticles.isPlaying)
				{
					this.ImpactParticles.Stop();
				}
			}
			RaycastResult.Pool.Free(ref raycastResult);
		}
		if (this.m_sprite)
		{
			this.m_sprite.IsPerpendicular = false;
			this.m_sprite.HeightOffGround = this.TargetHeightOffGround;
			this.m_sprite.UpdateZDepth();
			for (int i = 0; i < this.m_childLines.Count; i++)
			{
				this.m_childLines[i].dimensions = this.m_sprite.dimensions;
				if (this.lineMode == RailgunChargeEffectController.LineChargeMode.SCALING_PARALLEL && this.CompletionMap != null && this.CompletionMap.ContainsKey(this.m_childLines[i]))
				{
					float num2 = this.CompletionMap[this.m_childLines[i]];
					num2 = Mathf.Pow(num2, this.ScalingPower);
					this.m_childLines[i].dimensions = Vector2.Lerp(new Vector2(this.Width * 2f, this.Width * 2f), this.m_childLines[i].dimensions, num2);
				}
				this.m_childLines[i].IsPerpendicular = false;
				this.m_childLines[i].HeightOffGround = this.TargetHeightOffGround;
				this.m_childLines[i].UpdateZDepth();
			}
		}
	}

	// Token: 0x06006AEE RID: 27374 RVA: 0x0029F490 File Offset: 0x0029D690
	public void OnSpawned()
	{
		this.m_totalTimer = 0f;
		this.m_modTraversalTime = this.LineTraversalTime;
		this.m_lineTimer = 0f;
		this.UpdateAngleAndLength();
		this.m_hasConverged = false;
	}

	// Token: 0x06006AEF RID: 27375 RVA: 0x0029F4C4 File Offset: 0x0029D6C4
	public void OnDespawned()
	{
		base.StopAllCoroutines();
		for (int i = 0; i < this.m_childLines.Count; i++)
		{
			UnityEngine.Object.Destroy(this.m_childLines[i].gameObject);
		}
		this.m_childLines.Clear();
	}

	// Token: 0x06006AF0 RID: 27376 RVA: 0x0029F514 File Offset: 0x0029D714
	private IEnumerator HandleLine_ScalingParallel(float modTraversalTime)
	{
		if (this.CompletionMap == null)
		{
			this.CompletionMap = new Dictionary<tk2dTiledSprite, float>();
		}
		tk2dTiledSprite duplicate = this.CreateDuplicate(true);
		tk2dTiledSprite duplicate2 = this.CreateDuplicate(true);
		this.CompletionMap.Add(duplicate, 0f);
		this.CompletionMap.Add(duplicate2, 0f);
		this.m_childLines.Add(duplicate);
		this.m_childLines.Add(duplicate2);
		duplicate.transform.localPosition = new Vector3(-this.ScalingDistanceDepth, this.ScalingDistanceStart, 0f);
		duplicate2.transform.localPosition = new Vector3(-this.ScalingDistanceDepth, -this.ScalingDistanceStart, 0f);
		duplicate.color = this.ColorGradient.Evaluate(0f);
		duplicate2.color = this.ColorGradient.Evaluate(0f);
		float elapsed = 0f;
		while (elapsed < modTraversalTime)
		{
			elapsed += BraveTime.DeltaTime;
			float t = elapsed / modTraversalTime;
			if (this.SmoothLerpIn && this.SmoothLerpOut)
			{
				t = Mathf.SmoothStep(0f, 1f, t);
			}
			else if (this.SmoothLerpIn)
			{
				t = BraveMathCollege.SmoothStepToLinearStepInterpolate(0f, 1f, t);
			}
			else if (this.SmoothLerpOut)
			{
				t = BraveMathCollege.LinearToSmoothStepInterpolate(0f, 1f, t);
			}
			duplicate.transform.localPosition = Vector3.Lerp(new Vector3(-this.ScalingDistanceDepth, this.ScalingDistanceStart, 0f), Vector3.zero, t);
			duplicate2.transform.localPosition = Vector3.Lerp(new Vector3(-this.ScalingDistanceDepth, -this.ScalingDistanceStart, 0f), Vector3.zero, t);
			if (this.CompletionMap.ContainsKey(duplicate))
			{
				this.CompletionMap[duplicate] = t;
			}
			if (this.CompletionMap.ContainsKey(duplicate2))
			{
				this.CompletionMap[duplicate2] = t;
			}
			duplicate.color = this.ColorGradient.Evaluate(t);
			duplicate2.color = this.ColorGradient.Evaluate(t);
			yield return null;
		}
		yield break;
	}

	// Token: 0x06006AF1 RID: 27377 RVA: 0x0029F538 File Offset: 0x0029D738
	private IEnumerator HandleLine_SequentialParallel(float modTraversalTime)
	{
		tk2dTiledSprite duplicate = this.CreateDuplicate(false);
		tk2dTiledSprite duplicate2 = this.CreateDuplicate(false);
		this.m_childLines.Add(duplicate);
		this.m_childLines.Add(duplicate2);
		duplicate.transform.localPosition = new Vector3(0f, this.DistanceStart, 0f);
		duplicate2.transform.localPosition = new Vector3(0f, -this.DistanceStart, 0f);
		duplicate.color = this.ColorGradient.Evaluate(0f);
		duplicate2.color = this.ColorGradient.Evaluate(0f);
		float elapsed = 0f;
		while (elapsed < modTraversalTime)
		{
			elapsed += BraveTime.DeltaTime;
			float t = elapsed / modTraversalTime;
			if (this.SmoothLerpIn && this.SmoothLerpOut)
			{
				t = Mathf.SmoothStep(0f, 1f, t);
			}
			else if (this.SmoothLerpIn)
			{
				t = BraveMathCollege.SmoothStepToLinearStepInterpolate(0f, 1f, t);
			}
			else if (this.SmoothLerpOut)
			{
				t = BraveMathCollege.LinearToSmoothStepInterpolate(0f, 1f, t);
			}
			duplicate.transform.localPosition = Vector3.Lerp(new Vector3(0f, this.DistanceStart, 0f), Vector3.zero, t);
			duplicate2.transform.localPosition = Vector3.Lerp(new Vector3(0f, -this.DistanceStart, 0f), Vector3.zero, t);
			duplicate.color = this.ColorGradient.Evaluate(t);
			duplicate2.color = this.ColorGradient.Evaluate(t);
			yield return null;
		}
		yield break;
	}

	// Token: 0x06006AF2 RID: 27378 RVA: 0x0029F55C File Offset: 0x0029D75C
	private IEnumerator HandleLine_PyramidalConverge(float modTraversalTime)
	{
		tk2dTiledSprite duplicate = this.CreateDuplicate(true);
		tk2dTiledSprite duplicate2 = this.CreateDuplicate(true);
		tk2dTiledSprite duplicate3 = this.CreateDuplicate(true);
		duplicate.ShouldDoTilt = false;
		duplicate2.ShouldDoTilt = false;
		duplicate3.ShouldDoTilt = false;
		this.m_childLines.Add(duplicate);
		this.m_childLines.Add(duplicate2);
		this.m_childLines.Add(duplicate3);
		duplicate.transform.localRotation = Quaternion.Euler(0f, 0f, this.SolidAngleStart);
		duplicate2.transform.localRotation = Quaternion.Euler(120f, 0f, 0f) * Quaternion.Euler(0f, 0f, this.SolidAngleStart);
		duplicate3.transform.localRotation = Quaternion.Euler(240f, 0f, 0f) * Quaternion.Euler(0f, 0f, this.SolidAngleStart);
		duplicate.color = this.ColorGradient.Evaluate(0f);
		duplicate2.color = this.ColorGradient.Evaluate(0f);
		duplicate3.color = this.ColorGradient.Evaluate(0f);
		float elapsed = 0f;
		while (elapsed < modTraversalTime)
		{
			elapsed += BraveTime.DeltaTime;
			float t = elapsed / modTraversalTime;
			if (this.SmoothLerpIn && this.SmoothLerpOut)
			{
				t = Mathf.SmoothStep(0f, 1f, t);
			}
			else if (this.SmoothLerpIn)
			{
				t = BraveMathCollege.SmoothStepToLinearStepInterpolate(0f, 1f, t);
			}
			else if (this.SmoothLerpOut)
			{
				t = BraveMathCollege.LinearToSmoothStepInterpolate(0f, 1f, t);
			}
			float baseAngle = elapsed * this.SolidRotationSpeed;
			float solidAngle = Mathf.Lerp(this.SolidAngleStart, 0f, t);
			duplicate.transform.localRotation = Quaternion.Euler(baseAngle, 0f, 0f) * Quaternion.Euler(0f, 0f, solidAngle);
			duplicate2.transform.localRotation = Quaternion.Euler(baseAngle + 120f, 0f, 0f) * Quaternion.Euler(0f, 0f, solidAngle);
			duplicate3.transform.localRotation = Quaternion.Euler(baseAngle + 240f, 0f, 0f) * Quaternion.Euler(0f, 0f, solidAngle);
			duplicate.color = this.ColorGradient.Evaluate(t);
			duplicate2.color = this.ColorGradient.Evaluate(t);
			duplicate3.color = this.ColorGradient.Evaluate(t);
			duplicate.UpdateZDepth();
			duplicate2.UpdateZDepth();
			duplicate3.UpdateZDepth();
			yield return null;
		}
		yield break;
	}

	// Token: 0x06006AF3 RID: 27379 RVA: 0x0029F580 File Offset: 0x0029D780
	private IEnumerator HandleLine_VerticalConverge(float modTraversalTime)
	{
		tk2dTiledSprite duplicate = this.CreateDuplicate(true);
		tk2dTiledSprite duplicate2 = this.CreateDuplicate(true);
		tk2dTiledSprite duplicate3 = this.CreateDuplicate(true);
		this.m_childLines.Add(duplicate);
		this.m_childLines.Add(duplicate2);
		this.m_childLines.Add(duplicate3);
		duplicate.transform.localRotation = Quaternion.Euler(0f, 0f, 90f);
		duplicate2.transform.localRotation = Quaternion.Euler(0f, 0f, 90f);
		duplicate3.transform.localRotation = Quaternion.Euler(0f, 0f, 90f);
		duplicate.color = this.ColorGradient.Evaluate(0f);
		duplicate2.color = this.ColorGradient.Evaluate(0f);
		duplicate3.color = this.ColorGradient.Evaluate(0f);
		float elapsed = 0f;
		while (elapsed < modTraversalTime)
		{
			elapsed += BraveTime.DeltaTime;
			if (this.IsManuallyControlled)
			{
				elapsed = Mathf.Clamp01(this.ManualCompletionPercentage) * modTraversalTime;
			}
			float t = elapsed / modTraversalTime;
			if (this.SmoothLerpIn && this.SmoothLerpOut)
			{
				t = Mathf.SmoothStep(0f, 1f, t);
			}
			else if (this.SmoothLerpIn)
			{
				t = BraveMathCollege.SmoothStepToLinearStepInterpolate(0f, 1f, t);
			}
			else if (this.SmoothLerpOut)
			{
				t = BraveMathCollege.LinearToSmoothStepInterpolate(0f, 1f, t);
			}
			float baseAngle = elapsed * this.SolidRotationSpeed;
			float maxDistance = 2f;
			duplicate.transform.localPosition = Quaternion.Euler(0f, 0f, baseAngle) * new Vector3(maxDistance * (1f - t), 0f, 0f);
			duplicate2.transform.localPosition = Quaternion.Euler(0f, 0f, baseAngle + 120f) * new Vector3(maxDistance * (1f - t), 0f, 0f);
			duplicate3.transform.localPosition = Quaternion.Euler(0f, 0f, baseAngle + 240f) * new Vector3(maxDistance * (1f - t), 0f, 0f);
			duplicate.color = this.ColorGradient.Evaluate(t);
			duplicate2.color = this.ColorGradient.Evaluate(t);
			duplicate3.color = this.ColorGradient.Evaluate(t);
			duplicate.UpdateZDepth();
			duplicate2.UpdateZDepth();
			duplicate3.UpdateZDepth();
			yield return null;
		}
		yield break;
	}

	// Token: 0x06006AF4 RID: 27380 RVA: 0x0029F5A4 File Offset: 0x0029D7A4
	private IEnumerator HandleLine_TriangularConverge(float modTraversalTime)
	{
		tk2dTiledSprite duplicate = this.CreateDuplicate(false);
		tk2dTiledSprite duplicate2 = this.CreateDuplicate(false);
		this.m_childLines.Add(duplicate);
		this.m_childLines.Add(duplicate2);
		duplicate.transform.localRotation = Quaternion.Euler(0f, 0f, this.AngleStart);
		duplicate2.transform.localRotation = Quaternion.Euler(0f, 0f, -this.AngleStart);
		duplicate.color = this.ColorGradient.Evaluate(0f);
		duplicate2.color = this.ColorGradient.Evaluate(0f);
		float elapsed = 0f;
		while (elapsed < modTraversalTime)
		{
			elapsed += BraveTime.DeltaTime;
			float t = elapsed / modTraversalTime;
			if (this.SmoothLerpIn && this.SmoothLerpOut)
			{
				t = Mathf.SmoothStep(0f, 1f, t);
			}
			else if (this.SmoothLerpIn)
			{
				t = BraveMathCollege.SmoothStepToLinearStepInterpolate(0f, 1f, t);
			}
			else if (this.SmoothLerpOut)
			{
				t = BraveMathCollege.LinearToSmoothStepInterpolate(0f, 1f, t);
			}
			duplicate.transform.localRotation = Quaternion.Euler(0f, 0f, Mathf.Lerp(this.AngleStart, 0f, t));
			duplicate2.transform.localRotation = Quaternion.Euler(0f, 0f, Mathf.Lerp(-this.AngleStart, 0f, t));
			duplicate.color = this.ColorGradient.Evaluate(t);
			duplicate2.color = this.ColorGradient.Evaluate(t);
			yield return null;
		}
		yield break;
	}

	// Token: 0x06006AF5 RID: 27381 RVA: 0x0029F5C8 File Offset: 0x0029D7C8
	private void Update()
	{
		this.m_lineTimer -= BraveTime.DeltaTime;
		this.m_totalTimer += BraveTime.DeltaTime;
		if (this.m_totalTimer < this.StopCreatingLinesTime)
		{
			if (this.lineMode == RailgunChargeEffectController.LineChargeMode.SEQUENTIAL_PARALLEL && this.m_lineTimer <= 0f)
			{
				base.StartCoroutine(this.HandleLine_SequentialParallel((!this.SequentialLinesReduceTraversalTime) ? this.LineTraversalTime : this.m_modTraversalTime));
				this.m_lineTimer += this.NewLineFrequency;
				this.m_modTraversalTime -= this.NewLineFrequency;
			}
			else if (this.lineMode == RailgunChargeEffectController.LineChargeMode.TRIANGULAR_CONVERGE && this.m_lineTimer <= 0f)
			{
				base.StartCoroutine(this.HandleLine_TriangularConverge((!this.SequentialLinesReduceTraversalTime) ? this.LineTraversalTime : this.m_modTraversalTime));
				this.m_lineTimer += this.NewLineFrequency;
				this.m_modTraversalTime -= this.NewLineFrequency;
			}
			else if (this.lineMode == RailgunChargeEffectController.LineChargeMode.PYRAMIDAL_CONVERGE && this.m_lineTimer <= 0f)
			{
				base.StartCoroutine(this.HandleLine_PyramidalConverge((!this.SequentialLinesReduceTraversalTime) ? this.LineTraversalTime : this.m_modTraversalTime));
				this.m_lineTimer += this.NewLineFrequency;
				this.m_modTraversalTime -= this.NewLineFrequency;
			}
			else if (this.lineMode == RailgunChargeEffectController.LineChargeMode.VERTICAL_CONVERGE && this.m_lineTimer <= 0f)
			{
				base.StartCoroutine(this.HandleLine_VerticalConverge((!this.SequentialLinesReduceTraversalTime) ? this.LineTraversalTime : this.m_modTraversalTime));
				this.m_lineTimer += this.NewLineFrequency;
				this.m_modTraversalTime -= this.NewLineFrequency;
			}
			else if (this.lineMode == RailgunChargeEffectController.LineChargeMode.SCALING_PARALLEL && this.m_lineTimer <= 0f)
			{
				base.StartCoroutine(this.HandleLine_ScalingParallel((!this.SequentialLinesReduceTraversalTime) ? this.LineTraversalTime : this.m_modTraversalTime));
				this.m_lineTimer += this.NewLineFrequency;
				this.m_modTraversalTime -= this.NewLineFrequency;
			}
		}
		else
		{
			this.m_hasConverged = true;
			if (this.DestroyedOnCompletion)
			{
				SpawnManager.Despawn(base.gameObject);
			}
		}
	}

	// Token: 0x06006AF6 RID: 27382 RVA: 0x0029F858 File Offset: 0x0029DA58
	private void LateUpdate()
	{
		this.UpdateAngleAndLength();
	}

	// Token: 0x06006AF7 RID: 27383 RVA: 0x0029F860 File Offset: 0x0029DA60
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x0400678A RID: 26506
	public RailgunChargeEffectController.LineChargeMode lineMode;

	// Token: 0x0400678B RID: 26507
	public GameObject childLinePrefab;

	// Token: 0x0400678C RID: 26508
	public float Width = 1f;

	// Token: 0x0400678D RID: 26509
	public float NewLineFrequency = 0.5f;

	// Token: 0x0400678E RID: 26510
	public float LineTraversalTime = 0.5f;

	// Token: 0x0400678F RID: 26511
	public float StopCreatingLinesTime = 3f;

	// Token: 0x04006790 RID: 26512
	public bool SequentialLinesReduceTraversalTime;

	// Token: 0x04006791 RID: 26513
	[ShowInInspectorIf("lineMode", 0, false)]
	public float DistanceStart = 1f;

	// Token: 0x04006792 RID: 26514
	[ShowInInspectorIf("lineMode", 1, false)]
	public float AngleStart = 90f;

	// Token: 0x04006793 RID: 26515
	[ShowInInspectorIf("lineMode", 2, false)]
	public float SolidAngleStart = 60f;

	// Token: 0x04006794 RID: 26516
	[ShowInInspectorIf("lineMode", 2, false)]
	public float SolidRotationSpeed = 180f;

	// Token: 0x04006795 RID: 26517
	[ShowInInspectorIf("lineMode", 4, false)]
	public float ScalingDistanceDepth = 0.25f;

	// Token: 0x04006796 RID: 26518
	[ShowInInspectorIf("lineMode", 4, false)]
	public float ScalingDistanceStart = 1f;

	// Token: 0x04006797 RID: 26519
	[ShowInInspectorIf("lineMode", 4, false)]
	public float ScalingPower = 3f;

	// Token: 0x04006798 RID: 26520
	public bool SmoothLerpIn;

	// Token: 0x04006799 RID: 26521
	public bool SmoothLerpOut;

	// Token: 0x0400679A RID: 26522
	public bool UseRaycast;

	// Token: 0x0400679B RID: 26523
	public bool DestroyedOnCompletion;

	// Token: 0x0400679C RID: 26524
	public float TargetHeightOffGround = -0.5f;

	// Token: 0x0400679D RID: 26525
	public Gradient ColorGradient;

	// Token: 0x0400679E RID: 26526
	public ParticleSystem ImpactParticles;

	// Token: 0x0400679F RID: 26527
	private Gun m_ownerGun;

	// Token: 0x040067A0 RID: 26528
	private tk2dTiledSprite m_sprite;

	// Token: 0x040067A1 RID: 26529
	private List<tk2dTiledSprite> m_childLines;

	// Token: 0x040067A2 RID: 26530
	private float m_lineTimer;

	// Token: 0x040067A3 RID: 26531
	private float m_totalTimer;

	// Token: 0x040067A4 RID: 26532
	private float m_modTraversalTime;

	// Token: 0x040067A5 RID: 26533
	private bool m_hasConverged;

	// Token: 0x040067A6 RID: 26534
	public float? overrideBeamLength;

	// Token: 0x040067A7 RID: 26535
	[NonSerialized]
	public bool IsManuallyControlled;

	// Token: 0x040067A8 RID: 26536
	[NonSerialized]
	public float ManualCompletionPercentage;

	// Token: 0x040067A9 RID: 26537
	private Transform m_cachedParentTransform;

	// Token: 0x040067AA RID: 26538
	private Dictionary<tk2dTiledSprite, float> CompletionMap;

	// Token: 0x020012AC RID: 4780
	public enum LineChargeMode
	{
		// Token: 0x040067AC RID: 26540
		SEQUENTIAL_PARALLEL,
		// Token: 0x040067AD RID: 26541
		TRIANGULAR_CONVERGE,
		// Token: 0x040067AE RID: 26542
		PYRAMIDAL_CONVERGE,
		// Token: 0x040067AF RID: 26543
		VERTICAL_CONVERGE,
		// Token: 0x040067B0 RID: 26544
		SCALING_PARALLEL
	}
}
