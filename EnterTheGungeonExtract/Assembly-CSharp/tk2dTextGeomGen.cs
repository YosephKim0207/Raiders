using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000B92 RID: 2962
public static class tk2dTextGeomGen
{
	// Token: 0x06003DDB RID: 15835 RVA: 0x0013617C File Offset: 0x0013437C
	public static tk2dTextGeomGen.GeomData Data(tk2dTextMeshData textMeshData, tk2dFontData fontData, string formattedText)
	{
		tk2dTextGeomGen.tmpData.textMeshData = textMeshData;
		tk2dTextGeomGen.tmpData.fontInst = fontData;
		tk2dTextGeomGen.tmpData.formattedText = formattedText;
		return tk2dTextGeomGen.tmpData;
	}

	// Token: 0x06003DDC RID: 15836 RVA: 0x001361A4 File Offset: 0x001343A4
	public static Vector2 GetMeshDimensionsForString(string str, tk2dTextGeomGen.GeomData geomData)
	{
		tk2dTextMeshData textMeshData = geomData.textMeshData;
		tk2dFontData fontInst = geomData.fontInst;
		float num = 0f;
		float num2 = 0f;
		float num3 = 0f;
		bool flag = false;
		int num4 = 0;
		int num5 = 0;
		while (num5 < str.Length && num4 < textMeshData.maxChars)
		{
			if (flag)
			{
				flag = false;
			}
			else
			{
				int num6 = (int)str[num5];
				if (num6 == 10)
				{
					num = Mathf.Max(num2, num);
					num2 = 0f;
					num3 -= (fontInst.lineHeight + textMeshData.lineSpacing) * textMeshData.scale.y;
				}
				else
				{
					if (textMeshData.inlineStyling && num6 == 94 && num5 + 1 < str.Length)
					{
						if (str[num5 + 1] != '^')
						{
							int num7 = 0;
							char c = str[num5 + 1];
							if (c != 'C')
							{
								if (c != 'G')
								{
									if (c != 'c')
									{
										if (c == 'g')
										{
											num7 = 9;
										}
									}
									else
									{
										num7 = 5;
									}
								}
								else
								{
									num7 = 17;
								}
							}
							else
							{
								num7 = 9;
							}
							num5 += num7;
							goto IL_233;
						}
						flag = true;
					}
					bool flag2 = num6 == 94;
					tk2dFontChar tk2dFontChar;
					if (fontInst.useDictionary)
					{
						if (!fontInst.charDict.ContainsKey(num6))
						{
							num6 = 0;
						}
						tk2dFontChar = fontInst.charDict[num6];
					}
					else
					{
						if (num6 >= fontInst.chars.Length)
						{
							num6 = 0;
						}
						tk2dFontChar = fontInst.chars[num6];
					}
					if (flag2)
					{
					}
					num2 += (tk2dFontChar.advance + textMeshData.spacing) * textMeshData.scale.x;
					if (textMeshData.kerning && num5 < str.Length - 1)
					{
						foreach (tk2dFontKerning tk2dFontKerning in fontInst.kerning)
						{
							if (tk2dFontKerning.c0 == (int)str[num5] && tk2dFontKerning.c1 == (int)str[num5 + 1])
							{
								num2 += tk2dFontKerning.amount * textMeshData.scale.x;
								break;
							}
						}
					}
					num4++;
				}
			}
			IL_233:
			num5++;
		}
		num = Mathf.Max(num2, num);
		num3 -= (fontInst.lineHeight + textMeshData.lineSpacing) * textMeshData.scale.y;
		return new Vector2(num, num3);
	}

	// Token: 0x06003DDD RID: 15837 RVA: 0x00136434 File Offset: 0x00134634
	public static float GetYAnchorForHeight(float textHeight, tk2dTextGeomGen.GeomData geomData)
	{
		tk2dTextMeshData textMeshData = geomData.textMeshData;
		tk2dFontData fontInst = geomData.fontInst;
		int num = (int)(textMeshData.anchor / TextAnchor.MiddleLeft);
		float num2 = (fontInst.lineHeight + textMeshData.lineSpacing) * textMeshData.scale.y;
		if (num == 0)
		{
			return -num2;
		}
		if (num != 1)
		{
			if (num != 2)
			{
				return -num2;
			}
			return -textHeight - num2;
		}
		else
		{
			float num3 = -textHeight / 2f - num2;
			if (fontInst.version >= 2)
			{
				float num4 = fontInst.texelSize.y * textMeshData.scale.y;
				return Mathf.Floor(num3 / num4) * num4;
			}
			return num3;
		}
	}

	// Token: 0x06003DDE RID: 15838 RVA: 0x001364D8 File Offset: 0x001346D8
	public static float GetXAnchorForWidth(float lineWidth, tk2dTextGeomGen.GeomData geomData)
	{
		tk2dTextMeshData textMeshData = geomData.textMeshData;
		tk2dFontData fontInst = geomData.fontInst;
		int num = (int)(textMeshData.anchor % TextAnchor.MiddleLeft);
		if (num == 0)
		{
			return 0f;
		}
		if (num != 1)
		{
			if (num != 2)
			{
				return 0f;
			}
			return -lineWidth;
		}
		else
		{
			float num2 = -lineWidth / 2f;
			if (fontInst.version >= 2)
			{
				float num3 = fontInst.texelSize.x * textMeshData.scale.x;
				return Mathf.Floor(num2 / num3) * num3;
			}
			return num2;
		}
	}

	// Token: 0x06003DDF RID: 15839 RVA: 0x00136560 File Offset: 0x00134760
	private static void PostAlignTextData(Vector3[] pos, int offset, int targetStart, int targetEnd, float offsetX, List<int> inlineSpritePositions = null)
	{
		for (int i = targetStart * 4; i < targetEnd * 4; i++)
		{
			Vector3 vector = pos[offset + i];
			vector.x += offsetX;
			pos[offset + i] = vector;
		}
		if (inlineSpritePositions != null)
		{
			for (int j = 0; j < inlineSpritePositions.Count; j++)
			{
				List<Vector3> list;
				int num;
				(list = tk2dTextGeomGen.inlineSpriteOffsetsForLastString)[num = inlineSpritePositions[j]] = list[num] + new Vector3(offsetX, 0f, 0f);
			}
		}
	}

	// Token: 0x06003DE0 RID: 15840 RVA: 0x00136608 File Offset: 0x00134808
	private static int GetFullHexColorComponent(int c1, int c2)
	{
		int num = 0;
		if (c1 >= 48 && c1 <= 57)
		{
			num += (c1 - 48) * 16;
		}
		else if (c1 >= 97 && c1 <= 102)
		{
			num += (10 + c1 - 97) * 16;
		}
		else
		{
			if (c1 < 65 || c1 > 70)
			{
				return -1;
			}
			num += (10 + c1 - 65) * 16;
		}
		if (c2 >= 48 && c2 <= 57)
		{
			num += c2 - 48;
		}
		else if (c2 >= 97 && c2 <= 102)
		{
			num += 10 + c2 - 97;
		}
		else
		{
			if (c2 < 65 || c2 > 70)
			{
				return -1;
			}
			num += 10 + c2 - 65;
		}
		return num;
	}

	// Token: 0x06003DE1 RID: 15841 RVA: 0x001366DC File Offset: 0x001348DC
	private static int GetCompactHexColorComponent(int c)
	{
		if (c >= 48 && c <= 57)
		{
			return (c - 48) * 17;
		}
		if (c >= 97 && c <= 102)
		{
			return (10 + c - 97) * 17;
		}
		if (c >= 65 && c <= 70)
		{
			return (10 + c - 65) * 17;
		}
		return -1;
	}

	// Token: 0x06003DE2 RID: 15842 RVA: 0x00136738 File Offset: 0x00134938
	private static int GetStyleHexColor(string str, bool fullHex, ref Color32 color)
	{
		int num;
		int num2;
		int num3;
		int num4;
		if (fullHex)
		{
			if (str.Length < 8)
			{
				return 1;
			}
			num = tk2dTextGeomGen.GetFullHexColorComponent((int)str[0], (int)str[1]);
			num2 = tk2dTextGeomGen.GetFullHexColorComponent((int)str[2], (int)str[3]);
			num3 = tk2dTextGeomGen.GetFullHexColorComponent((int)str[4], (int)str[5]);
			num4 = tk2dTextGeomGen.GetFullHexColorComponent((int)str[6], (int)str[7]);
		}
		else
		{
			if (str.Length < 4)
			{
				return 1;
			}
			num = tk2dTextGeomGen.GetCompactHexColorComponent((int)str[0]);
			num2 = tk2dTextGeomGen.GetCompactHexColorComponent((int)str[1]);
			num3 = tk2dTextGeomGen.GetCompactHexColorComponent((int)str[2]);
			num4 = tk2dTextGeomGen.GetCompactHexColorComponent((int)str[3]);
		}
		if (num == -1 || num2 == -1 || num3 == -1 || num4 == -1)
		{
			return 1;
		}
		color = new Color32((byte)num, (byte)num2, (byte)num3, (byte)num4);
		return 0;
	}

	// Token: 0x06003DE3 RID: 15843 RVA: 0x00136820 File Offset: 0x00134A20
	private static int SetColorsFromStyleCommand(string args, bool twoColors, bool fullHex)
	{
		int num = ((!twoColors) ? 1 : 2) * ((!fullHex) ? 4 : 8);
		bool flag = false;
		if (args.Length >= num)
		{
			if (tk2dTextGeomGen.GetStyleHexColor(args, fullHex, ref tk2dTextGeomGen.meshTopColor) != 0)
			{
				flag = true;
			}
			if (twoColors)
			{
				string text = args.Substring((!fullHex) ? 4 : 8);
				if (tk2dTextGeomGen.GetStyleHexColor(text, fullHex, ref tk2dTextGeomGen.meshBottomColor) != 0)
				{
					flag = true;
				}
			}
			else
			{
				tk2dTextGeomGen.meshBottomColor = tk2dTextGeomGen.meshTopColor;
			}
		}
		else
		{
			flag = true;
		}
		if (flag)
		{
			tk2dTextGeomGen.meshTopColor = (tk2dTextGeomGen.meshBottomColor = tk2dTextGeomGen.errorColor);
		}
		return num;
	}

	// Token: 0x06003DE4 RID: 15844 RVA: 0x001368C4 File Offset: 0x00134AC4
	private static void SetGradientTexUFromStyleCommand(int arg)
	{
		tk2dTextGeomGen.meshGradientTexU = (float)(arg - 48) / (float)((tk2dTextGeomGen.curGradientCount <= 0) ? 1 : tk2dTextGeomGen.curGradientCount);
	}

	// Token: 0x06003DE5 RID: 15845 RVA: 0x001368E8 File Offset: 0x00134AE8
	private static int HandleStyleCommand(string cmd)
	{
		if (cmd.Length == 0)
		{
			return 0;
		}
		int num = (int)cmd[0];
		string text = cmd.Substring(1);
		int num2 = 0;
		if (num != 67)
		{
			if (num != 71)
			{
				if (num != 99)
				{
					if (num == 103)
					{
						num2 = 1 + tk2dTextGeomGen.SetColorsFromStyleCommand(text, true, false);
					}
				}
				else
				{
					num2 = 1 + tk2dTextGeomGen.SetColorsFromStyleCommand(text, false, false);
				}
			}
			else
			{
				num2 = 1 + tk2dTextGeomGen.SetColorsFromStyleCommand(text, true, true);
			}
		}
		else
		{
			num2 = 1 + tk2dTextGeomGen.SetColorsFromStyleCommand(text, false, true);
		}
		if (num >= 48 && num <= 57)
		{
			tk2dTextGeomGen.SetGradientTexUFromStyleCommand(num);
			num2 = 1;
		}
		return num2;
	}

	// Token: 0x06003DE6 RID: 15846 RVA: 0x00136994 File Offset: 0x00134B94
	public static void GetTextMeshGeomDesc(out int numVertices, out int numIndices, tk2dTextGeomGen.GeomData geomData)
	{
		tk2dTextMeshData textMeshData = geomData.textMeshData;
		numVertices = textMeshData.maxChars * 4;
		numIndices = textMeshData.maxChars * 6;
	}

	// Token: 0x06003DE7 RID: 15847 RVA: 0x001369BC File Offset: 0x00134BBC
	public static int SetTextMeshGeom(Vector3[] pos, Vector2[] uv, Vector2[] uv2, Color32[] color, int offset, tk2dTextGeomGen.GeomData geomData, int visibleCharacters, Vector2[] characterOffsetVectors, bool[] rainbowValues)
	{
		tk2dTextMeshData textMeshData = geomData.textMeshData;
		tk2dFontData fontInst = geomData.fontInst;
		string formattedText = geomData.formattedText;
		tk2dTextGeomGen.inlineSpriteOffsetsForLastString = new List<Vector3>();
		tk2dTextGeomGen.meshTopColor = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
		tk2dTextGeomGen.meshBottomColor = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
		tk2dTextGeomGen.meshGradientTexU = (float)textMeshData.textureGradient / (float)((fontInst.gradientCount <= 0) ? 1 : fontInst.gradientCount);
		tk2dTextGeomGen.curGradientCount = fontInst.gradientCount;
		float yanchorForHeight = tk2dTextGeomGen.GetYAnchorForHeight(tk2dTextGeomGen.GetMeshDimensionsForString(geomData.formattedText, geomData).y, geomData);
		float num = 0f;
		float num2 = 0f;
		int num3 = 0;
		int num4 = 0;
		int num5 = 0;
		List<int> list = new List<int>();
		bool flag = false;
		for (int i = 0; i < rainbowValues.Length; i++)
		{
			flag = flag || rainbowValues[i];
		}
		int num6 = 0;
		while (num6 < formattedText.Length && num3 < textMeshData.maxChars)
		{
			int num7 = (int)formattedText[num6];
			tk2dFontChar tk2dFontChar = null;
			bool flag2 = false;
			if (num7 == 91 && num6 < formattedText.Length - 1 && formattedText[num6 + 1] != ']')
			{
				for (int j = num6; j < formattedText.Length; j++)
				{
					char c = formattedText[j];
					if (c == ']')
					{
						flag2 = true;
						int num8 = j - num6;
						string text = formattedText.Substring(num6 + 9, num8 - 10);
						tk2dFontChar = tk2dTextGeomGen.GetSpecificSpriteCharDef(text);
						num6 += num8;
						num5 += num8;
						break;
					}
				}
			}
			bool flag3 = num7 == 94;
			if (!flag2)
			{
				if (fontInst.useDictionary)
				{
					if (!fontInst.charDict.ContainsKey(num7))
					{
						num7 = 0;
					}
					tk2dFontChar = fontInst.charDict[num7];
				}
				else
				{
					if (num7 >= fontInst.chars.Length)
					{
						num7 = 0;
					}
					tk2dFontChar = fontInst.chars[num7];
				}
			}
			if (flag3)
			{
				num7 = 94;
			}
			if (num7 == 10)
			{
				float num9 = num;
				int num10 = num3;
				if (num4 != num3)
				{
					float xanchorForWidth = tk2dTextGeomGen.GetXAnchorForWidth(num9, geomData);
					tk2dTextGeomGen.PostAlignTextData(pos, offset, num4, num10, xanchorForWidth, list);
				}
				num4 = num3;
				num = 0f;
				num2 -= (fontInst.lineHeight + textMeshData.lineSpacing) * textMeshData.scale.y;
				list.Clear();
			}
			else
			{
				if (textMeshData.inlineStyling && num7 == 94)
				{
					if (num6 + 1 >= formattedText.Length || formattedText[num6 + 1] != '^')
					{
						num6 += tk2dTextGeomGen.HandleStyleCommand(formattedText.Substring(num6 + 1));
						goto IL_A87;
					}
					num6++;
				}
				Vector2 vector = characterOffsetVectors[num6];
				vector = BraveUtility.QuantizeVector(vector.ToVector3ZUp(0f), 32f).XY();
				pos[offset + num3 * 4] = new Vector3(num + tk2dFontChar.p0.x * textMeshData.scale.x + vector.x, yanchorForHeight + num2 + tk2dFontChar.p0.y * textMeshData.scale.y + vector.y, 0f);
				pos[offset + num3 * 4 + 1] = new Vector3(num + tk2dFontChar.p1.x * textMeshData.scale.x + vector.x, yanchorForHeight + num2 + tk2dFontChar.p0.y * textMeshData.scale.y + vector.y, 0f);
				pos[offset + num3 * 4 + 2] = new Vector3(num + tk2dFontChar.p0.x * textMeshData.scale.x + vector.x, yanchorForHeight + num2 + tk2dFontChar.p1.y * textMeshData.scale.y + vector.y, 0f);
				pos[offset + num3 * 4 + 3] = new Vector3(num + tk2dFontChar.p1.x * textMeshData.scale.x + vector.x, yanchorForHeight + num2 + tk2dFontChar.p1.y * textMeshData.scale.y + vector.y, 0f);
				if (flag2)
				{
					tk2dTextGeomGen.inlineSpriteOffsetsForLastString.Add(pos[offset + num3 * 4 + 2]);
					list.Add(tk2dTextGeomGen.inlineSpriteOffsetsForLastString.Count - 1);
				}
				if (tk2dFontChar.flipped)
				{
					uv[offset + num3 * 4] = new Vector2(tk2dFontChar.uv1.x, tk2dFontChar.uv1.y);
					uv[offset + num3 * 4 + 1] = new Vector2(tk2dFontChar.uv1.x, tk2dFontChar.uv0.y);
					uv[offset + num3 * 4 + 2] = new Vector2(tk2dFontChar.uv0.x, tk2dFontChar.uv1.y);
					uv[offset + num3 * 4 + 3] = new Vector2(tk2dFontChar.uv0.x, tk2dFontChar.uv0.y);
				}
				else
				{
					uv[offset + num3 * 4] = new Vector2(tk2dFontChar.uv0.x, tk2dFontChar.uv0.y);
					uv[offset + num3 * 4 + 1] = new Vector2(tk2dFontChar.uv1.x, tk2dFontChar.uv0.y);
					uv[offset + num3 * 4 + 2] = new Vector2(tk2dFontChar.uv0.x, tk2dFontChar.uv1.y);
					uv[offset + num3 * 4 + 3] = new Vector2(tk2dFontChar.uv1.x, tk2dFontChar.uv1.y);
				}
				if (fontInst.textureGradients)
				{
					uv2[offset + num3 * 4] = tk2dFontChar.gradientUv[0] + new Vector2(tk2dTextGeomGen.meshGradientTexU, 0f);
					uv2[offset + num3 * 4 + 1] = tk2dFontChar.gradientUv[1] + new Vector2(tk2dTextGeomGen.meshGradientTexU, 0f);
					uv2[offset + num3 * 4 + 2] = tk2dFontChar.gradientUv[2] + new Vector2(tk2dTextGeomGen.meshGradientTexU, 0f);
					uv2[offset + num3 * 4 + 3] = tk2dFontChar.gradientUv[3] + new Vector2(tk2dTextGeomGen.meshGradientTexU, 0f);
				}
				if (num6 - num5 > visibleCharacters)
				{
					Color32 color2 = Color.clear;
					color[offset + num3 * 4] = color2;
					color[offset + num3 * 4 + 1] = color2;
					color[offset + num3 * 4 + 2] = color2;
					color[offset + num3 * 4 + 3] = color2;
				}
				else if (fontInst.isPacked)
				{
					Color32 color3 = tk2dTextGeomGen.channelSelectColors[tk2dFontChar.channel];
					color[offset + num3 * 4] = color3;
					color[offset + num3 * 4 + 1] = color3;
					color[offset + num3 * 4 + 2] = color3;
					color[offset + num3 * 4 + 3] = color3;
				}
				else if (rainbowValues[num6])
				{
					color[offset + num3 * 4] = BraveUtility.GetRainbowLerp(Time.time * 3f);
					color[offset + num3 * 4 + 1] = BraveUtility.GetRainbowLerp(Time.time * 3f);
					color[offset + num3 * 4 + 2] = BraveUtility.GetRainbowLerp(Time.time * 3f);
					color[offset + num3 * 4 + 3] = BraveUtility.GetRainbowLerp(Time.time * 3f);
				}
				else if (flag)
				{
					color[offset + num3 * 4] = Color.black;
					color[offset + num3 * 4 + 1] = Color.black;
					color[offset + num3 * 4 + 2] = Color.black;
					color[offset + num3 * 4 + 3] = Color.black;
				}
				else
				{
					color[offset + num3 * 4] = tk2dTextGeomGen.meshTopColor;
					color[offset + num3 * 4 + 1] = tk2dTextGeomGen.meshTopColor;
					color[offset + num3 * 4 + 2] = tk2dTextGeomGen.meshBottomColor;
					color[offset + num3 * 4 + 3] = tk2dTextGeomGen.meshBottomColor;
				}
				num += (tk2dFontChar.advance + textMeshData.spacing) * textMeshData.scale.x;
				if (textMeshData.kerning && num6 < formattedText.Length - 1)
				{
					foreach (tk2dFontKerning tk2dFontKerning in fontInst.kerning)
					{
						if (tk2dFontKerning.c0 == (int)formattedText[num6] && tk2dFontKerning.c1 == (int)formattedText[num6 + 1])
						{
							num += tk2dFontKerning.amount * textMeshData.scale.x;
							break;
						}
					}
				}
				num3++;
			}
			IL_A87:
			num6++;
		}
		if (num4 != num3)
		{
			float num11 = num;
			int num12 = num3;
			float xanchorForWidth2 = tk2dTextGeomGen.GetXAnchorForWidth(num11, geomData);
			tk2dTextGeomGen.PostAlignTextData(pos, offset, num4, num12, xanchorForWidth2, list);
		}
		for (int l = num3; l < textMeshData.maxChars; l++)
		{
			pos[offset + l * 4] = (pos[offset + l * 4 + 1] = (pos[offset + l * 4 + 2] = (pos[offset + l * 4 + 3] = Vector3.zero)));
			uv[offset + l * 4] = (uv[offset + l * 4 + 1] = (uv[offset + l * 4 + 2] = (uv[offset + l * 4 + 3] = Vector2.zero)));
			if (fontInst.textureGradients)
			{
				uv2[offset + l * 4] = (uv2[offset + l * 4 + 1] = (uv2[offset + l * 4 + 2] = (uv2[offset + l * 4 + 3] = Vector2.zero)));
			}
			if (!fontInst.isPacked)
			{
				color[offset + l * 4] = (color[offset + l * 4 + 1] = tk2dTextGeomGen.meshTopColor);
				color[offset + l * 4 + 2] = (color[offset + l * 4 + 3] = tk2dTextGeomGen.meshBottomColor);
			}
			else
			{
				color[offset + l * 4] = (color[offset + l * 4 + 1] = (color[offset + l * 4 + 2] = (color[offset + l * 4 + 3] = Color.clear)));
			}
		}
		return num3;
	}

	// Token: 0x06003DE8 RID: 15848 RVA: 0x001376C4 File Offset: 0x001358C4
	public static tk2dFontChar GetSpecificSpriteCharDef(string substringInsideBrackets)
	{
		tk2dBaseSprite component = ((GameObject)ResourceCache.Acquire("ControllerButtonSprite")).GetComponent<tk2dBaseSprite>();
		tk2dSpriteDefinition spriteDefinition = component.Collection.GetSpriteDefinition(substringInsideBrackets);
		if (spriteDefinition != null)
		{
			tk2dFontChar tk2dFontChar = new tk2dFontChar();
			float num = Mathf.Abs(spriteDefinition.position1.x - spriteDefinition.position0.x);
			tk2dFontChar.advance = num;
			tk2dFontChar.channel = 0;
			tk2dFontChar.p0 = new Vector3(0f, 0.6875f, 0f);
			tk2dFontChar.p1 = new Vector3(0.6875f, 0f, 0f);
			tk2dFontChar.uv0 = Vector3.zero;
			tk2dFontChar.uv1 = Vector3.zero;
			tk2dFontChar.flipped = false;
			return tk2dFontChar;
		}
		return tk2dTextGeomGen.GetGenericSpriteCharDef();
	}

	// Token: 0x06003DE9 RID: 15849 RVA: 0x00137784 File Offset: 0x00135984
	public static tk2dFontChar GetGenericSpriteCharDef()
	{
		return new tk2dFontChar
		{
			advance = 0.8125f,
			channel = 0,
			p0 = new Vector3(0f, 0.6875f, 0f),
			p1 = new Vector3(0.6875f, 0f, 0f),
			uv0 = Vector3.zero,
			uv1 = Vector3.zero,
			flipped = false
		};
	}

	// Token: 0x06003DEA RID: 15850 RVA: 0x001377FC File Offset: 0x001359FC
	public static int SetTextMeshGeom(Vector3[] pos, Vector2[] uv, Vector2[] uv2, Color32[] color, int offset, tk2dTextGeomGen.GeomData geomData, int visibleCharacters)
	{
		tk2dTextMeshData textMeshData = geomData.textMeshData;
		tk2dFontData fontInst = geomData.fontInst;
		string formattedText = geomData.formattedText;
		tk2dTextGeomGen.inlineSpriteOffsetsForLastString = new List<Vector3>();
		tk2dTextGeomGen.meshTopColor = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
		tk2dTextGeomGen.meshBottomColor = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
		tk2dTextGeomGen.meshGradientTexU = (float)textMeshData.textureGradient / (float)((fontInst.gradientCount <= 0) ? 1 : fontInst.gradientCount);
		tk2dTextGeomGen.curGradientCount = fontInst.gradientCount;
		float yanchorForHeight = tk2dTextGeomGen.GetYAnchorForHeight(tk2dTextGeomGen.GetMeshDimensionsForString(geomData.formattedText, geomData).y, geomData);
		float num = 0f;
		float num2 = 0f;
		int num3 = 0;
		int num4 = 0;
		int num5 = 0;
		List<int> list = new List<int>();
		int num6 = 0;
		while (num6 < formattedText.Length && num3 < textMeshData.maxChars)
		{
			int num7 = (int)formattedText[num6];
			tk2dFontChar tk2dFontChar = null;
			bool flag = false;
			if (num7 == 91 && num6 < formattedText.Length - 1 && formattedText[num6 + 1] != ']')
			{
				for (int i = num6; i < formattedText.Length; i++)
				{
					char c = formattedText[i];
					if (c == ']')
					{
						flag = true;
						int num8 = i - num6;
						string text = formattedText.Substring(num6 + 9, num8 - 10);
						tk2dFontChar = tk2dTextGeomGen.GetSpecificSpriteCharDef(text);
						num6 += num8;
						num5 += num8;
						break;
					}
				}
			}
			bool flag2 = num7 == 94;
			if (!flag)
			{
				if (fontInst.useDictionary)
				{
					if (!fontInst.charDict.ContainsKey(num7))
					{
						num7 = 0;
					}
					tk2dFontChar = fontInst.charDict[num7];
				}
				else
				{
					if (num7 >= fontInst.chars.Length)
					{
						num7 = 0;
					}
					tk2dFontChar = fontInst.chars[num7];
				}
			}
			if (flag2)
			{
				num7 = 94;
			}
			if (num7 == 10)
			{
				float num9 = num;
				int num10 = num3;
				if (num4 != num3)
				{
					float xanchorForWidth = tk2dTextGeomGen.GetXAnchorForWidth(num9, geomData);
					tk2dTextGeomGen.PostAlignTextData(pos, offset, num4, num10, xanchorForWidth, list);
				}
				num4 = num3;
				num = 0f;
				num2 -= (fontInst.lineHeight + textMeshData.lineSpacing) * textMeshData.scale.y;
				list.Clear();
			}
			else
			{
				if (textMeshData.inlineStyling && num7 == 94)
				{
					if (num6 + 1 >= formattedText.Length || formattedText[num6 + 1] != '^')
					{
						num6 += tk2dTextGeomGen.HandleStyleCommand(formattedText.Substring(num6 + 1));
						goto IL_8BA;
					}
					num6++;
				}
				pos[offset + num3 * 4] = new Vector3(num + tk2dFontChar.p0.x * textMeshData.scale.x, yanchorForHeight + num2 + tk2dFontChar.p0.y * textMeshData.scale.y, 0f);
				pos[offset + num3 * 4 + 1] = new Vector3(num + tk2dFontChar.p1.x * textMeshData.scale.x, yanchorForHeight + num2 + tk2dFontChar.p0.y * textMeshData.scale.y, 0f);
				pos[offset + num3 * 4 + 2] = new Vector3(num + tk2dFontChar.p0.x * textMeshData.scale.x, yanchorForHeight + num2 + tk2dFontChar.p1.y * textMeshData.scale.y, 0f);
				pos[offset + num3 * 4 + 3] = new Vector3(num + tk2dFontChar.p1.x * textMeshData.scale.x, yanchorForHeight + num2 + tk2dFontChar.p1.y * textMeshData.scale.y, 0f);
				if (flag)
				{
					tk2dTextGeomGen.inlineSpriteOffsetsForLastString.Add(pos[offset + num3 * 4 + 2]);
					list.Add(tk2dTextGeomGen.inlineSpriteOffsetsForLastString.Count - 1);
				}
				if (tk2dFontChar.flipped)
				{
					uv[offset + num3 * 4] = new Vector2(tk2dFontChar.uv1.x, tk2dFontChar.uv1.y);
					uv[offset + num3 * 4 + 1] = new Vector2(tk2dFontChar.uv1.x, tk2dFontChar.uv0.y);
					uv[offset + num3 * 4 + 2] = new Vector2(tk2dFontChar.uv0.x, tk2dFontChar.uv1.y);
					uv[offset + num3 * 4 + 3] = new Vector2(tk2dFontChar.uv0.x, tk2dFontChar.uv0.y);
				}
				else
				{
					uv[offset + num3 * 4] = new Vector2(tk2dFontChar.uv0.x, tk2dFontChar.uv0.y);
					uv[offset + num3 * 4 + 1] = new Vector2(tk2dFontChar.uv1.x, tk2dFontChar.uv0.y);
					uv[offset + num3 * 4 + 2] = new Vector2(tk2dFontChar.uv0.x, tk2dFontChar.uv1.y);
					uv[offset + num3 * 4 + 3] = new Vector2(tk2dFontChar.uv1.x, tk2dFontChar.uv1.y);
				}
				if (fontInst.textureGradients)
				{
					uv2[offset + num3 * 4] = tk2dFontChar.gradientUv[0] + new Vector2(tk2dTextGeomGen.meshGradientTexU, 0f);
					uv2[offset + num3 * 4 + 1] = tk2dFontChar.gradientUv[1] + new Vector2(tk2dTextGeomGen.meshGradientTexU, 0f);
					uv2[offset + num3 * 4 + 2] = tk2dFontChar.gradientUv[2] + new Vector2(tk2dTextGeomGen.meshGradientTexU, 0f);
					uv2[offset + num3 * 4 + 3] = tk2dFontChar.gradientUv[3] + new Vector2(tk2dTextGeomGen.meshGradientTexU, 0f);
				}
				if (num6 - num5 > visibleCharacters)
				{
					Color32 color2 = Color.clear;
					color[offset + num3 * 4] = color2;
					color[offset + num3 * 4 + 1] = color2;
					color[offset + num3 * 4 + 2] = color2;
					color[offset + num3 * 4 + 3] = color2;
				}
				else if (fontInst.isPacked)
				{
					Color32 color3 = tk2dTextGeomGen.channelSelectColors[tk2dFontChar.channel];
					color[offset + num3 * 4] = color3;
					color[offset + num3 * 4 + 1] = color3;
					color[offset + num3 * 4 + 2] = color3;
					color[offset + num3 * 4 + 3] = color3;
				}
				else
				{
					color[offset + num3 * 4] = tk2dTextGeomGen.meshTopColor;
					color[offset + num3 * 4 + 1] = tk2dTextGeomGen.meshTopColor;
					color[offset + num3 * 4 + 2] = tk2dTextGeomGen.meshBottomColor;
					color[offset + num3 * 4 + 3] = tk2dTextGeomGen.meshBottomColor;
				}
				num += (tk2dFontChar.advance + textMeshData.spacing) * textMeshData.scale.x;
				if (textMeshData.kerning && num6 < formattedText.Length - 1)
				{
					foreach (tk2dFontKerning tk2dFontKerning in fontInst.kerning)
					{
						if (tk2dFontKerning.c0 == (int)formattedText[num6] && tk2dFontKerning.c1 == (int)formattedText[num6 + 1])
						{
							num += tk2dFontKerning.amount * textMeshData.scale.x;
							break;
						}
					}
				}
				num3++;
			}
			IL_8BA:
			num6++;
		}
		if (num4 != num3)
		{
			float num11 = num;
			int num12 = num3;
			float xanchorForWidth2 = tk2dTextGeomGen.GetXAnchorForWidth(num11, geomData);
			tk2dTextGeomGen.PostAlignTextData(pos, offset, num4, num12, xanchorForWidth2, list);
		}
		for (int k = num3; k < textMeshData.maxChars; k++)
		{
			pos[offset + k * 4] = (pos[offset + k * 4 + 1] = (pos[offset + k * 4 + 2] = (pos[offset + k * 4 + 3] = Vector3.zero)));
			uv[offset + k * 4] = (uv[offset + k * 4 + 1] = (uv[offset + k * 4 + 2] = (uv[offset + k * 4 + 3] = Vector2.zero)));
			if (fontInst.textureGradients)
			{
				uv2[offset + k * 4] = (uv2[offset + k * 4 + 1] = (uv2[offset + k * 4 + 2] = (uv2[offset + k * 4 + 3] = Vector2.zero)));
			}
			if (!fontInst.isPacked)
			{
				color[offset + k * 4] = (color[offset + k * 4 + 1] = tk2dTextGeomGen.meshTopColor);
				color[offset + k * 4 + 2] = (color[offset + k * 4 + 3] = tk2dTextGeomGen.meshBottomColor);
			}
			else
			{
				color[offset + k * 4] = (color[offset + k * 4 + 1] = (color[offset + k * 4 + 2] = (color[offset + k * 4 + 3] = Color.clear)));
			}
		}
		return num3;
	}

	// Token: 0x06003DEB RID: 15851 RVA: 0x00138338 File Offset: 0x00136538
	public static void SetTextMeshIndices(int[] indices, int offset, int vStart, tk2dTextGeomGen.GeomData geomData, int target)
	{
		tk2dTextMeshData textMeshData = geomData.textMeshData;
		for (int i = 0; i < textMeshData.maxChars; i++)
		{
			indices[offset + i * 6] = vStart + i * 4;
			indices[offset + i * 6 + 1] = vStart + i * 4 + 1;
			indices[offset + i * 6 + 2] = vStart + i * 4 + 3;
			indices[offset + i * 6 + 3] = vStart + i * 4 + 2;
			indices[offset + i * 6 + 4] = vStart + i * 4;
			indices[offset + i * 6 + 5] = vStart + i * 4 + 3;
		}
	}

	// Token: 0x04003081 RID: 12417
	private static tk2dTextGeomGen.GeomData tmpData = new tk2dTextGeomGen.GeomData();

	// Token: 0x04003082 RID: 12418
	private static readonly Color32[] channelSelectColors = new Color32[]
	{
		new Color32(0, 0, byte.MaxValue, 0),
		new Color(0f, 255f, 0f, 0f),
		new Color(255f, 0f, 0f, 0f),
		new Color(0f, 0f, 0f, 255f)
	};

	// Token: 0x04003083 RID: 12419
	private static Color32 meshTopColor = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);

	// Token: 0x04003084 RID: 12420
	private static Color32 meshBottomColor = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);

	// Token: 0x04003085 RID: 12421
	private static float meshGradientTexU = 0f;

	// Token: 0x04003086 RID: 12422
	private static int curGradientCount = 1;

	// Token: 0x04003087 RID: 12423
	private static Color32 errorColor = new Color32(byte.MaxValue, 0, byte.MaxValue, byte.MaxValue);

	// Token: 0x04003088 RID: 12424
	public static List<Vector3> inlineSpriteOffsetsForLastString;

	// Token: 0x02000B93 RID: 2963
	public class GeomData
	{
		// Token: 0x04003089 RID: 12425
		internal tk2dTextMeshData textMeshData;

		// Token: 0x0400308A RID: 12426
		internal tk2dFontData fontInst;

		// Token: 0x0400308B RID: 12427
		internal string formattedText = string.Empty;
	}
}
