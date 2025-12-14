
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[AddComponentMenu("UI/Extensions/TextPro")]
public class TextPro : Text
{
	private List<AlignData> alignDatas = new List<AlignData>();

	private const string alignRightString = "<align=\"right\">";

	private const string alignLeftString = "<align=\"left\">";

	private const string alignCenterString = "<align=\"center\">";

	protected override void OnPopulateMesh(VertexHelper toFill)
	{
		RichTextAlignDataSupport(toFill);
	}

	private void RichTextAlignDataSupport(VertexHelper toFill)
	{
		if (!base.supportRichText)
		{
			base.OnPopulateMesh(toFill);
			return;
		}
		string text = m_Text;
		m_Text = DealWithTextContent(m_Text);
		base.OnPopulateMesh(toFill);
		m_Text = text;
		IList<UILineInfo> lines = base.cachedTextGenerator.lines;
		IList<UICharInfo> characters = base.cachedTextGenerator.characters;
		Rect rectExtents = base.cachedTextGenerator.rectExtents;
		List<UIVertex> list = new List<UIVertex>();
		toFill.GetUIVertexStream(list);
		List<AlignData> list2 = new List<AlignData>();
		for (int i = 0; i < alignDatas.Count; i++)
		{
			AlignData alignData = alignDatas[i];
			for (int j = 0; j < lines.Count; j++)
			{
				UILineInfo uILineInfo = lines[j];
				if (uILineInfo.startCharIdx <= alignData.startCharIndex)
				{
					alignData.lineIndex = j;
					alignData.lineStartCharIndex = uILineInfo.startCharIdx;
					if (j == lines.Count - 1)
					{
						if (alignData.startCharIndex > base.cachedTextGenerator.characterCountVisible)
						{
							list2.Add(alignData);
						}
						else
						{
							alignData.lineEndCharIndex = base.cachedTextGenerator.characterCountVisible;
						}
					}
					continue;
				}
				alignData.lineEndCharIndex = uILineInfo.startCharIdx - 1;
				break;
			}
			alignDatas[i] = alignData;
		}
		List<int> list3 = new List<int>();
		for (int k = 0; k < alignDatas.Count; k++)
		{
			AlignData item = alignDatas[k];
			if (list3.Contains(item.lineIndex))
			{
				list2.Add(item);
			}
			else
			{
				list3.Add(item.lineIndex);
			}
		}
		for (int l = 0; l < list2.Count; l++)
		{
			alignDatas.Remove(list2[l]);
		}
		for (int m = 0; m < alignDatas.Count; m++)
		{
			AlignData alignData2 = alignDatas[m];
			if (alignData2.lineEndCharIndex >= characters.Count || alignData2.lineStartCharIndex >= characters.Count || alignData2.lineEndCharIndex * 6 > list.Count || alignData2.lineStartCharIndex * 6 >= list.Count)
			{
				continue;
			}
			if (alignData2.alignType == AlignType.Right)
			{
				UICharInfo uICharInfo = characters[alignData2.lineEndCharIndex];
				float x = rectExtents.width / 2f - uICharInfo.cursorPos.x - uICharInfo.charWidth;
				for (int n = alignData2.lineStartCharIndex * 6; n < alignData2.lineEndCharIndex * 6; n++)
				{
					UIVertex value = list[n];
					value.position += new Vector3(x, 0f, 0f);
					list[n] = value;
				}
			}
			else if (alignData2.alignType == AlignType.Left)
			{
				UICharInfo uICharInfo2 = characters[alignData2.lineStartCharIndex];
				float x2 = (0f - rectExtents.width) / 2f - uICharInfo2.cursorPos.x;
				for (int num = alignData2.lineStartCharIndex * 6; num < alignData2.lineEndCharIndex * 6; num++)
				{
					UIVertex value2 = list[num];
					value2.position += new Vector3(x2, 0f, 0f);
					list[num] = value2;
				}
			}
			else if (alignData2.alignType == AlignType.Center)
			{
				float num2 = 0f;
				for (int num3 = alignData2.lineStartCharIndex; num3 < alignData2.lineEndCharIndex; num3++)
				{
					float num4 = num2;
					UICharInfo uICharInfo3 = characters[num3];
					num2 = num4 + uICharInfo3.charWidth;
				}
				float num5 = (0f - num2) / 2f;
				UICharInfo uICharInfo4 = characters[alignData2.lineStartCharIndex];
				float x3 = num5 - uICharInfo4.cursorPos.x;
				for (int num6 = alignData2.lineStartCharIndex * 6; num6 < alignData2.lineEndCharIndex * 6; num6++)
				{
					UIVertex value3 = list[num6];
					value3.position += new Vector3(x3, 0f, 0f);
					list[num6] = value3;
				}
			}
		}
		toFill.AddUIVertexTriangleStream(list);
	}

	private string DealWithTextContent(string content)
	{
		alignDatas.Clear();
		string text = content;
		
		for (int i = 0; i < text.Length; i++)
		{
			if (text[i] != '<')
			{
				continue;
			}
			int num = text.Length - i - 1;
			string text2 = string.Empty;
			bool flag = false;
			if (num >= "<align=\"right\">".Length)
			{
				text2 = text.Substring(i, "<align=\"right\">".Length);
				if (text2 == "<align=\"right\">")
				{
					flag = true;
					AlignData item = default(AlignData);
					item.alignType = AlignType.Right;
					item.startCharIndex = i;
					alignDatas.Add(item);
				}
			}
			if (!flag && num >= "<align=\"left\">".Length)
			{
				text2 = text.Substring(i, "<align=\"left\">".Length);
				if (text2 == "<align=\"left\">")
				{
					flag = true;
					AlignData item2 = default(AlignData);
					item2.alignType = AlignType.Left;
					item2.startCharIndex = i;
					alignDatas.Add(item2);
				}
			}
			if (!flag && num >= "<align=\"center\">".Length)
			{
				text2 = text.Substring(i, "<align=\"center\">".Length);
				if (text2 == "<align=\"center\">")
				{
					flag = true;
					AlignData item3 = default(AlignData);
					item3.alignType = AlignType.Center;
					item3.startCharIndex = i;
					alignDatas.Add(item3);
				}
			}
			if (flag)
			{
				text = text.Remove(i, text2.Length);
				i = 0;
			}
		}
		return text;
		
	}
}
