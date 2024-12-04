function alersweet(type, message) {
    const Toast = Swal.mixin({
        toast: true,
        position: "top-end",
        showConfirmButton: false,
        timer: 3000,
        timerProgressBar: true,
        didOpen: (toast) => {
            toast.onmouseenter = Swal.stopTimer;
            toast.onmouseleave = Swal.resumeTimer;
        }
    });
    Toast.fire({
        icon: type,
        title: message
    });
}
$(document).on("click", "#send-otp", async function () {
    const email = $("#email").val().trim();
    if (email === "") {
        alersweet("error", "Email không được để trống");
        return;
    }

    const res = await $.ajax({
        url: "/api/send-otp",
        type: "POST",
        data: { email: email }
    });

    if (res.success) {
        alersweet("success", res.message);
    } else {
        alersweet("error", res.message);
    }
});

$(document).on("click", "#verify-otp", async function () {
    const otp = $("#otp").val().trim();
    if (otp === "") {
        alersweet("error", "OTP không được để trống");
        return;
    }

    const res = await $.ajax({
        url: "/api/verify-otp",
        type: "POST",
        data: { otp: otp }
    });

    if (res.success) {
        alersweet("success", res.message);
    } else {
        alersweet("error", res.message);
    }
});

function validateEmail(email) {
    const re = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    return re.test(email);
}
$(document).on("click", "#submit", async function () {
    var username = $('#name').val().trim();
    var password = $('#password').val().trim();
    var email = $('#email').val().trim();
    var re_password = $('#re-password').val().trim();

    if (email === "") {
        alersweet("error", "Không được bỏ trống email");
        return;
    }

    if (username === "") {
        alersweet("error", "Không được bỏ trống username");
        return;
    }
    if (password === "") {
        alersweet("error", "Không được bỏ trống password");
        return;
    }
    if (re_password === "") {
        alersweet("error", "Vui lòng nhập lại mật khẩu");
        return;
    }
    if (password !== re_password) {
        alersweet("error", "Mật khẩu nhập lại không chính xác");
        return;
    }
    const otpCheck = await $.ajax({
        url: '/api/check-otp-verified',
        type: 'GET'
    });

    if (!otpCheck.success) {
        alersweet("error", "Bạn cần xác thực OTP trước khi đăng ký.");
        return;
    }
    Login(username, password, email);
});



async function Login(username, password, email) {
    const res = await $.ajax({
        url: '/api/register',
        type: 'POST',
        data: {
            email: email,
            username: username,
            password: password
        }
    });
    if (res.success) {
        Swal.fire({
            title: "Good job!",
            text: "Đăng ký tài khoản thành công",
            icon: "success"
        }).then((result) => {
            if (result.isConfirmed || result.isDismissed) {
                window.location.href = res.url;
            }
        });

    }
    else {
        alersweet("error", res.Message);
    }

}