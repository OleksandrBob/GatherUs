namespace GatherUs.Enums;

public enum CustomEventType : byte
{
    Event = 1,        // - Offline only, 100+ visitors, no invites
    Conference = 2,   // - Online/offline, 10-100 visitors, optional invite 
    Meeting = 3,      // - Online/offline, 3-15 visitors, invite only
}
