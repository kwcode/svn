using System;
using System.Data.SqlClient;
namespace trip
{
	public class SqlDataReaderCMD : IDisposable
	{
		private DataClass _dc;
		private DataPool _dp;
		public SqlDataReader Reader
		{
			get;
			private set;
		}
		internal SqlDataReaderCMD(DataPool dp, DataClass dc, SqlDataReader dr)
		{
			this._dc = dc;
			this._dp = dp;
			this.Reader = dr;
		}
		void IDisposable.Dispose()
		{
			this.Reader.Dispose();
			this._dp.Push(this._dc);
		}
	}
}
