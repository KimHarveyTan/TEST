using UnityEngine;

public class SwipeDetection : MonoBehaviour
{
	private Vector2 startPosition;
	private Vector2 endPosition;
	private Vector2 distancePoint;

	public float swipeThreshold = 50f; // Minimum distance to consider a swipe

	void Update()
	{
#if UNITY_EDITOR
		MouseInput();
#elif UNITY_ANDROID
		TouchInput();
#endif
	}

	void MouseInput()
	{
        if (Input.GetMouseButtonDown(0))
        {
			startPosition = Input.mousePosition;
		}
		if (Input.GetMouseButtonUp(0))
		{
			endPosition = Input.mousePosition;
			distancePoint = endPosition - startPosition;
			HandleSwipe();
		}
	}

	void TouchInput()
	{
		if (Input.touchCount > 0)
		{
			Touch touch = Input.GetTouch(0);
			switch(touch.phase)
			{
				case TouchPhase.Began: // Input.GetMouseButtonDown(0)
					startPosition = touch.position;
					break;
				case TouchPhase.Moved: //Input.GetMouseButton(0)
					break;
				case TouchPhase.Ended: //Input.GetMouseButtonUp(0)
					endPosition = touch.position;
					distancePoint = endPosition - startPosition;
					HandleSwipe();
					break;

			}
		}

	}

	void TouchHandler()
	{

	}

	private void HandleSwipe()
	{
		// Check if the swipe distance exceeds the threshold using magnitude
		if (distancePoint.magnitude < swipeThreshold)
		{
			return; // Not a valid swipe
		}

		// Determine swipe direction based on the swipeDelta vector
		float x = distancePoint.x;
		float y = distancePoint.y;

		if (Mathf.Abs(x) > Mathf.Abs(y))
		{
			// Horizontal swipe
			if (x > 0)
				OnSwipeRight();
			else
				OnSwipeLeft();
		}
		else
		{
			// Vertical swipe
			if (y > 0)
				OnSwipeUp();
			else
				OnSwipeDown();
		}

		// Reset swipe delta for the next swipe
		distancePoint = Vector2.zero;
	}

	private void OnSwipeRight()
	{
		this.GetComponent<Player>().Movement(Vector3.right);
		Debug.Log("Swipe Right");
		// Add your action for swipe right here
	}

	private void OnSwipeLeft()
	{
		Debug.Log("Swipe Left");
		this.GetComponent<Player>().Movement(Vector3.left);
		// Add your action for swipe left here
	}

	private void OnSwipeUp()
	{
		Debug.Log("Swipe Up");
		this.GetComponent<Player>().Movement(Vector3.up);
		// Add your action for swipe up here
	}

	private void OnSwipeDown()
	{
		Debug.Log("Swipe Down");
		this.GetComponent<Player>().Movement(Vector3.down);
		// Add your action for swipe down here
	}
}
