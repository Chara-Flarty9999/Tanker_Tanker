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
        Assault,      // �ˌ�
        Turret,   // �Œ�C��
        Tactical,     // ������
        Mouse,        // �`�����`����(���@��)
        Dreadnought   // ����{�X
    }

    [System.Flags]
    public enum SpecialFlags
    {
        None = 0,
        Stealth = 1 << 0,
        WallWalker = 1 << 1,
        Shiny = 1 << 2 // GoldTank����
    }

    //���Y�^�Ƃ��Ă�Flags���
    //
    // [System.Flags]�������g�p����ƁA�񋓌^�̊e�l���r�b�g�t���O�Ƃ��Ĉ������Ƃ��ł��܂��B
    // �Ⴆ�΁ASpecialFlags�񋓌^�ł́A�e������2�̗ݏ�̒l�������Ă��܂��B����ɂ��A�r�b�g���Z���g�p���ĕ����̓�����g�ݍ��킹�邱�Ƃ��ł��܂��B
    // �Ⴆ�΁AStealth��WallWalker�̗����̓��������G��\������ɂ́A���̂悤�ɂ��܂��B
    // SpecialFlags enemyFlags = SpecialFlags.Stealth | SpecialFlags.WallWalker;
    // ���̂悤�ɂ��āA1�̕ϐ��ŕ����̓����������I�ɊǗ��ł��܂��B
    // �܂��A����̓������ݒ肳��Ă��邩�ǂ������m�F����ɂ́A�r�b�g���Z���g�p���܂��B
    // if ((enemyFlags & SpecialFlags.Stealth) != 0)
    // {
    //     // Stealth�������ݒ肳��Ă���ꍇ�̏���
    // }
    // ����ɂ��A�R�[�h�����_��Ŋg�����̂�����̂ɂȂ�܂��B

    // �Ƃ����̂�Copilot�������Ă�����̘b��
    // Flags�Ɋւ��Ă͕ύX����邱�Ƃ͂Ȃ�����
    //if (enemyFlags == (enemyFlags | SpecialFlags.Stealth))
    //{ ...  }  //���������������ł��ǂ��c�͂�
}
