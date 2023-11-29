using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Niantic.Lightship.AR;
using Niantic.Lightship.AR.VpsCoverage;
using Niantic.Lightship.Maps;

using UnityEngine;
using UnityEngine.UI;

using MapsLatLng = Niantic.Lightship.Maps.Core.Coordinates.LatLng;

public class CoverageManager : MonoBehaviour
{
    // References to components
    [SerializeField]
    public CoverageClientManager CoverageClient;

    [SerializeField]
    private LightshipMapView _lightshipMapView;

    [SerializeField]
    private GameObject obj;

    private Dictionary<string, GameObject> currWayspots = new();

    void Update()
    {
        UpdateMapViewPosition();
    }

    private void OnTryGetCoverage(AreaTargetsResult args)
    {
        var areaTargets = args.AreaTargets;

        // Sort the area targets by proximity to the user
        areaTargets.Sort((a, b) =>
            a.Area.Centroid.Distance(args.QueryLocation).CompareTo(
                b.Area.Centroid.Distance(args.QueryLocation)));

        List<string> displayed = new();

        // Only populate the dropdown with the closest 5 locations.
        for (var i = 0; i < Math.Min(areaTargets.Count, 5); i++)
        {
            // Convert LatLng to game map position
            MapsLatLng mapLatLng = new MapsLatLng(areaTargets[i].Target.Center.Latitude, areaTargets[i].Target.Center.Longitude);
            Vector3 mapPos = _lightshipMapView.LatLngToScene(mapLatLng);
            
            string wayspotName = areaTargets[i].Target.Name;
            displayed.Add(wayspotName);

            if (currWayspots.ContainsKey(wayspotName))
            {
                // Update the wayspot position
                currWayspots[wayspotName].transform.position = mapPos;
                // Debug.Log("updated gameobj: " + wayspotName);
            }
            else
            {
                // Draw the new wayspot
                GameObject wayspot = Instantiate(obj, mapPos, Quaternion.identity);
                currWayspots[areaTargets[i].Target.Name] = wayspot;
                // Debug.Log("added gameobj: " + wayspotName);
            }
        }

        foreach(var item in currWayspots)
        {
            if (displayed.Contains(item.Key))
            {
                // 
            }
            else
            {
                // Delete far away wayspots
                Destroy(item.Value);
                currWayspots.Remove(item.Key);
                // Debug.Log("destroyed gameobj: " + item.Value);
            }
        }
    }

    private float _lastMapViewUpdateTime;

    private void UpdateMapViewPosition()
    {
        // Only update the map tile view periodically so as not to spam tile fetches
        if (Time.time < _lastMapViewUpdateTime + 1.0f)
        {
            return;
        }

        _lastMapViewUpdateTime = Time.time;

        // Update the map's view based on where our player is
        // _lightshipMapView.SetMapCenter(transform.position);
        CoverageClient.TryGetCoverage(OnTryGetCoverage);
    }
}