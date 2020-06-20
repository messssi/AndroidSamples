using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace AsyncSample
{
    /// <summary>
    /// https://qiita.com/ronkabu/items/997c4eee40e4668951a1
    /// LivedoorWebサービスの天気を表す
    /// </summary>
    public class LDTenki
    {
        public Pinpointlocation[] pinpointLocations { get; set; }
        public string link { get; set; }
        public Forecast[] forecasts { get; set; }
        public Location location { get; set; }
        public DateTime publicTime { get; set; }
        public Copyright copyright { get; set; }
        public string title { get; set; }
        public Description description { get; set; }
    }

    public class Location
    {
        public string city { get; set; }
        public string area { get; set; }
        public string prefecture { get; set; }
    }

    public class Copyright
    {
        public Provider[] provider { get; set; }
        public string link { get; set; }
        public string title { get; set; }
        public Image image { get; set; }
    }

    public class Image
    {
        public int width { get; set; }
        public string link { get; set; }
        public string url { get; set; }
        public string title { get; set; }
        public int height { get; set; }
    }

    public class Provider
    {
        public string link { get; set; }
        public string name { get; set; }
    }

    public class Description
    {
        public string text { get; set; }
        public DateTime publicTime { get; set; }
    }

    public class Pinpointlocation
    {
        public string link { get; set; }
        public string name { get; set; }
    }

    public class Forecast
    {
        public string dateLabel { get; set; }
        public string telop { get; set; }
        public string date { get; set; }
        public Temperature temperature { get; set; }
        public Image1 image { get; set; }
    }

    public class Temperature
    {
        public Min min { get; set; }
        public Max max { get; set; }
    }

    public class Min
    {
        public string celsius { get; set; }
        public string fahrenheit { get; set; }
    }

    public class Max
    {
        public string celsius { get; set; }
        public string fahrenheit { get; set; }
    }

    public class Image1
    {
        public int width { get; set; }
        public string url { get; set; }
        public string title { get; set; }
        public int height { get; set; }
    }
}