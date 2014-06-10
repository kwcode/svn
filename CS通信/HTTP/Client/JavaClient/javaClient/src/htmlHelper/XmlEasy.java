package htmlHelper;

import java.io.IOException;
import java.util.ArrayList;
import java.util.List;

import org.jdom.Document;
import org.jdom.Element;
import org.jdom.input.SAXBuilder;
import org.jdom.output.XMLOutputter;

import wox.serial.ObjectReader;
import wox.serial.ObjectWriter;
import wox.serial.SimpleReader;
import wox.serial.SimpleWriter;

public class XmlEasy {

	static String url = "http://localhost:2015/HLService.aspx?type=0";

	public static void main(String[] args) {
		// TODO Auto-generated method stub
		person person = new person(1, "tkw", "ÌÆ¿ªÎÄ");
		List<person> list = new ArrayList<person>();
		list.add(person);
		try {
			byte[] buff = xmlOutputByte(list);
			byte[] pbytes = htmlHelper.GetPost.post(url, buff);

			Object obj = xmlInputObj(pbytes);
			if (obj == null) {
				System.out.println("objÎª¿Õ£¡");
				return;
			} else {
				ArrayList<person> ar = (ArrayList<person>) obj;
				System.out.println(ar.get(0).getName());
			}

		} catch (Exception ex) {
			// TODO: handle exception
			ex.printStackTrace();
		}
	}

	public static byte[] xmlOutputByte(Object obj) throws IOException {
		byte[] buff = null;
		ObjectWriter writer = new SimpleWriter();
		Element el = writer.write(obj);
		XMLOutputter out = new XMLOutputter();
		java.io.ByteArrayOutputStream outstream = new java.io.ByteArrayOutputStream();
		out.output(el, outstream);
		buff = outstream.toByteArray();
		return buff;
	}

	public static Object xmlInputObj(byte[] buff) {
		try {
			SAXBuilder builder = new SAXBuilder();
			java.io.ByteArrayInputStream instream = new java.io.ByteArrayInputStream(
					buff);
			Document doc = builder.build(instream);
			Element el = doc.getRootElement();
			ObjectReader reader = new SimpleReader();
			return reader.read(el);
		} catch (Exception e) {
			e.printStackTrace();
			return null;
		}
	}
}
