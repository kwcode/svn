<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="demo2.aspx.cs" Inherits="LazyLoad.demo2" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <style>
        #div1 a {
            width: 400px;
            height: 400px;
            border: 1px solid red;
            margin: 20px;
            display: block;
            padding: 5px;
        }

        img {
            width: 400px;
            height: 400px;
        }

            img a, img {
                border: 0px;
            }

        div {
            margin-bottom: 10px;
        }
    </style>
    <script src="js/jquery-1.7.1.min.js"></script>
    <script src="js/jquery.lazyload.js"></script>
    <script>
        $(function () {
            $("img.lazy").lazyload({
                placeholder: "/img/loading.gif"
            });
        });

    </script>

</head>
<body>
    <form id="form1" runat="server">
        <div id="div1">
            <%if (imglist != null)
              {
                  foreach (string item in imglist)
                  {
            %>
            <div style="float: left;">
                <a href="#">
                    <img class="lazy item-img" src="<%=item%>" data-original="images/1.jpg" /></a>
            </div>

            <%
                  }
              } %>
        </div>
    </form>
</body>
</html>
