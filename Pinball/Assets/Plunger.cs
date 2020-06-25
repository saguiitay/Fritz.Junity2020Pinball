﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plunger : MonoBehaviour
{

	public float PullSpeed;
	public float MaxPullDistance;
	public float SnapBackSpeed;

	private Vector3 _StartingPosition;
	private float _PullDistance = 0;
	private bool _Released = false;
	private bool _WaitingToContact = false;
	private Rigidbody _rb;

	// Start is called before the first frame update
	void Start()
	{

		_StartingPosition = transform.localPosition;
		_rb = GetComponent<Rigidbody>();

	}

	// Update is called once per frame
	void Update()
	{

		HandlePlunger();

	}

	private void OnCollisionEnter(Collision collision)
	{

		if (!_WaitingToContact) return;

		var pinball = collision.collider.attachedRigidbody;
		pinball.AddRelativeForce(0, 0, 20, ForceMode.Impulse);

		_WaitingToContact = false;

	}

	private void HandlePlunger()
	{

		if (Input.GetKey(KeyCode.DownArrow)) PullbackPlunger();
		
		if (_Released) {
			ReleasePlunger();
		} else if (Input.GetKeyUp(KeyCode.UpArrow)) ReleasePlunger();

	}

	private void PullbackPlunger()
	{

		if (_Released) return;

		if (_PullDistance <= MaxPullDistance)
		{

			transform.localPosition -= transform.up * PullSpeed * Time.deltaTime; //you have this line\
																																						//increment distance amt
			_PullDistance += PullSpeed * Time.deltaTime;

		}

	}
	private void ReleasePlunger()
	{
		//requires private vec3 that stores original position on start
		transform.localPosition = Vector3.Lerp(transform.localPosition, _StartingPosition, SnapBackSpeed * Time.deltaTime);

		_PullDistance = 0;
		_Released = true;
		_WaitingToContact = true;

		if (transform.localPosition == _StartingPosition) {
			_Released = false;
		}

	}

}
