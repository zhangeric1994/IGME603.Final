using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CameraState
{
	Idle = 0, // the normal state of camera, in this state, camera will automatically adjust the position and scope for 4 players
	Focusing, // focus at a point.
    Overview
}

public class ResponsibleCamera : MonoBehaviour {

	// Use this for initialization
	public static ResponsibleCamera _instance;
	private float tempVelocity;
	private Vector2 velocity; // the speed reference for camera
	[Header("Camera")]
	[SerializeField]private float smoothTimeY; // the smooth time for camera change the position on Y - axis, the larger number will slow the camera moving speed. 0 will be response instantly 
	[SerializeField]private float smoothTimeX; // the smooth time for camera change the position on X - axis, the larger number will slow the camera moving speed. 0 will be response instantly 
	[SerializeField]private float zoomSensity; // how responsive you wanna the camera to do the zoom 
	[SerializeField]private bool disableZooming;
	private GameObject[] Players;
	
	[Header("CameraBounds")]
	[SerializeField]private bool FreezeX;
	[SerializeField]private bool FreezeY;
	[SerializeField]private bool bounds;
	[SerializeField]private Vector3 maxCameraPos;
	[SerializeField]private Vector3 minCameraPos;
	[SerializeField]private float maxCameraSize;
	[SerializeField]private float minCameraSize;
	
	[Header("Focusing")]
	[SerializeField]private float cameraSizeOnFocusing;
	
	//Shaking related
	private bool shaking;
	private float shakeMagnitude; // currently the smallest magnitude it allowed is 0.01f;
	private bool refeshing;
	
	
	//Focusing related
	private Transform target;
	
	
	
	private float lastZoomOut; // record the last second for zooming out
	private float lastZoomIn; // record the last second for zooming in
	
	// The helper function return value
	private Vector2 currentSmallestPlayer; // position of player on lower left corner
	private Vector2 currentLargestPlayer; // position of player on upper upper corner
	private Vector2 currentSmallestWindow; // position of camera on lower left corner
	private Vector2 currentLargestWindow; // position of camera on upper upper corner
	
	private bool zoomInChasing; // the need of chasing character for a while 
	private CameraState currentState = CameraState.Idle;
	void Start ()
	{
		Players = GameObject.FindGameObjectsWithTag("Player");
		maxCameraSize = maxCameraSize == 0?9999:maxCameraSize;
		_instance = this;
	}

    public void SetPlayers()
    {
        Players = GameObject.FindGameObjectsWithTag("Player");
    }

	void FixedUpdate()
	{
		float x = 0;
		float y = 0;
		if (shaking)
		{
			x = Random.Range(-1f, 1f) * shakeMagnitude;
			y = Random.Range(-1f, 1f) * shakeMagnitude;
		}
		var camera = GetComponent<Camera>();
		switch (currentState)
		{
			case CameraState.Idle:
				
				float posy = transform.position.y;
				float posx = transform.position.x;
				getPlayerBoundary(); // get two players boundary, the smallest and largest;
				getCameraBoundary(); // get two camera boundary, the smallest and largest;
				float playerDistance = (currentLargestPlayer - currentSmallestPlayer).magnitude; // calculate the distance between two player to determine the zoom
				float cameraBound = (currentLargestWindow - currentSmallestWindow).magnitude;

				Vector2 compareLargest = (currentLargestPlayer - currentLargestWindow);
				Vector2 compareSmallest = (currentSmallestPlayer - currentSmallestWindow);
				if (!disableZooming)
				{
					// responsive zoom
					if (zoomInChasing)
					{
						camera.orthographicSize = Mathf.SmoothDamp(camera.orthographicSize, camera.orthographicSize + 4f, ref tempVelocity,smoothTimeX);
						if (compareLargest.x + camera.orthographicSize * 0.15 < 0
						    && compareLargest.y + camera.orthographicSize * 0.15 < 0
						    && compareSmallest.x - camera.orthographicSize * 0.15 > 0
						    && compareSmallest.y - camera.orthographicSize * 0.15 > 0)
						{
							zoomInChasing = false;
						}
					}
					else
					{
						if (compareLargest.x > 0 || compareLargest.y > 0)
						{
							// if one player 's position even larger than the bounds;
							// give the breath time between zoom out and in
							if (lastZoomIn + 0.5f < Time.unscaledTime && camera.orthographicSize < maxCameraSize)
							{
								//camera.orthographicSize += 0.05f;
								camera.orthographicSize = Mathf.SmoothDamp(camera.orthographicSize,
									camera.orthographicSize + 4f, ref tempVelocity, smoothTimeX);
								lastZoomOut = Time.unscaledTime;
								zoomInChasing = true;
							}

						}
						else if (compareSmallest.x < 0 || compareSmallest.y < 0)
						{
							// or one player 's position even larger than the bounds;
							if (lastZoomIn + 0.5f < Time.unscaledTime && camera.orthographicSize < maxCameraSize)
							{
								//gameObject.GetComponent<Camera>().orthographicSize += 0.05f;
								camera.orthographicSize = Mathf.SmoothDamp(camera.orthographicSize,
									camera.orthographicSize + 4f, ref tempVelocity, smoothTimeX);
								lastZoomOut = Time.unscaledTime;
								zoomInChasing = true;
							}

						}
						else if (playerDistance + zoomSensity < cameraBound && lastZoomOut + 2f < Time.unscaledTime)
						{
							// we are safe to zoom in now
							//camera.orthographicSize -= 0.03f;
							camera.orthographicSize = Mathf.SmoothDamp(camera.orthographicSize,
								camera.orthographicSize - 4f, ref tempVelocity, smoothTimeX * 1.5f);
							lastZoomIn = Time.unscaledTime;
						}
					}
				}
				
				
				//movement
				
				Vector3 center = new Vector3((currentSmallestPlayer.x+currentLargestPlayer.x)/2,(currentSmallestPlayer.y+currentLargestPlayer.y)/2,-10.0f);
				if(!FreezeX)posx = Mathf.SmoothDamp(transform.position.x,center.x, ref velocity.x,smoothTimeX);
				if(!FreezeY)posy = Mathf.SmoothDamp(transform.position.y,center.y, ref velocity.y, smoothTimeY);
					transform.position = new Vector3(posx + x, posy + y, transform.position.z);
				if (bounds)
				{
					transform.position = new Vector3(Mathf.Clamp(transform.position.x, minCameraPos.x, maxCameraPos.x)+x,
						Mathf.Clamp(transform.position.y, minCameraPos.y, maxCameraPos.y)+y,
						transform.position.z
					);
				}
				break;
			
			case CameraState.Focusing:
				
				camera.orthographicSize = Mathf.SmoothDamp(camera.orthographicSize,cameraSizeOnFocusing, ref tempVelocity, smoothTimeX/10);
				posx = Mathf.SmoothDamp(transform.position.x,target.position.x, ref velocity.x,smoothTimeX/5);
				posy = Mathf.SmoothDamp(transform.position.y,target.position.y, ref velocity.y, smoothTimeY/5);
				transform.position = new Vector3(posx + x, posy + y, transform.position.z);
				break;

            case CameraState.Overview:
                camera.orthographicSize = Mathf.SmoothDamp(camera.orthographicSize, 15f, ref tempVelocity, smoothTimeX * 2);
                posx = Mathf.SmoothDamp(transform.position.x, 0f, ref velocity.x, smoothTimeX);
                posy = Mathf.SmoothDamp(transform.position.y, 0f, ref velocity.y, smoothTimeY);
                transform.position = new Vector3(posx + x, posy + y, transform.position.z);
                break;

        }
	}
	
//	void OnDrawGizmos()
//	{
//		// Draw a yellow sphere at the transform's position
//		Vector3 center = new Vector3((currentSmallestPlayer.x+currentLargestPlayer.x)/2,(currentSmallestPlayer.y+currentLargestPlayer.y)/2,1f); // get current center of players
//		Gizmos.color = Color.red;
//
//		Gizmos.DrawSphere(new Vector3(currentLargestPlayer.x,currentLargestPlayer.y,1f),0.5f);
//		
//		Gizmos.color = Color.blue;
//		Gizmos.DrawSphere(new Vector3(currentLargestWindow.x,currentLargestWindow.y,1f),0.5f);
//		
//		Gizmos.color = Color.yellow;
//		Gizmos.DrawSphere(center, 1);
//	}

	public void focusAt(Transform _target)
	{
		target = _target;
		currentState = CameraState.Focusing;
	}

    public void Overview()
    {
        currentState = CameraState.Overview;
    }

    public void reset()
	{
		target = null;
		currentState = CameraState.Idle;
	}
	
	
	public void Shaking(float strength,float duration)
	{
		if (!shaking)
		{
			refeshing = false;
			shaking = true;
			shakeMagnitude = strength;
			StartCoroutine(shakeRelease(duration));
		}
		else
		{
			shakeMagnitude = strength;
			refeshing = true; // refresh current Coroutine
		}
	}

	IEnumerator shakeRelease(float duration)
	{
		int counter = 0;
		while (counter * 0.01f < duration)
		{
			if (refeshing)
			{
				//get refreshed
				counter = 0;
				refeshing = false; 
			}
			yield return new WaitForSeconds(0.01f);
			counter++;
		}
		shaking = false;
		yield return null;
	}

	// helper function
	private void getPlayerBoundary()
	{
		float smallestX = 9999;
		float smallestY = 9999;
		float largestX = -9999;
		float largestY = -9999;
		foreach (var player in Players)
		{
			var temp = player.transform.position;
			if (temp.x < smallestX)
			{
				smallestX = temp.x;
			}
			if (temp.x > largestX)
			{
				largestX = temp.x;
			}
			
			if (temp.y < smallestY)
			{
				smallestY = temp.y;
			}
			if (temp.y > largestY)
			{
				largestY = temp.y;
			}
		}
		currentLargestPlayer = new Vector2(largestX,largestY);
		currentSmallestPlayer = new Vector2(smallestX,smallestY);
	}

	private void getCameraBoundary()
	{
		// Screens coordinate corner location
		var camera = GetComponent<Camera>();
		//var upperLeftScreen = new Vector3(Screen.width*0.15f, Screen.height*0.75f, 0 );
//		var upperRightScreen = new Vector3(Screen.width*0.90f + camera.orthographicSize*8, Screen.height*0.90f + camera.orthographicSize*4, 0);
//		var lowerLeftScreen = new Vector3(Screen.width*0.10f - camera.orthographicSize*8, Screen.height*0.10f - camera.orthographicSize*4, 0);
		var upperRightScreen = new Vector3(Screen.width*0.80f + camera.orthographicSize*8, Screen.height*0.80f + camera.orthographicSize*4, 0);
		var lowerLeftScreen = new Vector3(Screen.width*0.20f - camera.orthographicSize*8, Screen.height*0.20f - camera.orthographicSize*4, 0);
		//var lowerRightScreen = new Vector3(Screen.width*0.85f, Screen.height*0.25f, 0);
   
		//Corner locations in world coordinates
		
		var upperRight = camera.ScreenToWorldPoint(upperRightScreen);
		var lowerLeft = camera.ScreenToWorldPoint(lowerLeftScreen);

		currentLargestWindow = upperRight;
		currentSmallestWindow = lowerLeft;
	}
}
