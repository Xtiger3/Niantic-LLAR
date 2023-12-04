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

    private float waypointYPos;
    //private List<int> notDisplayedNPCs = new List<int> { 0, 0, 1, 2, 3 };

    public static CoverageManager Inst;
    public int numNPC = 3;
    public Vector3 NPCPos;
    public GameObject NPC;

    public Dictionary<string, GameObject> currWayspots = new Dictionary<string, GameObject>();
    

    private void Start()
    {
        waypointYPos = obj.transform.position.y;

        //MapsLatLng mapLatLng = new MapsLatLng(42.2814713, -83.7435344);
        //Vector3 mapPos = _lightshipMapView.LatLngToScene(mapLatLng);
        //mapPos[1] = waypointYPos;
        //GameObject test = Instantiate(obj, mapPos, obj.transform.rotation);
        //test.GetComponent<WaypointController>().npcChoice = 0;


        if (Inst == null)
        {
            Inst = this;
        }

    }

    void Update()
    {
        NPCPos = NPC.transform.position;
        UpdateMapViewPosition();
    }

    private void OnTryGetCoverage(AreaTargetsResult args)
    {
        var areaTargets = args.AreaTargets;

        // Sort the area targets by proximity to the user
        areaTargets.Sort((a, b) =>
            a.Area.Centroid.Distance(args.QueryLocation).CompareTo(
                b.Area.Centroid.Distance(args.QueryLocation)));

        List<string> displayedWaypoints = new();

        // Find the 5 closest locations
        for (var i = 0; i < Math.Min(areaTargets.Count, 5); i++)
        {
            string wayspotName = areaTargets[i].Target.Name;
            displayedWaypoints.Add(wayspotName);
        }

        // Delete the far away location
        foreach(var item in currWayspots)
        {
            if (displayedWaypoints.Contains(item.Key))
            {
                // 
            }
            else if (areaTargets.Count > 0)
            {
                int choice = item.Value.GetComponent<WaypointController>().npcChoice;
                if (VocabularySet.Instance.ongoingCategory[VocabularySet.Instance.npcToIndex[choice]] == 1)
                {
                    VocabularySet.Instance.notDisplayedNPCs.Add(0);

                } else
                {
                    VocabularySet.Instance.notDisplayedNPCs.Add(choice);
                }

                if (VocabularySet.Instance.dontDisplayWayspots.Contains(item.Key))
                {
                    VocabularySet.Instance.dontDisplayWayspots.Remove(item.Key);
                }

                Destroy(item.Value);
                currWayspots.Remove(item.Key);
                // Debug.Log("destroyed gameobj: " + item.Value);
            }
        }

        // Only populate the dropdown with the closest 5 locations
        for (var i = 0; i < Math.Min(areaTargets.Count, 5); i++)
        {
            // Convert LatLng to game map position
            MapsLatLng mapLatLng = new MapsLatLng(areaTargets[i].Target.Center.Latitude, areaTargets[i].Target.Center.Longitude);
            Vector3 mapPos = _lightshipMapView.LatLngToScene(mapLatLng);
            mapPos[1] = waypointYPos; // y offset so that it is above the map

            string wayspotName = areaTargets[i].Target.Name;

            // If the wayspot is already displayed
            if (currWayspots.ContainsKey(wayspotName))
            {
                // Update the wayspot position
                currWayspots[wayspotName].transform.position = mapPos;
                // Debug.Log("updated gameobj: " + wayspotName);
            }
            else
            {
                // If the wayspot is already clicked
                if (VocabularySet.Instance.dontDisplayWayspots.Contains(wayspotName))
                {

                }

                else
                {
                    // Draw the new wayspot
                    GameObject wayspot = Instantiate(obj, mapPos, obj.transform.rotation);

                    // Add it to dictionary
                    currWayspots[areaTargets[i].Target.Name] = wayspot;

                    // Decide NPC for waypoint
                    int choice = VocabularySet.Instance.notDisplayedNPCs[UnityEngine.Random.Range(0, VocabularySet.Instance.notDisplayedNPCs.Count)];
                    wayspot.GetComponent<WaypointController>().npcChoice = choice;
                    wayspot.GetComponent<WaypointController>().wayspotName = areaTargets[i].Target.Name;

                    VocabularySet.Instance.notDisplayedNPCs.Remove(choice);

                    //Debug.Log("choice is: " + choice);
                    //Debug.Log("entire list is: " + VocabularySet.Instance.notDisplayedNPCs.ToString());

                    //Debug.Log("added gameobj: " + wayspotName);
                }

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