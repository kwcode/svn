<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AlbumPopup.aspx.cs" Inherits="admin_photo_AlbumPopup" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>相册上传</title>
    <link href="/style/icon.css" rel="stylesheet" />
    <link href="/style/easyui.css" rel="stylesheet" />
    <link href="/admin/style/admin.css?v=12" rel="stylesheet" />
    <script src="/js/jquery-1.8.3.min.js"></script>
    <script src="/js/jquery.easyui.min.js"></script>
    <link href="/style/layer.css" rel="stylesheet" />
    <script src="/js/layer.js"></script>
    <script src="/js/jquery-twExt.js"></script>
    <link href="/js/uploadify/uploadify.css" rel="stylesheet" />
    <script src="/js/uploadify/jquery.uploadify.js"></script>
    <link href="/style/jquery.validationEngine.css" rel="stylesheet" />
    <script src="/js/jquery.validationEngine-zh_CN.js"></script>
    <script src="/js/jquery.validationEngine.js"></script>
    <script src="/js/jquery.lazyload.min.js"></script>
    <script>
        var bookid = '<%=BookId%>';
        //是否单选图片
        var isSingleSelect = false;

        $(function () {
            //全局ajax参数设置
            //$.ajaxSetup({
            //    url: "/admin/Photo/m_Photos.aspx",
            //    type: "POST",
            //    dataType: "json"
            //});
            function getAlbumPhoto($that) {
                var ajaxData = { bookid: $that, page: 1, rows: 999999 };
                $.ajax("/admin/Photo/AlbumPopup.aspx", { data: ajaxData, type: "POST" }).then(function (data) {
                    //填充相册图片数据
                    if ($(".photo_content").length > 0) {
                        appendAlbumPhoto(data);
                    }
                });
            }
            function appendAlbumPhoto(data) {
                //获取显示列表
                var $list = $(".photo_content");
                //判断空里列表,直接返回
                if (!data.rows || data.rows.length == 0) { return false; }
                for (var i = 0, item; item = data.rows[i++];) {
                    //克隆图片显示模板
                    var $item = $("#photo_template .photo_item").clone();
                    //图片参数赋值
                    $item.children("img").attr({
                        "data-tn": item["Tn"],
                        "data-original": item["Tn"],
                        "data-show": item["Show"],
                        "data-orig": item["Orig"],
                        "data-id": item["ID"],
                    });

                    $item.find(".remove").data("id", item["ID"]);
                    //附加到列表尾部
                    $item.appendTo($list);
                }
                //延迟加载图片
                $list.find("img.lazy").lazyload({ event: "load" });
            }

            function init() {
                var _layer = $.layer({ type: 3 });
                $("#sel_photobook").append('<option value="-1">全部</option>');
                if (typeof (jsonphotobook) != "undefined" && jsonphotobook.length > 0) {
                    for (var i = 0; i < jsonphotobook.length; i++) {
                        $("#sel_photobook").append('<option value="' + jsonphotobook[i].ID + '">' + jsonphotobook[i].Name + '</option>');
                    }
                }
                $("#sel_photobook").val(bookid);
                if ($("#Hide_SingleSelect").val() == "1") {
                    isSingleSelect = true;
                }
                console.log(isSingleSelect);
                if (isSingleSelect) {
                    $(".quanbu").remove();
                }
                //获取相册图片
                getAlbumPhoto(bookid);
                layer.close(_layer);
            }
            init();

            //上传图片
            $("#btn_start").uploadify({
                auto: true, 
                buttonCursor: "hand",
                buttonText: "上传图片",
                swf: '/js/uploadify/uploadify.swf',
                fileTypeExts: "",
                fileSizeLimit: "4194304",
                removeTimeout: "3",
                method: "POST",
                uploader: '/admin/Photo/uploadhandler.aspx?bookid=' + bookid,
                onSelect: function () {
                    var _layerIndex = $.layer({
                        type: 1,
                        title: "上传图片",
                        area: ['400px', '300px'],
                        page: {
                            dom: "#btn_start-queue"
                        },
                        success: function ($layer) {
                        }, close: function () {
                            window.location.reload();
                            layer.closeAll();
                        }
                    });

                },
                onUploadSuccess: function (file, data, response) {
                    window.location.reload();

                },
                OnRemoveTimeout: function () {
                    window.location.reload();
                    layer.closeAll();
                }, onCancel: function () {
                    window.location.reload();
                }

            });
            /*上传图片END*/
            $(document).on("click", ".photo_content .photo_item", function () {
                //单选配置
                if (isSingleSelect) {
                    $(".select").removeClass("select");
                }
                $(this).toggleClass("select");
            })
            $(document).on("click", "#btn_AddPhotoBook", function () {
                var _layerIndex = $.layer({
                    type: 1,
                    title: '增加相册',
                    area: ['400px', '300px'],
                    page: {
                        dom: ".Hide_EditPhotoBook"
                    },
                    success: function ($layer) {
                        $layer.on("click", ".btn_ok", function () {
                            var b = $('#form1').validationEngine('validate');//手动验证
                            if (!b) {
                                return;
                            }
                            var _name = $("#txt_name").val();
                            var _ispublic = $("#cb_ispublic").attr("checked") == "checked" ? 1 : 0;
                            var _showindex = $("#txt_showindex").val();
                            var _remark = $("#txt_remark").val();
                            var _layer = $.layer({ type: 3 });
                            $.post("/admin/Photo/ActionPhoto.aspx", {
                                action: "SavePhotoBook",
                                id: 0,
                                Name: _name,
                                IsPublic: _ispublic,
                                ShowIndex: _showindex,
                                Remark: _remark,
                            }).done(function (result) {
                                if (result.res > 0) {
                                    layer.close(_layer);
                                    layer.alert(result.desc, 1, function () {
                                        window.location.reload();
                                        layer.closeAll();
                                    });
                                }
                                else {
                                    layer.close(_layer);
                                    layer.alert(result.desc);
                                }
                            }).fail(function (ex) {
                                layer.close(_layer); layer.alert("请求失败" + ex.responseText);
                            });
                        });
                    }
                });
            });
            $(document).on("click", "#cb_all", function () {
                var ck = $(this).attr("checked") == "checked" ? 1 : 0;
                if (ck == 1)
                    $(".photo_content .photo_item").addClass("select");
                else
                    $(".photo_content .photo_item").removeClass("select");
            });
            $(document).on("change", "#sel_photobook", function () {
                var id = $(this).val();
                window.location.href = "/admin/Photo/AlbumPopup.aspx?bookid=" + id;
            });
            $(document).on("click", "#btn_AlbumPopupOk", function () {
                var returnData = { msg: "", tabid: 0, result: [] };
                //获取选中值
                $(".photo_box .photo_item").each(function () {
                    if ($(this).hasClass("select")) {
                        var itemval = $(this).children("img").data();
                        delete itemval.orig;
                        returnData.result.push(itemval);
                    }
                });
                //第1种:传递返回对象     
                if (parent.tw && parent.tw.AlbumPhotoPopup) {
                    //尝试执行回调函数
                    parent.tw.AlbumPhotoPopup.confirm(returnData);
                }
                //关闭弹出窗
                if (parent.layer) {
                    var index = 0;
                    if (parent.layer.getFrameIndex)
                        index = parent.layer.getFrameIndex();
                    parent.layer.close(index);
                }
            });

        });

    </script>
    <style>
        .photo_box { border: 1px solid #CCCCCC; }
        .photo_content { width: 100%; height: 250px; overflow-y: auto; overFlow-x: hidden; }
        .photo_item { position: relative; float: left; padding: 2px; width: 135px; height: 135px; border: 1px solid #97cce8; margin: 2px; }
        .photo_item:hover .operation { display: block; }
        .photo_content .select { border: 1px solid #085ef8; }
        .photo_content .select .gou { border: 1px solid #4cff00; display: block; position: absolute; margin: 0; padding: 0; top: 3px; left: 103px; width: 30px; height: 30px; background: url('/images/eui/icons/ok.png') no-repeat center center; }
        .photo_item img { max-width: 135px; height: 135px; }
        .photo_content .operation { position: absolute; left: 0; top: 107px; z-index: 5; width: 139px; height: 32px; line-height: 32px; text-align: right; filter: alpha(opacity=70); -moz-opacity: .7; -khtml-opacity: .7; opacity: .7; _filter: alpha(opacity=100); background: #000; display: none; }
        .photo_content .photo_item .remove { margin-right: 6px; background: url('/images/eui/icons/no.png') no-repeat center center; overflow: hidden; display: inline-block; font-size: 0; letter-spacing: 0; -webkit-text-size-adjust: none; vertical-align: text-bottom; width: 14px; height: 14px; cursor: pointer; }
        .photo_btnbox { padding: 10px; }
        select { width: 200px; line-height: 26px; padding: 3px; }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <input id="Hide_SingleSelect" type="hidden" value="<%=SingleSelect%>" />
        <div class="Hide_EditPhotoBook hide_box">
            <div>
                <ul>
                    <li><span>相册名称：</span>
                        <input type="text" class="validate[required]" id="txt_name" maxlength="30" />
                    </li>
                    <li><span>是否私有:</span>
                        <input type="checkbox" id="cb_ispublic" />
                    </li>
                    <li>
                        <span>排序：</span>
                        <input class="validate[required] validate[custom[integer]]" onkeyup="this.value=this.value.replace(/\D/g,'');" onblur="if(this.value==''||this.value==0)this.value=1" type="text" id="txt_showindex" maxlength="8" value="0" />
                    </li>
                    <li>
                        <span>相册描述：</span>
                        <textarea id="txt_remark" maxlength="200"></textarea>
                    </li>
                </ul>
            </div>
            <div class="btnbox">
                <input class="inpbbut3 btn_ok" value="确定" type="button" />
            </div>
        </div>
        <div>

            <div class="photo_title"></div>
            <div>
                <input type="file" id="btn_start" />
            </div>
            <div class="photo_box">
                <div class="photo_content clearfix">
                    <%-- <%foreach (System.Data.DataRow item in dtPhotos.Rows)
                      {
                    %>
                    <div class="photo_item">
                        <img src="<%=item["Tn"]%>" data-tn="<%=item["Tn"]%>" data-id="<%=item["ID"]%>" data-orig="<%=item["Orig"]%>" data-show="<%=item["Show"]%>" />
                        <span class="gou"></span>
                        <p class="operation">
                            <b class="remove" title="删除">&nbsp;</b>
                        </p>
                    </div>
                    <%
                      } %>--%>
                </div>

                <!-- 相册里图片填充模板 -->
                <div id="photo_template" class="dn" style="display: none;">
                    <div class="photo_item">
                        <img class="lazy" src="/images/img_loading.gif" />
                        <span class="gou"></span>
                        <p class="operation">
                            <b class="remove" title="删除">&nbsp;</b>
                        </p>
                    </div>
                </div>

            </div>
            <div class="photo_btnbox">
                <div style="float: left;">
                    <select id="sel_photobook">
                    </select>
                    <input class="inpbbut3" id="btn_AddPhotoBook" type="button" value="创建新相册" />
                </div>
                <div style="float: right;">
                    <input class="quanbu" type="checkbox" id="cb_all" />
                    <label class="quanbu" for="cb_all">全选</label>
                    <input class="inpbbut3" type="button" id="btn_AlbumPopupOk" value="确定" />
                </div>

            </div>
        </div>
    </form>
</body>
</html>
