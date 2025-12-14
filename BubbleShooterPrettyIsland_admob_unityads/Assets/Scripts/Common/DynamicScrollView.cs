
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(ScrollRect))]
public abstract class DynamicScrollView : UIBehaviour
{
	public enum Direction
	{
		Vertical,
		Horizontal
	}

	public int totalItemCount = 99;

	public int mapCount = 16;

	public RectTransform itemPrototype;

	protected Direction _direction;

	protected LinkedList<RectTransform> _containers = new LinkedList<RectTransform>();

	protected float _prevAnchoredPosition;

	protected int _nextInsertItemNo;

	protected float _itemSize = -1f;

	protected int _prevTotalItemCount = 99;

	protected RectTransform _viewportRect;

	protected RectTransform _contentRect;

	private bool isInitScrollView;

	protected abstract float contentAnchoredPosition
	{
		get;
		set;
	}

	protected abstract float contentSize
	{
		get;
	}

	protected abstract float viewportSize
	{
		get;
	}

	protected override void Awake()
	{
		if (!(itemPrototype == null))
		{
			base.Awake();
			ScrollRect component = GetComponent<ScrollRect>();
			_viewportRect = component.viewport;
			_contentRect = component.content;
		}
	}

	public void Init(int offset, int mapIndex)
	{
		itemPrototype.gameObject.SetActive(value: false);
		_prevTotalItemCount = totalItemCount;
		_contentRect.anchoredPosition = new Vector2(0f, -offset);
		int num = (int)(viewportSize / _itemSize) + 3;
		for (int i = 0; i < num; i++)
		{
			RectTransform rectTransform = Object.Instantiate(itemPrototype);
			rectTransform.SetParent(_contentRect, worldPositionStays: false);
			rectTransform.name = i.ToString();
			rectTransform.anchoredPosition = ((_direction != 0) ? new Vector2(_itemSize * (float)i, 0f) : new Vector2(0f, _itemSize * (float)i));
			_containers.AddLast(rectTransform);
			rectTransform.gameObject.SetActive(value: true);
			updateItem(i, rectTransform.gameObject);
		}
		resizeContent();
		isInitScrollView = true;
		Update();
	}

	public void Update()
	{
		if (!isInitScrollView)
		{
			return;
		}
		if (totalItemCount != _prevTotalItemCount)
		{
			_prevTotalItemCount = totalItemCount;
			bool flag = false;
			if (viewportSize - contentAnchoredPosition >= contentSize - _itemSize * 0.5f)
			{
				flag = true;
			}
			resizeContent();
			if (flag)
			{
				contentAnchoredPosition = viewportSize - contentSize;
			}
			refresh();
		}
		while (contentAnchoredPosition - _prevAnchoredPosition < _itemSize)
		{
			_prevAnchoredPosition -= _itemSize;
			RectTransform value = _containers.Last.Value;
			_containers.RemoveLast();
			_containers.AddFirst(value);
			_nextInsertItemNo--;
			float num = _itemSize * (float)_nextInsertItemNo;
			value.anchoredPosition = ((_direction != 0) ? new Vector2(num, 0f) : new Vector2(0f, num));
			updateItem(_nextInsertItemNo, value.gameObject);
		}
		while (contentAnchoredPosition - _prevAnchoredPosition > _itemSize * 2f)
		{
			_prevAnchoredPosition += _itemSize;
			RectTransform value2 = _containers.First.Value;
			_containers.RemoveFirst();
			_containers.AddLast(value2);
			float num2 = _itemSize * (float)(_containers.Count + _nextInsertItemNo);
			value2.anchoredPosition = ((_direction != 0) ? new Vector2(num2, 0f) : new Vector2(0f, num2));
			updateItem(_containers.Count + _nextInsertItemNo, value2.gameObject);
			_nextInsertItemNo++;
		}
	}

	public void insertItem()
	{
	}

	private void refresh()
	{
		int num = 0;
		if (contentAnchoredPosition != 0f)
		{
			num = (int)((0f - contentAnchoredPosition) / _itemSize);
		}
		foreach (RectTransform container in _containers)
		{
			float num2 = _itemSize * (float)num;
			container.anchoredPosition = ((_direction != 0) ? new Vector2(num2, 0f) : new Vector2(0f, num2));
			updateItem(num, container.gameObject);
			num++;
		}
		_nextInsertItemNo = num - _containers.Count;
		_prevAnchoredPosition = (float)(int)(contentAnchoredPosition / _itemSize) * _itemSize;
	}

	private void resizeContent()
	{
		Vector2 size = _contentRect.getSize();
		if (_direction == Direction.Vertical)
		{
			size.y = _itemSize * (float)totalItemCount;
		}
		else
		{
			size.x = _itemSize * (float)totalItemCount;
		}
		_contentRect.setSize(size);
	}

	private void updateItem(int index, GameObject itemObj)
	{
		if (index < 0 || index >= totalItemCount)
		{
			itemObj.SetActive(value: false);
			return;
		}
		itemObj.SetActive(value: true);
		itemObj.GetComponent<IDynamicScrollViewItem>()?.onUpdateItem(index, mapCount, isInitScrollView);
	}
}
