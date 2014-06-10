package BaseApiCommon;

import java.io.Serializable;

/**
 * Class InvokeParam
 */
// C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET
// attributes:
// [DataContract]
public class InvokeParam implements Serializable {
	public String Interface;
	public String MethodName;
	public Object[] Parameters;

}