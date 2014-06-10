package UserServiceInterface;

import java.io.Serializable;

public class javaUserInfo implements Serializable {

	private String _id;
	private String _code;

	/**
	 * @return the _id
	 */
	public String get_id() {
		return _id;
	}

	/**
	 * @param _id
	 *            the _id to set
	 */
	public void set_id(String _id) {
		this._id = _id;
	}

	/**
	 * @return the _code
	 */
	public String get_code() {
		return _code;
	}

	/**
	 * @param _code
	 *            the _code to set
	 */
	public void set_code(String _code) {
		this._code = _code;
	}

}