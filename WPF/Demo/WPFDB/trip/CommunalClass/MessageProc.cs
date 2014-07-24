using System;
namespace trip.CommunalClass
{
	/// <summary>
	/// 消息处理类
	/// </summary>
	public class MessageProc : MarshalByRefObject
	{
		/// <summary>
		/// 消息事件委托
		/// </summary>
		/// <param name="type"></param>
		/// <param name="mg"></param>
		public delegate void myNoticeEvent(string type, string mg);
		/// <summary>
		/// 消息事件
		/// </summary>
		public event MessageProc.myNoticeEvent noticeEvent;
		/// <summary>
		///
		/// </summary>
		/// <returns></returns>
		public override object InitializeLifetimeService()
		{
			return null;
		}
		/// <summary>
		/// 消息类别,默认为当前类名
		/// </summary>
		/// <returns></returns>
		protected virtual string msgType()
		{
			return base.GetType().Name;
		}
		/// <summary>
		/// 显示消息
		/// </summary>
		/// <param name="mg">消息内容</param>
		protected void msg(string mg)
		{
			if (this.noticeEvent != null)
			{
				MessageProc.myNoticeEvent notice = new MessageProc.myNoticeEvent(this.SubEvent);
				notice.BeginInvoke(this.msgType(), mg, null, null);
			}
		}
		private void SubEvent(string type, string mg)
		{
			if (this.noticeEvent != null)
			{
				this.noticeEvent(type, mg);
			}
		}
		/// <summary>
		/// 显示消息
		/// </summary>
		/// <param name="type">消息类别</param>
		/// <param name="mg">消息内容</param>
		protected void msg(string type, string mg)
		{
			if (this.noticeEvent != null)
			{
				this.noticeEvent(type, mg);
			}
		}
		protected void Show(string type, string content, DebugType debugType = DebugType.LogOnly)
		{
			switch (debugType)
			{
				case DebugType.Both:
				{
					this.msg(content);
					LogsRecord.write(type, content);
					break;
				}
				case DebugType.LogOnly:
				{
					LogsRecord.write(type, content);
					break;
				}
				case DebugType.MsgOnly:
				{
					this.msg(content);
					break;
				}
			}
		}
		protected void Show(string content, DebugType debugType = DebugType.LogOnly)
		{
			this.Show("Error", content, debugType);
		}
	}
}
