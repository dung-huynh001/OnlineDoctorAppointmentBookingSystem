$(document).ready(function () {
    showProfile();
    validateData();

    // Handle event click Edit button
    $('#btnEdit').click(function () {
        $('#btnSave').show();
        $('#btnCancel').show();
        $('#btnChangePhoto').show();
        $('input').prop('disabled', false);
        $('select').prop('disabled', false);
        $('#fullName').focus();
        $(this).hide();
    });

    // Handle event click Cancel button
    $('#btnCancel').click(function () {
        $('#btnEdit').show();
        $('#btnSave').hide();
        $('#btnChangePhoto').hide();
        showProfile();
        $('input').prop('disabled', true);
        $('select').prop('disabled', true);
        $(this).hide();
    });

    // Handle event click Save button
    $('#btnSave').click(function () {
        var profileData = {
            fullName: $('#fullName').val(),
            email: $('#email').val(),
            nationalID: $('#nationalID').val(),
            dateOfBirth: $('#dateOfBirth').val(),
            gender: $('#gender').val(),
            phoneNumber: $('#phoneNumber').val(),
            address: $('#address').val()
        }

        /*Get a new profile picture to send it to the server*/
        var uploadFile = $('#inputProfilePicture').get(0);
        var files = uploadFile.files;

        var fileData = new FormData();

        // Looping over all files and add it to FormData object  
        for (var i = 0; i < files.length; i++) {
            fileData.append(files[i].name, files[i]);
        }

        fileData.append('username', '');

        $.ajax({
            url: '/Profile/Edit',
            method: 'POST',
            dataType: 'JSON',
            data: profileData,
            success: function (res) {
                if (res.success == true) {
                    Swal.fire({
                        position: 'top',
                        icon: 'success',
                        title: 'Congratulations!',
                        text: res.message,
                        showConfirmButton: false,
                        timer: 3000,
                    })
                } else {
                    Swal.fire({
                        position: 'top',
                        icon: 'error',
                        title: 'Failed!',
                        text: res.message,
                        showConfirmButton: false,
                        timer: 3000,
                    })
                }
            },
            error: function (err) {
                Swal.fire({
                    position: 'top',
                    icon: 'error',
                    title: 'Failed!',
                    text: err.responseText,
                    showConfirmButton: false,
                    timer: 3000,
                })
            }
        });

        $.ajax({
            url: '/Account/UploadFiles',
            method: 'POST',
            contentType: false,
            processData: false,
            data: fileData,
            success: function (res) {
                console.log(res.message);
            },
            error: function (err) {
                console.log(err.responseText);
            }
        });

        $('input').prop('disabled', true);
        $('select').prop('disabled', true);
        $('#btnEdit').show();
        $(this).hide();
        $('#btnCancel').hide();
        $('#btnChangePhoto').hide();
    });
});


var showProfile = function () {
    $.ajax({
        url: '/Profile/ViewProfile',
        type: 'GET',
        dataType: 'JSON',
        success: function (res) {
            setDataProfile(res);
        },
        error: function (err) {
            Swal.fire({
                position: 'top',
                icon: 'error',
                title: 'Error!',
                text: err.responseText,
                showConfirmButton: false,
                timer: 3000,
                width: '30em'
            })
        }
    })
}

var setDataProfile = function (res) {
    if (res.data != undefined) {
        $('#fullName').val(res.data.fullName);
        $('#email').val(res.data.email);
        $('#nationalID').val(res.data.nationalID);
        $('#dateOfBirth').val(res.data.dateOfBirth);
        $('#gender').val(res.data.gender);
        $('#phoneNumber').val(res.data.phoneNumber);
        $('#address').val(res.data.address);
        var img = document.getElementById("profilePicture");
        img.src = "/Uploads/" + res.data.profilePicture;
    }
}

// Display the image immediately after selecting it from the computer
$("#inputProfilePicture").change(function (e) {
    for (var i = 0; i < e.originalEvent.srcElement.files.length; i++) {
        var file = e.originalEvent.srcElement.files[i];
        var img = document.getElementById("profilePicture");
        var reader = new FileReader();
        reader.onloadend = function () {
            img.src = reader.result;
        }
        reader.readAsDataURL(file);
    }    
});

// Custom select list
var firstOptions = document.getElementsByClassName('gender');
for (var i = 0; i < firstOptions.length; i++) {
    var option = firstOptions[i].getElementsByTagName('option')[0];
    option.setAttribute('hidden', true);
    option.setAttribute('selected', true);
    option.setAttribute('disabled', true);
}

var validateData = function () {
    jQuery.validator.addMethod('passwordFormat', function (value) {
        var regex = /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{6,50}$/;
        return value.trim().match(regex);
    });
    jQuery.validator.addMethod('phoneNumberFormat', function (value) {
        var regex = /^(84|0[3|5|7|8|9])+([0-9]{8})\b$/;
        return value.trim().match(regex);
    });

    $('#profileForm').validate({
        onfocusout: function (element) {
            $(element).valid()
        },
        rules: {
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
