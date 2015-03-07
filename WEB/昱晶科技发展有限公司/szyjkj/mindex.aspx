<%@ Page Language="C#" AutoEventWireup="true" CodeFile="mindex.aspx.cs" Inherits="mindex" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>UMEDITOR 完整demo</title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <script src="js/jquery-1.8.3.js" type="text/javascript"></script>
    <link href="meditor/themes/default/css/umeditor.css" type="text/css" rel="stylesheet">
    <script type="text/javascript" src="meditor/third-party/jquery.min.js"></script>
    <script type="text/javascript" charset="utf-8" src="meditor/umeditor.config.js"></script>
    <script type="text/javascript" charset="utf-8" src="meditor/umeditor.min.js"></script>
    <script type="text/javascript" src="meditor/lang/zh-cn/zh-cn.js"></script>
    <link href="style/css.css" rel="stylesheet" />
    <style>
        #txt_msg { width: 1024px; height: 150px; border: 1px solid #eecc95; padding: 3px 0 0 2px; color: red; font-size: 14px; }
        #txt_Content { height: 40px; line-height: 40px; border: 1px solid #eecc95; padding: 3px 0 0 2px; color: #333; width: 1024px; }
        .a_btn_1 { background-color: #e99255; width: auto; height: auto; color: white; outline: medium; border-radius: 3px; padding: 5px; margin: 5px; cursor: pointer; }
        .a_btn_1:hover { text-decoration: none; background-color: #b75d5d; }
        .a_btn_1:active { border: thin solid #3366FF; text-decoration: none; background-color: #b75d5d; }
    </style>
</head>
<body onload="Init()">
    <form id="form1" runat="server">
    <div style="margin-top: 10px;">
        <h1 style="color: red">
            完整demo</h1>
        <script id="editor" type="text/plain" style="width: 1024px; height: 300px;"></script>
    </div>
    <div style="margin-top: 10px;">
        <input type="text" id="txt_Content" value="<a style='color:red'>测试数据<a/>" />
    </div>
    <div style="margin-top: 10px;">
        <div>
            <a class="a_btn_1" onclick="Init()">初始化加载数据</a> <a class="a_btn_1" onclick="DelEditor()">
                删除编辑器</a> <a class="a_btn_1" onclick="SetCmd('setContent')">加载HTML文本</a> <a class="a_btn_1"
                    onclick="SetCmd('setContent',true)">追加文本</a> <a class="a_btn_1" onclick="SetCmd('insertHtml')">
                        插入给定的内容</a>
        </div>
        <div style="margin-top: 10px;">
            <a class="a_btn_1" onclick="GetCmd('getContentTxt')">获取纯文本</a> <a class="a_btn_1"
                onclick="GetCmd('getContent')">获取HTML文本</a> <a class="a_btn_1" onclick="GetCmd('getPlainTxt')">
                    带有格式的文本</a> <a class="a_btn_1" onclick="GetCmd('getAllHtml')">获取所有的Html</a>
            <a class="a_btn_1" onclick="GetCmd('hasContents')">判断是否为空</a> <a class="a_btn_1"
                onclick="GetCmd('getText')">获取当前选中内容</a>
        </div>
        <div style="margin-top: 10px;">
            <a class="a_btn_1" onclick="SetEditor('setEnabled')">可以编辑</a> <a class="a_btn_1"
                onclick="SetEditor('setDisabled')">不可编辑</a> <a class="a_btn_1" onclick="SetEditor('setHide')">
                    隐藏编辑器</a> <a class="a_btn_1" onclick="SetEditor('setShow')">显示编辑器</a>
            <%--  <a class="a_btn_1" onclick="SetEditor('setHeight',300)">设置高度为300默认关闭了自动长高</a>--%>
            <a class="a_btn_1" onclick="SetEditor('focus')">使编辑器获得焦点</a> <a class="a_btn_1" onclick="SetEditor('isFocus',event)">
                编辑器是否获得焦点</a> <a class="a_btn_1" onclick="SetEditor('blur',event)">编辑器失去焦点</a>
            <a class="a_btn_1" onclick="SetEditor('getlocaldata')">获取草稿</a> <a class="a_btn_1"
                onclick="SetEditor('clearlocaldata')">清空草稿</a>
        </div>
    </div>
    <div style="margin-top: 10px;">
        <textarea class="textarea" id="txt_msg"></textarea>
    </div>
    <script type="text/javascript">
        var ue;
        //测试数据
        function GetSomeData() {
            return document.getElementById("txt_Content").value;
        }
        //
        function SetSomeText(txt) {
            var data = new Date();
            data = data.toLocaleString();
            document.getElementById("txt_msg").value = data + ":" + txt;
        }
        function Init() {
            ue = UM.getEditor('editor'); //实例化编辑器 
        }
        function DelEditor() {
            ue.destroy();
        }

        function SetCmd(cmd, isAppendTo) {
            var val = GetSomeData();
            switch (cmd) {
                case "setContent":  //设置内容 isAppendTo是否追加
                    ue.setContent(val, isAppendTo);
                    break
                case "insertHtml":  //插入给定的内容
                    ue.execCommand('insertHtml', val);
                    break
                default:
            }
        }

        function GetCmd(cmd) {
            var val = "获取失败！";
            switch (cmd) {
                case "getContentTxt":  //纯文本
                    val = ue.getContentTxt();
                    break
                case "getAllHtml":  //所有的HTML
                    val = ue.getAllHtml();
                    break
                case "getContent":  //所有的HTML文本
                    val = ue.getContent();
                    break
                case "getPlainTxt":  //带有格式的文本
                    val = ue.getPlainTxt();
                    break
                case "hasContents":  //判断是否有内容
                    val = ue.hasContents();
                    break
                case "getText":  //获得当前选中的文本
                    //当你点击按钮时编辑区域已经失去了焦点，如果直接用getText将不会得到内容，所以要在选回来，然后取得内容
                    var range = ue.selection.getRange();
                    range.select();
                    val = ue.selection.getText();
                    break

            }
            SetSomeText(val);
        }
        function SetEditor(cmd, e) {
            var val = "获取失败！";
            switch (cmd) {
                case "focus":  //使编辑器获得焦点
                    val = ue.focus();
                    UE.dom.domUtils.preventDefault(e)
                    break
                case "isFocus":  //判断是否有焦点
                    val = ue.isFocus();
                    UE.dom.domUtils.preventDefault(e)
                    break
                case "blur":  //使编辑器获得焦点
                    val = ue.blur();
                    UE.dom.domUtils.preventDefault(e)
                    break
                case "getlocaldata":  //获取草稿
                    val = ue.execCommand("getlocaldata");
                    break
                case "clearlocaldata":  //已清空草稿箱
                    ue.execCommand("clearlocaldata");
                    val = "已清空草稿箱";
                    break
                case "setEnabled":  //可以编辑
                    ue.setEnabled();
                    val = "可以编辑";
                    break
                case "setDisabled":  //不可编辑
                    ue.setDisabled();
                    val = "不可编辑";
                    break
                case "setHide":  //隐藏编辑器
                    ue.setHide();
                    val = "隐藏编辑器";
                    break
                case "setShow":  //显示编辑器
                    ue.setShow();
                    val = "显示编辑器";
                    break
                case "setHeight":  //设置高度为300默认关闭了自动长高
                    ue.setHeight(e);
                    val = "显示编辑器";
                    break
                default:
            }
            SetSomeText(val);
        }

    </script>
    </form>
</body>
</html>
