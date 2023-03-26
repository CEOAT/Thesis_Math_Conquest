using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplorationModePuzzleWorldSpaceTransformRotation : ExplorationModePuzzleWorldSpaceTransform
{
    private Vector3 vectorRotationInitial;
    private Vector3 vectorRotationNew;


    private void Start()
    {
        vectorRotationInitial = new Vector3(objectWorldSpacePuzzle.rotation.x,
                                                objectWorldSpacePuzzle.rotation.y,
                                                objectWorldSpacePuzzle.rotation.z);
        vectorRotationNew = vectorRotationInitial;
    }

    public override void ApplyValueToObject()
    {
        vectorRotationNew  = new Vector3(base.vairableList[0],
                                        base.vairableList[1],
                                        base.vairableList[2]);
    }
    public override void ResetObjectValue()
    {
        vectorRotationNew = vectorRotationInitial;
        base.vairableList[0] = vectorRotationInitial.x;
        base.vairableList[1] = vectorRotationInitial.y;
        base.vairableList[2] = vectorRotationInitial.z;
        base.ApplyValuesToText();
    }

    public override void LerpToNewValue()
    {
        objectWorldSpacePuzzle.rotation = Quaternion.Lerp(objectWorldSpacePuzzle.rotation, Quaternion.Euler(vectorRotationNew), 0.1f);
    }
}