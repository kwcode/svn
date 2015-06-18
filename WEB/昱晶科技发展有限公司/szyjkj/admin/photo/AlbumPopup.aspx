<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AlbumPopup.aspx.cs" Inherits="admin_photo_AlbumPopup" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>相册上传</title>
    <script src="/js/jquery-1.8.3.min.js"></script>
    <script src="/js/uploadify/jquery.uploadify.js"></script>
    <link href="/style/uploadify.css" rel="stylesheet" />
    <%--  <script>
        $(function () {
            $("#uploadify").uploadify({
                auto: true,
                width: 1200,//设置浏览按钮的宽度，默认值：110。
                height: 30,//设置浏览按钮的高度，默认值：30。
                //buttonClass:"",//按钮的class
                buttonCursor: "hand",//这光标设置到悬停在浏览按钮时显示 hand arrow
                buttonImage: "",//浏览按图片
                buttonText: "[buttonText]选择",//浏览按钮的文本，默认值：BROWSE。
                // checkExisting: "UploadHandler.aspx",//用于检查文件名是否被上传的文件路径中当前存在的目标文件夹。该脚本应该返回1，如果文件名存在，或者如果文件名不存在。
                debug: "",//设置为true打开SWFUpload的调试模式。
                fileObjName: "",//服务器端脚本使用的文件对象的名称
                fileSizeLimit: "",//允许一个文件上传的最大尺寸。这个值可以是数字或字符串。如果它是一个字符串，它接受一个单位（B，KB，MB，GB或）。默认单位为KB。您可以将该值设置为0，没有限制。
                fileTypeDesc: "",//这个属性值必须设置fileExt属性后才有效，用来设置选择文件对话框中的提示文本，如设置fileDesc为“请选择rar doc pdf文件”，打开文件选择。
                fileTypeExts: "",//可以上传允许扩展名列表。 A的文件名手动输入可以绕过此安全级别，因此您应经常检查的文件类型在您的服务器端脚本。多个扩展应该用分号隔开（即“*.JPG，*。PNG;*.gif要点”）。
                formData: "",//通过GET或POST每个文件上传发送包含附加数据的对象。如果你打算动态地设置这些值，这应该用在onUploadStart事件的“设置”的方法来实现
                itemTemplate: "",//ItemTemplate模板选项允许你指定被添加到队列中的每个项目一个特殊的HTML模板。四个模板标签：•实例ID - 的Uploadify实例•FILEID的ID - 文件的ID添加到队列•文件名 - 添加到队列•文件大小的文件的名称 - 文件的大小增加队列模板标签插入像这样的模板：${文件名}。
                method: "POST",
                multi: false,//设置为true时可以上传多个文件。
                overrideEvents: "",//设置需要覆写的事件名称，格式为['','']
                preventCaching: "",//一个随机数将被加载  swf 文件URL 的后 面，防止浏览器缓存 
                progressData: "",//设置文件上传时显示的数据，有两个选择：‘上传速度‘或者’百分比‘，分别对应’speed’和’percentage’
                queueID: "", //文件队列的ID，该ID与存放文件队列的div的ID一致。
                queueSizeLimit: "",//队列长度限制
                removeCompleted: "",//在上传完成后是否删除队列中的对应元素   即上传完成后就看不到上传文件进度条了。
                removeTimeout: "",// 上传完成后多久删除队列中的进度条 3
                requeueErrors: "",//设置为    true    ，上传过程中因为出错导致上传失败的文件将被重新加入队列 
                successTimeout: "",//文件上传完成后等待服务器响应的时间。    超过该时间，那么将认为上传成功
                swf: '/js/uploadify/uploadify.swf',
                uploader: '/admin/photo/UploadHandler.aspx',
                uploadLimit: "",//最多上传文件数量 999
                onUploadSuccess: function (file, data, response) {
                    console.log(file, data, response);
                    var result = JSON.parse(data);
                    $("#img").attr("src", result.path);
                }
            });
        });
    </script>--%>
    <script>
        $(function () {
            $("#uploadify").uploadify({
                auto: true,
                buttonCursor: "hand",
                buttonText: "选择文件",
                swf: '/js/uploadify/uploadify.swf',
                fileTypeExts: "",
                fileSizeLimit: "4194304",
                removeTimeout: "10",
                method: "POST",
                uploader: '/admin/photo/UploadHandler.aspx',

                // onInit: function () { $.layer({ type: 3 }); },//触发在初始化的尽头时，Uploadify第一次调用。
                //onSelect: function () { $(".uploadify-queue").hide(); },
                onUploadSuccess: function (file, data, response) {
                    $('#' + file.id).find('.data').html(' - 完成');
                    console.log(file, data, response);
                    var result = JSON.parse(data);
                    $("#img").attr("src", result.path);
                    // layer.closeAll();
                }
            });

            //$("#btn_start").click(function () {
            //    $("#uploadify").uploadify("upload");
            //    //layerIndex = $.layer({
            //    //    type: 1,
            //    //    title: false,
            //    //    area: ['200px', '50px'],// 宽度 高度
            //    //    shift: 'left',////从左动画弹出 
            //    //    page: { dom: '.uploadify-queue' }
            //    //});
            //});
        });

    </script>
    <style>
        .photo_content { border: 1px solid #CCCCCC; }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <div id="fileQueue">
            </div>
            <input type="file" name="uploadify" id="uploadify" />
            <div class="photo_title"></div>
            <div class="photo_content"></div>
        </div>
    </form>
</body>
</html>
