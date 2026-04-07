using Godot;

public partial class CameraController : Camera3D {
    public Vector2 viewRotation { get; private set; }

    [Export] private PlayerController _attachedPlayer;
    [Export] private Vector3 _offset;
    [Export] private float _sensitivity;

    public override void _Process(double delta) {
        Position = _attachedPlayer.Position + _offset;

        Rotation = Rotation with {
            X = viewRotation.X,
            Y = viewRotation.Y
        };

        _attachedPlayer.UpdateRotation(viewRotation);
    }

    public override void _Input(InputEvent @event) {
        if (@event is InputEventMouseMotion motion) {
            viewRotation -= new Vector2(motion.Relative.Y, motion.Relative.X) * _sensitivity;
        }

        if (@event.IsActionPressed("ui_cancel")) {
            Input.MouseMode = Input.MouseMode switch {
                Input.MouseModeEnum.Captured => Input.MouseModeEnum.Visible,
                _ => Input.MouseModeEnum.Captured
            };
        }
    }
}
