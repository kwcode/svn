<%@ Control Language="C#" AutoEventWireup="true" CodeFile="uc_banner.ascx.cs" Inherits="master_uc_uc_banner" %>
<div class="banner">
    <script src="../../js/jquery-banner.js" type="text/javascript"></script>
    <style>
        .banner-no  { margin: 0; padding: 0; text-align: center; }
        ul li { list-style: none; }
        .banner { width: 100%; position: relative; }
        .banner-list { position: relative; width: 100%; height: 300px; overflow: hidden; }
        .banner-no { position: absolute; }

        .banner-list ul li img { width: 100%; height: 300px; }
        .banner-list ul li { display: none; }
        .banner-list ul li.on { display: block; }

        .bannertitle { }
        .banner-bg { height: 42px; line-height: 42px; border-radius: 3px; background-color: #000; opacity: 0.3; width: 100%; position: absolute; bottom: 10px; }
        .bannertitle-info { height: 42px; line-height: 42px; left: 10px; bottom: 10px; position: absolute; z-index: 1001; color: #fff; }

        .banner-bg { }
        .bannertitle-info { }

        .banner-no { position: absolute; right: 0; bottom: 20px; z-index: 1002; color: #fff; }
        .banner-no ul li { overflow: hidden; line-height: 20px; float: left; cursor: pointer; margin-right: 5px; border-radius: 20px; background: #F90; width: 20px; height: 20px; }
    </style>
    <div class="banner-list">
        <ul>
            <li class="on">
                <img src="images/banae.jpg" title="1" alt="1" />
                <div class="bannertitle">
                    <div class="banner-bg">
                    </div>
                    <a class="bannertitle-info">111111111</a>
                </div>
            </li>
            <li>
                <img src="images/jianjie.png" title="1" alt="1" />
                <div class="bannertitle">
                    <div class="banner-bg">
                    </div>
                    <a class="bannertitle-info">222222222</a>
                </div>
            </li>
            <li>
                <img src="images/kk.jpg" title="1" alt="1" />
                <div class="bannertitle">
                    <div class="banner-bg">
                    </div>
                    <a class="bannertitle-info">3333333333333</a>
                </div>
            </li>
            <li>
                <img src="images/logo.png" title="1" alt="1" />
                <div class="bannertitle">
                    <div class="banner-bg">
                    </div>
                    <a class="bannertitle-info">44444444444</a>
                </div>
            </li>
        </ul>
    </div>
</div>
