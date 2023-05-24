using System;
using System.Collections;
using UnityEngine;

// Token: 0x020016D0 RID: 5840
public class SquishyBounceWiggler : BraveBehaviour
{
	// Token: 0x17001449 RID: 5193
	// (get) Token: 0x060087D9 RID: 34777 RVA: 0x00384E54 File Offset: 0x00383054
	// (set) Token: 0x060087DA RID: 34778 RVA: 0x00384E5C File Offset: 0x0038305C
	public bool WiggleHold
	{
		get
		{
			return this.m_wiggleHold;
		}
		set
		{
			if (value && !this.m_wiggleHold)
			{
				if (this)
				{
					base.gameObject.SetLayerRecursively(LayerMask.NameToLayer("FG_Critical"));
				}
				this.ResetWiggle();
			}
			else if (!value && this.m_wiggleHold && this)
			{
				base.gameObject.SetLayerRecursively(LayerMask.NameToLayer("Unpixelated"));
			}
			this.m_wiggleHold = value;
		}
	}

	// Token: 0x060087DB RID: 34779 RVA: 0x00384EE0 File Offset: 0x003830E0
	private void Awake()
	{
		this.m_sprite = base.GetComponent<tk2dBaseSprite>();
	}

	// Token: 0x060087DC RID: 34780 RVA: 0x00384EF0 File Offset: 0x003830F0
	private void Start()
	{
		if (!this.m_sprite)
		{
			base.enabled = false;
		}
		Bounds bounds = this.m_sprite.GetBounds();
		this.m_spriteDimensions = new IntVector2(Mathf.RoundToInt(bounds.size.x / 0.0625f), Mathf.RoundToInt(bounds.size.y / 0.0625f));
		base.transform.position = base.transform.position.Quantize(0.0625f);
		if (base.specRigidbody)
		{
			base.specRigidbody.Reinitialize();
		}
		base.gameObject.SetLayerRecursively(LayerMask.NameToLayer("Unpixelated"));
		base.StartCoroutine(this.DoSquishyBounceWiggle());
	}

	// Token: 0x060087DD RID: 34781 RVA: 0x00384FBC File Offset: 0x003831BC
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x060087DE RID: 34782 RVA: 0x00384FC4 File Offset: 0x003831C4
	public void ResetWiggle()
	{
		if (this.m_sprite == null)
		{
			return;
		}
		MeshFilter component = base.GetComponent<MeshFilter>();
		Mesh mesh = component.mesh;
		Vector3[] vertices = mesh.vertices;
		Vector2[] uv = mesh.uv;
		Vector2 zero = Vector2.zero;
		Vector2 one = Vector2.one;
		Vector3 one2 = Vector3.one;
		Vector3 zero2 = Vector3.zero;
		this.SetClippedGeometry(this.m_sprite.GetCurrentSpriteDef(), vertices, uv, zero2, 0, one2, zero, one);
		Vector3[] normals = mesh.normals;
		Color[] colors = mesh.colors;
		mesh.Clear();
		mesh.vertices = vertices;
		mesh.uv = uv;
		mesh.normals = normals;
		mesh.colors = colors;
		int[] array = new int[6];
		tk2dSpriteGeomGen.SetClippedSpriteIndices(array, 0, 0, this.m_sprite.GetCurrentSpriteDef());
		mesh.triangles = array;
		mesh.RecalculateBounds();
		component.mesh = mesh;
	}

	// Token: 0x060087DF RID: 34783 RVA: 0x0038509C File Offset: 0x0038329C
	private IEnumerator DoSquishyBounceWiggle()
	{
		MeshFilter mf = base.GetComponent<MeshFilter>();
		Mesh sourceMesh = mf.mesh;
		Vector3[] vertices = sourceMesh.vertices;
		Vector2[] uvs = sourceMesh.uv;
		float horizontalPercentagePixel = 1f / (float)this.m_spriteDimensions.x;
		float verticalPercentagePixel = 1f / (float)this.m_spriteDimensions.y;
		int[] bottomOffsets = new int[5];
		int[] upTranslations = new int[] { 0, -3, 1, 2, -1 };
		float[] array = new float[] { 1f, 1f, 0f, 1f, 1f };
		array[2] = 1f - horizontalPercentagePixel * 2f;
		float[] horizontalScales = array;
		float[] array2 = new float[] { 1f, 0f, 1f, 1f, 1f };
		array2[1] = 1f - verticalPercentagePixel * 2f;
		float[] verticalScales = array2;
		float[] delays = new float[] { 0.8f, 0.1f, 0.1f, 0.1f, 0.1f };
		for (;;)
		{
			for (int i = 0; i < 5; i++)
			{
				if (this.WiggleHold)
				{
					i = 0;
				}
				bool hasOutlines = SpriteOutlineManager.HasOutline(this.m_sprite);
				tk2dBaseSprite[] outlineSprites = ((!hasOutlines) ? null : SpriteOutlineManager.GetOutlineSprites<tk2dBaseSprite>(this.m_sprite));
				Vector2 clipBottomLeft = new Vector2(0f, (float)bottomOffsets[i] * verticalPercentagePixel);
				Vector2 clipTopRight = new Vector2(1f, 1f);
				Vector3 scale = new Vector3(horizontalScales[i], verticalScales[i], 1f);
				Vector3 translation = new Vector3(0.0625f * ((1f - horizontalScales[i]) / 2f / horizontalPercentagePixel), 0.0625f * (float)upTranslations[i], 0f);
				this.SetClippedGeometry(this.m_sprite.GetCurrentSpriteDef(), vertices, uvs, translation, 0, scale, clipBottomLeft, clipTopRight);
				Vector3[] normals = sourceMesh.normals;
				Color[] colors = sourceMesh.colors;
				sourceMesh.Clear();
				sourceMesh.vertices = vertices;
				sourceMesh.uv = uvs;
				sourceMesh.normals = normals;
				sourceMesh.colors = colors;
				int[] indices = new int[6];
				tk2dSpriteGeomGen.SetClippedSpriteIndices(indices, 0, 0, this.m_sprite.GetCurrentSpriteDef());
				sourceMesh.triangles = indices;
				sourceMesh.RecalculateBounds();
				mf.mesh = sourceMesh;
				if (hasOutlines)
				{
					if (outlineSprites.Length == 1)
					{
						outlineSprites[0].scale = scale;
						outlineSprites[0].transform.localPosition = Vector3.Scale(translation, scale).WithZ(outlineSprites[0].transform.localPosition.z);
						SpriteOutlineManager.HandleSpriteChanged(outlineSprites[0]);
					}
					else
					{
						for (int j = 0; j < outlineSprites.Length; j++)
						{
							outlineSprites[j].scale = scale;
							outlineSprites[j].transform.localPosition = Vector3.Scale(IntVector2.Cardinals[j].ToVector3() * 0.0625f + translation, scale).WithZ(outlineSprites[j].transform.localPosition.z);
							SpriteOutlineManager.HandleSpriteChanged(outlineSprites[j]);
						}
					}
					this.m_sprite.UpdateZDepth();
				}
				float targetDelay = delays[i];
				float delayElapsed = 0f;
				while (delayElapsed < targetDelay)
				{
					delayElapsed += BraveTime.DeltaTime;
					if (i != 0)
					{
						base.transform.position = base.transform.position.Quantize(0.0625f);
					}
					yield return null;
				}
				if (i == 0)
				{
					while (this.WiggleHold)
					{
						if (i != 0)
						{
							base.transform.position = base.transform.position.Quantize(0.0625f);
						}
						yield return null;
					}
				}
			}
		}
		yield break;
	}

	// Token: 0x060087E0 RID: 34784 RVA: 0x003850B8 File Offset: 0x003832B8
	private void SetClippedGeometry(tk2dSpriteDefinition spriteDef, Vector3[] pos, Vector2[] uv, Vector3 translation, int offset, Vector3 scale, Vector2 clipBottomLeft, Vector2 clipTopRight)
	{
		Vector2 vector = clipBottomLeft;
		Vector2 vector2 = clipTopRight;
		Vector3 position = spriteDef.position0;
		Vector3 position2 = spriteDef.position3;
		Vector3 vector3 = new Vector3(Mathf.Lerp(position.x, position2.x, vector.x) * scale.x, Mathf.Lerp(position.y, position2.y, vector.y) * scale.y, position.z * scale.z);
		Vector3 vector4 = new Vector3(Mathf.Lerp(position.x, position2.x, vector2.x) * scale.x, Mathf.Lerp(position.y, position2.y, vector2.y) * scale.y, position.z * scale.z);
		pos[offset] = new Vector3(vector3.x, vector3.y, vector3.z) + translation;
		pos[offset + 1] = new Vector3(vector4.x, vector3.y, vector3.z) + translation;
		pos[offset + 2] = new Vector3(vector3.x, vector4.y, vector3.z) + translation;
		pos[offset + 3] = new Vector3(vector4.x, vector4.y, vector3.z) + translation;
		if (this.m_sprite.ShouldDoTilt)
		{
			for (int i = offset; i < offset + 4; i++)
			{
				if (this.m_sprite.IsPerpendicular)
				{
					int num = i;
					pos[num].z = pos[num].z - pos[i].y;
				}
				else
				{
					int num2 = i;
					pos[num2].z = pos[num2].z + pos[i].y;
				}
			}
		}
		if (spriteDef.flipped == tk2dSpriteDefinition.FlipMode.Tk2d)
		{
			Vector2 vector5 = new Vector2(Mathf.Lerp(spriteDef.uvs[0].x, spriteDef.uvs[3].x, vector.y), Mathf.Lerp(spriteDef.uvs[0].y, spriteDef.uvs[3].y, vector.x));
			Vector2 vector6 = new Vector2(Mathf.Lerp(spriteDef.uvs[0].x, spriteDef.uvs[3].x, vector2.y), Mathf.Lerp(spriteDef.uvs[0].y, spriteDef.uvs[3].y, vector2.x));
			uv[offset] = new Vector2(vector5.x, vector5.y);
			uv[offset + 1] = new Vector2(vector5.x, vector6.y);
			uv[offset + 2] = new Vector2(vector6.x, vector5.y);
			uv[offset + 3] = new Vector2(vector6.x, vector6.y);
		}
		else if (spriteDef.flipped == tk2dSpriteDefinition.FlipMode.TPackerCW)
		{
			Vector2 vector7 = new Vector2(Mathf.Lerp(spriteDef.uvs[0].x, spriteDef.uvs[3].x, vector.y), Mathf.Lerp(spriteDef.uvs[0].y, spriteDef.uvs[3].y, vector.x));
			Vector2 vector8 = new Vector2(Mathf.Lerp(spriteDef.uvs[0].x, spriteDef.uvs[3].x, vector2.y), Mathf.Lerp(spriteDef.uvs[0].y, spriteDef.uvs[3].y, vector2.x));
			uv[offset] = new Vector2(vector7.x, vector7.y);
			uv[offset + 2] = new Vector2(vector8.x, vector7.y);
			uv[offset + 1] = new Vector2(vector7.x, vector8.y);
			uv[offset + 3] = new Vector2(vector8.x, vector8.y);
		}
		else
		{
			Vector2 vector9 = new Vector2(Mathf.Lerp(spriteDef.uvs[0].x, spriteDef.uvs[3].x, vector.x), Mathf.Lerp(spriteDef.uvs[0].y, spriteDef.uvs[3].y, vector.y));
			Vector2 vector10 = new Vector2(Mathf.Lerp(spriteDef.uvs[0].x, spriteDef.uvs[3].x, vector2.x), Mathf.Lerp(spriteDef.uvs[0].y, spriteDef.uvs[3].y, vector2.y));
			uv[offset] = new Vector2(vector9.x, vector9.y);
			uv[offset + 1] = new Vector2(vector10.x, vector9.y);
			uv[offset + 2] = new Vector2(vector9.x, vector10.y);
			uv[offset + 3] = new Vector2(vector10.x, vector10.y);
		}
	}

	// Token: 0x04008D02 RID: 36098
	private bool m_wiggleHold;

	// Token: 0x04008D03 RID: 36099
	protected tk2dBaseSprite m_sprite;

	// Token: 0x04008D04 RID: 36100
	protected IntVector2 m_spriteDimensions;
}
