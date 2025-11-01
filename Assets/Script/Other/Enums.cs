using UnityEngine;

public class Enums
{
    public enum WeaponType
    {
        None,
        Cannon,
        MachineGun,
        TwinCannon
    }
    public enum BehaviorType
    {
        Assault,      // 突撃
        Turret,   // 固定砲台
        Tactical,     // 中距離
        Mouse,        // チョロチョロ(高機動)
        Dreadnought   // 巨大ボス
    }

    [System.Flags]
    public enum SpecialFlags
    {
        None = 0,
        Stealth = 1 << 0,
        WallWalker = 1 << 1,
        Shiny = 1 << 2 // GoldTank向け
    }

    //備忘録としてのFlags解説
    //
    // [System.Flags]属性を使用すると、列挙型の各値をビットフラグとして扱うことができます。
    // 例えば、SpecialFlags列挙型では、各特性が2の累乗の値を持っています。これにより、ビット演算を使用して複数の特性を組み合わせることができます。
    // 例えば、StealthとWallWalkerの両方の特性を持つ敵を表現するには、次のようにします。
    // SpecialFlags enemyFlags = SpecialFlags.Stealth | SpecialFlags.WallWalker;
    // このようにして、1つの変数で複数の特性を効率的に管理できます。
    // また、特定の特性が設定されているかどうかを確認するには、ビット演算を使用します。
    // if ((enemyFlags & SpecialFlags.Stealth) != 0)
    // {
    //     // Stealth特性が設定されている場合の処理
    // }
    // これにより、コードがより柔軟で拡張性のあるものになります。

    // というのはCopilotが言ってた解説の話で
    // Flagsに関しては変更されることはないから
    //if (enemyFlags == (enemyFlags | SpecialFlags.Stealth))
    //{ ...  }  //こういう書き方でも良い…はず
}
