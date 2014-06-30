using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CMS.entity
{
    public class UserCommon
    {
        public UserCommon()
        {
            Init();
        }

        private static List<UserInfo> _ulist = null;
        public static void RefreshData()
        {
            _ulist = null;
            Init();
        }
        private static void Init()
        {
            if (_ulist == null || _ulist.Count == 0)
            {
                _ulist = new List<UserInfo>();
                _ulist.Add(new UserInfo() { ID = 1, UserID = "1", Name = "tkw", NickName = "纯粹是糖", Password = "123", Email = "353328333@qq.com", LoginName = "tkw", SmallPhoto = "/images/smallphoto/blue_bird_16.png" });
                _ulist.Add(new UserInfo() { ID = 2, UserID = "2", Name = "csx", NickName = "AAAAAAAABBBBBBBBBBBBBBBBBBBBBBB添加的数据BBCCCCCCCCCCCCCCCCCCCCCCCCDDDDDDDDDDDDDDDDDDDEEEEEEEEEEEEEEEEAAAAAAAAAAAAAAAA", Password = "123", Email = "453352038@qq.com", LoginName = "tkw", SmallPhoto = "/images/smallphoto/ie_16.png" });
                _ulist.Add(new UserInfo() { ID = 3, UserID = "3", Name = "tkw", NickName = "纯粹是糖", Password = "123", Email = "353328333@qq.com", LoginName = "tkw", SmallPhoto = "/images/smallphoto/o_16.png" });
                _ulist.Add(new UserInfo() { ID = 4, UserID = "4", Name = "tkw", NickName = "纯粹是糖", Password = "123", Email = "353328333@qq.com", LoginName = "tkw", SmallPhoto = "/images/smallphoto/pin_suger_16.png" });
                _ulist.Add(new UserInfo() { ID = 5, UserID = "5", Name = "tkw", NickName = "纯粹是糖", Password = "123", Email = "353328333@qq.com", LoginName = "tkw", SmallPhoto = "/images/smallphoto/blue_bird_16.png" });
                _ulist.Add(new UserInfo() { ID = 6, UserID = "1", Name = "tkw", NickName = "纯粹是糖", Password = "123", Email = "353328333@qq.com", LoginName = "tkw", SmallPhoto = "/images/smallphoto/blue_bird_16.png" });
                _ulist.Add(new UserInfo() { ID = 7, UserID = "2", Name = "csx", NickName = "AAAAAAAAAAAAAAAAAAAAAAAA", Password = "123", Email = "453352038@qq.com", LoginName = "tkw", SmallPhoto = "/images/smallphoto/ie_16.png" });
                _ulist.Add(new UserInfo() { ID = 8, UserID = "3", Name = "tkw", NickName = "纯粹是糖", Password = "123", Email = "353328333@qq.com", LoginName = "tkw", SmallPhoto = "/images/smallphoto/o_16.png" });
                _ulist.Add(new UserInfo() { ID = 9, UserID = "4", Name = "tkw", NickName = "纯粹是糖", Password = "123", Email = "353328333@qq.com", LoginName = "tkw", SmallPhoto = "/images/smallphoto/pin_suger_16.png" });
                _ulist.Add(new UserInfo() { ID = 10, UserID = "5", Name = "tkw", NickName = "纯粹是糖", Password = "123", Email = "353328333@qq.com", LoginName = "tkw", SmallPhoto = "/images/smallphoto/blue_bird_16.png" });
                _ulist.Add(new UserInfo() { ID = 11, UserID = "1", Name = "tkw", NickName = "纯粹是糖", Password = "123", Email = "353328333@qq.com", LoginName = "tkw", SmallPhoto = "/images/smallphoto/blue_bird_16.png" });
                _ulist.Add(new UserInfo() { ID = 12, UserID = "2", Name = "csx", NickName = "AAAAAAAAAAAAAAAAAAAAAAAA", Password = "123", Email = "453352038@qq.com", LoginName = "tkw", SmallPhoto = "/images/smallphoto/ie_16.png" });
                _ulist.Add(new UserInfo() { ID = 13, UserID = "3", Name = "tkw", NickName = "纯粹是糖", Password = "123", Email = "353328333@qq.com", LoginName = "tkw", SmallPhoto = "/images/smallphoto/o_16.png" });
                _ulist.Add(new UserInfo() { ID = 14, UserID = "4", Name = "tkw", NickName = "纯粹是糖", Password = "123", Email = "353328333@qq.com", LoginName = "tkw", SmallPhoto = "/images/smallphoto/pin_suger_16.png" });
                _ulist.Add(new UserInfo() { ID = 15, UserID = "5", Name = "tkw", NickName = "纯粹是糖", Password = "123", Email = "353328333@qq.com", LoginName = "tkw", SmallPhoto = "/images/smallphoto/blue_bird_16.png" });
                _ulist.Add(new UserInfo() { ID = 16, UserID = "1", Name = "tkw", NickName = "纯粹是糖", Password = "123", Email = "353328333@qq.com", LoginName = "tkw", SmallPhoto = "/images/smallphoto/blue_bird_16.png" });
                _ulist.Add(new UserInfo() { ID = 17, UserID = "2", Name = "csx", NickName = "AAAAAAAAAAAAAAAAAAAAAAAA", Password = "123", Email = "453352038@qq.com", LoginName = "tkw", SmallPhoto = "/images/smallphoto/ie_16.png" });
                _ulist.Add(new UserInfo() { ID = 18, UserID = "3", Name = "tkw", NickName = "纯粹是糖", Password = "123", Email = "353328333@qq.com", LoginName = "tkw", SmallPhoto = "/images/smallphoto/o_16.png" });
                _ulist.Add(new UserInfo() { ID = 19, UserID = "4", Name = "tkw", NickName = "纯粹是糖", Password = "123", Email = "353328333@qq.com", LoginName = "tkw", SmallPhoto = "/images/smallphoto/pin_suger_16.png" });
                _ulist.Add(new UserInfo() { ID = 20, UserID = "5", Name = "tkw", NickName = "纯粹是糖", Password = "123", Email = "353328333@qq.com", LoginName = "tkw", SmallPhoto = "/images/smallphoto/blue_bird_16.png" });

            }
        }
        public static List<UserInfo> GetAllUser()
        {
            Init();
            return _ulist;
        }
        public static int GetUserCount()
        {
            return GetAllUser().Count();
        }
        public static List<UserInfo> GetUser(int startIndex, int endIndex)
        {
            if (_ulist == null || _ulist.Count == 0)
                Init();
            if (startIndex < 0) startIndex = 1;
            if (endIndex < 0) startIndex = 1;
            List<UserInfo> ulist = new List<UserInfo>();
            for (int i = startIndex; i < endIndex; i++)
            {
                if (i < _ulist.Count)
                    ulist.Add(_ulist[i]);
            }
            return ulist;
        }

        public static void Remove(UserInfo user)
        {
            _ulist.Remove(user);
        }
        public static void Remove(int id)
        {
            UserInfo user = _ulist.Find((delegate(UserInfo u) { return u.ID == id; }));
            if (user != null)
                Remove(user);
        }

        public static int ItemCount
        {
            get
            {
                if (_ulist == null)
                    Init();
                return _ulist.Count;
            }
        }
    }
}