using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using XMLContrast.Control;
using WPF.CustomControl;
namespace XMLContrast
{
    /// <summary>
    /// TablViewCtrl.xaml 的交互逻辑
    /// </summary>
    public partial class TablViewCtrl : HLUserControlExt
    {
        public TablViewCtrl()
        {
            InitializeComponent();
        }
        #region 属性 、缓存
        /// <summary>
        /// 招标信息
        /// </summary>
        private List<XMLNodeItem> _TenderDataNodes = new List<XMLNodeItem>();
        /// <summary>
        /// 导入投清单
        /// </summary>
        private List<XMLNodeItem> _BidDataNodes = new List<XMLNodeItem>();

        #endregion
        internal void Init(List<XMLNodeItem> tenderDataNodes, List<XMLNodeItem> bidDataNodes)
        {
            Clear();
            _TenderDataNodes = tenderDataNodes;
            _BidDataNodes = bidDataNodes;
        }
        Dictionary<string, UserControl> _UClist = new Dictionary<string, UserControl>();
        internal void SelectedItem(string tagstr, object owner)
        {
            if (string.IsNullOrEmpty(tagstr)) return;
            if (tagstr == "0")
            {
                if (_UClist.Keys.Contains(tagstr))
                    tabItem.Content = _UClist[tagstr];
                else
                {
                    // EngineeringStructuresCtrl es = new EngineeringStructuresCtrl(_TenderDataNodes, _BidDataNodes);//1、工程结构
                    EngineeringCtrl es = new EngineeringCtrl(_TenderDataNodes, _BidDataNodes, owner);
                    tabItem.Content = es;
                    _UClist.Add(tagstr, es);
                }
            }
            if (tagstr == "1")
            {
                if (_UClist.Keys.Contains(tagstr))
                    tabItem.Content = _UClist[tagstr];
                else
                {
                    //   FeesRatesCtrl es = new FeesRatesCtrl(_TenderDataNodes, _BidDataNodes, owner);//2、分部分项清单对比
                    清单子目 es = new 清单子目(_TenderDataNodes, _BidDataNodes, owner);
                    tabItem.Content = es;
                    _UClist.Add(tagstr, es);
                }
            }
            if (tagstr == "2")
            {
                if (_UClist.Keys.Contains(tagstr))
                    tabItem.Content = _UClist[tagstr];
                else
                {
                    MainMaterialCtrl es = new MainMaterialCtrl(_TenderDataNodes, _BidDataNodes, owner);//3、主要材料
                    tabItem.Content = es;
                    _UClist.Add(tagstr, es);
                }
            }
            if (tagstr == "3")
            {
                if (_UClist.Keys.Contains(tagstr))
                    tabItem.Content = _UClist[tagstr];
                else
                {
                    // RateMeasuresCtrl es = new RateMeasuresCtrl(_TenderDataNodes, _BidDataNodes, owner);//4 费率措施
                    措施一对比 es = new 措施一对比(_TenderDataNodes, _BidDataNodes, owner);
                    tabItem.Content = es;
                    _UClist.Add(tagstr, es);
                }
            }

            if (tagstr == "4")
            {
                if (_UClist.Keys.Contains(tagstr))
                    tabItem.Content = _UClist[tagstr];
                else
                {
                    // FixedMeasuresCtrl es = new FixedMeasuresCtrl(_TenderDataNodes, _BidDataNodes, owner);//5定额 措施
                    定额措施子目 es = new 定额措施子目(_TenderDataNodes, _BidDataNodes, owner);
                    tabItem.Content = es;
                    _UClist.Add(tagstr, es);
                }
            }
            if (tagstr == "5")
            {
                if (_UClist.Keys.Contains(tagstr))
                    tabItem.Content = _UClist[tagstr];
                else
                {
                    //  ProvisionalSumsSubheadCtrl es = new ProvisionalSumsSubheadCtrl(_TenderDataNodes, _BidDataNodes, owner);//6 暂列措施金额
                    暂列金对比 es = new 暂列金对比(_TenderDataNodes, _BidDataNodes, owner);
                    tabItem.Content = es;
                    _UClist.Add(tagstr, es);
                }
            }
            if (tagstr == "6")
            {
                if (_UClist.Keys.Contains(tagstr))
                    tabItem.Content = _UClist[tagstr];
                else
                {
                    //  MaterialValuationCtrl es = new MaterialValuationCtrl(_TenderDataNodes, _BidDataNodes, owner);//7材料暂估金额
                    材料暂估价对比 es = new 材料暂估价对比(_TenderDataNodes, _BidDataNodes, owner);
                    tabItem.Content = es;
                    _UClist.Add(tagstr, es);
                }
            }
            if (tagstr == "7")
            {
                if (_UClist.Keys.Contains(tagstr))
                    tabItem.Content = _UClist[tagstr];
                else
                {
                    //   ProfessionalEngineeringValuationCtrl es = new ProfessionalEngineeringValuationCtrl(_TenderDataNodes, _BidDataNodes, owner);//8\专业工程暂估价
                    专业工程暂估价 es = new 专业工程暂估价(_TenderDataNodes, _BidDataNodes, owner);
                    tabItem.Content = es;
                    _UClist.Add(tagstr, es);
                }
            }
            if (tagstr == "8")
            {
                if (_UClist.Keys.Contains(tagstr))
                    tabItem.Content = _UClist[tagstr];
                else
                {
                    //  TurnkeyServiceCtrl es = new TurnkeyServiceCtrl(_TenderDataNodes, _BidDataNodes, owner);//总承包服务费子目
                    总承包服务费子目 es = new 总承包服务费子目(_TenderDataNodes, _BidDataNodes, owner);
                    tabItem.Content = es;
                    _UClist.Add(tagstr, es);
                }
            }
            if (tagstr == "9")
            {
                if (_UClist.Keys.Contains(tagstr))
                    tabItem.Content = _UClist[tagstr];
                else
                {
                    //  SafetyRateCtrl es = new SafetyRateCtrl(_TenderDataNodes, _BidDataNodes, owner);//安全
                    安全文明施工费 es = new 安全文明施工费(_TenderDataNodes, _BidDataNodes, owner);
                    tabItem.Content = es;
                    _UClist.Add(tagstr, es);
                }
            }
            if (tagstr == "10")
            {
                if (_UClist.Keys.Contains(tagstr))
                    tabItem.Content = _UClist[tagstr];
                else
                {
                    //GuiHuiRatesCtrl es = new GuiHuiRatesCtrl(_TenderDataNodes, _BidDataNodes, owner);//规费子目
                    规费子目 es = new 规费子目(_TenderDataNodes, _BidDataNodes, owner);
                    tabItem.Content = es;
                    _UClist.Add(tagstr, es);
                }
            }
            if (tagstr == "11")
            {
                if (_UClist.Keys.Contains(tagstr))
                    tabItem.Content = _UClist[tagstr];
                else
                {
                    // TaxesRateCtrl es = new TaxesRateCtrl(_TenderDataNodes, _BidDataNodes, owner);//税金
                    税金 es = new 税金(_TenderDataNodes, _BidDataNodes, owner);
                    tabItem.Content = es;
                    _UClist.Add(tagstr, es);
                }
            }
            if (tagstr == "12")
            {
                if (_UClist.Keys.Contains(tagstr))
                    tabItem.Content = _UClist[tagstr];
                else
                {
                    FeesPriceCtrl es = new FeesPriceCtrl(_TenderDataNodes, _BidDataNodes, owner);//清单单价
                    tabItem.Content = es;
                    _UClist.Add(tagstr, es);
                }
            }
            if (tagstr == "13")
            {
                if (_UClist.Keys.Contains(tagstr))
                    tabItem.Content = _UClist[tagstr];
                else
                {
                    FixedMeasures2Ctrl es = new FixedMeasures2Ctrl(_TenderDataNodes, _BidDataNodes, owner);//  措施单价
                    tabItem.Content = es;
                    _UClist.Add(tagstr, es);
                }
            }
        }

        public string ItemHeader { set { tabItem.Header = value; } }
        /// <summary>
        /// 清理
        /// </summary>
        public void Clear()
        {
            _UClist = new Dictionary<string, UserControl>();
        }
    }
}
