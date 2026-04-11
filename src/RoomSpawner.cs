using Godot;
using System;
using System.Collections.Generic;

namespace Rooms;

public partial class RoomSpawner : Node3D {
    private const string RoomsPath = "res://assets/scenes/rooms";

    [Export] private Node3D? _worldNode;

    private readonly List<PackedScene> _roomScenes = [];
    private readonly Queue<Room> _activeRooms = [];

    private void LoadRooms() {
        string[] fileList = ResourceLoader.ListDirectory(RoomsPath);

        foreach (string fileName in fileList) {
            PackedScene scene = GD.Load<PackedScene>($"{RoomsPath}/{fileName}");

            if (scene.InstantiateOrNull<Room>() is { } room) {
                room.Free();
                _roomScenes.Add(scene);
            }
            else {
                GD.PushWarning($"Room {fileName} was actually not a Room lol");
            }
        }
    }

    private PackedScene RandomRoom() {
        int idx = Random.Shared.Next(_roomScenes.Count);
        return _roomScenes[idx];
    }

    private void SpawnRoom() {
        Room roomNode = RandomRoom().Instantiate<Room>();

        _worldNode?.AddChild(roomNode);

        roomNode.Transform *= GlobalTransform * roomNode.entrance!.GlobalTransform.Inverse();
        GlobalTransform = roomNode.exit!.GlobalTransform;

        _activeRooms.Enqueue(roomNode);
    }

    private void CullRoom() {
        Room roomNode = _activeRooms.Dequeue();
        roomNode.QueueFree();
    }

    public override void _Ready() {
        LoadRooms();

        SpawnRoom();
        SpawnRoom();
        SpawnRoom();
        SpawnRoom();
        SpawnRoom();

        CullRoom();
    }
}
