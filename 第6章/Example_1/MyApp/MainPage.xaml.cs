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

            Student stu = new Student { Name = "小楊", Course = "C語系" };
            // 案例化Binding物件
            Binding nameBinding = new Binding();
            // 設定源
            nameBinding.Source = stu;
            // 指定取得資料源中Name屬性的值
            nameBinding.Path = new PropertyPath("Name");

            Binding courseBinding = new Binding();
            // 設定源
            courseBinding.Source = stu;
            // 指定從資料源的Course屬性取得內容
            courseBinding.Path = new PropertyPath("Course");

            // 綁定方向為單向
            nameBinding.Mode = courseBinding.Mode = BindingMode.OneWay;

            // 將Binding案例與TextBlock控制項的Text屬性關聯
            tbName.SetBinding(TextBlock.TextProperty, nameBinding);
            tbCourse.SetBinding(TextBlock.TextProperty, courseBinding);
        }

    }


    public class Student
    {
        /// <summary>
        /// 學員姓名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 課程名稱
        /// </summary>
        public string Course { get; set; }
    }
}
