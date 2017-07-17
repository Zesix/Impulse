using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UnityStandardAssets.CrossPlatformInput
{
	[RequireComponent(typeof(Image))]
	public class TouchPad : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
	{
		// Options for which axes to use
		public enum AxisOption
		{
			Both, // Use both
			OnlyHorizontal, // Only horizontal
			OnlyVertical // Only vertical
		}


		public enum ControlStyle
		{
			Absolute, // operates from teh center of the image
			Relative, // operates from the center of the initial touch
			Swipe, // swipe to touch touch no maintained center
		}


		public AxisOption AxesToUse = AxisOption.Both; // The options for the axes that the still will use
		public ControlStyle controlStyle = ControlStyle.Absolute; // control style to use
		public string HorizontalAxisName = "Horizontal"; // The name given to the horizontal axis for the cross platform input
		public string VerticalAxisName = "Vertical"; // The name given to the vertical axis for the cross platform input
		public float Xsensitivity = 1f;
		public float Ysensitivity = 1f;

		private Vector3 _mStartPos;
		private Vector2 _mPreviousDelta;
		private Vector3 _mJoytickOutput;
		private bool _mUseX; // Toggle for using the x axis
		private bool _mUseY; // Toggle for using the Y axis
		private CrossPlatformInputManager.VirtualAxis _mHorizontalVirtualAxis; // Reference to the joystick in the cross platform input
		private CrossPlatformInputManager.VirtualAxis _mVerticalVirtualAxis; // Reference to the joystick in the cross platform input
		private bool _mDragging;
		private int _mId = -1;
		private Vector2 _mPreviousTouchPos; // swipe style control touch


#if !UNITY_EDITOR
    private Vector3 m_Center;
    private Image m_Image;
#else
		private Vector3 _mPreviousMouse;
#endif

		private void OnEnable()
		{
			CreateVirtualAxes();
		}

		private void Start()
        {
#if !UNITY_EDITOR
            m_Image = GetComponent<Image>();
            m_Center = m_Image.transform.position;
#endif
        }

		private void CreateVirtualAxes()
		{
			// set axes to use
			_mUseX = AxesToUse == AxisOption.Both || AxesToUse == AxisOption.OnlyHorizontal;
			_mUseY = AxesToUse == AxisOption.Both || AxesToUse == AxisOption.OnlyVertical;

			// create new axes based on axes to use
			if (_mUseX)
			{
				_mHorizontalVirtualAxis = new CrossPlatformInputManager.VirtualAxis(HorizontalAxisName);
				CrossPlatformInputManager.RegisterVirtualAxis(_mHorizontalVirtualAxis);
			}
			if (_mUseY)
			{
				_mVerticalVirtualAxis = new CrossPlatformInputManager.VirtualAxis(VerticalAxisName);
				CrossPlatformInputManager.RegisterVirtualAxis(_mVerticalVirtualAxis);
			}
		}

		private void UpdateVirtualAxes(Vector3 value)
		{
			value = value.normalized;
			if (_mUseX)
			{
				_mHorizontalVirtualAxis.Update(value.x);
			}

			if (_mUseY)
			{
				_mVerticalVirtualAxis.Update(value.y);
			}
		}


		public void OnPointerDown(PointerEventData data)
		{
			_mDragging = true;
			_mId = data.pointerId;
#if !UNITY_EDITOR
        if (controlStyle != ControlStyle.Absolute )
            m_Center = data.position;
#endif
		}

		private void Update()
		{
			if (!_mDragging)
			{
				return;
			}
			if (Input.touchCount >= _mId + 1 && _mId != -1)
			{
#if !UNITY_EDITOR

            if (controlStyle == ControlStyle.Swipe)
            {
                m_Center = m_PreviousTouchPos;
                m_PreviousTouchPos = Input.touches[m_Id].position;
            }
            Vector2 pointerDelta = new Vector2(Input.touches[m_Id].position.x - m_Center.x , Input.touches[m_Id].position.y - m_Center.y).normalized;
            pointerDelta.x *= Xsensitivity;
            pointerDelta.y *= Ysensitivity;
#else
				Vector2 pointerDelta;
				pointerDelta.x = Input.mousePosition.x - _mPreviousMouse.x;
				pointerDelta.y = Input.mousePosition.y - _mPreviousMouse.y;
				_mPreviousMouse = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f);
#endif
				UpdateVirtualAxes(new Vector3(pointerDelta.x, pointerDelta.y, 0));
			}
		}


		public void OnPointerUp(PointerEventData data)
		{
			_mDragging = false;
			_mId = -1;
			UpdateVirtualAxes(Vector3.zero);
		}

		private void OnDisable()
		{
			if (CrossPlatformInputManager.AxisExists(HorizontalAxisName))
				CrossPlatformInputManager.UnRegisterVirtualAxis(HorizontalAxisName);

			if (CrossPlatformInputManager.AxisExists(VerticalAxisName))
				CrossPlatformInputManager.UnRegisterVirtualAxis(VerticalAxisName);
		}
	}
}