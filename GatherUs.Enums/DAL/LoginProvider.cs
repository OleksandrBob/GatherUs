namespace GatherUs.Enums.DAL;

[Flags]
public enum LoginProvider
{
    None = 0,
    GatherUs = 1 << 1,
}