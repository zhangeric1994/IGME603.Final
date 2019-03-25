﻿using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public enum EnemyState : int
{
	// may move this part to stats manager or game manager
	CircleMove,
	ChaseMove,
	FindingJumpPoint,
	Idle
}
public abstract class Enemy : MonoBehaviour
{
	
	private Transform target;
	private Animator enemy_anim;
	private Rigidbody2D rb2d;
	public int health = 15;
	public float speed = 1f;
	public int Score_own = 5;
	public float Death_delay = 15;
	public bool  disableFacing;

	private Vector3 direction;

	private Vector3 defaultPos;

	private int lastTurnSecond;

	private float IdleStart;
	private float IdleDuration;
	
	private bool turnBacked;
	public Vector3 defaultScale;
	private bool deathCounted;
	private float lastFacingLeft;
	private float lastFacingRight;
	private float lastFindJump;
	// Use this for initialization
	public int distanceConstraint;
	
	public EnemyState defaultState;
	public EnemyState currentState;
	public float damage;
	
	protected EnemyState CurrentState
	{
		// this allowed to triggger codes when the state switched
		get
		{
			return currentState;
		}

		private set
		{
			if (value == currentState)
			{
				// nothing
			}
			else
			{
				EnemyState previousState = currentState;
				currentState = value;
				
				Debug.LogFormat("[Enemy] {0} --> {1}", previousState, currentState);
				
				switch (currentState)
				{
					case EnemyState.CircleMove:
						break;

					case EnemyState.ChaseMove:
						break;
				}
			}
		}
	}

	//knocking relate 
	private bool knocking;
	private float knockDuration;
	private float knockStart;
	public Transform self;
	public virtual void Awake ()
	{
		
		enemy_anim = gameObject.GetComponent<Animator>();
		rb2d = gameObject.GetComponent<Rigidbody2D>();
		defaultPos = transform.position;
		turnBacked = false;
		defaultScale = transform.localScale;
		deathCounted = false;
		self = transform;
		lastFindJump = -999.0f;
	}
	
	// Update is called once per frame
	protected void FixedUpdate ()
	{
		if (!target)
		{
			setTarget();
		}
		
		int seconds = (int)Time.fixedUnscaledTime;
		float secondsf = Time.fixedUnscaledTime;
		
		
	    // 1. circle type; circle around character
		// 2. chase type: rush into character
		if (health > 0 && !knocking)
		{
			float distanceToEnemy = (self.position - target.position).sqrMagnitude;
			switch (CurrentState)
			{
				case EnemyState.CircleMove:
					if (!turnBacked)
					{
						direction = (target.position - self.position).normalized;
					}

					// cycle check
					if (turnBacked && (defaultPos - self.position).magnitude < 2)
					{
						direction = (target.position - self.position).normalized;
						turnBacked = false;
					}

					if (seconds % 10 == 0 && lastTurnSecond != seconds && !turnBacked)
					{
						int x = Random.Range(0, 100);
						if (x < 50)
						{
							direction = (defaultPos - self.position).normalized;
							turnBacked = true;
						}

						lastTurnSecond = seconds;
					}
					
					if (direction.y > 0.1f && Mathf.Abs(direction.x) > 0.1f)
					{
						// uniform the direction vector
//						findCloestJumpPoint();
//						lastFindJump = secondsf;
//						currentState = EnemyState.FindingJumpPoint;
					}
					else
					{
						direction = new Vector3(direction.x,0.0f,0.0f);
					}
					
					if (distanceToEnemy > distanceConstraint )
					{
						//float step = speed * Time.deltaTime;
						enemy_anim.SetFloat("Speed", 1f);
						//Rigidbody2D.AddForce(direction * speed);
						//if(Mathf.Abs(rb2d.velocity.x) < Maxspeed)rb2d.AddForce(-direction * speed);
						transform.position += direction * speed * Time.deltaTime;
					}
					else
					{
						//float step = speed * Time.deltaTime;
						enemy_anim.SetFloat("Speed", 1f);
						//if(Mathf.Abs(rb2d.velocity.x) < Maxspeed)rb2d.AddForce(-direction * speed);
						transform.position += -direction * speed * Time.deltaTime ;
					}
					break;
				
				case EnemyState.ChaseMove:
					//chase type
					direction = (target.position - self.position).normalized;
					
					if (distanceToEnemy > 0.5f && direction.y > 0.2f && Mathf.Abs(direction.x) > 0.1f && lastFindJump + 4f < Time.unscaledTime)
					{
						// uniform the direction vector
						findCloestJumpPoint();
						lastFindJump = secondsf;
						currentState = EnemyState.FindingJumpPoint;
					}
					else
					{
						if (direction.y < -0.2f)
						{
							direction = new Vector3(1.0f ,0.0f,0.0f);
						}
						direction = new Vector3(direction.x,0.0f,0.0f);
					}


					if (distanceToEnemy >= distanceConstraint)
					{
						enemy_anim.SetFloat("Speed", 1f);
						//if(Mathf.Abs(rb2d.velocity.x) < Maxspeed)rb2d.AddForce(temp);
						transform.position += direction * speed * Time.deltaTime;
					}
					else
					{
						enemy_anim.SetFloat("Speed", 1f);
						//if(Mathf.Abs(rb2d.velocity.x) < Maxspeed)rb2d.AddForce(-direction * speed);
						transform.position += -direction * speed * Time.deltaTime;
					}
					break;
				
				case EnemyState.FindingJumpPoint:
					direction = (target.position - self.position).normalized;
					if (distanceToEnemy < 0.01f)
					{
						//arrived
						StartCoroutine(Jump());
						IdleStart = secondsf;
						IdleDuration = 2f;
						CurrentState = EnemyState.Idle;
					}
					else
					{
						if (distanceToEnemy > 0.5f && direction.y > 0.2f && Mathf.Abs(direction.x) > 0.1f)
						{
							findCloestJumpPoint();
							lastFindJump = secondsf;
							currentState = EnemyState.FindingJumpPoint;
						}
						else
						{
							enemy_anim.SetFloat("Speed", 1f);
							//if(Mathf.Abs(rb2d.velocity.x) < Maxspeed)rb2d.AddForce(direction * speed);
							transform.position += direction * speed * Time.deltaTime;
						}
					}
					break;
				
				case EnemyState.Idle:
					if (IdleDuration + IdleStart <= Time.unscaledTime)
					{
						// back to move
						CurrentState = defaultState;
						setTarget();
					}
					break;
			}


			if (direction.x < -0.1f && !disableFacing && lastFacingRight + 1.0 < Time.unscaledTime)
			{
				lastFacingLeft = Time.unscaledTime;
				transform.localScale = new Vector3(-defaultScale.x, defaultScale.y, defaultScale.z);
			}else if (direction.x > 0.1f && !disableFacing && lastFacingLeft + 1.0 < Time.unscaledTime)
			{
				lastFacingRight = Time.unscaledTime;
				transform.localScale = new Vector3(defaultScale.x, defaultScale.y, defaultScale.z);
			}
		}

		if (health <= 0&&!deathCounted)
		{
			//died
			enemy_anim.SetBool("Dead",true);
			StartCoroutine(Destroy_delay());
			gameObject.GetComponent<BoxCollider2D>().enabled = false;
			deathCounted = true;
		}
		
	}

	private void setTarget()
	{
		var players = GameObject.FindGameObjectsWithTag("Player");
		int playersIndex = Random.Range(0, players.Length);
		target = players[playersIndex].transform;
	}
	
	
	private void findCloestJumpPoint()
	{
		var JumpPoints = GameObject.FindGameObjectsWithTag("JumpPoint");
		float smallestDistance = 9999;
		Transform cloestJumpPoint = null;
		foreach (var jumpPoint in JumpPoints)
		{
			var direction = jumpPoint.transform.position - transform.position;
			if (Mathf.Abs(direction.y) < 0.1f)
			{
				// in the same level
				if (smallestDistance > direction.sqrMagnitude && jumpPoint.GetComponent<JumpPoint>().getJumpForce().x * direction.x > 0 )
				{
					
					smallestDistance = direction.sqrMagnitude;
					cloestJumpPoint = jumpPoint.transform;
				}

			}
		}
		target = cloestJumpPoint;
	}

	public void setState(EnemyState state)
	{
		CurrentState = state;
	}
	public void setTransform(Transform _self)
	{
		self = _self;
	}

	public void setIdle(float duration)
	{
		IdleDuration = duration;
		IdleStart = Time.unscaledTime;
		setState(EnemyState.Idle);
	}

	public void addKnock(float duration)
	{
		knocking = true;
		knockDuration = duration;
		knockStart = Time.unscaledTime;
	}

	IEnumerator Jump()
	{
		yield return new WaitForSeconds(0.5f);
		Vector3 force = target.gameObject.GetComponent<JumpPoint>().getJumpForce();
		rb2d.AddForce(force);
	}

	IEnumerator Destroy_delay(){
		yield return new WaitForSeconds(Death_delay);
		Destroy(gameObject);
	}
}
