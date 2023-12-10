var table = $('#registrationTbl').DataTable({
    ordering: false,
    searching: false,
    info: false,
    responsive: true,
    columns: [
        {
            className: 'dt-control',
            data: null,
            defaultContent: '',
        },
        {
            data: 'id',
            title: 'ID.',
        },
        {
            data: 'name',
            className: 'text-nowrap',
            title: 'Name',
        },
        {
            data: 'dateOfBirth',
            title: 'Date of Birth',
            className: 'text-nowrap',
        },
        {
            data: 'gender',
            title: 'Gender',
            className: 'text-nowrap',
        },
        {
            data: 'nationalID',
            title: 'National ID',
            className: 'text-nowrap',
        },
        {
            data: 'mobile',
            title: 'Mobile',
            className: 'text-nowrap',
        },
        {
            data: 'email',
            title: 'Email',
            className: 'text-nowrap',
        },
        {
            data: 'address',
            title: 'Address',
            className: 'text-nowrap',
        },
        {
            responsivePriority: 1,
            title: "Action",
            data: null,
            "render": function (data, type, row) {
                return '<button data-id="' + row.id + '" class="btn btn-outline-primary btn-sm btnAppointment" data-toggle="modal" data-target="#makeAppointmentModal"><i class="fa-solid fa-file-medical"></i></button>';
            }
        }
    ]
});

function search() {
    var nationalID = $('#searchNationalID').val();
    var name = $('#searchName').val();

    table.clear().draw();

    $.ajax({
        url: '/Doctor/Registration/SearchMembers',
        method: 'GET',
        data: {
            nationalID: nationalID,
            name: name
        },
        dataType: 'JSON',
        success: function (res) {
            if (res.success) {
                var data = res.data;
                if (data.length == 0) {
                    Swal.fire({
                        position: 'top',
                        title: 'Oops...',
                        icon: 'info',
                        text: "Patient information could not be found.",
                        showConfirmButton: false,
                        timer: 3000
                    })
                } else {
                    table.rows.add(data).draw();
                    table.responsive.rebuild();
                    table.responsive.recalc();
                }
            } else {
                Swal.fire({
                    position: 'top',
                    title: 'Oops...',
                    icon: 'warning',
                    text: res.message,
                    showConfirmButton: false,
                    timer: 3000
                })
            }
        },
        error: function (err) {
            console.log(err.responseText);
        }
    })
    $('#searchName').val('');
    $('#searchNationalID').val('');
}

$

$(document).on('click', '#btnSearch', function () {
    search()
})

var searchName = document.getElementById("searchName");

searchName.addEventListener("keydown", function (event) {
    if (event.keyCode === 13) {
        search()
        $(this).blur();
    }
});

var searchNationalID = document.getElementById("searchNationalID");

searchNationalID.addEventListener("keydown", function (event) {
    if (event.keyCode === 13) {
        search();
        $(this).blur();
    }
});

function responsive() {
    if (window.innerWidth <= 550) {
        document.getElementById("btnAddMember").textContent = "Add Member";
    } else {
        document.getElementById("btnAddMember").textContent = "Add New Member";
    }
}

window.addEventListener("resize", responsive);
window.addEventListener("load", responsive);

$('#btnCreate').on('click', function () {
    var isValid = true;

    $('.info-wrapper .input-box input, .info-wrapper .input-box select').each(function () {
        if (!$(this).valid()) {
            isValid = false;
        }
    });

    if (isValid) {
        var member = {
            username: $('#username').val(),
            password: $('#password').val(),
            name: $('#fullName').val(),
            dateOfBirth: $('#dateOfBirth').val(),
            nationalID: $('#nationalID').val(),
            gender: $('#gender').val(),
            mobile: $('#mobile').val(),
            email: $('#email').val(),
            address: $('#address').val(),
            appointmentDate: $('#appointmentDate').val(),
            appointmentTime: $('#time').val(),
            modeOfConsultant: $('#modeOfConsultant').val(),
            consultantType: $('#consultantType').val()
        }

        $.ajax({
            url: '/Doctor/Registration/CreateNewMember',
            method: 'POST',
            data: member,
            dataType: 'JSON',
            success: function (res) {
                if (res.success) {
                    Swal.fire({
                        position: 'top',
                        title: 'Success!',
                        icon: 'success',
                        text: res.message,
                        showConfirmButton: false,
                        timer: 3000
                    }).then(function () {
                        $('#addMemberForm').trigger('reset');
                        $('#addMemberModal').modal('hide');
                    })
                } else {
                    Swal.fire({
                        position: 'top',
                        title: 'Failed!',
                        icon: 'warning',
                        text: 'Failed to add a new members.',
                        showConfirmButton: false,
                        timer: 3000
                    });
                }
            },
            error: function (err) {
                console.log(err.responseText);
            }
        })
    } else {
        Swal.fire({
            position: 'top',
            title: 'Failed!',
            icon: 'warning',
            text: 'Please fill out all information before submitting.',
            showConfirmButton: false,
            timer: 3000
        });
    }
})
var patientID = 0;
$(document).on('click', '.btnAppointment', function () {
    var btnAppt = $(this);
    patientID = btnAppt.data('id');

    $.ajax({
        url: '/Doctor/Registration/LoadAppointmentData',
        method: 'GET',
        data: {
            id: patientID
        },
        dataType: 'JSON',
        success: function (res) {
            if (res.success) {
                var data = res.data;
                $('#apptFullName').val(data.name);
                $('#apptDateOfBirth').val(data.dateOfBirth);
                $('#apptGender').val(data.gender);
                $('#apptNationalID').val(data.nationalID);
                $('#apptMobile').val(data.mobile);
                $('#apptEmail').val(data.email);
                $('#apptAddress').val(data.address);
            } else {
                Swal.fire({
                    position: 'top',
                    title: 'Failed!',
                    icon: 'warning',
                    text: 'Patient data not found.',
                    showConfirmButton: false,
                    timer: 3000
                });
            }
        },
        error: function (err) {
            console.log(err.responseText);
        }
    })
})

$('#btnMakeAppt').on('click', function () {
    
    var isValid = true;

    $('#makeAppointmentForm .info-wrapper .input-box input, #makeAppointmentForm .info-wrapper .input-box select').each(function () {
        if (!$(this).valid()) {
            isValid = false;
        }
    });

    if (isValid) {
        var member = {
            id: patientID,
            name: $('#apptFullName').val(),
            dateOfBirth: $('#apptDateOfBirth').val(),
            nationalID: $('#apptNationalID').val(),
            gender: $('#apptGender').val(),
            mobile: $('#apptMobile').val(),
            email: $('#apptEmail').val(),
            address: $('#apptAddress').val(),
            appointmentDate: $('#apptAppointmentDate').val(),
            appointmentTime: $('#apptTime').val(),
            modeOfConsultant: $('#apptModeOfConsultant').val(),
            consultantType: $('#apptConsultantType').val()
        }

        console.log(member);

        $.ajax({
            url: '/Doctor/Registration/MakeAppointment',
            method: 'POST',
            data: {
                id: $('#btnMakeAppt').data('id'),
                member: member,
            },
            dataType: 'JSON',
            success: function (res) {
                if (res.success) {
                    Swal.fire({
                        position: 'top',
                        title: 'Success!',
                        icon: 'success',
                        text: res.message,
                        showConfirmButton: false,
                        timer: 3000
                    }).then(function () {
                        $('#makeAppointmentForm').trigger('reset');
                        $('#makeAppointmentModal').modal('hide');
                    })
                } else {
                    Swal.fire({
                        position: 'top',
                        title: 'Failed!',
                        icon: 'warning',
                        text: res.message,
                        showConfirmButton: false,
                        timer: 3000
                    });
                }
            },
            error: function (err) {
                console.log(err.responseText);
            }
        });
    } else {
        Swal.fire({
            position: 'top',
            title: 'Failed!',
            icon: 'warning',
            text: 'Please fill out all information before submitting.',
            showConfirmButton: false,
            timer: 3000
        });
    }
});

$('#btnAddMember').on('click', function () {
    document.getElementById('addMemberForm').reset();
    $('#addMemberForm').find('input').removeClass('border-2 border-danger');
    $('#addMemberForm').find('label').remove();
    validateInput();
});

function validateInput() {
    jQuery.validator.addMethod('passwordFormat', function (value) {
        var regex = /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{6,50}$/;
        return value.trim().match(regex);
    });
    jQuery.validator.addMethod('phoneNumberFormat', function (value) {
        var regex = /^(84|0[3|5|7|8|9])+([0-9]{8})\b$/;
        return value.trim().match(regex);
    });

    jQuery.validator.addMethod('emailFormat', function (value) {
        var regex = /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}(?:\.[a-zA-Z]{2,})?$/;
        return value.trim().match(regex);
    });

    $('#addMemberForm').validate({
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
            "FullName": {
                required: true
            },
            "Email": {
                required: true,
                emailFormat: true
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
            "FullName": {
                required: "Full name is required."
            },
            "Email": {
                required: "Email is required.",
                emailFormat: "Please enter the correct email format."
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
            "Mobile": {
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
    });
}

$(document).ready(function () {
    $('#apptGender').css('pointer-events', 'none');
})