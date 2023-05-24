using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;

// Token: 0x02000479 RID: 1145
[AddComponentMenu("Daikon Forge/Examples/Game Menu/Gravatar")]
[Serializable]
public class Gravatar : MonoBehaviour
{
	// Token: 0x06001A4C RID: 6732 RVA: 0x0007A888 File Offset: 0x00078A88
	private void OnEnable()
	{
		if (Gravatar.validator.IsMatch(this.email) && this.Sprite != null)
		{
			this.updateImage();
		}
	}

	// Token: 0x17000576 RID: 1398
	// (get) Token: 0x06001A4D RID: 6733 RVA: 0x0007A8B8 File Offset: 0x00078AB8
	// (set) Token: 0x06001A4E RID: 6734 RVA: 0x0007A8C0 File Offset: 0x00078AC0
	public string EmailAddress
	{
		get
		{
			return this.email;
		}
		set
		{
			if (value != this.email)
			{
				this.email = value;
				this.updateImage();
			}
		}
	}

	// Token: 0x06001A4F RID: 6735 RVA: 0x0007A8E0 File Offset: 0x00078AE0
	private void updateImage()
	{
		if (this.Sprite == null)
		{
			return;
		}
		if (Gravatar.validator.IsMatch(this.email))
		{
			string text = this.MD5(this.email.Trim().ToLower());
			this.Sprite.URL = string.Format("http://www.gravatar.com/avatar/{0}", text);
		}
		else
		{
			this.Sprite.Texture = this.Sprite.LoadingImage;
		}
	}

	// Token: 0x06001A50 RID: 6736 RVA: 0x0007A95C File Offset: 0x00078B5C
	public string MD5(string strToEncrypt)
	{
		UTF8Encoding utf8Encoding = new UTF8Encoding();
		byte[] bytes = utf8Encoding.GetBytes(strToEncrypt);
		MD5CryptoServiceProvider md5CryptoServiceProvider = new MD5CryptoServiceProvider();
		byte[] array = md5CryptoServiceProvider.ComputeHash(bytes);
		string text = string.Empty;
		for (int i = 0; i < array.Length; i++)
		{
			text += Convert.ToString(array[i], 16).PadLeft(2, '0');
		}
		return text.PadLeft(32, '0');
	}

	// Token: 0x0400149D RID: 5277
	private static Regex validator = new Regex("^[a-zA-Z][\\w\\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\\w\\.-]*[a-zA-Z0-9]\\.[a-zA-Z][a-zA-Z\\.]*[a-zA-Z]$", RegexOptions.IgnoreCase);

	// Token: 0x0400149E RID: 5278
	public dfWebSprite Sprite;

	// Token: 0x0400149F RID: 5279
	[SerializeField]
	protected string email = string.Empty;
}
