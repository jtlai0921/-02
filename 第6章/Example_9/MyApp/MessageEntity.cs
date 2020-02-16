using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp
{
    public class MessageEntity
    {
        /// <summary>
        /// 訊息型態
        /// </summary>
        public enum MsgType
        {
            /// <summary>
            /// 接收到的訊息
            /// </summary>
            From,
            /// <summary>
            /// 傳送的訊息
            /// </summary>
            To
        }


        /// <summary>
        /// 訊息內容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 訊息型態
        /// </summary>
        public MsgType MessageType { get; set; }
    }
}
