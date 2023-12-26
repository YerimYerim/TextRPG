using UnityEngine;
using System.Collections;

namespace Lofle.Tween
{
	public class UITweenPosition : TweenGenericVector3
	{
		override protected void SetValue( Vector3 value )
		{
			_target.transform.GetComponent<RectTransform>().anchoredPosition = value;
		}

		override protected Vector3 GetValue()
		{
			return _target.transform.GetComponent<RectTransform>().anchoredPosition;
		}
	}
}