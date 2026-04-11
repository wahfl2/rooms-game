using Godot;
using System;
using System.Linq;
using GodotDoctor.Core;
using GodotDoctor.Core.Primitives;
using Array = Godot.Collections.Array;

namespace Rooms;

public partial class Room : Node3D, IValidatable {
    [Export] public Area3D? bounds { get; set; }

    [Export] public Marker3D? entrance { get; set; }

    [Export] public Marker3D? exit { get; set; }

    private bool ValidateBounds() =>
        bounds is null || bounds.GetChildren()
            .Select(c => c as CollisionShape3D)
            .Any(c => c?.Shape is not null);

    public Array GetValidationConditions() {
        return new[] {
            new ValidationCondition(
                () => ValidateBounds(),
                "Bounds' Area3D node has no shape."
            ),
        }.ToGodotArray();
    }
}
