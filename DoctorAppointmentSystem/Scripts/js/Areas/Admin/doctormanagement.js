'use strict'
 //show on off form
function setButtonOnOffForm() {
    $("#l-form-doctor").hide();
    $("#form-update-doctor").hide();
    $("#resetpassword-doctor").hide();
    $("#table-list-doctor").show();

    //close form create
    $("#btn-close-form").on("click", function () {
        $("#l-form-doctor").hide();
        $("#table-list-doctor").show();
        $("#form-update-doctor").hide();
        $("#resetpassword-doctor").hide();
        $("#form-create-doctor").trigger('reset');
        $(".paswd-on-off").each(function () {
            var inp = this;
            inp.type = "password";

        });

        $('#form-create-doctor').find('input').removeClass('border-2 border-danger');
        $('#form-create-doctor').find('textarea').removeClass('border-2 border-danger');
        $('#form-create-doctor').find('select').removeClass('border-2 border-danger');
        $('#form-create-doctor').find('span').removeClass('border-2 border-danger');
        $('#form-create-doctor').find('label').remove();

    });
    // close from update
    $("#btn-close-form-update").on("click", function () {
        $("#l-form-doctor").hide();
        $("#table-list-doctor").show();
        $("#form-update-doctor").hide();
        $("#resetpassword-doctor").hide();

        $('#form-edit-doctor').find('input').removeClass('border-2 border-danger');
        $('#form-edit-doctor').find('textarea').removeClass('border-2 border-danger');
        $('#form-edit-doctor').find('select').removeClass('border-2 border-danger');
        $('#form-edit-doctor').find('span').removeClass('border-2 border-danger');
        $('#form-edit-doctor').find('label').remove();
    });
    // close from reset password
    $("#btn-close-form-reset-password").on("click", function () {
        $("#l-form-doctor").hide();
        $("#table-list-doctor").show();
        $("#form-update-doctor").hide();
        $("#resetpassword-doctor").hide();
    });
    //open from create
    $("#btn-open-form").on("click", function () {
        $("#l-form-doctor").show();
        $("#table-list-doctor").hide();
        $("#form-update-doctor").hide();
        $("#resetpassword-doctor").hide();
    });
    //open from update
    $(".btn-update-doctor").on("click", function () {
        $("#l-form-doctor").hide();
        $("#table-list-doctor").hide();
        $("#form-update-doctor").show();
        $("#resetpassword-doctor").hide();

    });
    //open from reset password
    $(".btn-resetpassword-doctor").on("click", function () {
        $("#l-form-doctor").hide();
        $("#table-list-doctor").hide();
        $("#form-update-doctor").hide();
        $("#resetpassword-doctor").show();
        
    });
}

//create doctor
function setSubmitFormByAjax() {
    $("#form-create-doctor").submit(function (event) {

        if ($("#form-create-doctor").valid()) {

            var formData = {

                DOCTORNAME: $("#doctorname").val(),
                USERNAME: $("#username").val(),
                PASSWORD: $("#password").val(),
                DOCTORNATIONALID: $("#nationalid").val(),
                DOCTORGENDER: $("#gender").val(), 
                DOCTORADDRESS: $("#address").val(),
                DEPARTMENTID: $("#department").val(),
                DOCTORDATEOFBIRTH: $("#brithofdate").val(),
                DOCTORMOBILENO: $("#mobile").val(),
                EMAIL: $("#email").val(),
                SPECIALITY: $("#specialy").val(),
                WORKINGENDDATE: $("#workingenddate").val(),
                WORKINGSTARTDATE: $("#workingstartdate").val(),
            };
           
            $.ajax({
                type: "POST",
                url: "/Admin/DoctorManage/CreateDoctor",
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
                    
                    $('#DoctorTable').DataTable().ajax.reload();
                    $("#l-form-doctor").hide();
                    $("#table-list-doctor").show();

                    Swal.fire({
                        position: 'top',
                        icon: 'success',
                        title: 'Success !',
                        text: 'Create doctor is success !',
                        showConfirmButton: false,
                        timer: 3000
                    });
                    $("#form-create-doctor").trigger('reset');
                    $("#department").trigger('change');
                    $('#password').get(0).type = 'password';
                    
                    
                }
            });
        }
       

        event.preventDefault();
    });
}

//load data to form update
function loadDataToForm(doctorid) {

    var formData = {
        DoctorId: doctorid,
    };

    $.ajax({
        type: "POST",
        url: "/Admin/DoctorManage/LoadDoctorInfo",
        data: formData,
        dataType: "json",
        encode: true,
    }).done(function (data) {
        
        if (data.error == 1) {
            //showMessageFormUpdate(data.msg);
            Swal.fire(
                'Failed!',
                data.msg,
                'error'
            )
        }
        if (data.error == 0) {

            $("#udoctorid").val(data.doctor.DOCTORID);
            $("#udoctorname").val(data.doctor.DOCTORNAME);
            $("#uusername").val(data.doctor.USERNAME);
            $("#upassword").val(data.doctor.PASSWORD);
            $("#unationalid").val(data.doctor.DOCTORNATIONALID);
            $("#uaddress").val(data.doctor.DOCTORADDRESS);
            $("#udepartment").val(String(data.doctor.DEPARTMENTID));
            $("#udepartment").trigger('change');
            $("#ubrithofdate").val(data.doctor.DOCTORDATEOFBIRTH);
            $("#umobile").val(data.doctor.DOCTORMOBILENO);
            $("#uworkingenddate").val(data.doctor.WORKINGENDDATE);
            $("#uworkingstartdate").val(data.doctor.WORKINGSTARTDATE);
            $("#ugender").val([data.doctor.DOCTORGENDER]);
            $("#uemail").val(data.doctor.EMAIL);
            $("#uspecialy").val(data.doctor.SPECIALITY);

        }
    });


}

//Update doctor
function setSubmitFormUdateByAjax() {
    $("#form-edit-doctor").submit(function (event) {
        if ($("#form-edit-doctor").valid()) {
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
                        DOCTORID: $("#udoctorid").val(),
                        DOCTORNAME: $("#udoctorname").val(),
                        DOCTORNATIONALID: $("#unationalid").val(),
                        DOCTORGENDER: $("#ugender").val(),
                        DOCTORADDRESS: $("#uaddress").val(),
                        DEPARTMENTID: $("#udepartment").val(),
                        DOCTORDATEOFBIRTH: $("#ubrithofdate").val(),
                        DOCTORMOBILENO: $("#umobile").val(),
                        EMAIL: $("#uemail").val(),
                        SPECIALITY: $("#uspecialy").val(),
                        WORKINGENDDATE: $("#uworkingenddate").val(),
                        WORKINGSTARTDATE: $("#uworkingstartdate").val(),

                    };
                    console.log($("#udoctorname").val());
                    $.ajax({
                        type: "POST",
                        url: "/Admin/DoctorManage/UpdateDoctor",
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
                            $('#DoctorTable').DataTable().ajax.reload();

                            $("#l-form-doctor").hide();
                            $("#table-list-doctor").show();
                            $("#form-update-doctor").hide();
                           
                            Swal.fire({
                                position: 'top',
                                icon: 'success',
                                title: 'Success !',
                                text: 'Update doctor is success !',
                                showConfirmButton: false,
                                timer: 3000
                            });
                            $("#form-edit-doctor").trigger('reset');

                        }
                    });
                } else if (result.dismiss === Swal.DismissReason.cancel ) {/* Read more about handling dismissals below */}
            })
        }

        event.preventDefault();
    });
}

//delete dialog
function setEventDeleteDoctorFoBtn() {
    var table = $('#DoctorTable').DataTable();

    table.on('draw', function () {

        $(".btn-delete-doctor").on("click", function () {
            var Button = $(this);
            var doctorname = Button.data("doctorname");
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

                    deleteDocTorByAjax(id) // delete doctor by  id
                    

                } else if (
                    /* Read more about handling dismissals below */
                    result.dismiss === Swal.DismissReason.cancel
                ) {

                }
            })
        });
    });
    
    $("#btn-accept-delete-doctor").on("click", function () {
        var Button = $(this);
        var id = Button.attr("data-id");
       
        DeleteDocTorByAjax(id);// delete doctor by id
    });

}

//delete doctor
function deleteDocTorByAjax(doctorid) {

    var formData = {
        DoctorId: doctorid,
    };

    $.ajax({
        type: "POST",
        url: "/Admin/DoctorManage/DeleteDoctor",
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
            $('#DoctorTable').DataTable().ajax.reload();
            
            Swal.fire({
                position: 'top',
                icon: 'success',
                title: 'Deleted !',
                text: 'Delete doctor is success !',
                showConfirmButton: false,
                timer: 3000
            });
        }
    });
}

//show update from
function setEventUpdateDoctorFoBtn() {
    var table = $('#DoctorTable').DataTable();

    table.on('draw', function () {

        $(".btn-update-doctor").on("click", function () {
            $("#l-form-doctor").hide();
            $("#table-list-doctor").hide();
            $("#form-update-doctor").show();

            var Button = $(this);
            var id = Button.data("id");
            
            loadDataToForm(id);

        });

    });

}


// Jquery datatable
function initJqueryDatatable() {
    var table = $('#DoctorTable').DataTable({
        
        "sAjaxSource": "/Admin/DoctorManage/LoadDoctorData",
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
                "data": "DOCTORID",
                "title": 'ID',

                "searchable": true

            },
            {
                "data": "DOCTORNAME",
                "title": "Name",

                "searchable": true

            },
            {
                "data": "USERNAME",
                "title": "Username",

                "searchable": true

            },
            {
                "data": "DEPARTMENT",
                "title": "Department",

                "searchable": true
            },
            {
                "data": "DOCTORGENDER",
                "title": "Gender",

                "searchable": true
            },
           
            {
                "data": "DOCTORDATEOFBIRTH",
                "title": "Date Of Birth",

                "searchable": true
            },
            {
                "data": "DOCTORNATIONALID",
                "title": "National ID",

                "searchable": true
            },
            {
                "data": "DOCTORMOBILENO",
                "title": "Mobile",

                "searchable": true
            },
            {
                "data": "EMAIL",
                "title": "Email",

                "searchable": true
            },
            {
                "data": "DOCTORADDRESS",
                "title": "Address",

                "searchable": true
            },
            {
                "data": "SPECIALITY",
                "title": "Speciality",

                "searchable": true
            },
            {
                "data": "WORKINGSTARTDATE",
                "title": "Working Start Date",

                "searchable": true
            },
            {
                "data": "WORKINGENDDATE",
                "title": "Working End Date",

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
                    
                    return "<btn class=\"btn-update-doctor btn btn-sm btn-outline-primary btn-action\" data-id=\"" + row.DOCTORID + "\" data-doctorname=\"" + row.DOCTORNAME + "\" data-toggle=\"popover\" data-trigger=\"hover\" data-placement=\"top\"  data-content=\"Edit user\"> <i class=\"fa-solid fa-user-pen\"></i></btn>"
                        + "<btn class=\"btn-delete-doctor btn btn-sm btn-outline-danger btn-action ml-2\" data-id=\"" + row.DOCTORID + "\" data-doctorname=\"" + row.DOCTORNAME + "\" data-toggle=\"popover\" data-trigger=\"hover\" data-placement=\"top\"  data-content=\"Delete user\" ><i class=\"fa-solid fa-trash\"></i></btn> "
                        
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
function validateFormDoctor() {
    jQuery.validator.addMethod('valid_phone', function (value) {
        var regex = /^(84|0[3|5|7|8|9])+([0-9]{8})\b$/;
        return value.trim().match(regex);
    });
    jQuery.validator.addMethod('valid_username', function (value) {
        var regex = /^[a-zA-Z0-9-]*$/;
        return value.trim().match(regex);
    });
  
    jQuery.validator.addMethod('valid_password', function (value) {
        var regex = /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,30}$/;
        return value.trim().match(regex);
    });
  
    jQuery.validator.addMethod('valid_email', function (value) {
        var regex = /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}(?:\.[a-zA-Z]{2,})?$/;
        return value.trim().match(regex);
    });
    jQuery.validator.addMethod("greaterThan",
        function (value, element, params) {

            if (!/Invalid|NaN/.test(new Date(value))) {
                return new Date(value) > new Date($(params).val());
            }

            return isNaN(value) && isNaN($(params).val())
                || (Number(value) > Number($(params).val()));
        }, 'Must be greater than {0}.');


    //Validate form create
    $("#form-create-doctor").validate({ 
        onfocusout: function (element) {
            $(element).valid()
        },
        ignore: ".ignore, .select2-input", 
        rules: {
            "doctorname": {
                required: true,
                maxlength: 50,
                
            },
            "username": {
                required: true,
                maxlength: 50,
                minlength: 3,
                valid_username: true,
            },
            "password": {
                required: true,
                maxlength: 30,
                minlength: 8,
                valid_password: true

            },
            "nationalid": {
                required: true,
                maxlength: 20
                
            },
            "brithofdate": {
                required: true,
                

            },
            "mobile": {
                required: true,
                valid_phone:true
               
            },
            "address": {
                required: true,
                maxlength: 256,
               
            }
            ,
            "specialy": {
                required: true,
                maxlength: 256,
            }
            ,
            "email": {
                required: true,
                email: true,
                valid_email: true
            }
            ,
            "Gender": {
                required: true
               
            },
            "Department": {
                required: true

            }
            ,
            "workingstartdate": {
                required: true

            }
            ,
            "workingenddate": {
                required: true,
                greaterThan: "#workingstartdate"
            }
        },
        messages: {
            "doctorname": {
                required: "Doctor name is required",
                maxlength: "Doctor name charater max lenght is 50!",
               
                
            },
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
                valid_password: "Password charater at least one uppercase letter, one lowercase letter, one number and one special character"
                
            },
            "nationalid": {
                required: "National ID is required",
                maxlength: "National ID charater max lenght is 20!"
            },
            "brithofdate": {
                required: "Date of birth is required",
                
            },
            "mobile": {
                required: "Mobile is required",
                valid_phone : "wrong phone number format"
                
            },
            "address": {
                required: "Address is required",
                maxlength: "Doctor address charater max lenght is 256 !"
            }
            ,
            "specialy": {
                required: "Specialty is required",
                maxlength: "Specialty charater max lenght is 256 !"
            }
            ,
            "email": {
                required: "Email is required",
                email: "wrong email format",
                valid_email: "wrong email format"
            },
            "Gender": {
                required: "Gender is required"

            },
            "Department": {
                required: "Department is required"

            }
            ,
            "workingstartdate": {
                required: "Working start date is required"

            }
            ,
            "workingenddate": {
                required: "Working end date is required",
                greaterThan:"Working start date smaller than Working end date"
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
    $("#form-edit-doctor").validate({
        onfocusout: function (element) {
            $(element).valid()
        },

        rules: {
            "udoctorname": {
                required: true,
                maxlength: 50
            },
            "unationalid": {
                required: true,
                maxlength: 20

            },
            "ubrithofdate": {
                required: true,


            },
            "umobile": {
                required: true,
                valid_phone: true

            },
            "uaddress": {
                required: true,
                maxlength: 256,

            }
            ,
            "uspecialy": {
                required: true,
                maxlength: 256,
            }
            ,
            "uemail": {
                required: true,
                email: true
            },
            "uGender": {
                required: true

            },
            "uDepartment": {
                required: true

            }
            ,
            "uworkingstartdate": {
                required: true

            }
            ,
            "uworkingenddate": {
                required: true,
                greaterThan: "#uworkingstartdate"
            }
        },
        messages: {
            "udoctorname": {
                required: "Doctor name is required",
                maxlength: "Doctor name charater max lenght is 50!",

            },
            
            "unationalid": {
                required: "National ID is required",
                maxlength: "National ID charater max lenght is 20!"
            },
            "ubrithofdate": {
                required: "Date of birth is required",

            },
            "umobile": {
                required: "Mobile is required",
                valid_phone: "wrong phone number format"

            },
            "uaddress": {
                required: "Address is required",
                maxlength: "Doctor address charater max lenght is 256 !"
            }
            ,
            "uspecialy": {
                required: "Specialty is required",
                maxlength: "Specialty charater max lenght is 256 !"
            }
            ,
            "uemail": {
                required: "Email is required",
                email: "wrong email format"
            },
            "Gender": {
                required: "Gender is required"

            },
            "Department": {
                required: "Department is required"

            }
            ,
            "workingstartdate": {
                required: "Working start date is required"

            }
            ,
            "workingenddate": {
                required: "Working end date is required",
                greaterThan: "Working start date smaller than Working end date"
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
                element = $("#udepartment").parent();

                error.insertAfter(element);
            } else {
                error.insertAfter(element);
            }
        }

    });
}

function isNumberKey(evt) { // accept number 
    var charCode = (evt.which) ? evt.which : evt.keyCode;
    if (charCode != 46 && charCode > 31
        && (charCode < 48 || charCode > 57))
        return false;
    return true;
}    
function isSpaceKey(evt) { //  
    var charCode = (evt.which) ? evt.which : evt.keyCode;
    if (charCode == 32 )
        return false;
    return true;
}


$("document").ready(function () {
    setButtonOnOffForm();
    setSubmitFormByAjax();
    setSubmitFormUdateByAjax();
    initJqueryDatatable();
    setEventUpdateDoctorFoBtn();
    setEventDeleteDoctorFoBtn();
    validateFormDoctor();
    

    $("#department").select2();
    
    $("#department").on('select2:close', function (e) {
        $(this).valid()
    });

    $("#udepartment").select2();
    $("#udepartment").on('select2:close', function (e) {
        $(this).valid()
    });
});