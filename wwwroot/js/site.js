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

    let data = new FormData();
    data.append("Sex", sex)
    data.append("Sex", sex)

    fetch("/Account/Login", {
        method: "POST",
        body: data
    }).then(res => res.json())
        .then(res => {
            if (res.statusCode == 200) {
                window.location.reload()
            }
        })
        .catch(err => {
            console.log("Lỗi đăng nhập ", err)
            alert("Lỗi đăng nhập")
        })
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

    var date = $("#birtday").val();
    var month = $("#birtmonth").val();
    var year = $("#birtyear").val();

    var sex = $('input[type="radio"]:checked').val() === 'male' ? true : false
    if (pass1 != pass) {
        error += '- Mật khẩu không trùng khớp. Vui lòng nhập lại';
    }

    if (error != "") {
        alert(error);
        return false;
    } else {

        let data = new FormData();
        data.append("UserName", email)
        data.append("FullName", full_name)
        data.append("Phone", mobile)
        data.append("Sex", sex)
        data.append("PasswordHash", password)
        data.append("ConfirmPassword", password)
        data.append("Address", address)
        data.append("Province", province)
        data.append("DayDate", date)
        data.append("MonthDate", month)
        data.append("YearMonth", year)

        fetch("/Account/Register", {
            method: "POST",
            body: data
        })

    }
}


function BuyNow(prId) {

}

function AddToCard(prId) {
    alert(prId)
}