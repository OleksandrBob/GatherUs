namespace GatherUs.Enums;

[Flags]
public enum LoginProvider
{
    None = 0,
    GatherUs = 1 << 0,
}