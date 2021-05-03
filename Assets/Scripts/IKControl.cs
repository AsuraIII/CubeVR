﻿using UnityEngine;
using System;
using System.Collections;

[RequireComponent(typeof(Animator))] 

public class IKControl : MonoBehaviour {

	protected Animator animator;

	public bool ikActive = false;
	public Transform rightHandTarget = null;
	public Transform leftHandTarget = null;
	public Transform lookAtTarget = null;
	public bool rightHandTargetEnabled = true;
	public bool leftHandTargetEnabled = true;

	void Start () 
	{
		animator = GetComponent<Animator>();
	}

	//a callback for calculating IK
	void OnAnimatorIK()
	{
		if(animator) {

			//if the IK is active, set the position and rotation directly to the goal. 
			if(ikActive) {

				// Set the look target position, if one has been assigned
				if(lookAtTarget != null) {
					animator.SetLookAtWeight(1);
					animator.SetLookAtPosition(lookAtTarget.position);
				}    

				// Set the right hand target position and rotation, if one has been assigned
				if(rightHandTarget != null && rightHandTargetEnabled) {
					animator.SetIKPositionWeight(AvatarIKGoal.RightHand,1);
					animator.SetIKRotationWeight(AvatarIKGoal.RightHand,1);  
					animator.SetIKPosition(AvatarIKGoal.RightHand,rightHandTarget.position);
					animator.SetIKRotation(AvatarIKGoal.RightHand,rightHandTarget.rotation);
				}    

				// Set the right hand target position and rotation, if one has been assigned
				if(leftHandTarget != null && leftHandTargetEnabled) {
					animator.SetIKPositionWeight(AvatarIKGoal.LeftHand,1);
					animator.SetIKRotationWeight(AvatarIKGoal.LeftHand,1);  
					animator.SetIKPosition(AvatarIKGoal.LeftHand,leftHandTarget.position);
					animator.SetIKRotation(AvatarIKGoal.LeftHand,leftHandTarget.rotation);
				}  

			}

			//if the IK is not active, set the position and rotation of the hand and head back to the original position
			else {          
				animator.SetIKPositionWeight(AvatarIKGoal.RightHand,0);
				animator.SetIKRotationWeight(AvatarIKGoal.RightHand,0); 
				animator.SetIKPositionWeight(AvatarIKGoal.LeftHand,0);
				animator.SetIKRotationWeight(AvatarIKGoal.LeftHand,0); 
				animator.SetLookAtWeight(0);
			}
		}
	}    
}
