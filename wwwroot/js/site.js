window.addEventListener('load', function () {
    console.log('Trang web đã tải xong.');
});

function check_login() {
    var error = "";
    var email = document.getElementById('email').value;
    if (email.length < 6) error += "- Mời bạn nhập địa chỉ email\n";

    var password = document.getElementById('password').value;
    if (password.length == 0) error += "- Bạn cần nhập mật khẩu \n";

    if (error != "") {
        alert(error);
        return false;
    }

    Hura.User.login(email, password).then(function (data) {
        if (data.status == 'error') {
            alert(data.message);
        } else {
            alert("Đăng nhập thành công !");

            location.href = '/taikhoan';
        }
    });
}

function check_field_registor() {
    var error = "";
    var email = document.getElementById('email').value;
    if (email.length < 6) error += "- Mời bạn nhập địa chỉ email\n";

    var password = document.getElementById('password').value;
    if (password.length < 6) error += "- Mật khẩu yếu\n";

    var full_name = document.getElementById('full_name').value;
    if (full_name.length < 2) error += "- Mời bạn nhập đúng tên\n";

    var mobile = document.getElementById('tel').value;
    if (mobile.length < 9) error += "- Mời bạn nhập đủ số điện thoại\n";

    var address = document.getElementById('address').value;
    if (address.length < 6) error += "- Mời bạn nhập địa chỉ\n";

    var province = $("#ship_to_province option:checked").val();
    if (province == '') error += "- Mời bạn chọn tỉnh/Thành phố\n";

    var district = $("#js-district-holder option:checked").val();
    if (district == '') error += "- Mời bạn chọn quận, huyện\n";

    var pass = $("#password").val();
    var pass1 = $("#password1").val();

    var sex = $('input[type="radio"]:checked').val()
    if (pass1 != pass) {
        error += '- Mật khẩu không trùng khớp. Vui lòng nhập lại';
    }

    if (error != "") {
        alert(error);
        return false;
    } else {

        var registerParams = {
            action_type: "register",
            info: {
                email: email,
                name: full_name,
                tel: mobile,
                mobile: mobile,
                sex: sex,
                birthday: '',
                password: password,
                address: address,
                province: province,
                district: district
            }
        }

        Hura.Ajax.post('customer', registerParams).then(function (data) {
            console.log(data);
            if (data.status == 'error' && data.message == 'Email exist') {
                alert('Email đã được sử dụng \n Vui lòng đăng ký lại ! ')
            } else {
                alert('Bạn đã đăng ký thành công ! ')
                location.href = "/dang-nhap";
            }
        })

    }
}


function BuyNow(prId) {

}

function AddToCard(prId) {
    alert(prId)
}