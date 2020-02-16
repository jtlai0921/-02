using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// “空白頁”項範本在 http://go.microsoft.com/fwlink/?LinkId=391641 上有介紹

namespace MyApp
{
    /// <summary>
    /// 可用於自己或導覽至 Frame 內定的空白頁。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;

            Car[] cars =
            {
                new Car { ID = "C1", Speed = 90d, Year = 1998 },
                new Car { ID = "C2", Speed = 100d, Year = 2010 },
                new Car { ID = "C3", Speed = 110d, Year = 2015 }
            };
            lbCars.ItemsSource = cars;
        }

    }


    public class Car
    {
        /// <summary>
        /// 汽車標誌
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// 時速
        /// </summary>
        public double Speed { get; set; }
        /// <summary>
        /// 年份
        /// </summary>
        public int Year { get; set; }
    }
}
