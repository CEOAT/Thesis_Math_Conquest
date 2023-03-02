using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplorationModePuzzleWorldSpaceTransformSize : ExplorationModePuzzleWorldSpaceTransform
{
    private  Vector3 sizeInitial;
    private Vector3 sizeNew;

    private void Start()
    {
        sizeInitial = objectWorldSpacePuzzle.localScale;
        sizeNew = sizeInitial;
    }

    public override void ApplyValueToObject()
    {
        sizeNew = new Vector3(base.vairableList[0],
                                base.vairableList[1],
                                base.vairableList[2]);
    }
    public override void ResetObjectValue()
    {
        sizeNew = sizeInitial;
        base.vairableList[0] = sizeNew.x;
        base.vairableList[1] = sizeNew.y;
        base.vairableList[2] = sizeNew.z;
        base.ApplyValuesToText();
    }

    public override void LerpToNewValue()
    {
        objectWorldSpacePuzzle.localScale = Vector3.Lerp(objectWorldSpacePuzzle.localScale, sizeNew, 0.1f);
    }
}