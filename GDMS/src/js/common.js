//动态加载js，成功后执行js方法
function loadJS(url,success){
  var domScript = document.createElement('script');
  domScript.type = "text/javascript";
  domScript.src = url;
  success = success || function(){};
  domScript.onload = domScript.onreadystatechange = function(){
    if (!this.readyState || 'loaded' === this.readyState || 'complete' === this.readyState){
      success();
      this.onload = this.onreadystatechange = null;
      this.parentNode.removeChild(this);
    }
  }
  document.getElementsByTagName('head')[0].appendChild(domScript);
}

//获取地址栏参数
function getPar(par){
    //获取当前URL
    var local_url = document.location.href; 
    //获取要取得的get参数位置
    var get = local_url.indexOf(par +"=");
    if(get == -1){
        return false;   
    }   
    //截取字符串
    var get_par = local_url.slice(par.length + get + 1);    
    //判断截取后的字符串是否还有其他get参数
    var nextPar = get_par.indexOf("&");
    if(nextPar != -1){
        get_par = get_par.slice(0, nextPar);
    }
    return get_par;
}

//js获取项目根路径，如： http://localhost:8083/uimcardprj
function getRootPath(){
    //获取当前网址，如： http://localhost:8083/uimcardprj/share/meun.jsp
    var curWwwPath=window.document.location.href;
    //获取主机地址之后的目录，如： uimcardprj/share/meun.jsp
    var pathName=window.document.location.pathname;
    var pos=curWwwPath.indexOf(pathName);
    //获取主机地址，如： http://localhost:8083
    var localhostPaht=curWwwPath.substring(0,pos);
    //获取带"/"的项目名，如：/uimcardprj
    var projectName=pathName.substring(0,pathName.substr(1).indexOf('/')+1);
    //return(localhostPaht+projectName);
	return(localhostPaht);
}
//获取项目根路径
rootpath = getRootPath();


//金额用逗号隔开（数字格式化）
//调用：fmoney("12345.675910", 3)，返回12,345.676 
function fmoney(s,n){  
	n = n > 0 && n <= 20 ? n : 2;  
	s = parseFloat((s + "").replace(/[^\d\.-]/g, "")).toFixed(n) + "";  
	var l = s.split(".")[0].split("").reverse(), r = s.split(".")[1];  
	t = "";  
	for (z = 0; z < l.length; z++) {  
		t += l[z] + ((z + 1) % 3 == 0 && (z + 1) != l.length ? "," : "");  
	}  
	return t.split("").reverse().join("") + "." + r;  
} 
//还原函数： 
function rmoney(s){  
	return parseFloat(s.replace(/[^\d\.-]/g, ""));  
}  


//设置指定的Cookie的值
function setCookie(Name,CookieValue){
  //document.cookie=Name+"="+CookieValue;

	//计算无限期的过期时间设置never的时间为当前时间加上十年的毫秒值
	//var never = new Date();
	//never.setTime(never.getTime()+10*365*24*60*60*1000);    
	//var expString = "expires="+ never.toGMTString()+";";
	//带过期时间的设置方法
  //document.cookie=Name+"="+CookieValue+"; "+expString;
  
	var never = new Date();
	never.setTime(never.getTime()+5*1000);    
	var expString = "expires="+ never.toGMTString()+";";
  document.cookie=Name+"="+CookieValue+";path=/;"+expString;  //加上"path=/"设置cookie域，"/"代表所有页面可以获取
} 

//取得指定的Cookie的值
function getCookie(Name){
  var result = null;
  //对cookie信息进行相应的处理，方便搜索
  var myCookie = ""+document.cookie+";"; 
  var searchName = Name+"=";
  var startOfCookie = myCookie.indexOf(searchName);
  var endOfCookie;
  if(startOfCookie != -1)
  {
    startOfCookie =startOfCookie + searchName.length;
    endOfCookie = myCookie.indexOf(";",startOfCookie);
    result = myCookie.substring(startOfCookie,endOfCookie);
  }
  return result;
}

//jQuery扩展向左划入，向右划出效果
jQuery.fn.slideLeftHide = function( speed, callback ) {  
  this.animate({  
    width : "hide",  
    paddingLeft : "hide",  
    paddingRight : "hide",  
    marginLeft : "hide",  
    marginRight : "hide"  
  }, speed, callback );  
};
jQuery.fn.slideLeftShow = function( speed, callback ) {  
  this.animate({  
    width : "show",  
    paddingLeft : "show",  
    paddingRight : "show",  
    marginLeft : "show",  
    marginRight : "show"  
  }, speed, callback );  
};  

//获取今天日期，默认格式YYYYmmdd，加-格式yyyy-mm-dd
function todayDate(format){
  var nowDate = new Date();
  var year = nowDate.getFullYear();
  var month = nowDate.getMonth() + 1 < 10 ? "0" + (nowDate.getMonth() + 1)
  : nowDate.getMonth() + 1;
  var day = nowDate.getDate() < 10 ? "0" + nowDate.getDate() : nowDate
  .getDate();
  if(format=='-'){
    return (year +'-'+ month +'-'+ day);
  }else{
    return (year + month + day);
  }
}
//格式化日期YYYYmmdd，参数年月日
function formatDate(y,m,d){
  
  m = m < 10 ? "0" + m:m;
  d = d < 10 ? "0" + d:d;
  return (y.toString() + m.toString() + d.toString());
}

function GetDateStr(date,AddDayCount) {
    year = parseInt(date.substr(0,4));
    month = parseInt(date.substr(4,2));
    day = parseInt(date.substr(6,2));
    var dd = new Date(year, month-1, day);
    dd.setDate(dd.getDate()+AddDayCount);//获取AddDayCount天后的日期
    var y = dd.getFullYear();
    var m = dd.getMonth()+1;//获取当前月份的日期
    var d = dd.getDate();
    
    m = m < 10 ? "0" + m:m;
    d = d < 10 ? "0" + d:d;
    return (y.toString() + m.toString() + d.toString());
}


var loadingNum = 0;
function loadingDiv(action){
  if(action=='load'){
    loadingNum = loadingNum +1;
    $('.layui-body').append(
      '<div class="load"><i class="layui-icon layui-anim layui-anim-rotate layui-anim-loop">&#xe63e;</i>Loading</div>'+
      '<div id="mask" class="mask"></div>' 
    );
    $("#mask").css("height",$(document).height()-50);     
    $("#mask").css("width",$(document).width());   
  }else if(action!='load'&&loadingNum>1){
    loadingNum = loadingNum -1;
  }else{
    loadingNum = loadingNum -1;
    $('.load').remove();
    $('#mask').remove();
  }
}




//ajax监听函数
;(function() {
    function ajaxEventTrigger(event) {
        var ajaxEvent = new CustomEvent(event, { detail: this });
        window.dispatchEvent(ajaxEvent);
    }
    
    var oldXHR = window.XMLHttpRequest;
 
    function newXHR() {
        var realXHR = new oldXHR();
 
        realXHR.addEventListener('abort', function () { ajaxEventTrigger.call(this, 'ajaxAbort'); }, false);
 
        realXHR.addEventListener('error', function () { ajaxEventTrigger.call(this, 'ajaxError'); }, false);
 
        realXHR.addEventListener('load', function () { ajaxEventTrigger.call(this, 'ajaxLoad'); }, false);
 
        realXHR.addEventListener('loadstart', function () { ajaxEventTrigger.call(this, 'ajaxLoadStart'); }, false);
 
        realXHR.addEventListener('progress', function () { ajaxEventTrigger.call(this, 'ajaxProgress'); }, false);
 
        realXHR.addEventListener('timeout', function () { ajaxEventTrigger.call(this, 'ajaxTimeout'); }, false);
 
        realXHR.addEventListener('loadend', function () { ajaxEventTrigger.call(this, 'ajaxLoadEnd'); }, false);
 
        realXHR.addEventListener('readystatechange', function() { ajaxEventTrigger.call(this, 'ajaxReadyStateChange'); }, false);
 
        return realXHR;
    }
 
    window.XMLHttpRequest = newXHR;
})();



//去掉字符串前后所有空格
function Trim(str){ 
 return str.replace(/(^\s*)|(\s*$)/g, ""); 
}


//进入全屏
function requestFullScreen() {
  var de = document.documentElement;
  if (de.requestFullscreen) {
    de.requestFullscreen();
  } else if (de.mozRequestFullScreen) {
    de.mozRequestFullScreen();
  } else if (de.webkitRequestFullScreen) {
    de.webkitRequestFullScreen();
  }
}
//退出全屏
function exitFullscreen() {
  var de = document;
  if (de.exitFullscreen) {
    de.exitFullscreen();
  } else if (de.mozCancelFullScreen) {
    de.mozCancelFullScreen();
  } else if (de.webkitCancelFullScreen) {
    de.webkitCancelFullScreen();
  }
}
