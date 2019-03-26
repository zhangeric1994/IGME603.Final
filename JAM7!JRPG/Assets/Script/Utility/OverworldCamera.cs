using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverworldCamera : MonoBehaviour
{
	[SerializeField]
	GameObject[] players;
	[SerializeField]
	float camMin;
	[SerializeField]
	float camMax;
	Camera cam;

    // Start is called before the first frame update
    void Start()
    {
		cam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
		Vector3 furthest = cam.WorldToViewportPoint(players[0].transform.position);
		Vector3 center = Vector3.zero;

		for(int i = 1; i < players.Length; i++)
		{
			Vector3 toView = cam.WorldToViewportPoint(players[i].transform.position);

			if(Vector3.Distance(toView, new Vector3(0.5f, 0.5f, 0.0f)) >
			   Vector3.Distance(furthest, new Vector3(0.5f, 0.5f, 0.0f)))
			{
				furthest = toView;
			}

			center += players[i].transform.position;
		}

		center /= players.Length;
		center.z = -10.0f;
		print(center);

		transform.position = center;

		float absx = Mathf.Abs(furthest.x);
		float absy = Mathf.Abs(furthest.y);

		if((absx > 0.99f || absx < 0.01f || absy > 0.99f || absy < 0.01f) && cam.orthographicSize < camMax)
		{
			cam.orthographicSize += 0.01f;
		}

		else if(((absx < 0.6f && absx > 0.4f) || (absy < 0.6f && absy > 0.4f))
				&& cam.orthographicSize > camMin)
		{
			cam.orthographicSize -= 0.01f;
		}
    }
}
