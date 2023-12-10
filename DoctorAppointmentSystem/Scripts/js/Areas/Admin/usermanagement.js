'use strict'
 //show on off form
function setButtonOnOffForm() {
    $("#reset-password-page").hide();
    $("#table-list-user").show();

    
    // close from reset password
    $("#btn-close-form-reset-password").on("click", function () {
        $("#form-reset-password-user").trigger('reset');
        $("#reset-password-page").hide();
        $("#table-list-user").show();
        $(".paswd-on-off").each(function () {
            var inp = this;
            inp.type = "password";
            
        });
        $('#form-reset-password-user').find('input').removeClass('border-2 border-danger');
        $('#form-reset-password-user').find('label').remove();
    });
  
    //open from reset password
    $(".btn-reset-password-user").on("click", function () { 
        $("#reset-password-page").show();
        $("#table-list-user").hide();
    });
}

//reset password
function setSubmitFormResetPasswordByAjax() {
    $("#form-reset-password-user").submit(function (event) {
        if ($("#form-reset-password-user").valid()) {
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
                confirmButtonText: 'Yes, reset password!',
                cancelButtonText: 'No, cancel!',
                reverseButtons: false
            }).then((result) => {
                if (result.isConfirmed) {

                    var formData = {
                        USERID: $("#userid").val(),
                        PASSWORD: $("#password").val(),
                    };
                    
                    $.ajax({
                        type: "POST",
                        url: "/Admin/UserManage/ResetPassword",
                        data: formData,
                        dataType: "json",
                        encode: true,
                    }).done(function (data) {
                        
                        if (data.error == 1) {
                            $("#form-reset-password-user").trigger('reset');
                            
                            Swal.fire(
                                'Failed!',
                                data.msg,
                                'error'
                            )
                        }
                        if (data.error == 0) {
                            $('#UserTable').DataTable().ajax.reload();

                            $("#reset-password-page").hide();
                            $("#table-list-user").show();
                            
                            $("#form-reset-password-user").trigger('reset');
                            Swal.fire({
                                position: 'top',
                                icon: 'success',
                                title: 'Success !',
                                text: 'Reset password for user is success !',
                                showConfirmButton: false,
                                timer: 3000
                            });
                            $('#password').get(0).type = 'password';
                            $('#passwordconfirm').get(0).type = 'password';
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

//set event show form reset password
function setEventResetPasswordForBtn() {
    var table = $('#UserTable').DataTable();

    table.on('draw', function () {

        $(".btn-reset-password-user").on("click", function () {
            $("#reset-password-page").show();
            $("#table-list-user").hide();

            var Button = $(this);
            var id = Button.data("id");
            var username = Button.data("username");
            $("#username-account-reset").text(username);
            $("#userid").val(id);
            
        });

       
    });

}

//delete dialog
function setEventDeleteUserForBtn() {
    var table = $('#UserTable').DataTable();

    table.on('draw', function () {

        $(".btn-delete-user").on("click", function () {

            var Button = $(this);
            var username = Button.data("username");
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
                title: 'Are you sure delete user ' + username + ' ?',
                text: "You won't be able to revert this!",
                icon: 'warning',
                showCancelButton: true,
                confirmButtonText: 'Yes, delete it!',
                cancelButtonText: 'No, cancel!',
                reverseButtons: false
            }).then((result) => {
                if (result.isConfirmed) {

                    deleteUserByAjax(id) // delete user by  id
                    

                } else if (
                    /* Read more about handling dismissals below */
                    result.dismiss === Swal.DismissReason.cancel
                ) {

                }
            })
        });
    });
    
    

}

//delete user
function deleteUserByAjax(doctorid) {

    var formData = {
        USERID: doctorid,
    };

    $.ajax({
        type: "POST",
        url: "/Admin/UserManage/DeleteUser",
        data: formData,
        dataType: "json",
        encode: true,
    }).done(function (data) {
        console.log(data);
        if (data.error == 1) {
            Swal.fire(
                'Failed!',
                data.msg,
                'error'
            )
        }
        if (data.error == 0) {
            $('#UserTable').DataTable().ajax.reload();
            Swal.fire({
                position: 'top',
                icon: 'success',
                title: 'Deleted !',
                text: 'Delete user is success !',
                showConfirmButton: false,
                timer: 3000
            });
           
        }
    });
}

//lock & unlock user dialog
function setEventLockUserForBtn() {
    var table = $('#UserTable').DataTable();

    table.on('draw', function () {
        //lock
        $(".btn-lock-user").on("click", function () {

            var Button = $(this);
            var username = Button.data("username");
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
                confirmButtonText: 'Yes, lock it!',
                cancelButtonText: 'No, cancel!',
                reverseButtons: false
            }).then((result) => {
                if (result.isConfirmed) {

                    // lock user by  id
                    lockOnOffUserByAjax(id, true) 

                } else if (
                    /* Read more about handling dismissals below */
                    result.dismiss === Swal.DismissReason.cancel
                ) {

                }
            })
        });
        //unlock
        $(".btn-unlock-user").on("click", function () {

            var Button = $(this);
            var username = Button.data("username");
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
                confirmButtonText: 'Yes, unlock it!',
                cancelButtonText: 'No, cancel!',
                reverseButtons: false
            }).then((result) => {
                if (result.isConfirmed) {

                    // lock user by  id
                    lockOnOffUserByAjax(id, false);

                } else if (
                    /* Read more about handling dismissals below */
                    result.dismiss === Swal.DismissReason.cancel
                ) {

                }
            })
        });
    });



}

//lock & unlock user
//lock : true or false
function lockOnOffUserByAjax(doctorid, lock) {

    var formData = {
        USERID: doctorid,
        LOCK: lock,
    };

    $.ajax({
        type: "POST",
        url: "/Admin/UserManage/LockUser",
        data: formData,
        dataType: "json",
        encode: true,
    }).done(function (data) {
        console.log(data);
        if (data.error == 1) {
            Swal.fire(
                'Failed!',
                data.msg,
                'error'
            )
        }
        if (data.error == 0) {
            $('#UserTable').DataTable().ajax.reload();
            //showMessage("Delete doctor is success ", "Success !")
            if (data.islock) {
                Swal.fire({
                    position: 'top',
                    icon: 'success',
                    title: 'Locked!',
                    text: 'Lock User is success !',
                    showConfirmButton: false,
                    timer: 3000
                });
               
            } else {
                Swal.fire({
                    position: 'top',
                    icon: 'success',
                    title: 'UnLocked!',
                    text: 'UnLock User is success !',
                    showConfirmButton: false,
                    timer: 3000
                });
                
            }
            
        }
    });
}

// Jquery datatable
function initJqueryDatatable() {
    var table = $('#UserTable').DataTable({
        
        "sAjaxSource": "/Admin/UserManage/LoadUserData",
        "sServerMethod": "POST",
        "bServerSide": true,
        "bProcessing": true,
        "responsive": true,
        "bSearchable": true,
        "order": [[1, 'asc']],
        "language": {
            "emptyTable": "There are no doctors in the list.",
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
                "data": "FULLNAME",
                "title": "Full Name",
                "searchable": true

            },
            {
                "data": "USERNAME",
                "title": "Username",

                "searchable": true

            },
            {
                "data": "USERTYPE",
                "title": 'User Type',
                "searchable": true

            },
           
            {
                "data": "GENDER",
                "title": "Gender",

                "searchable": true
            },
           
            {
                "data": "MOBILENO",
                "title": "Mobile",

                "searchable": true
            },
            {
                "data": "EMAIL",
                "title": "Email",

                "searchable": true
            },
            {
                "data": "LASTLOGIN",
                "title": "Last Login",

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
                "searchable": true,
                "orderable": false,
                "render": function (data, type, row) {
                    var lockbtn = "";
                    var html = "<btn class=\"btn-reset-password-user btn btn-sm btn-outline-primary btn-action ml-2\" data-id=\"" + row.USERID + "\" data-username=\"" + row.USERNAME + "\" data-toggle=\"popover\" data-trigger=\"hover\" data-placement=\"top\"  data-content=\"Reset password\" > <i class=\"fa-solid fa-key\"></i></btn>"
                        + "<btn class=\"btn-delete-user btn btn-sm btn-outline-danger btn-action ml-2 \" data-id=\"" + row.USERID + "\" data-username=\"" + row.USERNAME + "\" data-toggle=\"popover\" data-trigger=\"hover\" data-placement=\"top\"  data-content=\"Delete user\"> <i class=\"fa-solid fa-trash\" ></i> </btn> "
                    if (row.STATUS) lockbtn = "<btn class=\"btn-lock-user btn btn-sm btn-outline-success btn-action  \" data-id=\"" + row.USERID + "\" data-username=\"" + row.USERNAME + "\"  data-toggle=\"popover\" data-trigger=\"hover\" data-placement=\"top\"  data-content=\"Lock account\" > <i class=\"fa-solid fa-lock-open  \"></i> </btn>"
                    else lockbtn = "<btn class=\"btn-unlock-user btn btn-sm btn-outline-danger btn-action  \" data-id=\"" + row.USERID + "\" data-username=\"" + row.USERNAME + "\"  data-toggle=\"popover\" data-trigger=\"hover\" data-placement=\"top\"  data-content=\"Unlock account\" > <i class=\"fa-solid fa-lock  \"></i> </btn>"

                    return lockbtn + html;
                        
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

            if (inp.type == "password"){
                inp.type = "text";
            }
            else {
                inp.type = "password";
            }

         });
    }

//Validate form
function validateFormResetPassword() {
    

    jQuery.validator.addMethod('valid_password', function (value) {
        var regex = /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{6,50}$/;
        return value.trim().match(regex);
    });
    

    //Validate reset password
    $("#form-reset-password-user").validate({ 
        onfocusout: function (element) {
            $(element).valid()
        },
        rules: {
            "password": {
                required: true,
                maxlength: 30,
                minlength: 8,
                valid_password: true

            },
            "passwordconfirm": {
                required: true,
                equalTo: "#password",

            }
        },
        messages: {
            "password": {
                required: "Password is required",
                maxlength: "Maximum 30 characters",
                minlength: "Minimum 8 characters",
                valid_password: "Password charater at least one uppercase letter, one lowercase letter, one number and one special character"
            },
             "passwordconfirm": {
                required: "Password is required",
                 equalTo: "Password does not match !",
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
    setEventResetPasswordForBtn();
    setSubmitFormResetPasswordByAjax();
    validateFormResetPassword();
    setEventDeleteUserForBtn();
    setEventLockUserForBtn();
   
   
    $('[data-toggle="tooltip"]').tooltip()
    
});