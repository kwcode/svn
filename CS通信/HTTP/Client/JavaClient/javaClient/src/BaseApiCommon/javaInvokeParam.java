package BaseApiCommon;

import java.io.Serializable;

public class javaInvokeParam implements Serializable {

	private String Interface;
	private String MethodName;
	private Object[] Parameters;

	/**
	 * @return the interface
	 */
	public String getInterface() {
		return Interface;
	}

	/**
	 * @param interface1
	 *            the interface to set
	 */
	public void setInterface(String interface1) {
		Interface = interface1;
	}

	/**
	 * @return the methodName
	 */
	public String getMethodName() {
		return MethodName;
	}

	/**
	 * @param methodName
	 *            the methodName to set
	 */
	public void setMethodName(String methodName) {
		MethodName = methodName;
	}

	public javaInvokeParam() {
	}

	public javaInvokeParam(String Interface, String MethodName,
			Object[] Parameters) {
		this.Interface = Interface;
		this.MethodName = MethodName;
		this.Parameters = Parameters;
	}

	/**
	 * @return the parameters
	 */
	public Object[] getParameters() {
		return Parameters;
	}

	/**
	 * @param parameters
	 *            the parameters to set
	 */
	public void setParameters(Object[] parameters) {
		Parameters = parameters;
	}
}
