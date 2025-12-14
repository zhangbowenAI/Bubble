
using UnityEngine;

public class DynamicVScrollView : DynamicScrollView
{
	protected override float contentAnchoredPosition
	{
		get
		{
			Vector2 anchoredPosition = _contentRect.anchoredPosition;
			return 0f - anchoredPosition.y;
		}
		set
		{
			RectTransform contentRect = _contentRect;
			Vector2 anchoredPosition = _contentRect.anchoredPosition;
			contentRect.anchoredPosition = new Vector2(anchoredPosition.x, 0f - value);
		}
	}

	protected override float contentSize => _contentRect.rect.height;

	protected override float viewportSize => _viewportRect.rect.height;

	protected override void Awake()
	{
		base.Awake();
		_direction = Direction.Vertical;
		_itemSize = itemPrototype.rect.height;
	}
}
