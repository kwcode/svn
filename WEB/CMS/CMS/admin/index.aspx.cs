using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CMS.admin
{
    public partial class index : System.Web.UI.Page
    {
        BDAMenu dbMenu = new BDAMenu();
        private List<MenuEntity> _MenuList;
        protected List<MenuEntity> MenuList
        {
            get
            {
                if (_MenuList == null)
                    _MenuList = dbMenu.GetMenuList();
                return _MenuList;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}