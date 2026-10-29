using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //�������g
    [SerializeField] private Transform ParentGhost;

    //�^�[�Q�b�g
    [SerializeField] private Transform Cube;

    // ����p�i�x���@�j
    [SerializeField] private float _SightAngle;

    // ���E�̍ő勗��
    [SerializeField] private float _MaxDistance = float.PositiveInfinity;

    #region Logic

    /// <summary>
    /// �^�[�Q�b�g�������Ă��邩�ǂ���
    /// </summary>
    public bool IsVisible()
    {
        //���g�̈ʒu
        var ParentGhostPos = ParentGhost.position;

        //�^�[�Q�b�g�̈ʒu
        var CubePos = Cube.position;

        //���g�̌���(���K�����ꂽ�x�N�g��)
        var ParentGhostDir = ParentGhost.forward;

        //�^�[�Q�b�g�܂ł̌����Ƌ����̌v�Z
        var CubeDir = CubePos - ParentGhostPos;
        var CubeDistance = CubeDir.magnitude;


        // cos(��/2)��v�Z
        var cosHalf = Mathf.Cos(_SightAngle / 2 * Mathf.Deg2Rad);

        // ���g�ƃ^�[�Q�b�g�ւ̌����̓�όv�Z
        // �^�[�Q�b�g�ւ̌����x�N�g���𐳋K������K�v�����邱�Ƃɒ���
        var innerProduct = Vector3.Dot(ParentGhostDir, CubeDir.normalized);

        // ���E����
        return innerProduct > cosHalf && CubeDistance < _MaxDistance;
    }


    #endregion


}
    int minutes = 0;
    int seconds = 0;

    // �^���F�N���X��
    TimerManager timerManager;

    private void Start()
    {
        timerManager = GetComponent<TimerManager>();
    }

    private void Update()
    {
        // �^�C�}�[
        (minutes,seconds) = timerManager.TimerCalc();

        // �V�[���J�ڏ��
        if (minutes <= 0 && seconds <= 0)
        {
            SceneManager.LoadScene("ClearScene");
        }
    }
}
