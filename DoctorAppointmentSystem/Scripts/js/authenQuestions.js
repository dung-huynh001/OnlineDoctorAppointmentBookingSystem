

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

function setSubmitQuestions() {
    $("#authen-questions").submit(function (event) {

        if ($("#authen-questions").valid()) {
            // Get User's info
            var question = {
                username: $('#username').val(),
                passwordRecoveryQue1: $('#authQuestion1').val(),
                passwordRecoveryQue2: $('#authQuestion2').val(),
                passwordRecoveryQue3: $('#authQuestion3').val(),
                passwordRecoveryAns1: $('#answer1').val(),
                passwordRecoveryAns2: $('#answer2').val(),
                passwordRecoveryAns3: $('#answer3').val()
            }

            $.ajax({
                url: '/Account/AuthenQuestion',
                method: 'POST',
                data: {
                    question: question
                },
                success: function (res) {
                    if (res.success == true) {
                        Swal.fire({
                            position: 'top',
                            icon: 'success',
                            title: 'The answers have been recorded!',
                            text: res.message,
                            showConfirmButton: false,
                        })
                        setTimeout(function () {
                            window.location.href = res.url;
                            $('.register-form').trigger('reset');
                        }, 2000);
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
        }

        event.preventDefault();
    });
}




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
            }
        },
        messages: {
            
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
    setSubmitQuestions();
})
