using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.Media;
using Windows.Foundation.Collections;
using Windows.Media.Playback;

namespace AudioTask
{
    public sealed class BgTask : IBackgroundTask
    {
        #region 私有字段
        SystemMediaTransportControls mdcontrol = null;
        BackgroundTaskDeferral deferral = null;
        MediaPlayer currentPlayer = null;
        // 背景音頻是否已執行的標志
        bool isRunning = false;
        // 指示是否第一次播放
        bool isFirstPlaying = false;
        #endregion
        public void Run(IBackgroundTaskInstance taskInstance)
        {
            mdcontrol = SystemMediaTransportControls.GetForCurrentView();
            mdcontrol.IsEnabled = true;
            // 容許使用播放/暫停按鈕
            mdcontrol.IsPlayEnabled = true;
            mdcontrol.IsPauseEnabled = true;
            // 處理ButtonPressed事件
            mdcontrol.ButtonPressed += mdcontrol_ButtonPressed;
            // 取得MediaPlayer案例
            currentPlayer = BackgroundMediaPlayer.Current;
            // 處理事件，接收來自前景套用程式的訊息
            BackgroundMediaPlayer.MessageReceivedFromForeground += BackgroundMediaPlayer_MessageReceivedFromForeground;
            // 關閉自動開始播放
            currentPlayer.AutoPlay = false;
            // 設定播放源
            Uri audioUri = new Uri("ms-appx:///我愛我家.mp3");
            currentPlayer.SetUriSource(audioUri);
            deferral = taskInstance.GetDeferral();
            // 當背景工作被取消時引發事件
            taskInstance.Canceled += task_Canceled;
            isRunning = true;
        }

        private void task_Canceled(IBackgroundTaskInstance sender, BackgroundTaskCancellationReason reason)
        {
            // 解除事件綁定
            mdcontrol.ButtonPressed -= mdcontrol_ButtonPressed;
            BackgroundMediaPlayer.MessageReceivedFromForeground -= BackgroundMediaPlayer_MessageReceivedFromForeground;
            // 知會系統背景工作完成
            deferral.Complete();
        }

        private void BackgroundMediaPlayer_MessageReceivedFromForeground(object sender, MediaPlayerDataReceivedEventArgs e)
        {
            // 接收訊息
            ValueSet val = e.Data;
            if (val.ContainsKey("action"))
            {
                string msg = val["action"] as string;
                if (msg.Equals("play")) //播放
                {
                    isFirstPlaying = true;
                    Play();
                    isFirstPlaying = false;
                }
                else //暫停
                {
                    Pause();
                }
            }
        }

        void mdcontrol_ButtonPressed(SystemMediaTransportControls sender, SystemMediaTransportControlsButtonPressedEventArgs args)
        {
            // 若果使用者按下了播放鍵
            if (args.Button == SystemMediaTransportControlsButton.Play)
            {
                Play();
            }
            // 若果使用者按下了暫停按鈕
            else if (args.Button == SystemMediaTransportControlsButton.Pause)
            {
                if (currentPlayer.CurrentState == MediaPlayerState.Playing)
                {
                    Pause();
                }
            }
        }

        #region 私有方法
        /// <summary>
        /// 播放
        /// </summary>
        void Play()
        {
            if (isRunning)
            {
                currentPlayer.Play();
                if (isFirstPlaying)
                {
                    // 更新系統多媒體控制界面的顯示內容
                    SystemMediaTransportControlsDisplayUpdater displayUpdater = mdcontrol.DisplayUpdater;
                    // 顯示型態為音樂
                    displayUpdater.Type = MediaPlaybackType.Music;
                    // 設定顯示的字段值
                    displayUpdater.MusicProperties.Artist = "戴嬈";
                    displayUpdater.MusicProperties.Title = "我愛我家";
                    // 更新顯示
                    displayUpdater.Update();
                }
            }
        }

        /// <summary>
        /// 暫停
        /// </summary>
        void Pause()
        {
            if (isRunning)
            {
                currentPlayer.Pause();
            }
        }
        #endregion
    }
}
