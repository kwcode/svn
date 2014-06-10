package clientDebug;

import htmlHelper.person;

import javax.xml.ws.Service;

import BaseApiCommon.InvokeParam;
import UserServiceInterface.IUserService;
import UserServiceInterface.UserInfo;
import UserServiceInterface.javaUserInfo;

public class Main {

	public static String url = "http://localhost:2015/HLService.aspx?Type=java";

	public static void main(String[] args) {
		System.out.println("程序开始！");
		try {
			// C# 服务端的 类定义名称空间必须和java包名一致
			// C# 实体类必须含有字段对应java属性 
			javaUserInfo us = (javaUserInfo) InvokeMember(
					"UserServiceInterface.IUserService", "GetAllUser", null);
			System.out.println(us.get_id() + us.get_code());
		} catch (Exception e) {
		}
	}

	/**
	 * 根据接口名称 、方法名称 、参数，从服务端请求数据
	 * 
	 * @param intface
	 *            包.接口名
	 * @param moduleName
	 *            方法名称
	 * @param p
	 *            参数
	 * @return
	 * 
	 */
	public static <T> T InvokeMember(String intface, String moduleName,
			Object[] p) {
		Object obj = null;
		try {
			BaseApiCommon.javaInvokeParam parm = new BaseApiCommon.javaInvokeParam(
					intface, moduleName, p);
			byte[] buff = htmlHelper.XmlEasy.xmlOutputByte(parm);
			byte[] bytes = htmlHelper.GetPost.post(url, buff);
			obj = htmlHelper.XmlEasy.xmlInputObj(bytes);
		} catch (Exception e) {
		}
		return (T) obj;
	}
}
