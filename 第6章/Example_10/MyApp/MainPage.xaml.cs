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

            List<News> newsList = new List<News>();
            for (int i = 1; i <= 4; i++)
            {
                newsList.Add(new News { Category = "社會", Title = "範例新聞" + i.ToString(), Content = "測試新聞內容 " + i.ToString(), PublishDate = DateTime.Now.AddDays(i) });
            }
            for (int i = 5; i <= 7; i++)
            {
                newsList.Add(new News { Category = "娛樂", Title = "範例新聞" + i.ToString(), Content = "測試新聞內容 " + i.ToString(), PublishDate = DateTime.Now.AddDays(i) });
            }
            for (int i = 8; i <= 10; i++)
            {
                newsList.Add(new News { Category = "法制", Title = "範例新聞" + i.ToString(), Content = "測試新聞內容 " + i.ToString(), PublishDate = DateTime.Now.AddDays(i) });
            }

            // 對資料進行分群組
            var groups = from n in newsList
                         group n by n.Category;
            // 設定資料源
            this.cvs.Source = groups;
        }

    }


    public class News
    {
        /// <summary>
        /// 分類別
        /// </summary>
        public string Category { get; set; }
        /// <summary>
        /// 標題
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 發布日期
        /// </summary>
        public DateTime PublishDate { get; set; }
        /// <summary>
        /// 內容
        /// </summary>
        public string Content { get; set; }
    }


    public sealed class DateConverter : IValueConverter
    {
        public object Convert ( object value, Type targetType, object parameter, string language )
        {
            DateTime dt = (DateTime)value;
            return dt.ToString("yyyy-M-d");
        }

        public object ConvertBack ( object value, Type targetType, object parameter, string language )
        {
            return null;
        }
    }


}
