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
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Streams;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace MyApp
{
    /// <summary>
    /// 可用於自己或導覽至 Frame 內定的空白頁。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        ObservableCollection<StorageFile> picFiles = null;

        public MainPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;

            picFiles = new ObservableCollection<StorageFile>();
            this.lbPics.ItemsSource = picFiles;
        }


        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            // 從照片目錄中讀取檔案
            StorageFolder camrroll = KnownFolders.CameraRoll;
            var files = await camrroll.GetFilesAsync();
            picFiles.Clear();
            foreach (StorageFile f in files)
            {
                picFiles.Add(f);
            }
        }

        private async void OnClick(object sender, RoutedEventArgs e)
        {
            if (lbPics.SelectedIndex < 0) return;
            Button btn = sender as Button;
            btn.IsEnabled = false;
            StorageFile curFile = lbPics.SelectedItem as StorageFile;
            // 開啟檔案流
            using (IRandomAccessStream streamIn = await curFile.OpenReadAsync())
            {
                // 解碼圖形檔案
                BitmapDecoder decoder = await BitmapDecoder.CreateAsync(streamIn);
                // 取得像素資料
                PixelDataProvider pxprd = await decoder.GetPixelDataAsync(BitmapPixelFormat.Bgra8, BitmapAlphaMode.Straight, new BitmapTransform(), ExifOrientationMode.RespectExifOrientation, ColorManagementMode.DoNotColorManage);
                byte[] data = pxprd.DetachPixelData();
                // 建立新檔案
                StorageFolder savedPics=KnownFolders.SavedPictures;
                StorageFile newFile=await savedPics.CreateFileAsync(curFile.DisplayName + ".png", CreationCollisionOption.ReplaceExisting);
                // 解碼為PNG圖形
                using(IRandomAccessStream streamOut = await newFile.OpenAsync(FileAccessMode.ReadWrite))
                {
                    // 案例化解碼器
                    BitmapEncoder encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.PngEncoderId, streamOut);
                    // 寫入像素資料
                    encoder.SetPixelData(BitmapPixelFormat.Bgra8, BitmapAlphaMode.Straight, decoder.PixelWidth, decoder.PixelHeight, decoder.DpiX, decoder.DpiY, data);
                    // 傳送資料
                    await encoder.FlushAsync();
                }
            }
            btn.IsEnabled = true;
        }
    }


    public sealed class FileToBitmapConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            StorageFile srcFile = value as StorageFile;
            // 取得檔案縮圖
            var task = srcFile.GetThumbnailAsync(Windows.Storage.FileProperties.ThumbnailMode.PicturesView).AsTask();
            task.Wait();
            IRandomAccessStream inputStream = task.Result;
            // 建立圖形縮圖
            BitmapImage bmp = new BitmapImage();
            bmp.DecodePixelHeight = 100;
            bmp.SetSource(inputStream);
            return new
            {
                Preview = bmp,
                Name = srcFile.DisplayName,
                Type = srcFile.DisplayType
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return null;
        }
    }

}
