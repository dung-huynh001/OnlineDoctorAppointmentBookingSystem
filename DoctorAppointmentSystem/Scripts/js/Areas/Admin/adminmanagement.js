
//show on off form
function setButtonOnOffForm() {
    $("#create-admin-page").hide();
    $("#update-admin-page").hide();
    $("#list-admin-page").show();

    //close form create
    $("#btn-close-form").on("click", function () {
        $("#create-admin-page").hide();
        $("#update-admin-page").hide();
        $("#list-admin-page").show();

        $('#form-create-admin').find('input').removeClass('border-2 border-danger');
        $('#form-create-admin').find('label').remove();

    });
    // close from update
    $("#btn-close-form-update").on("click", function () {
        $("#create-admin-page").hide();
        $("#update-admin-page").hide();
        $("#list-admin-page").show();

        $('#form-update-admin').find('input').removeClass('border-2 border-danger');
        $('#form-update-admin').find('label').remove();

    });

    //open from create
    $("#btn-open-form").on("click", function () {
        $("#create-admin-page").show();
        $("#update-admin-page").hide();
        $("#list-admin-page").hide();

    });
    //open from update
    $(".btn-update-patient").on("click", function () {
        $("#create-patient-page").hide();
        $("#create-patient-page").hide();
        $("#list-patient-page").show();


    });

}

//create admin
function setSubmitFormByAjax() {
    $("#form-create-admin").submit(function (event) {
        if ($("#form-create-admin").valid()) {
            var formData = {

                USERNAME: $("#username").val(),
                PASSWORD: $("#password").val(),
                EMAIL: $("#email").val(),
                
            };
           
            $.ajax({
                type: "POST",
                url: "/Admin/AdminUserManage/CreateAdmin",
                data: formData,
                dataType: "json",
                encode: true,
            }).done(function (data) {
                
                if (data.error == 1) {
                   
                    Swal.fire(
                        'Failed!',
                        data.msg,
                        'error'
                    )
                }
                if (data.error == 0) {

                    $('#AdminTable').DataTable().ajax.reload();
                    $("#create-admin-page").hide();
                    $("#list-admin-page").show();
                   
                    Swal.fire({
                        position: 'top',
                        icon: 'success',
                        title: 'Success !',
                        text: 'Create admin account is success !',
                        showConfirmButton: false,
                        timer: 3000
                    });

                    $("#form-create-admin").trigger('reset');

                }
            });
        }


        event.preventDefault();
    });
}

//show update from
function setEventUpdateAdminForBtn() {
    var table = $('#AdminTable').DataTable();

    table.on('draw', function () {

        $(".btn-update-admin").on("click", function () {
            $("#create-admin-page").hide();
            $("#list-admin-page").hide();
            $("#update-admin-page").show();

            var Button = $(this);
            var id = Button.data("id");
           
            loadDataToForm(id);

        });

    });

}

//load data to form update
function loadDataToForm(adminid) {

    var formData = {
        USERID: adminid,
    };

    $.ajax({
        type: "POST",
        url: "/Admin/AdminUserManage/LoadAdminInfo",
        data: formData,
        dataType: "json",
        encode: true,
    }).done(function (data) {
        
        if (data.error == 1) {
            
            Swal.fire(
                'Failed!',
                data.msg,
                'error'
            )
        }
        if (data.error == 0) {

           

            $("#uadminid").val(data.admin.USERID);
            $("#uusername").val(data.admin.USERNAME);
            $("#uemail").val(data.admin.EMAIL);
        }
    });
}

//Update admin
function setSubmitFormUdateByAjax() {
    $("#form-update-admin").submit(function (event) {
        if ($("#form-update-admin").valid()) {
            const swalWithBootstrapButtons = Swal.mixin({
                customClass: {
                    confirmButton: 'btn btn-success mr-2',
                    cancelButton: 'btn btn-danger'
                },
                buttonsStyling: false
            })
            swalWithBootstrapButtons.fire({
                position: 'top',
                title: 'Are you sure?',
                text: "You won't be able to revert this!",
                icon: 'warning',
                showCancelButton: true,
                confirmButtonText: 'Yes, update it!',
                cancelButtonText: 'No, cancel!',
                reverseButtons: false
            }).then((result) => {
                if (result.isConfirmed) {

                    var formData = {
                        USERID: $("#uadminid").val(),
                        EMAIL: $("#uemail").val()
                    };

                    $.ajax({
                        type: "POST",
                        url: "/Admin/AdminUserManage/UpdateAdmin",
                        data: formData,
                        dataType: "json",
                        encode: true,
                    }).done(function (data) {
                       
                        if (data.error == 1) {

                           
                            Swal.fire(
                                'Failed!',
                                data.msg,
                                'error'
                            )
                        }
                        if (data.error == 0) {
                            $('#AdminTable').DataTable().ajax.reload();

                            $("#create-admin-page").hide();
                            $("#update-admin-page").hide();
                            $("#list-admin-page").show();
                            
                            Swal.fire({
                                position: 'top',
                                icon: 'success',
                                title: 'Success !',
                                text: 'Update admin account is success!',
                                showConfirmButton: false,
                                timer: 3000
                            });

                            $("#form-update-admin").trigger('reset');
                        }
                    });

                } else if (
                    /* Read more about handling dismissals below */
                    result.dismiss === Swal.DismissReason.cancel
                ) {

                }
            })
        }

        event.preventDefault();
    });
}

//show delete dialog
function setEventDeletePatientFoBtn() {
    var table = $('#AdminTable').DataTable();

    table.on('draw', function () {

        $(".btn-delete-admin").on("click", function () {
            var Button = $(this);
            var id = Button.data("id");
           
            

            const swalWithBootstrapButtons = Swal.mixin({
                customClass: {
                    confirmButton: 'btn btn-success mr-2',
                    cancelButton: 'btn btn-danger'
                },
                buttonsStyling: false
            })
            swalWithBootstrapButtons.fire({
                position: 'top',
                title: 'Are you sure?',
                text: "You won't be able to revert this!",
                icon: 'warning',
                showCancelButton: true,
                confirmButtonText: 'Yes, delete it!',
                cancelButtonText: 'No, cancel!',
                reverseButtons: false
            }).then((result) => {
                if (result.isConfirmed) {

                    deleteAdminByAjax(id);// delete admin by id

                } else if (
                    /* Read more about handling dismissals below */
                    result.dismiss === Swal.DismissReason.cancel
                ) {

                }
            })
        });
    });

}

// delete admin
function deleteAdminByAjax(adminid) {

    var formData = {
        USERID: adminid,
    };

    $.ajax({
        type: "POST",
        url: "/Admin/AdminUserManage/DeleteAdmin",
        data: formData,
        dataType: "json",
        encode: true,
    }).done(function (data) {
       
        if (data.error == 1) {
            Swal.fire(
                'Failed!',
                data.msg,
                'error'
            )
        }
        if (data.error == 0) {
            $('#AdminTable').DataTable().ajax.reload();
           
            Swal.fire({
                position: 'top',
                icon: 'success',
                title: 'Deleted!',
                text: 'Delete admin account is success !',
                showConfirmButton: false,
                timer: 3000
            });
        }
    });
}

// Jquery datatable
function initJqueryDatatable() {
    var table = $('#AdminTable').DataTable({

        "sAjaxSource": "/Admin/AdminUserManage/LoadAdminData",
        "sServerMethod": "POST",
        "bServerSide": true,
        "bProcessing": true,
        "responsive": true,
        "bSearchable": true,
        "order": [[1, 'asc']],
        "language": {
            "emptyTable": "There are no admin account in the list.",
            "processing":
                '<i class="fa fa-spinner fa-spin fa-3x fa-fw" style="color:#2a2b2b;"></i><span class="sr-only">Loading...</span> '
        },
        "columns": [
            {
                "className": 'dt-control',
                "orderable": false,
                "data": null,
                "defaultContent": '',
            },
            {
                "data": "USERID",
                "title": 'ID',

                "searchable": true

            },
            {
                "data": "USERNAME",
                "title": "Username",

                "searchable": true

            }
            ,
            {
                "data": "EMAIL",
                "title": "Email",

                "searchable": true
            },
            {
                "data": "LOGINLOCKDATE",
                "title": "Login Lock Date",

                "searchable": true
            },
            {
                "data": "LOGINRETRYCOUNT",
                "title": "Login Retry Count",

                "searchable": true
            },
            {
                "data": "CREATEDBY",
                "title": "Created By",

                "searchable": true
            },
            {
                "data": "CREATEDDATE",
                "title": "Created Date",

                "searchable": true
            },
            {
                "data": "UPDATEDBY",
                "title": "Updated By",

                "searchable": true
            },
            {
                "data": "UPDATEDDATE",
                "title": "Updated Date",

                "searchable": true
            },
            {
                "data": null,
                "title": "Action",
                "responsivePriority": 1,
                "searchable": false,
                "orderable": false,
                "render": function (data, type, row) {
                    
                    return '<btn class=\"btn-update-admin btn btn-sm btn-outline-primary btn-action \" data-id=\"' + row.USERID + '\" data-username=\"' + row.USERNAME + '\"  data-toggle=\"popover\" data-trigger=\"hover\" data-placement=\"top\"  data-content=\"Edit user\" > <i class=\"fa-solid fa-user-pen\"></i> </btn>'
                        + '<btn class=\"btn-delete-admin btn btn-sm btn-outline-danger btn-action ml-2\" data-id=\"' + row.USERID + '\" data-username=\"' + row.USERNAME + '\" data-toggle=\"popover\" data-trigger=\"hover\" data-placement=\"top\"  data-content=\"Delete user\" > <i class=\"fa-solid fa-trash\"></i> </btn> '

                }

            },
        ]

    });

    $(window).trigger('resize');

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



//Validate form
function validateFormAdminUser() {
   
    jQuery.validator.addMethod('valid_password', function (value) {
        var regex = /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,30}$/;
        return value.trim().match(regex);
    });

    jQuery.validator.addMethod('valid_username', function (value) {
        var regex = /^[a-z0-9-]*$/;
        return value.trim().match(regex);
    });
    jQuery.validator.addMethod('valid_email', function (value) {
        var regex = /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}(?:\.[a-zA-Z]{2,})?$/;
        return value.trim().match(regex);
    });
    
    //Validate form create
    $("#form-create-admin").validate({
        onfocusout: function (element) {
            $(element).valid()
        },

        rules: {
            "username": {
                required: true,
                maxlength: 50,
                minlength: 3,
                valid_username: true
            },
            "password": {
                required: true,
                maxlength: 30,
                minlength: 8,
                valid_password: true,
            },
            
            "email": {
                required: true,
                email: true,
                valid_email: true
            }
            
        },
        messages: {
            "username": {
                required: "User name is required",
                maxlength: "Username charater lenght is 3 to 50!",
                minlength: "Username charater lenght is 3 to 50!",
                valid_username: "Username does not contain any special characters"
            },
            "password": {
                required: "Password is required",
                maxlength: "Maximum 30 characters",
                minlength: "Minimum 8 characters",
                valid_password: "Password charater at least one uppercase letter, one lowercase letter, one number and one special character",
                
            },
            
            "email": {
                required: "Email is required",
                email: "wrong email format",
                valid_email: "wrong email format"
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
        errorPlacement: function (error, element) {
            var elem = $(element);
            if (elem.hasClass("select2-hidden-accessible")) {
                element = $("#department").parent();

                error.insertAfter(element);
            } else {
                error.insertAfter(element);
            }
        }

    });

    //Validate form update
    $("#form-update-admin").validate({
        onfocusout: function (element) {
            $(element).valid()
        },
        rules: {
            "uemail": {
                required: true,
                email: true,
                valid_email: true
            }
        },
        messages: {
            "uemail": {
                required: "Email is required",
                email: "wrong email format",
                valid_email: "wrong email format"

            },
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
        errorPlacement: function (error, element) {
            var elem = $(element);
            if (elem.hasClass("select2-hidden-accessible")) {
                element = $("#udepartment").parent();

                error.insertAfter(element);
            } else {
                error.insertAfter(element);
            }
        }

    });
}

function isSpaceKey(evt) { // accept number 
    var charCode = (evt.which) ? evt.which : evt.keyCode;
    if (charCode == 32)
        return false;
    return true;
}

$("document").ready(function () {
    setButtonOnOffForm();
    initJqueryDatatable();
    setSubmitFormByAjax();
    setEventUpdateAdminForBtn();
    setSubmitFormUdateByAjax();
    validateFormAdminUser();
    setEventDeletePatientFoBtn();
    

});