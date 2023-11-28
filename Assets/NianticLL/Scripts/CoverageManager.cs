using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Niantic.Lightship.AR;
using Niantic.Lightship.AR.VpsCoverage;
using Niantic.Lightship.Maps;
using Niantic.Lightship.Maps.Core.Coordinates;
using Niantic.Lightship.Maps.MapLayers.Components;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

 using ArdkLatLng = Niantic.Lightship.AR.VpsCoverage.LatLng;
using MapsLatLng = Niantic.Lightship.Maps.Core.Coordinates.LatLng;

public class CoverageManager : MonoBehaviour
{

    [SerializeField]
    private LightshipMapView _mapView;

    private CoverageClient _coverageClient;

    [SerializeField]
    private int _queryRadius;

    [SerializeField]
    private LayerLineRenderer _areaBorder;

    // Start is called before the first frame update
    void Start()
    {
        //_coverageClient = CoverageClientFactory.Create(RuntimeEnvironment.LiveDevice);
        RequestAreasAroundMapCenter();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void RequestAreasAroundMapCenter()
    {
         var mapCenter = ConvertArdkLatLng(_mapView.MapCenter);
         //_coverageClient.RequestCoverageAreas(_mapView.MapCenter, _queryRadius, OnAreasResult );
    }

    public void OnAreasResult(CoverageAreasResult areaResult)
    {
        if (areaResult.Status != ResponseStatus.Success)
        {
            return;
        }

        foreach (var area in areaResult.Areas)
        {
            var areaID = area.LocalizationTargetIdentifiers[0];
             _areaBorder.DrawLoop(ConvertMapsLatLng(area.Shape), areaID);
            //Instantiate(GameObject,position,Quaternion.identity)
        }
    }

    private ArdkLatLng ConvertArdkLatLng(MapsLatLng mapsLatLng)
    {
        return new ArdkLatLng(mapsLatLng.Latitude, mapsLatLng.Longitude);
    }

    private MapsLatLng ConvertMapsLatLng(ArdkLatLng ardkLatLng)
    {
        return new MapsLatLng(ardkLatLng.Latitude, ardkLatLng.Longitude);
    }

    private MapsLatLng[] ConvertMapsLatLng(ArdkLatLng[] ardkLatLng)
    {
        var results = new MapsLatLng[ardkLatLng.Length];
        for (int i = 0; i < results.Length; i++)
        {
            results[i] = ConvertMapsLatLng(ardkLatLng[i]);
        }

        return results;
    }
}