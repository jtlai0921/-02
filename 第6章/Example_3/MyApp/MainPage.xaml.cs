﻿using System;
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

            TaskItem item = new TaskItem
            {
                TaskID = 1000251,
                TaskName = "範例工序",
                TaskDesc = "範例描述。",
                TaskProgress = 60d
            };
            this.layoutRoot.DataContext = item;
        }

    }

    public class TaskItem
    {
        /// <summary>
        /// 工作ID
        /// </summary>
        public int TaskID { get; set; }
        /// <summary>
        /// 工作名稱
        /// </summary>
        public string TaskName { get; set; }
        /// <summary>
        /// 工作描述
        /// </summary>
        public string TaskDesc { get; set; }
        /// <summary>
        /// 工作進度
        /// </summary>
        public double TaskProgress { get; set; }
    }
}
