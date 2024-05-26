
var loginController = function () {
    this.initialize = function () {
        registerEvernts();
    }
    var registerEvernts = function () {
        $('#btnLogin').on("click",, function (e) {
            e.preventDefault();
            var user = $('#txtUserName').val();
            var pass = $('#txtPassword').val();
            login(user, pass);
        });
    }
    var login = function (user, pass) {

        $.ajax({
            type: "POST", data: {
                userName: user,
                password: pass
            }, dataType: "json",
            url: "Admin/login/Authen",
            success: function (res) {

                if (res.success) {
                    window.location.href = "/Admin/Home/Index";
                    shopshoe.notify(" login success","success")
                }
                else shopshoe.notify('Login faile', 'error');
           
            }
        })
    }
}

