﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Subtract_Int : Operator<int?, int?>{

    public override void SetDefaultText()
    {
        textMesh.text = "-";
    }
    public override void CheckOutput()
    {
        if (input[0] != null && input[1] != null)
        {
            result[0] = input[0] - input[1];
            output[0].Input(result[0]);
            input[0] = null;
            input[1] = null;
        }
    }
    public override string GetInfoString()
    {
        return "입력된 첫번째 정수에서 두번째 정수를 뺀 결과를 내보냅니다.";
    }
}
