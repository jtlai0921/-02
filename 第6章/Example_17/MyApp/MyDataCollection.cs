using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices.WindowsRuntime;

namespace MyApp
{
    public sealed class MyDataCollection : ObservableCollection<double> , ISupportIncrementalLoading
    {
        #region 私有字段
        /// <summary>
        /// 指示載入動作是否正在進行
        /// </summary>
        bool isLoading = false;
        /// <summary>
        /// 用於產生隨機數的物件
        /// </summary>
        Random rand = new Random();
        /// <summary>
        /// 要載入的最大專案數
        /// </summary>
        const uint MAX_ITEM_COUNT = 10000;
        #endregion

        #region 實現ISupportIncrementalLoading接口的成員
        public bool HasMoreItems
        {
            get 
            {
                if (this.Count >= MAX_ITEM_COUNT)
                {
                    return false;
                }
                return true;
            }
        }

        public Windows.Foundation.IAsyncOperation<LoadMoreItemsResult> LoadMoreItemsAsync ( uint count )
        {
            if (isLoading)
            {
                throw new InvalidOperationException("目前正在載入，請等待本次載入完成後重試。");
            }
            // 實際載入的項數不應超過最大專案數
            // 目前已載入的項與最大專案數的差值
            uint x = MAX_ITEM_COUNT - (uint)this.Count;
            uint itemCountToLoad = 0;
            // 若果差值x小於預期載入的數量，則實際應載入的專案數應為x的值
            // 否則，實際應載入的項的數量等於預期要載入的專案數
            if (x < count)
            {
                itemCountToLoad = x;
            }
            else
            {
                itemCountToLoad = count;
            }
            return AsyncInfo.Run(c => LoadMoreItemsPrivateAsync(c, itemCountToLoad));
        }
        #endregion

        #region 私有方法
        private async Task<LoadMoreItemsResult> LoadMoreItemsPrivateAsync ( CancellationToken ct, uint n )
        {
            // 標誌載入動作已經開始
            isLoading = true;
            if (LoadItemsStarted != null)
            {
                LoadItemsStarted(this, EventArgs.Empty);
            }
            // 用於統計已成功載入的專案數量
            uint hadLoaded = 0;
            for (uint i = 0; i < n; i++)
            {
                await Task.Delay(200);
                double val = rand.NextDouble();
                this.Add(val);
                hadLoaded++;
            }
            // 標誌載入動作完成
            isLoading = false;
            if (LoadItemsCompleted != null)
            {
                LoadItemsCompleted(this, EventArgs.Empty);
            }
            return new LoadMoreItemsResult { Count = hadLoaded };
        }
        #endregion

        #region 事件
        /// <summary>
        /// 當開始載入動作時發生
        /// </summary>
        public event EventHandler LoadItemsStarted;
        /// <summary>
        /// 當載入動作完成時發生
        /// </summary>
        public event EventHandler LoadItemsCompleted;
        #endregion
    }
}
