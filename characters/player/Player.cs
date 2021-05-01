using Godot;
using System;

public class Player : KinematicBody2D
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";
	private Vector2 velocity = Vector2.Zero;
	private AnimationPlayer animationPlayer;
	private AnimationTree animationTree;
	private AnimationNodeStateMachinePlayback animationState;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready() 
	{
		animationPlayer = GetNode<AnimationPlayer>("playerAnimations");
		animationTree = GetNode<AnimationTree>("playerAnimationTree");
		animationState = animationTree.Get("parameters/playback") as AnimationNodeStateMachinePlayback;
	}

    public override void _PhysicsProcess(float delta)
    {
		var inputVector = Vector2.Zero;
		const float acceleration = 200;
		const float maxSpeed = 75;
		const float friction = 250;
		
        base._PhysicsProcess(delta);
		inputVector.x = Input.GetActionStrength("ui_right") - Input.GetActionStrength("ui_left");
		inputVector.y = Input.GetActionStrength("ui_down") - Input.GetActionStrength("ui_up");
		inputVector = inputVector.Normalized();

		if(inputVector != Vector2.Zero) {
			animationTree.Set("parameters/idle/blend_position", inputVector);
			animationTree.Set("parameters/walk/blend_position", inputVector);
			animationState.Travel("walk");
			velocity = velocity.MoveToward(inputVector * maxSpeed, acceleration * delta);
		} else {
			animationState.Travel("idle");
			velocity = velocity.MoveToward(Vector2.Zero, friction * delta);
		}

		velocity = MoveAndSlide(velocity);
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //      
    

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
