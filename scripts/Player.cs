using Godot;
using System;

public partial class Player : CharacterBody3D {
    [Export] private float _movementSpeed = 5.0f;
    [Export] private float _jumpVelocity = 4.5f;

    [Export] private Camera3D _camera;
    [Export] private float _cameraSensitivity;

    private Vector2 _cameraRotation = new Vector2();

    public override void _Ready() {
        Input.MouseMode = Input.MouseModeEnum.Captured;
    }

    public override void _Process(double delta) {
        _camera.Rotation = _camera.Rotation with {
            X = _cameraRotation.X,
            Y = _cameraRotation.Y
        };
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
        inputDir = inputDir.Rotated(-_cameraRotation.Y);

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

    public override void _Input(InputEvent @event) {
        if (@event is InputEventMouseMotion motion) {
            _cameraRotation.X -= motion.Relative.Y * _cameraSensitivity;
            _cameraRotation.Y -= motion.Relative.X * _cameraSensitivity;
        }

        if (@event.IsActionPressed("ui_cancel")) {
            Input.MouseMode = Input.MouseMode switch {
                Input.MouseModeEnum.Captured => Input.MouseModeEnum.Visible,
                _ => Input.MouseModeEnum.Captured
            };
        }
    }
}
