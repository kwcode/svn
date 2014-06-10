package UserServiceInterface;

public interface IUserService {
	void Save(UserInfo user);

	UserInfo GetUserByID(String id);

	UserInfo[] GetAllUser();

}
