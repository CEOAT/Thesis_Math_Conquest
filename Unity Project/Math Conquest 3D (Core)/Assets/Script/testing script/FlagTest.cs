using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagTest : MonoBehaviour
{
    [Flags]
    private enum Targets : uint
    {
        None                = 0,
        Campaigns           = 1 << 0,
        CampaignGroups      = 1 << 1,
        Advertisers         = 1 << 2,
        AdvertiserGroups    = 1 << 3,
        AffiliateGroups     = 1 << 4,
    }

    [SerializeField] private Targets oof;
    // Start is called before the first frame update
    void Start()
    {
        Targets targets = Targets.Campaigns | Targets.AffiliateGroups;
        Debug.Log(targets);
      
    }

    // Update is called once per frame
    void Update()
    {
        if (oof == Targets.Advertisers)
        {
            Debug.Log(oof);
        }
    }
}
