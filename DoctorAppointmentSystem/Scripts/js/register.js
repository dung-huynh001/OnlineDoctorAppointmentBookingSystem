var firstOptions = document.getElementsByClassName('gender');
for (var i = 0; i < firstOptions.length; i++) {
    var option = firstOptions[i].getElementsByTagName('option')[0];
    option.setAttribute('hidden', true);
    option.setAttribute('selected', true);
    option.setAttribute('disabled', true);
}

// Processing event select value of Authetication Question
$(document).ready(function () {
    var selectedValues = [];
    $('.authQuestion').on('change', function () {
        var selectedValue = {
            value: $(this).val(),
            selector: $(this).attr('id')
        }
        if (selectedValues.length === 0) {
            selectedValues.push(selectedValue)
        } else {
            var flag = 0;
            for (var i = 0; i < selectedValues.length; i++) {
                flag = 0;
                if (selectedValue.selector === selectedValues[i].selector) {
                    selectedValues[i].value = selectedValue.value;
                    flag = 1;
                    break;
                }
            }
            if (flag === 0) selectedValues.push(selectedValue);
        }
        for (var i = 0; i < selectedValues.length; i++) {
            if (selectedValues[i].selector === $(this).attr('id')) {
                $('select').not(this).find('option[value="' + selectedValues[i].value + '"]').prop('selected', false);
            }
        }
    });
});

// Get registration data of User and Patient on form to send to AccountController
$('.register-form').submit(function (event) {
    event.preventDefault();
    var confirmPassword = $('#confirmPassword').val();
    var password = $('#password').val();

    // Get User's info
    var user = {
        username: $('#username').val(),
        password: password,
        email: $('#email').val(),
        passwordRecoveryQue1: $('#authQuestion1').val(),
        passwordRecoveryQue2: $('#authQuestion2').val(),
        passwordRecoveryQue3: $('#authQuestion3').val(),
        passwordRecoveryAns1: $('#answer1').val(),
        passwordRecoveryAns2: $('#answer2').val(),
        passwordRecoveryAns3: $('#answer3').val(),
        profilePicture: $('#profilePicture').val()
    }

    // Get Patient's info
    var patient = {
        fullName: $('#fullName').val(),
        nationalID: $('#nationalID').val(),
        dateOfBirth: $('#dateOfBirth').val(),
        gender: $('#gender').val(),
        phoneNumber: $('#phoneNumber').val(),
        address: $('#address').val()
    }

    // Get profile picture
    var fileUpload = $("#profilePicture").get(0);
    var files = fileUpload.files;

    var fileData = new FormData();

    // Looping over all files and add it to FormData object  
    for (var i = 0; i < files.length; i++) {
        fileData.append(files[i].name, files[i]);
    }

    // Adding username to FormData object 
    fileData.append('username', user.username);


    // Using AJAX send register data to server
    if ($('.register-form').valid()) {
        if (password !== confirmPassword) {
            Swal.fire({
                position: 'top',
                icon: 'error',
                title: 'Error!',
                text: 'Please check your registration information!',
                showConfirmButton: false,
                timer: 3000,
                width: '30em'
            })
            $('#confirmPassword').focus();
        } else {
            $.ajax({
                url: '/Account/RegisterAPI',
                method: 'POST',
                data: {
                    user: user,
                    patient: patient
                },
                success: function (res) {
                    if (res.success == true) {
                        Swal.fire({
                            position: 'top',
                            icon: 'success',
                            title: 'Congratulations!',
                            text: res.message,
                            showConfirmButton: false,
                        })
                        setTimeout(function () {
                            window.location.href = '/Account/Login';
                            $('.register-form').trigger('reset');
                        }, 3000);
                    } else {
                        Swal.fire({
                            position: 'top',
                            icon: 'error',
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
                        icon: 'error',
                        title: 'Failed!',
                        text: err.message,
                        showConfirmButton: false,
                        timer: 3000,
                        width: '30em'
                    })
                }
            });

            // Upload profile picture if it is not null
            if ($('#pictureName').val() != null) {
                $.ajax({
                    url: '/Account/UploadFiles',
                    type: "POST",
                    contentType: false,
                    processData: false,
                    data: fileData,
                    success: function (result) {
                        console.log('Great! ' + result.message);
                    },
                    error: function (err) {
                        console.log('Error! ' + err.statusText);
                    }
                });
            }
        }
    } else {
        Swal.fire({
            position: 'top',
            icon: 'error',
            title: 'Failed!',
            text: 'Please fill out the form completely!',
            showConfirmButton: false,
            timer: 3000,
            width: '30em'
        })
    }
})

// Focus on the answer when selected value onchange
$('#authQuestion1').change(function () {
    $('#answer1').val('');
    $('#answer1').focus();
})

$('#authQuestion2').change(function () {
    $('#answer2').val('');
    $('#answer2').focus();
})

$('#authQuestion3').change(function () {
    $('#answer3').val('');
    $('#answer3').focus();
})

var validateData = function () {
    jQuery.validator.addMethod('passwordFormat', function (value) {
        var regex = /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{6,50}$/;
        return value.trim().match(regex);
    });
    jQuery.validator.addMethod('phoneNumberFormat', function (value) {
        var regex = /^(84|0[3|5|7|8|9])+([0-9]{8})\b$/;
        return value.trim().match(regex);
    });

    $('.register-form').validate({
        onfocusout: function (element) {
            $(element).valid()
        },
        rules: {
            "Username": {
                required: true
            },
            "Password": {
                required: true,
                passwordFormat: true,
                minlength: 8
            },
            "Confirm": {
                required: true,
                passwordFormat: true,
                equalTo: "#password"
            },
            "AuthQuestion1": {
                required: true
            },
            "AuthQuestion2": {
                required: true
            },
            "AuthQuestion3": {
                required: true
            },
            "answer1": {
                required: true
            },
            "answer2": {
                required: true
            },
            "answer3": {
                required: true
            },
            "FullName": {
                required: true
            },
            "Email": {
                required: true,
                email: true
            },
            "NationalID": {
                required: true
            },
            "DateOfBirth": {
                required: true
            },
            "Gender": {
                required: true
            },
            "PhoneNumber": {
                required: true,
                phoneNumberFormat: true
            },
            "Address": {
                required: true
            }
        },
        messages: {
            "Username": {
                required: "Username is required."
            },
            "Password": {
                required: "Password is required.",
                passwordFormat: "Password charater at least one uppercase letter, one lowercase letter, one number and one special character",
                minlength: "Password must be between 8 and 50 characters."
            },
            "Confirm": {
                required: "Password is required.",
                passwordFormat: "Password charater at least one uppercase letter, one lowercase letter, one number and one special character",
                equalTo: "Does not match the password."
            },
            "AuthQuestion1": {
                required: "Please select authentication question.",
            },
            "AuthQuestion2": {
                required: "Please select authentication question.",
            },
            "AuthQuestion3": {
                required: "Please select authentication question.",
            },
            "answer1": {
                required: "This value is required."
            },
            "answer2": {
                required: "This value is required."
            },
            "answer3": {
                required: "This value is required."
            },
            "FullName": {
                required: "Full name is required."
            },
            "Email": {
                required: "Email is required.",
                email: "Please enter the correct email format."
            },
            "NationalID": {
                required: "National ID is required."
            },
            "DateOfBirth": {
                required: "Date of birth is required."
            },
            "Gender": {
                required: "Please select gender."
            },
            "PhoneNumber": {
                required: "Phone number is required.",
                phoneNumberFormat: "Please enter the correct Vietnamese phone number format."
            },
            "Address": {
                required: "Address is required."
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

$(document).ready(function () {
    validateData();
})
