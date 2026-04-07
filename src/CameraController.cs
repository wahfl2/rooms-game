using System;
using Godot;

namespace Rooms;

public partial class CameraController : Camera3D {
    [Export] private PlayerController _attachedPlayer = null!;
    [Export] private Vector3 _offset;
    [Export] private float _sensitivity;

    private Vector2 _viewRotation;

    public override void _Ready() {
        Input.MouseMode = Input.MouseModeEnum.Captured;
    }

    public override void _Process(double delta) {
        Position = _attachedPlayer.GetGlobalTransformInterpolated().Origin + _offset;

        Rotation = Rotation with {
            X = _viewRotation.X,
            Y = _viewRotation.Y
        };

        _attachedPlayer.UpdateRotation(_viewRotation);
    }

    public override void _Input(InputEvent @event) {
        if (@event.IsActionPressed("ui_cancel")) {
            Input.MouseMode = Input.MouseMode switch {
                Input.MouseModeEnum.Captured => Input.MouseModeEnum.Visible,
                _ => Input.MouseModeEnum.Captured
            };
        }

        if (Input.MouseMode != Input.MouseModeEnum.Captured)
            return;

        if (@event is InputEventMouseMotion motion) {
            _viewRotation -= new Vector2(motion.Relative.Y, motion.Relative.X) * _sensitivity;
            _viewRotation = _viewRotation with {
                X = Math.Clamp(_viewRotation.X, Mathf.DegToRad(-90.0f), Mathf.DegToRad(90.0f)),
                Y = _viewRotation.Y % Mathf.DegToRad(360.0f)
            };
        }
    }
}
