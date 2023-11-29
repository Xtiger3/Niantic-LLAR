using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Niantic.Lightship.AR;
using Niantic.Lightship.AR.VpsCoverage;

using UnityEngine;
using UnityEngine.UI;

public class CoverageManager : MonoBehaviour
{
    // References to components
    [SerializeField]
    public CoverageClientManager CoverageClient;

    // This will be populated by selecting an area target by name in the UI dropdown
    public string SelectedPayload;

    private Dictionary<string, string> LocationToPayload = new();

    void Start()
    {
        CoverageClient.TryGetCoverage(OnTryGetCoverage);
    }


    private void OnTryGetCoverage(AreaTargetsResult args)
    {
        // Clear any previous data
        LocationToPayload.Clear();
        SelectedPayload = null;

        var areaTargets = args.AreaTargets;
        foreach (var x in areaTargets)
        {
            Debug.Log(x.ToString());
        }

        // Sort the area targets by proximity to the user
        areaTargets.Sort((a, b) =>
            a.Area.Centroid.Distance(args.QueryLocation).CompareTo(
                b.Area.Centroid.Distance(args.QueryLocation)));

        // Only populate the dropdown with the closest 5 locations.
        // For a full sample with UI and image hints, see the VPSColocalization sample
        for (var i = 0; i < 5; i++)
        {
            Debug.Log("hello");
            LocationToPayload[areaTargets[i].Target.Name] = areaTargets[i].Target.DefaultAnchor;
            Debug.Log(areaTargets[i].Target.Name);
        }

        //Debug.Log(AddOptions(LocationToPayload.Keys.ToList());
    }
}