using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    class UserInfo
    {
        private static UserInfo _instance = null;
        private UserInfo()
        {

        }

        public static UserInfo Instance
        {
            get
            {
                if(_instance == null)
                {
                    _instance = new UserInfo();
                }
                return _instance;
            }
        }

        public int Id { get; set; }
        public string UserName { get; set; }
    }
}
