using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace CMS
{
    /// <summary>
    /// 页码
    /// </summary>
    public class Pagination
    {
        /// <summary>
        /// 初始化页码输出
        /// </summary>
        /// <param name="PageSize">每页记录数</param>
        /// <param name="CurPage">当前页页码</param>
        /// <param name="TotalRecords">总记录数</param>
        public Pagination(int CurPage, int PageSize, int TotalRecords, bool DisplayEdgeSpace = true, int DisplayNumberCount = 6, string Anchor = "")
        {
            this.PageSize = PageSize;
            this.CurPage = CurPage > 0 ? CurPage : 1;
            this.TotalRecords = TotalRecords;
            this.DisplayNumberCount = DisplayNumberCount;
            this.DisplayEdgeSpace = DisplayEdgeSpace;
            this.Anchor = Anchor;
        }

        #region [Pagination]页面基本控制属性

        private int pageNumbers = -1;
        private int prevPage = -1;
        private int nextPage = -1;
        private int curRecord = -1;
        private bool showNextPage, showPrevPage;

        /// <summary>
        /// 每页记录数
        /// </summary>
        private int PageSize { get; set; }

        /// <summary>
        /// 当前页页码
        /// </summary>
        private int CurPage { get; set; }

        /// <summary>
        /// 总记录数
        /// </summary>
        private int TotalRecords { get; set; }

        /// <summary>
        /// 显示页码按钮数目
        /// </summary>
        private int DisplayNumberCount { get; set; }

        /// <summary>
        /// 边界按钮是否显示
        /// </summary>
        private bool DisplayEdgeSpace { get; set; }

        /// <summary>
        /// 显示开始和结尾连接数
        /// </summary>
        private int DisplayEdgeCount { get; set; }

        /// <summary>
        /// 页面链接
        /// </summary>
        private string Url { get; set; }

        /// <summary>
        /// 页面锚点
        /// </summary>
        private string Anchor { get; set; }

        /// <summary>
        /// 上一页
        /// </summary> 
        private int PrevPage
        {
            get { return prevPage; }
        }

        /// <summary>
        /// 上一页显示标志
        /// </summary>
        private bool ShowPrevPage
        {
            get
            {
                if (prevPage < 0)
                {
                    if (CurPage > 1)
                    {
                        prevPage = CurPage - 1;
                        showPrevPage = true;
                    }
                    else
                    {
                        prevPage = CurPage;
                        showPrevPage = false;
                    }
                }
                return showPrevPage;
            }
        }

        /// <summary>
        /// 下一页
        /// </summary>
        private int NextPage
        {
            get { return nextPage; }
        }

        /// <summary>
        /// 下一页显示标志
        /// </summary>
        private bool ShowNextPage
        {
            get
            {
                if (nextPage < 0)
                {
                    if (CurPage < PageNumbers)
                    {
                        nextPage = CurPage + 1;
                        showNextPage = true;
                    }
                    else
                    {
                        nextPage = CurPage;
                        showNextPage = false;
                    }
                }
                return showNextPage;
            }
        }

        /// <summary>
        /// 总页数
        /// </summary>
        private int PageNumbers
        {
            get
            {
                if (pageNumbers < 0)
                {
                    if (TotalRecords % PageSize != 0)
                    {
                        pageNumbers = TotalRecords / PageSize + 1;
                    }
                    else
                    {
                        pageNumbers = TotalRecords / PageSize;
                    }

                    pageNumbers = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(TotalRecords) / Convert.ToDouble(PageSize)));
                }
                return pageNumbers;
            }
        }

        /// <summary>
        /// 当前记录
        /// </summary>
        private int CurRecord
        {
            get
            {
                if (curRecord < 0) curRecord = PageSize * (CurPage - 1);
                return curRecord;
            }
        }
        #endregion

        #region [options]内部选项实体
        /// <summary>
        /// 内部选项实体
        /// </summary>
        private class options
        {
            public int current_page;
            public int items_per_page;
            public int maxentries;
            public int num_display_entries;
            public int num_edge_entries;
            public string link_to = "#";
            public string prev_text = "上一页";
            public string next_text = "下一页";
            public string ellipse_text = "...";
            public bool prev_show_always = true;
            public bool next_show_always = true;
            public bool load_first_page = false;

            public string url = "";
            public string anchor = "";
            public string text = "";
            public string classes = "";
        }
        private options opts;
        #endregion

        #region [getInterval]计算开始和结束点的分页,当前页码数和要显示的页码
        /// <summary>
        /// 计算开始和结束点的分页,当前页码数和要显示的页码
        /// </summary>
        /// <param name="current_page">当前页</param>
        /// <param name="start">起始显示页</param>
        /// <param name="end">末尾显示页</param>
        private void getInterval(int current_page, out int start, out int end)
        {
            var ne_half = Convert.ToInt32(Math.Floor(Convert.ToDouble(this.DisplayNumberCount) / 2));
            var np = this.PageNumbers;
            var upper_limit = np - this.DisplayNumberCount;

            start = current_page > ne_half ?
              Math.Max(Math.Min(current_page - ne_half, upper_limit), 1) : 1;

            end = current_page > ne_half ?
              Math.Min(current_page + ne_half + (this.DisplayNumberCount % 2), np) :
              Math.Min(this.DisplayNumberCount, np);
        }
        #endregion

        #region [createLink]生成一个单独的链接
        /// <summary>
        /// 生成一个单独的链接
        /// </summary>
        /// <param name="page_id">新页的页码</param>
        /// <param name="current_page">当前页页码</param>
        /// <param name="appendopts">新页的options：文本和类</param>
        /// <returns></returns>
        private string createLink(int page_id, int current_page, options appendopts)
        {
            string lnk = "";
            int np = this.PageNumbers;

            page_id = page_id < 1 ? 1 : (page_id <= np ? page_id : np);

            //获取页码
            if (!appendopts.classes.Equals("pageprv") && !appendopts.classes.Equals("pagenxt"))
            {
                appendopts.text = Convert.ToString(page_id);
            }

            //判断是否为当前页
            if (page_id == current_page)
            {
                if (appendopts.classes.Equals("pageprv") || appendopts.classes.Equals("pagenxt"))
                {
                    lnk = string.Format("<a class='z-dis {{0}}'>{0}</a>", appendopts.text);
                }
                else
                {
                    lnk = string.Format("<a class='z-crt {{0}}'>{0}</a>", appendopts.text);
                }
            }
            else
            {
                lnk = string.Format("<a class='{{0}}' href='{1}{2}#{3}'>{0}</a>",
                    appendopts.text, this.Url, page_id, this.Anchor);
            }

            //附加样式
            lnk = string.Format(lnk, appendopts.classes);


            return lnk;
        }
        #endregion

        #region [appendRange]生成数字范围内的页码链接
        /// <summary>
        /// 生成数字范围内的页码链接
        /// </summary>
        private void appendRange(StringBuilder container, int current_page, int start, int end, options opts)
        {
            for (var i = start; i <= end; i++)
            {
                container.AppendLine(this.createLink(i, current_page, opts));
            }
        }
        #endregion

        #region [getLinks]生成链接字符串
        /// <summary>
        /// 生成链接字符串
        /// </summary>
        /// <returns></returns>
        private string getLinks(int current_page)
        {
            int iStart, iEnd, iOutputBegin, iOutputEnd, np;
            StringBuilder fragment = new StringBuilder();

            getInterval(current_page, out iStart, out iEnd);
            var interval = new { start = iStart, end = iEnd };

            np = this.PageNumbers;

            // 生成“上一页”的链接
            if (current_page > 1 || this.opts.prev_show_always)
            {
                fragment.AppendLine(createLink(current_page - 1, current_page, new options { text = this.opts.prev_text, classes = "pageprv" }));
            }

            // 起始边界项按钮
            if (interval.start > 1 && this.opts.num_edge_entries > 0)
            {
                iOutputEnd = Math.Min(this.opts.num_edge_entries, interval.start - 1);
                appendRange(fragment, current_page, 1, iOutputEnd, new options { classes = "sp" });
                if (this.opts.num_edge_entries < interval.start - 1)
                {
                    fragment.AppendLine("<i>...</i>");
                }
            }

            // 链接之间的间隔
            appendRange(fragment, current_page, interval.start, interval.end, opts);

            // 末尾边界项按钮
            if (interval.end < np && this.opts.num_edge_entries > 0)
            {
                if (np - this.opts.num_edge_entries > interval.end)
                {
                    fragment.AppendLine("<i>...</i>");
                }
                iOutputBegin = Math.Max(np - this.opts.num_edge_entries + 1, interval.end + 1);
                appendRange(fragment, current_page, iOutputBegin, np, new options { classes = "sp" });
            }

            // “下一页”链接
            if (current_page < np - 1 || this.opts.next_show_always)
            {
                fragment.AppendLine(createLink(current_page + 1, current_page, new options { text = this.opts.next_text, classes = "pagenxt" }));
            }

            return fragment.ToString();
        }
        #endregion

        #region [GetHtmlResult]输出带边界分页数据
        public string GetHtmlResult(string Url = "", int DisplayEdgeCount = 3)
        {

            string strParam = "", strQueryString = HttpContext.Current.Request.QueryString.ToString();

            //设置初始化跳转链接
            if (string.IsNullOrEmpty(Url))
            {

                //锚点获取
                string[] arrAnchor = strQueryString.Split('#');
                if (arrAnchor.Length > 1)
                {
                    this.Anchor = arrAnchor[1];
                }

                //查询字符串获取
                string[] arrQueryString = arrAnchor[0].Split('&');
                if (arrQueryString.Length > 0) for (int i = 0; i < arrQueryString.Length; i++)
                    {
                        //截取当前参数名
                        string[] arrCurParam = arrQueryString[i].Split('=');

                        //过滤page和空置参数
                        if (arrCurParam[0].Equals("page") || string.IsNullOrEmpty(arrCurParam[0]) || arrCurParam.Length < 2) continue;

                        if (string.IsNullOrEmpty(strParam))
                        {
                            strParam = string.Format("?{0}={1}", arrCurParam[0], arrCurParam[1]);
                        }
                        else
                        {
                            strParam += string.Format("&{0}={1}", arrCurParam[0], arrCurParam[1]);
                        }
                    }

                if (string.IsNullOrEmpty(strParam))
                {
                    this.Url = string.Format("?page=");
                }
                else
                {
                    this.Url = string.Format("{0}&page=", strParam);
                }
            }

            //显示边界按钮数
            this.DisplayEdgeCount = DisplayEdgeCount;

            opts = new options
            {
                current_page = CurPage,
                items_per_page = PageSize,
                maxentries = TotalRecords,
                num_display_entries = DisplayNumberCount,
                num_edge_entries = DisplayEdgeCount,
            };

            var current_page = opts.current_page;
            //var maxentries = opts.maxentries;
            //var np = this.PageNumbers;

            var links = "";
            if (TotalRecords > 0 && TotalRecords > PageSize)
            {
                links = getLinks(current_page);
            }

            return links;
        }
        #endregion

        #region [GetHtml]输出分页数据
        /// <summary>
        /// 输出分页数据
        /// </summary>
        /// <param name="Url">链接前置参数</param>
        /// <returns></returns>
        public string GetHtml(string Url = "")
        {

            //获取当前页QueryString参数
            string strParam = "", strAnchor = "",
                strQueryString = HttpContext.Current.Request.QueryString.ToString();

            //设置初始化跳转链接
            if (string.IsNullOrEmpty(Url))
            {

                //锚点获取
                string[] arrAnchor = strQueryString.Split('#');
                if (arrAnchor.Length > 1)
                    strAnchor = arrAnchor[1];
                else
                    strAnchor = Anchor;

                //查询字符串获取
                string[] arrQueryString = arrAnchor[0].Split('&');
                if (arrQueryString.Length > 0) for (int i = 0; i < arrQueryString.Length; i++)
                    {
                        //截取当前参数名
                        string[] arrCurParam = arrQueryString[i].Split('=');

                        //过滤page和空置参数
                        if (arrCurParam[0].Equals("page") || string.IsNullOrEmpty(arrCurParam[0]) || arrCurParam.Length < 2) continue;

                        if (string.IsNullOrEmpty(strParam))
                        {
                            strParam = string.Format("?{0}={1}", arrCurParam[0], arrCurParam[1]);
                        }
                        else
                        {
                            strParam += string.Format("&{0}={1}", arrCurParam[0], arrCurParam[1]);
                        }
                    }

                if (string.IsNullOrEmpty(strParam))
                {
                    Url = string.Format("?page=");
                }
                else
                {
                    Url = string.Format("{0}&page=", strParam);
                }

                //if (string.IsNullOrEmpty(strAnchor)) Url = string.Format("{0}#{1}", Url, strAnchor);
            }

            //设置上一页下一页跳转按钮
            string strPrevPage = "", strNextPage = "";
            strPrevPage = ShowPrevPage ? string.Format("<a class=\"pageprv\" href='{0}{1}#{2}'>上一页</a>\n", Url, PrevPage, strAnchor) : DisplayEdgeSpace ? "<a class=\"pageprv z-dis\">上一页</a>\n" : "";
            strNextPage = ShowNextPage ? string.Format("<a class=\"pagenxt\" href='{0}{1}#{2}'>下一页</a>\n", Url, NextPage, strAnchor) : DisplayEdgeSpace ? "<a class=\"pagenxt z-dis\">下一页</a>\n" : "";

            //处理页码按钮顺序
            int FirstNum;
            if (CurPage % DisplayNumberCount == 0)
            {
                FirstNum = (DisplayNumberCount * (CurPage / DisplayNumberCount - 1)) + 1;
            }
            else
            {
                FirstNum = (DisplayNumberCount * (CurPage / DisplayNumberCount)) + 1;
            }

            StringBuilder ExportNumbersList = new StringBuilder();
            for (int i = FirstNum; i < FirstNum + DisplayNumberCount; i++)
            {
                string strPageNum = i.ToString();

                if (CurPage.Equals(i))
                {
                    strPageNum = "<string>" + strPageNum + "</string>";
                    ExportNumbersList.AppendFormat("<a class=\"z-crt\">{2}</a>\n", Url, i, strPageNum);
                }
                else
                {
                    ExportNumbersList.AppendFormat("<a href='{0}{1}#{2}'>{3}</a>\n", Url, i, strAnchor, strPageNum);
                }

                if (i >= PageNumbers) break;
            }

            return string.Format("{0}{1}{2}", strPrevPage, ExportNumbersList.ToString(), strNextPage);
        }
        #endregion
    }
}