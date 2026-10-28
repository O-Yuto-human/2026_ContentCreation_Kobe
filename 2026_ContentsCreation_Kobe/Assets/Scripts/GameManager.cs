using UnityEngine;

public class GameManager : MonoBehaviour
{
    //自分自身
    [SerializeField] private Transform ParentGhost;

    //ターゲット
    [SerializeField] private Transform Cube;

    // 視野角（度数法）
    [SerializeField] private float _SightAngle;

    // 視界の最大距離
    [SerializeField] private float _MaxDistance = float.PositiveInfinity;

    #region Logic

    /// <summary>
    /// ターゲットが見えているかどうか
    /// </summary>
    public bool IsVisible()
    {
        //自身の位置
        var ParentGhostPos = ParentGhost.position;

        //ターゲットの位置
        var CubePos = Cube.position;

        //自身の向き(正規化されたベクトル)
        var ParentGhostDir = ParentGhost.forward;

        //ターゲットまでの向きと距離の計算
        var CubeDir = CubePos - ParentGhostPos;
        var CubeDistance = CubeDir.magnitude;


        // cos(θ/2)を計算
        var cosHalf = Mathf.Cos(_SightAngle / 2 * Mathf.Deg2Rad);

        // 自身とターゲットへの向きの内積計算
        // ターゲットへの向きベクトルを正規化する必要があることに注意
        var innerProduct = Vector3.Dot(ParentGhostDir, CubeDir.normalized);

        // 視界判定
        return innerProduct > cosHalf && CubeDistance < _MaxDistance;
    }


    #endregion


}