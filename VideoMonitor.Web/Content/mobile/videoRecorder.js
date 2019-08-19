//检查用户信息是否完整
function CheckUserInfo() {
    $.get("/user/CheckUserInfoIsIntactAsync", function (res) {
        if (res == false) {
            msgBox("请完善个人信息！");
            $('#maskBox').show();
            return false;
        }
        else {
            $('#maskBox').hide();
            return true;
        }
    });
}
function getFileExt(fileName) {
    var index1 = fileName.lastIndexOf(".");
    var index2 = fileName.length;
    var suffix = fileName.substring(index1 + 1, index2).toLowerCase();//后缀名
    return suffix;
}
//JS操作cookies方法!
//写cookies
function setCookie(name, value) {
    document.cookie = name + "=" + escape(value) + "; path=/";
}
function getCookie_(name) {
    var arr, reg = new RegExp("(^| )" + name + "=([^;]*)(;|$)");
    if (arr = document.cookie.match(reg))
        return unescape(arr[2]);
    else
        return null;
}
var Interval_;
//弹窗提示
function msgBox(msg, isreload) {
    clearTimeout(Interval_);
    $('.modal-body').text(msg);
    $('.modal').modal('show');
    Interval_ = setInterval(function () {
        $('.modal').modal('hide');
        if (isreload) {
            location.reload();
        }
    }, 2000);
}
//在界面上追加一行成功提示
function AppendRow(res) {
    var a = document.createElement('a');
    a.target = '_blank';
    a.innerHTML = res;
    var logPanel = document.querySelector('.alert-success')
    $('.alert-success').removeClass('hidden');
    logPanel.appendChild(a);
    //logPanel.appendChild(document.createElement('hr'));
}
function makeXMLHttpRequest(url, data, callback) {
    var request = new XMLHttpRequest();
    request.onreadystatechange = function () {
        $('#videos-container').hide();
        if (request.readyState == 4 && request.status == 200) {
            var res = request.responseText;
            callback(res);
        }
    };

    request.upload.onprogress = function (progress) {
        if (progress.lengthComputable) {
            var num_ = progress.loaded / progress.total * 100;
            console.log(num_);
            $('.progressSection').removeClass('hidden')

            $('.progress .progress-bar').css('width', num_ + '%');
            $('.progress .progress-bar .sr-only').text(num_ + '%')
        }
    };
    request.upload.onloadstart = function () {
        console.log('started...');
    };
    request.open('POST', url);
    request.send(data);
}

//上次视频
function uploadToDotNetServer(blob, videoType, subType, location,lng, lat) {

    // create FormData
    var formData = new FormData();
    formData.append('video-filename', blob.name);
    formData.append('video-blob', blob);
    //alert(blob.name);

    var boxNum_;
    if (document.querySelector('#boxNum') != undefined) {
        boxNum_ = document.querySelector('#boxNum').value;
    }
    var _url = '/video/UploadVideoAsync?type=' + videoType + '&boxNum=' + boxNum_ + '&subType=' + subType + '&location=' + location + '&lng=' + lng + '&lat=' + lat;
    makeXMLHttpRequest(_url, formData, function (res) {
        var downloadURL = '/UploadFile/Video' + blob.name;

        console.log('File uploaded to this path:', downloadURL);
        $('.progressSection').addClass('hidden');
        AppendRow("上传完成");
        //AppendRow(res);//测试用
        msgBox("上传完成！");
    });
}

/*----0313-------------验证数据 是数字：返回true；不是数字：返回false--------工具方法，不含有业务逻辑---------------------*/
function isNotANumber(inputData) {
    　　//isNaN(inputData)不能判断空串或一个空格
    　　//如果是一个空串或是一个空格，而isNaN是做为数字0进行处理的，而parseInt与parseFloat是返回一个错误消息，这个isNaN检查不严密而导致的。
    　　if (parseFloat(inputData).toString() == "NaN") {
        　　　　//alert("请输入数字……");注掉，放到调用时，由调用者弹出提示。
        　　　　return false;
    　　} else {
        　　　　return true;
    　　}
}
/*--End--0313-------------验证数据 是数字：返回true；不是数字：返回false---------------------------------*/