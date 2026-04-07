using Godot;

public partial class PlayerController : CharacterBody3D {
    [Export] private float _movementSpeed = 5.0f;
    [Export] private float _jumpVelocity = 4.5f;

    public override void _Ready() {
        Input.MouseMode = Input.MouseModeEnum.Captured;
    }

    public override void _PhysicsProcess(double delta) {
        Vector3 velocity = Velocity;

        if (!IsOnFloor()) {
            velocity += GetGravity() * (float)delta;
        }

        if (Input.IsActionJustPressed("ui_accept") && IsOnFloor()) {
            velocity.Y = _jumpVelocity;
        }

        Vector2 inputDir = Input.GetVector("move_left", "move_right", "move_forward", "move_backward");

        Vector3 direction = (Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();
        if (direction != Vector3.Zero) {
            velocity.X = direction.X * _movementSpeed;
            velocity.Z = direction.Z * _movementSpeed;
        }
        else {
            velocity.X = Mathf.MoveToward(Velocity.X, 0, _movementSpeed);
            velocity.Z = Mathf.MoveToward(Velocity.Z, 0, _movementSpeed);
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
