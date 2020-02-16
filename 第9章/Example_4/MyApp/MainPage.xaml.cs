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
using Windows.UI.Xaml.Media.Imaging;
using System.Runtime.InteropServices.WindowsRuntime;

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

        private void OnClick(object sender, RoutedEventArgs e)
        {
            byte[] data = new byte[8 * 6 * 4];
            for (int i = 0; i < data.Length; i += 4)
            {
                if ( (i / 4) % 2 == 0)
                {
                    data[i] = 0;        //B
                    data[i + 1] = 0;    //G
                    data[i + 2] = 255;  //R
                    data[i + 3] = 255;  //A
                }
                else
                {
                    data[i] = 0;        //B
                    data[i + 1] = 255;  //G
                    data[i + 2] = 0;    //R
                    data[i + 3] = 255;  //A
                }
            }
            WriteableBitmap wb = new WriteableBitmap(8, 6);
            data.CopyTo(wb.PixelBuffer);
            this.img.Source = wb;
        }
    }
}
