<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Demo1.aspx.cs" Inherits="LazyLoad.Demo1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <style>
        img {
            width: 100%;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div id="div1">
            <%if (imglist != null)
              {
                  foreach (string item in imglist)
                  {
            %>
            <p class="_j_note_content _j_seqitem" data-seq="81993477">
                自<a href="/travel-scenic-spot/mafengwo/10168.html" class="link _j_keyword_mdd" data-kw="希腊" target="_blank">希腊</a>行之后，一直后悔没有顺道去一趟隔壁的<a href="/travel-scenic-spot/mafengwo/10053.html" class="link _j_keyword_mdd" data-kw="土耳其" target="_blank">土耳其</a>。时隔2年，终于在今年秋天安排起自己<a href="/travel-scenic-spot/mafengwo/10053.html" class="link _j_keyword_mdd" data-kw="土耳其" target="_blank">土耳其</a>的行程。给这个向往已久的”星月之国“安排了足足18天的行程，选择随行同伴也是几经波折，辗转反复戏剧化的刺激着我的小心脏。但故事的最后一定是——总有那么几个跟你一样的小伙伴，在那里等着你，准备着和你一起去远方。<br>
                <br>
                在路上，遇见美好！<br>
                一切都好像见过，而一切又似乎是新的开始！<a href="/travel-scenic-spot/mafengwo/10053.html" class="link _j_keyword_mdd" data-kw="土耳其" target="_blank">土耳其</a>的文化元素，结合了欧、亚、非三大洲文化的精髓，文化的多元性让我们感受了风情、奇幻和小清新的不同风格。漫步在<a href="/travel-scenic-spot/mafengwo/11228.html" class="link _j_keyword_mdd" data-kw="伊斯坦布尔" target="_blank">伊斯坦布尔</a>的<a href="/poi/5443334.html" class="link _j_keyword_poi" data-poi_id="5443334" data-cs-p="ginfo_txt_poi" data-kw="独立大街" target="_blank">独立大街</a>，叮叮车行驶在街道中央，依偎在加拉塔餐厅独享观景窗口，感受着这座城市默默无声的深情和娓娓道来的故事；揣着兴奋一晚的心情，在外星球一般的卡帕多西亚乘坐<a href="/poi/6563711.html" class="link _j_keyword_poi" data-poi_id="6563711" data-cs-p="ginfo_txt_poi" data-kw="热气球" target="_blank">热气球</a>，破晓升空，鸟瞰着整个地表，犹如一副地图；赤着脚在散落着微光的<a href="/poi/5443331.html" class="link _j_keyword_poi" data-poi_id="5443331" data-cs-p="ginfo_txt_poi" data-kw="棉花堡" target="_blank">棉花堡</a>，感受这片柔软地平面，如似在云端一样轻盈；瞻仰着<a href="/travel-scenic-spot/mafengwo/10063.html" class="link _j_keyword_mdd" data-kw="罗马" target="_blank">罗马</a>时代留下的以弗所神迹，殊不知自己正被猫在角落里的神秘眼神仰望着；带着小伙伴，自驾在最美的D400沿海公路，感受着沿途<a href="/travel-scenic-spot/mafengwo/14430.html" class="link _j_keyword_mdd" data-kw="地中海" target="_blank">地中海</a>小镇的曼妙风情；而当我来到最后一站<a href="/poi/7915301.html" class="link _j_keyword_poi" data-poi_id="7915301" data-cs-p="ginfo_txt_poi" data-kw="阿拉恰特" target="_blank">阿拉恰特</a>，这个<a href="/travel-scenic-spot/mafengwo/17211.html" class="link _j_keyword_mdd" data-kw="爱琴海" target="_blank">爱琴海</a>边的美丽小镇让我仿佛重回<a href="/travel-scenic-spot/mafengwo/10168.html" class="link _j_keyword_mdd" data-kw="希腊" target="_blank">希腊</a>，路上总有记忆<a href="/travel-scenic-spot/mafengwo/139844.html" class="link _j_keyword_mdd" data-kw="中希腊" target="_blank">中希腊</a>的样子。<br>
                <br>
                十八天的时间，一天天的过，而我们的行程是一个圈，是结束，也是开始，是我的结束，也是你们行程的开始。<br>
                新浪微博：小A家的饭团
            </p>
            <%--data:image/gif;base64,R0lGODlhAQABAIAAAP///wAAACH5BAEAAAAALAAAAAABAAEAAAICRAEAOw==--%>
            <div class="add_pic _j_anchorcnt _j_seqitem">
                <a target="_blank" href="/photo/10053/scenery_5351680/87075682.html">
                    <img class="_j_lazyload item-img" style="min-height: 680px; display: block; background: rgb(252, 242, 220);"
                        src=""
                        data-src="<%=item%>" alt="" />
                </a>
            </div>
            <%
                  }
              } %>
        </div>
    </form>
    <script src="js/jquery-1.7.1.min.js"></script>
    <script>
        //function setImg(index) {
        //    var oDiv = document.getElementById("div1")
        //    var oUl = oDiv.children[0];
        //    var aLi = oUl.children;
        //    if (aLi[index].dataset) {
        //        var src = aLi[index].dataset.src;
        //    } else {
        //        var src = aLi[index].getAttribute('data-src');
        //    }
        //    var oImg = document.createElement('img');
        //    oImg.src = src;
        //    if (aLi[index].children.length == 0) {
        //        aLi[index].appendChild(oImg);
        //    }
        //}
        function setImg(index) {
            var img = $("#div1").find("img")[index];
            var src = $(img).attr("src");
            if (src == "") {
                $(img).attr("src", $(img).data("src"));
            }
        }
        //获得对象距离页面顶端的距离  
        function getH(obj) {
            var h = 0;
            while (obj) {
                h += obj.offsetTop;
                obj = obj.offsetParent;
            }
            return h;
        }
        window.onscroll = function () {
            var oDiv = document.getElementById('div1');
            var oUl = oDiv.children[0];
            var aLi = oUl.children;

            for (var i = 0, l = aLi.length; i < l; i++) {
                var oLi = aLi[i];
                //检查oLi是否在可视区域
                var t = document.documentElement.clientHeight + (document.documentElement.scrollTop || document.body.scrollTop);
                var h = getH(oLi);
                if (h < t) {
                    setTimeout("setImg(" + i + ")", 500);
                }
            }
        };
    </script>
</body>

</html>
