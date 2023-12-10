$(document).ready(function () {
    showProfile();
   
    

    $('#dateOfBirth').on('focus', function () {
        $(this).attr('type', 'date');
    });

    $('#dateOfBirth').on('blur', function () {
        $(this).attr('type', 'text');
    });

    // Handle event click Edit button
    $('#btnEdit').click(function () {
        $('#btnSave').show();
        $('#btnCancel').show();
        $('#btnChangePhoto').show();
        $(this).hide();
    });

    // Handle event click Cancel button
    $('#btnCancel').click(function () {
        $('#btnEdit').show();
        $('#btnSave').hide();
        $('#btnChangePhoto').hide();
        showProfile();
        $(this).hide();
    });

    // Handle event click Save button
    $('#btnSave').click(function () {
       

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
            url: '/Account/UploadFiles',
            method: 'POST',
            contentType: false,
            processData: false,
            data: fileData,
            success: function (res) {
                if (res.success == true) {
                    Swal.fire({
                        position: 'top',
                        icon: 'success',
                        title: 'Congratulations!',
                        text: 'Profile photo update successfully',
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
        url: '/Admin/AdminProfile/ViewProfile',
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
               
                width: '30em'
            })
        }
    })
}

var setDataProfile = function (res) {
    if (res.data != undefined) {
        $('#fullName').val(res.data.fullName);
        $('#email').val(res.data.email);
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

