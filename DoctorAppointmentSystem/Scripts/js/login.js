var submit = $('#login-form').on('submit', function (event) {
    event.preventDefault();

    var user = {
        username: $('#username').val(),
        password: $('#password').val()
    }
    var antiForgeryToken = $('#login-form input[name="__RequestVerificationToken"]').val();

    if ($('#login-form').valid()) {
        $.ajax({
            url: '/Account/Login',
            method: 'POST',
            headers: {
                'RequestVerificationToken': antiForgeryToken
            },
            data: {
                __RequestVerificationToken: antiForgeryToken,
                model: user
            },
            success: function (res) {
                if (res.success == true) {
                    window.location.href = res.url;
                    $('#login-form').trigger('reset');
                } else {
                    Swal.fire({
                        position: 'top',
                        icon: 'warning',
                        title: 'Failed!',
                        text: res.message,
                        showConfirmButton: false,
                        timer: 3000,
                        width: '30em'
                    })
                }
            },
            error: function (err) {
                Swal.fire({
                    position: 'top',
                    icon: 'warning',
                    title: 'Connect to Server failed!',
                    text: err.responseText,
                    showConfirmButton: false,
                    timer: 3000,
                    width: '30em'
                })
            }
        });
    } else {
        Swal.fire({
            position: 'top',
            icon: 'warning',
            title: 'Failed!',
            text: 'Please fill out the form completely!',
            showConfirmButton: false,
            timer: 3000,
            width: '30em'
        })
    }
});


var validData = $('#login-form').validate({
    onfocusout: function (element) {
        $(element).valid()
    },
    rules: {
        "Username": {
            required: true
        },
        "Password": {
            required: true
        }
    },
    messages: {
        "Username": {
            required: "Username is required."
        },
        "Password": {
            required: "Password is required."
        }
    },
    highlight: function (element) {
        var elem = $(element);
        if (elem.hasClass("select2-hidden-accessible")) {
            element = $(".select2-selection");

            element.addClass('border-2 border-danger')
        } else {
            elem.addClass('border-2 border-danger ')
        }

    },
    unhighlight: function (element) {
        var elem = $(element);
        if (elem.hasClass("select2-hidden-accessible")) {
            element = $(".select2-selection");

            element.removeClass('border-2 border-danger')
        } else {
            elem.removeClass('border-2 border-danger')
        }
    },
})

function showPass() {

    $(".paswd-on-off").each(function () {

        var inp = this;

        if (inp.type == "password") {
            inp.type = "text";
        }
        else {
            inp.type = "password";
        }

    });
}

$(document).ready(function () {
    $('#username').focus();
    submit.call;
    validData.call;
})