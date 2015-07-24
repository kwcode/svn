<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PayOrderManager.aspx.cs" Inherits="admin_PayManager_PayOrderManager" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="/admin/css/pm.css" rel="stylesheet" />

    <link href="/admin/css/button.css" rel="stylesheet" />
    <link href="/admin/css/Pager.css" rel="stylesheet" />
    <link href="/style/tripbutton.css" rel="stylesheet" />
    <link href="/style/indexcss.css" rel="stylesheet" />
    <script src="/js/jquery-1.8.3.min.js"></script>
    <script src="/js/jquery.layer.min.js"></script>
    <script src="/js/jqueryExt.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="g-div-e">
            <div>
                <span>支付号：</span>
                <input type="text" id="txt_bilnum" runat="server" />
                <span>订单号：</span>
                <input type="text" id="txt_ordernum" runat="server" />
                <span>用户ID：</span>
                <input type="text" id="txt_userid" runat="server" />
                <span>关键字：</span>
                <input type="text" id="txt_keywords" runat="server" />
                <span>支付状态：</span>
                <select runat="server" id="sel_status">
                    <option value="-1">所有</option>
                    <option value="0">待支付</option>
                    <option value="1">已支付</option>
                    <option value="4">已退款</option>
                    <option value="2">交易关闭</option>
                </select>
                <span>支付方式：</span>
                <select runat="server" id="sel_paytype">
                    <option value="-2">所有</option>
                    <option value="-1">待选择</option>
                    <option value="0">驼铃支付</option>
                    <option value="1">财付通</option>
                    <option value="2">支付宝</option>
                </select>
                <span>类型：</span>
                <select runat="server" id="sel_type">
                    <option value="-1">所有</option>
                    <option value="0">充值</option>
                    <option value="1">驼铃购物</option>
                    <option value="2">提现</option>
                    <option value="3">驼铃奖励</option>
                    <option value="11">保险</option>
                    <option value="22">租车</option>
                    <option value="33">活动</option>
                </select>

                <asp:LinkButton ID="btn_search" runat="server" CssClass="inpbbut2" OnClick="btn_search_Click">查询</asp:LinkButton>

                <a href="/admin/PayManager/PayOrderManager.aspx" class="inpbbut2">清空条件</a>
            </div>
            <table class="m-table">
                <thead>
                    <tr>
                        <th style="width: 90px;">支付号</th>
                        <th style="width: 90px;">订单号</th>
                        <th style="width: 80px;">用户ID</th>
                        <th style="width: 80px;">姓名</th>
                        <th style="width: 80px;">金额</th>
                        <th style="width: 80px;">状态</th>
                        <th style="width: 80px;">支付方式</th>
                        <th style="width: 80px;">类型</th>
                        <th style="width: 200px;">标题</th>
                        <th style="width: 200px;">备注</th>
                    </tr>
                </thead>
                <%if (dtOrder != null && dtOrder.Rows.Count > 0)
                  {
                      foreach (System.Data.DataRow item in dtOrder.Rows)
                      {
                %>
                <tr class="j-item">

                    <td><%=item["BillNum"] %></td>
                    <td><%=item["OrderNum"] %></td>
                    <td><%=item["UserID"] %></td>
                    <td><%=item["RealName"] %></td>
                    <td><%=item["Money"] %></td>
                    <td><%=ConvertToStatus(Convert.ToInt32(item["Status"]))%></td>
                    <td><%=ConvertToPayType(Convert.ToInt32(item["PayType"])) %></td>
                    <td><%=ConvertToType(Convert.ToInt32( item["Type"])) %></td>
                    <td title="<%=item["Title"]%>"><%=item["Title"].ToString().Length>25?item["Title"].ToString().Substring(0,25):item["Title"]%></td>
                    <td title="<%=item["Remark"]%>"><%=item["Remark"].ToString().Length>25?item["Title"].ToString().Substring(0,25):item["Remark"]%></td>
                   
                </tr>
                <%
                      }
                  }
                  else
                  {
                %><tr class="j-item">
                    <td colspan="12">暂无数据</td>
                </tr>
                <%
                  } %>
            </table>
            <div class="page" id="divPage" style="float: left" data-total="<%=TotalCount %>" data-size="<%=PageSize%>"></div>

        </div>
    </form>
</body>
</html>
