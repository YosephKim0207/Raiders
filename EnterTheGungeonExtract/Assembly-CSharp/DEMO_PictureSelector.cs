using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200048D RID: 1165
public class DEMO_PictureSelector : MonoBehaviour
{
	// Token: 0x06001AC8 RID: 6856 RVA: 0x0007D3DC File Offset: 0x0007B5DC
	public void OnEnable()
	{
		this.myImage = base.GetComponent<dfTextureSprite>();
	}

	// Token: 0x06001AC9 RID: 6857 RVA: 0x0007D3EC File Offset: 0x0007B5EC
	public IEnumerator OnDoubleTapGesture()
	{
		if (this.DisplayImage == null)
		{
			Debug.LogWarning("The DisplayImage property is not configured, cannot select the image");
			yield break;
		}
		dfTextureSprite photo = UnityEngine.Object.Instantiate<GameObject>(this.DisplayImage.gameObject).GetComponent<dfTextureSprite>();
		this.myImage.GetManager().AddControl(photo);
		photo.Texture = this.myImage.Texture;
		photo.Size = this.myImage.Size;
		photo.RelativePosition = this.myImage.GetAbsolutePosition();
		photo.transform.rotation = Quaternion.identity;
		photo.BringToFront();
		photo.Opacity = 1f;
		photo.IsVisible = true;
		Vector2 screenSize = this.myImage.GetManager().GetScreenSize();
		Vector2 fullSize = new Vector2((float)photo.Texture.width, (float)photo.Texture.height);
		Vector2 displaySize = DEMO_PictureSelector.fitImage(screenSize.x * 0.75f, screenSize.y * 0.75f, fullSize.x, fullSize.y);
		Vector3 displayPosition = new Vector3((screenSize.x - displaySize.x) * 0.5f, (screenSize.y - displaySize.y) * 0.5f);
		dfAnimatedVector3 animatedPosition = new dfAnimatedVector3(this.myImage.GetAbsolutePosition(), displayPosition, 0.2f);
		dfAnimatedVector2 animatedSize = new dfAnimatedVector2(this.myImage.Size, displaySize, 0.2f);
		while (!animatedPosition.IsDone || !animatedSize.IsDone)
		{
			photo.Size = animatedSize;
			photo.RelativePosition = animatedPosition;
			yield return null;
		}
		yield break;
	}

	// Token: 0x06001ACA RID: 6858 RVA: 0x0007D408 File Offset: 0x0007B608
	private static Vector2 fitImage(float maxWidth, float maxHeight, float imageWidth, float imageHeight)
	{
		float num = maxWidth / imageWidth;
		float num2 = maxHeight / imageHeight;
		float num3 = Mathf.Min(num, num2);
		return new Vector2(Mathf.Floor(imageWidth * num3), Mathf.Ceil(imageHeight * num3));
	}

	// Token: 0x0400151A RID: 5402
	public dfTextureSprite DisplayImage;

	// Token: 0x0400151B RID: 5403
	protected dfTextureSprite myImage;
}
