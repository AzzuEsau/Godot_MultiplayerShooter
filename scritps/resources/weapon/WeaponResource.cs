using Godot;
using MonoCustomResourceRegistry;
using System;

[RegisteredType(nameof(WeaponResource), "", nameof(Resource))]
public partial class WeaponResource : Resource {
    [Export] public Vector2 shootLocation;
    [Export] public Texture gunImage;
    [Export] public PackedScene bulletType;
}
