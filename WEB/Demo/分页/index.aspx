<%@ Page Language="C#" AutoEventWireup="true" CodeFile="index.aspx.cs" Inherits="index" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <script src="js/jquery-1.8.3.js"></script>
    <style>
        /*a { margin: 0px; padding: 0px; }
        .m-page a.z-dis, .m-page a.z-dis:hover, .m-page a.z-dis:active { cursor: default; color: #ccc; background: #fff; }
        .m-page a, .m-page a:hover { text-decoration: none; color: #39c; }
        .m-page a:first-child { margin-left: 0; border-top-left-radius: 5px; border-bottom-left-radius: 5px; }
        .m-page a { border: 1px solid #ddd; background: #fff; padding: 0 12px; }*/



        /* 普通翻页器-默认居中 */
        .m-page { margin: 10px 0 0; text-align: center; line-height: 32px; font-size: 0; letter-spacing: -0.307em; word-wrap: normal; white-space: nowrap; color: #999; }
        .m-page a, .m-page i { display: inline-block; vertical-align: top; padding: 0 12px; margin-left: -1px; border: 1px solid #ddd; font-size: 12px; letter-spacing: normal; text-shadow: 0 1px #fff; background: #fff; -webkit-transition: background-color 0.3s; -moz-transition: background-color 0.3s; -ms-transition: background-color 0.3s; transition: background-color 0.3s; }
        .m-page a, .m-page a:hover { text-decoration: none; color: #39c; }
        .m-page a:first-child { margin-left: 0; border-top-left-radius: 5px; border-bottom-left-radius: 5px; }
        .m-page a:last-child { margin-right: 0; border-top-right-radius: 5px; border-bottom-right-radius: 5px; }
        .m-page a.pageprv:before, .m-page a.pagenxt:after { font-weight: bold; font-family: \5b8b\4f53; vertical-align: top; }
        .m-page a.pageprv:before { margin-right: 3px; content: '\3C'; }
        .m-page a.pagenxt:after { margin-left: 3px; content: '\3E'; }
        .m-page a:hover { background: #f5f5f5; }
        .m-page a:active { background: #f0f0f0; }
        .m-page a.z-crt, .m-page a.z-crt:hover, .m-page a.z-crt:active { cursor: default; color: #999; background: #f5f5f5; }
        .m-page a.z-dis, .m-page a.z-dis:hover, .m-page a.z-dis:active { cursor: default; color: #ccc; background: #fff; }

        /* 居左 */
        .m-page-lt { text-align: left; }
        /* 居右 */
        .m-page-rt { text-align: right; }
        /* 较小 */
        .m-page-sm { line-height: 22px; }
        .m-page-sm a, .m-page-sm i { padding: 0 8px; }
        /* 分离 */
        .m-page-sr a, .m-page-sr i { margin: 0 3px; border-radius: 5px; }
        .m-page-sr i { border: 0; }
        .m-page-sr a:first-child, .m-page-sr a:last-child { border-radius: 5px; }


        /* 简易数据表格-格边框 */
        .m-table { table-layout: fixed; width: auto; line-height: normal; border-spacing: 0; }
        .m-table th, .m-table td { padding: 6px; border: 1px solid #ddd; line-height: 18px; }
        .m-table th { font-weight: bold; text-align: center; }
        .m-table tbody tr:nth-child(2n) { background: #fafafa; }
        .m-table tbody tr:hover { background: #f0f0f0; }
        .m-table .cola { width: auto; }
        .m-table .colb { width: auto; }
        /* 简易数据表格-行边框*/
        .m-table-row th, .m-table-row td { border-width: 0 0 1px; }
        /* 简易数据表格-圆角*/
        .m-table-rds { border-collapse: separate; border: 1px solid #ddd; border-width: 0 1px 1px 0; border-radius: 5px; }
        .m-table-rds th, .m-table-rds td { border-width: 1px 0 0 1px; }
        .m-table-rds > :first-child > :first-child > :first-child { border-top-left-radius: 5px; }
        .m-table-rds > :first-child > :first-child > :last-child { border-top-right-radius: 5px; }
        .m-table-rds > :last-child > :last-child > :first-child { border-bottom-left-radius: 5px; }
        .m-table-rds > :last-child > :last-child > :last-child { border-bottom-right-radius: 5px; }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="divMainComment">
        </div>
        <div class="page" id="divPage" data-total="10000" data-size="100"></div>
        <%--   <div>
            <div class="page m-page">
                <a class="pageprv z-dis" data-p="1" href="#page=1">上一页</a>
                <a data-p="1" class="z-crt" href="#page=1">1</a>
                <a data-p="2" href="#page=2">2</a>
                <a data-p="3" href="#page=3">3</a>
                <a data-p="4" href="#page=4">4</a>
                <a data-p="5" href="#page=5">5</a>
                <a class="pagenxt" href="#page=2">下一页</a>
            </div>
        </div>--%>
        <script>
            $(function () {
                pageInit();
                loadDataPage(1);
                $(".page a").click(function () {
                    var p = $(this).data().p;
                    //设置选中
                    $(".page").find("a").removeClass("z-crt").removeClass("z-dis");
                    var sum = $(".page").find("a").index();
                    var index = $(".page").find("a").index($(this)) - 1;
                    if (index >= 0) {

                        $(".page").find('a.z-item:eq(' + index + ')').addClass("z-crt");
                        if (index == 0) {
                            $(".page").find("a.pageprv").addClass("z-dis")
                        }
                        if (index + 2 == sum) {
                            $(".page").find("a.pagenxt").addClass("z-dis")
                        }
                    }

                    //alert(index + " " + sum);
                    loadDataPage(p);
                });
            });

            //加载数据到页面
            var pageInit = function () {
                var $page = $(".page");
                $page.addClass("m-page");
                var html = setpage(1, $page.data().size, $page.data().total);
                $page.html(html);
            }
            var setpage = function (pageNow, pageSize, total) {
                var html = [];
                var totalPage = parseInt((total / pageSize) + 1);
                if (totalPage > 1) {
                    html.push(getLink(pageNow - 1, '上一页', total, 1));
                    for (var i = 1; i <= totalPage; i++) {
                        html.push(getLink(i, i, totalPage, 2));
                    }
                    html.push(getLink(pageNow + 1, '下一页', totalPage, 3));
                }
                return html.join('');;
            }
            var getLink = function (p, text, totalPage, pagetype) {
                var html = "";
                if (pagetype == 1) {
                    html += '<a class="pageprv z-dis" data-p="' + p + '" href="#page=' + p + '">' + text + '</a>';
                }
                else if (pagetype == 3) { html += '<a class="pagenxt" data-p="' + p + '"  href="#page=' + totalPage + '">' + text + '</a>'; }
                else {
                    if (p == 1) {
                        html += '  <a class="z-crt z-item" data-p="' + p + '" href="#page=' + p + '">' + text + '</a>';
                    } else {
                        html += '  <a class="z-item" data-p="' + p + '" href="#page=' + p + '">' + text + '</a>';
                    }
                }
                return html;
            }
            var loadDataPage = function (pageNow) {
                $.ajax("ActionPage.aspx", {
                    data: {
                        action: "list",
                        pageNow: pageNow,
                        pageSize: 100
                    }, dataType: 'json', type: 'GET'
                }).success(function (result) {
                    loadDataContent(result.dataList);
                });
            }
            var loadDataContent = function (data) {
                var html = ' <table class="m-table"> <thead><tr> <th class="cola">ID</th> <th class="cola">Name</th> <th class="cola">Time</th> <th class="cola">Desc</th> </tr></thead <tbody>';

                for (var i = 0; i < data.length; i++) {
                    html += ' <tr class="j-item"> <td>' + data[i].ID + '</td><td>' + data[i].Name + '</td> <td>' + data[i].Time + '</td> <td>' + data[i].Desc + '</td> </tr>';
                }
                html += '</tbody> </table>';
                $(".divMainComment").html(html);
            }
        </script>
    </form>
</body>
</html>
