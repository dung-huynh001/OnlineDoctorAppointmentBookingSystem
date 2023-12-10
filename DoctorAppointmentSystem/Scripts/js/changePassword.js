

$(document).on('submit', '#formChangePassword', function (event) {
    event.preventDefault();

    var data = {
        currPass: $('#currPass').val(),
        newPass: $('#newPass').val(),
    }

    if ($('#formChangePassword').valid()) {
        $.ajax({
            url: '/Account/ChangePasswordAPI',
            method: 'POST',
            data: data,
            dataType: 'JSON',
            success: function (res) {
                console.log(res);
                if (res.success) {
                    Swal.fire({
                        position: 'top',
                        icon: 'success',
                        title: 'Congratulation!',
                        text: res.message,
                        showConfirmButton: false,
                        timer: 3000,
                        width: '30em'
                    });
                    setTimeout(function () {
                        $('#formChangePassword').trigger('reset');
                        logoutUser();
                    }, 2000);
                    $('#currPass').get(0).type = 'password';
                    $('#newPass').get(0).type = 'password';
                    $('#confirmPass').get(0).type = 'password';
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
                console.log(err.responseText);
            }
        })
    }
})

function logoutUser() {
    $.ajax({
        url: '/Account/LogoutAPI',
        method: 'GET',
        dataType: 'JSON',
        success: function (res) {
            if (res.success) {
                window.location.href = '/Account/Login';
            }
        },
        error: function (err) {
            console.log(err.responseText);
        }
    });
}

function validChangePasswordForm() {
    jQuery.validator.addMethod('passwordFormat', function (value) {
        var regex = /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{6,50}$/;
        return value.trim().match(regex);
    });

    $('#formChangePassword').validate({
        onfocusout: function (element) {
            $(element).valid()
        },
        rules: {
            "currPass": {
                required: true
                
            },
            "newPass": {
                required: true,
                passwordFormat: true
            },
            "confirmPass": {
                required: true,
                passwordFormat: true,
                equalTo: "#newPass"
            }
        },
        messages: {
            "currPass": {
                required: 'Password is required.'
                
            },
            "newPass": {
                required: 'New password is required.',
                passwordFormat: "Password charater at least one uppercase letter, one lowercase letter, one number and one special character"
            },
            "confirmPass": {
                required: 'Confirm password value is required.',
                passwordFormat: "Password charater at least one uppercase letter, one lowercase letter, one number and one special character",
                equalTo: "Does not match the password."
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
}
// show hihe pass
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
    
    validChangePasswordForm();
})