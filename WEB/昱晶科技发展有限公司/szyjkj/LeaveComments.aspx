<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/master/m.master" CodeFile="LeaveComments.aspx.cs" Inherits="LeaveComments" %>

<%@ Register Src="~/master/uc/uc_leftmenu.ascx" TagPrefix="uc1" TagName="uc_leftmenu" %>


<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="mbody">

    <script>
        $(function () {
            $.tw.lm = 1;
            $("#btn_LeaveC").click(function () {
                var title = $("#txt_title").val();
                var contacts = $("#txt_contacts").val();
                var content = $("#txt_content").val();
                var LeaveType = $("input[name='LeaveType']:checked").val();
                if (title == "" || title.trim().length == 0) {
                    alert("请输入留言主题");
                    return;
                }
                if (contacts == "" || contacts.trim().length == 0) {
                    alert("请输入联系方式");
                    return;
                }
                if (content == "" || content.trim().length == 0) {
                    alert("请输入留言内容");
                    return;
                }
                $.post("/PC_ActionCommon.aspx",
                    {
                        action: "AddLeaveComments",
                        title: title,
                        contacts: contacts,
                        content: content,
                        type: LeaveType
                    }).done(function (data) {
                        if (data.res > 0) {
                            var datenow = new Date();
                            var time = datenow.getFullYear() + "-" + datenow.getMonth() + "-" + datenow.getDay() + " " + datenow.getHours() + ":" + datenow.getMinutes();
                            var html = '<div class="kw_items"><div class="kw_title"> <span class="rt">' + time + '</span>' + title + '</div>  <div class="kw_info">' + content + '</div></div>';
                            $(".kw_list").prepend(html);
                            alert("留言成功");

                        }
                        else { alert("留言失败"); }
                    });
                console.log(title + " " + contacts + " " + content + " " + LeaveType);
            });
        });
    </script>
    <style>
       
    </style>
    <div class="d-content" style="margin-top: 5px; margin-bottom: 20px;">
        <div style="margin: 0 10px 0 0; float: left;">
            <uc1:uc_leftmenu runat="server" ID="uc_leftmenu" />
        </div>
        <div style="margin-left: 10px; overflow: hidden;">
            <div class="d-nvtitle ">
                <span class="ico"></span>
                <a href="/">首页</a>
                <span class="guai">></span>
                <a href="/lianxi.html">联系我们</a>
                <span class="guai">></span>
                <a href="/news.html">在线留言</a>
            </div>

            <div class="kw_box">
                <dl>
                    <dt><i class="red">*</i>留言主题：</dt>
                    <dd>
                        <input type="text" class="input_450" id="txt_title" /></dd>
                </dl>
                <dl>
                    <dt><i class="red">*</i> 联 系 人：</dt>
                    <dd>
                        <input type="text" class="input_450" id="txt_contacts" /></dd>
                </dl>
                <dl>
                    <dt><i class="red">*</i>内&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;容：</dt>
                    <dd>
                        <textarea style="width: 450px; height: 90px;" id="txt_content"></textarea>
                    </dd>
                </dl>
                <dl>
                    <dt><i class="red">*</i>类&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;型：</dt>
                    <dd style="padding-top: 12px;">
                        <input type="radio" name="LeaveType" id="type_0" checked="checked" value="0" />
                        <label for="type_0">咨询</label>
                        <input type="radio" name="LeaveType" id="type_1" value="1" />
                        <label for="type_1">建议</label>
                        <input type="radio" name="LeaveType" id="type_2" value="2" />
                        <label for="type_2">投诉</label>
                        <input type="radio" name="LeaveType" id="type_99" value="99" />
                        <label for="type_99">其他</label>
                    </dd>
                </dl>
                <div style="margin-left: 150px;">
                    <a class="inpbbut3" id="btn_LeaveC">确定</a>
                    <input type="reset" class="inpbbut3" value="重置" />
                </div>
            </div>


            <%if (dtComments != null && dtComments.Rows.Count > 0)
              {
            %><div class="kw_list">
                <%foreach (System.Data.DataRow item in dtComments.Rows)
                  {
                %>
                <div class="kw_items">
                    <div class="kw_title" style="font-weight: bold;">
                        <i class="kw_face1" style="background: url(/images/face1.gif) no-repeat 0 0; float: left; height: 20px; width: 20px;"></i>
                        <span class="rt"><%=Convert.ToDateTime(item["CreateTS"]).ToString("yyyy-MM-dd HH:mm")%></span>
                        <span title="<%=item["Title"]%>" style="line-height: 23px; margin-left: 3px;"><%=ConvertToLeaveType(Convert.ToInt32(item["LeaveType"]))%><%=item["Title"].ToString().Length>50?item["Title"].ToString().Substring(0,50)+"...":item["Title"]%></span>
                    </div>
                    <div class="kw_info">
                        <%=item["Content"]%>
                    </div>
                </div>
                <%
                  } %>
            </div>
            <%
              }
              else
              {

              } 
            %>
            <div class="page" id="divPage" style="float: left" data-total="<%=TotalCount%>" data-size="<%=PageSize%>"></div>
        </div>
    </div>
</asp:Content>
