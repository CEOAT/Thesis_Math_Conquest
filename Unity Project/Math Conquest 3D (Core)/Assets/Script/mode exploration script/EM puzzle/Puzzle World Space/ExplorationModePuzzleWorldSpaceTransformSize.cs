using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplorationModePuzzleWorldSpaceTransformSize : ExplorationModePuzzleWorldSpaceTransform
{
    public override void ApplyValueToObject()
    {
        base.objectWorldSpacePuzzle.localScale = new Vector3(base.vairableList[0],
                                                            base.vairableList[1],
                                                            base.vairableList[2]);
    }
}