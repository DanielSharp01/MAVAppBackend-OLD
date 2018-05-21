﻿using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MAVAppBackend
{
    /// <summary>
    /// Name with Latitude, longitude information.
    /// See also: <seealso cref="GoogleMaps.RequestPlaces"/>
    /// </summary>
    public class PlacesData
    {
        /// <summary>
        /// Name of the place
        /// </summary>
        public string Name
        {
            private set;
            get;
        }

        /// <summary>
        /// GPS Position as latitude (X) longitude (Y)
        /// </summary>
        public Vector2 GPSCoord
        {
            private set;
            get;
        }

        /// <param name="name">Name of the place</param>
        /// <param name="gpsCoord">GPS Position as latitude (X) longitude (Y)</param>
        public PlacesData(string name, Vector2 gpsCoord)
        {
            Name = name;
            GPSCoord = gpsCoord;
        }
    }

    /// <summary>
    /// Google maps APIs
    /// </summary>
    public class GoogleMaps
    {
        /// <summary>
        /// Your own GooglePlaces API key
        /// Note: You do have to set this up in your environment variables.
        /// </summary>
        private static string GooglePlacesAPIKey = Environment.GetEnvironmentVariable("PlacesAPIKey", EnvironmentVariableTarget.User);
        
        /// <summary>
        /// Request GooglePlaces API for train stations in a given position
        /// </summary>
        /// <param name="position">GPS Position as latitude (X) longitude (Y)</param>
        /// <returns>List of Google Places data containing name and coordinate of found stations</returns>
        public static List<PlacesData> RequestPlaces(Vector2 position)
        {
            List<PlacesData> ret = new List<PlacesData>();
            HttpWebRequest request = WebRequest.CreateHttp("https://maps.googleapis.com/maps/api/place/nearbysearch/json?location=" + position.ToString() + "&radius=2000&type=train_station&key=" + GooglePlacesAPIKey);
            request.Method = "GET";
            using (WebResponse response = request.GetResponse())
            {
                using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    string json = reader.ReadToEnd();
                    JArray results = JObject.Parse(json)["results"] as JArray;
                    foreach (JObject place in results)
                    {
                        ret.Add(new PlacesData(place["name"].ToString(),
                                               new Vector2(place["geometry"]["location"]["lat"].ToString(), place["geometry"]["location"]["lng"].ToString())));
                    }
                }
            }

            return ret;
        }
    }
}
