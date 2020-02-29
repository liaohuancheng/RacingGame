using UnityEngine;

//namespace Assets.Scripts.UI
//{
//    public class ViewBase: MonoBehaviour
//    {
//        public delegate void SFViewUdpate(float dt);

//        protected ISDelegateBase m_delegate;
//        bool m_isViewRemoved = false;

//        public bool isViewRemoved { get { return m_isViewRemoved; } }

//        public void removeView(bool immediately = false)
//        {
//            m_delegate.onViewRemoved();
//            m_isViewRemoved = true;
//            if (immediately)
//            {
//                GameObject.DestroyImmediate(gameObject);
//            }
//            else
//            {
//                GameObject.Destroy(gameObject);
//            }
//        }

//        void OnDestroy()
//        {
//            if (!m_isViewRemoved)
//            {
//                // 意外的情况，没有清理presenter的话补调用一下
//                m_delegate.onViewRemoved();
//            }
//            m_delegate = null;
//        }
//    }
//}
