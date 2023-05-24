using System;
using UnityEngine;

// Token: 0x02001545 RID: 5445
public class LineReticleController : MonoBehaviour
{
	// Token: 0x06007C9E RID: 31902 RVA: 0x00323028 File Offset: 0x00321228
	public void Awake()
	{
		this.m_slicedSprite = base.GetComponent<tk2dSlicedSprite>();
	}

	// Token: 0x06007C9F RID: 31903 RVA: 0x00323038 File Offset: 0x00321238
	public void Init(Vector3 pos, Quaternion rotation, float maxLength)
	{
		this.m_slicedSprite.transform.position = pos;
		this.m_slicedSprite.transform.localRotation = rotation;
		this.m_currentLength = this.MinLength;
		this.m_maxLength = maxLength;
		this.m_state = LineReticleController.State.Growing;
		this.m_slicedSprite.dimensions = new Vector2(this.m_currentLength * 16f, 5f);
		this.m_slicedSprite.UpdateZDepth();
	}

	// Token: 0x06007CA0 RID: 31904 RVA: 0x003230B0 File Offset: 0x003212B0
	public void Update()
	{
		if (this.m_state == LineReticleController.State.Growing)
		{
			this.m_currentLength = Mathf.Min(this.m_currentLength + this.Speed * BraveTime.DeltaTime, this.m_maxLength);
			this.m_slicedSprite.dimensions = new Vector2(this.m_currentLength * 16f, 5f);
			this.m_slicedSprite.UpdateZDepth();
			if (this.m_currentLength >= this.m_maxLength)
			{
				this.m_state = LineReticleController.State.Static;
			}
		}
		else if (this.m_state == LineReticleController.State.Shrinking)
		{
			float currentLength = this.m_currentLength;
			this.m_currentLength = Mathf.Max(this.m_currentLength - this.Speed * BraveTime.DeltaTime, this.MinLength);
			base.transform.position += base.transform.rotation * new Vector3(currentLength - this.m_currentLength, 0f, 0f);
			this.m_slicedSprite.dimensions = new Vector2(this.m_currentLength * 16f, 5f);
			this.m_slicedSprite.UpdateZDepth();
			if (this.m_currentLength <= this.MinLength)
			{
				this.m_state = LineReticleController.State.Static;
				SpawnManager.Despawn(base.gameObject);
			}
		}
	}

	// Token: 0x06007CA1 RID: 31905 RVA: 0x003231FC File Offset: 0x003213FC
	public void Cleanup()
	{
		this.m_state = LineReticleController.State.Shrinking;
	}

	// Token: 0x04007F98 RID: 32664
	public float MinLength;

	// Token: 0x04007F99 RID: 32665
	public float Speed;

	// Token: 0x04007F9A RID: 32666
	private LineReticleController.State m_state = LineReticleController.State.Static;

	// Token: 0x04007F9B RID: 32667
	private tk2dSlicedSprite m_slicedSprite;

	// Token: 0x04007F9C RID: 32668
	private float m_currentLength;

	// Token: 0x04007F9D RID: 32669
	private float m_maxLength;

	// Token: 0x02001546 RID: 5446
	public enum State
	{
		// Token: 0x04007F9F RID: 32671
		Growing,
		// Token: 0x04007FA0 RID: 32672
		Static,
		// Token: 0x04007FA1 RID: 32673
		Shrinking
	}
}
