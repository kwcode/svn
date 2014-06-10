package htmlHelper;

import java.io.ByteArrayInputStream;
import java.io.ByteArrayOutputStream;
import java.io.IOException;
import java.io.InputStream;
import java.io.UnsupportedEncodingException;

public class ConvertType {

	/**
	 * 转换String
	 * 
	 * @param stream
	 *            InputStream
	 * @return
	 * @throws IOException
	 */
	public static String ConvertStreamToString(InputStream stream)
			throws IOException {
		StringBuffer out = new StringBuffer();
		byte[] b = new byte[4096];
		for (int n; (n = stream.read(b)) != -1;) {
			out.append(new String(b, 0, n));
		}
		return out.toString();
	}

	/**
	 * 转换InputStream
	 * 
	 * @param str
	 * @return
	 * @throws java.io.IOException
	 */
	public static InputStream ConvertSringToStream(String str)
			throws java.io.IOException {
		byte[] buff = str.getBytes();// str.getBytes("UTF-8"));
		ByteArrayInputStream stream = new ByteArrayInputStream(buff);
		return stream;
	}

	/**
	 * 转换byte[]
	 * 
	 * @param inStream
	 * @return
	 * @throws IOException
	 */
	public static byte[] ConvertStreamToByte(InputStream inStream)
			throws IOException {
		ByteArrayOutputStream swapStream = new ByteArrayOutputStream();
		byte[] buff = new byte[4400];
		int rc = 0;
		while ((rc = inStream.read(buff, 0, 100)) > 0) {
			swapStream.write(buff, 0, rc);
		}
		byte[] in2b = swapStream.toByteArray();
		return in2b;
	}

	/**
	 * 转换InputStream
	 * 
	 * @param buff
	 * @return
	 */
	public static InputStream ConvertByteToStream(byte[] buff) {
		return new ByteArrayInputStream(buff);
	}

	/**
	 * 转换byte[]
	 * 
	 * @param buff
	 * @return
	 * @throws UnsupportedEncodingException
	 */
	public static String ConvertByteToString(byte[] buff)
			throws UnsupportedEncodingException {
		// StringBuffer out = new StringBuffer();
		// out.append(buff);
		// return out.toString();
		return new String(buff, "UTF-8");
	}

	public static byte[] ConvertStringToByte(String str) {
		byte[] buff = str.getBytes();// str.getBytes("UTF-8"));
		return buff;
	}
}
