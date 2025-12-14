
using UnityEngine;

public class LineManager : MonoBehaviour
{
    public LineRenderer line_zhu;

    public LineRenderer line_xu;

    public Material line_zhu_hong;

    public Material line_xu_hong;

    public Material line_zhu_huang;

    public Material line_xu_huang;

    public Material line_zhu_lan;

    public Material line_xu_lan;

    public Material line_zhu_lv;

    public Material line_xu_lv;

    public Material line_zhu_zi;

    public Material line_xu_zi;

    public Material line_zhu_cai;

    public Material line_xu_cai;

    public int oldIndex = 8;

    public void SetPos(Vector3 startPos, Vector3 endPos, int type)
    {
        if (oldIndex != type)
        {
            oldIndex = type;
            switch (type)
            {
                case 1:
                    line_zhu.material = line_zhu_hong;
                    line_xu.material = line_xu_hong;
                    break;
                case 2:
                    line_zhu.material = line_zhu_huang;
                    line_xu.material = line_xu_huang;
                    break;
                case 3:
                    line_zhu.material = line_zhu_lan;
                    line_xu.material = line_xu_lan;
                    break;
                case 4:
                    line_zhu.material = line_zhu_lv;
                    line_xu.material = line_xu_lv;
                    break;
                case 5:
                    line_zhu.material = line_zhu_zi;
                    line_xu.material = line_xu_zi;
                    break;
                default:
                    line_zhu.material = line_zhu_cai;
                    line_xu.material = line_xu_cai;
                    break;
            }
        }
        line_zhu.SetPosition(0, startPos);
        line_zhu.SetPosition(1, endPos);
        line_xu.SetPosition(0, startPos);
        line_xu.SetPosition(1, endPos);
    }
}
