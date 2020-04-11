using System.Collections.Generic;

public enum BulletType
{
    Frost,
    Cannon
}

public class BulletMethods
{
    public static IReadOnlyDictionary<int, BulletType> TypeById { get; } = new Dictionary<int, BulletType>
    {
        { 0,  BulletType.Frost },
        { 1,  BulletType.Cannon }
    };
}
