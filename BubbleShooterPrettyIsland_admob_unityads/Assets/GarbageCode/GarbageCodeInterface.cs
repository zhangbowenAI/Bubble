using UnityEngine;
using Garbage;
using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;


// by 伟杰
public class GarbageCodeInterface : MonoBehaviour{
    private static GarbageCodeInterface _instacne;
    public static GarbageCodeInterface Instance{
        get
        {
            if(!_instacne)
            {
                _instacne = new GarbageCodeInterface();
                _instacne.Init();
            }
            return _instacne;
        }
    }

    private string _garbageNameSpace = "Garbage";
    private List<string> classlist;


    // 通过反射获取类，方法信息
    public void Init()
    {
        // Type[] types = Assembly.GetExecutingAssembly().GetTypes();
        // List<string> namespacelist = new List<string>();
        // classlist = new List<string>();

        // foreach (Type type in types)
        // {
        //     if (type.Namespace == _garbageNameSpace)
        //     {
        //         namespacelist.Add(type.Name);
        //     }
        // }

        // foreach (string classname in namespacelist)
        // {
        //     classlist.Add(classname);
        // }

        // // Debug.Log(classlist.Count);
        // // foreach(string classname in classlist)
        // // {
        // //     Debug.Log(classname);
        // // }
    }
    
    private int i = 0;
    public void Next()
    {
        // int len = classlist.Count;
        // System.Random sr = new System.Random();
        // int ran = sr.Next(0, len);

        print("1");

        switch (i)
        {
            case 0:
                UseGarbage_1();
                break;
            case 1:
                UseGarbage_2();
                break;
            case 2:
                UseGarbage_3();
                break;
            case 3:
                UseGarbage_4();
                break;
            default:
                break;
        }
        if(++i >= 4){
            i = 0;
        }
    }

    public void UseGarbage_1()
    {
        _xXucjIYDFQXh g1 = new _xXucjIYDFQXh();
        g1.ACyhtSHqQEfT1O = false;
        g1.ED6SRgEt6LcKhXMnda6rJL_5WhorH2 = "you meng niu bi";
        g1.B8rf2NDogb();
        g1.a89wLqsxpAcBCH0();
        g1.B2PQRbmI_qtWlbi();
        g1.BFyMQJFLdduQ();
    }

    public void UseGarbage_2()
    {
        AZRb2tEx8Z5I6 g2 = new AZRb2tEx8Z5I6();
        g2.B7xOYcAhtTPUDn3xaMvICFcz1UbYAOIvStIqpht = 0;
        g2.FeByABm2xjaI = 2019+09+20;
        g2.mRtavXXJzO9p_CYaw9ckixpoRz0Va();
        g2.c4Jm1gguuRGcEQqzAvYk();
        g2.F5ka1PE5lPlVFZ3nXHUlp3lG4();
        g2.tqbQHuPbs0tBgGnEvERDbB4wQDC();
        g2.XBDiMneUkAJ79UbHzk1fvL4row();

    }

    public void UseGarbage_3()
    {
        BtAYqkyKyoz0cvki2X3haj221pShnsLYHt g3 = new BtAYqkyKyoz0cvki2X3haj221pShnsLYHt();
        g3.AEuRKCBF5ya();
        g3.DGhqBG4mWjQp_AjW();
        g3.CjUWrpcovqtDaekd4YFRdLvu3XIjGlhl7P = 0;
        g3.GRdo5PGhmvK6uflWB = 1.0;
        g3.GtzbT3MEjCiLU();
    }

    public void UseGarbage_4()
    {
        BXJq_rGYRE g4 = new BXJq_rGYRE();
        g4.CTS24W3Ko9TAiG1_mtPU55il = 0;
        g4.DjBeSlNJSSi1yR8VShVTy6JRFX0ZMx7tRU_V();
        g4.EjV3x0_kBNJ_fK = 0;
        g4.WVbJDeTT8kRQ4S6xl();
    }

}