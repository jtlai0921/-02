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
        }

    }

    /// <summary>
    /// 產品訊息類別
    /// </summary>
    public class Product
    {
        /// <summary>
        /// 產品名稱
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 生產日期
        /// </summary>
        public string Date { get; set; }
        /// <summary>
        /// 產品彩色
        /// </summary>
        public string Color { get; set; }
        /// <summary>
        /// 產品長度
        /// </summary>
        public double Size { get; set; }
    }
}
