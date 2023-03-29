using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplorationModePuzzleWorldSpaceTransformPosition : ExplorationModePuzzleWorldSpaceTransform
{
    private  Vector3 positionInitial;
    private Vector3 positionNew;

    private void Start()
    {
        positionInitial = objectWorldSpacePuzzle.localPosition;
        positionNew = positionInitial;
    }

    public override void ApplyValueToObject()
    {
        positionNew = new Vector3(base.vairableList[0],
                                base.vairableList[1],
                                base.vairableList[2]);
    }
    public override void ResetObjectValue()
    {
        positionNew = positionInitial;
        base.vairableList[0] = positionNew.x;
        base.vairableList[1] = positionNew.y;
        base.vairableList[2] = positionNew.z;
        base.ApplyValuesToText();
    }

    public override void LerpToNewValue()
    {
        objectWorldSpacePuzzle.localPosition = Vector3.Lerp(objectWorldSpacePuzzle.localPosition, positionNew, 0.1f);
    }
}