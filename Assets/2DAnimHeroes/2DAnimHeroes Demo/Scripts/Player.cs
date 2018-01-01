﻿/*
 * Unity 2D Anim Heroes
 * Company: Sun And Moon Studios
 * Website: http://www.sunandmoonstudios.co.uk
 * Updated for Unity 5.5.0f3
 * */

using UnityEngine;
using Spine.Unity;
using System.Collections;

public class Player : MonoBehaviour
{
	//Public
	public CameraFollowPlayer mainCamera;
	// Camera follow script used to adjust camera when climbing ledges
	public float walkSpeed = 2;
	//Character walking speed.
	public float runSpeed = 7;
	// Character running speed.
	public float swimSpeed = 2;
	//Character swimming speed.
	public float pullSpeed = 2;
	//Character pulling object movement speed.
	public float pushSpeed = 2;
	// Character pushing object movement speed.
	public float sprintSpeed = 10;
	//Character sprinting speed.
	public float sprintTimer = 1.5f;
	//Time taken for character to burst in to a sprint after running.
	public float skidSpeedReduce = 1.0f;
	//Value for how fast to reduce speed to 0 when skidding.
	public float jumpHeight = 10;
	//Character jump height.
	public float landingSpeedReduce = 2.0f;
	//Speed divisor for landing, e.g. a value of 2 will half the characters movement speed when in the landing state.
	public float landingSpeedReduceTime = 4.0f;
	//time multiplier for landing, a higher number will return the character to normal movement speed quicker.
	public float machineGunFireRate = 0.3f;
	//Machine gun fire rate (button 5)
	public float pistolFireRate = 0.3f;
	//Pistol fire rate (button 2)
	public float gunFireRate = 0.4f;
	//Shotgun fire rate (button 3)
	public float skinChangeTimer = 1.0f;
	//Time taken to swap skins in automatic mode (button Y)
	public float pushPullEaseTimer = 0.6f;
	// Time base for push and pull animations so we can apply a fake ease in and out to the player speed.
	public float terminalVelocityWall = -10f;
	// Terminal velocity when on a wall, we don't want him getting skin burn when going down walls!
	public Transform GroundCheck;
	public Transform WallCheck;
	public Transform LadderCheck;
	public Transform BulletPos;
	public LayerMask groundLayer;
	public LayerMask wallLayer;
	public LayerMask ladderLayer;
	public LayerMask interactiveLayer;
	public LayerMask swimLayer;
	public GameObject bulletPrefab;
	public BoxCollider2D swordHitbox;
	public Transform[] spawnPoints;

	//Private
	public enum PlayerStates
	{
		idle,
		running,
		sprinting,
		walking,
		crouchIdle,
		crouchWalk,
		jumping,
		doubleJump,
		falling,
		rolling,
		landing,
		wallIdle,
		wallJump,
		ladderIdle,
		ladderClimbUp,
		ladderClimbDown,
		pushIdle,
		push,
		pullIdle,
		pull,
		swim,
		swimIdle,
		edgeClimb,
		edgeIdle,
		skid,
		celebration}

	;

	public enum CombatStates
	{
		unarmed,
		melee,
		pistol,
		gun,
		machineGun}

	;

	private PlayerStates currentState, previousState;
	private CombatStates combatState = CombatStates.unarmed;
	/*private string[] skins = {"StumpyPete", "BeardyBuck", "BuckMatthews", "ChuckMatthews", "Commander-Darkstrike", "Commander-Firestrike", "Commander-Icestrike", "Commander-Stonestrike",
		"DuckMatthews", "Dummy", "Fletch", "GabrielCaine", "MetalMan", "MetalMan-Blue", "MetalMan-Red", "MetalMan-Green", "PamelaFrost",
		"PamelaFrost-02", "PamelaFrost-03", "PamelaFrost-04", "PamelaFrost-05", "TruckMatthews", "TurboTed", "TurboTed-Blue", "TurboTed-Green", "YoungBuck"};*/
	private string[] skins = {"StumpyPete", "Assassin", "PamelaFrost-05", "BuckMatthews",  "Commander-Darkstrike", "ChuckMatthews",  "TurboTed-Blue", "Commander-Icestrike",  "MetalMan-Green", "PamelaFrost-04", "Commander-Stonestrike",
		"DuckMatthews", "Dummy", "Fletch", "GabrielCaine", "MetalMan",  "PamelaFrost", "PamelaFrost-02", "TurboTed-Green",  "MetalMan-Blue", "PamelaFrost-03",  "BeardyBuck", "TruckMatthews", "TurboTed",
		"Commander-Firestrike", "MetalMan-Red", "YoungBuck"
	};
	private bool jumpFrames = true;
	private bool wallFrames = true;
	private bool ladderFrames = true;
	private bool ladderToGroundFrames = true;
	private bool isGrounded = false;
	private bool allowMovement = true;
	private bool isSwim = false;
	private bool wallTouch = false;
	private bool ladderTouch = false;
	private bool interactiveTouch = false;
	private bool flipEnabled = true;
	private bool skinChangeToggle = false;
	private bool mouseEnabled = true;
	private bool block = false;
	private bool pushPullState = false;
	private float currentSkinChangeTime = 1.0f;
	private float landingTimeSpeed = 1.0f;
	private float jumpSpeed = 0;
	private float originJumpSpeed = 0;
	private float bodyGravity = 1;
	private float currentSprintTimer = 0;
	private float currentMachineGunFireRate = 0;
	private float currentPistolFireRate = 0;
	private float currentGunFireRate = 0;
	private float currentPushPullTimer = 0;
	private int currentSpawnPoint = 0;
	private int skinCount = 0;
	private Vector2 previousVelocity = Vector2.zero;
	private Vector2 velocity = Vector2.zero;
	public  SkeletonAnimation animation;
	private Rigidbody2D currentInteractiveObject;
	private Rigidbody2D body;
	private Quaternion flippedRotation = Quaternion.Euler (0, 180, 0);
	private Spine.Bone leftShoulder, rightShoulder, neck;

	
	void Start ()
	{
		currentSprintTimer = sprintTimer;
		animation = GetComponent<SkeletonAnimation> ();
		body = GetComponent<Rigidbody2D> ();
		bodyGravity = body.gravityScale;
		neck = animation.skeleton.FindBone ("neck");
		leftShoulder = animation.skeleton.FindBone ("arm_upper_far");
		rightShoulder = animation.skeleton.FindBone ("arm_upper_near");
		animation.UpdateLocal += HandleUpdateLocal;

		//Random Skin on startup
		skinCount = Random.Range (0, skins.Length - 1);
		animation.skeleton.SetSkin (skins [skinCount]);
		skinCount++;
	}

	//All local bone rotations need to be called in UpdateLocal.
	void HandleUpdateLocal (ISkeletonAnimation skeletonRenderer)
	{
		if (!block)
			MouseLook ();
	}

	void Update ()
	{
		CheckSwim ();
		CheckLadderTouch ();
		CheckIsGrounded ();
		CheckWallTouch ();
		Interact ();
		Combat ();
		Weapon ();
		Movement ();
		Flip ();
		SkinChange ();
		if (currentState != previousState) {
			SetAnimation ();
		}
		if (currentState != PlayerStates.ladderClimbUp && currentState != PlayerStates.ladderClimbDown && currentState != PlayerStates.ladderIdle && currentState != PlayerStates.swim && currentState != PlayerStates.swimIdle) {
			velocity.y = body.velocity.y;
			body.gravityScale = bodyGravity;
		} else
			body.gravityScale = 0;
		body.velocity = velocity;
		previousVelocity = velocity;
		velocity.x = 0;
		previousState = currentState;
		//SpawnPointSystem();
	}

	//Simple change spawn for video recording.
	void SpawnPointSystem ()
	{
		if (Input.GetKeyDown (KeyCode.K)) {
			if (currentSpawnPoint >= spawnPoints.Length) {
				currentSpawnPoint = 0;
			}
			transform.position = spawnPoints [currentSpawnPoint].position;
			currentSpawnPoint++;
		}
	}

	//Simple system to work with objects on the interactive layer so we can push and pull them.
	void Interact ()
	{
		if (Input.GetKeyDown (KeyCode.E) && isGrounded) {
			if (interactiveTouch) {
				if (currentInteractiveObject != null)
					currentInteractiveObject.isKinematic = true;
				flipEnabled = true;
				interactiveTouch = false;
				currentState = PlayerStates.idle;
			} else {
				Collider2D collider = Physics2D.OverlapCircle (WallCheck.position, 0.8f, interactiveLayer);
				if (collider != null) {
					if (collider) {
						flipEnabled = false;
						interactiveTouch = true;
						currentState = PlayerStates.pushIdle;
						currentInteractiveObject = collider.transform.GetComponent<Rigidbody2D> ();
					}
				}
			}
		} else if (currentState == PlayerStates.pushIdle || currentState == PlayerStates.push || currentState == PlayerStates.pullIdle || currentState == PlayerStates.pull) {
			if (!Physics2D.OverlapCircle (WallCheck.position, 0.8f, interactiveLayer)) {
				flipEnabled = true;
				interactiveTouch = false;
				currentState = PlayerStates.idle;
			}
		}
	}

	/*Rotate shoulder and neck bones based on mouse position. Ideally the angles should be adjusted depending on the animation as the torso bone impacts the rotation of the bones,
      therefore currently things such as crouching do not line up with the mouse.*/
	void MouseLook ()
	{
		Vector3 mousePos = new Vector3 (Input.mousePosition.x, Input.mousePosition.y, 10);
		Vector3 lookPos = Camera.main.ScreenToWorldPoint (mousePos);
		if (lookPos.x < transform.position.x) {
			lookPos.x = lookPos.x + (transform.position.x - lookPos.x) * 2;
		}
		lookPos = lookPos - transform.position;
		float angle = Mathf.Atan2 (lookPos.y, lookPos.x) * Mathf.Rad2Deg;
		//animation.skeleton.UpdateWorldTransform();
		//This offset is quick-fix used to reorientate the right shoulder when looking up and down (when you hold a gun, one of your arms extends as you aim up or down more.)
		float offset = angle;
		if (angle > 90)
			angle = 90;
		if (angle < -30)
			angle = -30;
		if (angle > 10 || angle < 0)
			offset = angle * 1.2f;
		angle -= 12;
		neck.Rotation = neck.Rotation + angle;
		leftShoulder.Rotation = leftShoulder.Rotation + angle + offset;
		rightShoulder.Rotation = rightShoulder.Rotation + angle;
	}

	//Simple combat system
	void Combat ()
	{
		if (Input.GetMouseButtonDown (0)) {
			switch (combatState) {
			case CombatStates.unarmed:
				animation.state.SetAnimation (1, "punch1", false);
				animation.state.AddAnimation (1, "punch2", false, 0);
				animation.state.AddAnimation (1, "punch3", false, 0);
				//allowMovement = false;
				break;
			case CombatStates.melee:
				if (previousVelocity.x == 0 && isGrounded == true) {
					animation.state.SetAnimation (1, "meleeSwing1-fullBody", false);
					animation.state.AddAnimation (1, "meleeSwing2-fullBody", false, 0);
					animation.state.AddAnimation (1, "meleeSwing3-fullBody", false, 0);
					//allowMovement = false; //Movement should probably be limited in this situations.
				} else {
					animation.state.SetAnimation (1, "meleeSwing1", false);
					animation.state.AddAnimation (1, "meleeSwing2", false, 0);
					animation.state.AddAnimation (1, "meleeSwing3", false, 0);
				}
				swordHitbox.enabled = true;
				break;
			case CombatStates.pistol:
				animation.state.SetAnimation (1, "pistolNearShoot", true);
				break;
			case CombatStates.gun:
				animation.state.SetAnimation (1, "gunShoot", true);
				break;
			case CombatStates.machineGun:
				animation.state.SetAnimation (1, "machineGunShoot", true);
				break;
			}
			currentPistolFireRate = 0;
			currentMachineGunFireRate = 0;
			currentGunFireRate = 0;
		}
		if (Input.GetMouseButton (0)) {
			switch (combatState) {
			case CombatStates.unarmed:
				break;
			case CombatStates.melee:
				break;
			case CombatStates.pistol:
				if (currentPistolFireRate <= 0) {
					Instantiate (bulletPrefab, BulletPos.position, BulletPos.rotation);
					currentPistolFireRate = pistolFireRate;
				} else {
					currentPistolFireRate -= Time.deltaTime;
				}
				break;
			case CombatStates.gun:
				if (currentGunFireRate <= 0) {
					for (int i = 0; i < 21; i++) {
						GameObject bullet = (GameObject)Instantiate (bulletPrefab, BulletPos.position, BulletPos.rotation);
						float ranX = Random.Range (-25, +25);
						float ranY = Random.Range (-25, +25);
						float ranZ = Random.Range (-25, +25);
						bullet.transform.Rotate (new Vector3 (ranX, ranY, ranZ));
					}

					currentGunFireRate = gunFireRate;
				} else {
					currentGunFireRate -= Time.deltaTime;
				}
				break;
			case CombatStates.machineGun:
				if (currentMachineGunFireRate <= 0) {
					Instantiate (bulletPrefab, BulletPos.position, BulletPos.rotation);
					currentMachineGunFireRate = machineGunFireRate;
				} else {
					currentMachineGunFireRate -= Time.deltaTime;
				}
				break;
			}
		}
		if (Input.GetMouseButtonUp (0)) {
			switch (combatState) {
			case CombatStates.unarmed:
				animation.state.SetAnimation (1, "reset", false);
				allowMovement = true;
				break;
			case CombatStates.melee:
				animation.state.SetAnimation (1, "meleeIdle", true);
				allowMovement = true;
				break;
			case CombatStates.pistol:
				animation.state.SetAnimation (1, "pistolNearIdle", true);
				break;
			case CombatStates.gun:
				animation.state.SetAnimation (1, "gunIdle", true);
				break;
			case CombatStates.machineGun:
				animation.state.SetAnimation (1, "machineGunIdle", true);
				break;
			}
			swordHitbox.enabled = false;
		}
		if (Input.GetMouseButtonDown (1)) {
			animation.state.SetAnimation (1, "block", true);
			block = true;
		}
		if (Input.GetMouseButtonUp (1)) {
			animation.state.SetAnimation (1, "reset", false);
			block = false;
		}
	}

	//Combat state controller, the combat animations run on a seperate track that allows us to override the movement animations inplace of them.
	void Weapon ()
	{
		//Unarmed
		if (Input.GetKeyDown (KeyCode.Alpha1)) {
			animation.state.SetAnimation (1, "reset", false);
			combatState = CombatStates.unarmed;
		}
		if (Input.GetKeyDown (KeyCode.Alpha2)) {
			animation.state.SetAnimation (1, "meleeIdle", true);
			combatState = CombatStates.melee;
		}
		if (Input.GetKeyDown (KeyCode.Alpha3)) {
			animation.state.SetAnimation (1, "pistolNearIdle", true);
			combatState = CombatStates.pistol;
		}
		if (Input.GetKeyDown (KeyCode.Alpha4)) {
			animation.state.SetAnimation (1, "gunIdle", true);
			combatState = CombatStates.gun;
		}
		if (Input.GetKeyDown (KeyCode.Alpha5)) {
			animation.state.SetAnimation (1, "machineGunIdle", true);
			combatState = CombatStates.machineGun;
		}
		//Simple reload animations (Don't do anything, just to showcase reload.)
		if (Input.GetKeyDown (KeyCode.F)) {
			switch (combatState) {
			case CombatStates.gun:
				mouseEnabled = false;
				animation.state.SetAnimation (1, "gunReload1", false).Complete += delegate {
					animation.state.SetAnimation (1, "gunIdle", true);
					mouseEnabled = true;
				};
				break;
			case CombatStates.machineGun:
				mouseEnabled = false;
				animation.state.SetAnimation (1, "machineGunReload", false).Complete += delegate {
					animation.state.SetAnimation (1, "machineGunIdle", true);
					mouseEnabled = true;
				};
				break;
			}
		}
	}

	//Simple hotkeys to cycle through character skins.
	void SkinChange ()
	{
		//Single skin change.
		if (Input.GetKeyDown (KeyCode.T)) {
			animation.skeleton.SetSkin (skins [skinCount]);
			skinCount++;
			if (skinCount >= skins.Length) {
				skinCount = 0;
			}
		}
		//Toggle skin change mode.
		if (Input.GetKeyDown (KeyCode.Y)) {
			if (skinChangeToggle)
				skinChangeToggle = false;
			else
				skinChangeToggle = true;
		}

		if (skinChangeToggle) {
			if (currentSkinChangeTime < 0) {
				animation.skeleton.SetSkin (skins [skinCount]);
				skinCount++;
				if (skinCount >= skins.Length) {
					skinCount = 0;
				}
				currentSkinChangeTime = skinChangeTimer;
			}
			currentSkinChangeTime -= Time.deltaTime;
		}
	}

	//State controller for all the characters movement
	void Movement ()
	{
		if (BlackListStates () && allowMovement) {
			//Walking/Running/Crouching/Sprinting (Default) movement.
			if (Input.GetKey (KeyCode.A)) {
				if (Input.GetKey (KeyCode.LeftControl)) {
					velocity.x = -walkSpeed;
					currentState = PlayerStates.crouchWalk;
				} else if (Input.GetKey (KeyCode.LeftShift)) {
					velocity.x = -walkSpeed;
					currentState = PlayerStates.walking;
				} else {
					velocity.x = -(runSpeed);
					currentState = PlayerStates.running;
					if (previousState == PlayerStates.running || previousState == PlayerStates.sprinting) {
						if (currentSprintTimer > 0) {
							currentSprintTimer -= Time.deltaTime;
						} else {
							velocity.x = -sprintSpeed;
							currentState = PlayerStates.sprinting;
						}
					}
				}
			}
			if (Input.GetKey (KeyCode.D)) {
				if (Input.GetKey (KeyCode.LeftControl)) {
					velocity.x = walkSpeed;
					currentState = PlayerStates.crouchWalk;
				} else if (Input.GetKey (KeyCode.LeftShift)) {
					velocity.x = walkSpeed;
					currentState = PlayerStates.walking;
				} else {
					velocity.x = (runSpeed);
					currentState = PlayerStates.running;
					if (previousState == PlayerStates.running || previousState == PlayerStates.sprinting) {
						if (currentSprintTimer > 0) {
							currentSprintTimer -= Time.deltaTime;
						} else {
							velocity.x = sprintSpeed;
							currentState = PlayerStates.sprinting;
						}
					}
				}
			}
		}
		//Ladder movement.
		else if (currentState == PlayerStates.ladderIdle || currentState == PlayerStates.ladderClimbUp || currentState == PlayerStates.ladderClimbDown) {
			if (ladderTouch) {
				if (Input.GetKey (KeyCode.D)) {
					if (transform.localRotation == Quaternion.identity) {
						currentState = PlayerStates.ladderClimbUp;
						velocity.x = runSpeed;
						velocity.y = walkSpeed;
					} else {
						currentState = PlayerStates.ladderClimbDown;
						if (isGrounded)
							velocity.x = runSpeed;
						else
							velocity.x = -runSpeed;
						velocity.y = -walkSpeed;
					}
				} else if (Input.GetKey (KeyCode.A)) {
					if (transform.localRotation == Quaternion.identity) {
						currentState = PlayerStates.ladderClimbDown;
						if (isGrounded)
							velocity.x = -runSpeed;
						else
							velocity.x = runSpeed;
						velocity.y = -walkSpeed;
					} else {
						currentState = PlayerStates.ladderClimbUp;
						velocity.y = walkSpeed;
						velocity.x = -runSpeed;
					}
				} else {
					currentState = PlayerStates.ladderIdle;
					velocity.y = 0;
				}
				if (Input.GetKey (KeyCode.W)) {
					if (transform.localRotation == Quaternion.identity) {
						if (isGrounded)
							velocity.x = -runSpeed;
						else
							velocity.x = runSpeed;
					} else {
						if (isGrounded)
							velocity.x = runSpeed;
						else
							velocity.x = -runSpeed;
						;
					}
					velocity.y = walkSpeed;
					currentState = PlayerStates.ladderClimbUp;
				}
				if (Input.GetKey (KeyCode.S)) {
					if (transform.localRotation == Quaternion.identity) {
						if (isGrounded)
							velocity.x = -runSpeed;
						else
							velocity.x = runSpeed;
					} else {
						if (isGrounded)
							velocity.x = runSpeed;
						else
							velocity.x = -runSpeed;
						;
					}
					velocity.y = -walkSpeed;
					currentState = PlayerStates.ladderClimbDown;
				}
			} else {
				currentState = PlayerStates.idle;
			}
		}
		//Interactive movement.
		else if (interactiveTouch) {
			if (Input.GetKey (KeyCode.D)) {

				if (transform.localRotation == Quaternion.identity) {
					currentState = PlayerStates.push;
					if (pushPullState) {

						if (currentPushPullTimer > ((pushPullEaseTimer / 10) * 8) || currentPushPullTimer < ((pushPullEaseTimer / 10) * 2))
							velocity.x = pushSpeed / 2;
						else
							velocity.x = pushSpeed;
						currentPushPullTimer -= Time.deltaTime;
						currentInteractiveObject.velocity = new Vector2 (velocity.x, currentInteractiveObject.velocity.y);
					}
				} else {
					currentState = PlayerStates.pull;
					if (pushPullState) {
						if (currentPushPullTimer > ((pushPullEaseTimer / 10) * 8) || currentPushPullTimer < ((pushPullEaseTimer / 10) * 2))
							velocity.x = pullSpeed / 2;
						else
							velocity.x = pullSpeed;
						currentInteractiveObject.velocity = new Vector2 (velocity.x * 1.5f, currentInteractiveObject.velocity.y);
						currentPushPullTimer -= Time.deltaTime;
					}
				}
				currentInteractiveObject.isKinematic = false;
			} else if (Input.GetKey (KeyCode.A)) {

				if (transform.localRotation == Quaternion.identity) {
					currentState = PlayerStates.pull;
					if (pushPullState) {
						if (currentPushPullTimer > ((pushPullEaseTimer / 10) * 8) || currentPushPullTimer < ((pushPullEaseTimer / 10) * 2))
							velocity.x = -pullSpeed / 2;
						else
							velocity.x = -pullSpeed;
						currentInteractiveObject.velocity = new Vector2 (velocity.x * 1.5f, currentInteractiveObject.velocity.y);
						currentPushPullTimer -= Time.deltaTime;
					}
				} else {
					currentState = PlayerStates.push;
					if (pushPullState) {
						if (currentPushPullTimer > ((pushPullEaseTimer / 10) * 8) || currentPushPullTimer < ((pushPullEaseTimer / 10) * 2))
							velocity.x = -pushSpeed / 2;
						else
							velocity.x = -pushSpeed;
						currentInteractiveObject.velocity = new Vector2 (velocity.x, currentInteractiveObject.velocity.y);
						currentPushPullTimer -= Time.deltaTime;
					}
				}
				currentInteractiveObject.isKinematic = false;

			} else {
				if (previousState == PlayerStates.push)
					currentState = PlayerStates.pushIdle;
				else if (previousState == PlayerStates.pull)
					currentState = PlayerStates.pullIdle;
				currentInteractiveObject.velocity = new Vector2 (0, currentInteractiveObject.velocity.y);
			}
		}
		//Swimming movement.
		else if (isSwim) {
			currentState = PlayerStates.swimIdle;
			velocity.y = 0;
			if (Input.GetKey (KeyCode.A)) {
				transform.localRotation = flippedRotation;
				velocity.x = -swimSpeed;
				currentState = PlayerStates.swim;
			}
			if (Input.GetKey (KeyCode.D)) {
				transform.localRotation = Quaternion.identity;
				velocity.x = swimSpeed;
				currentState = PlayerStates.swim;
			}
		}
		//Edge climb.
		else if (currentState == PlayerStates.edgeClimb || currentState == PlayerStates.edgeIdle) {
			if (Input.GetKeyDown (KeyCode.Space)) {
				mainCamera.follow = transform;
				currentState = PlayerStates.edgeClimb;
			}
		}
		//A little movement speed boost when rolling.
		else if (currentState == PlayerStates.rolling) {
			velocity.x = jumpSpeed * 1.5f;
		} else if (currentState == PlayerStates.celebration) {

		}
		//Reduce movement towards 0 when skidding. 
		else if (currentState == PlayerStates.skid) {
			if (transform.rotation == Quaternion.identity) {
				velocity.x = jumpSpeed;
				if ((jumpSpeed - skidSpeedReduce * Time.deltaTime) > 0)
					jumpSpeed -= skidSpeedReduce * Time.deltaTime;
				else
					jumpSpeed = 0;
			} else {
				velocity.x = jumpSpeed;
				if ((jumpSpeed + skidSpeedReduce * Time.deltaTime) < 0)
					jumpSpeed += skidSpeedReduce * Time.deltaTime;
				else
					jumpSpeed = 0;
			}
		}
		//Landing movement control.
		else if (currentState == PlayerStates.landing) {
			if (Input.GetKey (KeyCode.A)) {
				if (jumpSpeed < 0)
					velocity.x = (jumpSpeed / landingTimeSpeed);
				else
					velocity.x = -(jumpSpeed / landingTimeSpeed);
			}
			if (Input.GetKey (KeyCode.D)) {
				if (jumpSpeed > 0)
					velocity.x = (jumpSpeed / landingTimeSpeed);
				else
					velocity.x = -(jumpSpeed / landingTimeSpeed);
			}
			if (landingTimeSpeed > 1)
				landingTimeSpeed -= landingSpeedReduceTime * Time.deltaTime;
		} else if (isGrounded) {
			landingTimeSpeed = landingSpeedReduce;
			currentState = PlayerStates.landing;

		}
		//All the jump states movement control.
		else if (currentState == PlayerStates.jumping || currentState == PlayerStates.wallJump || currentState == PlayerStates.doubleJump) {
			velocity.x = jumpSpeed;
			if (wallTouch && velocity.x != 0) {
				if (jumpSpeed * -1 > 0)
					transform.localRotation = Quaternion.identity;
				else
					transform.localRotation = flippedRotation;
				currentState = PlayerStates.wallIdle;
			}
			if (Input.GetKey (KeyCode.D)) {
				if (transform.rotation == Quaternion.identity) {
					velocity.x = Mathf.Lerp (velocity.x, originJumpSpeed, 2.0f * Time.deltaTime);
				} else {
					velocity.x = Mathf.Lerp (velocity.x, 0, 2.0f * Time.deltaTime);
				}
				jumpSpeed = velocity.x;
			}
			if (Input.GetKey (KeyCode.A)) {
				if (transform.rotation == Quaternion.identity) {
					velocity.x = Mathf.Lerp (velocity.x, 0, 2.0f * Time.deltaTime);
				} else {
					velocity.x = Mathf.Lerp (velocity.x, originJumpSpeed, 2.0f * Time.deltaTime);
				}
				jumpSpeed = velocity.x;
			}
		}
		//Wall idle
		else if (currentState == PlayerStates.wallIdle) {
			if (!wallTouch)
				currentState = PlayerStates.falling;
			if (Input.GetKeyDown (KeyCode.Space)) {
				if (jumpSpeed > 0 && jumpSpeed < runSpeed)
					jumpSpeed = runSpeed;
				else if (jumpSpeed < 0 && jumpSpeed > -runSpeed)
					jumpSpeed = -runSpeed;
				jumpSpeed = -jumpSpeed;
				originJumpSpeed = jumpSpeed;
				body.velocity = new Vector2 (body.velocity.x / 2, 0);
				body.AddForce (new Vector2 (0, jumpHeight));
				currentState = PlayerStates.wallJump;
				wallTouch = false;
				StartCoroutine (WallFrames ());
			}
			//Set a terminal velocity when touching a wall to account for friction. (Can't do any cool moves if your fall at a realistic speed!)
			if (body.velocity.y < terminalVelocityWall)
				body.velocity = new Vector2 (body.velocity.x, terminalVelocityWall);
			velocity.x = jumpSpeed;
		}
		//Fall!
		else if (currentState == PlayerStates.falling) {
			velocity.x = jumpSpeed;
			if (wallTouch) {
				if (jumpSpeed * -1 > 0)
					transform.localRotation = Quaternion.identity;
				else
					transform.localRotation = flippedRotation;
				currentState = PlayerStates.wallIdle;
			}
			if (Input.GetKey (KeyCode.D)) {
				if (transform.rotation == Quaternion.identity) {
					velocity.x = Mathf.Lerp (velocity.x, originJumpSpeed, 2.0f * Time.deltaTime);
				} else {
					velocity.x = Mathf.Lerp (velocity.x, 0, 2.0f * Time.deltaTime);
				}
				jumpSpeed = velocity.x;
			}
			if (Input.GetKey (KeyCode.A)) {
				if (transform.rotation == Quaternion.identity) {
					velocity.x = Mathf.Lerp (velocity.x, 0, 2.0f * Time.deltaTime);
				} else {
					velocity.x = Mathf.Lerp (velocity.x, originJumpSpeed, 2.0f * Time.deltaTime);
				}
				jumpSpeed = velocity.x;
			}
		}
		//Jump!
		if (Input.GetKeyDown (KeyCode.Space)) {
			if (isGrounded && currentState != PlayerStates.edgeIdle && currentState != PlayerStates.edgeClimb && currentState != PlayerStates.push && currentState != PlayerStates.pull
			    && currentState != PlayerStates.pushIdle && currentState != PlayerStates.pullIdle) {
				currentState = PlayerStates.jumping;
				body.AddForce (new Vector2 (0, jumpHeight));
				isGrounded = false;
				jumpSpeed = velocity.x;
				if (velocity.x > 0 && velocity.x < runSpeed)
					jumpSpeed = runSpeed;
				else if (velocity.x < 0 && velocity.x > -runSpeed)
					jumpSpeed = -runSpeed;
				originJumpSpeed = jumpSpeed;
				StartCoroutine (JumpFrames ());
			} else if (currentState == PlayerStates.jumping) {
				currentState = PlayerStates.doubleJump;
				if (Input.GetKey (KeyCode.D)) {
					if (transform.rotation == flippedRotation) {
						body.velocity = new Vector2 (-velocity.x, 0);
						body.AddForce (new Vector2 (-velocity.x, jumpHeight));
						originJumpSpeed = -originJumpSpeed;
						jumpSpeed = originJumpSpeed;
						transform.rotation = Quaternion.identity;
					} else {
						body.velocity = new Vector2 (body.velocity.x, 0);
						body.AddForce (new Vector2 (velocity.x, jumpHeight));
					}
				} else if (Input.GetKey (KeyCode.A)) {
					if (transform.rotation == Quaternion.identity) {
						body.velocity = new Vector2 (-velocity.x, 0);
						body.AddForce (new Vector2 (-velocity.x, jumpHeight));
						originJumpSpeed = -originJumpSpeed;
						jumpSpeed = originJumpSpeed;
						transform.rotation = flippedRotation;
					} else {
						body.velocity = new Vector2 (body.velocity.x, 0);
						body.AddForce (new Vector2 (velocity.x, jumpHeight));
					}
				} else {
					body.velocity = new Vector2 (body.velocity.x, 0);
					body.AddForce (new Vector2 (velocity.x, jumpHeight));
				}
				isGrounded = false;
			}
		}
		//Roll animation state.
		if (Input.GetKeyDown (KeyCode.R) && isGrounded && currentState != PlayerStates.rolling) {
			currentState = PlayerStates.rolling;
			jumpSpeed = velocity.x;
		}
		//Ground Idle animation states.
		if (velocity.x == 0 && BlackListStates () == true) {
			if (Input.GetKey (KeyCode.LeftControl)) {
				currentState = PlayerStates.crouchIdle;	
			} else {
				currentState = PlayerStates.idle;
			}
		}
		//Trigger Fall animation state.
		if (body.velocity.y < 0 && !isGrounded && BlackListStates () == true && ladderToGroundFrames == true) {
			jumpSpeed = velocity.x;
			originJumpSpeed = jumpSpeed;
			currentState = PlayerStates.falling;
		}
		//Reset the sprint timer transition if we stop running.
		if (previousState == PlayerStates.running || previousState == PlayerStates.sprinting) {
			if (currentState != PlayerStates.running && currentState != PlayerStates.sprinting) {
				currentSprintTimer = sprintTimer;
			}
		}
		//Simple idle ladder.
		if (ladderTouch && currentState != PlayerStates.ladderClimbUp && currentState != PlayerStates.ladderClimbDown && currentState != PlayerStates.ladderIdle) {
			currentState = PlayerStates.ladderIdle;
		}
		//Player will skid when changing state from sprint unless he jumps.
		if (previousState == PlayerStates.sprinting && currentState != PlayerStates.skid && currentState != PlayerStates.rolling) {
			if (previousVelocity.x > 0 && velocity.x <= 0) {
				currentSprintTimer = sprintTimer;
				currentState = PlayerStates.skid;
				jumpSpeed = previousVelocity.x;
				velocity.x = previousVelocity.x;
			} else if (previousVelocity.x < 0 && velocity.x >= 0) {
				currentSprintTimer = sprintTimer;
				currentState = PlayerStates.skid;
				jumpSpeed = previousVelocity.x;
				velocity.x = previousVelocity.x;
			} else if (previousState == PlayerStates.sprinting && currentState != PlayerStates.sprinting && isGrounded) {
				currentSprintTimer = sprintTimer;
				currentState = PlayerStates.skid;
				jumpSpeed = previousVelocity.x;
			}
		}
		//Don't play the falling animation when going from the ladder to the ground for a few frames.
		if (previousState == PlayerStates.ladderClimbUp && currentState != PlayerStates.ladderClimbUp && currentState != PlayerStates.ladderClimbDown && currentState != PlayerStates.ladderIdle) {
			StartCoroutine (LadderToGroundFrames ());
		}
	}


	/*The animation controller for our class, it is only called when the previous and current character states are not equal (so we never call the same animation twice in a row) 
	 * as we are looping most of the animations. Anything that doesn't loop can be linked on to another animation e.g. jumping -> falling as these are animated to go together.*/
	void SetAnimation ()
	{
		switch (currentState) {
		case PlayerStates.idle:
			animation.state.SetAnimation (0, "idle", true);
			break;
		case PlayerStates.running:
			animation.state.SetAnimation (0, "run", true);
			break;
		case PlayerStates.walking:
			animation.state.SetAnimation (0, "walk", true);
			break;
		case PlayerStates.jumping:
			animation.state.SetAnimation (0, "jump", false).Complete += delegate {
				currentState = PlayerStates.falling;
			};
			break;
		case PlayerStates.doubleJump:
			animation.state.SetAnimation (0, "jump3", false);
			break;
		case PlayerStates.crouchIdle:
			animation.state.SetAnimation (0, "crouchIdle", true);
			break;
		case PlayerStates.crouchWalk:
			animation.state.SetAnimation (0, "crouchWalk", true);
			break;
		case PlayerStates.sprinting:
			animation.state.SetAnimation (0, "run2", true);
			break;
		case PlayerStates.rolling:  
			animation.state.SetAnimation (0, "roll", true).Complete += delegate {
				currentState = PlayerStates.idle;
			};
			break;
		case PlayerStates.landing:
			animation.state.SetAnimation (0, "land", false).Complete += delegate {
				currentState = PlayerStates.idle;
			};
			break;
		case PlayerStates.wallIdle:
			animation.state.SetAnimation (0, "wallIdle", true);
			break;
		case PlayerStates.wallJump:
			animation.state.SetAnimation (0, "wallJump", false).Complete += delegate {
				currentState = PlayerStates.falling;
			};
			break;
		case PlayerStates.falling:
			animation.state.SetAnimation (0, "falling", true);
			break;
		case PlayerStates.ladderIdle:
			animation.state.SetAnimation (0, "climbIdle", true);
			break;
		case PlayerStates.ladderClimbUp:
			animation.state.SetAnimation (0, "climbUp", true);
			break;
		case PlayerStates.ladderClimbDown:
			animation.state.SetAnimation (0, "climbDown", true);
			break;
		case PlayerStates.pushIdle:
			animation.state.SetAnimation (0, "pushIdle", true);
			break;
		case PlayerStates.push:
			animation.state.SetAnimation (0, "push", true).Event += delegate(Spine.TrackEntry trackIndex, Spine.Event e) {
				/*A few animations have events keyed into their spine animations, the push animation for example has two so that we can control the character's movement speed throughout the animation
                  as the character will NOT be moving at a constant speed when pushing or pulling an object. Another use for event keys is for things such as melee attacks, you can place
				 event keys in the frames where you want an active hitbox.*/
				if (e.Data.name == "Move Start") {
					currentPushPullTimer = pushPullEaseTimer;
					pushPullState = true;
				} else if (e.Data.name == "Move End") {
					pushPullState = false;
				}
			};
			break;
		case PlayerStates.pullIdle:
			animation.state.SetAnimation (0, "pullIdle", true);
			break;
		case PlayerStates.pull:
			animation.state.SetAnimation (0, "pull", true).Event += delegate(Spine.TrackEntry trackIndex, Spine.Event e) {
				if (e.Data.name == "Move Start") {
					currentPushPullTimer = pushPullEaseTimer;
					pushPullState = true;
				} else if (e.Data.name == "Move End") {
					pushPullState = false;
				}
			};
			break;
		case PlayerStates.swim:
			animation.state.SetAnimation (0, "swim", true);
			break;
		case PlayerStates.swimIdle:
			animation.state.SetAnimation (0, "swimIdle", true);
			break;
		case PlayerStates.edgeIdle:
			break;
		case PlayerStates.edgeClimb:
			animation.state.SetAnimation (0, "edgeClimb", false).Complete += delegate {
				currentState = PlayerStates.idle;
			};
			break;
		case PlayerStates.skid:
			animation.state.SetAnimation (0, "skid", false).Complete += delegate {
				currentState = PlayerStates.idle;
			};
			break;
		case PlayerStates.celebration:
			animation.state.SetAnimation (0, "celebration", false).Complete += delegate {
				currentState = PlayerStates.idle;
			};
			break;
		default:
			break;
		}
	}

	//4 Physics checks called every frame:
	void CheckSwim ()
	{
		isSwim = Physics2D.OverlapCircle (GroundCheck.position, 0.1f, swimLayer);
		if (!isSwim) {
			if (currentState == PlayerStates.swim || currentState == PlayerStates.swimIdle) {
				currentState = PlayerStates.idle;
			}
		}
	}

	void CheckIsGrounded ()
	{
		if (jumpFrames) {
			isGrounded = Physics2D.OverlapCircle (GroundCheck.position, 0.1f, groundLayer);
		}
	}

	void CheckLadderTouch ()
	{
		if (ladderFrames) {
			ladderTouch = Physics2D.OverlapCircle (LadderCheck.position, 0.8f, ladderLayer);
		}
	}

	void CheckWallTouch ()
	{
		if (wallFrames) {
			wallTouch = Physics2D.OverlapCircle (WallCheck.position, 0.8f, wallLayer);
		}
	}

	//Check called every frame incase we need to flip the character based on their velocity.
	void Flip ()
	{
		if (flipEnabled) {
			if (currentState != PlayerStates.wallIdle && currentState != PlayerStates.falling && currentState != PlayerStates.doubleJump) {
				if (velocity.x > 0)
					transform.localRotation = Quaternion.identity;
				else if (velocity.x < 0)
					transform.localRotation = flippedRotation;
			}
		}
	}

	//Blacklisted states for special cases when velocity is 0 and we do not want to go to the idle animation.
	bool BlackListStates ()
	{
		if (currentState != PlayerStates.jumping &&
		    currentState != PlayerStates.doubleJump &&
		    currentState != PlayerStates.rolling &&
		    currentState != PlayerStates.landing &&
		    currentState != PlayerStates.wallIdle &&
		    currentState != PlayerStates.wallJump &&
		    currentState != PlayerStates.falling &&
		    currentState != PlayerStates.ladderIdle &&
		    currentState != PlayerStates.ladderClimbUp &&
		    currentState != PlayerStates.ladderClimbDown &&
		    currentState != PlayerStates.pushIdle &&
		    currentState != PlayerStates.push &&
		    currentState != PlayerStates.pullIdle &&
		    currentState != PlayerStates.pull &&
		    currentState != PlayerStates.swim &&
		    currentState != PlayerStates.swimIdle &&
		    currentState != PlayerStates.edgeIdle &&
		    currentState != PlayerStates.edgeClimb &&
		    currentState != PlayerStates.skid &&
		    currentState != PlayerStates.celebration) {
			return true;
		}
		return false;
	}

	//Set currentstate of player, useful for triggering bullets and hanging off ledges.
	public void SetCurrentState (PlayerStates state)
	{
		currentState = state;
	}

	//Delay groundChecks for a few frames after jumping incase we are still touching the floor after jumping.
	IEnumerator JumpFrames ()
	{

		jumpFrames = false;
		yield return new WaitForSeconds (0.2f);
		jumpFrames = true;
	}

	//Controls a boolean to prevent character from proceeding to falling animation between ladder to ground transition.
	IEnumerator LadderToGroundFrames ()
	{
		ladderToGroundFrames = false;
		yield return new WaitForSeconds (0.4f);
		ladderToGroundFrames = true;
	}

	//Delay wallChecks for a few frames after wall jumping incase we are still touching the wall after jumping.
	IEnumerator WallFrames ()
	{
		wallFrames = false;
		yield return new WaitForSeconds (0.2f);
		wallFrames = true;
	}
	
}
