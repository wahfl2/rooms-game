using System;
using Godot;

namespace Rooms;

public partial class PlayerController : CharacterBody3D {
    [Export] private float _jumpVelocity = 7.5f;
    [Export] private float _walkSpeed = 5.0f;
    [Export] private float _sprintSpeed = 8.0f;
    [Export] private float _friction = 4.0f;

    public override void _PhysicsProcess(double delta) {
        Vector3 velocity = Velocity;

        if (!IsOnFloor()) {
            velocity += GetGravity() * (float)delta;
            // TODO: do we want terminal velocity?
        }

        if (Input.IsActionPressed("gameplay_jump") && IsOnFloor()) {
            // prevent jumping cancelling high upward velocity?
            velocity.Y = Math.Max(velocity.Y, _jumpVelocity);
        }

        Vector2 inputDir = Input.GetVector("gameplay_left", "gameplay_right", "gameplay_forward", "gameplay_backward");
        Vector3 direction = (Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();
        float speed = Input.IsActionPressed("gameplay_sprint") ? _sprintSpeed : _walkSpeed;

        if (direction != Vector3.Zero) {
            velocity.X = direction.X * speed;
            velocity.Z = direction.Z * speed;
        }
        else {
            float currentSpeed = velocity.Length();
            if (currentSpeed != 0.0f) {
                // TODO: separate friction value in air?
                velocity -= velocity with { Y = 0.0f } / currentSpeed * Math.Min(_friction, currentSpeed);
            }
        }

        Velocity = velocity;
        MoveAndSlide();
    }

    public void UpdateRotation(Vector2 rotation) {
        Rotation = Rotation with {
            Y = rotation.Y
        };
    }
}
