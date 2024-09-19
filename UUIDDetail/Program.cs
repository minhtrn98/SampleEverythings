for (int i = 0; i < 100; i++)
{
    Guid uuidv4 = Guid.NewGuid();
    Print(uuidv4);
}

Console.WriteLine("================================================================================================");

for (int i = 0; i < 100; i++)
{
    Guid uuidv7 = Guid.CreateVersion7();
    Print(uuidv7);
}

Console.WriteLine("================================================================================================");

for (int i = 0; i < 100; i++)
{
    Ulid ulid = Ulid.NewUlid();
    Guid ulidAsGuid = ulid.ToGuid();
    Print(ulidAsGuid, ulid);
}

Console.ReadKey();

static DateTimeOffset GetTimestampFromGuidV7(Guid uuid)
{
    byte[] uuidBytes = uuid.ToByteArray();
    byte[] ts = new byte[8];
    Array.Copy(uuidBytes, 0, ts, 2, 4);
    Array.Copy(uuidBytes, 4, ts, 0, 2);
    long timestamp = BitConverter.ToInt64(ts, 0);
    return DateTimeOffset.FromUnixTimeMilliseconds(timestamp);
}

static void Print(Guid guid, Ulid? ulid = default)
{
    if (ulid is not null)
    {
        Console.WriteLine("ULID - time {0}: {1} - {2}", ulid.Value.Time.LocalDateTime, ulid, guid);
    }
    else if (guid.Version == 7)
    {
        Console.WriteLine("UUID v{0} - variant {1:D2} - time {2}: {3}", guid.Version, guid.Variant, GetTimestampFromGuidV7(guid).LocalDateTime, guid);
    }
    else
    {
        Console.WriteLine("UUID v{0} - variant {1:D2}: {2}", guid.Version, guid.Variant, guid);
    }
}