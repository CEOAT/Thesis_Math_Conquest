using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplorationModePuzzleWorldSpaceTransformSize : ExplorationModePuzzleWorldSpaceTransform
{
    private Vector3 previousize;
    private Vector3 newSize;

    private void Start()
    {
        newSize = objectWorldSpacePuzzle.localScale;
        previousize = newSize;
    }

    public override void ApplyValueToObject()
    {
        previousize = objectWorldSpacePuzzle.localScale;
        newSize = new Vector3(base.vairableList[0],
                                base.vairableList[1],
                                base.vairableList[2]);
    }
    public override void LerpToNewValue()
    {
        objectWorldSpacePuzzle.localScale = Vector3.Lerp(objectWorldSpacePuzzle.localScale, newSize, 0.1f);
    }
}