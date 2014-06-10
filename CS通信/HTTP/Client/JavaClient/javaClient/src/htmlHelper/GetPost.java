package htmlHelper;

import java.io.IOException;
import java.io.InputStream;
import java.net.HttpURLConnection;
import java.net.URL;

public class GetPost {

	public static void main(String[] args) {
		// 此地址为C# 服务端固定地址
		String url = "http://localhost:2015/HLService.aspx?type=0";
		try {

			byte[] PostData = htmlHelper.ConvertType
					.ConvertStringToByte("paraaa123我加大们");
			byte[] buff = post(url, PostData);
			String str = htmlHelper.ConvertType.ConvertByteToString(buff);
			System.out.println(str);

			buff = get("http://www.baidu.com");
			str = htmlHelper.ConvertType.ConvertByteToString(buff);
			System.out.println(str);
		} catch (Exception ex) {
			// TODO: handle exception
			ex.printStackTrace();
		}
	}

	/**
	 * java.net 的应用[java的标准接口]
	 */
	public static byte[] get(String httpurl) throws IOException {
		URL url = new URL(httpurl);
		java.net.HttpURLConnection con = (HttpURLConnection) url
				.openConnection();

		con.setConnectTimeout(100000);// 连接超时 单位毫秒
		con.setReadTimeout(20000);// 读取超时 单位毫秒
		InputStream inStream = con.getInputStream();
		return htmlHelper.ConvertType.ConvertStreamToByte(inStream);
	}

	public static byte[] post(String httpurl, byte[] buff) throws IOException {
		URL url = new URL(httpurl);
		java.net.HttpURLConnection con = (HttpURLConnection) url
				.openConnection();
		con.setRequestMethod("POST");// 默认GETk
		con.setConnectTimeout(100000);// 连接超时 单位毫秒
		con.setReadTimeout(20000);// 读取超时 单位毫秒
		con.setDoOutput(true);// 是否输入参数
		con.getOutputStream().write(buff);// 输入参数
		InputStream inStream = con.getInputStream();
		return htmlHelper.ConvertType.ConvertStreamToByte(inStream);
	}
}
